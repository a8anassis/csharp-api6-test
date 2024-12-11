using SchoolApp.Data;
using SchoolApp.DTO;

namespace SchoolApp.Services
{
    public interface ITeacherService
    {
        Task SignUpUserAsync(TeacherSignupDTO request);
        Task<List<User>> GetAllUsersTeachersAsync();
        Task<List<User>> GetAllUsersTeachersAsync(int pageNumber, int pageSize);
        Task<int> GetTeacherCountAsync();
        Task<User?> GetTeacherByUsernameAsync(string username);
    }
}
