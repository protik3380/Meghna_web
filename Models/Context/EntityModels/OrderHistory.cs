using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.Context.EntityModels
{
    [Serializable]
    public class OrderHistory
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long OrderStateId { get; set; }
        public System.DateTime OrderStateChangedOn { get; set; }
        public Nullable<long> OrderStateChangedBy { get; set; }
        public string Remarks { get; set; }

        public virtual Order Order { get; set; }
        public virtual OrderState OrderState { get; set; }
        public virtual User User { get; set; }
    }
}