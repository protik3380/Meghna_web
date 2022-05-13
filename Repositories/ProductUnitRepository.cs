using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class ProductUnitRepository: CommonRepository<ProductUnit>, IProductUnitRepository
    {
        public ProductUnitRepository() : base(new FreshContext())
        {
        }
    }
}