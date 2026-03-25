using System;

namespace Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    public string Customer { get; set; } = "";

    public string Product { get; set; } = "";

    public int Qty { get; set; }

    public decimal Price { get; set; }

    public decimal Total => Qty * Price;
}