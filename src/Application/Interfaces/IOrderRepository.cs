using Domain.Entities;
using System.Collections.Generic;

public interface IOrderRepository
{
    void Save(Order order);
    List<Order> GetLast();
}