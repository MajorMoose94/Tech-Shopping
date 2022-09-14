using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image { get; set; } = "NoImageFound.png";
        [Required]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

    }
}
