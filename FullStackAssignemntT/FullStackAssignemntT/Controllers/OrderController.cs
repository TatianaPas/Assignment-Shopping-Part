using Assignment.Utility;
using FullStackAssignemntT.Data;
using FullStackAssignemntT.Models;
using FullStackAssignemntT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssignemntT.Controllers
{
    [Authorize]
    //add ADMIN AND EMPLOYEE after testing!!
    public class OrderController : Controller
    {
        

        private readonly ApplicationDbContext _context;
        [BindProperty]
        public OrderViewModel OrderVM { get; set; }
      

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
          
        }
        public IActionResult Index()
        {
            return View();
        }

        //01.11 Tatiana Display order details while clicking on details button
        public async Task<IActionResult> Details(int orderId)
        {
            OrderVM = new OrderViewModel()
            {
                OrderHeader = _context.ShopOrderHeaders
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(u => u.Id == orderId).GetAwaiter().GetResult(),
                OrderDetails = await _context.ShopOrderDetails
                .Include(i=>i.Product)
                .Where(u => u.OrderId == orderId).ToListAsync()
            };
            return View(OrderVM);
        }


        //Create API end point to display data in Data Tables API

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            //01.11 Tatiana declare collection of order headers
            IEnumerable<OrderHeader> orderHeaders;

            //01.11 Tatiana retrive order headers from database
            orderHeaders = _context.ShopOrderHeaders.Include(p => p.ApplicationUser);
            switch(status)
            {
                //01.11 Tatiana load data to dattables according to which toggle button was pressed (sort by status)
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusInProcess);
                    break;
                case "approoved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusApproved);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusShipped);
                    break;
                default:
                    break;
            }



            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
