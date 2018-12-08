using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Share;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
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
                //Imie = "fdfds",
                NrTelefonu = "567-123-213",
                Nazwisko = "dfdsfd",
                DataUrodzenia = DateTime.MinValue,
                Email = "patrykrafal33@o2.pl",
            };

            var errorcount = check.myValidation(p).Count;

            NUnit.Framework.Assert.AreEqual(0, errorcount);
        }
    }
}
