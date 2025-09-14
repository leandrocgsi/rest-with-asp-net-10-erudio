using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Services;

namespace RestWithASPNET10Erudio.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
    public class BookController : ControllerBase
    {
        private IBookServices _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookServices bookService,
            ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get()
        {
            _logger.LogInformation("Fetching all books");
            return Ok(_bookService.FindAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get(long id)
        {
            _logger.LogInformation("Fetching book with ID {id}", id);
            var book = _bookService.FindById(id);
            if (book == null)
            {
                _logger.LogWarning("Book with ID {id} not found", id);
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Post([FromBody] BookDTO book)
        {
            _logger.LogInformation("Creating new Book: {firstName}", book.Title);

            var createdBook = _bookService.Create(book);
            if (createdBook == null)
            {
                _logger.LogError("Failed to create book with name {firstName}", book.Title);
                return NotFound();
            }
            Response.Headers.Add("X-API-Deprecated", "true");
            Response.Headers.Add("X-API-Deprecation-Date", "2026-12-31");
            return Ok(createdBook);
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Put([FromBody] BookDTO book)
        {
            _logger.LogInformation("Updating book with ID {id}", book.Id);

            var createdBook = _bookService.Update(book);
            if (createdBook == null)
            {
                _logger.LogError("Failed to update book with ID {id}", book.Id);
                return NotFound();
            }
            _logger.LogDebug("Book updated successfully: {firstName}", createdBook.Title);
            return Ok(createdBook);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Deleting book with ID {id}", id);
            _bookService.Delete(id);
            _logger.LogDebug("Book with ID {id} deleted successfully", id);
            return NoContent();
        }
    }
}
