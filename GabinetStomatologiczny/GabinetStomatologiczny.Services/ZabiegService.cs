using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Models;
using PagedList;
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
        Task<Zab> SaveTooth(Zab zab, int userID);
        Task<Zab> DeleteTooth(Zab zab, int userID);
        Task<Zabieg> FindZabieg(int? id);
        Task<Zabieg> EditZabeg(Zabieg zabieg);
        Task<IEnumerable<Zabieg>> PacejntZabiegHistory(PaginationType ps, int id);
        Task<Zabieg> ZabiegCreate(Zabieg zabieg);
        Task<Zabieg> DeleteZabieg(int zabieg);
        Task<ZabDescriptions> ZabDescription(Zab zab, ZabDescriptions zDesc, int id);
        Task<object> GetHistoryToothDescr(string idTooth, int pacejntID);
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
            var idZabeg = await _db.Zabieg.Where(_ => _.Wizyta.ID == idWizyta.ID).FirstOrDefaultAsync();
            Zab z = await _db.Zab.Where(_ => _.IdTooth == zab.IdTooth && _.ZabiegID == idZabeg.ID).FirstOrDefaultAsync();
            _db.Zab.Remove(z);
            await _db.SaveChangesAsync();
            return z;
        }

        public async Task<Zabieg> DeleteZabieg(int id)
        {
            var zabieg = await FindZabieg(id);
            _db.Zabieg.Remove(zabieg);
            await _db.SaveChangesAsync();
            return zabieg;
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

        public async Task<object> GetHistoryToothDescr(string idTooth, int pacejntID)
        {
            var pacejntWizytaQuery = await _db.Wizyta.
                Where(_ => _.PacjentID == pacejntID).Select(_ => _.ID).FirstOrDefaultAsync();

            var pacejntZabiegQuery = await _db.Zabieg.
                Where(_ => _.WizytaID == pacejntWizytaQuery).
                Select(_ => _.ID).FirstOrDefaultAsync();

            var zabZabiegQuery = await _db.Zab.Where(_ => _.ZabiegID == pacejntZabiegQuery && _.IdTooth == idTooth).
                Select(_ => _.ID).FirstOrDefaultAsync();

            var zabDescriptionQuery = await _db.ZabDescriptions.Where(_ => _.ZabID == zabZabiegQuery).
                Select(_ => new {description = _.Description, dateZabieg =  _.ZabDateTreatment }).ToListAsync();


            //var zebyHistory = await zebyHistoryQuery.Select(_ =>  _.Description ).ToListAsync();
            return zabDescriptionQuery;
        }

        public async Task<IEnumerable<Zabieg>> PacejntZabiegHistory(PaginationType ps, int id)
        {
            var zabiegs = await _db.Zabieg.
               Include(_ => _.Wizyta).Include(_ => _.Wizyta.Pacjent).
               Where(_ => _.Wizyta.PacjentID == id).
               OrderByDescending(_ => _.ZabiegDate).ToListAsync();

            ps.pageSize = 10;
            ps.pageNumber = (ps.page ?? 1);

            return zabiegs.ToPagedList(ps.pageNumber, ps.pageSize);
        }

        public async Task<Zab> SaveTooth(Zab zab, int userID)
        {
            var idWizyta = await _db.Wizyta.Where(_ => _.Pacjent.ID == userID).FirstOrDefaultAsync();
            var idZabieg = await _db.Zabieg.Where(_ => _.Wizyta.ID == idWizyta.ID).FirstOrDefaultAsync();

            zab.ZabiegID = idZabieg.ID;
            zab.TreatmentDate = DateTime.Now;

            _db.Zab.Add(zab);
            await _db.SaveChangesAsync();
            return zab;

        }

        public async Task<ZabDescriptions> ZabDescription(Zab zab, ZabDescriptions zDesc, int id)
        {
            var pacejntWizytaQuery = await _db.Wizyta.
                Where(_ => _.PacjentID == id).Select(_ => _.ID).FirstOrDefaultAsync();

            var pacejntZabiegQuery = await _db.Zabieg.
                Where(_ => _.WizytaID == pacejntWizytaQuery).
                Select(_ => _.ID).FirstOrDefaultAsync();

            var zabZabiegQuery = await _db.Zab.Where(_ => _.ZabiegID == pacejntZabiegQuery && _.IdTooth == zab.IdTooth).FirstOrDefaultAsync();

            zDesc.ZabID = zabZabiegQuery.ID;
            zDesc.ZabDateTreatment = DateTime.Now;

            _db.ZabDescriptions.Add(zDesc);
            await _db.SaveChangesAsync();

            return zDesc;
        }

        public async Task<Zabieg> ZabiegCreate(Zabieg zabieg)
        {
            var pacjet = await _db.Pacjent.Where(p => (p.Imie + " " + p.Nazwisko).ToLower().
                     Contains(zabieg.SearchQuery.ToLower())).Select(p => p.ID).FirstOrDefaultAsync();

            var wizyta = await _db.Wizyta.
                Where(p => p.PacjentID == pacjet).Select(p => p.ID).FirstOrDefaultAsync();

            zabieg.WizytaID = wizyta;
            zabieg.ZabiegDate = DateTime.Now;
            _db.Zabieg.Add(zabieg);
            await _db.SaveChangesAsync();
            return zabieg;
        }
    }
}
