using EFreshStore.Models.Context;

namespace EFreshStore.Interfaces.Managers
{
    interface IDistrictManager: ICommonManager<District>
    {
        District GetById(long districtId);
        bool DoesDistrictNameExist(string name);
    }
}
