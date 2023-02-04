using Microsoft.Extensions.DependencyInjection;

namespace Upwork.TaskService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddDomainServices();
        services.AddInfrastructureServices();

        return services;
    }
}