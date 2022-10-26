using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullStackAssignemntT.Migrations
{
    public partial class updateUSerAndAddShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_Size_SizeId",
                table: "ShopProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Size",
                table: "Size");

            migrationBuilder.RenameTable(
                name: "Size",
                newName: "ShopSize");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ShopAspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ShopAspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopSize",
                table: "ShopSize",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ShopShoppingCart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopShoppingCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopShoppingCart_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopShoppingCart_ShopProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ShopProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopShoppingCart_ApplicationUserId",
                table: "ShopShoppingCart",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopShoppingCart_ProductId",
                table: "ShopShoppingCart",
                column: "ProductId");

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
                name: "ShopShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopSize",
                table: "ShopSize");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ShopAspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ShopAspNetUsers");

            migrationBuilder.RenameTable(
                name: "ShopSize",
                newName: "Size");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Size",
                table: "Size",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_Size_SizeId",
                table: "ShopProducts",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
