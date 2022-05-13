using System.Collections.Generic;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class CorporateUserManager : CommonManager<CorporateUser>, ICorporateUserManager
    {
        public CorporateUserManager() : base(new CorporateUserRepository())
        {
        }

        public CorporateUser GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id,
                c => c.CorporateContract);
        }

        public bool GetByUserEmail(string email)
        {
            CorporateUser user = GetFirstOrDefault(c => c.Email == email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public CorporateUser GetByUserId(long id)
        {
            return GetFirstOrDefault(c => c.UserId == id,
                c => c.CorporateContract);
        }

        public override bool Add(CorporateUser entity)
        {
            entity.User = new User
            {
                Username = entity.Email,
                Password = "123456",
                IsActive = true,
                IsDeleted = false,
                UserTypeId = (int)UserTypeEnum.Corporate
            };
            return base.Add(entity);
        }

        public override bool Add(ICollection<CorporateUser> entities)
        {
            foreach (CorporateUser corporateUser in entities)
            {
                corporateUser.User = new User
                {
                    Username = corporateUser.Email,
                    Password = "123456",
                    IsActive = true,
                    IsDeleted = false,
                    UserTypeId = (int)UserTypeEnum.Corporate
                };
            }

            return base.Add(entities);
        }
    }
}