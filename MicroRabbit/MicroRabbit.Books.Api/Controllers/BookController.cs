using MicroRabbit.Books.Application.Dtos;
using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Domain.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MicroRabbit.Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookService _bookService;

        public BookController(ILogger<BookController> logger,
            IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksAsync()
        {
            var response = await _bookService.GetAllAsync();
            return Ok(response);
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

        [HttpPost]
        public async Task<ActionResult<BookResponse>> PostAsync([FromBody] AddBookRequest addBookRequest)
        {
            if (addBookRequest == null)
            {
                return BadRequest();
            }

            var bookResponse = await _bookService.AddAsync(addBookRequest);

            return CreatedAtRoute(nameof(GetBookAsync), new { Id = bookResponse.Id }, bookResponse);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateBookRequest updateBookRequest)
        {
            if (updateBookRequest == null)
            {
                return BadRequest();
            }

            var response = await _bookService.UpdateAsync(updateBookRequest);

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

            var response = await _bookService.DeleteAsync(id);

            if (response == -1)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}