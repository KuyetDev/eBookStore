using DataAccess.DTOs.Authors;
using DataAccess.DTOs.Publishers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories;

namespace eBookStoreWebAPI.Controllers
{
    [Route("api/publishers")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }
        [HttpGet]
        [EnableQuery]
        public IActionResult GetAllPublisher()
        {
            var authors = _publisherRepository.GetAllPublisher();
            return Ok(authors);
        }

        [HttpDelete("delete-publisher/{id:int}")]
        public IActionResult DeleteAuthor(int id)
        {
            var result = _publisherRepository.DeletePublisher(id);
            return Ok(result);
        }

        [HttpPost("add-publisher")]
        public IActionResult Create([FromBody] CreatePublisherRequest addPublisher)
        {
            var result = _publisherRepository.AddPublisher(addPublisher);

            return Ok(result);
        }

        [HttpPut("update-publisher/{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdatePublisherRequest updatePublisher)
        {
            var result = _publisherRepository.UpdatePublisher(updatePublisher, id);

            return Ok(result);
        }
    }
}
