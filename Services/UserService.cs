using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchoolApp.Core.Enums;
using SchoolApp.Core.Filters;
using SchoolApp.Data;
using SchoolApp.DTO;
using SchoolApp.Repositories;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
     

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<UserService>();
        }

        public async Task<List<User>> GetAllUsersFiltered(int pageNumber, int pageSize, 
            UserFiltersDTO userFiltersDTO)
        {
            List<User> users;
            List<Func<User, bool>> predicates = new();

            try
            {
                if (!string.IsNullOrEmpty(userFiltersDTO.Username))
                {
                    predicates.Add(u => u.Username == userFiltersDTO.Username);
                }
                if (!string.IsNullOrEmpty(userFiltersDTO.Email))
                {
                    predicates.Add(u => u.Email == userFiltersDTO.Email);
                }
                if (!string.IsNullOrEmpty(userFiltersDTO.UserRole))
                {
                    predicates.Add(u => u.UserRole.ToString() == userFiltersDTO.UserRole);
                }

                users = await _unitOfWork.UserRepository
                    .GetAllUsersFilteredPaginatedAsync(pageNumber, pageSize, predicates);
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return users;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            User? user = null;

            try
            {
                user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
            } 
            catch (Exception e)
            {
                _logger.LogError("{Message}{Excpetion}", e.Message, e.StackTrace);
            }
            return user;
        }

        public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials)
        {
            User? user;

            try
            {
                user = await _unitOfWork.UserRepository.GetUserAsync(credentials.Username!, credentials.Password!);
                _logger.LogInformation("{Message}", "User: " + user + " found and returned.");   // ToDo toString()
            }
            catch (Exception ex) 
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
            return user;
        }

        public string CreateUserToken(int userId, string? username, string? email, UserRole? userRole,
            string? appSecurityKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey!));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username!),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email!),
                new Claim(ClaimTypes.Role, userRole.ToString()!)
            };

            var jwtSecurityToken = new JwtSecurityToken(null, null, claimsInfo, DateTime.UtcNow,
                DateTime.UtcNow.AddHours(3), signingCredentials);

            // Serialize the token
            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            //return "Bearer " + userToken;
            return userToken;
        }
    }
}
