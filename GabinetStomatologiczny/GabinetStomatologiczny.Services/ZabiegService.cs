using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface IZabiegService
    {
        IQueryable<object> GetAllTooth(int id);
        Pacjent GetPacjentFullName(int id);
        void SaveTooth(Zab zab,int userID);
        void DeleteTooth(Zab zab, int userID);
    }
    public class ZabiegService : IZabiegService
    {
        private readonly GabinetContext _db;

        public ZabiegService(GabinetContext db)
        {
            this._db = db;
        }

        public void DeleteTooth(Zab zab, int userID)
        {
            var idWizyta = _db.Wizyta.FirstOrDefault(_ => _.Pacjent.ID == userID).ID;
            var idZabieg = _db.Zabieg.FirstOrDefault(_ => _.Wizyta.ID == idWizyta).ID;
            Zab z = _db.Zab.FirstOrDefault(_ => _.IdTooth == zab.IdTooth);
            _db.Zab.Remove(z);
            _db.SaveChanges();
        }

        public IQueryable<object> GetAllTooth(int id)
        {
            var idWizyta = _db.Wizyta.First(_ => _.Pacjent.ID == id).ID;
            var tooth = _db.Zabieg.Where(_ => _.WizytaID == idWizyta && _.Wizyta.PacjentID == id).Select(_ => _.Zeby);

            return tooth;
        }

        public Pacjent GetPacjentFullName(int id)
        {
           return _db.Pacjent.FirstOrDefault(_ => _.ID == id);
        }

        public void SaveTooth(Zab zab,int userID)
        {
            var idWizyta = _db.Wizyta.First(_ => _.Pacjent.ID == userID).ID;
            var tooth = _db.Zabieg.Where(_ => _.Wizyta.ID == idWizyta).Select(_ => _.Zeby);
            var idZabieg = _db.Zabieg.FirstOrDefault(_ => _.Wizyta.ID == idWizyta).ID;

            zab.ZabiegID = idZabieg;

            _db.Zab.Add(zab);
            _db.SaveChanges();
        }
    }
}
