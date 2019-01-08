using GabinetStomatologiczny.Controllers;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Infrastucture;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Services;
using GabinetStomatologiczny.Share;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gabinet.Tests
{
    [TestClass]
    public class TestPacjentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
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
            var result = service.GetAllPacjents();

            Assert.AreEqual(result.Count(), 12);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public async Task PacejntCreate()
        {
            var context = new GabinetStomatologiczny.Share.TestContext();

            context.Pacjent.Add(new Pacjent() { ID = 1, Imie = "Kowalski", Nazwisko = "Jan", Email = "patrykrafal33@o2.pl" });
            context.Pacjent.Add(new Pacjent() { ID = 2, Imie = "Słowik", Nazwisko = "FDS", Email = "patrykrafal33@o2.pl" });
            context.Pacjent.Add(new Pacjent() { ID = 3, Imie = "FDS", Nazwisko = "JFDan", Email = "patrykrafal33@o2.pl" });

            var service = new PacjentsService(context);
            var pacjents = context.SaveChanges();
            var allPacjents = service.GetAllPacjents().Count();
            var namePacjent = await service.GetPacjentById(2);

            Assert.AreEqual(namePacjent.Imie, "Słowik");
            Assert.AreEqual(1, pacjents);
            Assert.AreEqual(3, allPacjents);
        }

        [TestMethod]
        public void PacejntDpendencyTest()
        {
            var moqService = new Mock<IPacjentsService>();

            Assert.AreNotEqual(moqService, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PacjentFindById()
        {

            var moqkSet = new Mock<DbSet<Pacjent>>();
            var moqContext = new Mock<GabinetContext>();

            //moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Provider).Returns(allPacjents().Provider);
            moqkSet.As<IDbAsyncEnumerable<Pacjent>>()
             .Setup(m => m.GetAsyncEnumerator())
             .Returns(new TestDbAsyncEnumerator<Pacjent>(allPacjents().GetEnumerator()));

            moqkSet.As<IQueryable<Pacjent>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Pacjent>(allPacjents().Provider));

            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Expression).Returns(allPacjents().Expression);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.ElementType).Returns(allPacjents().ElementType);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.GetEnumerator()).Returns(allPacjents().GetEnumerator());

            moqContext.SetupGet(c => c.Pacjent).Returns(moqkSet.Object);

            IPacjentsService service = new PacjentsService(moqContext.Object);
            var result = await service.Find(2);

            Assert.AreEqual(result.Imie, "Kamila");
        }

        [TestMethod]
        public void TestSugestionPacjents()
        {
            var moqkSet = new Mock<DbSet<Pacjent>>();
            var moqContext = new Mock<GabinetContext>();

            //moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Provider).Returns(allPacjents().Provider);
            moqkSet.As<IDbAsyncEnumerable<Pacjent>>()
             .Setup(m => m.GetAsyncEnumerator())
             .Returns(new TestDbAsyncEnumerator<Pacjent>(allPacjents().GetEnumerator()));

            moqkSet.As<IQueryable<Pacjent>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Pacjent>(allPacjents().Provider));

            moqkSet.As<IQueryable<Pacjent>>().Setup(m => m.Expression).Returns(allPacjents().Expression);
            moqkSet.As<IQueryable<Pacjent>>().Setup(m => m.ElementType).Returns(allPacjents().ElementType);
            moqkSet.As<IQueryable<Pacjent>>().Setup(m => m.GetEnumerator()).Returns(allPacjents().GetEnumerator());

            moqContext.SetupGet(c => c.Pacjent).Returns(moqkSet.Object);

            ISugestionService p = new SugestionService(moqContext.Object);
            Sugestion s = new Sugestion();
            s.term = "Kam";
            var service = p.SugestionPacjent(s);
            var elem = service.FirstOrDefault();
            var obj = new { label = "Kamila Rafal",ID = 2 };

            s.term = "dfsffdsfdfsdfds";
            var service2 = p.SugestionPacjent(s);
            var elem2 = service2.FirstOrDefault();
            var obj2 = new { label = "Nie jesteś zapisany", ID = 0 };


            Assert.AreEqual(obj.ToString(), elem.ToString());
            Assert.AreEqual(obj2.ToString(), elem2.ToString());
        }

        [TestMethod]
        public void TestPacejntPagination()
        {
            var moqkSet = new Mock<DbSet<Pacjent>>();
            var moqContext = new Mock<GabinetContext>();

            //moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Provider).Returns(allPacjents().Provider);
            moqkSet.As<IDbAsyncEnumerable<Pacjent>>()
             .Setup(m => m.GetAsyncEnumerator())
             .Returns(new TestDbAsyncEnumerator<Pacjent>(allPacjents().GetEnumerator()));

            moqkSet.As<IQueryable<Pacjent>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Pacjent>(allPacjents().Provider));

            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.Expression).Returns(allPacjents().Expression);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.ElementType).Returns(allPacjents().ElementType);
            moqkSet.As<IQueryable<Pacjent>>().Setup(p => p.GetEnumerator()).Returns(allPacjents().GetEnumerator());

            moqContext.SetupGet(c => c.Pacjent).Returns(moqkSet.Object);

            var service = new SortPacjentService(moqContext.Object);
            var paginationType = new GabinetStomatologiczny.Models.PaginationType() { };

            paginationType.page = 2;
            paginationType.sortOrder = RodzajSort.nrtel_asc;
            var test = service.SortAndPaginate(paginationType);

            Assert.AreEqual(paginationType.sortOrder, RodzajSort.nrtel_asc);
            Assert.AreEqual(test.Count(), 2);
            Assert.AreEqual(2, paginationType.pageNumber);
            Assert.AreEqual(service.RodzajSort, RodzajSort.nrtel_asc);
            
        }

        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool pacjentPlec(Pacjent p)
        {
            return char.ToLower(p.Imie.ElementAt(p.Imie.Length - 1)).Equals('a');
        }

        [TestMethod]
        public void TestDatePacjentAttribue()
        {
            
            var pacjentDateAttribute = new DatePacjentAttribute();

            var isNull = pacjentDateAttribute.IsValid(null);
            var isAfter = pacjentDateAttribute.IsValid(DateTime.Now.AddDays(1));
            var isBefore = pacjentDateAttribute.IsValid(DateTime.MinValue);
            var isMaxDate = pacjentDateAttribute.IsValid(DateTime.MaxValue);
            var isNow = pacjentDateAttribute.IsValid(DateTime.Now);

            Assert.AreEqual(false, isAfter, "Podana data nie jest wcześniejsza");
            Assert.AreEqual(true, isBefore,"Podana data nie jest wcześniejsza");
            Assert.AreEqual(true, isNull, "Podana data nie jest wcześniejsza");
            Assert.AreEqual(true, isNow, "Podana data nie jest wcześniejsza");
            Assert.AreEqual(false, isMaxDate, "Podana data nie jest wcześniejsza");
        }
    }
}
