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
    
}