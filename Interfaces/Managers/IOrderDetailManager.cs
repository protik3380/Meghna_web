using EFreshStore.Models.Context;
using System.Collections;
using System.Collections.Generic;

namespace EFreshStore.Interfaces.Managers
{
    public interface IOrderDetailManager : ICommonManager<OrderDetail>
    {
        List<OrderDetail> GetById(long id);
        ICollection<OrderDetail> GetByOderId(long id);
    }
}