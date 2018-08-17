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
    public interface ISortZabiegsService
    {
        IEnumerable<Zabieg> SortAndPaginate(PaginationType ps);
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

        public IEnumerable<Zabieg> SortAndPaginate(PaginationType ps)
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

            switch (ps.sortOrder)
            {
                case RodzajSort.title_asc:
                    zabieg = zabieg.OrderBy(_ => _.Wizyta.Tytul);
                    RodzajSort = ps.sortOrder;
                    break;
                case RodzajSort.title_desc:
                    zabieg = zabieg.OrderByDescending(_ => _.Wizyta.Tytul);
                    RodzajSort = ps.sortOrder;
                    break;
                case RodzajSort.zabiegT_asc:
                    zabieg = zabieg.OrderBy(_ => _.TypZabiegu);
                    RodzajSort = ps.sortOrder;
                    break;
                case RodzajSort.zabiegT_desc:
                    zabieg = zabieg.OrderByDescending(_ => _.TypZabiegu);
                    RodzajSort = ps.sortOrder;
                    break;
                case RodzajSort.cenaZ_asc:
                    zabieg = zabieg.OrderBy(_ => _.CenaZabiegu);
                    RodzajSort = ps.sortOrder;
                    break;
                case RodzajSort.cenaZ_desc:
                    zabieg = zabieg.OrderByDescending(_ => _.CenaZabiegu);
                    RodzajSort = ps.sortOrder;
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
