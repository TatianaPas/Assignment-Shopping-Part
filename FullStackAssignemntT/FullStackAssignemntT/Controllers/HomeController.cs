using FullStackAssignemntT.Data;
using FullStackAssignemntT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

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
        public async Task<IActionResult> Details(int productId)
        {
            if (productId == 0 || _context.ShopProducts == null)
            {
                return NotFound();
            }

            ShoppingCart shoppingCart = new()
            {
                Count = 1,
                Product = await _context.ShopProducts
                .Include(p => p.Category)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == productId),
            };

            return View(shoppingCart);
        }


        //26.10 Tatiana - product details add to cart. Can be done only by registered users.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart, int productId)
        {
            shoppingCart.Product = await _context.ShopProducts.Include(p => p.Category).Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == productId);

            //26.10 Tatiana - retrieve id of logged in user who adding product to cart
            var claimsIDentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIDentity.FindFirst(ClaimTypes.NameIdentifier);
            
            shoppingCart.ApplicationUserId = claim.Value;

            //26.10 Tatiana check if user already have started shopping cart in database and it has item with same id
            ShoppingCart cartFromDb = await _context.ShopShoppingCart.FirstOrDefaultAsync(
                u => u.ApplicationUserId == claim.Value && u.ProductId== shoppingCart.ProductId);
            

            if (cartFromDb == null)
            {
                //06.11 Tatiana Check if amout of items more than Stock, replase count with maximum stock
                if (shoppingCart.Count> shoppingCart.Product.Stock)
                {
                    shoppingCart.Count = shoppingCart.Product.Stock;
                }
                await _context.ShopShoppingCart.AddAsync(shoppingCart);
            }
            else
            {// 06.11 Tatiana Check if amout of items less than stock, then increase count
                if (shoppingCart.Count < shoppingCart.Product.Stock)
                {
                    shoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
                }                    
            }

            shoppingCart.Product = await _context.ShopProducts
                .Include(p => p.Category)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == productId);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Shop));
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