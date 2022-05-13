using System.Collections.Generic;
using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface IThanaManager : ICommonManager<Thana>
    {
        Thana GetById(long id);
        ICollection<Thana> GetByDistrictId(long id);
    }
}