using Ecommerce.SharedLibrary.Interface;
using OrderApi.Core.Entities;
using System.Linq.Expressions;

namespace OrderApi.Application.Interface
{
    public interface IOrder : IGenericInterface<Order>
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
    }
}
