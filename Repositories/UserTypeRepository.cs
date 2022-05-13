using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class UserTypeRepository : CommonRepository<UserType>, ICommonRepository<UserType>
    {
        public UserTypeRepository() : base(new FreshContext())
        {
        }
    }
}