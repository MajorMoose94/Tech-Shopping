using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice
        {
            get { return Quantity * Price; }
        }
        public string Image { get; set; }
        public Cart() { }
        public Cart(Product product)
        {
            CartId = CartId;
            UserId = UserId;
            ProductId = product.Id;
            ProductName = product.Name;
            Price = product.Price;
            Quantity = 1;
            Image = product.Image;

        }

        public virtual Product  Product { get; set; }
    }
}
