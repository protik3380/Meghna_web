using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    interface IProductUnitManager:ICommonManager<ProductUnit>
    {
        ICollection<ProductUnit> GetAll();
        ProductUnit GetById(long id);
        ICollection<ProductUnit> GetByBrand(long brandId);
        ICollection<ProductUnit> GetByCategory(long categoryId);
        bool SaveProductDetails(ProductUnit productUnit);
        bool EditProductDetails(ProductUnit productUnit);
    }
}
