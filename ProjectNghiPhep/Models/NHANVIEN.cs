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
        [Display(Name = "Họ và tên")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Giới tính")]
        public string gender { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Địa chỉ")]
        public string address { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Số điện thoại")]
        public string mobile { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public string chucvu { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Loại hợp đồng")]
        public string contractId { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Mã nhân viên")]
        
        public string username { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Mật khẩu")]
        public string password { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Chức vụ")]
        public string titleId { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Phòng ban")]
        public string departmentId { get; set; }
    
    }
}
