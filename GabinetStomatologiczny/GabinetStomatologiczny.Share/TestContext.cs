using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.DAL;

namespace GabinetStomatologiczny.Share
{
    public class TestContext : GabinetContext, ITestGabinetContext
    {
        public TestContext()
        {
            this.Pacjent = new TestDbSet<Pacjent>();
            this.Wizyta = new TestDbSet<Wizyta>();
        }

        public int SaveChangesCount { get; private set; }
        public override int SaveChanges()
        {
            this.SaveChangesCount++;
            return this.SaveChangesCount;
        }
    }
}
