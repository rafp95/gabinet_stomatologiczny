using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using GabinetStomatologiczny.DAO.Models;
using System.Collections.Generic;
using System.Data.Entity;
using Moq;
using GabinetStomatologiczny.DAL;
using System.Data.Entity.Infrastructure;
using GabinetStomatologiczny.Share;
using GabinetStomatologiczny.Services;
using System.Threading.Tasks;

namespace Gabinet.Tests
{
    [TestClass]
    public class TestZabiegService
    {

        [TestInitialize]
        private IQueryable<Wizyta> allWizyta()
        {
            var wizyta = new List<Wizyta>()
            {
                new Wizyta() {
                      ID = 1,DataDodania = DateTime.UtcNow
                    , Poczatek = DateTime.UtcNow
                    , Koniec = DateTime.UtcNow.AddMinutes(30)
                    , LekarzID = 1
                    , PacjentID = 1
                    , Lekarz = new Lekarz() {ID = 1, Imie = "Anna", Nazwisko = "Rafał" }
                    , Pacjent = new Pacjent() {ID = 1, Imie = "Patryk",Nazwisko = "Rafał" }}
            }.AsQueryable();
            return wizyta;
        }
        [TestInitialize]
        private IQueryable<Zabieg> allZabieg()
        {
            var zabieg = new List<Zabieg>()
            {
                new Zabieg()
                {
                    ID = 1,
                    WizytaID = 1,
                    Wizyta = new Wizyta() {
                              ID = 1,DataDodania = DateTime.UtcNow
                            , Poczatek = DateTime.UtcNow
                            , Koniec = DateTime.UtcNow.AddMinutes(30)
                            , LekarzID = 1
                            , PacjentID = 1
                            , Lekarz = new Lekarz() {ID = 1, Imie = "Anna", Nazwisko = "Rafał" }
                            , Pacjent = new Pacjent() {ID = 1, Imie = "Patryk",Nazwisko = "Rafał" }
                        }
                }
            }.AsQueryable();
            return zabieg;
        }
        [TestInitialize]
        private IQueryable<Zab> allZab()
        {
            var zeby = new List<Zab>()
            {
                new Zab()
                {
                    IdTooth = "Tooth15",
                    ID = 1,
                    ZabiegID = 1,
                    Zabieg =
                     new Zabieg()
                        {
                            ID = 1,
                            WizytaID = 1,
                            Wizyta = new Wizyta() {
                                      ID = 1,DataDodania = DateTime.UtcNow
                                    , Poczatek = DateTime.UtcNow
                                    , Koniec = DateTime.UtcNow.AddMinutes(30)
                                    , LekarzID = 1
                                    , PacjentID = 1
                                    , Lekarz = new Lekarz() {ID = 1, Imie = "Anna", Nazwisko = "Rafał" }
                                    , Pacjent = new Pacjent() {ID = 1, Imie = "Patryk",Nazwisko = "Rafał" }
                                }
                        }
                }
            }.AsQueryable();
            return zeby;
        }

        [TestMethod]
        public async Task ZabSaveTest()
        {
            var moqkSetWizyta = new Mock<DbSet<Wizyta>>();
            var moqSetZabieg = new Mock<DbSet<Zabieg>>();
            var moqSetZab = new Mock<DbSet<Zab>>();

            var moqContext = new Mock<GabinetContext>();

            #region wizyta
            moqkSetWizyta.As<IDbAsyncEnumerable<Wizyta>>()
             .Setup(m => m.GetAsyncEnumerator())
             .Returns(new TestDbAsyncEnumerator<Wizyta>(allWizyta().GetEnumerator()));

            moqkSetWizyta.As<IQueryable<Wizyta>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Wizyta>(allWizyta().Provider));

            moqkSetWizyta.As<IQueryable<Wizyta>>().Setup(m => m.Expression).Returns(allWizyta().Expression);
            moqkSetWizyta.As<IQueryable<Wizyta>>().Setup(m => m.ElementType).Returns(allWizyta().ElementType);
            moqkSetWizyta.As<IQueryable<Wizyta>>().Setup(m => m.GetEnumerator()).Returns(allWizyta().GetEnumerator());
            #endregion

            #region zabieg
            moqSetZabieg.As<IDbAsyncEnumerable<Zabieg>>()
            .Setup(m => m.GetAsyncEnumerator())
            .Returns(new TestDbAsyncEnumerator<Zabieg>(allZabieg().GetEnumerator()));

            moqSetZabieg.As<IQueryable<Zabieg>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Zabieg>(allZabieg().Provider));

            moqSetZabieg.As<IQueryable<Zabieg>>().Setup(m => m.Expression).Returns(allZabieg().Expression);
            moqSetZabieg.As<IQueryable<Zabieg>>().Setup(m => m.ElementType).Returns(allZabieg().ElementType);
            moqSetZabieg.As<IQueryable<Zabieg>>().Setup(m => m.GetEnumerator()).Returns(allZabieg().GetEnumerator());
            #endregion

            #region zab
            moqSetZab.As<IDbAsyncEnumerable<Zab>>()
           .Setup(m => m.GetAsyncEnumerator())
           .Returns(new TestDbAsyncEnumerator<Zab>(allZab().GetEnumerator()));

            moqSetZab.As<IQueryable<Zab>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Zab>(allZab().Provider));

            moqSetZab.As<IQueryable<Zab>>().Setup(m => m.Expression).Returns(allZab().Expression);
            moqSetZab.As<IQueryable<Zab>>().Setup(m => m.ElementType).Returns(allZab().ElementType);
            moqSetZab.As<IQueryable<Zab>>().Setup(m => m.GetEnumerator()).Returns(allZab().GetEnumerator());
            #endregion

            moqContext.SetupGet(c => c.Wizyta).Returns(moqkSetWizyta.Object);
            moqContext.SetupGet(c => c.Zabieg).Returns(moqSetZabieg.Object);
            moqContext.SetupGet(c => c.Zab).Returns(moqSetZab.Object);

            IZabiegService zabiegService = new ZabiegService(moqContext.Object);

            await zabiegService.SaveTooth(new Zab() { IdTooth = "tooth1", ID = 1 }, 1);

        }
    }
}
