using AutoMapper;
using OrderApi.Application.DTOs;
using OrderApi.Core.Entities;

namespace OrderApi.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<Order, OrderDetailsDto>();
        }
    }
}
