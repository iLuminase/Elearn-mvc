using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EbookMVC.Controllers
{
    public class AccessController : Controller
    {
        [AllowAnonymous]
        public IActionResult Denied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }
    }
}
