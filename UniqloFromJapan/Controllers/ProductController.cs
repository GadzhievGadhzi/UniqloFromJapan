using Microsoft.AspNetCore.Mvc;
using UniqloFromJapan.Data;
using UniqloFromJapan.Models;
using Newtonsoft.Json;
using UniqloFromJapan.Services;
using Microsoft.EntityFrameworkCore;

namespace UniqloFromJapan.Controllers {
    public class ProductController : Controller {
        private readonly DataRepository _dataRepository;
        private readonly CachingService _cachingService;
        public ProductController(DataRepository dataRepository, CachingService cachingService) {
            _dataRepository = dataRepository;
            _cachingService = cachingService;
        }

        [HttpGet]
        public IActionResult GetAll() {

            var products = _dataRepository.Products.ToArray();

            ProductAllViewModel? model = new ProductAllViewModel();
            model.WishList = new int[] { };

            if (Request.Cookies.TryGetValue("UserId", out string? userId)) {
                var id = int.Parse(userId);
                var user = _dataRepository.Users.Where(x => x.Id == id).First();
                model.WishList = user.WishList;
            }

            model.Products = products;

            var aboutUsModel = _dataRepository.AboutUsModels.FirstOrDefault();
            return View((model, (aboutUsModel == null) ? new AboutUsModel() : aboutUsModel));
        }

        [HttpGet]
        public async Task<IActionResult> Remove() {
            if (Request.Cookies.TryGetValue("UserId", out string? value)) {
                var userId = int.Parse(value!);
                var user = await _dataRepository.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null) {
                    return RedirectToAction("Login", "Auth");
                }

                if (user.Status != UserStatus.Admin) {
                    return RedirectToAction("Index", "Home");
                }
            } else {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }  

        [HttpPost]

        public async Task<IActionResult> Remove(int id) {
            if (Request.Cookies.TryGetValue("UserId", out string? value)) {
                var userId = int.Parse(value!);
                var user = await _dataRepository.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null) {
                    return RedirectToAction("Login", "Auth");
                }

                if(user.Status != UserStatus.Admin) {
                    return RedirectToAction("Index", "Home");
                }
            } else {
                return RedirectToAction("Login", "Auth");
            }

            var product = _dataRepository.Products.Where(x => x.Id == id).FirstOrDefault();
            if (product != null) {
                _dataRepository.Products.Remove(product);
                _dataRepository.SaveChanges();
                return View(model: "Продукт успешно удален!");
            }
            return View(model: "Продукт не найден!");
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id) {
            var product = await _cachingService.GetObjectFromCache(id);
            if(product == null) {
                return RedirectToAction("Index", "Home");
            }

            return View(product);
        }

        [HttpPost]
        public async Task Get(int id, object _) {
            var product = await _cachingService.GetObjectFromCache(id);
            Response.Cookies.Append(id.ToString(), JsonConvert.SerializeObject(product));
        }

        [HttpGet]
        public async Task<IActionResult> Add() {
            if (Request.Cookies.TryGetValue("UserId", out string? value)) {
                var userId = int.Parse(value!);
                var user = await _dataRepository.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                user.Status = UserStatus.Admin;
                if (user == null) {
                    return RedirectToAction("Login", "Auth");
                }

                if (user.Status != UserStatus.Admin) {
                    return RedirectToAction("Index", "Home");
                }
            } else {
                return RedirectToAction("Login", "Auth");
            }

            return View(new ProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model) {
            if (Request.Cookies.TryGetValue("UserId", out string? value)) {
                var userId = int.Parse(value!);
                var user = await _dataRepository.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null) {
                    return RedirectToAction("Login", "Auth");
                }

                if (user.Status != UserStatus.Admin) {
                    return RedirectToAction("Index", "Home");
                }
            } else {
                return RedirectToAction("Login", "Auth");
            }

            var isColorSelected = model.CheckBoxColorItems!.Where(p => p.IsColorSelected).Select(x => x.Color).ToArray();
            var isSizeSelected = model.CheckBoxSizeItems!.Where(p => p.IsSizeSelected).Select(x => x.Size).ToArray();

            var product = new Product() {
                Title = model.Title,
                Price = model.Price,
                Gender = model.Gender,
                Materials = model.Materials,
                ShortDescription = model.ShortDescription,
                BigDescription = model.BigDescription,
                Colors = isColorSelected,
                Size = isSizeSelected,
            };

            List<string> images = new List<string>();
            IFormFileCollection files = HttpContext.Request.Form.Files;
            for(var i = 0; i < files.Count; i++) {
                byte[] imageData;
                using (var binaryReader = new BinaryReader(files[i].OpenReadStream())) {
                    imageData = binaryReader.ReadBytes((int)files[i].Length);
                }

                var convertedToBase64 = Convert.ToBase64String(imageData);
                images.Add(convertedToBase64);
            }
            product.Images = images.ToArray();

            await _cachingService.AddObjectFromCache(product);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> WishList() {
            if (Request.Cookies.TryGetValue("UserId", out string? value)) {
                var userId = int.Parse(value!);
                var user = await _dataRepository.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null) {
                    return RedirectToAction("Login", "Auth");
                }

                var products = new List<Product>();
                foreach (var item in user.WishList) {
                    var product = _dataRepository.Products.Where(x => x.Id == item).FirstOrDefault();
                    if (product == null) {
                        continue;
                    }
                    products.Add(product);
                }
                return View(products);  
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> AddWishList(int id) {
            List<Product>? products = new List<Product>();
            var product = await _dataRepository.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(product == null) {
                return RedirectToAction("Index", "Home");
            }

            if(Request.Cookies.TryGetValue("UserId", out string? value)){
                var userId = int.Parse(value!);
                var user = await _dataRepository.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                if(user == null) {
                    return RedirectToAction("Login", "Auth");
                }

                var wishList = user.WishList.ToList();
                if (!wishList.Contains(id)) {
                    wishList.Add(id);
                    user.WishList = wishList.ToArray();
                    await _dataRepository.SaveChangesAsync();
                } else {
                    wishList.Remove(id);
                    user.WishList = wishList.ToArray();
                    _dataRepository.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("WishList", "Product");
        }
    }
}