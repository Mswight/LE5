using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        private static List<User> _users = new List<User>()
        {
            new User
            {
                Id = 1,
                Username = "john@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            },
            new User
            {
                Id = 2,
                Username = "jane@example.com",
                Password = "password123",
                FirstName = "Jane",
                LastName = "Smith"
            }
        };

        // POST: api/login/login
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"Login attempt for user: {request.Username}");

            var user = _users.FirstOrDefault(u =>
                u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                _logger.LogWarning($"Failed login attempt for user: {request.Username}");
                return Unauthorized(new { message = "Invalid username or password" });
            }

            _logger.LogInformation($"Successful login for user: {request.Username}");
            return Ok(new LoginResponse
            {
                Id_token = Guid.NewGuid().ToString(),
                Id = user.Id
            });
        }

        // POST: api/login/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            _logger.LogInformation($"Registration attempt for user: {newUser.Username}");

            // Check if user already exists
            if (_users.Any(u => u.Username == newUser.Username))
            {
                _logger.LogWarning($"User {newUser.Username} already exists");
                return BadRequest(new { message = "User already exists" });
            }

            newUser.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(newUser);

            _logger.LogInformation($"User {newUser.Username} registered successfully");
            return Ok("Registration successful");
        }
    }
}