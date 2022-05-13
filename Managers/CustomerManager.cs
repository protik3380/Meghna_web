using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class CustomerManager : CommonManager<Customer>, ICustomerManager
    {
        public CustomerManager() : base(new CustomerRepository())
        {
        }
        public Customer GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id,
                c => c.User);
        }

        public Customer GetByUserId(long id)
        {
            return GetFirstOrDefault(c => c.UserId == id,
                c => c.User);
        }

        public bool GetByUserEmail(string email)
        {
            Customer customer = GetFirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                return false;
            }
            return true;
        }

        public override bool Add(Customer entity)
        {
            entity.User = new User
            {
                Username = entity.Email,
                Password = entity.User.Password,
                IsActive = true,
                IsDeleted = false,
                UserTypeId = (int)UserTypeEnum.Customer
            };
            return base.Add(entity);
        }
    }
}