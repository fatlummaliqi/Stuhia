using Stuhia.Configurations;
using System.Reflection;

namespace Stuhia.Context;

internal abstract class EventContext
{
    private static readonly object _lock = new();

    private static EventContext _current;

    public static EventContext Current
    {
        get
        {
            lock (_lock)
            {
                if (_current == null)
                {
                    var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

                    var eventContextType = assemblyTypes.FirstOrDefault(type => !type.IsAbstract && !type.IsInterface && type.IsSubclassOf(typeof(EventContext)));

                    if (eventContextType == null)
                    {
                        throw new ArgumentNullException("Event context is not initialized.");
                    }

                    _current = Activator.CreateInstance(eventContextType) as EventContext;
                }

                return _current;
            }
        }
    }

    public abstract bool SilentFailuresEnabled { get; }

    public abstract void Construct(StuhiaConfiguration configuration);

    public abstract IEventHandler<TEvent> ResolveHandler<TEvent>(IServiceProvider serviceProvider) where TEvent : IApplicationEvent;
}
