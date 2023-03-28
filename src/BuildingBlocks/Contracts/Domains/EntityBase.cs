using Contracts.Domains.Intefaces;

namespace Contracts.Domains;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; } = default!;
}