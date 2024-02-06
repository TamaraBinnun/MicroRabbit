using MicroRabbit.Domain.Core.Dtos;
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
        private readonly IBooksService _bookService;

        public BooksController(ILogger<BooksController> logger,
            IBooksService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookResponse>> GetAll()
        {
            return Ok(_bookService.GetAll());
        }

        [HttpGet("{id}", Name = "GetBookAsync")]
        public async Task<ActionResult<BookResponse>> GetByIdAsync(int id)
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

        /*Used in books microservice for synchronous communication between microservices
         *after creating a new book, when rabbitmq communication is down, it uses directly this service
         *by sending -book ids that have been ordered- and -their titles-, for updating books table in order microservice*/

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookAsync(int id, [FromBody] CommonBookData bookData)
        {
            if (bookData == null)
            {
                return BadRequest();
            }

            await _bookService.UseEventToUpdateBookAsync(bookData);

            return Ok();
        }
    }
}