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
                .ForMember(dest => dest.StudentIds, opt => opt.MapFrom(src => src.Students.Select(s => s.Id)));

            CreateMap<CreateCourseDTO, Course>();
        }
    }
}
