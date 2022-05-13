using System;
using System.Collections.Generic;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class BrandManager : CommonManager<Brand>, IBrandManager
    {
        public BrandManager() : base(new BrandRepository())
        {
        }

        public Brand GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                && c.IsDeleted.HasValue && !c.IsDeleted.Value
                && c.IsActive.HasValue && c.IsActive.Value);
        }

        public bool DoesBrandNameExist(string name)
        {
            Brand brand = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())
                && c.IsDeleted.HasValue && !c.IsDeleted.Value);
            return brand != null;
        }

        public ICollection<Brand> Get()
        {
            return Get(c => c.IsDeleted.HasValue && !c.IsDeleted.Value
                && c.IsActive.HasValue && c.IsActive.Value);
        }
    }
}