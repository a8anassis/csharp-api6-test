using SchoolApp.Core.Enums;
using SchoolApp.Core.Filters;
using SchoolApp.Data;
using SchoolApp.DTO;

namespace SchoolApp.Services
{
    public interface IUserService
    {
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<List<User>> GetAllUsersFiltered(int pageNumber, int pageSize,
            UserFiltersDTO userFiltersDTO);
        //string CreateUserToken(int userId, string username, string email, UserRole userRole,
        //    string appSecurityKey);
        Task<User?> GetUserByIdAsync(int id);
    }
}
