using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quest_web.Data;
using quest_web.Models;
using quest_web.Utils;
using System.Linq;
using BCrypt.Net;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly JwtTokenUtil _jwtTokenUtil;

        public AuthenticationController(ApiDbContext context, JwtTokenUtil jwtTokenUtil)
        {
            _context = context;
            _jwtTokenUtil = jwtTokenUtil;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {

            // Vérifiez si des attributs requis sont manquants ou invalides
            if (string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                user.BirthDate == default ||
                string.IsNullOrWhiteSpace(user.Address) ||
                string.IsNullOrWhiteSpace(user.PostalCode) ||
                string.IsNullOrWhiteSpace(user.City) ||
                string.IsNullOrWhiteSpace(user.Country) ||
                string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                return BadRequest("Username, password, last name, first name, email, birth date, address, postal code, city, country, and phone number are required.");
            }

            if (_context.Users.Any(u => u.Username.ToLower() == user.Username.ToLower()))
            {
                return Conflict("Username already exists.");
            }

            if (string.IsNullOrWhiteSpace(user.Role))
            {
                user.Role = "ROLE_USER";
            }
            
            if (user.Role == "ROLE_ADMIN" && user.Role != "ROLE_USER" && user.Role != "ROLE_OWNER")
            {
                return Unauthorized("Invalid role.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hacher le mot de passe

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Register), new { id = user.Id }, new { user.Id, user.Username, user.Role });
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginRequest? loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var existingUser = _context.Users.SingleOrDefault(u => u.Username == loginRequest.Username);

            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, existingUser.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _jwtTokenUtil.GenerateToken(existingUser.Username, existingUser.Role);
            return Ok(new { token });
        }



        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var username = User?.Identity?.Name;
            if (username == null)
            {
                return BadRequest("Username is null.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            return Ok(new 
            { 
                user.Id, 
                user.Username, 
                user.Role, 
                user.LastName, 
                user.FirstName, 
                user.Email, 
                user.PhoneNumber, 
                user.BirthDate, 
                user.Address, 
                user.AddressComplement, 
                user.PostalCode, 
                user.City, 
                user.Country 
            });
        }
    }
}
