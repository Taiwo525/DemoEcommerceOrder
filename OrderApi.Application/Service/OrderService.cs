using AutoMapper;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interface;
using Polly.Registry;
using System.Net.Http.Json;

namespace OrderApi.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrder _orderRepo;
        private readonly HttpClient _httpClient;
        private readonly ResiliencePipelineProvider<string> _resiliencePipeline;
        private readonly IMapper _mapper;

        public OrderService(
            IOrder orderRepo,
            HttpClient httpClient,
            ResiliencePipelineProvider<string> resiliencePipeline,
            IMapper mapper)
        {
            _orderRepo = orderRepo;
            _httpClient = httpClient;
            _resiliencePipeline = resiliencePipeline;
            _mapper = mapper;
        }

        public async Task<ProductDto?> GetProduct(int productId)
        {
            var getProduct = await _httpClient.GetAsync($"/api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode) return null;
            return await getProduct.Content.ReadFromJsonAsync<ProductDto>();
        }

        public async Task<UserDto?> GetUser(int userId)
        {
            var getUser = await _httpClient.GetAsync($"/api/users/{userId}"); 
            if (!getUser.IsSuccessStatusCode) return null;
            return await getUser.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<OrderDetailsDto?> GetOrderDetails(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order is null || order.Id <= 0) return null;

            var retryPipeline = _resiliencePipeline.GetPipeline("my-retry-pipeline");

            var productDto = await retryPipeline.ExecuteAsync(
                async token => await GetProduct(order.ProductId));
            if (productDto == null) return null;

            var userDto = await retryPipeline.ExecuteAsync(
                async token => await GetUser(order.UserId));
            if (userDto == null) return null;

            return new OrderDetailsDto(
                order.Id,
                productDto.Id,
                userDto.Id,
                userDto.Name,
                userDto.Email,
                userDto.PhoneNumber,
                userDto.Address,
                productDto.Name,
                order.PurchaseQuantity,
                productDto.Price,
                productDto.Price * order.PurchaseQuantity,
                order.OrderedDate
            );
        }

        public async Task<IEnumerable<OrderDto>?> GetOrdersByUserId(int userId)
        {
            var orders = await _orderRepo.GetOrdersAsync(x => x.UserId == userId);
            return orders?.Any() == true ? _mapper.Map<IEnumerable<OrderDto>>(orders) : null;
        }
    }
    
}
