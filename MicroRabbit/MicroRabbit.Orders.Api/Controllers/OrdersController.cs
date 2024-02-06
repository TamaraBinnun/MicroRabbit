using AutoMapper;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Orders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrdersService _ordersService;
        private readonly IBooksService _bookService;
        private readonly IMapper _mapper;

        public OrdersController(ILogger<OrdersController> logger,
                                IOrdersService ordersService,
                                IBooksService bookService,
                                IMapper mapper)
        {
            _logger = logger;
            _ordersService = ordersService;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderResponse>?> GetAll()
        {
            return Ok(_ordersService.GetAll
             (filter: x => x.OrderStatus == (int)OrderStatus.Created || x.OrderStatus == (int)OrderStatus.Updated,
              orderBy: x => x.OrderByDescending(x => x.CreatedDate),
            includeProperties: new string[] { "OrderItems" }));
        }

        [HttpGet("{id}", Name = "GetByIdAsync")]
        public async Task<ActionResult<OrderResponse>> GetByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var response = await _ordersService.GetByIdAsync(id, includeProperties: new string[] { "OrderItems" });
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> PostAsync([FromBody] AddOrderRequest addOrderRequest)
        {
            if (addOrderRequest == null)
            {
                return BadRequest();
            }

            var addedOrder = await _ordersService.AddAsync(addOrderRequest);//add order and its items

            //create event
            await CreateEvent(addedOrder);

            return CreatedAtRoute(nameof(GetByIdAsync), new { Id = addedOrder.Id }, addedOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderResponse>> PutAsync(int id, [FromBody] UpdateOrderRequest updateOrderRequest)
        {
            var isValid = CheckValidation(updateOrderRequest);
            if (!isValid) { return BadRequest(); }

            ////***************update order and it's items
            var updatedOrder = await _ordersService.UpdateOrder(id, updateOrderRequest);
            if (updatedOrder == null)
            {
                return NotFound();
            }

            //***************create event
            await CreateEvent(updatedOrder);

            return Ok(updatedOrder);
        }

        private bool CheckValidation(UpdateOrderRequest updateOrderRequest)
        {
            if (updateOrderRequest == null)
            {
                return false;
            }

            //validation: check that all same BookId have same UnitPrice
            var groupByBookId = updateOrderRequest.OrderItems?.Select(x => x.BookId).Distinct().ToList().Count;
            var groupByBookIdUnitPrice = updateOrderRequest.OrderItems?.Select(x => new { x.BookId, x.UnitPrice }).Distinct().ToList().Count;

            var isValid = groupByBookId == groupByBookIdUnitPrice;
            if (!isValid)
            {//different prices for same books
                return false;
            }
            return true;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderResponse>> DeleteAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            //var deletedOrderItems = await _orderItemsService.DeleteManyByFilterAsync(filter: x => x.OrderId == id);
            var deletedOrder = await _ordersService.DeleteAsync(id);

            if (deletedOrder == null)
            {
                return NotFound();
            }

            //not passing deleteted items for create event because another another system should delete it's items
            //passing only added and updated items
            await CreateEvent(deletedOrder);

            return Ok(deletedOrder);
        }

        private async Task CreateEvent(OrderResponse? order)
        {
            if (order == null) { return; }

            var orderCreateEvent = _mapper.Map<CommonOrder>(order);
            orderCreateEvent.OrderItems = await GetOrderItemsCreateEvent(orderCreateEvent.OrderItems);
            await _ordersService.CreateEventToUpdateOrderedBooksAsync(orderCreateEvent);
        }

        private async Task<IEnumerable<CommonOrderedBook>?> GetOrderItemsCreateEvent(IEnumerable<CommonOrderedBook>? orderItemsCreateEvent)
        {
            if (orderItemsCreateEvent == null) { return null; }

            var bookIds = orderItemsCreateEvent.Select(x => x.BookId);
            var bookData = await _bookService.GetByIdsAsync(bookIds);
            if (bookData == null) { return orderItemsCreateEvent; }

            var response = (from orderItemCreateEvent in orderItemsCreateEvent
                            join book in bookData
                            on orderItemCreateEvent.BookId equals book.Id
                            select _mapper.Map<CommonOrderedBook>((orderItemCreateEvent, book)));

            return response;
        }
    }
}