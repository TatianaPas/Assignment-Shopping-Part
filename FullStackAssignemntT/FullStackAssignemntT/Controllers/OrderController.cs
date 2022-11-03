using Assignment.Utility;
using FullStackAssignemntT.Data;
using FullStackAssignemntT.Models;
using FullStackAssignemntT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FullStackAssignemntT.Controllers
{
    [BindProperties]
    [Authorize]
    //add ADMIN AND EMPLOYEE after testing!!
    public class OrderController : Controller
    {
        

        private readonly ApplicationDbContext _context;

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
        public  IActionResult Details(int orderId)
        {
            OrderVM = new OrderViewModel()
            {
                OrderHeader = _context.ShopOrderHeaders.Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(u => u.Id == orderId)
                .GetAwaiter().GetResult(),
                OrderDetails = _context.ShopOrderDetails
                .Include(i=>i.Product)
                .Where(u => u.OrderId == orderId).ToList()
            };
            return View(OrderVM);
        }

        //03.11 Tatiana update order details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetails()
        {
            //find order header in DB
            var orderHeaderDB =  await _context.ShopOrderHeaders.FirstOrDefaultAsync(o => o.Id == OrderVM.OrderHeader.Id);
            if (ModelState.IsValid)
            {
                orderHeaderDB.Name = OrderVM.OrderHeader.Name;
                orderHeaderDB.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
                orderHeaderDB.StreetAddress = OrderVM.OrderHeader.StreetAddress;
                orderHeaderDB.City = OrderVM.OrderHeader.City;
                orderHeaderDB.Suburb = OrderVM.OrderHeader.Suburb;
                orderHeaderDB.PostalCode = OrderVM.OrderHeader.PostalCode;

                if (OrderVM.OrderHeader.Courier != null)
                {
                    orderHeaderDB.Courier = OrderVM.OrderHeader.Courier;
                }
                if (OrderVM.OrderHeader.TrackingNumber != null)
                {
                    orderHeaderDB.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
                }
                _context.ShopOrderHeaders.Update(orderHeaderDB);
                await _context.SaveChangesAsync();
            }

            //display notification that details updated
            TempData["Success"] = "Order Details Updated Successfully";
            return RedirectToAction("Details", "Order", new { orderId = orderHeaderDB.Id});
        }



        //03.11 Tatiana ship order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShipOrder()
        {
            //find order header in DB
            var orderHeaderDB = await _context.ShopOrderHeaders.FirstOrDefaultAsync(o => o.Id == OrderVM.OrderHeader.Id);
            //update order status
            orderHeaderDB.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeaderDB.Courier = OrderVM.OrderHeader.Courier;
            orderHeaderDB.OrderStatus = StaticDetails.StatusShipped;
            orderHeaderDB.ShippingDate = DateTime.Now;

            _context.ShopOrderHeaders.Update(orderHeaderDB);
            await _context.SaveChangesAsync();
           

            //display notification that details updated
            TempData["Success"] = "Order Shipped";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        //03.11 Tatiana process order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder()
        {
            //find order header in DB
            var orderHeaderDB = await _context.ShopOrderHeaders.FirstOrDefaultAsync(o => o.Id == OrderVM.OrderHeader.Id);
            //update order status
            orderHeaderDB.OrderStatus = StaticDetails.StatusInProcess;
            _context.ShopOrderHeaders.Update(orderHeaderDB);
            await _context.SaveChangesAsync();


            //display notification that details updated
            TempData["Success"] = "Order Processing";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        //03.11 Tatiana cancel order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder()
        {
            //find order header in DB
            var orderHeaderDB = await _context.ShopOrderHeaders.FirstOrDefaultAsync(o => o.Id == OrderVM.OrderHeader.Id);
            //check if payment was made
           if(orderHeaderDB.PaymentStatus==StaticDetails.PaymentStatusApproved)
            {
                //do refund
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderDB.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                orderHeaderDB.OrderStatus = StaticDetails.StatusCancelled;
                orderHeaderDB.PaymentStatus = StaticDetails.StatusRefunded;
            }
            else
            {
                orderHeaderDB.OrderStatus = StaticDetails.StatusCancelled;
                orderHeaderDB.PaymentStatus = StaticDetails.StatusRefunded;

            }
            _context.ShopOrderHeaders.Update(orderHeaderDB);
            await _context.SaveChangesAsync();


            //display notification that details updated
            TempData["Success"] = "Order Cancelled";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
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
