using AutoMapper;
using ScholarSystem.Application.DTO.Course;
using ScholarSystem.Domain.Entities;

namespace ScholarSystem.Application.Mapping
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
