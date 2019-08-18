using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectNghiPhep.Models;
using System.Data.Entity.Validation;
using ProjectNghiPhep.Email;

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
                var user = db.Users.FirstOrDefault(x => x.username == User.Identity.Name);
                float startDate = (float)(DateTime.ParseExact(dnp.dateStart, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
                float endDate = (float)(DateTime.ParseExact(dnp.dateEnd, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
                int count = (int)((endDate - startDate) / 1000 / 3600 / 24);
                if (user.dayOff <= 0)
                {
                    return RedirectToAction("CreateNew");
                }
                if (count >= user.dayOff)
                {
                    return RedirectToAction("CreateNew");
                }
                var doc = new Document()
                {
                    C_id = code,
                    code = code,
                    status = 0,
                    createdAt = (float)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000,
                    startDate = startDate,
                    endDate = endDate,
                    createdById = user.C_id,
                    reason = dnp.reason
                };
                db.Documents.Add(doc);
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
                //Gửi mail khi tạo đơn, tương tự bên dưới luôn (CancelDocument)
                string mailBody = "Xin chào: " + user.fullName + "<br>"
                                + "Thông tin đơn nghỉ phép của bạn đang chờ duyệt: <br>"
                                + "- Từ ngày: " + dnp.dateStart + "<br>"
                                + "- Đến ngày: " + dnp.dateEnd + "<br>"
                                + "- Lý do: " + dnp.reason + "<br>"
                                + "Xin cảm ơn.";
                var mail = new MailModel
                {
                    ListToEmail = new List<string> { user.email },
                    Body = mailBody,
                    EmailSubject = "Thông tin đơn xin nghỉ phép"
                };
                EmailHelper.SendMail(mail);
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
            var admin = db.Users.FirstOrDefault(x => x.username == User.Identity.Name);
            if (result != null)
            {
                var verifyAt = (float)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
                result.status = 99;
                result.verifiedById = admin.C_id;
                result.verifiedAt = verifyAt;
                db.SaveChanges();
            }
            var user = db.Users.SingleOrDefault(b => b.C_id == result.createdById);
            if (user != null)
            {
                user.dayOff = user.dayOff - (int)((result.endDate - result.startDate) / 1000 / 3600 / 24);
                db.SaveChanges();
            }
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(result.startDate) / 1000d)).ToLocalTime().ToString();
            var end = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(result.endDate) / 1000d)).ToLocalTime().ToString();

            //Gửi mail khi người quản lý approve đơn nghỉ phép (tương tự như hàm dưới CancelDocument)
            string mailBody = "Xin chào: " + user.fullName + "<br>"
                                + "Đơn nghỉ phép của bạn đã được duyệt" + "<br><br>"
                                + "- Mã đơn: " + result.C_id + "<br>"
                                + "- Từ ngày: " + start + "<br>"
                                + "- Đến ngày: " + end + "<br>"
                                + "- Lý do: " + result.reason + "<br>"
                                + "Người duyệt: " + admin.fullName + "<br>"
                                + "Xin cảm ơn.";
            var mail = new MailModel
            {
                ListToEmail = new List<string> { user.email },
                Body = mailBody,
                EmailSubject = "Đơn nghỉ phép được duyệt"
            };
            EmailHelper.SendMail(mail);
            return RedirectToAction("Manager");
        }

        [HttpGet]
        public ActionResult CancelDocument(string id)
        {
            NghiphepEntities db = new NghiphepEntities();
            Document document = queryDocument();
            var result = db.Documents.SingleOrDefault(b => b.C_id == id);
            var admin = db.Users.FirstOrDefault(x => x.username == User.Identity.Name);
            if (result != null)
            {
                var verifyAt = (float)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
                result.status = 100;
                result.verifiedById = admin.C_id;
                result.verifiedAt = verifyAt;
                db.SaveChanges();

                var start = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(result.startDate) / 1000d)).ToLocalTime().ToString();
                var end = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(System.Convert.ToDouble(result.endDate) / 1000d)).ToLocalTime().ToString();
                var user = db.Users.SingleOrDefault(b => b.C_id == result.createdById);
                //Tạo nội dung email, nó là 1 chuỗi dạng html hay string thường đều được
                string mailBody = "Xin chào: " + user.fullName + "<br><br>"
                                    + "Đơn nghỉ phép của bạn không được duyệt" + "<br>"
                                    + "- Mã đơn: " + result.C_id + "<br>"
                                    + "- Từ ngày: " + start + "<br>"
                                    + "- Đến ngày: " + end + "<br>"
                                    + "- Lý do: " + result.reason + "<br>"
                                    + "Người hủy: " + admin.fullName + "<br>"
                                    + "Xin cảm ơn.";
                //Tạo 1 email theo model bên kia (MailModel)
                var mail = new MailModel
                {
                    //Danh sách địa chỉ nhận mail,nếu muốn gửi nhiều email cùng lúc thì thêm dô list này
                    //Ở đây chỉ gửi cho email người tạo đơn
                    ListToEmail = new List<string> { user.email },
                    //Nội dung email
                    Body = mailBody,
                    //Subject email
                    EmailSubject = "Đơn nghỉ phép không được duyệt"
                };
                //Gọi hàm gửi mail
                EmailHelper.SendMail(mail);
            }

            return RedirectToAction("Manager");
        }

    }
}