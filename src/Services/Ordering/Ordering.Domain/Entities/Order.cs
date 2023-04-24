using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Events;
using Ordering.Domain.Enums;
using Ordering.Domain.OrderAggregate.Events;

namespace Ordering.Domain.Entities;

public class Order : AuditableEventEntity<long>
{
    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public string UserName { get; set; } = default!;

    public string DocumentNo { get; set; } = new Guid().ToString();

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

    public Order AddedOrder()
    {
        AddDomainEvent(new OrderCreatedEvent(Id, UserName, DocumentNo, TotalPrice, FirstName, LastName, EmailAddress, ShippingAddress, InvoiceAddress));
        return this;
    }

    public Order DeletedOrder()
    {
        AddDomainEvent(new OrderDeletedEvent(Id));
        return this;
    }
}