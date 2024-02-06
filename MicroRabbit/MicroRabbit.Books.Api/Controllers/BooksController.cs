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
        public ActionResult<IEnumerable<BookResponse>> GetAll()
        {
            var response = _bookService.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetByIdAsync")]
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

            return CreatedAtRoute(nameof(GetByIdAsync), new { Id = bookResponse?.Id }, bookResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateBookRequest updateBookRequest)
        {
            if (updateBookRequest == null)
            {
                return BadRequest();
            }

            var response = await _bookService.UpdateAsync(id, updateBookRequest);

            if (response == null)
            {
                return NotFound();
            }

            //send event EventToUpdateBook to rabbitmq for inserting or updating book data in order microservice
            var bookData = _mapper.Map<CommonBookData>(response);
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

            var bookResponse = await _bookService.DeleteAsync(id);

            if (bookResponse == null)
            {
                return NotFound();
            }

            //send event EventToUpdateBook to rabbitmq for updating book data in order microservice
            var bookData = _mapper.Map<CommonBookData>((bookResponse, true));//IsDeleted = true

            await _bookService.CreateEventToUpdateBookAsync(bookData);

            return Ok();
        }
    }
}