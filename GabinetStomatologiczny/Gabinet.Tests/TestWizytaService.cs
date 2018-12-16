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
using AutoMapper;
using GabinetStomatologiczny;
using System.Reflection;
using AutoMapper.Configuration;
using System.Web.Mvc;

namespace Gabinet.Tests
{
    [TestClass]
    public class TestWizytaService
    {

        private readonly DateTime data = DateTime.Now;

        [TestInitialize]
        public void AutoMapperTestVerify()
        {
            //var ass = typeof(Startup).Assembly;
            //var all = ass.DefinedTypes
            //    .Where(type => type.AsType().GetTypeInfo().IsSubclassOf(typeof(MapperConfigurationExpression)));

            //foreach (var ti in all)
            //{
            //    var t = ti.AsType();

            //    var mappingProfile = Activator.CreateInstance(t) as MapperConfigurationExpression;

            //    var config = new MapperConfiguration(mappingProfile);
            //    var mapper = new Mapper(config);

            //    (mapper as IMapper).ConfigurationProvider.AssertConfigurationIsValid();
            // }

            Mapper.Initialize(conf =>
            {
                conf.CreateMap<Wizyta, Wizyta>()
                .ForMember(dest => dest.Koniec,
               opts => opts.MapFrom(src => src.Poczatek.AddMinutes(30)))
               .ForMember(dest => dest.Lekarz,
               opts => opts.Ignore())
               .ForPath(dest => dest.Pacjent.Imie, opts => opts.Ignore())
               .ForPath(dest => dest.Pacjent.Nazwisko, opts => opts.Ignore());
            });


        }
        private IQueryable<Wizyta> allWizyta()
        {
            var wizyta = new List<Wizyta>()
            {
                new Wizyta() {
                      ID = 1,DataDodania = DateTime.Now
                    , Poczatek = data
                    , Koniec = data.AddMinutes(30)
                    , LekarzID = 1
                    , PacjentID = 1
                    , Lekarz = new Lekarz() {ID = 1, Imie = "Anna", Nazwisko = "Rafał" }
                    , Pacjent = new Pacjent() {ID = 1, Imie = "Patryk",Nazwisko = "Rafał" }}
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
                ID = 1,
                Poczatek = DateTime.Now,
                Koniec = DateTime.Now,
                Lekarz = new Lekarz() { ID = 1, Imie = "Patryk", Nazwisko = "Rafał" },
                Pacjent = new Pacjent() { ID = 1, Imie = "Anna", Nazwisko = "DFDS" },
            };
            var wServiceCreateTtrue = await wizyta.SaveEvent(w, identity);

            Wizyta wFalse = new Wizyta()
            {
                ID = 1,
                Poczatek = data,
                Koniec = data,
                Lekarz = new Lekarz() { ID = 1, Imie = "Patryk", Nazwisko = "Rafał" },
                Pacjent = new Pacjent() { ID = 1, Imie = "Anna", Nazwisko = "DFDS" },
            };
            var wServiceCreateFalse = await wizyta.SaveEvent(w, identity);

            Assert.AreEqual(userRole, "Pacjent");
            Assert.AreEqual(identity.Name, "patrykrafal33@o2.pl");
            Assert.AreEqual(wServiceCreateTtrue, true);
            Assert.AreEqual(wServiceCreateFalse, false);
        }

        [TestMethod]
        public async Task GetEventTest()
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

            var listsIsNotAutenticated = await wizyta.GetEvents(false, "", "");

            var listsIsAutenticated = await wizyta.GetEvents(true, userRole, identity.Name);

            Assert.AreEqual(true, listsIsNotAutenticated.Count() > 0);
            Assert.AreEqual(true, listsIsAutenticated.Count() > 0);
        }

    }

}
