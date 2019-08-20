using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectNghiPhep.Controllers;
using System.Web.Mvc;
using ProjectNghiPhep.Email;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class EmailTest
    {
        //Nhập địa chỉ email hợp lệ
        [TestMethod]
        public void TestSendMailSuccess()
        {
            var mail = new MailModel
            {
                ListToEmail = new List<string> { "hethongqlnghiphep@gmail.com" }, //Nhập email đúng để kiểm tra
                Body = "Đây là email test chức năng gửi email",
                EmailSubject = "Test gửi email"
            };
            var result = EmailHelper.SendMail(mail);
            Assert.IsTrue(result == true);
        }

        //Nhập địa chỉ email không hợp lệ
        [TestMethod]
        public void TestSendMailFail()
        {
            var mail = new MailModel
            {
                ListToEmail = new List<string> { "abc@xyz.st" },  //Nhập email sai
                Body = "Đây là email test chức năng gửi email",
                EmailSubject = "Test gửi email"
            };
            var result = EmailHelper.SendMail(mail);
            Assert.IsFalse(result == false);
        }
    }
}
