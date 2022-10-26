using Microsoft.AspNetCore.Mvc;

namespace FullStackAssignemntT.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
