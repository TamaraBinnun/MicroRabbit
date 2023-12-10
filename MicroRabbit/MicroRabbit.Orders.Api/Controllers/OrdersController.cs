using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Orders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;

        public OrdersController(ILogger<OrdersController> logger,
                                IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersAsync()
        {
            return Ok(await _orderService.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetOrderAsync")]
        public async Task<ActionResult<OrderResponse>> GetOrderAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var response = await _orderService.GetByIdAsync(id);
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

            var orderResponse = await _orderService.AddAsync(addOrderRequest);

            await _orderService.UpdateBookUnitsInStockAsync(addOrderRequest.OrderItems);

            return CreatedAtRoute(nameof(GetOrderAsync), new { Id = orderResponse.Id }, orderResponse);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateOrderRequest updateOrderRequest)
        {
            if (updateOrderRequest == null)
            {
                return BadRequest();
            }

            var response = await _orderService.UpdateAsync(updateOrderRequest);

            if (response <= 0)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var response = await _orderService.DeleteAsync(id);

            if (response == -1)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet]
        [Route("~/api/BookUnitsInStock/")]
        public async Task<ActionResult<IEnumerable<BookUnits>>> GetBookUnitsInStockAsync([FromQuery] int[] bookIds)
        {
            if (bookIds == null)
            {
                return BadRequest();
            }

            var response = await _orderService.GetBookUnitsInStockAsync(bookIds.ToList());

            return Ok(response);
        }
    }
}