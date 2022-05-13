using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    interface IBrandManager : ICommonManager<Brand>
    {
        Brand GetById(long brandId);
        bool DoesBrandNameExist(string name);
        ICollection<Brand> Get();
    }
}
