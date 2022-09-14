using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Data;
using Shopping.Models;

namespace Shopping.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("ShoppingCart") ?? new List<Cart>();
            ShoppingCartVM shoppingCartVM = new()
            {
                ShoppingCartItems = cart,
                Total = cart.Sum(x => x.Quantity * x.Price)
            };

            return View(shoppingCartVM);
        }
        public async Task<IActionResult> AddProduct(int id)
        {
            Product product = await _context.products.FindAsync(id);

            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("ShoppingCart") ?? new List<Cart>();

            Cart cartProduct = cart.Where(c => c.ProductId == id).FirstOrDefault();

            if (cartProduct == null)
            {
                cart.Add(new Cart(product));
            }
            else
            {
                cartProduct.Quantity += 1;
            }

            HttpContext.Session.SetJson("ShoppingCart", cart);

            TempData["Success"] = "The product has been added!";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> DecreaseProduct(int id)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("ShoppingCart");

            Cart cartProduct = cart.Where(c => c.ProductId == id).FirstOrDefault();

            if (cartProduct.Quantity > 1)
            {
                --cartProduct.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("ShoppingCart");
            }
            else
            {
                HttpContext.Session.SetJson("ShoppingCart", cart);
            }

            TempData["Success"] = "The product has been removed!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("ShoppingCart");

            cart.RemoveAll(p => p.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("ShoppingCart");
            }
            else
            {
                HttpContext.Session.SetJson("ShoppingCart", cart);
            }

            TempData["Success"] = "The product has been removed!";

            return RedirectToAction("Index");
        }

        public IActionResult ClearShoppingCart()
        {
            HttpContext.Session.Remove("ShoppingCart");

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            HttpContext.Session.Remove("ShoppingCart");

            return View();
        }
    }
}
