using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Controllers {
    public class HomeController : Controller {
        private readonly IServiceProvider _serviceProvider;
        public HomeController(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index() {
            var registeredServices = new List<Type>();

            using (var scope = _serviceProvider.CreateScope()) {
                var services = scope.ServiceProvider;

                foreach (var service in services.GetServices<object>()) {
                    registeredServices.Add(service.GetType());
                }
            }

            return RedirectToAction("GetAll", "Product");
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}