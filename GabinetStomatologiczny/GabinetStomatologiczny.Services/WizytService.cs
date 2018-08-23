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
    public interface IWizytService
    {
        Task<IEnumerable<object>> GetEvents(string searchQuery, bool isLogedin, string r, string u);
    }
    public class WizytServie : IWizytService
    {
        private readonly GabinetContext db;

        public WizytServie(GabinetContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<object>> GetEvents(string searchQuery, bool isLogedin, string r, string u)
        {
            var eventsQuery = db.Wizyta.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                eventsQuery = eventsQuery.Where(p => p.Tytul.Contains(searchQuery));
            }
            var events = await eventsQuery.Select(_ =>
            new
            {
                _.ID,
                _.Koniec,
                _.Poczatek,
                Tytul = isLogedin ? _.Tytul : "*",
                p = isLogedin ?
                    new
                    {
                        _.Pacjent.NrTelefonu,
                        _.Pacjent.Email,
                        activeUser = u,
                        role = r
                    } :
                    null,
            }).ToListAsync();

            return events;
        }
    }
}
