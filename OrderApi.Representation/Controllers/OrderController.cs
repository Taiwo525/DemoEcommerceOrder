using AutoMapper;
using Ecommerce.SharedLibrary.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interface;
using OrderApi.Core.Entities;

namespace OrderApi.Representation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderRepo;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IOrder orderRepo, IMapper mapper, IOrderService orderService)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var Orders = await _orderRepo.GetAllAsync();
            if (!Orders.Any()) return NotFound("No order detected");
            var results = _mapper.Map<IEnumerable<OrderDto>>(Orders);
            return Ok(results);
        }

        [HttpGet("{id}")] // Fixed: Removed quotes and added parameter binding
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            if (id <= 0) return BadRequest("Invalid order ID");
            var Order = await _orderRepo.GetByIdAsync(id);
            if (Order is null) return NotFound("No order detected");
            var result = _mapper.Map<OrderDto>(Order);
            return Ok(result);
        }

        [HttpGet("user/{userId}")] // Fixed: Corrected route template and casing
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders(int userId) // Fixed: Return type
        {
            if (userId <= 0) return BadRequest("Invalid user ID");
            var orders = await _orderService.GetOrdersByUserId(userId);
            if (orders == null || !orders.Any()) return NotFound("No orders found for this user");
            return Ok(orders);
        }

        [HttpGet("details/{orderId}")] // Fixed: Removed space in route template
        public async Task<ActionResult<OrderDetailsDto>> GetOrderDetails(int orderId) // Fixed: Method name typo
        {
            if (orderId <= 0) return BadRequest("Invalid order ID");
            var orderDetail = await _orderService.GetOrderDetails(orderId);
            if (orderDetail == null) return NotFound("No order found");
            return Ok(orderDetail);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var getEntity = _mapper.Map<Order>(orderDto);
            var response = await _orderRepo.CreateAsync(getEntity);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var getEntity = _mapper.Map<Order>(orderDto);
            var response = await _orderRepo.UpdateAsync(getEntity);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDto orderDto)
        {
            var getEntity = _mapper.Map<Order>(orderDto);
            var response = await _orderRepo.DeleteAsync(getEntity);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
    //public class OrderController : ControllerBase
    //{
    //    private readonly IOrder _orderRepo;
    //    private readonly IMapper _mapper;
    //    private readonly IOrderService _orderService;
    //    public OrderController(IOrder orderRepo, IMapper mapper, IOrderService orderService)
    //    {
    //        _orderRepo = orderRepo;
    //        _mapper = mapper;
    //        _orderService = orderService;
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    //    {

    //        var Orders = await _orderRepo.GetAllAsync();
    //        if(!Orders.Any()) return NotFound("No order detected");

    //        var results = _mapper.Map<IEnumerable<OrderDto>>(Orders);
    //        return Ok(results);
    //    }

    //    [HttpGet("id")]
    //    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    //    {

    //        var Order = await _orderRepo.GetByIdAsync(id);
    //        if (Order is null) return NotFound("No order detected");

    //        var result = _mapper.Map<OrderDto>(Order);
    //        return Ok(result);
    //    }

    //    [HttpGet("User/{userId}")]
    //    public async Task<ActionResult<OrderDto>> GetUserOrders(int userId)
    //    {
    //        if (userId <= 0 ) return BadRequest("Invalid data provided");

    //        var orders = await _orderService.GetOrdersByUserId(userId);
    //        return !orders.Any() ? NotFound(null) : Ok(orders);
    //    }

    //    [HttpGet("details/{orderId}")]
    //    public async Task<ActionResult<OrderDetailsDto>> GetOrderDatails(int orderId)
    //    {
    //        if (orderId <= 0) return BadRequest("Invalid data provided");

    //        var orderdetail = await _orderService.GetOrderDetails(orderId);
    //        return orderdetail.OrderId > 0 ? Ok(orderdetail) :NotFound("No order found");
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult<Response>> CreateOrder(OrderDto orderDto)
    //    {
    //        if (!ModelState.IsValid) return BadRequest(ModelState);

    //        var getEntity = _mapper.Map<Order>(orderDto);
    //        var response = await _orderRepo.CreateAsync(getEntity); 

    //        return response.Success ? Ok(response) : BadRequest(response);
    //    }

    //    [HttpPut]
    //    public async Task<ActionResult<Response>> UpdateOrder(OrderDto orderDto)
    //    {
    //        if (!ModelState.IsValid) return BadRequest(ModelState);

    //        var getEntity = _mapper.Map<Order>(orderDto);
    //        var response = await _orderRepo.UpdateAsync(getEntity);

    //        return response.Success ? Ok(response) : BadRequest(response);
    //    }

    //    [HttpDelete]
    //    public async Task<ActionResult<Response>> DeleteOrder(OrderDto orderDto)
    //    {

    //        var getEntity = _mapper.Map<Order>(orderDto);
    //        var response = await _orderRepo.DeleteAsync(getEntity);
    //        return response.Success ? Ok(response) : BadRequest(response);
    //    }
    //}
}
