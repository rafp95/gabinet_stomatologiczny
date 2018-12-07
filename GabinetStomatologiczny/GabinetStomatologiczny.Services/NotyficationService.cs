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
    public interface INotyficationService
    {
        Task<IEnumerable<object>> GetWizyts(DateTime afterDate);
    }

    public class NotyficationService : INotyficationService
    {
        private readonly GabinetContext db;
        public NotyficationService(GabinetContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<object>> GetWizyts(DateTime afterDate)
        {
            var wizytaQuery = db.Wizyta.AsQueryable();
            var wizyta = await wizytaQuery.Where(_ => _.DataDodania >= afterDate).OrderByDescending(_ => _.DataDodania)
                .Select(_ => new {_.Poczatek,tytul = _.Pacjent.Imie + " " + _.Pacjent.Nazwisko }).ToListAsync();
            return wizyta;

        }

    }
}
