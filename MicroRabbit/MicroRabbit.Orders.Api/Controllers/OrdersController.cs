using MicroRabbit.Orders.Application.Dtos.OrderBooks;
using MicroRabbit.Orders.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Orders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderBooksService _orderBooksService;

        public OrdersController(ILogger<OrdersController> logger,
                                IOrderBooksService orderBooksService)
        {
            _logger = logger;
            _orderBooksService = orderBooksService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderBookResponse>>> GetOrdersAsync()
        {
            return Ok(await _orderBooksService.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetByOrderIdAsync")]
        public async Task<ActionResult<OrderBookResponse>> GetByOrderIdAsync(int orderId)
        {
            if (orderId < 1)
            {
                return BadRequest();
            }

            var response = await _orderBooksService.GetByOrderIdAsync(orderId);
            if (response?.Order == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<OrderBookResponse>> PostAsync([FromBody] AddOrderBookRequest addOrderRequest)
        {
            if (addOrderRequest == null)
            {
                return BadRequest();
            }

            var orderBookResponse = await _orderBooksService.AddAsync(addOrderRequest);

            return CreatedAtRoute(nameof(GetByOrderIdAsync), new { Id = orderBookResponse.Order?.Id }, orderBookResponse);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateOrderBookRequest updateOrderRequest)
        {
            if (updateOrderRequest == null)
            {
                return BadRequest();
            }

            var response = await _orderBooksService.UpdateAsync(updateOrderRequest);

            if (response <= 0)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int orderId)
        {
            if (orderId < 1)
            {
                return BadRequest();
            }

            var response = await _orderBooksService.DeleteAsync(orderId);

            if (response == -1)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}