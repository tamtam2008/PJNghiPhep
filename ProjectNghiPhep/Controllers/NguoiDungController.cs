using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVCGrid.Models;
using MVCGrid.Web;
using ProjectNghiPhep.Models;
using ProjectNghiPhep.Models.ViewModels;

namespace ProjectNghiPhep.Controllers
{
    public class NguoiDungController : Controller
    {
        //
        // GET: /DonNghiPhep/
           
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            var result = new QueryResult<User>();
            NghiphepEntities db = new NghiphepEntities();
            // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
            // Use Entity Framework, a module from your IoC Container, or any other method.
            // Return QueryResult object containing IEnumerable<YouModelItem>
            var query = (from user in db.Users
                         join contract in db.ContractTypes
                         on user.contractId equals contract.C_id
                         where user.C_id == id
                         select new
                         {
                             C_id = user.C_id,
                             username = user.username,
                             dayOff = user.dayOff,
                             contractId = user.contractId,
                             createdAt = user.createdAt,
                             ContractType = contract,
                             fullName = user.fullName,
                             email = user.email,
                             mobile = user.mobile
                         });
            var user_result = query.ToList().Select(r => new User
            {
                C_id = r.C_id,
                username = r.username,
                fullName = r.fullName,
                dayOff = r.dayOff,
                createdAt = r.createdAt,
                contractId = r.ContractType.name,
                ContractType = r.ContractType,
                email = r.email,
                mobile = r.mobile
            }).ToList()[0];

