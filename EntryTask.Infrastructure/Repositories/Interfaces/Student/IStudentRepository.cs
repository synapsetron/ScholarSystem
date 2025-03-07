using EntryTask.Domain.Entities;
using EntryTask.Infrastructure.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryTask.Infrastructure.Repositories.Interfaces.Students
{
    public interface IStudentRepository : IRepositoryBase<Student>
    {
    }
}
