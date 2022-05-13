using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class ThanaManager : CommonManager<Thana>, IThanaManager
    {
        public ThanaManager() : base(new ThanaRepository())
        {
        }

        public Thana GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                && c.IsDeleted.HasValue && !c.IsDeleted.Value);
        }

        public ICollection<Thana> GetByDistrictId(long id)
        {
            return Get(c => c.DistrictId == id
                && c.IsDeleted.HasValue && !c.IsDeleted.Value);
        }
    }
}