using System.Collections.Generic;

namespace DataHandler.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }
        
        public string Color { get; set; }

        public int Stock { get; set; }

        public int Price { get; set; }

        public List<ProductDetails> ProductDetails { get; set; }
    }

    public class ProductDetails
    {
        public string Description { get; set; }

        public string ImgUrl { get; set; }
    }


}
