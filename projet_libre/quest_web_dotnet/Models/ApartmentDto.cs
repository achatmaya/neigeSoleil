namespace quest_web.Models;

public class ApartmentDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string BuildingNumber { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string AddressComplement { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string AdditionalInfo { get; set; } = string.Empty;
    public string ApartmentType { get; set; } = string.Empty;
    public int Area { get; set; }
    public string Exposure { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int DistanceToSlope { get; set; }
    public string ImageUrls { get; set; } = string.Empty;
    public int Amount { get; set; }



public class ApartmentWithReservationsDto : ApartmentDto
{
    public List<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
}


public class ApartmentWithReservationsAndUsersDto : ApartmentDto
{
    public List<ReservationWithUserDto> Reservations { get; set; } = new List<ReservationWithUserDto>();
}

public class ApartmentUpdateModel
{
    public string? Code { get; set; }
    public string? BuildingNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? Address { get; set; }
    public string? AddressComplement { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public int? Floor { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? ApartmentType { get; set; }
    public int? Area { get; set; }
    public string? Exposure { get; set; }
    public int? Capacity { get; set; }
    public int? DistanceToSlope { get; set; }
    public string? ImageUrls { get; set; }
    public int? Amount { get; set; }

}

public class ApartmentCreateRequest
{
    public string Code { get; set; } = string.Empty;
    public string BuildingNumber { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string AddressComplement { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string AdditionalInfo { get; set; } = string.Empty;
    public string ApartmentType { get; set; } = string.Empty;
    public int Area { get; set; } 
    public string Exposure { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int DistanceToSlope { get; set; }
    public string ImageUrls { get; set; } = string.Empty;
    public int Amount { get; set; }
}

}
