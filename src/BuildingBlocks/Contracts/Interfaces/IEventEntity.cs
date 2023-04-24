using Contracts.Domains.Intefaces;
using Contracts.Events;

namespace Contracts.Interfaces;

public interface IEventEntity
{
    void AddDomainEvent(BaseEvent domainEvent);
    void RemoveEvent(BaseEvent domainEvent);
    void ClearDomainEvents();
    IReadOnlyCollection<BaseEvent> DomainEvents();
}

public interface IEventEntity<T> : IEntityBase<T>, IEventEntity
{
    
}