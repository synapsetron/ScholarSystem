using EntryTask.Infrastructure.Repositories.Interfaces.Courses;
using EntryTask.Infrastructure.Repositories.Interfaces.Students;
using EntryTask.Infrastructure.Repositories.Interfaces.Teachers;
using System.Transactions;

namespace EntryTask.Infrastructure.Repositories.Interfaces.Base
{
    public interface IRepositoryWrapper
    {
        ICourseRepository CourseRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        IStudentRepository StudentRepository { get; }
        public int SaveChanges();
        public Task<int> SaveChangesAsync();
        public TransactionScope BeginTransaction();
    }
}
