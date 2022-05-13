using System.Collections.Generic;
using System.Linq;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class ThanaWiseMasterDepotManager : CommonManager<ThanaWiseMasterDepot>, IThanaWiseMasterDepotManager
    {
        public ThanaWiseMasterDepotManager() : base(new ThanaWiseMasterDepotRepository())
        {
        }

        public ThanaWiseMasterDepot GetByThana(long thanaId)
        {
            return GetFirstOrDefault(c => c.Id == thanaId, c => c.MasterDepot, c => c.Thana);
        }


        public bool IsExistMasterDepot(ThanaWiseMasterDepot thanaWiseMasterDepot)
        {
            var masterDepot = GetFirstOrDefault(c => c.MasterDepotId == thanaWiseMasterDepot.MasterDepotId && c.ThanaId == thanaWiseMasterDepot.ThanaId);
            if (masterDepot != null)
            {
                return true;
            }
            return false;
        }
    }
}