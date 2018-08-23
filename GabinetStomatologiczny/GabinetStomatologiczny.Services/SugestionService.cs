using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface ISugestionService
    {
        IQueryable<object> SugestionPacjent(Sugestion sugestion);
        IQueryable<object> SugestionLekarz(Sugestion sugestion);
        IQueryable<object> SugestionZabeg(Sugestion Sugestion);
        IEnumerable<object> SugestionWizyt(Sugestion sugestion);
    }
    public class SugestionService : ISugestionService
    {

        private readonly GabinetContext _db = null;

        public SugestionService(GabinetContext db)
        {
            this._db = db;
        }

        public IQueryable<object> SugestionLekarz(Sugestion sugestion)
        {
            var personList = _db.Lekarz
                .Where(p => (p.Imie + " " + p.Nazwisko)
                .ToLower().Contains(sugestion.term.ToLower()))
                .Take(5)
                .Select(p => new { label = p.Imie + " " + p.Nazwisko });

            personList = personList.Count() == 0 ? personList.DefaultIfEmpty().
               Select(p => new { label = "Nie znaleziono" }).Take(1) : personList;

            return personList;
        }

        public IQueryable<object> SugestionPacjent(Sugestion sugestion)
        {
            var personList = _db.Pacjent
                .Where(p => (p.Imie + " " + p.Nazwisko)
                .ToLower().Contains(sugestion.term.ToLower()))
                .Take(5)
                .Select(p =>  new { label = p.Imie + " " + p.Nazwisko } );

            personList = personList.Count() == 0 ? personList.DefaultIfEmpty().
                Select(p => new { label = "Nie jesteś zapisany" }).Take(1) : personList;

            return personList;
        }

        public IEnumerable<object> SugestionWizyt(Sugestion sugestion)
        {
             var events = _db.Wizyta.ToList().
                 Select(_ => new
                 {
                     _.Tytul,
                     _.Poczatek
                 }).
                 Where(p => p.Tytul.ToLower().
                 Contains(sugestion.term.ToLower()) &&
                 p.Poczatek >= DateTime.Now).
                 OrderBy(p => p.Poczatek).
                 Take(5).
                 Select(p => new { label = p.Tytul + " " + p.Poczatek.ToString("dd.MMM.yyyy, H:mm") });
            events = events.Count() == 0 ? events.DefaultIfEmpty().Select(p => new { label = "Nie jesteś zapisany" }).Take(1) : events;
            return events;
        }

        public IQueryable<object> SugestionZabeg(Sugestion Sugestion)
        {
            var zabieg = _db.Zabieg.Include(_ => _.Wizyta).
                Where(z => z.Wizyta.Tytul.ToLower().Contains(Sugestion.term.ToLower())).
                Take(5).Select(_ => new {label = _.Wizyta.Tytul});

            zabieg = zabieg.Count() == 0 ? zabieg.DefaultIfEmpty().Select(p => new { label = "Brak danych" }).Take(1) : zabieg;

            return zabieg;
        }
    }
}
