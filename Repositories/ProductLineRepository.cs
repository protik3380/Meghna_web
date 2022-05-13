using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class ProductLineRepository : CommonRepository<ProductLine>, IProductLineRepository
    {
        public ProductLineRepository() : base(new FreshContext())
        {
        }
    }
}