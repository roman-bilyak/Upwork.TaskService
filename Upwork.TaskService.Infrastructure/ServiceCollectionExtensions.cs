using Microsoft.Extensions.DependencyInjection;
using Upwork.TaskService.Tasks;

namespace Upwork.TaskService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IRepository<TaskEntity>, TaskStoredProcedureRepository>();

        return services;
    }
}