using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using quest_web.Data;
using quest_web.Models;
using System.Linq;
using AutoMapper;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;


        public EquipmentController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        // GET: Equipment/Apartment/5
        [HttpGet("Apartment/{apartmentId}")]
        [AllowAnonymous]
        public IActionResult GetEquipmentsByApartment(int apartmentId)
        {
            var equipments = _context.Equipments
                .Where(e => e.ApartmentId == apartmentId)
                .ToList();
            
            var equipmentsDots = _mapper.Map<List<EquipmentDto>>(equipments);

            return Ok(equipmentsDots);
        }
        
        // GET: Equipment/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetEquipment(int id)
        {
            var equipment = _context.Equipments.Find(id);

            if (equipment == null)
            {
                return NotFound();
            }
            var equipmentsDots = _mapper.Map<EquipmentDto>(equipment);

            return Ok(equipmentsDots);
        }

        // POST: Equipment
        [HttpPost]
        [Authorize(Roles = "ROLE_OWNER,ROLE_ADMIN")]
        public IActionResult AddEquipment([FromBody] EquipmentUpdateModel equipmentDto)
        {
            var currentUser = User?.Identity?.Name;
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == currentUser);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var apartment = _context.Apartments.Find(equipmentDto.ApartmentId);
            if (apartment == null)
            {
                return BadRequest("Invalid ApartmentId.");
            }

            var managementContract = _context.ManagementContracts.SingleOrDefault(mc => mc.ApartmentId == equipmentDto.ApartmentId && mc.UserId == user.Id);
            if (managementContract == null && !(User?.IsInRole("ROLE_ADMIN") ?? false))
            {
                return Forbid();
            }

            var equipment = _mapper.Map<Equipment>(equipmentDto);
            _context.Equipments.Add(equipment);
            _context.SaveChanges();

            var equipmentDtoResponse = _mapper.Map<EquipmentDto>(equipment);
            return CreatedAtAction(nameof(GetEquipment), new { id = equipment.Id }, equipmentDtoResponse);
        }


        // PATCH: Equipment/5
        [HttpPatch("{id}")]
        [Authorize(Roles = "ROLE_OWNER,ROLE_ADMIN")]
        public IActionResult UpdateEquipment(int id, [FromBody] EquipmentUpdateModel updatedEquipment)
        {
            var equipment = _context.Equipments.Find(id);

            if (equipment == null)
            {
                return NotFound();
            }

            var currentUser = User?.Identity?.Name;
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == currentUser);
            var managementContract = _context.ManagementContracts.SingleOrDefault(mc => user != null && mc.ApartmentId == equipment.ApartmentId && mc.UserId == user.Id);

            if (User != null && (user == null || (managementContract == null && !User.IsInRole("ROLE_ADMIN"))))
            {
                return Forbid();
            }

            equipment.Name = updatedEquipment.Name ?? equipment.Name;
            equipment.Quantity = updatedEquipment.Quantity ?? equipment.Quantity;
            equipment.ApartmentId = updatedEquipment.ApartmentId ?? equipment.ApartmentId;

            _context.Equipments.Update(equipment);
            _context.SaveChanges();

            var equipmentDto = _mapper.Map<EquipmentDto>(equipment);

            return Ok(equipmentDto);
        }



        // DELETE: Equipment/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_OWNER,ROLE_ADMIN")]
        public IActionResult DeleteEquipment(int id)
        {
            var equipment = _context.Equipments.Find(id);

            if (equipment == null)
            {
                return NotFound();
            }

            var currentUser = User?.Identity?.Name;
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == currentUser);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var managementContract = _context.ManagementContracts.SingleOrDefault(mc => mc.ApartmentId == equipment.ApartmentId && mc.UserId == user.Id);

            if (managementContract == null && !(User?.IsInRole("ROLE_ADMIN") ?? false))
            {
                return Forbid();
            }

            _context.Equipments.Remove(equipment);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
