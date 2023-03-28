namespace Contracts.Domains.Intefaces;

public interface IDateTracking
{
    DateTimeOffset CreatedDate { get; set; }
    
    DateTimeOffset? LastModifiedDate { get; set; }
}