﻿using ScholarSystem.Infrastructure.Persistence;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Courses;
using ScholarSystem.Infrastructure.Repositories.Interfaces.StudentCourses;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Students;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Teachers;
using ScholarSystem.Infrastructure.Repositories.Realizations.Course;
using ScholarSystem.Infrastructure.Repositories.Realizations.Student;
using ScholarSystem.Infrastructure.Repositories.Realizations.Teacher;
using System.Transactions;

namespace ScholarSystem.Infrastructure.Repositories.Realizations.Base
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _context;
        private IStudentRepository? _studentRepository;
        private ICourseRepository? _courseRepository;
        private ITeacherRepository? _teacherRepository;
        private IStudentCourseRepository? _studentCourseRepository;

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

        public IStudentCourseRepository StudentCourseRepository
        {
            get
            {
                if (_studentCourseRepository == null)
                {
                    _studentCourseRepository = new StudentCourseRepository(_context);
                }
                return _studentCourseRepository;
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
