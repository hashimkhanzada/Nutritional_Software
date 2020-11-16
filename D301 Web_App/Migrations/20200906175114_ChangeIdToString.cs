using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace D301_WebApp.Migrations
{
    public partial class ChangeIdToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropTable(
                name: "CustomFood");

            migrationBuilder.CreateTable(
                name: "CustomFood",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Water = table.Column<float>(nullable: true),
                    Enegry = table.Column<float>(nullable: true),
                    EnegryNIP = table.Column<float>(nullable: true),
                    Protein = table.Column<float>(nullable: true),
                    Fat = table.Column<float>(nullable: true),
                    Carbohydrates = table.Column<float>(nullable: true),
                    DietaryFibre = table.Column<float>(nullable: true),
                    Sugars = table.Column<float>(nullable: true),
                    Starch = table.Column<float>(nullable: true),
                    SFA = table.Column<float>(nullable: true),
                    MUFA = table.Column<float>(nullable: true),
                    PUFA = table.Column<float>(nullable: true),
                    AlphaLinolenicAcid = table.Column<float>(nullable: true),
                    LinoleicAcid = table.Column<float>(nullable: true),
                    Cholesterol = table.Column<float>(nullable: true),
                    Sodium = table.Column<float>(nullable: true),
                    Iodine = table.Column<float>(nullable: true),
                    Potassium = table.Column<float>(nullable: true),
                    Phosphorus = table.Column<float>(nullable: true),
                    Calcium = table.Column<float>(nullable: true),
                    Iron = table.Column<float>(nullable: true),
                    Zinc = table.Column<float>(nullable: true),
                    Selenium = table.Column<float>(nullable: true),
                    VitaminA = table.Column<float>(nullable: true),
                    BetaCarotene = table.Column<float>(nullable: true),
                    Thiamin = table.Column<float>(nullable: true),
                    Riboflavin = table.Column<float>(nullable: true),
                    Niacin = table.Column<float>(nullable: true),
                    VitaminB6 = table.Column<float>(nullable: true),
                    VitaminB12 = table.Column<float>(nullable: true),
                    DietaryFolate = table.Column<float>(nullable: true),
                    VitaminC = table.Column<float>(nullable: true),
                    VitaminD = table.Column<float>(nullable: true),
                    VitaminE = table.Column<float>(nullable: true),
                    Name = table.Column<string>(nullable: false),
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
                    CustomFoodId = table.Column<string>(nullable: true),
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
            migrationBuilder.DropForeignKey(
                name: "FK_Intake_CustomFood_CustomFoodId",
                table: "Intake");

            migrationBuilder.AlterColumn<int>(
                name: "CustomFoodId",
                table: "Intake",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CustomFood",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Intake_CustomFood_CustomFoodId",
                table: "Intake",
                column: "CustomFoodId",
                principalTable: "CustomFood",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
