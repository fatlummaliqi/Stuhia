﻿using Moq;
using Stuhia.Configurations;
using Stuhia.Context;
using Stuhia.Models.Exceptions;
using Stuhia.Tests.Unit.Helpers.Events;
using Stuhia.Tests.Unit.Helpers.Events.Handlers;
using System.Reflection;
using Xunit;

namespace Stuhia.Tests.Unit.Tests;

public class EventContextTests
{
    private static readonly StuhiaConfiguration _configuration = new()
    {
        SilentFailures = true,
        AssembliesToScan = [Assembly.GetExecutingAssembly()]
    };

    [Fact]
    public void Construct_Should_Not_Throw()
    {
        //Arrange 
        var configuration = _configuration;

        //Act
        EventContext.Current.Construct(configuration);

        //Assert
        Assert.True(true);
    }

    [Fact]
    public void Construct_Should_Throw_ArgumentNullException()
    {
        //Act
        static void Act() => EventContext.Current.Construct(configuration: null);

        //Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void ResolveHandler_Should_Throw_NotConstructedEventContextException()
    {
        //Act
        static void Act() => EventContext.Current.ResolveHandler<PeekabooEvent>(serviceProvider: null);

        //Assert
        Assert.Throws<NotConstructedEventContextException>(Act);
    }

    [Fact]
    public void ResolveHandler_Should_Throw_ArgumentNullException()
    {
        //Arrange 
        EventContext.Current.Construct(_configuration);

        //Act
        static void Act() => EventContext.Current.ResolveHandler<PeekabooEvent>(serviceProvider: null);

        //Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void ResolveHandler_Should_Return_Expected_Handler()
    {
        //Arrange
        var serviceProvider = Mock.Of<IServiceProvider>();

        EventContext.Current.Construct(_configuration);

        //Act
        var handler = EventContext.Current.ResolveHandler<PeekabooEvent>(serviceProvider);

        //Assert
        Assert.NotNull(handler);
        Assert.Equal(typeof(PeekabooEventHandler), handler.GetType());
    }

    [Fact]
    public void ResolveHandler_Should_Throw_UnSupportedHandlerException()
    {
        //Arrange 
        var serviceProvider = Mock.Of<IServiceProvider>();

        EventContext.Current.Construct(_configuration);

        //Act
        void Act() => EventContext.Current.ResolveHandler<PikachuEvent>(serviceProvider);

        //Assert
        Assert.Throws<UnSupportedHandlerException>(Act);
    }
}
