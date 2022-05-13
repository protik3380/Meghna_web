using EFreshStore.Models.Context;
using System.Collections.Generic;

namespace EFreshStore.Interfaces.Managers
{
    public interface IOrderManager : ICommonManager<Order>
    {
        ICollection<Order> GetAll();
        List<Order> GetByUserId(long id);
        int CountOrder();
        Order GetById(long id);
    }
}