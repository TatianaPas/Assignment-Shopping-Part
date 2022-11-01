using FullStackAssignemntT.Models;

namespace FullStackAssignemntT.ViewModels
{
    //01.11.2022 Tatiana
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
