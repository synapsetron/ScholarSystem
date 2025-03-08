using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EntryTask.Infrastructure.Migrations
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
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
                    Subject = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseStudent",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    StudentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudent", x => new { x.CoursesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CourseStudent_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateOfBirth", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alice Brown" },
                    { 2, new DateTime(2001, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bob Wilson" },
                    { 3, new DateTime(2000, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Charlie Davis" },
                    { 4, new DateTime(2001, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diana Miller" },
                    { 5, new DateTime(2000, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edward Thompson" },
                    { 6, new DateTime(2001, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fiona Garcia" },
                    { 7, new DateTime(2000, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "George Martinez" },
                    { 8, new DateTime(2001, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hannah Lee" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Name", "Subject" },
                values: new object[,]
                {
                    { 1, "John Smith", "Mathematics" },
                    { 2, "Sarah Johnson", "Physics" },
                    { 3, "Michael Brown", "Computer Science" },
                    { 4, "Emily Davis", "Chemistry" },
                    { 5, "David Wilson", "Biology" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "TeacherId", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Calculus I" },
                    { 2, 1, "Linear Algebra" },
                    { 3, 2, "Quantum Mechanics" },
                    { 4, 2, "Particle Physics" },
                    { 5, 3, "Programming Fundamentals" },
                    { 6, 3, "Data Structures" },
                    { 7, 4, "Organic Chemistry" },
                    { 8, 5, "Molecular Biology" }
                });

            migrationBuilder.InsertData(
                table: "CourseStudent",
                columns: new[] { "CoursesId", "StudentsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 6 },
                    { 1, 7 },
                    { 2, 2 },
                    { 2, 6 },
                    { 3, 1 },
                    { 3, 3 },
                    { 3, 8 },
                    { 4, 3 },
                    { 5, 1 },
                    { 5, 5 },
                    { 5, 7 },
                    { 6, 2 },
                    { 6, 5 },
                    { 7, 4 },
                    { 7, 8 },
                    { 8, 4 },
                    { 8, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudent_StudentsId",
                table: "CourseStudent",
                column: "StudentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseStudent");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
