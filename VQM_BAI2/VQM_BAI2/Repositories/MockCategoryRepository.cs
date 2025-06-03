using VQM_BAI2.Models;

namespace VQM_BAI2.Repositories
{
    using System.Collections.Generic;

    public class MockCategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categories = new()
    {
        new Category { Id = 1, Name = "Laptop" },
        new Category { Id = 2, Name = "Desktop" }
    };

        public IEnumerable<Category> GetAllCategories() => _categories;
    }


}
