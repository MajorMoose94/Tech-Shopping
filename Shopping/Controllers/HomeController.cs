using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Models;
using System.Diagnostics;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            BindDropDownList("0");
            var products = _context.products.Include(p=>p.Category).ToList();
            return View(products);
        }
        private void BindDropDownList(string categoryId)
        {
            var categories = _context.categories.ToList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var category in categories)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                    Selected = categoryId == category.Id.ToString()
                });
            }
            ViewBag.categories = listItems;
        }

        [HttpPost]
        public IActionResult Index(string category)
        {
            List<Product> products = new List<Product>();
            if (category != "0")
            {
                products = _context.products.Where(p => p.CategoryId == Convert.ToInt32(category)).ToList();
            }
            else
            {
                products = _context.products.ToList();
            }
            BindDropDownList(category);
            return View(products);
        }

        public IActionResult ProductDetails(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var product = _context.products.Include(p=>p.Category).FirstOrDefault(p=>p.Id == Id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}