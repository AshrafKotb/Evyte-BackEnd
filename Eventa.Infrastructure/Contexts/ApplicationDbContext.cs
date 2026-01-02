using Eventa.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eventa.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestData> RequestsData { get; set; }
        public DbSet<RequestGalleryPhoto> RequestsGallery { get; set; }
        public DbSet<Design> Designs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<GeneralInformation> GeneralInformation { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        // New DbSets
        public DbSet<Memory> Memories { get; set; }
        public DbSet<Dedication> Dedications { get; set; }
        public DbSet<RequestAudio> RequestAudios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Memory -> Request (Many to One)
            modelBuilder.Entity<Memory>()
                .HasOne(m => m.Request)
                .WithMany(r => r.Memories)
                .HasForeignKey(m => m.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Dedication -> Request (Many to One)
            modelBuilder.Entity<Dedication>()
                .HasOne(d => d.Request)
                .WithMany(r => r.Dedications)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // RequestAudio -> Request (One to One)
            modelBuilder.Entity<RequestAudio>()
                .HasOne(a => a.Request)
                .WithOne(r => r.Audio)
                .HasForeignKey<RequestAudio>(a => a.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // RequestGalleryPhoto -> Request (Many to One)
            modelBuilder.Entity<RequestGalleryPhoto>()
                .HasOne(p => p.Request)
                .WithMany(r => r.GalleryPhotos)
                .HasForeignKey(p => p.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
