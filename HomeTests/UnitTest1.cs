using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using ProjectNghiPhep;
using ProjectNghiPhep.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HomeIndexTests()
        {
            //UnitTest1 controller = new UnitTest1();

            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Hello World !", result.ViewBag.Message);
        }
    }
}
