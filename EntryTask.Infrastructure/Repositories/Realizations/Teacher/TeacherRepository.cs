using EntryTask.Infrastructure.Persistence;
using EntryTask.Infrastructure.Repositories.Interfaces.Teachers;
using EntryTask.Infrastructure.Repositories.Realizations.Base;
using teacherEntity = EntryTask.Domain.Entities.Teacher;
namespace EntryTask.Infrastructure.Repositories.Realizations.Teacher
{
    public class TeacherRepository : RepositoryBase<teacherEntity>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context) { }
    }
}
