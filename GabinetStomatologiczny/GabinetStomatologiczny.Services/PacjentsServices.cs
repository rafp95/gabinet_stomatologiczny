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
        void Edit(Pacjent pacjent);
        Task<Pacjent> Find(int? id);
        void Remove(Pacjent pacjent);
        Pacjent CreatePacjnt(Pacjent pacjent);
        
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

        public IEnumerable<Pacjent> getAllPacjents()
        {
            return this._db.Pacjent.ToList();
        }

        public void Edit(Pacjent pacjent)
        {
            pacjent.DataRejestracji = DateTime.Now;
            this._db.Entry(pacjent).State = EntityState.Modified;
            this._db.SaveChangesAsync();
        }

        public void Remove(Pacjent pacjent)
        {
            if(pacjent != null)
            {
                this._db.Pacjent.Remove(pacjent);
                this._db.SaveChangesAsync();
            }
            
        }


        public Pacjent CreatePacjnt(Pacjent pacjent)
        {
            Pacjent p = this._db.Pacjent.Add(pacjent);
            this._db.SaveChanges();
            return p;
        }

        public Task<Pacjent> Find(int? id)
        {
            return this._db.Pacjent.FindAsync(id);
        }
    }
}
