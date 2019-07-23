using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectNghiPhep.Models;

namespace ProjectNghiPhep.Controllers
{
    public class DonNghiPhepController : Controller
    {
        //
        // GET: /DonNghiPhep/
           
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNew()
        {
            return View();
        }

        private string CreateAutoCode(string currentCode)
        {
            if (currentCode != null)
            {
                string resultString = "";
                string numberString = currentCode.Split('_')[1];
                int number = Int32.Parse(numberString) + 1;
                resultString = number.ToString();
                while(resultString.Length < 6) {
                    resultString = "0" + resultString;
                }
                return "NP_" + resultString;
            } else {
                return "NP_000001";
            }
        }

        [HttpPost]
        public ActionResult CreateLeaveForm(TaoDonNghiPhep dnp)
        {
            System.Diagnostics.Debug.WriteLine("CreateLeaveForm CreateLeaveForm CreateLeaveForm");
            if (ModelState.IsValid)
            {
                NghiphepEntities db = new NghiphepEntities();
                Document document = queryDocument();
                string code = CreateAutoCode(document.code);
                db.Documents.Add(new Document()
                {
                    C_id = code,
                    code = code,
                    status = 0,
                    createdAt = (float)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000,
                    startDate = (float)(DateTime.ParseExact(dnp.dateStart, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000,
                    endDate = (float)(DateTime.ParseExact(dnp.dateEnd, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000,
                    createdById = "USER_002"
                });
                db.SaveChanges();
                //write code to update student 
                return RedirectToAction("Index");
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
            return documents[0];
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
            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

    }
}
