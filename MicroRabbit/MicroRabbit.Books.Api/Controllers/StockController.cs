using Azure;
using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private readonly IStockService _stockService;

        public StockController(ILogger<StockController> logger,
                                       IStockService stockService)
        {
            _logger = logger;
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookUnits>>> GetBookUnitsInStockAsync(List<int> bookIds)
        {
            if (bookIds == null)
            {
                return BadRequest();
            }

            var response = await _stockService.GetBookUnitsInStockAsync(bookIds);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookUnitsInStockAsync([FromBody] List<BookUnits> updateInStockRequest)
        {
            if (updateInStockRequest == null)
            {
                return BadRequest();
            }

            var response = await _stockService.UpdateBookUnitsInStockAsync(updateInStockRequest);

            if (response <= 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}