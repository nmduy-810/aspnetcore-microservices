using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domains;
using Ordering.Domain.Enums;

namespace Ordering.Domain.Entities;

public class Order : EntityAuditBase<long>
{
    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public string UserName { get; set; } = default!;
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }
    
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = default!;
    
    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string LastName { get; set; } = default!;
    
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;
    
    [Column(TypeName = "nvarchar(max)")]
    public string ShippingAddress { get; set; } = default!;
    
    [Column(TypeName = "nvarchar(max)")]
    public string InvoiceAddress { get; set; } = default!;

    public OrderStatusEnum Status { get; set; }
}