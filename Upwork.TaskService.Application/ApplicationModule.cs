using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Upwork.TaskService.Tasks;

namespace Upwork.TaskService;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddDomainModule();
        services.AddInfrastructureModule();
        services.AddDataValidation();
        services.AddAutoMapper(typeof(ApplicationModule));

        services.AddMediatR(typeof(ApplicationModule));

        return services;
    }

    internal static IServiceCollection AddDataValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateTaskDto>, CreateTaskDtoValidator>();
        services.AddScoped<IValidator<UpdateTaskDto>, UpdateTaskDtoValidator>();

        return services;
    }
}