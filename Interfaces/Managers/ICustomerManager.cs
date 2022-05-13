using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
   public interface ICustomerManager : ICommonManager<Customer>
    {
        Customer GetById(long id);
        Customer GetByUserId(long id);
        bool GetByUserEmail(string email);
    }
}
