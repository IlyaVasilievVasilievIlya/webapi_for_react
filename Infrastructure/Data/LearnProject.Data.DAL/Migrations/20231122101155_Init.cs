using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "car_models",
                columns: table => new
                {
                    car_id = table.Column<int>(type: "integer", nullable: false, comment: "car id")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "car brand"),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "car model")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_car_models", x => x.car_id);
                },
                comment: "car models");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false, comment: "user id")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "user password"),
                    login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "user login")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                },
                comment: "users table");

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    car_id = table.Column<int>(type: "integer", nullable: false, comment: "car id")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    car_model_id = table.Column<int>(type: "integer", maxLength: 100, nullable: false, comment: "car model id"),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Car's color")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.car_id);
                    table.ForeignKey(
                        name: "FK_cars_car_models_car_model_id",
                        column: x => x.car_model_id,
                        principalTable: "car_models",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "car table");

            migrationBuilder.InsertData(
                table: "car_models",
                columns: new[] { "car_id", "brand", "model" },
                values: new object[,]
                {
                    { 1, "Toyota", "Mark 1" },
                    { 2, "Mercedes", "Benz" },
                    { 3, "Toyota", "Mark 2" },
                    { 4, "Renault", "Logan" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "login", "password" },
                values: new object[] { 1, "login", "1234" });

            migrationBuilder.InsertData(
                table: "cars",
                columns: new[] { "car_id", "car_model_id", "color" },
                values: new object[,]
                {
                    { 1, 1, "Yellow" },
                    { 2, 2, "Black" },
                    { 3, 2, "White" },
                    { 4, 3, "Yellow" },
                    { 5, 3, "Yellow" },
                    { 6, 1, "Yellow" },
                    { 7, 1, "Yellow" },
                    { 8, 4, "Yellow" },
                    { 9, 4, "Yellow" },
                    { 10, 4, "Yellow" },
                    { 11, 2, "White" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_cars_car_model_id",
                table: "cars",
                column: "car_model_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cars");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "car_models");
        }
    }
}
