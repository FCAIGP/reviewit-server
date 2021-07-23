using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReviewItServer.Controllers;
using ReviewItServer.Data;
using ReviewItServer.DTOs;
using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Tests.Controllers
{
    [TestClass]
    public class ReviewControllerTests
    {
        private DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "TestDb")
           .Options;
        private ReviewController controller;

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MapProfiles.ReviewProfile>();
                cfg.AddProfile<MapProfiles.ReplyProfile>();
            });
            controller = new ReviewController(new AppDbContext(dbContextOptions), config.CreateMapper());
            SeedDb();
        }

        private void SeedDb()
        {
            using var context = new AppDbContext(dbContextOptions);
            context.Database.EnsureDeleted();
            context.AddRange(Seeder.GetCompaniesSeed());
            context.AddRange(Seeder.GetUsersSeed());
            context.AddRange(Seeder.GetReviewsSeed());
            context.AddRange(Seeder.GetRepliesSeed());
            context.SaveChanges();
        }

        [TestMethod]
        public async Task Test_GetReview()
        {
            using var context = new AppDbContext(dbContextOptions);
            var review = (await controller.GetReview("1")).Value;
            review.Body.Should().Be("This is a review");
            review.Salary.Should().Be(5000);
            review.Tags.Length.Should().Be(3);
        }
        [TestMethod]
        public async Task Test_CreateReview()
        {
            using var context = new AppDbContext(dbContextOptions);
            var newReview = new ReviewDTO
            {
                Body = "Simple Test",
                IsAnonymous = true,
                CompanyId = "1",
                Contact = "test@test.com",
                JobDescription = "Testing",
                Tags = new string[] { "environment" },
            };
            var request = new DefaultHttpContext();
            request.Connection.RemoteIpAddress = new System.Net.IPAddress(new byte[] { 50,200,100,200 });
            controller.ControllerContext = new ControllerContext { HttpContext = request };
            var response = (await controller.CreateReview(newReview)).Result as CreatedAtActionResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var review = context.Reviews.Where((review) => review.Body == "Simple Test").FirstOrDefault();
            review.Should().NotBeNull();
            review.Contact.Should().Be("test@test.com");
            review.AuthorIP.Should().Be("50.200.100.200");
        }
        [TestMethod]
        public async Task Test_DeleteReview()
        {
            using var context = new AppDbContext(dbContextOptions);
            int old = context.Reviews.Count();
            var response = (await controller.DeleteReview("1")) as NoContentResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            (await context.Reviews.FindAsync("1")).Should().BeNull();
            context.Reviews.Count().Should().Be(old - 1);
        }

        [TestMethod]
        public async Task Test_GetReplies()
        {
            using var context = new AppDbContext(dbContextOptions);
            var replies = (await controller.GetReplies("3")).Value.ToList();
            replies.Count.Should().Be(1);
            replies[0].Body.Should().Be("This is a reply");
        }
    }
}
