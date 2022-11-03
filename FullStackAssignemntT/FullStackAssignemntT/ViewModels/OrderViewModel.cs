using FullStackAssignemntT.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FullStackAssignemntT.ViewModels
{
    //01.11.2022 Tatiana
    public class OrderViewModel
    {
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [ValidateNever]
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
