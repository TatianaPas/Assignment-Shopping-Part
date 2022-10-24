using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackAssignemntT.Models
{
    public class Product
    {
        //23.10 Tatiana 

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Color { get; set; }
        [Required]
        public int Stock { get; set; }
        public string Description { get; set; }
        [Required]
        public string SKU { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        //navigation property
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [Display(Name="Size")]
        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        [ValidateNever]
        public Size Size { get; set; }
    }
}
