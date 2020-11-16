using Microsoft.EntityFrameworkCore.Migrations;

namespace D301_WebApp.Migrations
{
    public partial class ColumnsAddedFoodTypeAlcoholLactate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<float>(
                name: "Alcohol",
                table: "Rdi",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "Lactating",
                table: "Rdi",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pregnant",
                table: "Rdi",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "Intake",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Alcohol",
                table: "Food",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Alcohol",
                table: "CustomFood",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AlcoholGoal",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "Lactating",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pregnant",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alcohol",
                table: "Rdi");

            migrationBuilder.DropColumn(
                name: "Lactating",
                table: "Rdi");

            migrationBuilder.DropColumn(
                name: "Pregnant",
                table: "Rdi");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "Intake");

            migrationBuilder.DropColumn(
                name: "Alcohol",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "Alcohol",
                table: "CustomFood");

            migrationBuilder.DropColumn(
                name: "AlcoholGoal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Lactating",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Pregnant",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Ethnicity",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }
    }
}
