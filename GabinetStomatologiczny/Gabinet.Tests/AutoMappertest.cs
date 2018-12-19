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
    }
}
