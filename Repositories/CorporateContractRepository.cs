using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class CorporateContractRepository : CommonRepository<CorporateContract>, ICorporateContractRepository
    {
        public CorporateContractRepository() : base(new FreshContext())
        {
        }
    }
}