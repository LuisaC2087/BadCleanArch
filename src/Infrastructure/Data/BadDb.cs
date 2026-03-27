using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace Infrastructure.Data;

public class SqlOrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public SqlOrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Save(Order order)

    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(
            "INSERT INTO Orders (Id, Customer, Product, Qty, Price) VALUES (@Id, @Customer, @Product, @Qty, @Price)",
            conn
        );

        cmd.Parameters.AddWithValue("@Id", order.Id);
        cmd.Parameters.AddWithValue("@Customer", order.Customer);
        cmd.Parameters.AddWithValue("@Product", order.Product);
        cmd.Parameters.AddWithValue("@Qty", order.Qty);
        cmd.Parameters.AddWithValue("@Price", order.Price);

        cmd.ExecuteNonQuery();
    }

    public List<Order> GetLast()
    {
        var list = new List<Order>();

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT TOP 10 * FROM Orders ORDER BY Id DESC", conn);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Order
            {
                Id = reader.GetGuid(0),
                Customer = reader.GetString(1),
                Product = reader.GetString(2),
                Qty = reader.GetInt32(3),
                Price = reader.GetDecimal(4)
            });
        }

        return list;
    }
}