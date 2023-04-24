using Contracts.Events;
using MediatR;

namespace Ordering.Domain.OrderAggregate.Events;

public class OrderDeletedEvent : BaseEvent, INotification
{
    public long Id { get; set; }

    public OrderDeletedEvent(long id)
    {
        Id = id;
    }
}