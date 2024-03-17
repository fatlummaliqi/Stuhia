using Microsoft.Extensions.DependencyInjection;
using Stuhia.Configurations;
using Stuhia.Context;
using Stuhia.Models.Exceptions;

namespace Stuhia.Core;

internal class InternalEventContext : EventContext
{
    private bool _isConstructed;
    private bool _loggingEnabled;
    private bool _silentFailuresEnabled;

    private readonly IDictionary<string, Type> _eventHandlers = new Dictionary<string, Type>();

    public override bool LoggingEnabled
    {
        get
        {
            if (!_isConstructed)
            {
                throw new InvalidOperationException("Event Context is not constructed.");
            }

            return _loggingEnabled;
        }
    }

    public override bool SilentFailuresEnabled
    {
        get
        {
            if (!_isConstructed)
            {
                throw new InvalidOperationException("Event Context is not constructed.");
            }

            return _silentFailuresEnabled;
        }
    }

    public override void Construct(StuhiaConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        foreach(var assembly in configuration.AssembliesToScan)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    foreach (var @interface in type.GetInterfaces())
                    {
                        if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                        {
                            _eventHandlers[@interface.GetGenericArguments().First().FullName] = type;
                        }
                    }
                }
            }
        }

        _silentFailuresEnabled = configuration.SilentFailures;
        _loggingEnabled = configuration.EnableLogging;

        _isConstructed = true;
    }

    public override IEventHandler<TEvent> ResolveHandler<TEvent>(IServiceProvider serviceProvider)
    {
        if (!_isConstructed)
        {
            throw new NotConstructedEventContextException();
        }

        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        var eventTypeFullName = typeof(TEvent).FullName;
        var handlerTypePresent = _eventHandlers.ContainsKey(eventTypeFullName);

        if (!handlerTypePresent)
        {
            throw new UnSupportedHandlerException(typeof(TEvent).Name);
        }

        var handlerType = _eventHandlers[eventTypeFullName];
        var handlerDependencies = handlerType.GetConstructors().First().GetParameters().Select(parameter => parameter.ParameterType);

        var handlerDependencyInstances = new List<object>();

        foreach (var handlerDependency in handlerDependencies)
        {
            handlerDependencyInstances.Add(serviceProvider.GetRequiredService(handlerDependency));
        }

        var handlerInstance = Activator.CreateInstance(handlerType, handlerDependencies.ToArray());

        return handlerInstance as IEventHandler<TEvent>;
    }
}
