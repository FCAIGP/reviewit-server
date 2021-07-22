using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReviewItServer.Controllers;
using ReviewItServer.Data;
using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewItServer.Controllers.Tests
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
            SeedDb();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapProfiles.CompanyProfile>());
            controller = new CompanyController(new AppDbContext(dbContextOptions), config.CreateMapper());
        }

        private void SeedDb()
        {
            using var context = new AppDbContext(dbContextOptions);
            var companies = new List<Company>
            {
                new Company {
                    CompanyId= "1",
                    Name="Facebook" ,
                    Headquarters="Menlo Park, California, United States",
                    Industry="Internet Content & Information",
                    Region="Global",
                    CreatedDate=new DateTime(2020,12,26),
                    SubscribersEmails=new string[]{"ahmed@gmail.com" },
                    LogoURL="https://cutt.ly/Rh459w8",
                },
                new Company
                {
                    CompanyId="2",
                    Name="Google" ,
                    Headquarters="Mountain View, California, United States",
                    Industry="Internet Cloud computing Computer software",
                    Region="Global",
                    CreatedDate=new DateTime(2020,12,26),
                    LogoURL="https://cutt.ly/Mh46l0C",
                }
            };


            context.AddRange(companies);
            context.SaveChanges();
        }

        [TestMethod]
        public async Task HttpGet_GetCompanyList()
        {
            using var context = new AppDbContext(dbContextOptions);
            var companies = (await controller.GetCompanyList()).Value.ToList();
            companies.Count.Should().Be(2);
            companies.All(r => r.Region == "Global").Should().BeTrue();
            companies[0].Name.Should().Be("Facebook");
            companies[1].Name.Should().Be("Google");
        }
    }
}