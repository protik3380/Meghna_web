using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface ICorporateUserManager : ICommonManager<CorporateUser>
    {
        CorporateUser GetById(long id);
        CorporateUser GetByUserId(long id);
        bool GetByUserEmail(string email);
    }
}
