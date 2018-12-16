using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.DAO.Models;
using AutoMapper;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny;
using AutoMapper.Configuration;
using System.Linq;
using System.Reflection;

namespace Gabinet.Tests
{
    [TestClass]
    public class AutoMapperTest
    {

        [TestMethod]
        public void AutoMapperTestVerify()
        {
            var ass = typeof(Startup).Assembly;
            var all = ass.DefinedTypes
                .Where(type => type.AsType().GetTypeInfo().IsSubclassOf(typeof(MapperConfigurationExpression)));

            foreach (var ti in all)
            {
                var t = ti.AsType();

                var mappingProfile = Activator.CreateInstance(t) as MapperConfigurationExpression;

                var config = new MapperConfiguration(mappingProfile);
                var mapper = new Mapper(config);

                (mapper as IMapper).ConfigurationProvider.AssertConfigurationIsValid();
            }
        }

        [TestMethod]
        public void TestMapping()
        {
            //var w = new Wizyta() {Tytul = "Patryk Rafal" ,ID = 1};
            //var w2 = new Wizyta() { Tytul = "Patryk Rafal", ID = 4 };

            //var checkValue = verifyAutoMapper(w, w2);

            //Assert.AreEqual(true,checkValue);
            
        }

        private bool verifyAutoMapper(Wizyta w, Wizyta w2)
        {
            var m = Mapper.Map<Wizyta, Wizyta>(w,w2);
            return w.ID == m.ID;
        }
    }
}
