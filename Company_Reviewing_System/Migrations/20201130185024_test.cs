using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Company_Reviewing_System.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "ClaimRequest",
                columns: table => new
                {
                    ClaimRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdentificationCard = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProofOfWork = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LinkedInAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmitterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimStatus = table.Column<int>(type: "int", nullable: false),
                    CompanyPageCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimRequest", x => x.ClaimRequestId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPage",
                columns: table => new
                {
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Headquarters = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscribersEmails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoURL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PendingStatusChange = table.Column<bool>(type: "bit", nullable: false),
                    IsScoreUpToDate = table.Column<bool>(type: "bit", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    AcceptedClaimRequestClaimRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CloseStatus = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPage", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanyPage_ClaimRequest_AcceptedClaimRequestClaimRequestId",
                        column: x => x.AcceptedClaimRequestClaimRequestId,
                        principalTable: "ClaimRequest",
                        principalColumn: "ClaimRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentJob = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CurrentCompanyCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateHired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_CompanyPage_CurrentCompanyCompanyId",
                        column: x => x.CurrentCompanyCompanyId,
                        principalTable: "CompanyPage",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatusChangeRequest",
                columns: table => new
                {
                    RequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyPageCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusChangeRequest", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_StatusChangeRequest_CompanyPage_CompanyPageCompanyId",
                        column: x => x.CompanyPageCompanyId,
                        principalTable: "CompanyPage",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PageCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Post_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_CompanyPage_PageCompanyId",
                        column: x => x.PageCompanyId,
                        principalTable: "CompanyPage",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: true),
                    JobDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyPageCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_CompanyPage_CompanyPageCompanyId",
                        column: x => x.CompanyPageCompanyId,
                        principalTable: "CompanyPage",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reply",
                columns: table => new
                {
                    ReplyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentReviewId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reply", x => x.ReplyId);
                    table.ForeignKey(
                        name: "FK_Reply_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reply_Review_ParentReviewId",
                        column: x => x.ParentReviewId,
                        principalTable: "Review",
                        principalColumn: "ReviewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthorIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_Review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review",
                        principalColumn: "ReviewId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    VoteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsUpVote = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Vote_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vote_Review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review",
                        principalColumn: "ReviewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CurrentCompanyCompanyId",
                table: "AspNetUsers",
                column: "CurrentCompanyCompanyId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimRequest_CompanyPageCompanyId",
                table: "ClaimRequest",
                column: "CompanyPageCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimRequest_SubmitterId",
                table: "ClaimRequest",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPage_AcceptedClaimRequestClaimRequestId",
                table: "CompanyPage",
                column: "AcceptedClaimRequestClaimRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPage_OwnerId",
                table: "CompanyPage",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_AuthorId",
                table: "Post",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PageCompanyId",
                table: "Post",
                column: "PageCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reply_AuthorId",
                table: "Reply",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reply_ParentReviewId",
                table: "Reply",
                column: "ParentReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_AuthorId",
                table: "Report",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ReviewId",
                table: "Report",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_AuthorId",
                table: "Review",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_CompanyPageCompanyId",
                table: "Review",
                column: "CompanyPageCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeRequest_CompanyPageCompanyId",
                table: "StatusChangeRequest",
                column: "CompanyPageCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_ReviewId",
                table: "Vote",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_UserId",
                table: "Vote",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimRequest_AspNetUsers_SubmitterId",
                table: "ClaimRequest",
                column: "SubmitterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimRequest_CompanyPage_CompanyPageCompanyId",
                table: "ClaimRequest",
                column: "CompanyPageCompanyId",
                principalTable: "CompanyPage",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyPage_AspNetUsers_OwnerId",
                table: "CompanyPage",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimRequest_AspNetUsers_SubmitterId",
                table: "ClaimRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyPage_AspNetUsers_OwnerId",
                table: "CompanyPage");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimRequest_CompanyPage_CompanyPageCompanyId",
                table: "ClaimRequest");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Reply");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "StatusChangeRequest");

            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CompanyPage");

            migrationBuilder.DropTable(
                name: "ClaimRequest");
        }
    }
}
