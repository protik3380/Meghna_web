using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;

namespace EFreshStore.Repositories
{
    public class CategoryRepository : CommonRepository<Category>, ICategoryRepository
    {
        public CategoryRepository() : base(new FreshContext())
        {
        }
    }
}