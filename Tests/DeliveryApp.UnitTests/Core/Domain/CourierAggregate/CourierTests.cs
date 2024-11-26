using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;
using Primitives;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.CourierAggregate;

public class CourierTests
{
    public static IEnumerable<object[]> GetTransports()
    {
        // Пешеход, заказ X:совпадает, Y: совпадает
        yield return
        [
            Transport.Pedestrian, Location.Create(1, 1).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value
        ];
        yield return
        [
            Transport.Pedestrian, Location.Create(5, 5).Value, Location.Create(5, 5).Value, Location.Create(5, 5).Value
        ];

        // Пешеход, заказ X:совпадает, Y: выше
        yield return
        [
            Transport.Pedestrian, Location.Create(1, 1).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value
        ];
        yield return
        [
            Transport.Pedestrian, Location.Create(1, 1).Value, Location.Create(1, 5).Value, Location.Create(1, 2).Value
        ];

        // Пешеход, заказ X:правее, Y: совпадает
        yield return
        [
            Transport.Pedestrian, Location.Create(2, 2).Value, Location.Create(3, 2).Value, Location.Create(3, 2).Value
        ];
        yield return
        [
            Transport.Pedestrian, Location.Create(5, 5).Value, Location.Create(6, 5).Value, Location.Create(6, 5).Value
        ];

        // Пешеход, заказ X:правее, Y: выше
        yield return
        [
            Transport.Pedestrian, Location.Create(2, 2).Value, Location.Create(3, 3).Value, Location.Create(3, 2).Value
        ];
        yield return
        [
            Transport.Pedestrian, Location.Create(1, 1).Value, Location.Create(5, 5).Value, Location.Create(2, 1).Value
        ];

        // Пешеход, заказ X:совпадает, Y: ниже
        yield return
        [
            Transport.Pedestrian, Location.Create(1, 2).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value
        ];
        yield return
        [
            Transport.Pedestrian, Location.Create(5, 5).Value, Location.Create(5, 1).Value, Location.Create(5, 4).Value
        ];

        // Пешеход, заказ X:левее, Y: совпадает
        yield return
        [
            Transport.Pedestrian, Location.Create(2, 2).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value
        ];
        yield return
        [
            Transport.Pedestrian, Location.Create(5, 5).Value, Location.Create(1, 5).Value, Location.Create(4, 5).Value
        ];

        // Пешеход, заказ X:левее, Y: ниже
        yield return
        [
            Transport.Pedestrian, Location.Create(2, 2).Value, Location.Create(1, 1).Value, Location.Create(1, 2).Value
        ];
        yield return
        [
            Transport.Pedestrian, Location.Create(5, 5).Value, Location.Create(1, 1).Value, Location.Create(4, 5).Value
        ];


        // Велосипедист, заказ X:совпадает, Y: совпадает
        yield return
            [Transport.Bicycle, Location.Create(1, 1).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(5, 5).Value, Location.Create(5, 5).Value];

        // Велосипедист, заказ X:совпадает, Y: выше
        yield return
            [Transport.Bicycle, Location.Create(1, 1).Value, Location.Create(1, 3).Value, Location.Create(1, 3).Value];
        yield return
            [Transport.Bicycle, Location.Create(1, 1).Value, Location.Create(1, 5).Value, Location.Create(1, 3).Value];

        // Велосипедист, заказ X:правее, Y: совпадает
        yield return
            [Transport.Bicycle, Location.Create(2, 2).Value, Location.Create(4, 2).Value, Location.Create(4, 2).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(8, 5).Value, Location.Create(7, 5).Value];

        // Велосипедист, заказ X:правее, Y: выше
        yield return
            [Transport.Bicycle, Location.Create(2, 2).Value, Location.Create(4, 4).Value, Location.Create(4, 2).Value];
        yield return
            [Transport.Bicycle, Location.Create(1, 1).Value, Location.Create(5, 5).Value, Location.Create(3, 1).Value];

        // Велосипедист, заказ X:совпадает, Y: ниже
        yield return
            [Transport.Bicycle, Location.Create(1, 3).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(5, 1).Value, Location.Create(5, 3).Value];

        // Велосипедист, заказ X:левее, Y: совпадает
        yield return
            [Transport.Bicycle, Location.Create(3, 2).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(1, 5).Value, Location.Create(3, 5).Value];

        // Велосипедист, заказ X:левее, Y: ниже
        yield return
            [Transport.Bicycle, Location.Create(3, 3).Value, Location.Create(1, 1).Value, Location.Create(1, 3).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(1, 1).Value, Location.Create(3, 5).Value];

        // Велосипедист, заказ ближе чем скорость
        yield return
            [Transport.Bicycle, Location.Create(1, 1).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value];
        yield return
            [Transport.Bicycle, Location.Create(1, 1).Value, Location.Create(2, 1).Value, Location.Create(2, 1).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(5, 4).Value, Location.Create(5, 4).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(4, 5).Value, Location.Create(4, 5).Value];

        // Велосипедист, заказ с шагами по 2 осям
        yield return
            [Transport.Bicycle, Location.Create(1, 1).Value, Location.Create(2, 2).Value, Location.Create(2, 2).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(4, 4).Value, Location.Create(4, 4).Value];
        yield return
            [Transport.Bicycle, Location.Create(1, 1).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value];
        yield return
            [Transport.Bicycle, Location.Create(5, 5).Value, Location.Create(5, 4).Value, Location.Create(5, 4).Value];
    }

