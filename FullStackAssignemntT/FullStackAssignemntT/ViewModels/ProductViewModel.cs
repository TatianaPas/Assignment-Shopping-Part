using FullStackAssignemntT.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FullStackAssignemntT.ViewModels
{
    //24.10 - Tatiana View Model for inserting new product
    public class ProductViewModel
    {
        [ValidateNever]
        public Product Product {get; set;}
        [ValidateNever]

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SizeList { get; set; }


    }
}
