using Microsoft.AspNetCore.Mvc;
using UniqloFromJapan.Data;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Controllers {
    public class ManController : Controller {
        private DataRepository _dataRepository { get; set; }
        public ManController(DataRepository dataRepository) {
            _dataRepository = dataRepository;
        }

        public IActionResult Index() {
            var products = _dataRepository.Products.Where(x => x.Gender == "Man").ToArray();
            return View(products);
        }
    }
}
