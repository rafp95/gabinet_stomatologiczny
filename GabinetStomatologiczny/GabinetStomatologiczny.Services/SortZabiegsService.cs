using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface ISortZabiegsService
    {
        IEnumerable<Zabieg> SortAndPaginate(ZabiegsSort ps);
        RodzajSort RodzajSort { get; }
    }

    public class SortZabiegsService : ISortZabiegsService
    {
        private readonly GabinetContext _db;

        public SortZabiegsService(GabinetContext db)
        {
            this._db = db;
        }
        public RodzajSort RodzajSort { get; private set; } = RodzajSort.name_asc;

        public IEnumerable<Zabieg> SortAndPaginate(ZabiegsSort ps)
        {
            if (!string.IsNullOrEmpty(ps.searchString))
            {
                ps.page = 1;
            }

            var zabieg = _db.Zabieg.Include(z => z.Wizyta).Include(p => p.Wizyta.Pacjent);

            if (!string.IsNullOrEmpty(ps.searchString))
            {

                zabieg = zabieg.Where(s => (s.Wizyta.Tytul + " " + s.TypZabiegu + " " + s.CenaZabiegu)
                .ToLower().Contains(ps.searchString.ToLower()));
            }

            zabieg = zabieg.OrderBy(_ => _.Wizyta.Tytul);

            ps.pageSize = 10;
            ps.pageNumber = (ps.page ?? 1);

            return zabieg.ToPagedList(ps.pageNumber, ps.pageSize);
        }
    }
}
