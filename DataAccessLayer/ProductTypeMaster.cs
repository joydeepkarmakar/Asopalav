//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductTypeMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductTypeMaster()
        {
            this.ProductMasters = new HashSet<ProductMaster>();
        }
    
        public int ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> Creation_Date { get; set; }
        public Nullable<int> ParentProductTypeId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMaster> ProductMasters { get; set; }
    }
}
