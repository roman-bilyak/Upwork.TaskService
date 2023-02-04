using Microsoft.Extensions.DependencyInjection;
using Upwork.TaskService.Tasks;

namespace Upwork.TaskService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ITaskManager, TaskManager>();

        return services;
    }
}