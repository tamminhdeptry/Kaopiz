using AutoMapper;
using CET.Domain.Dtos;
using CET.Repository.Entity.Users;

namespace CET.Service.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserEntity, UserDto>().ReverseMap();
        }
    }
}
