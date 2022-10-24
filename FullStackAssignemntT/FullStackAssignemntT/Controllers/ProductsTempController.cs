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
    public class ProductsTempController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsTempController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductsTemp
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShopProducts.Include(p => p.Category).Include(p => p.Size);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductsTemp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShopProducts == null)
            {
                return NotFound();
            }

            var product = await _context.ShopProducts
                .Include(p => p.Category)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: ProductsTemp/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.ShopCategories, "Id", "Name");
            ViewData["SizeId"] = new SelectList(_context.ShopSize, "Id", "Name");
            return View();
        }

        // POST: ProductsTemp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Color,Stock,Description,SKU,ListPrice,ImageUrl,CategoryId,SizeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ShopCategories, "Id", "Name", product.CategoryId);
            ViewData["SizeId"] = new SelectList(_context.ShopSize, "Id", "Name", product.SizeId);
            return View(product);
        }

        // GET: ProductsTemp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShopProducts == null)
            {
                return NotFound();
            }

            var product = await _context.ShopProducts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ShopCategories, "Id", "Name", product.CategoryId);
            ViewData["SizeId"] = new SelectList(_context.ShopSize, "Id", "Name", product.SizeId);
            return View(product);
        }

        // POST: ProductsTemp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Color,Stock,Description,SKU,ListPrice,ImageUrl,CategoryId,SizeId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.ShopCategories, "Id", "Name", product.CategoryId);
            ViewData["SizeId"] = new SelectList(_context.ShopSize, "Id", "Name", product.SizeId);
            return View(product);
        }

        // GET: ProductsTemp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShopProducts == null)
            {
                return NotFound();
            }

            var product = await _context.ShopProducts
                .Include(p => p.Category)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductsTemp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShopProducts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShopProducts'  is null.");
            }
            var product = await _context.ShopProducts.FindAsync(id);
            if (product != null)
            {
                _context.ShopProducts.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.ShopProducts.Any(e => e.Id == id);
        }
    }
}
