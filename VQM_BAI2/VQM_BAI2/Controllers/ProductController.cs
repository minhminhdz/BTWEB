using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using VQM_BAI2.Models;
using VQM_BAI2.Repositories;

namespace VQM_BAI2.Controllers
{

    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index() => View(_productRepo.GetAll()); 

        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_categoryRepo.GetAllCategories(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                    product.ImageUrl = await SaveImage(imageUrl);

                if (imageUrls != null)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                        product.ImageUrls.Add(await SaveImage(file));
                }

                _productRepo.Add(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Display(int id)
        {
            var product = _productRepo.GetById(id);
            return product == null ? NotFound() : View(product);
        }

        public IActionResult Update(int id)
        {
            var product = _productRepo.GetById(id);
            return product == null ? NotFound() : View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepo.Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepo.GetById(id);
            return product == null ? NotFound() : View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepo.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var path = Path.Combine("wwwroot/images", image.FileName);
            using var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);
            return "/images/" + image.FileName;
        }
    }
}
