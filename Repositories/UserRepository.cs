using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class UserRepository : CommonRepository<User>, IUserRepository
    {
        public UserRepository() : base(new FreshContext())
        {
        }
    }
}