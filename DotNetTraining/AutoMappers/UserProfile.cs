using AutoMapper;
using DotNetTraining.Domains.Dtos;
using DotNetTraining.Domains.Models;
using DotNetTraining.Domains.Entities;
using Common.Application.Models;

namespace DotNetTraining.AutoMappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            CreateMap<User, AuthenticatedUserModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Roles))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
        }
    }
}
