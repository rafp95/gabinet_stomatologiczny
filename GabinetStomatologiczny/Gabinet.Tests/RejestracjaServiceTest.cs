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

        private IQueryable<Pacjent> allPacjents()
        {
            var pacjents = new List<Pacjent>() {
              new Pacjent() { ID = 1, Imie = "Patryk", Nazwisko = "Rafal" },
              new Pacjent() { ID = 2, Imie = "Kamila", Nazwisko = "Rafal" },
              new Pacjent() { ID = 3, Imie = "Piotr", Nazwisko = "Rafal" },
              new Pacjent() { ID = 4, Imie = "Anna", Nazwisko = "Rafal" },
              new Pacjent() { ID = 5, Imie = "Pavvtryk", Nazwisko = "Rafal" },
              new Pacjent() { ID = 6, Imie = "Kavcmila", Nazwisko = "Rafal" },
              new Pacjent() { ID = 7, Imie = "Pvcxiotr", Nazwisko = "Rafal" },
              new Pacjent() { ID = 8, Imie = "Anvna", Nazwisko = "Rafal" },
              new Pacjent() { ID = 9, Imie = "Patvxryk", Nazwisko = "Rafal" },
              new Pacjent() { ID = 10, Imie = "Kamvcxila", Nazwisko = "Rafal" },
              new Pacjent() { ID = 11, Imie = "Piovcxtr", Nazwisko = "Rafal" },
              new Pacjent() { ID = 12, Imie = "Anvxcna", Nazwisko = "Rafal" }
            }.AsQueryable();

            return pacjents;

        }

        [TestMethod]
        public void RejestracjaTest()
        {

            #region pacjent contect

            var moqkSet = new Mock<DbSet<Pacjent>>();
            var moqContext = new Mock<GabinetContext>();

            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Provider).Returns(allPacjents().Provider);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Expression).Returns(allPacjents().Expression);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.ElementType).Returns(allPacjents().ElementType);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.GetEnumerator()).Returns(allPacjents().GetEnumerator());

            moqContext.SetupGet(c => c.Pacjent).Returns(moqkSet.Object);

            #endregion

            IRejestracjaService testRejestr = new RejestracjaService(moqContext.Object, null, null, null, null);
            #region osoba
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

            testRejestr.CreateUserInIdentity
                (pacejnt);
            #endregion

            Assert.AreEqual(pacejnt.RoleType, RoleType.Pacjent);
            Assert.AreEqual(lekarz.RoleType, RoleType.Lekarz);
            Assert.AreEqual(admin.RoleType, RoleType.Administrator);


        }
    }
}
