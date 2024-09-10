using System;
namespace ECommerceBFF.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int OrderStatusId { get; set; }
        public bool IsActive { get; set; }
        public OrderStatusDto OrderStatus { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}

