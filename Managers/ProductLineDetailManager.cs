using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class ProductLineDetailManager : CommonManager<ProductLineDetail>, IProductLineDetailManager
    {
        public ProductLineDetailManager() : base(new ProductLineDetailRepository())
        {
        }

        public List<ProductLineDetail> GetByProduct(long productId)
        {
            return Get(c => c.ProductId == productId, c => c.ProductLine, c => c.ProductLine.MasterDepotProductLines).ToList();
        }

        public bool IsExistProductLine(ProductLineDetail productLineDetail)
        {
            var productLine = GetFirstOrDefault(c => c.ProductId == productLineDetail.ProductId && c.ProductLineId == productLineDetail.ProductLineId);
            if(productLine!=null)
            {
                return true;
            }
            return false;
        }
    }
}