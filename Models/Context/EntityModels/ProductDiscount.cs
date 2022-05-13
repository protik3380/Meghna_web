using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.Context.EntityModels
{
    [Serializable]
    public partial class ProductDiscount
    {
        public long Id { get; set; }
        public Nullable<long> ProductUnitId { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<System.DateTime> Validity { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual ProductUnit ProductUnit { get; set; }
    }
}