using System.ComponentModel.DataAnnotations;

namespace FullStackAssignemntT.Models
{
    //created 21.10 Tatiana
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
