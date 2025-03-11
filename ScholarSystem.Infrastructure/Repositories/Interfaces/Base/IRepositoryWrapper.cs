using ScholarSystem.Infrastructure.Repositories.Interfaces.Courses;
using ScholarSystem.Infrastructure.Repositories.Interfaces.StudentCourses;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Students;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Teachers;
using System.Transactions;

namespace ScholarSystem.Infrastructure.Repositories.Interfaces.Base
{
    public interface IRepositoryWrapper
    {
        ICourseRepository CourseRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        IStudentRepository StudentRepository { get; }
        IStudentCourseRepository StudentCourseRepository { get; }
        public int SaveChanges();
        public Task<int> SaveChangesAsync();
        public TransactionScope BeginTransaction();
    }
}
