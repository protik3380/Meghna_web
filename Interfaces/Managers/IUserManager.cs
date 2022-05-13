using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface IUserManager : ICommonManager<User>
    {
        User GetById(long id);
        User ValidateUser(string username, string password);
        User GetByUserEmail(string email);
    }
}
