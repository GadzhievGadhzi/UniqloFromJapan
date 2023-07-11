using Microsoft.AspNetCore.Mvc;
using UniqloFromJapan.Data;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Controllers {
    public class AdminController : Controller {

        private readonly DataRepository _dataRepository;
        public AdminController(DataRepository dataRepository) {
            _dataRepository = dataRepository;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Admin target) {
            var admin = _dataRepository.Admins
                .Where(x => x.Login == target.Login && x.Password == target.Password)
                .FirstOrDefault();

            if(admin == null) {
                return View(model: "Такого пользователя нет!");
            }

            Response.Cookies.Append("Status", "Admin");
            return View(model: "Вы успешно авторизовались!");
        }
    }
}
