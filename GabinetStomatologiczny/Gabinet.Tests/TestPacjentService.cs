using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabinet.Tests
{
    [TestClass]
    public class TestPacjentService
    {

        [TestMethod]
        public void TestPlec()
        {
            Pacjent plec = new Pacjent() { Imie = "Anna" , Nazwisko = "Rafał"};
            var isValid = pacjentPlec(plec);
            Assert.AreEqual(true, isValid);

            plec.Imie = "Patryk";
            plec.Nazwisko = "Kowal";
            isValid = pacjentPlec(plec);
            Assert.AreEqual(false, isValid);

        }

        private bool pacjentPlec(Pacjent p)
        {
            return char.ToLower(p.Imie.ElementAt(p.Imie.Length - 1)).Equals('a');
        }
    }
}
