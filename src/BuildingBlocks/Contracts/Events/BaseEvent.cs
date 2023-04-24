using Contracts.Interfaces;

namespace Contracts.Events;

public abstract class BaseEvent : IEventEntity
{
    public void AddDomainEvent(BaseEvent domainEvent)
    {
        throw new NotImplementedException();
    }

    public void RemoveEvent(BaseEvent domainEvent)
    {
        throw new NotImplementedException();
    }

    public void ClearDomainEvents()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<BaseEvent> DomainEvents()
    {
        throw new NotImplementedException();
    }
}