//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectNghiPhep.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_permision_detail
    {
        public int id_pd { get; set; }
        public string code_action { get; set; }
        public Nullable<int> id_per { get; set; }
    
        public virtual tbl_permision tbl_permision { get; set; }
    }
}