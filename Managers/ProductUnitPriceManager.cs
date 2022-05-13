using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Managers
{
    public class ProductUnitPriceManager : CommonManager<ProductUnitPrice>, IProductUnitPriceManager
    {
        public ProductUnitPriceManager() : base(new ProductUnitPriceRepository())
        {
        }
    }
}