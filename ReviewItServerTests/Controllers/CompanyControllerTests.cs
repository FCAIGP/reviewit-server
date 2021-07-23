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
    public class CompanyControllerTests
    {
        private DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "TestDb")
           .Options;
        private CompanyController controller;

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MapProfiles.CompanyProfile>();
                cfg.AddProfile<MapProfiles.ReviewProfile>();
            });
            controller = new CompanyController(new AppDbContext(dbContextOptions), config.CreateMapper());
            SeedDb();
        }

        private void SeedDb()
        {
            using var context = new AppDbContext(dbContextOptions);
            context.Database.EnsureDeleted();
            context.AddRange(Seeder.GetCompaniesSeed());
            context.AddRange(Seeder.GetReviewsSeed());
            context.SaveChanges();
        }

        [TestMethod]
        public async Task Test_GetCompanyList()
        {
            using var context = new AppDbContext(dbContextOptions);
            var companies = (await controller.GetCompanyList()).Value.ToList();
            companies.Count.Should().Be(2);
            companies.All(r => r.Region == "Global").Should().BeTrue();
            companies[0].Name.Should().Be("Facebook");
            companies[1].Name.Should().Be("Google");
        }
        [TestMethod]
        public async Task Test_GetCompany()
        {
            using var context = new AppDbContext(dbContextOptions);
            var company = (await controller.GetCompany("1")).Value;
            company.Name.Should().Be("Facebook");
            company.Region.Should().Be("Global");
            company.Industry.Should().Be("Internet Content & Information");
        }
        [TestMethod]
        public async Task Test_UpdateCompany()
        {
            using var context = new AppDbContext(dbContextOptions);
            var changedCompany = new CompanyDTO
            {
                Industry = "Technology",
                Name = "Facebook",
                Headquarters = "California",
                Region = "International",
                LogoURL = "https://cutt.ly/Rh459w8"
            };
            var response = (await controller.UpdateCompany("1", changedCompany)) as OkResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var company = (await context.Companies.FindAsync("1"));
            company.Name.Should().Be("Facebook");
            company.Region.Should().Be("International");
            company.Industry.Should().Be("Technology");
            company.Headquarters.Should().Be("California");
        }

        [TestMethod]
        public async Task Test_DeleteCompany()
        {
            using var context = new AppDbContext(dbContextOptions);
            var response = (await controller.DeleteCompany("2")) as NoContentResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            context.Companies.Count().Should().Be(1);
            (await context.Companies.FindAsync("2")).Should().BeNull();
        }

        [TestMethod]
        public async Task Test_GetReviews()
        {
            using var context = new AppDbContext(dbContextOptions);
            var reviews = (await controller.GetReviews("2")).Value.ToList();
            reviews.Count.Should().Be(2);
            reviews[0].Salary.Should().Be(5000);
            reviews[0].Body.Should().Be("This is a review");
            reviews[1].Salary.Should().BeNull();
            reviews[1].Body.Should().Be("This is another review");

        }
    }
}