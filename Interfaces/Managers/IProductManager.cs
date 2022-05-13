using System.Collections.Generic;
using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface IProductManager : ICommonManager<Product>
    {
        Product GetById(long id);
        List<Product> GetAll();
        ICollection<Product> GetByBrand(long brandId);
        ICollection<Product> GetByCategory(long categoryId);
    }
}