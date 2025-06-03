﻿using VQM_BAI2.Models;

namespace VQM_BAI2.Repositories
{
    using System.Collections.Generic;

    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }


}
