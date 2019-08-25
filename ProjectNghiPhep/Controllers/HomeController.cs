using ProjectNghiPhep.Models;
using ProjectNghiPhep.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;

namespace ProjectNghiPhep.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //Lấy dữ liệu đơn nghỉ phép để hiển thị ra dashbord
            using (NghiphepEntities db = new NghiphepEntities())
            {
                
                var user = db.Users.FirstOrDefault(x => x.username == User.Identity.Name);
                var documents = db.Documents.Where(d => d.createdById == user.C_id);
                int dateOff = documents.Sum(d => (int)((d.endDate - d.startDate) / 1000 / 3600 / 24));
                var result = from u in db.Users
                             join d in db.Documents
                             on u.C_id equals d.createdById
                             select new
                             {
                                 DocumentId = d.C_id,
                                 CreateBy = u.fullName,
                                 StartDate = d.startDate,
                                 EndDate = d.endDate,
                                 Status = d.status,
                                 Reason = d.reason,
                                 ApproveOrRejectBy = d.verifiedById,
                                 ApproveOrRejectDate = d.verifiedAt,
                                 usedDayOff = dateOff,
                                 totalDayOff = u.dayOff
                             };
                if (user != null && user.titleId != "TITLE_001")
                {
                    result = from u in db.Users
                             join d in db.Documents
                             on u.C_id equals d.createdById
                             where d.createdById == user.C_id
                             select new
                             {
                                 DocumentId = d.C_id,
                                 CreateBy = u.fullName,
                                 StartDate = d.startDate,
                                 EndDate = d.endDate,
                                 Status = d.status,
                                 Reason = d.reason,
                                 ApproveOrRejectBy = d.verifiedById,
                                 ApproveOrRejectDate = d.verifiedAt,
                                 usedDayOff = dateOff,
                                 totalDayOff = u.dayOff
                             };
                }
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
                        Status = item.Status == 0 ? "đã được nộp" : item.Status == 99 ? "đã được duyệt" : "Đã bị hủy",
                        ApproveOrRejectBy = item.ApproveOrRejectBy,
                        ApproveOrRejectDate = item.ApproveOrRejectDate.HasValue ? convertDoubleToDatetime(item.ApproveOrRejectDate.Value) : "",
                        usedDayOff = item.usedDayOff,
                        totalDayOff = (int)item.totalDayOff,
                        DayOff = (int)(item.totalDayOff - item.usedDayOff)
                    };
                    listData.Add(r);
                }
                return View(listData);
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
