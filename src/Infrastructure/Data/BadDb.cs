using System; 
namespace Infrastructure.Data; 

using System.Data; 
using Microsoft.Data.SqlClient; 



public static class BadDb 
{
    // codigo original public static string ConnectionString ="Server=localhost;Database=OrdersDb;Trusted_Connection=True;TrustServerCertificate=True";

    public static string ConnectionString = String.Format(
    "server=database-server;uid=user;pwd=%s;database=ProductionData",
    Environment.GetEnvironmentVariable("DB_PASSWORD")
);

    public static int ExecuteNonQueryUnsafe(string sql) 
    {
        var conn = new SqlConnection(ConnectionString); 
        var cmd = new SqlCommand(sql, conn); 
        conn.Open();
        return cmd.ExecuteNonQuery(); 
    }

    public static IDataReader ExecuteReaderUnsafe(string sql) 
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn); 
        conn.Open(); 
        return cmd.ExecuteReader(); 
    }


}
