using Shared.DTOs.Basket;

namespace Saga.Orchestrator.HttpRepositories.Interfaces;

public interface IBasketHttpRepository
{
    Task<CartDto?> GetBasket(string username);

    Task<bool> DeleteBasket(string username);
}