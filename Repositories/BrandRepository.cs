using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class BrandRepository : CommonRepository<Brand>, IBrandRepository
    {
        public BrandRepository() : base(new FreshContext())
        {
        }
    }
}