using Microsoft.Extensions.DependencyInjection;
using Stuhia.Configurations;
using Stuhia.Context;
using Stuhia.Core;

namespace Stuhia.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationEvents(this IServiceCollection services, Action<StuhiaConfiguration> config)
    {
        var configuration = new StuhiaConfiguration();

        config.Invoke(configuration);

        EventContext.Current.Construct(configuration);

        services.AddSingleton<IEventPublisher, InternalEventPublisher>();

        return services;
    }
}
