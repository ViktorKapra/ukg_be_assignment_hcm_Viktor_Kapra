using HR_sustem.BLogic.Services.Authentication;
using HR_sustem.BLogic.Services.Interfaces;

namespace HR_system.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBussinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
