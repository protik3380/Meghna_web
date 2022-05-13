using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class DistributorManager: CommonManager<Distributor>, IDistributorManager
    {
        public DistributorManager() : base(new DistributorRepository())
        {
        }
        public Distributor GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }
    }
}