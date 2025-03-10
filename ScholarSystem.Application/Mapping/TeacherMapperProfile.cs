using AutoMapper;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Domain.Entities;

namespace ScholarSystem.Application.Mapping
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherDTO>()
                .ForMember(dest => dest.CourseTitles, opt => opt.MapFrom(src => src.Courses.Select(c => c.Title)));

            CreateMap<CreateTeacherDTO, Teacher>().ReverseMap();
            CreateMap<UpdateTeacherDTO, Teacher>().ReverseMap();
        }
    }
}
