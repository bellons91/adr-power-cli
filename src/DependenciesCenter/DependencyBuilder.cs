using FileSystemAccess;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DependenciesCenter;

public static class DependencyBuilder
{
    public static IServiceCollection AddCliDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IConfigurationService, ConfigurationService>();

        return services;
    }

}
