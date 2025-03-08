using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryTask.Application.DTO.Course
{
    public  class CreateCourseDTO
    {
        public string Title { get; set; } = string.Empty;
        public int TeacherId { get; set; }
    }
}
