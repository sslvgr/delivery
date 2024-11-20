using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel;

public sealed class Location : ValueObject
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public static readonly Location MinLocation = new(1, 1);

    public static readonly Location MaxLocation = new(10, 10);

    [ExcludeFromCodeCoverage]
    private Location()
    {
    }

    private Location(int x, int y) : this()
    {
        X = x;
        Y = y;
    }

    public static Result<Location, Error> Create(int x, int y)
    {
        if (x < MinLocation.X || x > MaxLocation.X) return GeneralErrors.ValueIsInvalid(nameof(x));
        if (y < MinLocation.Y || y > MaxLocation.Y) return GeneralErrors.ValueIsInvalid(nameof(y));

        return new Location(x, y);
    }

    public int DistanceTo(Location other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    public static Location CreateRandom()
    {
        return new Location(Random.Shared.Next(MinLocation.X, MaxLocation.X), Random.Shared.Next(MinLocation.X, MaxLocation.Y));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}
