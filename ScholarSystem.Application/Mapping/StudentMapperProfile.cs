using AutoMapper;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Domain.Entities;

namespace ScholarSystem.Application.Mapping
{
    public class StudentMapperProfile : Profile
    {
        public StudentMapperProfile()
        {
            // Mapping Student → StudentDTO (including enrolled courses)
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.CourseTitles,
                    opt => opt.MapFrom(src => src.StudentCourses.Select(sc => sc.Course.Title)));

            CreateMap<CreateStudentDTO, Student>().ReverseMap();

            CreateMap<UpdateStudentDTO, Student>().ReverseMap();


        }
    }
}