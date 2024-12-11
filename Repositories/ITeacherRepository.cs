using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Repositories
{
    public interface ITeacherRepository
    {
        Task<List<Course>> GetTeacherCoursesAsync(int id);
        Task<Teacher?> GetByPhoneNumberAsync(string phoneNumber);
        Task<List<User>> GetAllUsersTeachersAsync();
        Task<List<User>> GetAllUsersTeachersPaginatedAsync(int pageNumber, int pageSize);
        Task<User?> GetUserTeacherByUsernameAsync(string username);
        Task<PaginatedResult<User>> GetPaginatedUsersTeachersAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<User>> GetPaginatedUsersTeachersFilteredAsync(int pageNumber, int pageSize,
            List<Func<User, bool>> predicates);
    }
}
