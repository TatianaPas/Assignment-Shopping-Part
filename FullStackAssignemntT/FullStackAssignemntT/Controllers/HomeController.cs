using FullStackAssignemntT.Data;
using FullStackAssignemntT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FullStackAssignemntT.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;



        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //25.10 Tatiana-shop page
        public async Task<IActionResult> Shop()
        {
            IEnumerable<Product> products = await _context.ShopProducts.Include(p => p.Category).Include(p => p.Size).ToListAsync();
            return View(products);
        }

        //25.10 Tatiana - product details page
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShopProducts == null)
            {
                return NotFound();
            }

            ShoppingCart shoppingCart = new()
            {
                Count = 1,
                Product = await _context.ShopProducts
                .Include(p => p.Category)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id),
            };

            return View(shoppingCart);
        }

        public IActionResult Privacy()
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