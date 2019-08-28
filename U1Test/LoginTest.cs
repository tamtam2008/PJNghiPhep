using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using ProjectNghiPhep.Models;
using ProjectNghiPhep.Controllers;
using System.Collections.Generic;

namespace U1Test
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void TestLogin()
        {
            AccountController u = new AccountController();
            ViewResult r = u.Login() as ViewResult;
            Assert.IsNotNull(r);
        }
        
        //
        //[TestMethod]
        //public void TestUnameInvalid()
        //{
        //    AccountController controller = new AccountController();
        //    controller.ViewData.ModelState.Clear();
        //    var dataTest = new LoginModel
        //    {
        //        UserName = "nhanvien0001",
        //        Password = "12345678"
        //    };
        //    ViewResult r = controller.Login(dataTest.UserName,dataTest.Password, null) as ViewResult;
        //    Assert.IsNotNull(r);
        //    Assert.IsTrue(controller.ViewData.ModelState.IsValid == true);
        //}
        
    }
}
