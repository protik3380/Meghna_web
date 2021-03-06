//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFreshStore.Models.Context
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class CorporateContract
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CorporateContract()
        {
            this.CorporateUsers = new HashSet<CorporateUser>();
        }
    
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public Nullable<System.DateTime> Validity { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CorporateUser> CorporateUsers { get; set; }
        public Nullable<decimal> FMCGDiscountPercentage { get; set; }
        public Nullable<decimal> LPGDiscountPercentage { get; set; }
    }
}
