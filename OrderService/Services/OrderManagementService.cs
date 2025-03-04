using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using OrderService.DTOs;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Services
{
    public class OrderManagementService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public OrderManagementService(IOrderRepository repository, IMapper mapper, IMemoryCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            string cacheKey = "all_orders";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<OrderDto> cachedOrders))
            {
                var orders = await _repository.GetAllAsync();
                cachedOrders = _mapper.Map<IEnumerable<OrderDto>>(orders);
                _cache.Set(cacheKey, cachedOrders, _cacheDuration);
            }
            return cachedOrders;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, int userId)
        {
            var order = _mapper.Map<Order>(createOrderDto);
            order.UserId = userId;
            await _repository.AddAsync(order);
            return _mapper.Map<OrderDto>(order);
        }
    }
}