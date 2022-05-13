using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class CorporateUserRepository : CommonRepository<CorporateUser>, ICorporateUserRepository
    {
        public CorporateUserRepository() : base(new FreshContext())
        {
        }
    }
}