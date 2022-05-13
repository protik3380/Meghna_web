using EFreshStore.Models.Context;
using System.Collections;
using System.Collections.Generic;

namespace EFreshStore.Interfaces.Managers
{
    public interface ICategoryManager : ICommonManager<Category>
    {
        ICollection<Category> GetAll();
        Category GetById(long id);
        bool DoesCategoryNameExist(string name);
    }
}