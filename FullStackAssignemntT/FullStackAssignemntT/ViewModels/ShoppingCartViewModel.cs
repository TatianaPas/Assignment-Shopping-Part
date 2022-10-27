using FullStackAssignemntT.Models;

namespace FullStackAssignemntT.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> CartList { get; set; }
        public double CartTotal { get; set; }
    }
}
