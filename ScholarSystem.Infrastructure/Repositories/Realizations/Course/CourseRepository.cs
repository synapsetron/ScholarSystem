using ScholarSystem.Infrastructure.Persistence;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Courses;
using ScholarSystem.Infrastructure.Repositories.Realizations.Base;
using courseEntity = ScholarSystem.Domain.Entities.Course;
namespace ScholarSystem.Infrastructure.Repositories.Realizations.Course
{
    public class CourseRepository : RepositoryBase<courseEntity>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }
    }
}
