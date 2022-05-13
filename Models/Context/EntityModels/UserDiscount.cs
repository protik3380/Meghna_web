using System;

namespace EFreshStore.Models.Context.EntityModels
{
    [Serializable]
    public class UserDiscount
    {
        public long Id { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}