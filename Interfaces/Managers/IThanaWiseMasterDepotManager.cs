using System.Collections.Generic;
using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface IThanaWiseMasterDepotManager : ICommonManager<ThanaWiseMasterDepot>
    {
        ThanaWiseMasterDepot GetByThana(long thanaId);
        bool IsExistMasterDepot(ThanaWiseMasterDepot thanaWiseMasterDepot);
    }
}