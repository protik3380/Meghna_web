using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class DistrictManager : CommonManager<District>, IDistrictManager
    {
        public DistrictManager() : base(new DistrictRepository())
        {
        }

        public District GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                && c.IsDeleted.HasValue && !c.IsDeleted.Value);
        }

        public bool DoesDistrictNameExist(string name)
        {
            District district = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower()) 
                && c.IsDeleted.HasValue && !c.IsDeleted.Value);
            return district != null;
        }
    }
}