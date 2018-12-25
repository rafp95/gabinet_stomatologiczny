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
        IEnumerable<object> SugestionPacjent(Sugestion sugestion);
        IEnumerable<object> SugestionLekarz(Sugestion sugestion);
        IEnumerable<object> SugestionZabeg(Sugestion Sugestion);
        IEnumerable<object> SugestionWizyt(Sugestion sugestion);
    }
    public class SugestionService : ISugestionService
    {

        private readonly GabinetContext _db = null;

        public SugestionService(GabinetContext db)
        {
            this._db = db;
        }

        public IEnumerable<object> SugestionLekarz(Sugestion sugestion)
        {
            var personQuery = _db.Lekarz.AsQueryable();
            var personList = personQuery.Where(p => (p.Imie + " " + p.Nazwisko)
              .ToLower().Contains(sugestion.term.ToLower()))
              .Take(5)
              .Select(p => new { label = p.Imie + " " + p.Nazwisko });

            personList = personList.Count() == 0 ? personList.DefaultIfEmpty().
               Select(p => new { label = "Nie znaleziono" }).Take(1) : personList;

            return personList;
        }

        public IEnumerable<object> SugestionPacjent(Sugestion sugestion)
        {
            var personQuery = _db.Pacjent.AsQueryable();
            var personList = personQuery.Where(p => (p.Imie + " " + p.Nazwisko)
            .ToLower().Contains(sugestion.term.ToLower()))
            .Take(5)
            .Select(p => new { label = p.Imie + " " + p.Nazwisko });

            personList = personList.Count() == 0 ? personList.DefaultIfEmpty().
                Select(p => new { label = "Nie jesteś zapisany" }).Take(1) : personList;

            return personList;
        }

        public IEnumerable<object> SugestionWizyt(Sugestion sugestion)
        {
            var events = _db.Wizyta.Include(_ => _.Pacjent).AsEnumerable().
                 Select(_ => new
                 {
                     Tytul = _.Pacjent.Imie + " " + _.Pacjent.Nazwisko,
                     _.Poczatek
                 }).
                 Where(p => p.Tytul.ToLower().
                 Contains(sugestion.term.ToLower()) &&
                 p.Poczatek >= DateTime.Now).
                 OrderBy(p => p.Poczatek).
                 Take(5).
                 Select(p => new { label = p.Tytul + " " + p.Poczatek.ToString("dd.MMM.yyyy, HH:mm") ?? "" });
            events = events.Count() == 0 ? events.DefaultIfEmpty().Select(p => new { label = "Nie jesteś zapisany" }).Take(1) : events;
            return events;
        }

        public IEnumerable<object> SugestionZabeg(Sugestion Sugestion)
        {
            var zabiegQuery = _db.Zabieg.Include(_ => _.Wizyta).AsQueryable();

            var zabieg = zabiegQuery.Where(z => (z.Wizyta.Pacjent.Imie + " " +  z.Wizyta.Pacjent.Nazwisko).ToLower().Contains(Sugestion.term.ToLower()) )
                .GroupBy(_ => new { Tytul = _.Wizyta.Pacjent.Imie + " " + _.Wizyta.Pacjent.Nazwisko }).Take(5).
               Select(_ => new { label = _.Key.Tytul });

            zabieg = zabieg.Count() == 0 ? zabieg.DefaultIfEmpty().Select(z => new { label = "Brak danych" }).Take(1) : zabieg;

            return zabieg;
        }
    }
}
