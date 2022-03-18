using AutoMapper;
using Sas.Domain.Users;
using Sas.Identity.Service.Models;

namespace Sas.Identity.Service.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>();
        }
    }
}
