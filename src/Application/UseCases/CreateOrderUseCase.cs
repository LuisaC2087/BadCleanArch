using Domain.Entities;
using System;
using Application.Interfaces;

namespace Application.UseCases;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _repo;

    public CreateOrderUseCase(IOrderRepository repo)
    {
        _repo = repo;
    }

    public Order Execute(string customer, string product, int qty, decimal price)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Customer = customer,
            Product = product,
            Qty = qty,
            Price = price
        };

        _repo.Save(order);

        return order;
    }
}