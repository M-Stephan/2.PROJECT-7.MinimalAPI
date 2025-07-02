// Using Solutions
using Solution.Services;
using Solution.Users;
using Solution.DTOs;
// Using System for Tasks and lists
using System.Collections.Generic;
using System.Threading.Tasks;

// Using Microsoft for JWT Auth
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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

        // Route for get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            return users.Any() ? Ok(users) : NotFound("No Users Found");
        }

        // Route for get a user by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userService.GetUser(id);
            return user is not null ? Ok(user) : NotFound("User ID Not Found");
        }

        // Route for update a user by ID
        [HttpPut("{id:int}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _userService.UpdateUser(id, user);
            return updated is not null ? Ok(updated) : NotFound("User ID Not Found");
        }

        // Route for delete a user by ID
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool deleted = await _userService.DeleteUser(id);
            return deleted ? Ok($"User {id} deleted") : NotFound("User ID Not Found");
        }

        // Route for login with email & password
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


        // Route for register a new user
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _userService.Register(request);

            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
        }
    }
}
