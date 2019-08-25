using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace ProjectNghiPhep.Models
{
    public class TaoDonNghiPhep
    {
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Ngày bắt đầu")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string dateStart { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Ngày kết thúc")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string dateEnd { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Lý do")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string reason { get; set; }
    }
}