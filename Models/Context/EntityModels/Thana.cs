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
    public partial class Thana
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Thana()
        {
            this.Distributors = new HashSet<Distributor>();
            this.Orders = new HashSet<Order>();
            this.ThanaWiseMasterDepots = new HashSet<ThanaWiseMasterDepot>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<long> DistrictId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public virtual ICollection<Distributor> Distributors { get; set; }
        public virtual District District { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThanaWiseMasterDepot> ThanaWiseMasterDepots { get; set; }
    }
}
