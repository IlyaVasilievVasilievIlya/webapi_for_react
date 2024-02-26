using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ConfirmSuperUserEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "concurrency_stamp", "email_confirmed", "password_hash", "security_stamp" },
                values: new object[] { "6c78d6b3-de8f-4d74-be35-2d1e40af8ac2", true, "AQAAAAIAAYagAAAAELrzq87cwO0Z/wL+G5McrEECRabNarKED5W3z/0cCBr3wQIydoAedEN87wKKzsGK1Q==", "68d5b256-bca5-48aa-82fc-374b41aafb04" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "concurrency_stamp", "email_confirmed", "password_hash", "security_stamp" },
                values: new object[] { "941e3423-e7b4-4e7c-a397-61ba5b7e993f", false, "AQAAAAIAAYagAAAAEJfBCBz0CcYDw9wyDgiqJNlCbqqISolvb5y2xTO+Gsuk7NDdeKm3XltgXOgKEGIZXA==", "c9f83eb4-9c17-4f16-9ea8-519cb0665806" });
        }
    }
}
