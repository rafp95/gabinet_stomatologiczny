using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{

    public interface IPacjentsService
    {
        IEnumerable<IGrouping<DateTime, Pacjent>> Statistic();
    }


    public class PacjentsService : IPacjentsService
    {
        private readonly GabinetContext _db;

        public PacjentsService(GabinetContext db)
        {
            this._db = db;
        }

        public IEnumerable<IGrouping<DateTime, Pacjent>> Statistic()
        {
            return this._db.Pacjent.ToList().GroupBy(m => m.DataRejestracji.Date); 
        }
    }
}
