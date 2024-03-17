using Moq;
using Stuhia.Configurations;
using Stuhia.Context;
using Stuhia.Core;
using Stuhia.Tests.Unit.Helpers.Events;
using System.Reflection;
using Xunit;

namespace Stuhia.Tests.Unit.Tests;

public class EventPublisherTests
{
    private readonly StuhiaConfiguration _configuration = new()
    {
        EnableLogging = false,
        SilentFailures = false,
        AssembliesToScan = [ Assembly.GetExecutingAssembly() ]
    };

    [Fact]
    public async Task PublishAsync_Should_Complete_Succefully()
    {
        //Arrange
        EventContext.Current.Construct(_configuration);
        var serviceProvider = Mock.Of<IServiceProvider>();  
        var eventPublisher = new InternalEventPublisher(serviceProvider, logger: null);

        //Act
        await eventPublisher.PublishAsync(new PeekabooEvent());

        //Assert
        Assert.True(true);
    }
}
