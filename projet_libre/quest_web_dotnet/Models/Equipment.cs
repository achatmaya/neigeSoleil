using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quest_web.Models
{
    [Table("equipment")]
    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("quantity")]
        public int Quantity { get; set; }

        public int ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public Apartment Apartment { get; set; } = null!;
    }
}