using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Customer;

public abstract class CreateOrUpdateCustomerDto
{
    [Required]
    public string UserName { get; set; } = default!;

    [Required]
    [MaxLength(100, ErrorMessage = "Maximum length for First Name is 100 characters.")]
    public string FirstName { get; set; } = default!;

    [Required]
    [MaxLength(150, ErrorMessage = "Maximum length for First Name is 150 characters.")]
    public string LastName { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;
}