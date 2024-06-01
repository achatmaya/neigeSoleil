using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quest_web.Data;
using quest_web.Models;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public UserController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: User
        [HttpGet]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            var userDots = _mapper.Map<List<UserDto>>(users);
            return Ok(userDots);
        }

        // GET: User/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }
            var userDots = _mapper.Map<UserDto>(user);
            return Ok(userDots);
        }

        // PATCH: User/5
        [HttpPatch("{id}")]
        [Authorize]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateModel updatedUser)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            // Check if the user is trying to update their own details or if they are an admin
            var currentUser = User?.Identity?.Name;
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            if (User != null && !string.Equals(user.Username, currentUser, StringComparison.OrdinalIgnoreCase) && !User.IsInRole("ROLE_ADMIN"))
            {
                return Forbid();
            }

            // Check if the patch document attempts to change the role
            if (User != null && !string.IsNullOrWhiteSpace(updatedUser.Role) && !User.IsInRole("ROLE_ADMIN"))
            {
                return Forbid("Only admins can modify the role.");
            }

            // Update only the fields that are provided
            user.LastName = updatedUser.LastName ?? user.LastName;
            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.Email = updatedUser.Email ?? user.Email;
            user.PhoneNumber = updatedUser.PhoneNumber ?? user.PhoneNumber;
            user.BirthDate = updatedUser.BirthDate ?? user.BirthDate;
            user.Address = updatedUser.Address ?? user.Address;
            user.AddressComplement = updatedUser.AddressComplement ?? user.AddressComplement;
            user.PostalCode = updatedUser.PostalCode ?? user.PostalCode;
            user.City = updatedUser.City ?? user.City;
            user.Country = updatedUser.Country ?? user.Country;

            if (User != null && User.IsInRole("ROLE_ADMIN") && !string.IsNullOrWhiteSpace(updatedUser.Role))
            {
                user.Role = updatedUser.Role;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
            var userDots = _mapper.Map<UserDto>(user);

            return Ok(userDots);
        }


        // DELETE: User/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            // Check if the user is trying to delete their own account or if they are an admin
            var currentUser = User?.Identity?.Name;
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }            if (User != null && user.Username != currentUser && !User.IsInRole("ROLE_ADMIN"))
            {
                return Forbid();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
