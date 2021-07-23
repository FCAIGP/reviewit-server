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
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ReviewItServer.Tests.Controllers
{
    [TestClass]
    public class ReplyControllerTests
    {
        private DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "TestDb")
           .Options;
        private ReplyController controller;

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MapProfiles.ReplyProfile>();
            });
            controller = new ReplyController(new AppDbContext(dbContextOptions), config.CreateMapper());
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
        public async Task Test_GetReply()
        {
            using var context = new AppDbContext(dbContextOptions);
            var review = (await controller.GetReply("1")).Value;
            review.Body.Should().Be("This is a reply");
        }
        [TestMethod]
        public async Task Test_CreateReply()
        {
            using var context = new AppDbContext(dbContextOptions);
            var newReply = new ReplyDTO
            {
                Body = "Simple Test",
                ParentId = "1",
            };
        
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = MockFactory.MockUserPrincipal("1c197c9a-5ed6-4e57-bf41-eefee3a71e92") }
            };
            var response = (await controller.CreateReply(newReply)).Result as CreatedAtActionResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var reply = context.Replies.Where(reply => reply.Body == "Simple Test").FirstOrDefault();
            reply.Should().NotBeNull();
            reply.AuthorId.Should().Be("1c197c9a-5ed6-4e57-bf41-eefee3a71e92");
        }
       
        [TestMethod]
        public async Task Test_DeleteReply()
        {
            using var context = new AppDbContext(dbContextOptions);
            int old = context.Replies.Count();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = MockFactory.MockUserPrincipal("1c197c9a-5ed6-4e57-bf41-eefee3a71e92") }
            };

            var response = (await controller.DeleteReply("1")) as NoContentResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            context.Replies.Count().Should().Be(old - 1);
            (await context.Replies.FindAsync("1")).Should().BeNull();
        }
    }
}
