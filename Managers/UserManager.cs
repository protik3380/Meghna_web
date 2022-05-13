using System;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class UserManager : CommonManager<User>, IUserManager
    {
        public UserManager() : base(new UserRepository())
        {
        }

        public User GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id&&c.IsActive.HasValue&&c.IsActive.Value&&c.IsDeleted.HasValue&&!c.IsDeleted.Value);
        }

        public User ValidateUser(string username, string password)
        {
            User user = GetFirstOrDefault(c =>
                c.Username.ToLower().Equals(username.ToLower())
                && c.Password.Equals(password)
                && c.IsActive == true &&
                c.IsDeleted == false);
            return user;
        }
        public User GetByUserEmail(string email)
        {
            return GetFirstOrDefault(c => c.Username == email && c.IsActive.HasValue && c.IsActive.Value && c.IsDeleted.HasValue && !c.IsDeleted.Value);

        }

       
    }
}