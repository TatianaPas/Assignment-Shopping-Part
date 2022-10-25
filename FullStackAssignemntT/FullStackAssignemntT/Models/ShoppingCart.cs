using System.ComponentModel.DataAnnotations;

namespace FullStackAssignemntT.Models
{
    public class ShoppingCart
    {
        public Product Product { get; set; }
        [Range(1, 10, ErrorMessage = "Please add correct amount of product")]
        public int Count { get; set; }

        public ShoppingCart()
        {
            Count = 1;
        }
    }
}
