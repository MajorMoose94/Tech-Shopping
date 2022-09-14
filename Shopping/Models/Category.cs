using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Category")]
        public string Name { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
