using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVCGrid.Models;
using MVCGrid.Web;
using ProjectNghiPhep.Models;
    

namespace ProjectNghiPhep.Controllers
{
    public class RegistersController : Controller
    {
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        NghiphepEntities db = new NghiphepEntities();
        //
        // GET: /Registers/
        public ActionResult SetDataInDataBase()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SetDataInDataBase(NHANVIEN model)
        {
            User tbl = new User();
            tbl.fullName = model.fullName;
            tbl.gender = model.gender;
            tbl.dateOfBirth = (float)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
            tbl.address = model.address;
            tbl.email = model.email;
            tbl.mobile = model.mobile;
            tbl.username = model.username;
            tbl.password = model.password;
            //tbl.chucvu = "";

            db.Users.Add(tbl);
            db.SaveChanges();
            return View();
        }


        public ActionResult ShowDataBaseForUser()
        {
            var item = db.Users.ToList();
            return View(item);
        }

        public ActionResult Delete(string id)
        {
            var item = db.Users.Where(x => x.C_id == id).First();
            db.Users.Remove(item);
            db.SaveChanges();
            var item2 = db.Users.ToList();
            return View("ShowDataBaseForUser", item2);
        }

        public ActionResult Edit(string id)
        {
            var item = db.Users.Where(x => x.C_id == id).First();
            return View(item);
        }
        //[HttpPost]


        ////////////////////////////////
        //[HttpPost]
        //public ActionResult PhanQuyen(tbl_per_relationship model)
        //{
        //    tbl_per_relationship tbl2 = new tbl_per_relationship();
        //    tbl2.id_user_rel = model.id_user_rel;
        //    tbl2.id_per_rel = model.id_per_rel;

        //    db.tbl_per_relationship.Add(tbl2);
        //    db.SaveChanges();
        //    return View();
        //}
        
        //public ActionResult PhanQuyen()
        //{
        //   
        //    return View();
        //}

        //public ActionResult ShowDataBaseForPhanQuyen()
        //{
        //    SqlCommand cmd = new SqlCommand("SELECT [MaNV],[TenNV],[Email] ,[chucvu],[user_name],[id_per_rel],[name_per] FROM [dbo].[NHANVIEN] NV INNER JOIN [dbo].[tbl_per_relationship] RL ON NV.MaNV = RL.id_user_rel INNER JOIN [dbo].[tbl_permision] PM ON RL.id_per_rel = PM.id_per WHERE NV.MaNV = RL.id_user_rel", con);

        //    return View();
        //}

	}
}