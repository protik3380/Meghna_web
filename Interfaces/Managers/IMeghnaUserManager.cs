using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface IMeghnaUserManager : ICommonManager<MeghnaUser>
    {
        MeghnaUser GetById(long id);
        MeghnaUser GetByUserId(long id);
        bool GetByUserEmail(string email);
    }
}