using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectNghiPhep.Models.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "ID")]
        public string C_id { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Tên đăng nhập")]
        public string username { get; set; }
        public string createdById { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Chức vụ")]
        public string titleId { get; set; }
        public string departmentId { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Loại hợp đồng")]
        public string contractId { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Số ngày phép")]
        public int dayOff { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Tên nhân viên")]
        public string fullName { get; set; }
        [Display(Name = "Địa chỉ")]
        public string address { get; set; }
        public string gender { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Mật khẩu")]
        public string password { get; set; }
        public double? dateOfBirth { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Email")]
        public string email { get; set; }
        public string mobile { get; set; }
        public bool isActive { get; set; }
    }
    
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu xác nhận không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        public string NewPasswordAgain { get; set; }

        public string Username { get; set; }
    }

    public class DashboardViewModel
    {
        public string DocumentId { get; set; }
        public string CreateBy { get; set; }
        public string StartDate { get; set; }
        public string EndDate  { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string ApproveOrRejectBy { get; set; }
        public string ApproveOrRejectDate { get; set; }
        public int DayOff { get; set; }
    };
}