using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class ProductLineDetailRepository : CommonRepository<ProductLineDetail>, IProductLineDetailRepository
    {
        public ProductLineDetailRepository() : base(new FreshContext())
        {
        }
    }
}