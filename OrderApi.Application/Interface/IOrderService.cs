using OrderApi.Application.DTOs;

namespace OrderApi.Application.Interface
{
    public interface IOrderService
    {
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
        Task<IEnumerable<OrderDto>> GetOrdersByUserId(int userId);
    }
}
