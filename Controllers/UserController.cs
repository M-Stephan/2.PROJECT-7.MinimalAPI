using Microsoft.AspNetCore.Mvc;
using Solution.Services;
using Solution.Users;
using Solution.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            return users.Any() ? Ok(users) : NotFound("No Users Found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUser(id);
            return user is not null ? Ok(user) : NotFound("User ID Not Found");
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _userService.UpdateUser(id, user);
            return updated is not null ? Ok(updated) : NotFound("User ID Not Found");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool deleted = await _userService.DeleteUser(id);
            return deleted ? Ok($"User {id} deleted") : NotFound("User ID Not Found");
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.Authenticate(loginRequest.Email, loginRequest.Password);
            if (user == null)
                return Unauthorized("Invalid email or password");

            var token = _userService.GenerateJwtToken(user);

            var expiration = DateTime.UtcNow.AddHours(2);

            var response = new LoginResponseDTO
            {
                Token = token,
                Expiration = expiration
            };

            return Ok(response);
        }
    }
}
