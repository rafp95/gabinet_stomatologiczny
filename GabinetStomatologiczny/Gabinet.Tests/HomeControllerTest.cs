using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.Controllers;
using System.Web.Mvc;
using AutoMapper;
using System.Collections.Generic;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny.Utils;
using System.Linq;

namespace Gabinet.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeController()
        {
            //var controller = new HomeController();

            //var result = controller.Index() as ViewResult;
         
            //Assert.AreEqual("", result.ViewName);

        }

        [TestMethod]
        public void HomeControllerAbout()
        {
            //var about = new HomeController();

            //var result = about.About() as ViewResult;

            //var list = Mapper.Map<IEnumerable<LiczbaPacjentow>>(db.Pacjent.ToList().GroupBy(m => m.DataRejestracji.Date).
            //    Select(m => new LiczbaPacjentow { DataRejestracji = m.Key, Pacjentow = m.Count() }));

            //Assert.AreEqual(list, result);
        }
    }
}
