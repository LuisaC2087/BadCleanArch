using Xunit;
using Application.UseCases;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Tests;


public class FakeRepo : IOrderRepository
{
    public void Save(Order order) { }
    public List<Order> GetLast() => new();
}

public class FakeLogger : ILog
{
    public void Log(string message) { }
    public void Error(string message) { }
}

public class CreateOrderUseCaseTests
{
    [Fact]
    public void Should_Create_Order()
    {
        var repo = new FakeRepo();
        var logger = new FakeLogger();

        var useCase = new CreateOrderUseCase(repo, logger);

        var result = useCase.Execute("Luisa", "Cuaderno", 2, 5000);

        Assert.NotNull(result);
        Assert.Equal("Luisa", result.Customer);
        Assert.Equal(2, result.Qty);
    }
}