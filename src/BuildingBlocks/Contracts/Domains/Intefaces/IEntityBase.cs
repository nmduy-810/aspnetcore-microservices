namespace Contracts.Domains.Intefaces;

public interface IEntityBase<T>
{
    T Id { get; set; }
}