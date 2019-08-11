using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace ProjectNghiPhep.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NHANVIEN
    {
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string C_id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ và tên")]
        public string fullName { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string email { get; set; }
 
        public string gender { get; set; }

        public string address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string mobile { get; set; }

        public Nullable<System.DateTime> dateOfBirth { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string username { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        public string password { get; set; }

        public string isActive { get; set; }
    }
}
