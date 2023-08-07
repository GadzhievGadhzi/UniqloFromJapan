using Microsoft.AspNetCore.Mvc;
using UniqloFromJapan.Models;
using UniqloFromJapan.Services;

namespace UniqloFromJapan.Controllers.Admin {
    public class AdminController : Controller {

        private readonly CachingService _cachingService;
        public AdminController(CachingService cachingService) {
            _cachingService = cachingService;
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int productId) {
            var product = await _cachingService.GetObjectFromCache(productId);
            if(product == null) {
                return NotFound();
            }

            return View(new ProductViewModel(product));
        }
    }
}
