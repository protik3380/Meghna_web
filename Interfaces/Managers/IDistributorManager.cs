using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface IDistributorManager : ICommonManager<Distributor>
    {
        Distributor GetById(long distrobutorId);
    }
   
}