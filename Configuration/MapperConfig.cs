using AutoMapper;
using SchoolApp.Data;
using SchoolApp.DTO;


namespace UsersStudentsMVCApp.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserReadOnlyDTO>().ReverseMap();
            CreateMap<User, UserTeacherReadOnlyDTO>().ReverseMap();
            CreateMap<User, UserTeacherReadOnlyDTO>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.Teacher!.PhoneNumber}"))
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => $"{src.Teacher!.Institution}")).ReverseMap();

            CreateMap<User, TeacherSignupDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.Username}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.Email}"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => $"{src.Password}"))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => $"{src.Firstname}"))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => $"{src.Lastname}"))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => $"{src.UserRole}"))
                .ReverseMap();

            CreateMap<Teacher, TeacherSignupDTO>()
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => $"{src.Institution}"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.PhoneNumber}"))
                .ReverseMap();

            CreateMap<User, UserTeacherReadOnlyDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.Username}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.Email}"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => $"{src.Password}"))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => $"{src.Firstname}"))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => $"{src.Lastname}"))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => $"{src.UserRole}"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => $"{src.Teacher!.PhoneNumber}"))
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => $"{src.Teacher!.Institution}"))
                .ReverseMap();
        }
    }
}
