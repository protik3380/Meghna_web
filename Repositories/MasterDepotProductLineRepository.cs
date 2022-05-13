using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class MasterDepotProductLineRepository: CommonRepository<MasterDepotProductLine>, IMasterDepotProductLineRepository
    {
        public MasterDepotProductLineRepository() : base(new FreshContext())
        {
        }
    }
}