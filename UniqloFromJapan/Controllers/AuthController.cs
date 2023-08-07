using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using UniqloFromJapan.Data;
using UniqloFromJapan.Models;

namespace UniqloFromJapan.Controllers {
    public class AuthController : Controller {

        private readonly DataRepository _dataRepository;
        public AuthController(DataRepository dataRepository) {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password) {
            if(email == null || password == null) {
                return View(model: "Login and password fields must be filled in ");
            }

            var user = await _dataRepository.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
            if(user == null) {
                return View(model: "Incorrect login or password");
            }

            Response.Cookies.Append("UserId", user.Id.ToString());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string firstName, string lastName, string patronymic, string email, string password) {
            if (email == null || password == null) {
                return View(model: "Login and password fields must be filled in");
            }

            var expression = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
            if (!Regex.IsMatch(email, expression)) {
                return View(model: "You have entered an incorrect e-mail address");
            }

            if (password.Length < 8 || password.Length > 15) {
                return View(model: "Password must be no less than 8 characters and no more than 15 characters");
            }

            _dataRepository.Users.Add(new User() {
                FirstName = firstName,
                LastName = lastName,
                Patronymic = patronymic,
                Email = email,
                Password = password,
                Status = UserStatus.Default
            });

            await _dataRepository.SaveChangesAsync();
            var user = await _dataRepository.Users.Where(x => x.Email == email && x.Password == password).FirstAsync();
            Response.Cookies.Append("UserId", user.Id.ToString());
            return RedirectToAction("Index", "Home");
        }
    }
}
