using Microsoft.EntityFrameworkCore;
using EntryTask.Domain.Entities;

namespace EntryTask.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Teachers
            var teacher1 = new Teacher { Id = 1, Name = "John Smith", Subject = "Mathematics" };
            var teacher2 = new Teacher { Id = 2, Name = "Sarah Johnson", Subject = "Physics" };

            modelBuilder.Entity<Teacher>().HasData(teacher1, teacher2);

            // Courses
            var course1 = new Course { Id = 1, Title = "Algebra 101", TeacherId = teacher1.Id };
            var course2 = new Course { Id = 2, Title = "Quantum Mechanics", TeacherId = teacher2.Id };
            var course3 = new Course { Id = 3, Title = "Linear Algebra", TeacherId = teacher1.Id };

            modelBuilder.Entity<Course>().HasData(course1, course2, course3);

            // Students
            var student1 = new Student { Id = 1, Name = "Alice Brown", DateOfBirth = new DateTime(2000, 5, 20) };
            var student2 = new Student { Id = 2, Name = "Bob White", DateOfBirth = new DateTime(1999, 11, 10) };
            var student3 = new Student { Id = 3, Name = "Charlie Green", DateOfBirth = new DateTime(2001, 3, 15) };

            modelBuilder.Entity<Student>().HasData(student1, student2, student3);

            // Students that enrolled  to courses
            modelBuilder.Entity<StudentCourse>().HasData(
                new StudentCourse { StudentId = 1, CourseId = 1 }, // Alice → Algebra 101
                new StudentCourse { StudentId = 1, CourseId = 2 }, // Alice → Quantum Mechanics
                new StudentCourse { StudentId = 2, CourseId = 1 }, // Bob → Algebra 101
                new StudentCourse { StudentId = 2, CourseId = 3 }, // Bob → Linear Algebra
                new StudentCourse { StudentId = 3, CourseId = 3 }  // Charlie → Linear Algebra
            );
        }
    }
}
