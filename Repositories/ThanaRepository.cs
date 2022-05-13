using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class ThanaRepository : CommonRepository<Thana>, IThanaRepository
    {
        public ThanaRepository() : base(new FreshContext())
        {
        }
    }
}