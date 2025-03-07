using EntryTask.Infrastructure.Persistence;
using EntryTask.Infrastructure.Repositories.Interfaces.Base;
using EntryTask.Infrastructure.Repositories.Interfaces.Courses;
using EntryTask.Infrastructure.Repositories.Interfaces.Students;
using EntryTask.Infrastructure.Repositories.Interfaces.Teachers;
using EntryTask.Infrastructure.Repositories.Realizations.Course;
using EntryTask.Infrastructure.Repositories.Realizations.Student;
using EntryTask.Infrastructure.Repositories.Realizations.Teacher;
using System.Transactions;

namespace EntryTask.Infrastructure.Repositories.Realizations.Base
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _context;
        private IStudentRepository? _studentRepository;
        private ICourseRepository? _courseRepository;
        private ITeacherRepository? _teacherRepository;

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICourseRepository CourseRepository
        {
            get
            {
                if (_courseRepository == null)
                {
                    _courseRepository = new CourseRepository(_context);
                }
                return _courseRepository;
            }
        }

        public ITeacherRepository TeacherRepository
        {
            get
            {
                if (_teacherRepository == null)
                {
                    _teacherRepository = new TeacherRepository(_context);
                }
                return _teacherRepository;
            }
        }

        public IStudentRepository StudentRepository
        {
            get
            {
                if (_studentRepository == null)
                {
                    _studentRepository = new StudentRepository(_context);
                }
                return _studentRepository;
            }
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public TransactionScope BeginTransaction()
        {
            return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
