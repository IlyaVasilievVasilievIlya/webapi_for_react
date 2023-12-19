using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changeBirthDateType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "birth_date",
                table: "users",
                type: "date",
                nullable: false,
                comment: "user birth date",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldComment: "user birth date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "birth_date",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                comment: "user birth date",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "user birth date");
        }
    }
}
