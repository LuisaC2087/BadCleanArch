using Domain.Entities;
using System;
using Application.Interfaces;

namespace Application.UseCases;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _repo;
    private readonly ILog _log;

    public CreateOrderUseCase(IOrderRepository repo, ILog logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public Order Execute(string customer, string product, int qty, decimal price)
    {
        _logger.Log("Creating order");

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