using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.Context.EntityModels
{
    [Serializable]
    public class DeliveryCharge
    {
        public long Id { get; set; }
        public decimal FMCGDeliveryCharge { get; set; }
        public decimal LPGDeliveryCharge { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}