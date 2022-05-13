using EFreshStore.Models.Context;
using System.Collections;
using System.Collections.Generic;

namespace EFreshStore.Interfaces.Managers
{
    public interface ICorporateContractManager : ICommonManager<CorporateContract>
    {
        CorporateContract GetById(long id);
        ICollection<CorporateContract> Get();
        
    }
}