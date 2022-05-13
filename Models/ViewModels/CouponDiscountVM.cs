using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.ViewModels
{
    [Serializable]
    public class CouponDiscountVM
    {
        public long CouponId { get; set; }
        public string CouponCode { get; set; }
        public double FinalCouponDiscount { get; set; }
        public double TotalUpdatedPrice { get; set; }
        public long UserTypeId { get; set; }
        public long UserId { get; set; }
        public double GrandTotal { get; set; }
    }
}