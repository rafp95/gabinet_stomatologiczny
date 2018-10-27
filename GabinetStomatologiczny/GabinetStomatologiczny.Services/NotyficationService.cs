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
        Task<List<Wizyta>> GetWizyts(DateTime afterDate);
    }

    public class NotyficationService : INotyficationService
    {
        private readonly GabinetContext db;
        public NotyficationService(GabinetContext db)
        {
            this.db = db;
        }
        public async Task<List<Wizyta>> GetWizyts(DateTime afterDate)
        {
            var wizyta = db.Wizyta.AsQueryable().
            Where(_ => _.DataDodania >= afterDate).OrderByDescending(_ => _.DataDodania);
            return await wizyta.ToListAsync();

        }

    }
}
