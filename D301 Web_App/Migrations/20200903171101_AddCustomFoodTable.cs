using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace D301_WebApp.Migrations
{
    public partial class AddCustomFoodTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "CustomFood",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Water = table.Column<float>(nullable: false),
                    Enegry = table.Column<float>(nullable: false),
                    EnegryNIP = table.Column<float>(nullable: false),
                    Protein = table.Column<float>(nullable: false),
                    Fat = table.Column<float>(nullable: false),
                    Carbohydrates = table.Column<float>(nullable: false),
                    DietaryFibre = table.Column<float>(nullable: false),
                    Sugars = table.Column<float>(nullable: false),
                    Starch = table.Column<float>(nullable: false),
                    SFA = table.Column<float>(nullable: false),
                    MUFA = table.Column<float>(nullable: false),
                    PUFA = table.Column<float>(nullable: false),
                    AlphaLinolenicAcid = table.Column<float>(nullable: false),
                    LinoleicAcid = table.Column<float>(nullable: false),
                    Cholesterol = table.Column<float>(nullable: false),
                    Sodium = table.Column<float>(nullable: false),
                    Iodine = table.Column<float>(nullable: false),
                    Potassium = table.Column<float>(nullable: false),
                    Phosphorus = table.Column<float>(nullable: false),
                    Calcium = table.Column<float>(nullable: false),
                    Iron = table.Column<float>(nullable: false),
                    Zinc = table.Column<float>(nullable: false),
                    Selenium = table.Column<float>(nullable: false),
                    VitaminA = table.Column<float>(nullable: false),
                    BetaCarotene = table.Column<float>(nullable: false),
                    Thiamin = table.Column<float>(nullable: false),
                    Riboflavin = table.Column<float>(nullable: false),
                    Niacin = table.Column<float>(nullable: false),
                    VitaminB6 = table.Column<float>(nullable: false),
                    VitaminB12 = table.Column<float>(nullable: false),
                    DietaryFolate = table.Column<float>(nullable: false),
                    VitaminC = table.Column<float>(nullable: false),
                    VitaminD = table.Column<float>(nullable: false),
                    VitaminE = table.Column<float>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFood", x => x.Id);
                });

            migrationBuilder.DropTable(
                name: "Intake");
            migrationBuilder.CreateTable(
                name: "Intake",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<int>(nullable: false),
                    Measure = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    VariationId = table.Column<string>(nullable: true),
                    VariationName = table.Column<string>(nullable: true),
                    FoodId = table.Column<string>(nullable: true),
                    CustomFoodId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intake", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Intake_CustomFood_CustomFoodId",
                        column: x => x.CustomFoodId,
                        principalTable: "CustomFood",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Intake_Food_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Intake_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Intake_CustomFoodId",
                table: "Intake",
                column: "CustomFoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Intake_FoodId",
                table: "Intake",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Intake_UserId",
                table: "Intake",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomFood");

        }
    }
}
