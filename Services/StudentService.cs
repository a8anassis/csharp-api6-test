using AutoMapper;
using SchoolApp.Data;
using SchoolApp.Repositories;
using Serilog;

namespace SchoolApp.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = new LoggerFactory().AddSerilog().CreateLogger<StudentService>();
        }



        public async Task<bool> DeleteStudentAsync(int id)
        {
            bool studentDeleted = false;

            try
            {
                studentDeleted = await _unitOfWork.StudentRepository.DeleteAsync(id);
                _logger.LogInformation("{Message}", "Student with id: " + id + " deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
            return studentDeleted;
        }

        public async Task<IEnumerable<User>> GetAllStudentsAsync()
        {
            List<User> usersStudents = new List<User>();
            try
            {
                usersStudents = await _unitOfWork.StudentRepository.GetAllUsersStudentsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
            return usersStudents;
        }

        public async Task<Student?> GetStudentAsync(int id)
        {
            Student? student;

            try
            {
                student = await _unitOfWork.StudentRepository.GetAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
            return student;
        }

        public async Task<int> GetStudentCountAsync()
        {
            int count;

            try
            {
                count = await _unitOfWork.StudentRepository.GetCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
            return count;
        }

        public async Task<List<Course>> GetStudentCoursesAsync(int id)
        {
            List<Course> courses;

            try
            {
                courses = await _unitOfWork.StudentRepository.GetStudentCoursesAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
            return courses;
        }
    }
}
