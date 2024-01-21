using DataAccess.DTOs.Authors;
using DataAccess.DTOs.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories;

namespace eBookStoreWebAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet]
        [EnableQuery]
        public IActionResult GetAllBooks()
        {
            var books = _bookRepository.GetAllBook();
            return Ok(books);
        }

        [HttpDelete("delete-book/{id:int}")]
        public IActionResult DeleteBook(int id)
        {
            var result = _bookRepository.DeleteBook(id);
            return Ok(result);
        }

        [HttpPost("add-book")]
        public IActionResult Create([FromBody] CreateBookRequest addBook)
        {
            var result = _bookRepository.AddBook(addBook);

            return Ok(result);
        }

        [HttpPut("update-book/{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateBookRequest updateBook)
        {
            var result = _bookRepository.UpdateBook(updateBook, id);

            return Ok(result);
        }
    }
}
