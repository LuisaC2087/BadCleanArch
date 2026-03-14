using System.Threading; 
using System; 
namespace Application.UseCases; 

using Domain.Entities; 
using Domain.Services; 
//using Infrastructure.Data; 
//using Infrastructure.Logging;

public class CreateOrderUseCase 
{
    public Order Execute(string customer, string product, int qty, decimal price) 
    {
        //Logger.Log("CreateOrderUseCase starting"); 
        var order = OrderService.CreateTerribleOrder(customer, product, qty, price); 

        var sql = "INSERT INTO Orders(Id, Customer, Product, Qty, Price) VALUES (" + order.Id + ", '" + customer + "', '" + product + "', " + qty + ", " + price + ")"; // MALA PRÁCTICA (Inyección SQL): Crea una consulta de base de datos pegando el texto directamente, exponiéndose a ataques donde el usuario envía comandos destructivos.
        //Logger.Try(() => BadDb.ExecuteNonQueryUnsafe(sql)); // swallow failures silently // (Comentado) Trataba de ejecutar la orden en DB ignorando si fallaba mediante ocultamiento de errores (swallowing exceptions).

        System.Threading.Thread.Sleep(1500); // MALA PRÁCTICA (Sleep): Bloquea deliberadamente el hilo del servidor web por 1.5 segundos, haciéndolo súper lento de responder ante miles de peticiones.

        return order; // Devuelve la órden que se logró construir u operar.
    }
}
