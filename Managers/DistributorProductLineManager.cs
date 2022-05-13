using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class DistributorProductLineManager: CommonManager<DistributorProductLine>, IDistributorProductLineManager
    {
        public DistributorProductLineManager() : base(new DistributorProductLineRepository())
        {
        }
    }
}