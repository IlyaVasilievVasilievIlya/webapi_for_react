using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "asp_net_roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "2c5e174e-3b0e-446f-86af-483d56fd7210", null, "SuperUser", "SUPERUSER" },
                    { "2c5e174e-3b0e-446f-86af-483d56fd7211", null, "Administrator", "ADMINISTRATOR" },
                    { "2c5e174e-3b0e-446f-86af-483d56fd7212", null, "Manager", "MANAGER" },
                    { "2c5e174e-3b0e-446f-86af-483d56fd7213", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "access_failed_count", "birth_date", "concurrency_stamp", "email", "email_confirmed", "lockout_enabled", "lockout_end", "name", "normalized_email", "normalized_user_name", "password_hash", "patronymic", "phone_number", "phone_number_confirmed", "security_stamp", "surname", "two_factor_enabled", "user_name" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, new DateOnly(1999, 2, 2), "e484e650-d4aa-49b1-8f4b-9549376dd8ea", "ilyavasilev56@gmail.com", false, false, null, "Ivan", "ILYAVASILEV56@GMAIL.COM", "ILYAVASILEV56@GMAIL.COM", "AQAAAAIAAYagAAAAEOoVJcEnnv4YQPTzBfMrd5CM1TFTAYXgz1oJnXjaC+oOZx212NTQ3AkSirQAYhJ8lA==", null, null, false, "cf1725a7-7b20-496a-9ea9-08fd4e10fce5", "Ivanov", false, "ilyavasilev56@gmail.com" });

            migrationBuilder.InsertData(
                table: "asp_net_user_roles",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "8e445865-a24d-4543-a6c6-9443d048cdb9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "asp_net_roles",
                keyColumn: "id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7211");

            migrationBuilder.DeleteData(
                table: "asp_net_roles",
                keyColumn: "id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7212");

            migrationBuilder.DeleteData(
                table: "asp_net_roles",
                keyColumn: "id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7213");

            migrationBuilder.DeleteData(
                table: "asp_net_user_roles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

            migrationBuilder.DeleteData(
                table: "asp_net_roles",
                keyColumn: "id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210");

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");
        }
    }
}
