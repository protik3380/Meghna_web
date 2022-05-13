using EFreshStore.Models.Context;
using System.Collections.Generic;

namespace EFreshStore.Interfaces.Managers
{
    public interface IMasterDepotManager : ICommonManager<MasterDepot>
    {
        MasterDepot GetById(long id);
        MasterDepot GetByThanaAndProduct(long thanaId);
    }
}