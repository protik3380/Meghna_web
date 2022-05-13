using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Interfaces.Repositories;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class MasterDepotManager : CommonManager<MasterDepot>, IMasterDepotManager
    {
        public MasterDepotManager() : base(new MasterDepotRepository())
        {
        }

        public MasterDepot GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id, c=> c.ThanaWiseMasterDepots);
        }

        public MasterDepot GetByThanaAndProduct(long thanaId)
        {
            IThanaWiseMasterDepotManager _thanaWiseMasterDepotManager = new ThanaWiseMasterDepotManager();
            IMasterDepotManager masterDepotManager = new MasterDepotManager();

            List<ThanaWiseMasterDepot> thanaWiseMasterDepots;
            thanaWiseMasterDepots = (List<ThanaWiseMasterDepot>) _thanaWiseMasterDepotManager.GetAll();
            List<MasterDepot> masterDepots = new List<MasterDepot>();
            MasterDepot aDepot = new MasterDepot();
            foreach (var thanaWiseMasterDepot in thanaWiseMasterDepots)
            {
                if (thanaWiseMasterDepot.ThanaId == thanaId)
                {
                    masterDepots.Add(masterDepotManager.GetById((long) thanaWiseMasterDepot.MasterDepotId));
                    aDepot = masterDepots.FirstOrDefault();
                    return aDepot;
                }
            }
            return aDepot;
        }
    }
}