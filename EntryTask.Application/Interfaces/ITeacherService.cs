using EntryTask.Application.DTO.Teacher;

namespace EntryTask.Application.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDTO>> GetAllTeachersAsync();
        Task<TeacherDTO?> GetTeacherByIdAsync(int id);
        Task<TeacherDTO> CreateTeacherAsync(TeacherDTO teacherDto);
        Task<TeacherDTO> UpdateTeacherAsync(int id, TeacherDTO teacherDto);
        Task<bool> DeleteTeacherAsync(int id);
    }
}
