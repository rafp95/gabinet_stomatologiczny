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
    public interface IGenerateZabiegsPdf
    {
        IQueryable<Zabieg> GetPacjents(string user);
    }
    public class GenerateZabiegsPdf : IGenerateZabiegsPdf
    {
        private readonly GabinetContext _db;

        public GenerateZabiegsPdf(GabinetContext db)
        {
            this._db = db;
        }

        public IQueryable<Zabieg> GetPacjents(string user)
        {
            var zabieg = _db.Zabieg.Include(z => z.Wizyta);
            zabieg.Include(p => p.Wizyta.Pacjent);
            zabieg = zabieg.Where(_ => _.Wizyta.Pacjent.Email == user);
            return zabieg;
        }
    }
}
