using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class DistrictRepository: CommonRepository<District>, IDistrictRepository
    {
        public DistrictRepository() : base(new FreshContext())
        {
        }
    }
}