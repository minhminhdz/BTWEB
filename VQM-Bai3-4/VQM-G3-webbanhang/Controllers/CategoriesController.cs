using Microsoft.AspNetCore.Mvc;
using VQM_G3_webbanhang.Models;
using VQM_G3_webbanhang.Repositories;
using System.Linq;
using System.Threading.Tasks;

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
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
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
                await _categoryRepository.AddAsync(category);
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
                // Lấy hoặc tạo danh mục mặc định
                var defaultCategory = await GetOrCreateDefaultCategory();

                // Lấy các sản phẩm thuộc danh mục bị xóa
                var products = await _productRepository.GetAllAsync();
                var relatedProducts = products.Where(p => p.CategoryId == id).ToList();

                // Gán sản phẩm về danh mục mặc định
                foreach (var product in relatedProducts)
                {
                    product.CategoryId = defaultCategory.Id;
                    await _productRepository.UpdateAsync(product);
                }

                // Xóa danh mục
                await _categoryRepository.DeleteAsync(id);

                TempData["SuccessMessage"] = "Đã xóa danh mục và chuyển sản phẩm về 'Chưa có danh mục'.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
