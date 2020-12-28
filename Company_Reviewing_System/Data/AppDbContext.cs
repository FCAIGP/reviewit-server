using Company_Reviewing_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company_Reviewing_System.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext() : base()
        {
            
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<ClaimRequest> ClaimRequest { get; set; }
        public DbSet<CompanyPage> CompanyPage { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Reply> Reply { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<StatusChangeRequest> StatusChangeRequest { get; set; }
        public DbSet<Vote> Vote { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            const char delimeter = ',';
            modelBuilder.Entity<CompanyPage>()
                        .Property(e => e.SubscribersEmails)
                        .HasConversion(
                            v => string.Join(delimeter, v),
                            v => v.Split(delimeter, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Post>()
            .Property(e => e.Images)
            .HasConversion(
                v => string.Join(delimeter, v),
                v => v.Split(delimeter, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Review>()
            .Property(e => e.Tags)
            .HasConversion(
                v => string.Join(delimeter, v),
                v => v.Split(delimeter, StringSplitOptions.RemoveEmptyEntries));
            
        }

    }
}
