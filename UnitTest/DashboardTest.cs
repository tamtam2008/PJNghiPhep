using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectNghiPhep.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
namespace UnitTest
{
    [TestClass]
    public class DashboardTest
    {
        [TestMethod]
        public void TestIndex()
        {
            HomeController home = new HomeController();
            ViewResult r = home.Index() as ViewResult;
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", r.ViewBag.Message);
            Assert.IsNotNull(r);
        }
        [TestMethod]
        public void TestAbout()
        {
            HomeController home = new HomeController();
            ViewResult r = home.About() as ViewResult;
            Assert.IsNotNull(r);
        }
        [TestMethod]
        public void TestContact()
        {
            HomeController home = new HomeController();
            ViewResult r = home.Contact() as ViewResult;
            Assert.IsNotNull(r);
        }
    }
}
