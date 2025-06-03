using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VQM_G3_webbanhang.Models;
using VQM_G3_webbanhang.Repositories;

namespace VQM_G3_webbanhang.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(IProductRepository productRepository,
        ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var category = await _categoryRepository.GetAllAsync();
            return View(category);
        }
        public async Task<IActionResult> Display(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category); // cần await
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));

            }
            return View(category);
        }

        private async Task<Category> GetOrCreateDefaultCategory()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var defaultCategory = categories.FirstOrDefault(c => c.Name == "Chưa có danh mục");

            if (defaultCategory == null)
            {
                defaultCategory = new Category { Name = "Chưa có danh mục" };
                await _categoryRepository.AddAsync(defaultCategory);
            }
            return defaultCategory;
        }



        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                // Lấy tất cả sản phẩm có CategoryId bằng id
                var products = await _productRepository.GetAllAsync();
                var relatedProducts = products.Where(p => p.CategoryId == id).ToList();

                // Cập nhật CategoryId về null
                foreach (var product in relatedProducts)
                {
                    product.CategoryId = null;
                    await _productRepository.UpdateAsync(product);
                }

                // Xóa danh mục
                await _categoryRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Đã xóa danh mục và cập nhật sản phẩm liên quan.";
            }

            return RedirectToAction(nameof(Index));
        }

        //[HttpPost, ActionName("DeleteConfirmed")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var category = await _categoryRepository.GetByIdAsync(id);
        //    if (category != null)
        //    {
        //        await _categoryRepository.DeleteAsync(id);
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
    }
}