using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using DataAccess;
using DataAccess.DTOs.Authors;
using Microsoft.AspNetCore.OData.Query;

namespace eBookStoreWebAPI.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetAllAuthor()
        {
            var authors = _authorRepository.GetAuthors();
            return Ok(authors);
        }

        [HttpDelete("delete-author/{id:int}")]
        public IActionResult DeleteAuthor(int id)
        {
            var result = _authorRepository.DeleteAuthor(id);
            return Ok(result);
        }

        [HttpPost("add-author")]
        public IActionResult Create([FromBody] CreateAuthorRequest addAuthor)
        {
            var result = _authorRepository.AddAuthor(addAuthor);

            return Ok(result);
        }

        [HttpPut("update-author/{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateAuthorRequest updateAuthor)
        {
            var result = _authorRepository.UpdateAuthor(updateAuthor, id);

            return Ok(result);
        }
    }
}
