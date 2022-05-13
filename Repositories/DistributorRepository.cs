using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class DistributorRepository : CommonRepository<Distributor>, IDistributorRepository
    {
        public DistributorRepository() : base(new FreshContext())
        {
        }
    }
}