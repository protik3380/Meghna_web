using System.Collections.Generic;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class MeghnaUserManager : CommonManager<MeghnaUser>, IMeghnaUserManager
    {
        public MeghnaUserManager() : base(new MeghnaUserRepository())
        {
        }

        public MeghnaUser GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id&&!c.IsDeleted.Value&&c.IsActive.Value,
                c => c.User);
        }

        public bool GetByUserEmail(string email)
        {
            MeghnaUser user = GetFirstOrDefault(c => c.Email == email && !c.IsDeleted.Value && c.IsActive.Value);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public MeghnaUser GetByUserId(long id)
        {
            return GetFirstOrDefault(c => c.UserId == id && !c.IsDeleted.Value && c.IsActive.Value,
                c => c.User);
        }

        public override bool Add(MeghnaUser entity)
        {
            entity.User = new User
            {
                Username = entity.Email,
                Password = "123456",
                IsActive = true,
                IsDeleted = false,
                UserTypeId = (int)UserTypeEnum.MeghnaUser
            };
            return base.Add(entity);
        }

        public override bool Add(ICollection<MeghnaUser> entities)
        {
            foreach (MeghnaUser meghnaUser in entities)
            {
                meghnaUser.User = new User
                {
                    Username = meghnaUser.Email,
                    Password = "123456",
                    IsActive = true,
                    IsDeleted = false,
                    UserTypeId = (int)UserTypeEnum.MeghnaUser
                };
            }
            return base.Add(entities);
        }
    }
}