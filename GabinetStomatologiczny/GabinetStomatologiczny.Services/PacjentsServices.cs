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

        IEnumerable<Pacjent> getAllPacjents();
        IEnumerable<IGrouping<DateTime, Pacjent>> Statistic();
        Pacjent About(int? id);

        void Edit(Pacjent pacjent);
        
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

        public Pacjent About(int? id)
        {
            return this._db.Pacjent.Find(id);
        }

        public IEnumerable<Pacjent> getAllPacjents()
        {
            return this._db.Pacjent.ToList();
        }

        public void Edit(Pacjent pacjent)
        {
            pacjent.DataRejestracji = DateTime.Now;
            this._db.Entry(pacjent).State = EntityState.Modified;
            this._db.SaveChanges();
        }
    }
}
