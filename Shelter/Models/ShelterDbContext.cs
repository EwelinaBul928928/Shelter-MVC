using Microsoft.EntityFrameworkCore;

namespace Shelter.Models
{
    public class ShelterDbContext : DbContext
    {
        public ShelterDbContext(DbContextOptions<ShelterDbContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AdoptionApplication> AdoptionApplications { get; set; }
        public DbSet<VeterinaryVisit> VeterinaryVisits { get; set; }
        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }
        public DbSet<VeterinaryService> VeterinaryServices { get; set; }
        public DbSet<ClientAnimal> ClientAnimals { get; set; }
        public DbSet<VeterinaryAppointment> VeterinaryAppointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdoptionApplication>()
                .HasOne(a => a.User)
                .WithMany(u => u.AdoptionApplications)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdoptionApplication>()
                .HasOne(a => a.Animal)
                .WithMany(an => an.AdoptionApplications)
                .HasForeignKey(a => a.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VeterinaryVisit>()
                .HasOne(v => v.Animal)
                .WithMany(a => a.VeterinaryVisits)
                .HasForeignKey(v => v.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VeterinaryVisit>()
                .HasOne(v => v.Veterinarian)
                .WithMany()
                .HasForeignKey(v => v.VeterinarianId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VeterinaryVisit>()
                .HasOne(v => v.Service)
                .WithMany()
                .HasForeignKey(v => v.ServiceId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<NewsPost>()
                .HasOne(n => n.Author)
                .WithMany(u => u.NewsPosts)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ClientAnimal>()
                .HasOne(c => c.User)
                .WithMany(u => u.ClientAnimals)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VeterinaryAppointment>()
                .HasOne(a => a.Veterinarian)
                .WithMany()
                .HasForeignKey(a => a.VeterinarianId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VeterinaryAppointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VeterinaryAppointment>()
                .HasOne(a => a.Animal)
                .WithMany()
                .HasForeignKey(a => a.AnimalId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VeterinaryAppointment>()
                .HasOne(a => a.ClientAnimal)
                .WithMany()
                .HasForeignKey(a => a.ClientAnimalId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VeterinaryAppointment>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
