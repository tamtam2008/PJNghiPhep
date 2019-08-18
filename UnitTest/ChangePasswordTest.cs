using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectNghiPhep.Controllers;
using System.Web.Mvc;
using ProjectNghiPhep.Models.ViewModels;

namespace UnitTest
{
    [TestClass]
    public class ChangePasswordTest
    {
        [TestMethod]
        public void TestChangePasswordGet()
        {
            NguoiDungController u = new NguoiDungController();
            ViewResult r = u.ChangePassword() as ViewResult;
            Assert.IsNotNull(r);
        }
        
        //Test trường hợp nhập mật khẩu mới không khớp
        [TestMethod]
        public void TestNewPasswordIsNotMatch()
        {
            NguoiDungController controller = new NguoiDungController();
            controller.ViewData.ModelState.Clear();
            var dataTest = new ChangePasswordViewModel
            {
                Username = "Admin",         //Username đúng
                CurrentPassword = "123",    //Password đúng
                NewPassword = "1234",       //Mật khẩu mới
                NewPasswordAgain = "12345" //Mật khẩu nhập lại không đúng
            };
            ViewResult r = controller.ChangePassword(dataTest) as ViewResult;
            Assert.IsNotNull(r);
            Assert.IsTrue(controller.ViewData.ModelState.IsValid == false);
        }

        //Test trường hợp nhập username sai
        [TestMethod]
        public void TestUsernameInvalid()
        {
            NguoiDungController controller = new NguoiDungController();
            controller.ViewData.ModelState.Clear();
            var dataTest = new ChangePasswordViewModel
            {
                Username = "Admin1",        //Username sai
                CurrentPassword = "123",    //Password đúng
                NewPassword = "1234",       //Mật khẩu mới
                NewPasswordAgain = "1234"   //Mật khẩu nhập lại đúng
            };
            ViewResult r = controller.ChangePassword(dataTest) as ViewResult;
            Assert.IsNotNull(r);
            Assert.IsTrue(controller.ViewData.ModelState.IsValid == false);
        }

        //Test trường hợp nhập password sai
        [TestMethod]
        public void TestPasswordInvalid()
        {
            NguoiDungController controller = new NguoiDungController();
            controller.ViewData.ModelState.Clear();
            var dataTest = new ChangePasswordViewModel
            {
                Username = "Admin",         //Username đúng
                CurrentPassword = "12",     //Password sai
                NewPassword = "1234",       //Mật khẩu mới
                NewPasswordAgain = "1234"   //Mật khẩu nhập lại đúng
            };
            ViewResult r = controller.ChangePassword(dataTest) as ViewResult;
            Assert.IsNotNull(r);
            Assert.IsTrue(controller.ViewData.ModelState.IsValid == false);
        }

        [TestMethod]
        //Test trường hợp nhập thông tin hợp lệ
        public void TestChangePasswordPostSuccess()
        {
            NguoiDungController controller = new NguoiDungController();
            controller.ViewData.ModelState.Clear();
            var dataTest = new ChangePasswordViewModel
            {
                Username = "Admin",         //Username đúng
                CurrentPassword = "123",    //Password đúng
                NewPassword = "12345678",       //Mật khẩu mới
                NewPasswordAgain = "12345678"   //Mật khẩu nhập lại đúng
            };
            ViewResult r = controller.ChangePassword(dataTest) as ViewResult;
            Assert.IsNotNull(r);
            Assert.IsTrue(controller.ViewData.ModelState.IsValid == true);
        }
    }
}
