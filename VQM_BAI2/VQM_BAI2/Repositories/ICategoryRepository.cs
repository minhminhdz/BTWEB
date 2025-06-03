using VQM_BAI2.Models;

namespace VQM_BAI2.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
    }

}
