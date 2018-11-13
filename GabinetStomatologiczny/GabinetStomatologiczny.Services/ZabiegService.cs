using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface IZabiegService
    {
        Task<IEnumerable<object>> GetAllTooth(int id);
        Task <Pacjent> GetPacjentById(int id);
        Task <Zab> SaveTooth(Zab zab,int userID);
        void DeleteTooth(Zab zab, int userID);
    }
    public class ZabiegService : IZabiegService
    {
        private readonly GabinetContext _db;

        public ZabiegService(GabinetContext db)
        {
            this._db = db;
        }

        public async void DeleteTooth(Zab zab, int userID)
        {
            var idWizyta = _db.Wizyta.FirstOrDefault(_ => _.Pacjent.ID == userID).ID;
            var idZabieg = _db.Zabieg.FirstOrDefault(_ => _.Wizyta.ID == idWizyta).ID;
            Zab z = _db.Zab.FirstOrDefault(_ => _.IdTooth == zab.IdTooth);
            _db.Zab.Remove(z);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<object>> GetAllTooth(int id)
        {
            var idWizyta = await _db.Wizyta.FirstAsync(_ => _.Pacjent.ID == id);
            var tooth = _db.Zabieg.Where(_ => _.WizytaID == idWizyta.ID && _.Wizyta.PacjentID == id).AsQueryable();
            var getAllTooth = await tooth.Select(_ => _.Zeby).ToListAsync();
            return getAllTooth;
        }

        public async Task<Pacjent> GetPacjentById(int id)
        {
            return await _db.Pacjent.FirstOrDefaultAsync(_ => _.ID == id);
        }

        public async Task<Zab> SaveTooth(Zab zab,int userID)
        {
            var idWizyta = await _db.Wizyta.FirstOrDefaultAsync(_ => _.Pacjent.ID == userID);
            var tooth = _db.Zabieg.Where(_ => _.Wizyta.ID == idWizyta.ID).Select(_ => _.Zeby).AsQueryable();
            var idZabieg = await _db.Zabieg.FirstOrDefaultAsync(_ => _.Wizyta.ID == idWizyta.ID);

            zab.ZabiegID = idZabieg.ID;
            zab.TreatmentDate = DateTime.Now;

            _db.Zab.Add(zab);
            await _db.SaveChangesAsync();
            return zab;

        }
    }
}
