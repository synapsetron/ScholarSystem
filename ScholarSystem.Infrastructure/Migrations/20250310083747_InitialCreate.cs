using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScholarSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => new { x.StudentId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Email", "EnrollmentDate", "Name" },
                values: new object[,]
                {
                    { 1, "alice.brown@email.com", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alice Brown" },
                    { 2, "bob.wilson@email.com", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bob Wilson" },
                    { 3, "charlie.davis@email.com", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Charlie Davis" },
                    { 4, "diana.miller@email.com", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diana Miller" },
                    { 5, "edward.thompson@email.com", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edward Thompson" },
                    { 6, "fiona.garcia@email.com", new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fiona Garcia" },
                    { 7, "george.martinez@email.com", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "George Martinez" },
                    { 8, "hannah.lee@email.com", new DateTime(2023, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hannah Lee" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "john.smith@email.com", "John Smith" },
                    { 2, "sarah.johnson@email.com", "Sarah Johnson" },
                    { 3, "michael.brown@email.com", "Michael Brown" },
                    { 4, "emily.davis@email.com", "Emily Davis" },
                    { 5, "david.wilson@email.com", "David Wilson" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Credits", "Description", "TeacherId", "Title" },
                values: new object[,]
                {
                    { 1, 3, "Basic introduction to calculus.", 1, "Calculus I" },
                    { 2, 4, "Study of vectors, matrices, and linear transformations.", 1, "Linear Algebra" },
                    { 3, 3, "Introduction to quantum physics.", 2, "Quantum Mechanics" },
                    { 4, 4, "Exploring subatomic particles and interactions.", 2, "Particle Physics" },
                    { 5, 3, "Learn basic programming concepts.", 3, "Programming Fundamentals" },
                    { 6, 4, "Introduction to common data structures.", 3, "Data Structures" },
                    { 7, 3, "Study of organic compounds and reactions.", 4, "Organic Chemistry" },
                    { 8, 3, "Exploring the molecular mechanisms of life.", 5, "Molecular Biology" }
                });

            migrationBuilder.InsertData(
                table: "StudentCourses",
                columns: new[] { "CourseId", "StudentId", "EnrollmentDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 2, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, new DateTime(2022, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 3, new DateTime(2022, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 4, new DateTime(2023, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 4, new DateTime(2023, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, new DateTime(2022, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 5, new DateTime(2022, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 6, new DateTime(2023, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 6, new DateTime(2023, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 7, new DateTime(2022, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 7, new DateTime(2022, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 7, new DateTime(2022, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 8, new DateTime(2023, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 8, new DateTime(2023, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentCourses",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
