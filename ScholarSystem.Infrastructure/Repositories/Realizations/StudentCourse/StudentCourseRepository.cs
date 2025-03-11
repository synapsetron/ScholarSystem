using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Persistence;
using ScholarSystem.Infrastructure.Repositories.Interfaces.StudentCourses;
using ScholarSystem.Infrastructure.Repositories.Realizations.Base;

namespace ScholarSystem.Infrastructure.Repositories.Realizations
{
    public class StudentCourseRepository : RepositoryBase<StudentCourse>, IStudentCourseRepository
    {
        public StudentCourseRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
