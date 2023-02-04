using Microsoft.Extensions.DependencyInjection;
using Upwork.TaskService.Tasks;

namespace Upwork.TaskService;

public static class DomainModule
{
    public static IServiceCollection AddDomainModule(this IServiceCollection services)
    {
        services.AddScoped<ITaskManager, TaskManager>();

        return services;
    }
}