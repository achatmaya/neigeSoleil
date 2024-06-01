using Microsoft.EntityFrameworkCore;
using quest_web.Models;

namespace quest_web.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Apartment> Apartments { get; set; } = null!;
        public DbSet<ManagementContract> ManagementContracts { get; set; } = null!;
        public DbSet<Equipment> Equipments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("ROLE_USER");

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Apartment)
                .WithMany(a => a.Reservations)
                .HasForeignKey(r => r.ApartmentId);

            modelBuilder.Entity<ManagementContract>()
                .HasOne(c => c.Apartment)
                .WithMany(a => a.ManagementContracts)
                .HasForeignKey(c => c.ApartmentId);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Apartment)
                .WithMany(a => a.Equipments)
                .HasForeignKey(e => e.ApartmentId);
        }
    }
}