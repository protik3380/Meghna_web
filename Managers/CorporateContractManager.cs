using System;
using System.Collections.Generic;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class CorporateContractManager : CommonManager<CorporateContract>, ICorporateContractManager
    {
        public CorporateContractManager() : base(new CorporateContractRepository())
        {
        }

        public ICollection<CorporateContract> Get()
        {
            return Get(c => c.IsDeleted.HasValue && !c.IsDeleted.Value);
        }

        public CorporateContract GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id&&!c.IsDeleted.Value);
        }


    }
}