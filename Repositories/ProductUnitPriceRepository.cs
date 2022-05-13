using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace EFreshStore.Repositories
{
    public class ProductUnitPriceRepository : CommonRepository<ProductUnitPrice>, IProductUnitPriceRepository
    {
        public ProductUnitPriceRepository() : base(new FreshContext())
        {
        }

       
    }
}