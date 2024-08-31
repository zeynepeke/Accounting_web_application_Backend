using System;

namespace WebAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int Barcode {get; set;}
        public string Name { get; set; }
         
        public decimal Price { get; set; }
            public string Description {get; set;}  
             public int Stockquantity {get; set;}
       
 
      
    }
}
