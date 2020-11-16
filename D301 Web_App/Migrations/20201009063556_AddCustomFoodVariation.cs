using Microsoft.EntityFrameworkCore.Migrations;

namespace D301_WebApp.Migrations
{
    public partial class AddCustomFoodVariation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomFoodVariations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    CustomFoodId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFoodVariations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodBookmark_CustomFood_CustomFoodId",
                        column: x => x.CustomFoodId,
                        principalTable: "CustomFood",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomFoodVariations");
        }
    }
}
