using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Orders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;

        public BooksController(ILogger<BooksController> logger,
            IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksAsync()
        {
            return Ok(await _bookService.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetBookAsync")]
        public async Task<ActionResult<BookResponse>> GetBookAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var response = await _bookService.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}