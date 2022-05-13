using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{

    public class OrderRepository : CommonRepository<Order>, IOrderRepository
    {
        public OrderRepository() : base(new FreshContext())
        {
        }
    }
}