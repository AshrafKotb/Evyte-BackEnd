using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Infrastructure
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
        public DbSet<DesignCategory> DesignsCategories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<GeneralInfomarion> GeneralInfomarion { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Request -> GalleryPhotos (One-to-Many)
            modelBuilder.Entity<Request>()
                .HasMany(r => r.GalleryPhotos)
                .WithOne(p => p.Request)
                .HasForeignKey(p => p.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Request -> RequestData (One-to-One)
            modelBuilder.Entity<Request>()
                .HasOne(r => r.RequestData)
                .WithOne()
                .HasForeignKey<Request>(r => r.RequestDataId)
                .OnDelete(DeleteBehavior.Cascade);

            // Request -> Design (One-to-One)
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Design)
                .WithOne()
                .HasForeignKey<Request>(r => r.DesignId)
                .OnDelete(DeleteBehavior.Cascade);

            // Request -> User (Many-to-One)
            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
