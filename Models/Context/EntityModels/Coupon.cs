using System;

namespace EFreshStore.Models.Context.EntityModels
{
    public class Coupon
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> MaximumDiscount { get; set; }
        public Nullable<decimal> MinimumOrderAmount { get; set; }
        public Nullable<System.DateTime> Validity { get; set; }
        public Nullable<int> MaximumOrderNo { get; set; }
        public Nullable<long> UserTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }

        public virtual UserType UserType { get; set; }
    }
}