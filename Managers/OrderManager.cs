using System;
using System.Collections.Generic;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class OrderManager : CommonManager<Order>, IOrderManager
    {
        public OrderManager() : base(new OrderRepository())
        {
        }

        public ICollection<Order> GetAll()
        {
            return base.GetAll(c => c.MasterDepot);
        }

        public int CountOrder()
        {
            ICollection<Order> orders = GetAll();
            int count = orders.Count;
            return count;
        }

        public Order GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public List<Order> GetByUserId(long id)
        {
            return (List<Order>)Get(c => c.UserId == id);
        }
    }
}