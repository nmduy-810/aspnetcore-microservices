using Ordering.Domain.Entities;
using Contracts.Common.Interfaces;

namespace Ordering.Application.Common.Interfaces;

public interface IOrderRepository : IRepositoryBase<Order, long>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    Task<Order> CreateOrder(Order order);
}