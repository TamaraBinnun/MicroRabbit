using AutoMapper;
using MicroRabbit.Books.Application.Dtos.Books;
using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Domain.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksService _bookService;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger,
            IBooksService bookService,
            IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
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

            if (bookResponse != null)
            {
                //send event EventToUpdateBook to rabbitmq for updating book data in order microservice
                var bookData = _mapper.Map<CommonBookData>(bookResponse);
                await _bookService.CreateEventToUpdateBookAsync(bookData);
            }

            return CreatedAtRoute(nameof(GetBookAsync), new { Id = bookResponse?.Id }, bookResponse);
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

            //send event EventToUpdateBook to rabbitmq for updating book data in order microservice
            var bookData = _mapper.Map<CommonBookData>(updateBookRequest);
            await _bookService.CreateEventToUpdateBookAsync(bookData);

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

            //send event EventToUpdateBook to rabbitmq for updating book data in order microservice
            var bookData = new CommonBookData
            {
                BookId = id,
                IsDeleted = true
            };
            await _bookService.CreateEventToUpdateBookAsync(bookData);

            return Ok();
        }
    }
}