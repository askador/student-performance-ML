using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPerformanceApp.Migrations
{
    /// <inheritdoc />
    public partial class default_student_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValueSql: "\"left\"(concat('Student_', uuid_generate_v4()), 16)",
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Students",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "\"left\"(concat('Student_', uuid_generate_v4()), 16)");
        }
    }
}
