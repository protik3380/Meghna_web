using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EFreshStore.Interfaces.Managers;
using EFreshStore.Models.Context;
using EFreshStore.Repositories;

namespace EFreshStore.Managers
{
    public class OrderDetailManager : CommonManager<OrderDetail>, IOrderDetailManager
    {
        public OrderDetailManager() : base(new OrderDetailRepository())
        {
        }
  
        public List<OrderDetail> GetById(long id)
        {
            return (List<OrderDetail>) Get(c => c.Order.UserId == id);
        }

        public ICollection<OrderDetail> GetByOderId(long id)
        {
            List<OrderDetail> detailsList = Get(c => c.OrderId == id,
                c => c.ProductUnit,
                c => c.ProductUnit.ProductImages,
                c => c.ProductUnit.Product).ToList();
            return detailsList;
        }
    }
}