using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPerformanceApp.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolName = table.Column<string>(type: "text", nullable: false),
                    SchoolCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, defaultValueSql: "\"left\"(concat('Student_', uuid_generate_v4()), 16)"),
                    Sex = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    FamilySize = table.Column<string>(type: "text", nullable: false),
                    ParentStatus = table.Column<string>(type: "text", nullable: false),
                    MotherEducation = table.Column<int>(type: "integer", nullable: false),
                    FatherEducation = table.Column<int>(type: "integer", nullable: false),
                    MotherJob = table.Column<string>(type: "text", nullable: false),
                    FatherJob = table.Column<string>(type: "text", nullable: false),
                    SchoolReason = table.Column<string>(type: "text", nullable: false),
                    Guardian = table.Column<string>(type: "text", nullable: false),
                    TravelTime = table.Column<int>(type: "integer", nullable: false),
                    StudyTime = table.Column<int>(type: "integer", nullable: false),
                    PastFailures = table.Column<int>(type: "integer", nullable: false),
                    ExtraEducationalSupport = table.Column<string>(type: "text", nullable: false),
                    FamilyEducationalSupport = table.Column<string>(type: "text", nullable: false),
                    PaidClasses = table.Column<string>(type: "text", nullable: false),
                    ExtraCurricularActivities = table.Column<string>(type: "text", nullable: false),
                    AttendedNursery = table.Column<string>(type: "text", nullable: false),
                    HigherEducationAspirations = table.Column<string>(type: "text", nullable: false),
                    InternetAccess = table.Column<string>(type: "text", nullable: false),
                    RomanticRelationship = table.Column<string>(type: "text", nullable: false),
                    FamilyRelationshipQuality = table.Column<int>(type: "integer", nullable: false),
                    FreeTime = table.Column<int>(type: "integer", nullable: false),
                    GoingOutWithFriends = table.Column<int>(type: "integer", nullable: false),
                    WorkdayAlcoholConsumption = table.Column<int>(type: "integer", nullable: false),
                    WeekendAlcoholConsumption = table.Column<int>(type: "integer", nullable: false),
                    HealthStatus = table.Column<int>(type: "integer", nullable: false),
                    Absences = table.Column<int>(type: "integer", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    G1 = table.Column<float>(type: "real", nullable: true),
                    G2 = table.Column<float>(type: "real", nullable: true),
                    G3 = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseId",
                table: "Students",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
