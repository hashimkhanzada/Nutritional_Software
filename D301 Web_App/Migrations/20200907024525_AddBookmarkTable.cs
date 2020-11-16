using Microsoft.EntityFrameworkCore.Migrations;

namespace D301_WebApp.Migrations
{
    public partial class AddBookmarkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodBookmark",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: true),
                    FoodId = table.Column<string>(nullable: true),
                    CustomFoodId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodBookmark", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodBookmark_CustomFood_CustomFoodId",
                        column: x => x.CustomFoodId,
                        principalTable: "CustomFood",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FoodBookmark_Food_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FoodBookmark_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodBookmark_CustomFoodId",
                table: "FoodBookmark",
                column: "CustomFoodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodBookmark_FoodId",
                table: "FoodBookmark",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodBookmark_UserId",
                table: "FoodBookmark",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodBookmark");
        }
    }
}
