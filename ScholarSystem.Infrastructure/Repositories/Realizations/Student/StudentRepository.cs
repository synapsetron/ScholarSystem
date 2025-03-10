using ScholarSystem.Infrastructure.Persistence;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Students;
using ScholarSystem.Infrastructure.Repositories.Realizations.Base;
using studentEntity = ScholarSystem.Domain.Entities.Student;

namespace ScholarSystem.Infrastructure.Repositories.Realizations.Student
{
    public class StudentRepository : RepositoryBase<studentEntity>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }       
    }
}
