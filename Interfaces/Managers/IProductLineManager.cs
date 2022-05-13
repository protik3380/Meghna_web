using System.Collections.Generic;
using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    public interface IProductLineManager : ICommonManager<ProductLine>
    {
        List<ProductLine> GetByProduct(long productId);
        bool IsExistByName(string productLineName);
    }
}