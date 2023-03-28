namespace Contracts.Domains.Intefaces;

public interface IUserTracking
{
    string CreatedBy { get; set; }
    
    string LastModifiedBy { get; set; }
}