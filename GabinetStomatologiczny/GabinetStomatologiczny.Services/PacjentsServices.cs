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
        Task<Pacjent> CreatePacjnt(Pacjent pacjent);
        
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

        public async void Edit(Pacjent pacjent)
        {
            pacjent.Plec = pacjentPlec(pacjent) ? Plec.Kobieta : Plec.Mężczyzna;
            this._db.Entry(pacjent).State = EntityState.Modified;
            await this._db.SaveChangesAsync();
        }

        public async void Remove(Pacjent pacjent)
        {
            if(pacjent != null)
            {
                this._db.Pacjent.Remove(pacjent);
                await this._db.SaveChangesAsync();
            }
            
        }


        public async Task<Pacjent> CreatePacjnt(Pacjent pacjent)
        {
            pacjent.Plec = pacjentPlec(pacjent) ? Plec.Kobieta : Plec.Mężczyzna;
            Pacjent p = this._db.Pacjent.Add(pacjent);
            await this._db.SaveChangesAsync();
            return p;
        }

        public async Task<Pacjent> Find(int? id)
        {
            var pacjent = await this._db.Pacjent.FindAsync(id);
            pacjent.OdzialNfz = pacjent.OdzialNfz > 0 ? pacjent.OdzialNfz : null;
            pacjent.Wojewodztwo = pacjent.Wojewodztwo > 0 ? pacjent.Wojewodztwo : null;
            return pacjent;
        }

        private bool pacjentPlec(Pacjent p)
        {
            return char.ToLower(p.Imie.ElementAt(p.Imie.Length - 1)).Equals('a');
        }
    }
}
