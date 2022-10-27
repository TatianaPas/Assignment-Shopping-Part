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
                CartList = await _context.ShopShoppingCart.Where(m => m.ApplicationUserId == claim.Value).Include(s => s.Product).Include(d => d.Product.Size).ToListAsync()
            };
            //27.10 Tatiana  - calculate cart total
            foreach (var cartItem in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.CartTotal += (cartItem.Product.ListPrice * cartItem.Count);
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
                CartList = await _context.ShopShoppingCart.Where(m => m.ApplicationUserId == claim.Value).Include(s => s.Product).Include(d => d.Product.Size).ToListAsync()
            };
            //27.10 Tatiana  - calculate cart total
            foreach (var cartItem in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.CartTotal += (cartItem.Product.ListPrice * cartItem.Count);
            }

            return View(ShoppingCartVM);
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
