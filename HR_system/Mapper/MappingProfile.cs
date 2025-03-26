using AutoMapper;
using Data.Account;
using HR_system.BLogic.DTOs;
using HR_system.BLogic.Templates;
using HR_system.Models;

namespace HR_system.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginViewModel, UserCredentialsDTO>();
            CreateMap<RegisterViewModel, UserDTO>();
            CreateMap<UserDTO, ApplicationUser>().ForMember(dest => dest.Id, opt => opt.DoNotAllowNull());
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<UserDTO, UserViewModel>().ReverseMap();
            CreateMap<UserFilterDTO, Query<ApplicationUser>>()
              .ConstructUsing(src => new Query<ApplicationUser>
              {
                  Expression = x => ((string.IsNullOrEmpty(src.Email) || x.Email == src.Email)
                                   && (string.IsNullOrEmpty(src.FirstName) || x.FirstName == src.FirstName)
                                   && (string.IsNullOrEmpty(src.LastName) || x.LastName == src.LastName)
                                   && (string.IsNullOrEmpty(src.JobTitle) || x.JobTitle == src.JobTitle)
                                   && (string.IsNullOrEmpty(src.Department) || x.Department == src.Department)
                                   && (src.UpperSalaryBound == decimal.MaxValue || x.Salary <= src.UpperSalaryBound)
                                   && (src.LowerSalaryBound == decimal.MinValue || x.Salary >= src.LowerSalaryBound)),
                  Limit = src.Limit,
                  Offset = src.Offset
              });
            // CreateMap<Source, Destination>();
        }
    }
}
