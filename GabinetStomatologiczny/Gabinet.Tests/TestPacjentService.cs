using GabinetStomatologiczny.Controllers;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabinet.Tests
{
    [TestClass]
    public class TestPacjentService
    {

        private IQueryable<Pacjent> allPacjents()
        {
            var pacjents = new List<Pacjent>() {
              new Pacjent() { ID = 1, Imie = "Patryk", Nazwisko = "Rafal" },
              new Pacjent() { ID = 2, Imie = "Kamila", Nazwisko = "Rafal" },
              new Pacjent() { ID = 3, Imie = "Piotr", Nazwisko = "Rafal" },
              new Pacjent() { ID = 4, Imie = "Anna", Nazwisko = "Rafal" }
            }.AsQueryable();

            return pacjents;

        }

        [TestMethod]
        public void PacejntListCount()
        {
            
            var moqkSet = new Mock<DbSet<Pacjent>>();
            var moqContext = new Mock<GabinetContext>();

            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Provider).Returns(allPacjents().Provider);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Expression).Returns(allPacjents().Expression);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.ElementType).Returns(allPacjents().ElementType);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.GetEnumerator()).Returns(allPacjents().GetEnumerator());

            moqContext.SetupGet(c => c.Pacjent).Returns(moqkSet.Object);

            IPacjentsService service = new PacjentsService(moqContext.Object);
            var result = service.getAllPacjents();

            Assert.AreEqual(result.Count(), 4);
        }

        [TestMethod]
        public async void PacjentFindById()
        {

            var moqkSet = new Mock<DbSet<Pacjent>>();
            var moqContext = new Mock<GabinetContext>();

            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Provider).Returns(allPacjents().Provider);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Expression).Returns(allPacjents().Expression);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.ElementType).Returns(allPacjents().ElementType);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.GetEnumerator()).Returns(allPacjents().GetEnumerator());

            moqContext.SetupGet(c => c.Pacjent).Returns(moqkSet.Object);

            IPacjentsService service = new PacjentsService(moqContext.Object);
            var result = await service.Find(2);

            Assert.AreEqual(result.Imie, "Kadadsadsmila");
        }


        [TestMethod]
        public void TestPlec()
        {
            Pacjent plec = new Pacjent() { Imie = "Anna", Nazwisko = "Rafał" };
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
