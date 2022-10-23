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
        public double ListPrice { get; set; }
        public string ImageUrl { get; set; }
        //navigation property
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
