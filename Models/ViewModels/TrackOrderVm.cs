using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Models.Context.EntityModels;

namespace EFreshStore.Models.ViewModels
{
    public class TrackOrderVm
    {
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public long? CurrentOrderStateId { get; set; }
        public ICollection<OrderHistory> OrderHistories { get; set; }
        public string DeliveryManName { get; set; }
        public string DeliveryManMobile { get; set; }
        public string DeliveryManEmail { get; set; }
        public string DeliveryManImageUrl { get; set; }

    }
}