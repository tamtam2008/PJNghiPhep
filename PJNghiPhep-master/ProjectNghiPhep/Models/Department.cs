
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
    
public partial class Department
{

    public Department()
    {

        this.Users = new HashSet<User>();

    }


    public string C_id { get; set; }

    public string code { get; set; }

    public string name { get; set; }

    public string createdById { get; set; }

    public Nullable<double> createdAt { get; set; }

    public string managerId { get; set; }



    public virtual User User { get; set; }

    public virtual User User1 { get; set; }

    public virtual ICollection<User> Users { get; set; }

}

}
