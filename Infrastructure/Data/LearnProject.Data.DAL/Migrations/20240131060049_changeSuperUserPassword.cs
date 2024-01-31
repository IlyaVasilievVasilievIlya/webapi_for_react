using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changeSuperUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "941e3423-e7b4-4e7c-a397-61ba5b7e993f", "AQAAAAIAAYagAAAAEJfBCBz0CcYDw9wyDgiqJNlCbqqISolvb5y2xTO+Gsuk7NDdeKm3XltgXOgKEGIZXA==", "c9f83eb4-9c17-4f16-9ea8-519cb0665806" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "3beadcc5-d5f7-445f-990b-164e9d54df94", "AQAAAAIAAYagAAAAEOibEqLieGE/ywOuo9PbCDETtRSRb3K3ButMfKbCBeVGT1P6Pjw3IkHQZPpZBUpJIA==", "8575893e-57dd-4654-bf0e-597d6a60e469" });
        }
    }
}
