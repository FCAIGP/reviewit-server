using ReviewItServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewItServer.Tests
{
    public class Seeder
    {
        public static List<User> GetUsersSeed()
        {
            return new List<User>{
                new User
                {
                    Id = "1c197c9a-5ed6-4e57-bf41-eefee3a71e92",
                    FirstName = "Abdelrahman",
                    LastName = "Ahmed",
                    CurrentJob = "Tester",
                    CurrentCompanyCompanyId = null,
                    DateHired = new DateTime(2020, 12, 26),
                    Bio = null,
                    Image = null,
                    UserName = "abdo123",
                    NormalizedUserName = "ABDO123",
                    Email = "abdo@gmail.com",
                    NormalizedEmail = "ABDO@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEIjrQ5k2luEHk6y5tKwtpHM2TD3ZemV8fyfOUPntzmc+m4wy2UujSYixX2o+a1YuEQ==",
                    SecurityStamp = "3NTG6V6XA66QJVBE56QUC2SZQMDO35Z4",
                    ConcurrencyStamp = "a0304445-380f-430a-bc56-871d5b847899",
                    PhoneNumber = "123456789",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                }
            };
        }
        public static List<Company> GetCompaniesSeed()
        {
            return new List<Company>
            {
                new Company
                {
                    CompanyId = "1",
                    Name = "Facebook",
                    Headquarters = "Menlo Park, California, United States",
                    Industry = "Internet Content & Information",
                    Region = "Global",
                    CreatedDate = new DateTime(2020, 12, 26),
                    SubscribersEmails = new string[] { "ahmed@gmail.com" },
                    LogoURL = "https://cutt.ly/Rh459w8",
                },
                new Company
                {
                    CompanyId = "2",
                    Name = "Google",
                    Headquarters = "Mountain View, California, United States",
                    Industry = "Internet Cloud computing Computer software",
                    Region = "Global",
                    CreatedDate = new DateTime(2020, 12, 26),
                    LogoURL = "https://cutt.ly/Mh46l0C",
                    OwnerId="1c197c9a-5ed6-4e57-bf41-eefee3a71e92"
                }
            };
        }

        public static List<Review> GetReviewsSeed()
        {
            return new List<Review>
            {
                new Review
                {
                    ReviewId="1",
                    CompanyId="2",
                    Body="This is a review",
                    Tags=new string[]{"a","b","c"},
                    Salary=5000
                },
                new Review
                {
                    ReviewId="2",
                    CompanyId="2",
                    Body="This is another review",
                    AuthorId="1c197c9a-5ed6-4e57-bf41-eefee3a71e92",
                    Tags=new string[]{"a","b","d"}
                },
                new Review
                {
                    ReviewId="3",
                    CompanyId="1",
                    Body="This is yet another review",
                    Tags=new string[]{"a","b","d"}
                }
            };
        }

        public static List<Reply> GetRepliesSeed()
        {
            return new List<Reply>
            {
                new Reply
                {
                    ReplyId="1",
                    AuthorId="1c197c9a-5ed6-4e57-bf41-eefee3a71e92",
                    ParentId="3",
                    Body="This is a reply",
                    Date= new DateTime(2021,1,1),
                },
                new Reply
                {
                    ReplyId="2",
                    AuthorId="1c197c9a-5ed6-4e57-bf41-eefee3a71e92",
                    ParentId="2",
                    Body="This is another reply",
                    Date= new DateTime(2021,12,12),
                },
            };
        }
    }
}
