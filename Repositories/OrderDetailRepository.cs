using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class OrderDetailRepository : CommonRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository() : base(new FreshContext())
        {
        }
    }
}