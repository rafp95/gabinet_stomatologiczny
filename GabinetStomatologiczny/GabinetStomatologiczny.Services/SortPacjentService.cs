using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Models;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface ISortPacjentService
    {
        IEnumerable<Pacjent> SortAndPaginate(PacjentSort ps);
    }

    public class SortPacjentService : ISortPacjentService
    {

        private readonly GabinetContext _db;

        public SortPacjentService(GabinetContext db)
        {
            this._db = db;
        }

        public IEnumerable<Pacjent> SortAndPaginate(PacjentSort ps)
        {
            var pacjent = this._db.Pacjent.AsQueryable();
            if (!string.IsNullOrEmpty(ps.searchString))
            {
                pacjent = pacjent.Where(s => (s.Imie + " " + s.Nazwisko + " " + s.NrTelefonu + " " + s.Email + " " + s.Plec)
                .ToLower().Contains(ps.searchString.ToLower()));
            }
            pacjent = pacjent.OrderBy(p => p.Imie);
            return pacjent.ToPagedList(ps.pageNumber,ps.pageSize);
        }
    }
}
