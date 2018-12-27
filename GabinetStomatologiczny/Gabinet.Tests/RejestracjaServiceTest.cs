using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.Services;
using System.Data.Entity;
using GabinetStomatologiczny.DAO.Models;
using Moq;
using GabinetStomatologiczny.DAL;
using System.Linq;
using GabinetStomatologiczny.Models;
using System.Threading.Tasks;

namespace Gabinet.Tests
{
    /// <summary>
    /// Summary description for RejestracjaServiceTest
    /// </summary>
    [TestClass]
    public class RejestracjaServiceTest
    {
        public RejestracjaServiceTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void RejestracjaTest()
        {
            Osoba pacejnt = new Osoba()
            {
                Imie = "Kamil",
                Nazwisko = "RafaL",
                Haslo = "fds",
                PowtorzHaslo = "fds",
                Email = "patrykrafal33@o2.pl",
                NrTelefonu = "121-122-122",
            };

            Osoba lekarz = new Osoba()
            {
                Imie = "Kamil",
                Nazwisko = "RafaL",
                Haslo = "fds",
                PowtorzHaslo = "fds",
                Email = "patrykrafal33@o2.pl",
                NrTelefonu = "121-122-122",
                RoleType = RoleType.Lekarz
            };

            Osoba admin = new Osoba()
            {
                Imie = "Kamil",
                Nazwisko = "RafaL",
                Haslo = "fds",
                PowtorzHaslo = "fds",
                Email = "patrykrafal33@o2.pl",
                NrTelefonu = "121-122-122",
                RoleType = RoleType.Administrator
            };

            Assert.AreEqual(pacejnt.RoleType, RoleType.Pacjent);
            Assert.AreEqual(lekarz.RoleType, RoleType.Lekarz);
            Assert.AreEqual(admin.RoleType, RoleType.Administrator);


        }
    }
}
