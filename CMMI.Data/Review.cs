//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMMI.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public short Rating { get; set; }
        public string Comment { get; set; }
        public bool Approved { get; set; }
        public System.Guid UserGuid { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual Restaurant Restaurant { get; set; }
    }
}
