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
    
    public partial class CartDetail
    {
        public int CartDetailsId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<int> ImageID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string PrimaryEmail { get; set; }
        public string CurrentSessionId { get; set; }
    }
}
