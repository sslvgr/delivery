using System.Collections.Generic;
using DeliveryApp.Core.Domain.SharedKernel;
using Xunit;

namespace DeliveryApp.UnitTests.Core.Domain.SharedKernel;

public sealed class LocationTests
{
    [Fact]
    public void Constructor_ShouldCreateProperLocation()
    {
        // Arrange
        var x = 1;
        var y = 2;

        // Act
        var location = Location.Create(x, y);

        // Assert
        Assert.True(location.IsSuccess);
        Assert.Equal(location.Value.X, x);
        Assert.Equal(location.Value.Y, y);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Constructor_ShouldValidateRange(int x, int y, bool isValid)
    {
        // Arrange
        // Act
        var location = Location.Create(x, y);

        // Assert
        if (isValid)
        {
            Assert.Equal(location.IsSuccess, isValid);
            Assert.Equal(x, location.Value.X);
            Assert.Equal(y, location.Value.Y);
        }
        else
        {
            Assert.Equal(location.IsSuccess, isValid);
            Assert.NotNull(location.Error);
        }
    }

    [Theory]
    [InlineData(1, 1, 1, 1, 0)]
    [InlineData(1, 1, 2, 2, 2)]
    [InlineData(5, 5, 8, 8, 6)]
    [InlineData(10, 1, 1, 10, 18)]
    public void DistanceTo_ShouldReturnCorrectDifference(int x1, int y1, int x2, int y2, int expected)
    {
        // Arrange
        var location1 = Location.Create(x1, y1).Value;
        var location2 = Location.Create(x2, y2).Value;

        // Act
        var actual = location1.DistanceTo(location2);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CreateRandom_ShouldCreateNewLocation()
    {
        var location = Location.CreateRandom();

        Assert.NotNull(location);
        Assert.InRange(location.X, 1, 10);
        Assert.InRange(location.Y, 1, 10);
    }

    [Fact]
    public void Location_Equals_ShouldReturnTrue_ForEqualLocations()
    {
        // Arrange
        var location1 = Location.Create(5, 5);
        var location2 = Location.Create(5, 5);

        // Act
        var areEqual = location1.Equals(location2);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Location_Equals_ShouldReturnFalse_ForDifferentX()
    {
        // Arrange
        var location1 = Location.Create(5, 5);
        var location2 = Location.Create(6, 5);

        // Act
        var areEqual = location1.Equals(location2);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void Location_Equals_ShouldReturnFalse_ForDifferentY()
    {
        // Arrange
        var location1 = Location.Create(5, 5);
        var location2 = Location.Create(5, 6);

        // Act
        var areEqual = location1.Equals(location2);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void Location_Equals_ShouldReturnFalse_ForNull()
    {
        // Arrange
        var location1 = Location.Create(5, 5);

        // Act
        var areEqual = location1.Equals(null);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void Location_Equals_ShouldReturnFalse_ForDifferentType()
    {
        // Arrange
        var location1 = Location.Create(5, 5);
        var notALocation = new object();

        // Act
        var areEqual = location1.Equals(notALocation);

        // Assert
        Assert.False(areEqual);
    }

    public static IEnumerable<object[]> GetTestData()
    {
        for (int x = 0; x <= 11; x++)
        {
            for (int y = 0; y <= 11; y++)
            {
                bool isValid = x >= 1 && x <= 10 && y >= 1 && y <= 10;
                yield return new object[] { x, y, isValid };
            }
        }
    }
}
