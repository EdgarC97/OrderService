using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.DTOs;
using OrderService.Services;
using System.Security.Claims;

namespace OrderService.Controllers
{
    /// <summary>
    /// Controller for managing orders. All endpoints require authentication.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="orderService">The order service used to handle order-related operations.</param>
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        /// <response code="200">Returns the list of orders.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves a specific order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order with the specified ID.</returns>
        /// <response code="200">Returns the requested order.</response>
        /// <response code="404">If the order is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return order == null ? NotFound() : Ok(order);
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="createOrderDto">The data transfer object containing the information for the new order.</param>
        /// <returns>The newly created order.</returns>
        /// <response code="201">Returns the created order.</response>
        /// <response code="400">If the input data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var order = await _orderService.CreateOrderAsync(createOrderDto, userId);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
    }
}
