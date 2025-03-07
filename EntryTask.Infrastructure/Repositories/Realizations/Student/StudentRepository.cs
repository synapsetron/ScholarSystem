using EntryTask.Infrastructure.Persistence;
using EntryTask.Infrastructure.Repositories.Interfaces.Students;
using EntryTask.Infrastructure.Repositories.Realizations.Base;
using studentEntity = EntryTask.Domain.Entities.Student;

namespace EntryTask.Infrastructure.Repositories.Realizations.Student
{
    public class StudentRepository : RepositoryBase<studentEntity>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }       
    }
}
