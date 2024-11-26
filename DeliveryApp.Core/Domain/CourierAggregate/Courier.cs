using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.CourierAggregate;

public sealed class Courier : Aggregate
{
    [ExcludeFromCodeCoverage]
    private Courier()
    {
    }

    private Courier(string name, Transport transport, Location location)
    {
        Id = Guid.NewGuid();
        Name = name;
        Transport = transport;
        Location = location;
        Status = CourierStatus.Free;
    }

    public string Name { get; }
    public Transport Transport { get; }
    public Location Location { get; private set; }
    public CourierStatus Status { get; private set; }

    public static Result<Courier, Error> Create(string name, Transport transport, Location location)
    {
        if (string.IsNullOrWhiteSpace(name)) return Errors.InvalidName();
        if (transport == null) return GeneralErrors.ValueIsRequired(nameof(transport));
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        return new Courier(name, transport, location);
    }

    public UnitResult<Error> SetBusy()
    {
        Status = CourierStatus.Busy;

        return new UnitResult<Error>();
    }

    public UnitResult<Error> SetFree()
    {
        Status = CourierStatus.Free;

        return new UnitResult<Error>();
    }

    public Result<int, Error> CalculateStepsToLocation(Location location)
    {
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        return Location.DistanceTo(location) / Transport.Speed;
    }

    public UnitResult<Error> Move(Location targetLocation)
    {
        if (targetLocation == null) return GeneralErrors.ValueIsRequired(nameof(targetLocation));

        var difX = targetLocation.X - Location.X;
        var difY = targetLocation.Y - Location.Y;

        var newX = Location.X;
        var newY = Location.Y;

        var cruisingRange = Transport.Speed;

        if (difX > 0)
        {
            if (difX >= cruisingRange)
            {
                newX += cruisingRange;
                Location = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (difX < cruisingRange)
            {
                newX += difX;
                Location = Location.Create(newX, newY).Value;
                if (Location == targetLocation)
                    return UnitResult.Success<Error>();
                cruisingRange -= difX;
            }
        }

        if (difX < 0)
        {
            if (Math.Abs(difX) >= cruisingRange)
            {
                newX -= cruisingRange;
                Location = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (Math.Abs(difX) < cruisingRange)
            {
                newX -= Math.Abs(difX);
                Location = Location.Create(newX, newY).Value;
                if (Location == targetLocation)
                    return UnitResult.Success<Error>();
                cruisingRange -= Math.Abs(difX);
            }
        }

        if (difY > 0)
        {
            if (difY >= cruisingRange)
            {
                newY += cruisingRange;
                Location = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (difY < cruisingRange)
            {
                newY += difY;
                Location = Location.Create(newX, newY).Value;
                if (Location == targetLocation)
                    return UnitResult.Success<Error>();
            }
        }

        if (difY < 0)
        {
            if (Math.Abs(difY) >= cruisingRange)
            {
                newY -= cruisingRange;
                Location = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (Math.Abs(difY) < cruisingRange)
            {
                newY -= Math.Abs(difY);
                Location = Location.Create(newX, newY).Value;
                if (Location == targetLocation)
                    return UnitResult.Success<Error>();
            }
        }

        Location = Location.Create(newX, newY).Value;
        return UnitResult.Success<Error>();
    }

    /// <summary>
    ///     Ошибки, которые может возвращать сущность
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error InvalidName()
        {
            return new Error("Courier.InvalidName",
                "Некорректное имя курьера");
        }
    }
}