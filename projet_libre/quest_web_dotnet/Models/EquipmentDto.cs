namespace quest_web.Models;

public class EquipmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class EquipmentWithApartmentDto : EquipmentDto
{
    public ApartmentDto Apartment { get; set; } = new ApartmentDto();
}

public class EquipmentUpdateModel
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public int? ApartmentId { get; set; }
}
