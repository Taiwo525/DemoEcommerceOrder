using Ecommerce.SharedLibrary.Common;
using Ecommerce.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interface;
using OrderApi.Core.Entities;
using OrderApi.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                await context.Orders.AddAsync(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order placed successfully");
            }catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while placing order");
            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.UserId);
                if (order is null) return new Response(false, "order not found");

                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response(true, "order deleted");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while deleting order");
            } 
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.ToListAsync();

                return orders is not null ? orders : null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while retrieving orders");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();

                return order is not null ? order : null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while retrieving order");
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders.FindAsync(id);
                
                return order is not null ? order: null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while retrieving order");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).ToListAsync();

                return order is not null ? order : null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while retrieving order");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.UserId);
                if (order is null) return new Response(false, "order not found");

                context.Entry(entity).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "order updated");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while updating order");
            }
        }
    }
}
