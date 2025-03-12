using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScholarSystem.Domain.Entities;

namespace ScholarSystem.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedUsers(modelBuilder);
            SeedTeachers(modelBuilder);
            SeedCourses(modelBuilder);
            SeedStudents(modelBuilder);
            SeedStudentCourses(modelBuilder);
        }

        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var users = new[]
            {
                CreateUser("1", "admin", "admin@email.com", "System", "Admin", "Other", "Admin123!", passwordHasher),
                CreateUser("2", "teacher1", "teacher1@email.com", "John", "Doe", "Male", "Teacher123!", passwordHasher),
                CreateUser("3", "student1", "student1@email.com", "Alice", "Brown", "Female", "Student123!", passwordHasher)
            };

            modelBuilder.Entity<ApplicationUser>().HasData(users);
        }

        private static ApplicationUser CreateUser(string id, string userName, string email, string firstName, string lastName, string gender, string password, PasswordHasher<ApplicationUser> hasher)
        {
            var user = new ApplicationUser
            {
                Id = id,
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null!, password)
            };

            return user;
        }

        private static void SeedTeachers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>().HasData(new[]
            {
                new Teacher { Id = 1, Name = "John Smith", Email = "john.smith@email.com" },
                new Teacher { Id = 2, Name = "Sarah Johnson", Email = "sarah.johnson@email.com" },
                new Teacher { Id = 3, Name = "Michael Brown", Email = "michael.brown@email.com" },
                new Teacher { Id = 4, Name = "Emily Davis", Email = "emily.davis@email.com" },
                new Teacher { Id = 5, Name = "David Wilson", Email = "david.wilson@email.com" }
            });
        }

        private static void SeedCourses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(new[]
            {
                new Course { Id = 1, Title = "Calculus I", Description = "Basic introduction to calculus.", TeacherId = 1, Credits = 3 },
                new Course { Id = 2, Title = "Linear Algebra", Description = "Study of vectors, matrices, and linear transformations.", TeacherId = 1, Credits = 4 },
                new Course { Id = 3, Title = "Quantum Mechanics", Description = "Introduction to quantum physics.", TeacherId = 2, Credits = 3 },
                new Course { Id = 4, Title = "Particle Physics", Description = "Exploring subatomic particles and interactions.", TeacherId = 2, Credits = 4 },
                new Course { Id = 5, Title = "Programming Fundamentals", Description = "Learn basic programming concepts.", TeacherId = 3, Credits = 3 },
                new Course { Id = 6, Title = "Data Structures", Description = "Introduction to common data structures.", TeacherId = 3, Credits = 4 },
                new Course { Id = 7, Title = "Organic Chemistry", Description = "Study of organic compounds and reactions.", TeacherId = 4, Credits = 3 },
                new Course { Id = 8, Title = "Molecular Biology", Description = "Exploring the molecular mechanisms of life.", TeacherId = 5, Credits = 3 }
            });
        }

        private static void SeedStudents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new[]
            {
                new Student { Id = 1, Name = "Alice Brown", Email = "alice.brown@email.com", EnrollmentDate = new DateTime(2022, 9, 1) },
                new Student { Id = 2, Name = "Bob Wilson", Email = "bob.wilson@email.com", EnrollmentDate = new DateTime(2023, 1, 10) },
                new Student { Id = 3, Name = "Charlie Davis", Email = "charlie.davis@email.com", EnrollmentDate = new DateTime(2022, 9, 1) },
                new Student { Id = 4, Name = "Diana Miller", Email = "diana.miller@email.com", EnrollmentDate = new DateTime(2023, 3, 15) },
                new Student { Id = 5, Name = "Edward Thompson", Email = "edward.thompson@email.com", EnrollmentDate = new DateTime(2022, 9, 1) },
                new Student { Id = 6, Name = "Fiona Garcia", Email = "fiona.garcia@email.com", EnrollmentDate = new DateTime(2023, 5, 20) },
                new Student { Id = 7, Name = "George Martinez", Email = "george.martinez@email.com", EnrollmentDate = new DateTime(2022, 9, 1) },
                new Student { Id = 8, Name = "Hannah Lee", Email = "hannah.lee@email.com", EnrollmentDate = new DateTime(2023, 7, 25) }
            });
        }

        private static void SeedStudentCourses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>().HasData(new[]
            {
                new StudentCourse { StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2022, 9, 5) },
                new StudentCourse { StudentId = 1, CourseId = 3, EnrollmentDate = new DateTime(2022, 9, 5) },
                new StudentCourse { StudentId = 1, CourseId = 5, EnrollmentDate = new DateTime(2022, 9, 5) },

                new StudentCourse { StudentId = 2, CourseId = 2, EnrollmentDate = new DateTime(2023, 1, 12) },
                new StudentCourse { StudentId = 2, CourseId = 6, EnrollmentDate = new DateTime(2023, 1, 12) },

                new StudentCourse { StudentId = 3, CourseId = 3, EnrollmentDate = new DateTime(2022, 9, 7) },
                new StudentCourse { StudentId = 3, CourseId = 4, EnrollmentDate = new DateTime(2022, 9, 7) },

                new StudentCourse { StudentId = 4, CourseId = 7, EnrollmentDate = new DateTime(2023, 3, 18) },
                new StudentCourse { StudentId = 4, CourseId = 8, EnrollmentDate = new DateTime(2023, 3, 18) },

                new StudentCourse { StudentId = 5, CourseId = 5, EnrollmentDate = new DateTime(2022, 9, 10) },
                new StudentCourse { StudentId = 5, CourseId = 6, EnrollmentDate = new DateTime(2022, 9, 10) },

                new StudentCourse { StudentId = 6, CourseId = 1, EnrollmentDate = new DateTime(2023, 5, 22) },
                new StudentCourse { StudentId = 6, CourseId = 2, EnrollmentDate = new DateTime(2023, 5, 22) },

                new StudentCourse { StudentId = 7, CourseId = 1, EnrollmentDate = new DateTime(2022, 9, 15) },
                new StudentCourse { StudentId = 7, CourseId = 5, EnrollmentDate = new DateTime(2022, 9, 15) },
                new StudentCourse { StudentId = 7, CourseId = 8, EnrollmentDate = new DateTime(2022, 9, 15) },

                new StudentCourse { StudentId = 8, CourseId = 3, EnrollmentDate = new DateTime(2023, 7, 28) },
                new StudentCourse { StudentId = 8, CourseId = 7, EnrollmentDate = new DateTime(2023, 7, 28) }
            });
        }
    }
}
