using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quest_web.Models
{
    [Table("apartment")]
    public class Apartment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(50)]
        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("building_number")]
        public string BuildingNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("apartment_number")]
        public string ApartmentNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("address_complement")]
        public string AddressComplement { get; set; } = string.Empty;

        [MaxLength(10)]
        [Column("postal_code")]
        public string PostalCode { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("country")]
        public string Country { get; set; } = string.Empty;

        [Column("floor")]
        public int Floor { get; set; }

        [MaxLength(255)]
        [Column("additional_infos")]
        public string AdditionalInfo { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("apartment_type")]
        public string ApartmentType { get; set; } = string.Empty;

        [Column("area")]
        public int Area { get; set; }

        [MaxLength(50)]
        [Column("exposure")]
        public string Exposure { get; set; } = string.Empty;

        [Column("capacity")]
        public int Capacity { get; set; }
        
        [Column("amount")]
        public int Amount { get; set; }

        [Column("distance_to_slope")]
        public int DistanceToSlope { get; set; }
        
        [Column("image_urls")]
        public string ImageUrls { get; set; } = "[]";

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<ManagementContract> ManagementContracts { get; set; } = new List<ManagementContract>();
        public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    }
}