    [Fact]
    public void ConstructorShouldBePrivate()
    {
        // Arrange
        var typeInfo = typeof(Courier).GetTypeInfo();

        // Act

        // Assert
        typeInfo.DeclaredConstructors.All(x => x.IsPrivate).Should().BeTrue();
    }

    [Fact]
    public void BeCorrectWhenParamsIsCorrect()
    {
        //Arrange
        var transport = Transport.Pedestrian;

        //Act
        var result = Courier.Create("Ваня", transport, Location.MinLocation);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Name.Should().Be("Ваня");
        result.Value.Transport.Should().Be(transport);
        result.Value.Location.Should().Be(Location.MinLocation);
    }

    [Fact]
    public void ReturnValueIsRequiredErrorWhenNameIsEmpty()
    {
        //Arrange
        var name = "";
        var transport = Transport.Pedestrian;

        //Act
        var result = Courier.Create(name, transport, Location.MinLocation);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(Courier.Errors.InvalidName());
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void CanMove(Transport transport, Location currentLocation, Location targetLocation,
        Location locationAfterMove)
    {
        //Arrange
        var courier = Courier.Create("Ваня", transport, currentLocation).Value;

        //Act
        var result = courier.Move(targetLocation);

        //Assert
        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(locationAfterMove);
    }

    [Fact]
    public void CantMoveToIncorrectLocation()
    {
        //Arrange
        var courier = Courier.Create("Ваня", Transport.Pedestrian, Location.MinLocation).Value;

        //Act
        var result = courier.Move(null);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(GeneralErrors.ValueIsRequired("targetLocation"));
    }

    [Fact]
    public void CanCalculateTimeToLocation()
    {
        /*
        Изначальная точка курьера: [1,1]
        Целевая точка: [5,10]
        Количество шагов, необходимое курьеру: 13 (4 по горизонтали и 9 по вертикали)
        Скорость транспорта (пешехода): 1 шаг в 1 такт
        Время подлета: 13/1 = 13.0 time потребуется курьеру, чтобы доставить заказ
        */

        //Arrange
        var location = Location.Create(5, 10).Value;
        var courier = Courier.Create("Ваня", Transport.Pedestrian, Location.MinLocation).Value;

        //Act
        var result = courier.CalculateStepsToLocation(location);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(13);
    }
}