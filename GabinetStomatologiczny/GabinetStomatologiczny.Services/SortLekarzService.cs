using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{

    public interface ISortLekarzService
    {
        IEnumerable<Lekarz> SortAndPaginate(PaginationType ls);
        RodzajSort RodzajSort { get; }
    }
    public class SortLekarzService : ISortLekarzService
    {
        private readonly GabinetContext _db;

        public SortLekarzService(GabinetContext db)
        {
            this._db = db;
        }

        public RodzajSort RodzajSort { get; private set; } = RodzajSort.name_asc;

        public RodzajSort SortActiveClass(RodzajSort rodzajSort)
        {
            return rodzajSort;
        }

        public IEnumerable<Lekarz> SortAndPaginate(PaginationType ls)
        {
            if (!string.IsNullOrEmpty(ls.searchString))
            {
                ls.page = 1;
            }

            var lekarz = this._db.Lekarz.AsQueryable();
            if (!string.IsNullOrEmpty(ls.searchString))
            {

                lekarz = lekarz.Where(s => (s.Imie + " " + s.Nazwisko + " " + s.NrTelefonu + " " + s.Email)
                .ToLower().Contains(ls.searchString.ToLower()));
            }

            switch (ls.sortOrder)
            {
                case RodzajSort.name_asc:
                    lekarz = lekarz.OrderBy(p => p.Imie);
                    this.RodzajSort = RodzajSort.name_asc;
                    break;
                case RodzajSort.name_desc:
                    lekarz = lekarz.OrderByDescending(p => p.Imie);
                    this.RodzajSort = RodzajSort.name_desc;
                    break;
                case RodzajSort.surname_asc:
                    lekarz = lekarz.OrderBy(p => p.Nazwisko);
                    this.RodzajSort = RodzajSort.surname_asc;
                    break;
                case RodzajSort.surname_desc:
                    lekarz = lekarz.OrderByDescending(p => p.Nazwisko);
                    this.RodzajSort = RodzajSort.surname_desc;
                    break;
                case RodzajSort.nrtel_asc:
                    lekarz = lekarz.OrderBy(p => p.NrTelefonu);
                    this.RodzajSort = RodzajSort.nrtel_asc;
                    break;
                case RodzajSort.nrtel_desc:
                    lekarz = lekarz.OrderByDescending(p => p.NrTelefonu);
                    this.RodzajSort = RodzajSort.nrtel_desc;
                    break;
                default:
                    lekarz = lekarz.OrderBy(p => p.Imie);
                    break;
            }

            ls.pageSize = 10;
            ls.pageNumber = (ls.page ?? 1);

            return lekarz.ToPagedList(ls.pageNumber, ls.pageSize);
        }
    }
}
