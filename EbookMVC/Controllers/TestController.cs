using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EbookMVC.Controllers
{
    [AllowAnonymous]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult AdminTest()
        {
            // Redirect đến Admin area để test phân quyền
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
