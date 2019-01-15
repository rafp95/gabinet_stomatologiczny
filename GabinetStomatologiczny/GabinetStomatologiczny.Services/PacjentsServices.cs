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

        Task<IEnumerable<Pacjent>> GetAllPacjents();
        Task<IEnumerable<IGrouping<DateTime, Pacjent>>> Statistic();
        Task<Pacjent> Edit(Pacjent pacjent);
        Task<Pacjent> Find(int? id);
        Task<Pacjent> Remove(Pacjent pacjent);
        Task<Pacjent> CreatePacjnt(Pacjent pacjent);
        Task<Pacjent> GetPacjentById(int? id);
    }


    public class PacjentsService : IPacjentsService
    {
        private readonly GabinetContext _db;

        public PacjentsService(GabinetContext db)
        {
            this._db = db;
        }

        /// <summary>
        /// Statystki rejestracji
        /// </summary>
        /// <returns></returns>
        #region Statystki

        public async Task<IEnumerable<IGrouping<DateTime, Pacjent>>> Statistic()
        {
            var pacjents = await this.GetAllPacjents();
            return pacjents.GroupBy(m => m.DataRejestracji.Date);
        }
        #endregion

        public async Task<IEnumerable<Pacjent>> GetAllPacjents()
        {
            return await this._db.Pacjent.ToListAsync();
        }

        public async Task<Pacjent> Edit(Pacjent pacjent)
        {
            pacjent.Plec = pacjentPlec(pacjent) ? Plec.Kobieta : Plec.Mężczyzna;
            this._db.Entry(pacjent).State = EntityState.Modified;
            await this._db.SaveChangesAsync();
            return pacjent;
        }

        public async Task<Pacjent> Remove(Pacjent pacjent)
        {
            if (pacjent != null)
            {
                this._db.Pacjent.Remove(pacjent);
                await this._db.SaveChangesAsync();
            }
            return pacjent;
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
            var pacjent = await GetPacjentById(id);
            pacjent.OdzialNfz = pacjent.OdzialNfz > 0 ? pacjent.OdzialNfz : null;
            pacjent.Wojewodztwo = pacjent.Wojewodztwo > 0 ? pacjent.Wojewodztwo : null;
            return pacjent;
        }

        private bool pacjentPlec(Pacjent p)
        {
            return char.ToLower(p.Imie.ElementAt(p.Imie.Length - 1)).Equals('a');
        }

        public async Task<Pacjent> GetPacjentById(int? id)
        {
            return await _db.Pacjent.Where(_ => _.ID == id).FirstOrDefaultAsync();
        }
    }
}
