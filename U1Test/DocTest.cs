using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectNghiPhep.Controllers;
using System.Web.Mvc;

namespace U1Test
{
    [TestClass]
    public class DocTest
    {
        [TestMethod]
        public void TestIndex() {
            NguoiDungController u = new NguoiDungController();
            ViewResult r = u.Index() as ViewResult;
            Assert.IsNotNull(r);
        }
    }
}
