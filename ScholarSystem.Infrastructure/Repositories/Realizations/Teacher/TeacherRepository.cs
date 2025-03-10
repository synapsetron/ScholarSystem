using ScholarSystem.Infrastructure.Persistence;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Teachers;
using ScholarSystem.Infrastructure.Repositories.Realizations.Base;
using teacherEntity = ScholarSystem.Domain.Entities.Teacher;
namespace ScholarSystem.Infrastructure.Repositories.Realizations.Teacher
{
    public class TeacherRepository : RepositoryBase<teacherEntity>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context) { }
    }
}
