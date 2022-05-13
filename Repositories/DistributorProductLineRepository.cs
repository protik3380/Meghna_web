using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class DistributorProductLineRepository : CommonRepository<DistributorProductLine>, IDistributorProductLineRepository
    {
        public DistributorProductLineRepository() : base(new FreshContext())
        {
        }
    }
}