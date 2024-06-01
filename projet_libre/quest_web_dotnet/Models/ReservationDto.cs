namespace quest_web.Models;

public class ReservationDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ReservationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal Amount { get; set; }
    public string ReservationNumber { get; set; } = string.Empty;
}

public class ReservationWithUserDto : ReservationDto
{
    public UserDto User { get; set; } = new UserDto();
}

public class ReservationWithApartmentDto : ReservationDto
{
    public ApartmentDto Apartment { get; set; } = new ApartmentDto();
}

public class ReservationWithUserAndApartmentDto : ReservationDto
{
    public UserDto User { get; set; } = new UserDto();
    public ApartmentDto Apartment { get; set; } = new ApartmentDto();
}

public class ReservationUpdateModel
{
    public string? Status { get; set; }
    public DateTime? ReservationDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? NumberOfPeople { get; set; }
    public decimal? Amount { get; set; }
    public string? ReservationNumber { get; set; }
    public int? UserId { get; set; }
    public int? ApartmentId { get; set; }
}



