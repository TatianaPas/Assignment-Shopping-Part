using Assignment.Utility;
using FullStackAssignemntT.Data;
using FullStackAssignemntT.Models;
using FullStackAssignemntT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FullStackAssignemntT.Controllers
{


    [Authorize]
    public class CartController : Controller
    {
        //27.10 Tatiana declare Db Context and shopping cart view model
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        public double OrderTotal { get; set; }

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            //27.10 Tatiana - retrieve id of logged in user who adding product to cart
            var claimsIDentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIDentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartViewModel
            {
                CartList = await _context.ShopShoppingCart.Where(m => m.ApplicationUserId == claim.Value)
                .Include(s => s.Product).Include(d => d.Product.Size).ToListAsync(),
                OrderHeader = new OrderHeader()
            };
            //27.10 Tatiana  - calculate cart total
            foreach (var cartItem in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cartItem.Product.ListPrice * cartItem.Count);
            }

            return View(ShoppingCartVM);
        }


        public async Task<IActionResult> Summary()
        {
            //27.10 Tatiana - retrieve id of logged in user who adding product to cart
            var claimsIDentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIDentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartViewModel
            {
                CartList = await _context.ShopShoppingCart.Where(m => m.ApplicationUserId == claim.Value)
                .Include(s => s.Product).Include(d => d.Product.Size).ToListAsync(),
                OrderHeader = new OrderHeader()
            };
            //27.10 Tatiana  - calculate cart total
            foreach (var cartItem in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cartItem.Product.ListPrice * cartItem.Count);
            }
            //27.10 retreave user Name from application user database
            ShoppingCartVM.OrderHeader.ApplicationUser = _context.ShopApplicationUsers.FirstOrDefault(u => u.Id == claim.Value);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;

            return View(ShoppingCartVM);
        }

        //27.10 Tatiana placing order action

        [HttpPost]       
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Summary(ShoppingCartViewModel ShoppingCartVM)
        {
            //27.10 Tatiana - retrieve id of logged in user who adding product to cart
            var claimsIDentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIDentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.CartList = await _context.ShopShoppingCart.Where(m => m.ApplicationUserId == claim.Value)
                .Include(s => s.Product).Include(d => d.Product.Size).ToListAsync();

            //27.10 set status as pending for the order header
            ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus= StaticDetails.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            //27.10 Tatiana  - calculate cart total
            foreach (var cartItem in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cartItem.Product.ListPrice * cartItem.Count);
            }

            await _context.ShopOrderHeaders.AddAsync(ShoppingCartVM.OrderHeader);
            _context.SaveChanges();

            //27.10 Enter data to Order Details table
            foreach (var cartItem in ShoppingCartVM.CartList)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = cartItem.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cartItem.Product.ListPrice,
                    Count = cartItem.Count
                };
                _context.ShopOrderDetails.Add(orderDetails);
                _context.SaveChanges();
            }

            _context.RemoveRange((ShoppingCartVM.CartList));
            _context.SaveChanges();
            return RedirectToAction("Shop", "Home");
        }


        //27.10 Tatiana  - increase count of items in the cart
        public IActionResult Plus(int cartId)
        {
            var cart = _context.ShopShoppingCart.FirstOrDefault(c => c.Id == cartId);
            cart.IncrementCount(cart, 1);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //27.10 Tatiana  - decrease count of items in the cart
        public IActionResult Minus(int cartId)
        {
            var cart = _context.ShopShoppingCart.FirstOrDefault(c => c.Id == cartId);
            if (cart.Count <= 1)
            {
                _context.ShopShoppingCart.Remove(cart);
            }
            else
            {
                cart.DecrementCount(cart, 1);               
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


        //27.10 Tatiana  - delete item from the cart
        public IActionResult Remove(int cartId)
        {
            var cart = _context.ShopShoppingCart.FirstOrDefault(c => c.Id == cartId);
            _context.ShopShoppingCart.Remove(cart);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }


    }
}
