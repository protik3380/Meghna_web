using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.Context.EntityModels
{
    [Serializable]
    public class Cart
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProductUnitId { get; set; }
        public int Quantity { get; set; }
        public System.DateTime AddedOn { get; set; }
        public virtual ProductUnit ProductUnit { get; set; }
        public virtual User User { get; set; }
    }
}