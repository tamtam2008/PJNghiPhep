using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectNghiPhep.Models
{
    public class ShowDataBaseForPhanQuyen
    {
        public int MaNV {get; set;}
        public string TenNV { get; set; }
        public string Email { get; set; }
        public string chucvu { get; set; }
        public string user_name { get; set; }
        public int id_per_rel { get; set; }
        public string name_per { get; set; }
    }
}