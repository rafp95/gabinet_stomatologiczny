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
        IQueryable<object> SugestionPacjent(Sugestion sugestion);
        IQueryable<object> SugestionLekarz(Sugestion sugestion);
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

            return personList;
        }

        public IQueryable<object> SugestionPacjent(Sugestion sugestion)
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
