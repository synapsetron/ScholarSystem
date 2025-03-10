using Microsoft.EntityFrameworkCore;
using ScholarSystem.Domain.Entities;
using System;

namespace ScholarSystem.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed Teachers
            var teachers = new[]
            {
                new Teacher { Id = 1, Name = "John Smith", Email = "john.smith@email.com" },
                new Teacher { Id = 2, Name = "Sarah Johnson", Email = "sarah.johnson@email.com" },
                new Teacher { Id = 3, Name = "Michael Brown", Email = "michael.brown@email.com" },
                new Teacher { Id = 4, Name = "Emily Davis", Email = "emily.davis@email.com" },
                new Teacher { Id = 5, Name = "David Wilson", Email = "david.wilson@email.com" }
            };
            modelBuilder.Entity<Teacher>().HasData(teachers);

            // Seed Courses
            var courses = new[]
             {
                new Course { Id = 1, Title = "Calculus I", Description = "Basic introduction to calculus.", TeacherId = 1, Credits = 3 },
                new Course { Id = 2, Title = "Linear Algebra", Description = "Study of vectors, matrices, and linear transformations.", TeacherId = 1, Credits = 4 },
                new Course { Id = 3, Title = "Quantum Mechanics", Description = "Introduction to quantum physics.", TeacherId = 2, Credits = 3 },
                new Course { Id = 4, Title = "Particle Physics", Description = "Exploring subatomic particles and interactions.", TeacherId = 2, Credits = 4 },
                new Course { Id = 5, Title = "Programming Fundamentals", Description = "Learn basic programming concepts.", TeacherId = 3, Credits = 3 },
                new Course { Id = 6, Title = "Data Structures", Description = "Introduction to common data structures.", TeacherId = 3, Credits = 4 },
                new Course { Id = 7, Title = "Organic Chemistry", Description = "Study of organic compounds and reactions.", TeacherId = 4, Credits = 3 },
                new Course { Id = 8, Title = "Molecular Biology", Description = "Exploring the molecular mechanisms of life.", TeacherId = 5, Credits = 3 }
            };
            modelBuilder.Entity<Course>().HasData(courses);

            // Seed Students
            var students = new[]
            {
                new Student { Id = 1, Name = "Alice Brown", Email = "alice.brown@email.com", EnrollmentDate = new DateTime(2022, 9, 1) },
                new Student { Id = 2, Name = "Bob Wilson", Email = "bob.wilson@email.com", EnrollmentDate = new DateTime(2023, 1, 10) },
                new Student { Id = 3, Name = "Charlie Davis", Email = "charlie.davis@email.com", EnrollmentDate = new DateTime(2022, 9, 1) },
                new Student { Id = 4, Name = "Diana Miller", Email = "diana.miller@email.com", EnrollmentDate = new DateTime(2023, 3, 15) },
                new Student { Id = 5, Name = "Edward Thompson", Email = "edward.thompson@email.com", EnrollmentDate = new DateTime(2022, 9, 1) },
                new Student { Id = 6, Name = "Fiona Garcia", Email = "fiona.garcia@email.com", EnrollmentDate = new DateTime(2023, 5, 20) },
                new Student { Id = 7, Name = "George Martinez", Email = "george.martinez@email.com", EnrollmentDate = new DateTime(2022, 9, 1) },
                new Student { Id = 8, Name = "Hannah Lee", Email = "hannah.lee@email.com", EnrollmentDate = new DateTime(2023, 7, 25) }
            };
            modelBuilder.Entity<Student>().HasData(students);

            // Seed Student-Course relationships (many-to-many)
            var studentCourses = new[]
            {
                // Alice is taking Calculus, Quantum Mechanics, and Programming
                new StudentCourse { StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2022, 9, 5) },
                new StudentCourse { StudentId = 1, CourseId = 3, EnrollmentDate = new DateTime(2022, 9, 5) },
                new StudentCourse { StudentId = 1, CourseId = 5, EnrollmentDate = new DateTime(2022, 9, 5) },

                // Bob is taking Linear Algebra and Data Structures
                new StudentCourse { StudentId = 2, CourseId = 2, EnrollmentDate = new DateTime(2023, 1, 12) },
                new StudentCourse { StudentId = 2, CourseId = 6, EnrollmentDate = new DateTime(2023, 1, 12) },

                // Charlie is taking Physics courses
                new StudentCourse { StudentId = 3, CourseId = 3, EnrollmentDate = new DateTime(2022, 9, 7) },
                new StudentCourse { StudentId = 3, CourseId = 4, EnrollmentDate = new DateTime(2022, 9, 7) },

                // Diana is taking Chemistry and Biology
                new StudentCourse { StudentId = 4, CourseId = 7, EnrollmentDate = new DateTime(2023, 3, 18) },
                new StudentCourse { StudentId = 4, CourseId = 8, EnrollmentDate = new DateTime(2023, 3, 18) },

                // Edward is taking Programming courses
                new StudentCourse { StudentId = 5, CourseId = 5, EnrollmentDate = new DateTime(2022, 9, 10) },
                new StudentCourse { StudentId = 5, CourseId = 6, EnrollmentDate = new DateTime(2022, 9, 10) },

                // Fiona is taking Math courses
                new StudentCourse { StudentId = 6, CourseId = 1, EnrollmentDate = new DateTime(2023, 5, 22) },
                new StudentCourse { StudentId = 6, CourseId = 2, EnrollmentDate = new DateTime(2023, 5, 22) },

                // George is taking mixed courses
                new StudentCourse { StudentId = 7, CourseId = 1, EnrollmentDate = new DateTime(2022, 9, 15) },
                new StudentCourse { StudentId = 7, CourseId = 5, EnrollmentDate = new DateTime(2022, 9, 15) },
                new StudentCourse { StudentId = 7, CourseId = 8, EnrollmentDate = new DateTime(2022, 9, 15) },

                // Hannah is taking science courses
                new StudentCourse { StudentId = 8, CourseId = 3, EnrollmentDate = new DateTime(2023, 7, 28) },
                new StudentCourse { StudentId = 8, CourseId = 7, EnrollmentDate = new DateTime(2023, 7, 28) }
            };

            modelBuilder.Entity<StudentCourse>().HasData(studentCourses);
        }
    }
}
