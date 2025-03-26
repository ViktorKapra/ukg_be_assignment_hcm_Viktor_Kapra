using HR_sustem.BLogic.Services.Authentication;
using HR_sustem.BLogic.Services.Interfaces;
using HR_system.BLogic.Services.Interfaces;
using HR_system.BLogic.Services.User;

namespace HR_system.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBussinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
