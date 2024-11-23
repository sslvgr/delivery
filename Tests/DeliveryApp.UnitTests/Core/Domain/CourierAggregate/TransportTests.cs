using System.Collections.Generic;
using DeliveryApp.Core.Domain.CourierAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.CourierAggregate;

public class TransportTests
{
    public static IEnumerable<object[]> GetTransports()
    {
        yield return [Transport.Pedestrian, "pedestrian"];
        yield return [Transport.Bicycle, "bicycle"];
        yield return [Transport.Car, "car"];
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void ReturnCorrectIdAndName(Transport transport, string name)
    {
        //Arrange

        //Act

        //Assert
        transport.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(1, "pedestrian")]
    [InlineData(2, "bicycle")]
    [InlineData(3, "car")]
    public void CanBeFoundById(int id, string name)
    {
        //Arrange

        //Act
        var status = Transport.FromId(id).Value;

        //Assert
        status.Id.Should().Be(id);
        status.Name.Should().Be(name);
    }

    [Theory]
    [InlineData("pedestrian")]
    [InlineData("bicycle")]
    [InlineData("car")]
    public void CanBeFoundByName(string name)
    {
        //Arrange

        //Act
        var result = Transport.FromName(name);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(name);
    }

    [Fact]
    public void ReturnErrorWhenTransportNotFoundById()
    {
        //Arrange
        var id = -1;

        //Act
        var result = Transport.FromId(id);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void ReturnErrorWhenStatusNotFoundByName()
    {
        //Arrange
        var name = "not-existent-name";

        //Act
        var result = Transport.FromName(name);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void ReturnListOfStatuses()
    {
        //Arrange

        //Act
        var allStatuses = Transport.List();

        //Assert
        allStatuses.Should().NotBeEmpty();
    }
}