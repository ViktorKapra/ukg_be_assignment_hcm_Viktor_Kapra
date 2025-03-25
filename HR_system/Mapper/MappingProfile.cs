using AutoMapper;
using HR_system.BLogic.DTOs;
using HR_system.Models;

namespace HR_system.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginViewModel, UserCredentialsDTO>();
            CreateMap<RegisterViewModel, UserDTO>();
            // CreateMap<Source, Destination>();
        }
    }
}
