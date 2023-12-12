using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    token = table.Column<string>(type: "text", nullable: false, comment: "token"),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "token creation date"),
                    expiry_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "token expiry date"),
                    user_id = table.Column<string>(type: "text", nullable: false, comment: "user id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.token);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "e484e650-d4aa-49b1-8f4b-9549376dd8ea", "AQAAAAIAAYagAAAAEOoVJcEnnv4YQPTzBfMrd5CM1TFTAYXgz1oJnXjaC+oOZx212NTQ3AkSirQAYhJ8lA==", "cf1725a7-7b20-496a-9ea9-08fd4e10fce5" });
        }
    }
}
