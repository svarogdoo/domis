using System.Data;

namespace domis.api.Repositories;

interface IOrderRepository
{
    
}
public class OrderRepository(IDbConnection connection) : IOrderRepository
{
    
}