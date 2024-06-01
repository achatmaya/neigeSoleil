using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using quest_web.Data;
using quest_web.Models;
using Microsoft.EntityFrameworkCore;

namespace quest_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApartmentController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;


        public ApartmentController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: Apartment
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllApartments()
        {
            var apartments = _context.Apartments.ToList();
            var apartmentDtos = _mapper.Map<List<ApartmentDto>>(apartments);
            return Ok(apartmentDtos);
        }

        // GET: Apartment/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetOneApartments(int id)
        {
            var apartment = _context.Apartments.Find(id);

            if (apartment == null)
            {
                return NotFound();
            }
            var apartmentDtos = _mapper.Map<ApartmentDto>(apartment);

            return Ok(apartmentDtos);
        }
        
        

        // GET: Apartment/Owner/5
        [HttpGet("Owner/{ownerId}")]
        [AllowAnonymous]
        public IActionResult GetApartmentsByOwner(int ownerId)
        {
            var apartments = _context.Apartments
                .Include(a => a.Reservations)
                .ThenInclude(r => r.User)
                .Where(a => a.ManagementContracts.Any(mc => mc.UserId == ownerId))
                .ToList();
            var apartmentDtos = _mapper.Map<List<ApartmentDto>>(apartments);
            return Ok(apartmentDtos);
        }


        // POST: Apartment
        [HttpPost]
        [Authorize(Roles = "ROLE_OWNER,ROLE_ADMIN")]
        public IActionResult AddApartment([FromBody] Apartment apartment)
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

            // Vérifiez si l'utilisateur a le rôle approprié
            if (User != null && !User.IsInRole("ROLE_OWNER") && !User.IsInRole("ROLE_ADMIN"))
            {
                return Forbid("User does not have the appropriate role.");
            }

            // Ensure ImageUrls is not null
            apartment.ImageUrls = apartment.ImageUrls ?? "[]";

            _context.Apartments.Add(apartment);
            _context.SaveChanges();

            // Create a management contract for the owner
            var managementContract = new ManagementContract
            {
                ApartmentId = apartment.Id,
                UserId = user.Id,
                Status = "Active",
                SignatureDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Code = Guid.NewGuid().ToString()
            };

            _context.ManagementContracts.Add(managementContract);
            _context.SaveChanges();

            var apartmentSimpleDto  = _mapper.Map<ApartmentDto>(apartment);

            return CreatedAtAction(nameof(GetOneApartments), new { id = apartment.Id }, apartmentSimpleDto );
        }
        
        // PATCH: Apartment/5
        [HttpPatch("{id}")]
        [Authorize]
        public IActionResult UpdateApartment(int id, [FromBody] ApartmentDto.ApartmentUpdateModel updatedApartment)
        {
            var apartment = _context.Apartments.Find(id);

            if (apartment == null)
            {
                return NotFound();
            }

            var currentUser = User?.Identity?.Name;
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == currentUser);
            var managementContract = _context.ManagementContracts.SingleOrDefault(mc => user != null && mc.ApartmentId == id && mc.UserId == user.Id);

            if (User != null && (user == null || (managementContract == null && !User.IsInRole("ROLE_ADMIN"))))
            {
                return Forbid();
            }

            apartment.Code = updatedApartment.Code ?? apartment.Code;
            apartment.BuildingNumber = updatedApartment.BuildingNumber ?? apartment.BuildingNumber;
            apartment.ApartmentNumber = updatedApartment.ApartmentNumber ?? apartment.ApartmentNumber;
            apartment.Address = updatedApartment.Address ?? apartment.Address;
            apartment.AddressComplement = updatedApartment.AddressComplement ?? apartment.AddressComplement;
            apartment.PostalCode = updatedApartment.PostalCode ?? apartment.PostalCode;
            apartment.Country = updatedApartment.Country ?? apartment.Country;
            apartment.Floor = updatedApartment.Floor ?? apartment.Floor;
            apartment.AdditionalInfo = updatedApartment.AdditionalInfo ?? apartment.AdditionalInfo;
            apartment.ApartmentType = updatedApartment.ApartmentType ?? apartment.ApartmentType;
            apartment.Area = updatedApartment.Area ?? apartment.Area;
            apartment.Exposure = updatedApartment.Exposure ?? apartment.Exposure;
            apartment.Capacity = updatedApartment.Capacity ?? apartment.Capacity;
            apartment.DistanceToSlope = updatedApartment.DistanceToSlope ?? apartment.DistanceToSlope;
            apartment.ImageUrls = updatedApartment.ImageUrls ?? apartment.ImageUrls;
            apartment.Amount = updatedApartment.Amount ?? apartment.Amount;

            
            _context.Apartments.Update(apartment);
            _context.SaveChanges();

            var apartmentSimpleDto  = _mapper.Map<ApartmentDto>(apartment);
            return Ok(apartmentSimpleDto);
        }

        // DELETE: Apartment/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteApartment(int id)
        {
            var apartment = _context.Apartments.Find(id);

            if (apartment == null)
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

            var managementContract = _context.ManagementContracts.SingleOrDefault(mc => mc.ApartmentId == id && mc.UserId == user.Id);

            if (User != null && managementContract == null && !User.IsInRole("ROLE_ADMIN"))
            {
                return Forbid();
            }

            _context.Apartments.Remove(apartment);
            _context.SaveChanges();

            return NoContent();
        }

        // POST: Apartment/5/RenewContract
        [HttpPost("{id}/RenewContract")]
        [Authorize(Roles = "ROLE_OWNER,ROLE_ADMIN")]
        public IActionResult RenewContract(int id)
        {
            var apartment = _context.Apartments.Find(id);

            if (apartment == null)
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

            var managementContracts = _context.ManagementContracts
                .Where(mc => mc.ApartmentId == id && mc.UserId == user.Id).ToList();

            if (User != null && !managementContracts.Any() && !User.IsInRole("ROLE_ADMIN"))
            {
                return Forbid();
            }

            // Handle multiple management contracts
            foreach (var managementContract in managementContracts)
            {
                if (managementContract.EndDate <= DateTime.Now)
                {
                    managementContract.EndDate = DateTime.Now.AddYears(1);
                    _context.ManagementContracts.Update(managementContract);
                }
            }

            if (!managementContracts.Any())
            {
                var newManagementContract = new ManagementContract
                {
                    ApartmentId = apartment.Id,
                    UserId = user.Id,
                    Status = "Active",
                    SignatureDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(1),
                    Code = Guid.NewGuid().ToString()
                };

                _context.ManagementContracts.Add(newManagementContract);
            }

            _context.SaveChanges();

            return NoContent();
        }
        
    }
}
