using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Upwork.TaskService;

internal abstract class BaseIntegrationTests
{
    protected IServiceProvider ServiceProvider { get; set; }

    [SetUp]
    public void Init()
    {
        IServiceCollection services = new ServiceCollection()
            .AddApplicationModule();

        services.AddTransient<IConfiguration>(x =>
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();
        });

        ConfigureServices(services);

        ServiceProvider = services.BuildServiceProvider();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {

    }

    [TearDown]
    public void Cleanup()
    {

    }

    protected T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    protected T GetRequiredService<T>()
        where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}