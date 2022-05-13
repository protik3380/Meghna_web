using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class MasterDepotRepository : CommonRepository<MasterDepot>, IMasterDepotRepository
    {
        public MasterDepotRepository() : base(new FreshContext())
        {
        }
    }
}