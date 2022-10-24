using System.ComponentModel.DataAnnotations;

namespace FullStackAssignemntT.Models
{  //created 24.10 Tatiana
    public class Size
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
