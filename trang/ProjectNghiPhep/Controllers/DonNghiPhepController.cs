using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectNghiPhep.Models;
using System.Data.Entity.Validation;

namespace ProjectNghiPhep.Controllers
{
    public class DonNghiPhepController : Controller
    {
        //
        // GET: /DonNghiPhep/

        public ActionResult Manager(string id)
        {
            if (id != null)
            {
                NghiphepEntities db = new NghiphepEntities();
                var query = (from user in db.Users
                             join title in db.Titles
                             on user.titleId equals title.C_id
                             where user.C_id == id
                             select new
                             {
                                 C_id = user.C_id,
                                 title = title,
                                 username = user.username
                             });
                var users = query.ToList().Select(r => new User
                {
                    C_id = r.C_id,
                    Title = r.title,
                    username = r.username
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

        public ActionResult Employee(string id)
        {
            if (id != null)
            {
                NghiphepEntities db = new NghiphepEntities();
                var query = (from user in db.Users
                             join title in db.Titles
                             on user.titleId equals title.C_id
                             where user.C_id == id
                             select new
                             {
                                 C_id = user.C_id,
                                 title = title,
                                 username = user.username
                             });
                var users = query.ToList().Select(r => new User
                {
                    C_id = r.C_id,
                    Title = r.title,
                    username = r.username
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

        public ActionResult CreateNew()
        {
            return View();
        }

        public ActionResult ViewDocument(string id)
        {
            NghiphepEntities db = new NghiphepEntities();
            var query = (from doc in db.Documents
                         join user in db.Users
                         on doc.createdById equals user.C_id
                         where doc.C_id == id
                         select new
                         {
                             C_id = doc.C_id,
                             createdBy = user,
                             code = doc.code,
                             status = doc.status,
                             createdAt = doc.createdAt,
                             startDate = doc.startDate,
                             endDate = doc.endDate,
                             reason = doc.reason
                         }).OrderBy(x => x.code);
            var documents = query.ToList().Select(r => new Document
            {
                C_id = r.C_id,
                createdBy = r.createdBy,
                code = r.code,
                status = r.status,
                reason = r.reason,
                createdAt = r.createdAt,
                startDateString = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(r.startDate) / 1000d)).ToLocalTime().ToString(),
                endDateString = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(r.endDate) / 1000d)).ToLocalTime().ToString(),
                count = (int)((r.endDate - r.startDate) / 1000 / 3600 / 24) == 0 ? "1 ngày" : ((int)((r.endDate - r.startDate) / 1000 / 3600 / 24)).ToString() + " ngày"
            }).ToList();
            documents.Reverse();
            return View(documents[0]);
        }

        private string CreateAutoCode(string currentCode)
        {
            if (currentCode != null)
            {
                string resultString = "";
                string numberString = currentCode.Split('_')[1];
                int number = Int32.Parse(numberString) + 1;
                resultString = number.ToString();
                while (resultString.Length < 6)
                {
                    resultString = "0" + resultString;
                }
                return "NP_" + resultString;
            }
            else
            {
                return "NP_000001";
            }
        }

        [HttpPost]
        public ActionResult CreateLeaveForm(TaoDonNghiPhep dnp)
        {
            System.Diagnostics.Debug.WriteLine("CreateLeaveForm CreateLeaveForm CreateLeaveForm ", dnp.reason);
            if (ModelState.IsValid)
            {
                NghiphepEntities db = new NghiphepEntities();
                Document document = queryDocument();
                string code = CreateAutoCode(document != null ? document.code : null);
                db.Documents.Add(new Document()
                {
                    C_id = code,
                    code = code,
                    status = 0,
                    createdAt = (float)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000,
                    startDate = (float)(DateTime.ParseExact(dnp.dateStart, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000,
                    endDate = (float)(DateTime.ParseExact(dnp.dateEnd, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000,
                    createdById = "USER_002",
                    reason = dnp.reason
                });
                //try
                //{
                db.SaveChanges();
                //}
                //catch (DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        System.Diagnostics.Debug.WriteLine(eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //}
                //write code to update student 
                return RedirectToAction("Employee");
            }
            return RedirectToAction("CreateNew");
        }

        private Document queryDocument()
        {
            NghiphepEntities db = new NghiphepEntities();
            var query = (from doc in db.Documents
                         join user in db.Users
                         on doc.createdById equals user.C_id
                         select new
                         {
                             C_id = doc.C_id,
                             createdBy = user,
                             code = doc.code,
                             status = doc.status,
                             createdAt = doc.createdAt
                         }).OrderBy(x => x.code);
            var documents = query.ToList().Select(r => new Document
            {
                C_id = r.C_id,
                createdBy = r.createdBy,
                code = r.code,
                status = r.status,
                createdAt = r.createdAt
            }).ToList();
            documents.Reverse();
            return documents.Count > 0 ? documents[0] : null;
        }

        [HttpGet]
        public ActionResult VerifyDocument(string id)
        {
            NghiphepEntities db = new NghiphepEntities();
            Document document = queryDocument();
            var result = db.Documents.SingleOrDefault(b => b.C_id == id);
            if (result != null)
            {
                result.status = 99;
                db.SaveChanges();
            }
            var user = db.Users.SingleOrDefault(b => b.C_id == result.createdById);
            if (user != null)
            {
                user.dayOff = user.dayOff - (int)((result.endDate - result.startDate) / 1000 / 3600 / 24);
                db.SaveChanges();
            }
            return RedirectToAction("Manager");
        }

        [HttpGet]
        public ActionResult CancelDocument(string id)
        {
            NghiphepEntities db = new NghiphepEntities();
            Document document = queryDocument();
            var result = db.Documents.SingleOrDefault(b => b.C_id == id);
            if (result != null)
            {
                result.status = 100;
                db.SaveChanges();
            }
            return RedirectToAction("Manager");
        }

    }
}