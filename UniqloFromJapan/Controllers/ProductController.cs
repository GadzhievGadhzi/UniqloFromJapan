using Microsoft.AspNetCore.Mvc;
using UniqloFromJapan.Data;
using UniqloFromJapan.Models;
using Newtonsoft.Json;

namespace UniqloFromJapan.Controllers {
    public class ProductController : Controller {
        private DataRepository _dataRepository { get; set; }
        public ProductController(DataRepository dataRepository) {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public IActionResult GetAll() {
            var products = _dataRepository.Products.ToArray();
            return View(products);
        }

        [HttpGet]
        public IActionResult Remove() {
            return View();  
        }

        [HttpPost]
        public IActionResult Remove(int id) {
            var product = _dataRepository.Products.Where(x => x.Id == id).FirstOrDefault();
            if (product != null) {
                _dataRepository.Products.Remove(product);
                _dataRepository.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Get(int id) {
            var product = _dataRepository.Products.Where(x => x.Id == id).FirstOrDefault();
            if(product == null) {
                return RedirectToAction("Index", "Home");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Add() {
            ProductViewModel model = new ProductViewModel();

            var enumColorModels = new List<EnumColorModel>();
            var enumSizeModels = new List<EnumSizeModel>();

            var colors = Enum.GetNames(typeof(ProductColor)).ToList();
            colors.ForEach(x => {
                enumColorModels.Add(new EnumColorModel() { Color = (ProductColor)Enum.Parse(typeof(ProductColor), x), IsColorSelected = false });
            });

            var sizes = Enum.GetNames(typeof(ProductSize)).ToList();
            sizes.ForEach(x => {
                enumSizeModels.Add(new EnumSizeModel() { Size = (ProductSize)Enum.Parse(typeof(ProductSize), x), IsSizeSelected = false });
            });

            model.CheckBoxColorItems = enumColorModels;
            model.CheckBoxSizeItems = enumSizeModels;
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel model) {

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

            _dataRepository.Products.Add(product);
            _dataRepository.SaveChanges();

            return RedirectToAction("Index", "Home");
            /*return RedirectToAction("Get", "Product", new { Id = id });*/
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
        public IActionResult AddWishList(int id) {
            List<Product>? products = new List<Product>();
            var product = _dataRepository.Products.Where(x => x.Id == id).First();

            if (Request.Cookies.TryGetValue("WishList", out string? value)) {
                products = JsonConvert.DeserializeObject<List<Product>>(value!);
                products!.Add(product);
                Response.Cookies.Append("WishList", JsonConvert.SerializeObject(products), new CookieOptions() {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });
            } else {
                products.Add(product);
                Response.Cookies.Append("WishList", JsonConvert.SerializeObject(products), new CookieOptions() {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });
            }

            var cookie = Request.Cookies["WishList"];
            return View(products);
        }
    }
}