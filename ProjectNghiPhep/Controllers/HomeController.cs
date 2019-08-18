using ProjectNghiPhep.Models;
using ProjectNghiPhep.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectNghiPhep.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //Lấy dữ liệu đơn nghỉ phép để hiển thị ra dashbord
            using (NghiphepEntities db = new NghiphepEntities())
            {
                var user = db.Users.FirstOrDefault(x => x.username == User.Identity.Name);
                var result = from u in db.Users
                             join d in db.Documents
                             on u.C_id equals d.createdById
                             where user.titleId == "TITLE_001" || (user.titleId == "TITLE_002" && d.createdById == user.C_id) //phân biệt dữ liệu là của nv hay admin
                             select new
                             {
                                 Name = u.fullName,
                                 DocumentId = d.C_id,
                                 CreateBy = u.fullName,
                                 StartDate = d.startDate,
                                 EndDate = d.endDate,
                                 Status = d.status,
                                 Reason = d.reason,
                                 ApproveOrRejectBy = d.verifiedById,
                                 ApproveOrRejectDate = d.verifiedAt,
                                 DayOff = u.dayOff
                             };
                var listData = new List<DashboardViewModel>();
                foreach (var item in result)
                {
                    var r = new DashboardViewModel
                    {
                        DocumentId = item.DocumentId,
                        CreateBy = item.CreateBy,
                        StartDate = item.StartDate.HasValue ? convertDoubleToDatetime(item.StartDate.Value) : "",
                        EndDate = item.StartDate.HasValue ? convertDoubleToDatetime(item.EndDate.Value) : "",
                        Reason = item.Reason,
                        Status = item.Status == 0 ? "Chờ duyệt" : item.Status == 99 ? "Đã duyệt" : "Đã hủy",
                        ApproveOrRejectBy = item.ApproveOrRejectBy,
                        ApproveOrRejectDate = item.ApproveOrRejectDate.HasValue ? convertDoubleToDatetime(item.ApproveOrRejectDate.Value) : "",
                        DayOff = item.DayOff.HasValue ? item.DayOff.Value : 0,
                    };
                    //Lấy số ngày phép được duyệt theo user
                    var totalDayOfApproved = result.Where(x => x.Name.ToUpper() == item.CreateBy.ToUpper() && x.Status == 99).Count();
                    //Tổng số ngày phép được cấp = Tổng số ngày phép còn lại + số ngày được duyệt (Do database thiết kế không có lưu số ngày phép được cấp ban đầu) 
                    r.TotalDay = r.DayOff + totalDayOfApproved;
                    listData.Add(r);
                }
                return View(listData);//đổ dữ liệu ra view
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private string convertDoubleToDatetime(double date)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(date) / 1000d)).ToLocalTime().ToString();
        }

    }
}
