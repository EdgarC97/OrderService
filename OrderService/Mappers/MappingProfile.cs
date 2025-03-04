using AutoMapper;
using OrderService.Models;
using OrderService.DTOs;

namespace OrderService.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}