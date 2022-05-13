using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class ProductLineManager : CommonManager<ProductLine>, IProductLineManager
    {
        public ProductLineManager() : base(new ProductLineRepository())
        {
        }

        public bool IsExistByName(string productLineName)
        {
            ProductLine productLine = GetFirstOrDefault(c => c.Name== productLineName);
            if(productLine!=null)
            {
                return true;
            }
            return false;
        }

        public List<ProductLine> GetByProduct(long productId)
        {
            IProductLineDetailManager productLineDetailManager = new ProductLineDetailManager();
            List<ProductLineDetail> productLineDetails = productLineDetailManager.GetByProduct(productId);

            var productLines = new List<ProductLine>();
            foreach (ProductLineDetail productLineDetail in productLineDetails)
            {
                productLines.Add(productLineDetail.ProductLine);
            }

            return productLines;
        }
    }
}