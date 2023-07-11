using Microsoft.AspNetCore.Mvc;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Controllers {
    public class AdminController : Controller {
        public IActionResult Index() {
            Response.Cookies.Append("test", "test");
            return View();
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Admin admin) {

            return RedirectToAction("Index", "Home");
        }
    }
}
