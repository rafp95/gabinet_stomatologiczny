using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.Controllers;
using System.Web.Mvc;

namespace Gabinet.Tests
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void HomeController()
        {
            var controller = new HomeController();

            var result = controller.Index() as ViewResult;
         
            Assert.AreEqual("", result.ViewName);

        }
    }
}
