using Company_Reviewing_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company_Reviewing_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ClaimRequest> ClaimRequests;
        public DbSet<CompanyPage> CompanyPages;
        public DbSet<Post> Posts;
        public DbSet<Reply> Replies;
        public DbSet<Report> Reports;
        public DbSet<Review> Reviews;
        public DbSet<StatusChangeRequest> StatusChangeRequests;
        public DbSet<Vote> Votes;


        #region Required
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
        #endregion
    }
}
