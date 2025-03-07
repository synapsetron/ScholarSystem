using EntryTask.Infrastructure.Persistence;
using EntryTask.Infrastructure.Repositories.Interfaces.Courses;
using EntryTask.Infrastructure.Repositories.Realizations.Base;
using courseEntity = EntryTask.Domain.Entities.Course;
namespace EntryTask.Infrastructure.Repositories.Realizations.Course
{
    public class CourseRepository : RepositoryBase<courseEntity>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }
    }
}
