using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GabinetStomatologiczny.Utils;
using Moq;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.Services;
using GabinetStomatologiczny.DAO.Models;
using System.Data.Entity;

namespace Gabinet.Tests
{
    [TestClass]
    public class TestDataBase
    {

        public class PacjentContext : DbContext
        {
            public virtual DbSet<Pacjent> Pacjent { get; set; }
        }


        public class PacjentService
        {
            private readonly PacjentContext pacjentContext;

            public PacjentService(PacjentContext pacjentContext)
            {
                this.pacjentContext = pacjentContext;
            }

            public Pacjent AddUser(Pacjent p)
            {
                var newUser = this.pacjentContext.Pacjent.Add(p);

                this.pacjentContext.SaveChanges();
                return newUser;
            }
        }

        [TestMethod]
        public void TestSaveToDataBase()
        {
            // Arrange
            var userContextMock = new Mock<PacjentContext>();
            userContextMock.Setup(x => x.Pacjent.
            Add(It.IsAny<Pacjent>())).Returns((Pacjent u) => u);

            var pacjentServcie = new PacjentService(userContextMock.Object);

            // Act
            Pacjent p = new Pacjent();
            pacjentServcie.AddUser(p);

            userContextMock.Verify(x => x.SaveChanges());
        }
    }
}
