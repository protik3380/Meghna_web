using System.Collections.Generic;
using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface IProductLineDetailManager : ICommonManager<ProductLineDetail>
    {
        List<ProductLineDetail> GetByProduct(long productId);
        bool IsExistProductLine(ProductLineDetail productLineDetail);
        
    }
}