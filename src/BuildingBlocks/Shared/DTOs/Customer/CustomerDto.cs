namespace Shared.DTOs.Customer;

public class CustomerDto
{
    public string UserName { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
    
    public string EmailAddress { get; set; } = default!;
}