            var contract_query = (from contract in db.ContractTypes
                         select new
                         {
                             C_id = contract.C_id,
                             code = contract.code,
                             name = contract.name,
                             dayOff = contract.dayOff
                         });
            var contracts = contract_query.ToList().Select(c => new ContractType
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name,
                dayOff = c.dayOff
            }).ToList();
            var department_query = (from department in db.Departments
                                  select new
                                  {
                                      C_id = department.C_id,
                                      code = department.code,
                                      name = department.name
                                  });
            var departments = department_query.ToList().Select(c => new Department
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name
            }).ToList();
            ViewData["contracts"] = contracts;
            ViewData["departments"] = departments; 
            return View(user_result);
        }

        public ActionResult Profile(string id)
        {
            var result = new QueryResult<User>();
            NghiphepEntities db = new NghiphepEntities();
            // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
            // Use Entity Framework, a module from your IoC Container, or any other method.
            // Return QueryResult object containing IEnumerable<YouModelItem>
            var query = (from user in db.Users
                         join contract in db.ContractTypes
                         on user.contractId equals contract.C_id
                         where user.C_id == id
                         select new
                         {
                             C_id = user.C_id,
                             username = user.username,
                             dayOff = user.dayOff,
                             contractId = user.contractId,
                             createdAt = user.createdAt,
                             ContractType = contract,
                             fullName = user.fullName,
                             email = user.email,
                             mobile = user.mobile
                         });
            var user_result = query.ToList().Select(r => new User
            {
                C_id = r.C_id,
                username = r.username,
                fullName = r.fullName,
                dayOff = r.dayOff,
                createdAt = r.createdAt,
                contractId = r.ContractType.name,
                ContractType = r.ContractType,
                email = r.email,
                mobile = r.mobile
            }).ToList()[0];

            var contract_query = (from contract in db.ContractTypes
                                  select new
                                  {
                                      C_id = contract.C_id,
                                      code = contract.code,
                                      name = contract.name,
                                      dayOff = contract.dayOff
                                  });
            var contracts = contract_query.ToList().Select(c => new ContractType
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name,
                dayOff = c.dayOff
            }).ToList();
            var department_query = (from department in db.Departments
                                    select new
                                    {
                                        C_id = department.C_id,
                                        code = department.code,
                                        name = department.name
                                    });
            var departments = department_query.ToList().Select(c => new Department
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name
            }).ToList();
            var titles_query = (from contract in db.Titles
                                select new
                                {
                                    C_id = contract.C_id,
                                    code = contract.code,
                                    name = contract.name
                                });
            var titles = titles_query.ToList().Select(c => new Title
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name,
            }).ToList();
            ViewData["contracts"] = contracts;
            ViewData["departments"] = departments;
            ViewData["titles"] = titles;
            return View(user_result);
        }

        public ActionResult DeleteUser(string id)
        {
            NghiphepEntities db = new NghiphepEntities();
            var result = db.Users.SingleOrDefault(b => b.C_id == id);
            if (result != null)
            {
                result.isActive = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult SaveUser (User user)
        {
            NghiphepEntities db = new NghiphepEntities();
            var result = db.Users.SingleOrDefault(b => b.username == user.username);
            if (result != null)
            {
                result.username = user.username;
                result.fullName = user.fullName;
                result.email = user.email;
                result.mobile = user.mobile;
                result.departmentId = user.departmentId;
                result.titleId = user.titleId;
                result.contractId = user.contractId;
                //result.contractId = user.contractId;
                result.dayOff = user.dayOff;
                db.SaveChanges();
            }
            return RedirectToAction("Edit/" + result.C_id);
        }

        //Gọi form thêm người dùng
        public ActionResult CreateUser()
        {
            NghiphepEntities db = new NghiphepEntities();
            var contract_query = (from contract in db.ContractTypes
                                  select new
                                  {
                                      C_id = contract.C_id,
                                      code = contract.code,
                                      name = contract.name,
                                      dayOff = contract.dayOff
                                  });
            var contracts = contract_query.ToList().Select(c => new ContractType
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name,
                dayOff = c.dayOff
            }).ToList();
            var department_query = (from department in db.Departments
                                    select new
                                    {
                                        C_id = department.C_id,
                                        code = department.code,
                                        name = department.name
                                    });
            var departments = department_query.ToList().Select(c => new Department
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name
            }).ToList();
            var titles_query = (from contract in db.Titles
                                select new
                                {
                                    C_id = contract.C_id,
                                    code = contract.code,
                                    name = contract.name
                                });
            var titles = titles_query.ToList().Select(c => new Title
            {
                C_id = c.C_id,
                code = c.code,
                name = c.name,
            }).ToList();
            ViewData["contracts"] = contracts;
            ViewData["departments"] = departments;
            ViewData["titles"] = titles;
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
                while (resultString.Length < 6)
                {
                    resultString = "0" + resultString;
                }
                return "USER_" + resultString;
            }
            else
            {
                return "USER_000001";
            }
        }

        private User queryUser()
        {
            NghiphepEntities db = new NghiphepEntities();
            var query = (from user in db.Users
                         select new
                         {
                             C_id = user.C_id,
                         }).OrderBy(x => x.C_id);
            var users = query.ToList().Select(r => new User
            {
                C_id = r.C_id
            }).ToList();
            users.Reverse();
            return users.Count > 0 ? users[0] : null;
        }

        //Tạo người dùng từ thông tin đc post lên
        //Nhớ thêm email đúng để nhận đc mail
        [HttpPost]
        public ActionResult CreateUserForm(NHANVIEN nv)
        {
            NghiphepEntities db = new NghiphepEntities();
            User user = queryUser();
            User u = new User
            {
                C_id = CreateAutoCode(user != null ? user.C_id : null),
                username = nv.username,
                password = nv.password,
                dateOfBirth = 0,
                address = nv.address,
                contractId = nv.contractId,
                titleId = nv.titleId,
                departmentId = nv.departmentId,
                email = nv.email,
                mobile = nv.mobile,
                gender = nv.gender,
                dayOff = 12,
                fullName = nv.fullName,
                isActive = true,
            };
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //Đổi mật khẩu
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            //validate
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //validate
            if (model.NewPassword != model.NewPasswordAgain)
            {
                ModelState.AddModelError("", "Mật khẩu mới được nhập lại không khớp.");
                return View(model);
            }
            //Đổi trong database
            using (NghiphepEntities db = new NghiphepEntities())
            {
                var user = db.Users.SingleOrDefault(x => x.username == User.Identity.Name && x.password == model.CurrentPassword);
                if (user != null)
                {
                    user.password = model.NewPassword;
                    db.SaveChanges();
                    ViewBag.MessageSuccess = "Mật khẩu đã được đổi thành công.";
                    return View(model);
                }
                ModelState.AddModelError("", "Mật khẩu không đúng.");
                return View(model);
            }
        }
    }
}

