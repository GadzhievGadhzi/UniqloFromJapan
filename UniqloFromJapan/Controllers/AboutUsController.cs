using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloFromJapan.Data;
using UniqloFromJapan.Models;
using UniqloFromJapan.Services;

namespace UniqloFromJapan.Controllers {
    public class AboutUsController : Controller { 

        private readonly DataRepository _dataRepository;
        private readonly CachingService _cachingService;
        public AboutUsController(DataRepository dataRepository, CachingService cachingService) {
            _dataRepository = dataRepository;
            _cachingService = cachingService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit() {
            var aboutUsModel = await _dataRepository.AboutUsModels.FirstOrDefaultAsync();
            return View(model: (aboutUsModel != null) ? aboutUsModel : new AboutUsModel());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string title, string description) {
            /*var firstFileInCollection = HttpContext.Request.Form.Files.FirstOrDefault();
            if(firstFileInCollection != null) {
                byte[] imageData;
                using (var binaryReader = new BinaryReader(firstFileInCollection.OpenReadStream())) {
                    imageData = binaryReader.ReadBytes((int)firstFileInCollection.Length);
                }

                var convertedToBase64 = Convert.ToBase64String(imageData);
                _dataRepository.AboutUsModels.RemoveRange(_dataRepository.AboutUsModels.ToArray());
                _dataRepository.AboutUsModels.Add(new AboutUsModel { 
                    Title = title,
                    Description = description,
                    ImageInBase64 = convertedToBase64,
                });

                await _dataRepository.SaveChangesAsync();
            }*/

            _dataRepository.AboutUsModels.RemoveRange(_dataRepository.AboutUsModels.ToArray());
            _dataRepository.AboutUsModels.Add(new AboutUsModel {
                Title = title,
                Description = description,
            });

            await _dataRepository.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}