using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class UserTypeManager : CommonManager<UserType>, IUserTypeManager
    {
        public UserTypeManager() : base(new UserTypeRepository())
        {
        }
    }
}