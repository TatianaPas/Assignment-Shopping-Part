using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FullStackAssignemntT.Data;
using FullStackAssignemntT.Models;

namespace FullStackAssignemntT.Controllers
{
    public class ShoppingCartsTempController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartsTempController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCartsTemp
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShopShoppingCart.Include(s => s.ApplicationUser).Include(s => s.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShoppingCartsTemp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShopShoppingCart == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShopShoppingCart
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCartsTemp/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ShopApplicationUsers, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.ShopProducts, "Id", "Name");
            return View();
        }

        // POST: ShoppingCartsTemp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Count,ApplicationUserId")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ShopApplicationUsers, "Id", "Id", shoppingCart.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.ShopProducts, "Id", "Name", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // GET: ShoppingCartsTemp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShopShoppingCart == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShopShoppingCart.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ShopApplicationUsers, "Id", "Id", shoppingCart.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.ShopProducts, "Id", "Name", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // POST: ShoppingCartsTemp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Count,ApplicationUserId")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ShopApplicationUsers, "Id", "Id", shoppingCart.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.ShopProducts, "Id", "Name", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // GET: ShoppingCartsTemp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShopShoppingCart == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShopShoppingCart
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCartsTemp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShopShoppingCart == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShopShoppingCart'  is null.");
            }
            var shoppingCart = await _context.ShopShoppingCart.FindAsync(id);
            if (shoppingCart != null)
            {
                _context.ShopShoppingCart.Remove(shoppingCart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(int id)
        {
          return _context.ShopShoppingCart.Any(e => e.Id == id);
        }
    }
}
