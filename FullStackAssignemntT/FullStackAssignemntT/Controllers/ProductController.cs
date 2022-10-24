using FullStackAssignemntT.Data;
using FullStackAssignemntT.Models;
using FullStackAssignemntT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FullStackAssignemntT.Controllers
{
    [BindProperties]
    public class ProductController : Controller
    {
        

        private readonly ApplicationDbContext _context;
        //24.10 Tatiana - host environemnt to build a path to wwwroot folder to store pictures
        private readonly IWebHostEnvironment _hostEnvironemnt;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment hostEnvironemnt)
        {
            _context = context;
            _hostEnvironemnt = hostEnvironemnt;
        }

        // GET: ProductsTemp
        public IActionResult Index()
        {
            return View();
        }

        //GET
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductViewModel productVM = new()
            {
                Product = new(),
                CategoryList = _context.ShopCategories.ToList().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                SizeList = _context.ShopSize.ToList().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    })
            };
            //Product product = new();

            ////24.10 - Tatiana - pass category list into category drop down menu
            //IEnumerable<SelectListItem> categoryList =  _context.ShopCategories.ToList().Select(
            //    u=> new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value =u.Id.ToString()
            //    }); 


            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = categoryList;


                return View(productVM);
            }
            else
            {
                //update product
               productVM.Product = await  _context.ShopProducts.FirstOrDefaultAsync(m => m.Id == id);
                return View(productVM);

            }


        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upsert(ProductViewModel productVM, IFormFile? file)

        {
            if (ModelState.IsValid)
            {
                string rootPath = _hostEnvironemnt.WebRootPath;
                if (file != null)
                {
                    //give file a new unique name
                    string fileName = Guid.NewGuid().ToString();
                    //show a path to the folder where images will be saved
                    var uploads = Path.Combine(rootPath, @"images\products");
                    var extensiosn = Path.GetExtension(file.FileName);

                    //check if image already exists( in case if updating product)
                    if(productVM.Product.ImageUrl!=null)
                    {
                        var oldImagePath = Path.Combine(rootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        //check if file existis in this path
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            //delete old image
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                   
                    //save image to the file path folder
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extensiosn), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    //save file path into product database
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extensiosn;
                }
                // if creating a new product, add to database
                if (productVM.Product.Id == 0)
                {
                    _context.ShopProducts.Add(productVM.Product);
                    TempData["success"] = "Product added successfully";
                }
                else
                {
                    // update if product alreayd exists
                    _context.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully";
                }

                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");

            }
            return View(productVM);
        }



        //Create API end point to display data in Data Tables API

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _context.ShopProducts.Include(p => p.Category).Include(p => p.Size);
            return Json(new { data = productList });
            }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var productToDelete = await _context.ShopProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if(productToDelete == null)
            {
                return Json(new { success = false, message = "Error, try again" });
            }

            //delete old image if it exists

            var oldImagePath = Path.Combine(_hostEnvironemnt.WebRootPath, productToDelete.ImageUrl.TrimStart('\\'));
            //check if file existis in this path
            if (System.IO.File.Exists(oldImagePath))
            {
                //delete old image
                System.IO.File.Delete(oldImagePath);
            }

            _context.ShopProducts.Remove(productToDelete);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Product Deleted Sucessfully" });

        }
        #endregion
    }
}
