using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quest_web.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

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
        [Column("city")]
        public string City { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("country")]
        public string Country { get; set; } = string.Empty;

        [MaxLength(20)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("role")]
        public string Role { get; set; } = string.Empty;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
