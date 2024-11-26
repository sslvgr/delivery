using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate;

public sealed class Order : Aggregate
{
    [ExcludeFromCodeCoverage]
    private Order()
    {
    }

    private Order(Guid id, Location location) : base(id)
    {
        Id = id;
        Location = location;
        Status = OrderStatus.Created;
        CourierId = null;
    }

    public Location Location { get; }
    public OrderStatus Status { get; private set; }
    public Guid? CourierId { get; private set; }

    public static Result<Order, Error> Create(Guid id, Location location)
    {
        if (id == Guid.Empty) return Errors.EmptyCourierId();
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        return new Order(id, location);
    }

    public UnitResult<Error> AssignCourier(Courier courier)
    {
        if (courier == null) return GeneralErrors.ValueIsRequired(nameof(courier));

        CourierId = courier.Id;
        Status = OrderStatus.Assigned;

        return new UnitResult<Error>();
    }

    public UnitResult<Error> Complete()
    {
        if (Status != OrderStatus.Assigned) return Errors.CantCompleteOrderIfNotAssigned();

        Status = OrderStatus.Completed;

        return new UnitResult<Error>();
    }

    /// <summary>
    ///     Ошибки, которые может возвращать сущность
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error CantCompleteOrderIfNotAssigned()
        {
            return new Error("Order.CantCompleteOrderIfNotAssigned",
                $"Невозможно завершить заказ в статусе отличном от {nameof(OrderStatus.Assigned)}");
        }

        public static Error EmptyCourierId()
        {
            return new Error("Order.EmptyCourierId",
                "Некорректный идентификатор курьера");
        }
    }
}