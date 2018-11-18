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
        Task <Zab> DeleteTooth(Zab zab, int userID);
        Task<Zabieg> FindZabieg(int? id);
        Task<Zabieg> EditZabeg(Zabieg zabieg);
    }
    public class ZabiegService : IZabiegService
    {
        private readonly GabinetContext _db;

        public ZabiegService(GabinetContext db)
        {
            this._db = db;
        }

        public async Task<Zab> DeleteTooth(Zab zab, int userID)
        {
            var idWizyta = await _db.Wizyta.Where(_ => _.Pacjent.ID == userID).FirstOrDefaultAsync();
            Zab z = _db.Zab.FirstOrDefault(_ => _.IdTooth == zab.IdTooth);
            _db.Zab.Remove(z);
            await _db.SaveChangesAsync();
            return z;
        }

        public async Task<Zabieg> EditZabeg(Zabieg zabieg)
        {
            _db.Entry(zabieg).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return zabieg;
        }

        public async Task<Zabieg> FindZabieg(int? id)
        {
            return await _db.Zabieg.Include(_ => _.Wizyta).Where(_ => _.ID == id).FirstOrDefaultAsync();
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
