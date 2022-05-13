using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class ProductImageManager: CommonManager<ProductImage>, IProductImageManager
    {
        public ProductImageManager() : base(new ProductImageRepository())
        {
        }
    }
}