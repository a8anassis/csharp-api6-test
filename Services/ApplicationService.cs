using AutoMapper;
using SchoolApp.Repositories;

namespace SchoolApp.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
      
       
        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;         
        }


        public UserService UserService => new(_unitOfWork, _mapper);
        public TeacherService TeacherService => new(_unitOfWork, _mapper);
        public StudentService StudentService => new(_unitOfWork, _mapper);
    }
}
