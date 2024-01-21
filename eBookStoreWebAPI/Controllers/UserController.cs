using DataAccess.DTOs.Authors;
using DataAccess.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories;

namespace eBookStoreWebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetAllUser()
        {
            var users = _userRepository.GetAllUser();
            return Ok(users);
        }

        [HttpDelete("delete-user/{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userRepository.DeleteUser(id);
            return Ok(result);
        }

        [HttpPost("add-user")]
        public IActionResult Create([FromBody] CreateUserRequest addUser)
        {
            var result = _userRepository.AddUser(addUser);

            return Ok(result);
        }

        [HttpPut("update-user/{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateUserRequest updateUser)
        {
            var result = _userRepository.UpdateUser(updateUser, id);

            return Ok(result);
        }
    }
}
