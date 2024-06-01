using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quest_web.Models
{
    [Table("reservation")]
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(50)]
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("reservation_date")]
        public DateTime ReservationDate { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("number_of_people")]
        public int NumberOfPeople { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [MaxLength(50)]
        [Column("reservation_number")]
        public string ReservationNumber { get; set; } = string.Empty;

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public Apartment Apartment { get; set; } = null!;
    }
}