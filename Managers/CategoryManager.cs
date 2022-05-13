using System;
using System.Collections.Generic;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class CategoryManager : CommonManager<Category>, ICategoryManager
    {
        public CategoryManager() : base(new CategoryRepository())
        {
        }

        public Category GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                                          && !c.IsDeleted.Value 
                                          && c.IsActive.Value);
        }

        public bool DoesCategoryNameExist(string name)
        {
            Category category = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower()) 
                                                       && !c.IsDeleted.Value);
            return category != null;
        }

        public ICollection<Category> GetAll()
        {
            return Get(c => c.IsActive.HasValue 
            && c.IsActive.Value 
            && c.IsDeleted.HasValue
            && !c.IsDeleted.Value);
        }
    }
}