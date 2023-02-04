using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Upwork.TaskService;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddDomainModule();
        services.AddInfrastructureModule();

        services.AddMediatR(typeof(ApplicationModule));

        return services;
    }
}