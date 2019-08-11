using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ProjectNghiPhep.Models;

namespace ProjectNghiPhep.Controllers
{
    public class PhanQuyensController : Controller
    {
        //
        // GET: /PhanQuyens/
        dataForPQ.dbpq dbPQ = new dataForPQ.dbpq();

        public ActionResult ShowDataBasePhanQuyen()
        {
            return View();
        }

        public JsonResult get_data() {
            DataSet ds = dbPQ.DataForPhanQuyen();
            List<ShowDataBaseForPhanQuyen> listPQ = new List<ShowDataBaseForPhanQuyen>();
            foreach (DataRow dr in ds.Tables[0].Rows){
                listPQ.Add(new ShowDataBaseForPhanQuyen
                {
                    MaNV = Convert.ToInt32(dr["MaNV"]),
                    TenNV = dr["TenNV"].ToString(),
                    Email = dr["Email"].ToString(),
                    chucvu = dr["chucvu"].ToString(),
                    user_name = dr["user_name"].ToString(),
                    id_per_rel = Convert.ToInt32(dr["id_per_rel"]),
                    name_per = dr["name_per"].ToString()
                });
            }
            return Json(listPQ, JsonRequestBehavior.AllowGet);
        }
	}
}