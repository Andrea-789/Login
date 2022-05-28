using AutoMapper;
using GestUser.Dtos;
using GestUser.Models;

namespace GestUser.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UsersDto>();
            
        }
    
    }
}
