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
        public ActionResult SetDataInDataBase(User model)
        {
            User tbl = new User();
            tbl.C_id = model.C_id;
            tbl.fullName = model.fullName;
            tbl.gender = model.gender;
            //tbl.dateOfBirth = (float)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
            tbl.dateOfBirth = model.dateOfBirth;
            tbl.address = model.address;
            tbl.email = model.email;
            tbl.mobile = model.mobile;
            tbl.username = model.username;
            tbl.password = model.password;
            /////////////////////////Kiểm tra radiobutton///////////////
            if (model.titleId != null)
            {
                tbl.titleId = "TITLE_001";
            }
            else
            {
                tbl.titleId = "TITLE_002";
            }
            ///////////////////////////////////////////////////////////
            tbl.isActive = true;

            db.Users.Add(tbl);
            db.SaveChanges();
            return View();
        }


        public ActionResult ShowDataBaseForUser(string id)
        {
            //var item = db.Users.ToList();
            //return View(item);
            if (id != null)
            {
                NghiphepEntities db = new NghiphepEntities();
                var query = (from user in db.Users
                             select new
                             {
                                 C_id = user.C_id,
                                 isActive = user.isActive,
                                 titleId = user.titleId,
                                 username = user.username,
                                 fullName = user.fullName,
                                 address = user.address,
                                 gender = user.gender,
                                 email = user.email,
                                 mobile = user.mobile,

                             });
                var users = query.ToList().Select(r => new User
                {
                    C_id = r.C_id,
                    isActive = r.isActive,
                    titleId = r.titleId,
                    username = r.username,
                    fullName = r.fullName,
                    address = r.address,
                    gender = r.gender,
                    email = r.email,
                    mobile = r.mobile,
                }).ToList();
                return View(users[0]);
            }
            else
            {
                return View(new User()
                {
                    username = "default",
                    Title = new Title()
                    {
                        name = "Default"
                    }
                });
            }
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

        [HttpPost]
        public ActionResult Edit(User modl)
        {
            var item = db.Users.Where(x => x.C_id == modl.C_id).First();
            item.C_id = modl.C_id;
            item.fullName = modl.fullName;
            item.gender = modl.gender;
            //item.dateOfBirth = (float)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
            item.dateOfBirth = modl.dateOfBirth;
            item.address = modl.address;
            item.email = modl.email;
            item.mobile = modl.mobile;
            item.username = modl.username;
            item.password = modl.password;

            db.SaveChanges();
            return View();
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
        //[HttpPost]
        //public ActionResult VerifyUser(User id)
        //{
        //    NghiphepEntities db = new NghiphepEntities();
        //    var item = db.Users.Where(x => x.C_id == modl.C_id).First();
        //    var result = db.Users.Where(b => b.C_id == id.C_id).First();
        //    if (result != null)
        //    {
        //        result.isActive = true;
        //        db.SaveChanges();
        //    }
        //    return View();
        //    return RedirectToAction("Index");
        //}

        //private User queryUser()
        //{
        //    NghiphepEntities db = new NghiphepEntities();
        //    var query = (from user in db.Users
        //                 join tit in db.Titles
        //                 on user.titleId equals tit.C_id
        //                 select new
        //                 {
        //                     titleId = user.C_id
        //                     //C_id = user.C_id,
        //                     //createdBy = user,
        //                     //code = doc.code,
        //                     //status = doc.status,
        //                     //createdAt = doc.createdAt
        //                 }).OrderBy(x => x.titleId);
        //    var users = query.ToList().Select(r => new User
        //    {
        //        titleId = r.titleId
        //    }).ToList();
        //    users.Reverse();
        //    return users[0];
        //}
	}
}