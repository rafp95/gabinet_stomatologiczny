using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface ISortZabiegsService
    {
        IEnumerable<Zabieg> SortAndPaginate(PaginationType ps, ClaimsIdentity user);
        RodzajSort RodzajSort { get; }
    }

    public class SortZabiegsService : ISortZabiegsService
    {
        private readonly GabinetContext _db;
        private readonly IUserRoleService _userRole;

        public SortZabiegsService(GabinetContext db, IUserRoleService userRole)
        {
            this._db = db;
            this._userRole = userRole;
        }
        public RodzajSort RodzajSort { get; private set; } = RodzajSort.title_asc;

        public IEnumerable<Zabieg> SortAndPaginate(PaginationType ps, ClaimsIdentity user)
        {
            if (!string.IsNullOrEmpty(ps.searchString))
            {
                ps.page = 1;
            }
            else
            {
                ps.searchString = ps.currentFilter;
            }

            var zabieg = _db.Zabieg.Include(z => z.Wizyta).Include(p => p.Wizyta.Pacjent);

            string role = this._userRole.GetUserRole(user);

            if (role != null && role == "Pacjent")
            {
                zabieg = zabieg.Where(_ => _.Wizyta.Pacjent.Email == user.Name);
            }

            if (!string.IsNullOrEmpty(ps.searchString))
            {

                zabieg = zabieg.Where(s => (s.Wizyta.Pacjent.Imie + " " + s.Wizyta.Pacjent.Nazwisko + " " + s.TypZabiegu + " " + s.CenaZabiegu)
                .ToLower().Contains(ps.searchString.ToLower()));
            }
            else
            {
                ps.searchString = ps.currentFilter;
            }

            switch (ps.sortOrder)
            {
                case RodzajSort.title_asc:
                    zabieg = zabieg.OrderBy(_ => _.Wizyta.Pacjent.Imie).ThenBy(_ => _.Wizyta.Pacjent.Nazwisko);
                    RodzajSort = ps.sortOrder;
                    break;
                case RodzajSort.title_desc:
                    zabieg = zabieg.OrderByDescending(_ => _.Wizyta.Pacjent.Imie).ThenByDescending(_ => _.Wizyta.Pacjent.Nazwisko);
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
                case RodzajSort.dataZ_asc:
                    zabieg = zabieg.OrderBy(_ => _.ZabiegDate);
                    RodzajSort = ps.sortOrder;
                    break;
                case RodzajSort.dataZ_desc:
                    zabieg = zabieg.OrderByDescending(_ => _.ZabiegDate);
                    RodzajSort = ps.sortOrder;
                    break;
                default:
                    zabieg = zabieg.OrderBy(_ => _.Wizyta.Pacjent.Imie);
                    break;
            }

            ps.pageSize = 10;
            ps.pageNumber = (ps.page ?? 1);

            return zabieg.ToPagedList(ps.pageNumber, ps.pageSize);
        }
    }
}
