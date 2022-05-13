using System.Collections.Generic;
using System.Linq;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class ProductManager : CommonManager<Product>, IProductManager
    {
        public ProductManager() : base(new ProductRepository())
        {
        }

        public Product GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id, 
                c => c.Category);
        }


        public List<Product> GetAll()
        {
            return base.GetAll(c => c.Category).ToList();
        }

        public ICollection<Product> GetByBrand(long brandId)
        {
            return base.Get(c=>c.BrandId == brandId, 
                c => c.Category).ToList();
        }
        public ICollection<Product> GetByCategory(long categoryId)
        {
            return base.Get(c=>c.CategoryId == categoryId, 
                c => c.Brand).ToList();
        }
    }
}