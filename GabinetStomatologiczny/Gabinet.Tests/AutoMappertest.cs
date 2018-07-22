using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.DAO.Models;
using AutoMapper;
using GabinetStomatologiczny.Models;

namespace Gabinet.Tests
{
    [TestClass]
    public class AutoMapperTest
    {

        public AutoMapperTest()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperWizyta>());
            Mapper.Configuration.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void TestMapping()
        {
            var w = new Wizyta() {Tytul = "Patryk Rafal" ,ID = 1};
            var w2 = new Wizyta() { Tytul = "Patryk Rafal", ID = 4 };

            var checkValue = verifyAutoMapper(w, w2);

            Assert.AreEqual(true,checkValue);
            
        }

        private bool verifyAutoMapper(Wizyta w, Wizyta w2)
        {
            var m = Mapper.Map<Wizyta, Wizyta>(w,w2);
            return w.ID == m.ID;
        }
    }
}
