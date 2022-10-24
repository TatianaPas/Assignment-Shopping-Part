using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullStackAssignemntT.Migrations
{
    public partial class updateProductAddSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "ShopProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShopSize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSize", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_ShopSize_CategoryId",
                table: "ShopProducts",
                column: "CategoryId",
                principalTable: "ShopSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_ShopSize_CategoryId",
                table: "ShopProducts");

            migrationBuilder.DropTable(
                name: "ShopSize");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "ShopProducts");
        }
    }
}
