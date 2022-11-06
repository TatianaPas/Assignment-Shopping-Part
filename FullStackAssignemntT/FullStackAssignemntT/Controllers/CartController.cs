using Assignment.Utility;
using FullStackAssignemntT.Data;
using FullStackAssignemntT.Models;
using FullStackAssignemntT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
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
                //06.11 Tatiana check if count of item not more than stock, tehn set it to maximum stock
                if(cartItem.Count > cartItem.Product.Stock)
                {
                    cartItem.Count = cartItem.Product.Stock;
                }
               
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


            //27.10 Tatiana Stripe pyment settings from stripe page https://stripe.com/docs/checkout/quickstart

            var domain = "https://localhost:44360/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"cart/index",
            };

            foreach (var item in ShoppingCartVM.CartList)
            {
                //27.10 Tatiana - display product details like name and quantity on checkout page
                var sessionLineItem = new SessionLineItemOptions
                {
                    
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.ListPrice * 100),
                        Currency = "nzd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            //add stripe session and payment intent ID to database
            ShoppingCartVM.OrderHeader.SessionId = session.Id;
            
            _context.SaveChanges();


            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            
        }


        //27.10 Tatiana  - check if payment is sucessfull from stripe card pauyment
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            OrderHeader orderHeader = await _context.ShopOrderHeaders.FirstOrDefaultAsync(s=>s.Id == id);            
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            //check Stripe status
            if(session.PaymentStatus.ToLower()=="paid")
            {
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                orderHeader.OrderStatus = StaticDetails.StatusApproved;
                orderHeader.PaymentStatus = StaticDetails.PaymentStatusApproved;

                _context.SaveChanges();
            }
            List<ShoppingCart> shoppicCart = _context.ShopShoppingCart
                .Where(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            //06.11 Tatiana decrease product stock by the count of products sold
            foreach (var item in shoppicCart)
            {
                Product product = _context.ShopProducts.Where(p => p.Id == item.ProductId).FirstOrDefault();
                product.Stock = (item.Product.Stock - item.Count);
            }

            _context.RemoveRange(shoppicCart);
            _context.SaveChanges();
            return View(id);
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
