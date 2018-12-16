using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using GabinetStomatologiczny.Share;
using System.Threading;
using System.Linq;
using GabinetStomatologiczny.DAO.Models;
using System.Collections.Generic;
using GabinetStomatologiczny.DAL;
using Moq;
using System.Data.Entity;
using GabinetStomatologiczny.Services;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace Gabinet.Tests
{
    [TestClass]
    public class TestWizytaService
    {

        private IQueryable<Wizyta> allWizyta()
        {
            var wizyta = new List<Wizyta>()
            {
                new Wizyta() {ID = 1,DataDodania = DateTime.Now,LekarzID = 1, Poczatek = DateTime.Now.AddMinutes(30), Koniec = DateTime.Now.AddMinutes(60) }
            }.AsQueryable();
            return wizyta;
        }

        [TestMethod]
        public async Task TestCreateWizyta()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "patrykrafal33@o2.pl"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Pacjent"),
                new Claim("name", "Patryk Rafał"),
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            IUserRoleService role = new UserRoleService();

            var userRole = role.GetUserRole(identity);

            var moqkSet = new Mock<DbSet<Wizyta>>();
            var moqContext = new Mock<GabinetContext>();

            moqkSet.As<IDbAsyncEnumerable<Wizyta>>()
             .Setup(m => m.GetAsyncEnumerator())
             .Returns(new TestDbAsyncEnumerator<Wizyta>(allWizyta().GetEnumerator()));

            moqkSet.As<IQueryable<Wizyta>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Wizyta>(allWizyta().Provider));

            moqkSet.As<IQueryable<Wizyta>>().Setup(m => m.Expression).Returns(allWizyta().Expression);
            moqkSet.As<IQueryable<Wizyta>>().Setup(m => m.ElementType).Returns(allWizyta().ElementType);
            moqkSet.As<IQueryable<Wizyta>>().Setup(m => m.GetEnumerator()).Returns(allWizyta().GetEnumerator());

            moqContext.SetupGet(c => c.Wizyta).Returns(moqkSet.Object);


            IWizytService wizyta = new WizytServie(moqContext.Object, role);

            Wizyta w = new Wizyta()
            {
                ID = 0,
                Poczatek = DateTime.Now
            };

            var wService = await wizyta.SaveEvent(w, identity);

            Assert.AreEqual(userRole, "Pacjent");
            Assert.AreEqual(identity.Name, "patrykrafal33@o2.pl");
            Assert.AreEqual(wService, false);
        }
    }
}
