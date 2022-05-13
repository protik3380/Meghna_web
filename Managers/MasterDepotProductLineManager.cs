using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class MasterDepotProductLineManager : CommonManager<MasterDepotProductLine>, IMasterDepotProductLineManager
    {
        public MasterDepotProductLineManager() : base(new MasterDepotProductLineRepository())
        {
        }
    }
}