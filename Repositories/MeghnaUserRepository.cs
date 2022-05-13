using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class MeghnaUserRepository : CommonRepository<MeghnaUser>, IMeghnaUserRepository
    {
        public MeghnaUserRepository() : base(new FreshContext())
        {
        }
    }
}