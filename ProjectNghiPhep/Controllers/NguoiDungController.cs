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
            ViewData["contracts"] = contracts; 
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
            ViewData["contracts"] = contracts;
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
                //result.contractId = user.contractId;
                result.dayOff = user.dayOff;
                db.SaveChanges();
            }
            return RedirectToAction("Edit/" + result.C_id);
        }

        //Gọi form thêm người dùng
        public ActionResult CreateUser()
        {
            return View();
        }
        //Tạo người dùng từ thông tin đc post lên
        //Nhớ thêm email đúng để nhận đc mail
        [HttpPost]
        public ActionResult CreateUser(UserViewModel user)
        {
            //Validate dữ liệu, nếu chưa đúng thì nhập lại
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            using (NghiphepEntities db = new NghiphepEntities())
            {
                //Nếu id đã tồn tại thì báo lỗi id đã tồn tại
                var use = db.Users.FirstOrDefault(x => x.C_id == user.C_id);
                if (use != null)
                {
                    ModelState.AddModelError("", "ID đã tồn tại");
                    return View(user);
                }
                var use1 = db.Users.FirstOrDefault(x => x.username == user.username);
                if (use1 != null)
                {
                    //Nếu username đã tồn tại thì báo lỗi username đã tồn tại
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                    return View(user);
                }
                //Tạo model user từ thông tin đã nhập để them vào database
                var newUser = new User
                {
                    C_id = user.C_id,
                    username = user.username,
                    address = user.address,
                    contractId = user.contractId,
                    email = user.email,
                    gender = user.gender,
                    fullName = user.fullName,
                    isActive = true,
                    dayOff = user.dayOff,
                    password = user.password,
                    mobile = user.mobile,
                    titleId = user.titleId,
                    departmentId = user.departmentId,
                    dateOfBirth = user.dateOfBirth,

                };
                try
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //Lưu thất bại báo lỗi (khi không kết nối đc database)
                    ModelState.AddModelError("", "Lỗi hệ thống");
                    return View(user);
                }
                //Lưu thành công thì chuyển đến tran index
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.NewPassword != model.NewPasswordAgain)
            {
                ModelState.AddModelError("", "Mật khẩu mới được nhập lại không khớp.");
                return View(model);
            }

            using (NghiphepEntities db = new NghiphepEntities())
            {
                if (User != null && User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name)) {
                    model.Username = User.Identity.Name;
                }
                var user = db.Users.SingleOrDefault(x => x.username == model.Username  && x.password == model.CurrentPassword);
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
