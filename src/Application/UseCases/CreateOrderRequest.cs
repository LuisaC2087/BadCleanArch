namespace Application.UseCases;

public record CreateOrderRequest(
    string Customer,
    string Product,
    int Qty,
    decimal Price
);