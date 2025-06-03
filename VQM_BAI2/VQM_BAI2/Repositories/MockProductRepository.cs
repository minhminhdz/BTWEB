using VQM_BAI2.Models;

namespace VQM_BAI2.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 1000, Description = "High-end laptop", CategoryId = 1 }
    };

        public IEnumerable<Product> GetAll() => _products;
        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }
        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1) _products[index] = product;
        }
        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null) _products.Remove(product);
        }
    }


}
