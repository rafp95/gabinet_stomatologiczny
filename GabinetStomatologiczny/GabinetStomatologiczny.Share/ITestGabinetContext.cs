using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Share
{
    public interface ITestGabinetContext
    {
        DbSet<Pacjent> Pacjent { get; }
        int SaveChanges();
    }
}
