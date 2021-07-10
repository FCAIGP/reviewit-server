using ReviewItServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewItServer.Data
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
        public DbSet<ClaimRequest> ClaimRequests { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<StatusChangeRequest> StatusChangeRequests { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            const char delimeter = ',';
            modelBuilder.Entity<Company>()
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
