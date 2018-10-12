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
        Task<IEnumerable<Zabieg>> GetPacjents(string user);
    }
    public class GenerateZabiegsPdf : IGenerateZabiegsPdf
    {
        private readonly GabinetContext _db;

        public GenerateZabiegsPdf(GabinetContext db)
        {
            this._db = db;
        }

        public async Task<IEnumerable<Zabieg>> GetPacjents(string user)
        {
            var zabieg = await _db.Zabieg.Include(z => z.Wizyta)
                .AsQueryable().Where(_ => _.Wizyta.Pacjent.Email == user).ToListAsync();

            zabieg = zabieg.Count() > 0 ? zabieg : null;

            return zabieg;
        }
    }
}
