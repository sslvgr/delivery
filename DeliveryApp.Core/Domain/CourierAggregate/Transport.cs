using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.CourierAggregate;

public sealed class Transport : Entity<int>
{
    public static readonly Transport Pedestrian = new(1, nameof(Pedestrian).ToLowerInvariant(), 1);
    public static readonly Transport Bicycle = new(2, nameof(Bicycle).ToLowerInvariant(), 2);
    public static readonly Transport Car = new(3, nameof(Car).ToLowerInvariant(), 3);

    [ExcludeFromCodeCoverage]
    private Transport()
    {
    }

    private Transport(int id, string name, int speed)
    {
        Id = id;
        Name = name;
        Speed = speed;
    }

    public string Name { get; }
    public int Speed { get; }

    public static IEnumerable<Transport> List()
    {
        yield return Pedestrian;
        yield return Bicycle;
        yield return Car;
    }

    public static Result<Transport, Error> FromName(string name)
    {
        var transport = List()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (transport == null) return Errors.StatusIsWrong();

        return transport;
    }

    public static Result<Transport, Error> FromId(int id)
    {
        var transport = List()
            .SingleOrDefault(x => x.Id == id);
        if (transport == null) return Errors.StatusIsWrong();

        return transport;
    }

    /// <summary>
    ///     Ошибки, которые может возвращать сущность
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error StatusIsWrong()
        {
            return new Error($"{nameof(Transport).ToLowerInvariant()}.is.wrong",
                $"Не верное значение. Допустимые значения: {nameof(Transport).ToLowerInvariant()}:{string.Join(",", List().Select(s => s.Name))}");
        }
    }
}