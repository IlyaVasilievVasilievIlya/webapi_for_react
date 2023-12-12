using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LearnProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class dropCustomUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false, comment: "user id")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "user login"),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "user password")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                },
                comment: "users table");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "login", "password" },
                values: new object[] { 1, "login", "1234" });
        }
    }
}
