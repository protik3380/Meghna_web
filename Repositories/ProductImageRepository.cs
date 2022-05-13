using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class ProductImageRepository: CommonRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository() : base(new FreshContext())
        {
        }
    }
}