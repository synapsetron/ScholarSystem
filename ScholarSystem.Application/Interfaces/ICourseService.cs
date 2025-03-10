using ScholarSystem.Application.DTO.Course;

namespace ScholarSystem.Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCoursesAsync();
        Task<CourseDTO?> GetCourseByIdAsync(int id);
        Task<CourseDTO> CreateCourseAsync(CourseDTO courseDto);
        Task<CourseDTO> UpdateCourseAsync(int id, CourseDTO courseDto);
        Task<bool> DeleteCourseAsync(int id);
    }
}
