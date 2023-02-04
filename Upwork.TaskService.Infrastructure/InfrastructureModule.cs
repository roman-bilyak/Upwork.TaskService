using Microsoft.Extensions.DependencyInjection;
using Upwork.TaskService.Tasks;

namespace Upwork.TaskService;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
    {
        services.AddScoped<IRepository<TaskEntity>, TaskStoredProcedureRepository>();

        return services;
    }
}