using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Share;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gabinet.Tests
{
    [TestClass]  
    public class PacejntPropTest
    {
        [TestMethod]
        public void TestPropPacjent()
        {
            CheckPropertyValidation check = new CheckPropertyValidation();

            var p = new Pacjent()
            {
                Imie = "fdfds",
                Nazwisko = "dfdsfd",
                DataUrodzenia = DateTime.MinValue,
                NrTelefonu = "3232",
                Email = "patrykrafal33@o2.pl",
            };

            var errorcount = check.myValidation(p).Count;

            Assert.AreEqual(0, errorcount);
        }
    }
}
