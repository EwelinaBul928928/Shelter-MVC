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

            modelBuilder.Entity<NewsPost>()
                .HasOne(n => n.Author)
                .WithMany(u => u.NewsPosts)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
