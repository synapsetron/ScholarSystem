using AutoMapper;
using EntryTask.Application.DTO.Course;
using EntryTask.Domain.Entities;

namespace EntryTask.Application.Mapping
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDTO>()
          .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name));

            CreateMap<CreateCourseDTO, Course>().ReverseMap();
        }
    }
}
