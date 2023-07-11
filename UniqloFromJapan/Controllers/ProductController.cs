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
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult GetAll() {
            var products = _dataRepository.Products.ToArray();
            return View(products);
        }

        [HttpGet]
        public IActionResult Remove() {
            if (Request.Cookies["Status"] != "Admin") {
                return RedirectToAction("Login", "Admin");
            }

            return View();
        }  

        [HttpPost]
        public IActionResult Remove(int id) {
            if (Request.Cookies["Status"] != "Admin") {
                return RedirectToAction("Login", "Admin");
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
        public IActionResult Add() {
            if (Request.Cookies["Status"] != "Admin") {
                return RedirectToAction("Login", "Admin");
            }

            return View(new ProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model) {
            if (Request.Cookies["Status"] != "Admin") {
                return RedirectToAction("Login", "Admin");
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
        public IActionResult WishList() {
            List<Product>? products = new List<Product>();
            if(HttpContext.Request.Cookies.TryGetValue("WishList", out string? value)) {
                products = JsonConvert.DeserializeObject<List<Product>>(value);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddWishList(int id) {
            List<Product>? products = new List<Product>();
            var product = await _dataRepository.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(product == null) {
                return Ok();
            }

            if (Request.Cookies.TryGetValue("WishList", out string? value)) {
                products = JsonConvert.DeserializeObject<List<Product>>(value!);
                products!.Add(product);
                Response.Cookies.Append("WishList", "2");
            } else {
                products.Add(product);
                Response.Cookies.Append("WishList", "1");
            }

            var cookie = Request.Cookies["WishList"];
            return View(products);
        }
    }
}