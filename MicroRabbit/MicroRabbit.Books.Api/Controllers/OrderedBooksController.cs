using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Domain.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderedBooksController : ControllerBase
    {
        private readonly ILogger<OrderedBooksController> _logger;
        private readonly IOrderedBooksService _orderedBooksService;

        public OrderedBooksController(ILogger<OrderedBooksController> logger,
                                       IOrderedBooksService orderedBooksService)
        {
            _logger = logger;
            _orderedBooksService = orderedBooksService;
        }

        /*Used in orders microservice for synchronous communication between microservices
         *after creating a new order, when rabbitmq communication is down, it uses directly this service
         *by sending -book ids that have been ordered- and -how many units ordered- */

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderedBooksAsync(int id, [FromBody] CommonOrder commonOrder)
        {
            var response = await _orderedBooksService.UseEventToUpdateOrderedBooksAsync(commonOrder);

            if (response == null)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}