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
    public class CreateNewAFTest
    {
        [TestMethod]
        public void TestCreateNewAF()
        {
            DonNghiPhepController u = new DonNghiPhepController();
            ViewResult r = u.CreateNew() as ViewResult;
            Assert.IsNotNull(r);
        }
    }
}
