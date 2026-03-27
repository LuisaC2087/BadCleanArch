using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces;

public interface IOrderRepository
{
    void Save(Order order);
    List<Order> GetLast();
}