using Microsoft.AspNetCore.Mvc;
using UniqloFromJapan.Data;

namespace UniqloFromJapan.Controllers {
    public class WomanController : Controller {
        private DataRepository _dataRepository { get; set; }
        public WomanController(DataRepository dataRepository) {
            _dataRepository = dataRepository;
        }

        public IActionResult Index() {
            var products = _dataRepository.Products.Where(x => x.Gender == "Woman").ToArray();
            return View(products);
        }
    }
}
