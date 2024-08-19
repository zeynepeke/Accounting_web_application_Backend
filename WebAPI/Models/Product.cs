using System;

namespace WebAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
       
        public decimal Price { get; set; }
       
 
        public int StockQuantity { get; set; }
    }
}
