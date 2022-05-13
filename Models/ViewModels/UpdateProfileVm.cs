using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStore.Models.ViewModels
{
    public class UpdateProfileVm
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string AlternativeMobileNo { get; set; }
        public Nullable<long> CorporateDepartmentId { get; set; }
        public Nullable<long> CorporateDesignationId { get; set; }
        public Nullable<long> MeghnaDepartmentId { get; set; }
        public Nullable<long> MeghnaDesignationId { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
    }
}