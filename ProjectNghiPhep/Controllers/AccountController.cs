﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ProjectNghiPhep.Models;
using ProjectNghiPhep.Models.ViewModels;

namespace ProjectNghiPhep.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            //Thêm hàm logout, khi logout thì clear session đi
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckLogin(LoginModel model)
        {
            System.Diagnostics.Debug.WriteLine("UserName ", model.UserName);
            System.Diagnostics.Debug.WriteLine("Password ", model.Password);
            NghiphepEntities db = new NghiphepEntities();
            var query = (from user in db.Users
                         join title in db.Titles
                         on user.titleId equals title.C_id
                         where user.username == model.UserName && user.password == model.Password && user.isActive == true
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
            if (users.Count > 0)
            {
                //Sửa lại hàm login, trong asp.net dùng cái này để login
                //Lưu session login
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                var identity = new GenericIdentity(model.UserName);
                var principal = new GenericPrincipal(identity, new string[0]);
                HttpContext.User = principal;
                Thread.CurrentPrincipal = principal;
                // If we got this far, something failed, redisplay form
                if (users[0].Title.code == "GIAMDOC")
                {
                    return RedirectToAction("Manager/" + users[0].C_id, "DonNghiPhep");
                }
                else
                {
                    return RedirectToAction("Employee/" + users[0].C_id, "DonNghiPhep");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            //NghiphepEntities db = new NghiphepEntities();
            ////contract
            //var contract_query = (from contract in db.ContractTypes
            //                      select new
            //                      {
            //                          C_id = contract.C_id,
            //                          code = contract.code,
            //                          name = contract.name,
            //                          dayOff = contract.dayOff
            //                      });
            //var contracts = contract_query.ToList().Select(c => new ContractType
            //{
            //    C_id = c.C_id,
            //    code = c.code,
            //    name = c.name,
            //    dayOff = c.dayOff
            //}).ToList();
            //ViewData["contracts"] = contracts;
           
            ////department
            //var department_query = (from department in db.Departments
            //                        select new
            //                        {
            //                            C_id = department.C_id,
            //                            code = department.code,
            //                            name = department.name
            //                        });
            //var departments = department_query.ToList().Select(c => new Department
            //{
            //    C_id = c.C_id,
            //    code = c.code,
            //    name = c.name
            //}).ToList();
            //ViewData["departments"] = departments;
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(NHANVIEN nv)
        {

            //Validate dữ liệu, nếu chưa đúng thì nhập lại
            if (!ModelState.IsValid)
            {
                return View(nv);
            }
            using (NghiphepEntities db = new NghiphepEntities())
            {
                var use1 = db.Users.FirstOrDefault(x => x.username == nv.username);
                if (use1 != null)
                {
                    //Nếu username đã tồn tại thì báo lỗi username đã tồn tại
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                    return View(nv);
                }
                //Tạo model user từ thông tin đã nhập để them vào database
                var newUser = new User
                {
                    
                    username = nv.username,
                    address = nv.address,
                    contractId = nv.contractId,
                    email = nv.email,
                    gender = nv.gender,
                    fullName = nv.fullName,
                    isActive = false,
                    password = nv.password,
                    mobile = nv.mobile,
                    departmentId = nv.departmentId,
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
                    return View(nv);
                }
                //Lưu thành công thì chuyển đến tran index
                return RedirectToAction("Index");
            }

            
            //{
            //    C_id = "_User_01",
            //    username = nv.username,
                
            //    address = nv.address,
            //    contractId = nv.contractId,
            //    email = nv.email,
            //    dayOff = 12,
            //    fullName = nv.fullName,
            //    mobile = nv.mobile,
            //    gender = nv.gender,
                
            //    password = nv.password,
            //    departmentId = nv.departmentId
            //};
            //db.Users.Add(tbl);
            //db.SaveChanges();
            //var contract_query = (from contract in db.ContractTypes
            //                      select new
            //                      {
            //                          C_id = contract.C_id,
            //                          code = contract.code,
            //                          name = contract.name,
            //                          dayOff = contract.dayOff
            //                      });
            //var contracts = contract_query.ToList().Select(c => new ContractType
            //{
            //    C_id = c.C_id,
            //    code = c.code,
            //    name = c.name,
            //    dayOff = c.dayOff
            //}).ToList();
            //ViewData["contracts"] = contracts;
          
            //var department_query = (from department in db.Departments
            //                        select new
            //                        {
            //                            C_id = department.C_id,
            //                            code = department.code,
            //                            name = department.name
            //                        });
            //var departments = department_query.ToList().Select(c => new Department
            //{
            //    C_id = c.C_id,
            //    code = c.code,
            //    name = c.name
            //}).ToList();
            //ViewData["departments"] = departments;
            //return View();

            //NghiphepEntities db = new NghiphepEntities();
            //User u = new User
            //{
            //    C_id = "_User_01",
            //    address = nv.address,
            //    contractId = nv.contractId,
            //    email = nv.email,
            //    dayOff = 12,
            //    fullName = nv.fullName
            //};
            //db.Users.Add(u);
            //db.SaveChanges();
            //var contract_query = (from contract in db.ContractTypes
            //                      select new
            //                      {
            //                          C_id = contract.C_id,
            //                          code = contract.code,
            //                          name = contract.name,
            //                          dayOff = contract.dayOff
            //                      });
            //var contracts = contract_query.ToList().Select(c => new ContractType
            //{
            //    C_id = c.C_id,
            //    code = c.code,
            //    name = c.name,
            //    dayOff = c.dayOff
            //}).ToList();
            //ViewData["contracts"] = contracts;
            //return View();

        }

        //    //

        //    //
        //    // POST: /Account/Disassociate

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Disassociate(string provider, string providerUserId)
        //    {
        //        string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
        //        ManageMessageId? message = null;

        //        // Only disassociate the account if the currently logged in user is the owner
        //        if (ownerAccount == User.Identity.Name)
        //        {
        //            // Use a transaction to prevent the user from deleting their last login credential
        //            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
        //            {
        //                bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //                if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
        //                {
        //                    OAuthWebSecurity.DeleteAccount(provider, providerUserId);
        //                    scope.Complete();
        //                    message = ManageMessageId.RemoveLoginSuccess;
        //                }
        //            }
        //        }

        //        return RedirectToAction("Manage", new { Message = message });
        //    }

        //    //
        //    // GET: /Account/Manage

        //    public ActionResult Manage(ManageMessageId? message)
        //    {
        //        ViewBag.StatusMessage =
        //            message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //            : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //            : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //            : "";
        //        ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //        ViewBag.ReturnUrl = Url.Action("Manage");
        //        return View();
        //    }

        //    //
        //    // POST: /Account/Manage

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Manage(LocalPasswordModel model)
        //    {
        //        bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //        ViewBag.HasLocalPassword = hasLocalAccount;
        //        ViewBag.ReturnUrl = Url.Action("Manage");
        //        if (hasLocalAccount)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
        //                bool changePasswordSucceeded;
        //                try
        //                {
        //                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
        //                }
        //                catch (Exception)
        //                {
        //                    changePasswordSucceeded = false;
        //                }

        //                if (changePasswordSucceeded)
        //                {
        //                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // User does not have a local password so remove any validation errors caused by a missing
        //            // OldPassword field
        //            ModelState state = ModelState["OldPassword"];
        //            if (state != null)
        //            {
        //                state.Errors.Clear();
        //            }

        //            if (ModelState.IsValid)
        //            {
        //                try
        //                {
        //                    WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
        //                    return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //                }
        //                catch (Exception)
        //                {
        //                    ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
        //                }
        //            }
        //        }

        //        // If we got this far, something failed, redisplay form
        //        return View(model);
        //    }

        //    //
        //    // POST: /Account/ExternalLogin

        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult ExternalLogin(string provider, string returnUrl)
        //    {
        //        return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //    }

        //    //
        //    // GET: /Account/ExternalLoginCallback

        //    [AllowAnonymous]
        //    public ActionResult ExternalLoginCallback(string returnUrl)
        //    {
        //        AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        //        if (!result.IsSuccessful)
        //        {
        //            return RedirectToAction("ExternalLoginFailure");
        //        }

        //        if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
        //        {
        //            return RedirectToLocal(returnUrl);
        //        }

        //        if (User.Identity.IsAuthenticated)
        //        {
        //            // If the current user is logged in add the new account
        //            OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
        //            return RedirectToLocal(returnUrl);
        //        }
        //        else
        //        {
        //            // User is new, ask for their desired membership name
        //            string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
        //            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
        //            ViewBag.ReturnUrl = returnUrl;
        //            return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        //        }
        //    }

        //    //
        //    // POST: /Account/ExternalLoginConfirmation

        //    [HttpPost]
        //    [AllowAnonymous]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        //    {
        //        string provider = null;
        //        string providerUserId = null;

        //        if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
        //        {
        //            return RedirectToAction("Manage");
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            // Insert a new user into the database
        //            using (UsersContext db = new UsersContext())
        //            {
        //                UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
        //                // Check if user already exists
        //                if (user == null)
        //                {
        //                    // Insert name into the profile table
        //                    db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
        //                    db.SaveChanges();

        //                    OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
        //                    OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

        //                    return RedirectToLocal(returnUrl);
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
        //                }
        //            }
        //        }

        //        ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
        //        ViewBag.ReturnUrl = returnUrl;
        //        return View(model);
        //    }

        //    //
        //    // GET: /Account/ExternalLoginFailure

        //    [AllowAnonymous]
        //    public ActionResult ExternalLoginFailure()
        //    {
        //        return View();
        //    }

        //    [AllowAnonymous]
        //    [ChildActionOnly]
        //    public ActionResult ExternalLoginsList(string returnUrl)
        //    {
        //        ViewBag.ReturnUrl = returnUrl;
        //        return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //    }

        //    [ChildActionOnly]
        //    public ActionResult RemoveExternalLogins()
        //    {
        //        ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
        //        List<ExternalLogin> externalLogins = new List<ExternalLogin>();
        //        foreach (OAuthAccount account in accounts)
        //        {
        //            AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

        //            externalLogins.Add(new ExternalLogin
        //            {
        //                Provider = account.Provider,
        //                ProviderDisplayName = clientData.DisplayName,
        //                ProviderUserId = account.ProviderUserId,
        //            });
        //        }

        //        ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //        return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        //    }

        //    #region Helpers
        //    private ActionResult RedirectToLocal(string returnUrl)
        //    {
        //        if (Url.IsLocalUrl(returnUrl))
        //        {
        //            return Redirect(returnUrl);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    public enum ManageMessageId
        //    {
        //        ChangePasswordSuccess,
        //        SetPasswordSuccess,
        //        RemoveLoginSuccess,
        //    }

        //    internal class ExternalLoginResult : ActionResult
        //    {
        //        public ExternalLoginResult(string provider, string returnUrl)
        //        {
        //            Provider = provider;
        //            ReturnUrl = returnUrl;
        //        }

        //        public string Provider { get; private set; }
        //        public string ReturnUrl { get; private set; }

        //        public override void ExecuteResult(ControllerContext context)
        //        {
        //            OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
        //        }
        //    }

        //    private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        //    {
        //        // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //        // a full list of status codes.
        //        switch (createStatus)
        //        {
        //            case MembershipCreateStatus.DuplicateUserName:
        //                return "User name already exists. Please enter a different user name.";

        //            case MembershipCreateStatus.DuplicateEmail:
        //                return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        //            case MembershipCreateStatus.InvalidPassword:
        //                return "The password provided is invalid. Please enter a valid password value.";

        //            case MembershipCreateStatus.InvalidEmail:
        //                return "The e-mail address provided is invalid. Please check the value and try again.";

        //            case MembershipCreateStatus.InvalidAnswer:
        //                return "The password retrieval answer provided is invalid. Please check the value and try again.";

        //            case MembershipCreateStatus.InvalidQuestion:
        //                return "The password retrieval question provided is invalid. Please check the value and try again.";

        //            case MembershipCreateStatus.InvalidUserName:
        //                return "The user name provided is invalid. Please check the value and try again.";

        //            case MembershipCreateStatus.ProviderError:
        //                return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //            case MembershipCreateStatus.UserRejected:
        //                return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //            default:
        //                return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        //        }
        //    }
        //    #endregion



       
    }
}