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
    public interface ISugestionService
    {
        IEnumerable<object> SugestionPacjentAsync(Sugestion sugestion);
        IEnumerable<object> SugestionLekarzAsync(Sugestion sugestion);
    }
    public class SugestionService : ISugestionService
    {

        private readonly GabinetContext _db = null;

        public SugestionService(GabinetContext db)
        {
            this._db = db;
        }

        public IEnumerable<object> SugestionLekarzAsync(Sugestion sugestion)
        {
            var personList = _db.Lekarz
                .Where(p => (p.Imie + " " + p.Nazwisko)
                .ToLower().Contains(sugestion.term.ToLower()))
                .Take(5)
                .Select(p => new { label = p.Imie + " " + p.Nazwisko });

            return personList;
        }

        public IEnumerable<object> SugestionPacjentAsync(Sugestion sugestion)
        {
            var personList = _db.Pacjent
                .Where(p => (p.Imie + " " + p.Nazwisko)
                .ToLower().Contains(sugestion.term.ToLower()))
                .Take(5)
                .Select(p =>  new { label = p.Imie + " " + p.Nazwisko } );

            return personList;
        }
    }
}
