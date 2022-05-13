using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFreshStore.Models.Context.Enum;

namespace EFreshStore.Models.Context.EntityModels
{
    public class LPGComboDiscount
    {
        public long Id { get; set; }
        public int DiscountType { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime Validity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DiscountTypeEnum DiscountTypes { get; set; }
    }
}