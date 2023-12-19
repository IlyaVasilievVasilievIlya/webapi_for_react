using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class OneToOneToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "3beadcc5-d5f7-445f-990b-164e9d54df94", "AQAAAAIAAYagAAAAEOibEqLieGE/ywOuo9PbCDETtRSRb3K3ButMfKbCBeVGT1P6Pjw3IkHQZPpZBUpJIA==", "8575893e-57dd-4654-bf0e-597d6a60e469" });

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "b47d578a-1adc-4438-85d1-9f0e2a86abac", "AQAAAAIAAYagAAAAEPhb0mVRjUoJ3FWh6xxEqlpOvyqLTW9hbbxmi70+0Cn/Uo7YUOai7m3w9Z+ZSxAnLg==", "c3d3e0fd-f7b8-41c7-aaf1-dc2bed70e41f" });

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");
        }
    }
}
