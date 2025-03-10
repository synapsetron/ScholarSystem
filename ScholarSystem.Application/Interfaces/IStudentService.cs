using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudentsAsync();
        Task<StudentDTO?> GetStudentByIdAsync(int id);
        Task<StudentDTO> CreateStudentAsync(StudentDTO studentDto);
        Task<StudentDTO> UpdateStudentAsync(int id, StudentDTO studentDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
