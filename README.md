![Stuhia](https://raw.githubusercontent.com/fatlummaliqi/Stuhia/main/assets/images/storm.png)

[![Nuget](https://img.shields.io/nuget/v/Stuhia?logo=nuget&style=default)](https://www.nuget.org/packages/Stuhia)
![Nuget](https://img.shields.io/nuget/dt/Stuhia?color=blue&label=Downloads)

# Stuhia

**Stuhia** is a .NET NuGet package designed to simplify event handling and decouple application concerns. With this package, developers can easily publish and subscribe to application-level events, enabling a more flexible and maintainable architecture. By abstracting event handling, **Stuhia** promotes cleaner code and better separation of concerns. Start using **Stuhia** today to enhance the modularity and extensibility of your .NET applications!

## How it works

It's important to understand the purpose of application events. In enterprise level software, where code keeps getting more and more bulky, it is required to keep it clean, readable and foremost modular keeping domain concerns decoupled from each-other. Application events in one way or other help us achieve that, let's take a really basic example. Let's say whenever a domain entity is updated successfully we want to remove its cache, so here we have two different concerns: updating entity and removing its cache. In order to decouple these concerns we could easily achieve that through **Stuhia** in three steps listed below:

1. We need a model which represents this event and consists the required properties for fulfilling the cache removal requirements. Let's name it EntityUpdatedEvent and make sure we have marked it with the `IApplicationEvent` interface.

````csharp
public class EntityUpdatedEvent : IApplicationEvent 
{
	public string EntityId { get; init; }
	public string EntityName { get; init; }
}
````

2. Every time we define an application event we need to define its handler. We can do this by implementing the `IEventHandler<>` interface, where we pass the event type we want to handle as its generic type parameter.

````csharp
public class EntityUpdatedEventHandler(IMemoryCache memoryCache) : IEventHandler<EntityUpdatedEvent>
{
	private readonly IMemoryCache _memoryCache = memoryCache;

	public Task HandleAsync(EntityUpdatedEvent @event, CancellationToken cancellationToken)
	{
		_memoryCache.Remove($"{@event.EventName}_{@event.EventId}");
		
		return Task.CompletedTask;
	}
}
````
In the code snippet above, we can see that we are injecting the `IMemoryCache` in our handler constructor, this is because **Stuhia** supports injecting services that have been registered in DI container. 

> **Note** Stuhia doesn't register event handlers in the DI container, in order to reduce memory allocation, event handler instances are lazy loaded.

3. Last but not least there's only one way to publish this event so all this could work out, and that's it through the `IEventPublisher` interface, so we can inject this in our service constructor and call its `PublishAsync` method whenever the domain logic flow has completed successfully.

````csharp
public async Task<bool> UpdateAsync(int id, DomainEntity entity)
{
	//business logic for updaing domain entity
	
    if (isUpdated) 
    {
	    await _eventPublisher.PublishAsync(new EntityUpdatedEvent
	    {
		    EntityId = id,
		    EntityName = entity.GetType().Name
	    });
    }
}
````

### One more thing

In order to make this thing work, you need to register Stuhia in your `Program.cs`. This can be done by invoking the desired extension method, like shown below.

````csharp
builder.Services.AddApplicationEvents(config =>
{
    config.SilentFailures = true;
    config.RegisterHandlersFromAssembly(Assembly.GetAssembly(typeof(EntityUpdatedEventHandler));
});
````

And by that, you're ready to go. Happy publishing and handling!

