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
    public class ReservationController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;


        public ReservationController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/Reservation
        [HttpGet]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IActionResult GetAllReservations()
        {
            var reservations = _context.Reservations.ToList();
            var reservationDots = _mapper.Map<List<ReservationWithApartmentDto>>(reservations);
            return Ok(reservationDots);
        }

        // GET: Reservation/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check if the user is trying to get their own reservation, the owner, or an admin
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

            var managementContract = _context.ManagementContracts.SingleOrDefault(mc => mc.ApartmentId == reservation.ApartmentId && mc.UserId == user.Id);

            if (reservation.UserId != user.Id && managementContract == null && !(User?.IsInRole("ROLE_ADMIN") ?? false))
            {
                return Forbid();
            }
            var reservationDots = _mapper.Map<ReservationWithApartmentDto>(reservation);
            return Ok(reservationDots);
        }

        // GET: Reservation/Owner/5
        [HttpGet("Owner/{ownerId}")]
        [Authorize(Roles = "ROLE_OWNER,ROLE_ADMIN")]
        public IActionResult GetReservationsByOwner(int ownerId)
        {
            var reservations = _context.Reservations
                .Where(r => _context.ManagementContracts.Any(mc => mc.ApartmentId == r.ApartmentId && mc.UserId == ownerId))
                .ToList();
            var reservationDots = _mapper.Map<List<ReservationWithApartmentDto>>(reservations);
            return Ok(reservationDots);
        }

        // GET: Reservation/Apartment/5
        [HttpGet("Apartment/{apartmentId}")]
        [Authorize(Roles = "ROLE_OWNER,ROLE_ADMIN")]
        public IActionResult GetReservationsByApartment(int apartmentId)
        {
            var reservations = _context.Reservations.Where(r => r.ApartmentId == apartmentId).ToList();
            var reservationDots = _mapper.Map<List<ReservationWithApartmentDto>>(reservations);
            return Ok(reservationDots);
        }

        // POST: Reservation
        [HttpPost]
        [Authorize]
        public IActionResult CreateReservation([FromBody] ReservationUpdateModel reservationDto)
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

            var reservation = _mapper.Map<Reservation>(reservationDto);

            // Check for overlapping reservations
            var overlappingReservation = _context.Reservations.Any(r =>
                r.ApartmentId == reservation.ApartmentId &&
                r.StartDate < reservation.EndDate &&
                r.EndDate > reservation.StartDate);

            if (overlappingReservation)
            {
                return Conflict("The apartment is already reserved for the selected dates.");
            }
            
            var apartmentExists = _context.Apartments.Any(a => a.Id == reservation.ApartmentId);
            if (!apartmentExists)
            {
                return BadRequest("The specified apartment does not exist.");
            }

            reservation.UserId = user.Id;
            reservation.ReservationDate = DateTime.Now;
            reservation.ReservationNumber = Guid.NewGuid().ToString();
            int amout = _context.Apartments
                .Where(a => a.Id == reservation.ApartmentId)
                .Select(a =>  a.Amount)
                .FirstOrDefault();
            reservation.Amount = amout * (reservation.EndDate - reservation.StartDate).Days;
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            var reservationDot = _mapper.Map<ReservationDto>(reservation);

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservationDot);
        }


        // PATCH: Reservation/5
        [HttpPatch("{id}")]
        [Authorize]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationUpdateModel updatedReservation)
        {
            var reservation = _context.Reservations.Find(id);

            if (reservation == null)
            {
                return NotFound();
            }

            var currentUser = User?.Identity?.Name;
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == currentUser);
            var managementContract = _context.ManagementContracts.SingleOrDefault(mc => user != null && mc.ApartmentId == reservation.ApartmentId && mc.UserId == user.Id);

            if (User != null && (user == null || (reservation.UserId != user.Id && managementContract == null && !User.IsInRole("ROLE_ADMIN"))))
            {
                return Forbid();
            }

            reservation.Status = updatedReservation.Status ?? reservation.Status;
            reservation.ReservationDate = updatedReservation.ReservationDate ?? reservation.ReservationDate;
            reservation.StartDate = updatedReservation.StartDate ?? reservation.StartDate;
            reservation.EndDate = updatedReservation.EndDate ?? reservation.EndDate;
            reservation.NumberOfPeople = updatedReservation.NumberOfPeople ?? reservation.NumberOfPeople;
            reservation.Amount = updatedReservation.Amount ?? reservation.Amount;
            reservation.ReservationNumber = updatedReservation.ReservationNumber ?? reservation.ReservationNumber;
            reservation.UserId = updatedReservation.UserId ?? reservation.UserId;
            reservation.ApartmentId = updatedReservation.ApartmentId ?? reservation.ApartmentId;

            _context.Reservations.Update(reservation);
            _context.SaveChanges();
            var reservationDots = _mapper.Map<ReservationDto>(reservation);

            return Ok(reservationDots);
        }



        // DELETE: Reservation/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);

            if (reservation == null)
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

            var managementContract = _context.ManagementContracts.SingleOrDefault(mc => mc.ApartmentId == reservation.ApartmentId && mc.UserId == user.Id);

            if (reservation.UserId != user.Id && managementContract == null && !(User?.IsInRole("ROLE_ADMIN") ?? false))
            {
                return Forbid();
            }

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
