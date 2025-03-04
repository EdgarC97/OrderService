// DTOs/CreateOrderDto.cs
namespace OrderService.DTOs
{
    public class CreateOrderDto
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}