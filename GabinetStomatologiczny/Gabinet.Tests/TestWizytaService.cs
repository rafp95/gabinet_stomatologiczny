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

namespace Gabinet.Tests
{
    [TestClass]
    public class TestWizytaService
    {

        private IQueryable<Wizyta> allWizyta()
        {
            var wizyta = new List<Wizyta>()
            {
                new Wizyta() {ID = 1,DataDodania = DateTime.Now,LekarzID = 1, Poczatek = DateTime.Now }
            }.AsQueryable();
            return wizyta;
        }

        [TestMethod]
        public void TestCreateWizyta()
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

            moqkSet.As<IQueryable<Wizyta>>().Setup(p => p.Provider).Returns(allWizyta().Provider);
            moqkSet.As<IQueryable<Wizyta>>().Setup(p => p.Expression).Returns(allWizyta().Expression);
            moqkSet.As<IQueryable<Wizyta>>().Setup(p => p.ElementType).Returns(allWizyta().ElementType);
            moqkSet.As<IQueryable<Wizyta>>().Setup(p => p.GetEnumerator()).Returns(allWizyta().GetEnumerator());

            moqContext.SetupGet(c => c.Wizyta).Returns(moqkSet.Object);

            Assert.AreEqual(userRole, "Pacjent");
        }
    }
}
