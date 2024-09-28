using System;
namespace ECommerceBFF.DTOs
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}

