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
        public RodzajSort RodzajSort { get; private set; } = RodzajSort.title_asc;

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
            RodzajSort = ps.sortOrder;
            switch (ps.sortOrder)
            {
                case RodzajSort.title_asc:
                    zabieg = zabieg.OrderBy(_ => _.Wizyta.Tytul);
                    break;
                case RodzajSort.title_desc:
                    zabieg = zabieg.OrderByDescending(_ => _.Wizyta.Tytul);
                    break;
                case RodzajSort.zabiegT_asc:
                    zabieg = zabieg.OrderBy(_ => _.TypZabiegu);
                    break;
                case RodzajSort.zabiegT_desc:
                    zabieg = zabieg.OrderByDescending(_ => _.TypZabiegu);
                    break;
                case RodzajSort.cenaZ_asc:
                    zabieg = zabieg.OrderBy(_ => _.CenaZabiegu);
                    break;
                case RodzajSort.cenaZ_desc:
                    zabieg = zabieg.OrderByDescending(_ => _.CenaZabiegu);
                    break;
                default:
                    zabieg = zabieg.OrderBy(_ => _.Wizyta.Tytul);
                    break;
            }

            ps.pageSize = 10;
            ps.pageNumber = (ps.page ?? 1);

            return zabieg.ToPagedList(ps.pageNumber, ps.pageSize);
        }
    }
}
