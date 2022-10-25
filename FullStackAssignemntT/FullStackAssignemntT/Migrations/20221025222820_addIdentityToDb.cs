using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullStackAssignemntT.Migrations
{
    public partial class addIdentityToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_ShopSize_CategoryId",
                table: "ShopProducts");

            migrationBuilder.CreateTable(
                name: "ShopAspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopAspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopAspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_ShopAspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopAspNetRoleClaims",
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
                    table.PrimaryKey("PK_ShopAspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopAspNetRoleClaims_ShopAspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ShopAspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopAspNetUserClaims",
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
                    table.PrimaryKey("PK_ShopAspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopAspNetUserClaims_ShopAspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ShopAspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopAspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopAspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_ShopAspNetUserLogins_ShopAspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ShopAspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopAspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopAspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ShopAspNetUserRoles_ShopAspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ShopAspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopAspNetUserRoles_ShopAspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ShopAspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopAspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopAspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_ShopAspNetUserTokens_ShopAspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ShopAspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopProducts_SizeId",
                table: "ShopProducts",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopAspNetRoleClaims_RoleId",
                table: "ShopAspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "ShopAspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShopAspNetUserClaims_UserId",
                table: "ShopAspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopAspNetUserLogins_UserId",
                table: "ShopAspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopAspNetUserRoles_RoleId",
                table: "ShopAspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "ShopAspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "ShopAspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_ShopSize_SizeId",
                table: "ShopProducts",
                column: "SizeId",
                principalTable: "ShopSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_ShopSize_SizeId",
                table: "ShopProducts");

            migrationBuilder.DropTable(
                name: "ShopAspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "ShopAspNetUserClaims");

            migrationBuilder.DropTable(
                name: "ShopAspNetUserLogins");

            migrationBuilder.DropTable(
                name: "ShopAspNetUserRoles");

            migrationBuilder.DropTable(
                name: "ShopAspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ShopAspNetRoles");

            migrationBuilder.DropTable(
                name: "ShopAspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_ShopProducts_SizeId",
                table: "ShopProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_ShopSize_CategoryId",
                table: "ShopProducts",
                column: "CategoryId",
                principalTable: "ShopSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
