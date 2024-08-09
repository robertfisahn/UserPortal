using AutoMapper;
using UserPortal.Entities;
using UserPortal.Models.Dtos;

namespace UserPortal
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDto, User>();
        }
    }
}
