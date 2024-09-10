using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBFF.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ProductCode { get; set; }
        public int CurrentStock { get; set; }
        public int LowStockAlert { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<int> CategoryIds { get; set; }

    }
}

