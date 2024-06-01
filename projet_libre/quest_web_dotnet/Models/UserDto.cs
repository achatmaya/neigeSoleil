namespace quest_web.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Address { get; set; } = string.Empty;
    public string AddressComplement { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class UserWithReservationsDto : UserDto
{
    public List<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
}


public class UserWithApartmentsDto : UserDto
{
    public List<ApartmentDto> Apartments { get; set; } = new List<ApartmentDto>();
}


public class UserWithReservationsAndApartmentsDto : UserDto
{
    public List<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
    public List<ApartmentDto> Apartments { get; set; } = new List<ApartmentDto>();
}

public class UserUpdateModel
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public string? AddressComplement { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Role { get; set; }
}
