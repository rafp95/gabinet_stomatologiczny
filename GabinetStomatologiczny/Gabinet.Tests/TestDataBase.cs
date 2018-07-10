using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.Utils;
using Moq;
using GabinetStomatologiczny.DAL;

namespace Gabinet.Tests
{
    [TestClass]
    public class TestDataBase : BaseController
    {
        public TestDataBase(GabinetContext db) : base(db)
        {
        }

        [TestMethod]
        public void TestSaveToDataBase()
        {
            //var mockContect = new Mock<GabinetCon>
        }
    }
}
