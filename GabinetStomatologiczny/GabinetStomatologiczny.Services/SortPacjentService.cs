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
        RodzajSort RodzajSort { get; }
    }

    public class SortPacjentService : ISortPacjentService
    {

        private readonly GabinetContext _db;

        public SortPacjentService(GabinetContext db)
        {
            this._db = db;
        }

        public RodzajSort RodzajSort { get; private set; } = RodzajSort.name_asc;

        public RodzajSort SortActiveClass(RodzajSort rodzajSort)
        {
            return rodzajSort;
        }

        public IEnumerable<Pacjent> SortAndPaginate(PacjentSort ps)
        {
            if (!string.IsNullOrEmpty(ps.searchString))
            {
                ps.page = 1;
            }
             
            var pacjent = this._db.Pacjent.AsQueryable();
            if (!string.IsNullOrEmpty(ps.searchString))
            {

                pacjent = pacjent.Where(s => (s.Imie + " " + s.Nazwisko + " " + s.NrTelefonu + " " + s.Email + " " + s.Plec)
                .ToLower().Contains(ps.searchString.ToLower()));
            }

            switch (ps.sortOrder)
            {
                case RodzajSort.name_asc:
                    pacjent = pacjent.OrderBy(p => p.Imie);
                    this.RodzajSort = RodzajSort.name_asc;
                    break;
                case RodzajSort.name_desc:
                    pacjent = pacjent.OrderByDescending(p => p.Imie);
                    this.RodzajSort = RodzajSort.name_desc;
                    break;
                case RodzajSort.surname_asc:
                    pacjent = pacjent.OrderBy(p => p.Nazwisko);
                    this.RodzajSort = RodzajSort.surname_asc;
                    break;
                case RodzajSort.surname_desc:
                    pacjent = pacjent.OrderByDescending(p => p.Nazwisko);
                    this.RodzajSort = RodzajSort.surname_desc;
                    break;
                case RodzajSort.pesel_asc:
                    pacjent = pacjent.OrderBy(p => p.Pesel);
                    this.RodzajSort = RodzajSort.pesel_asc;
                    break;
                case RodzajSort.nrtel_asc:
                    pacjent = pacjent.OrderBy(p => p.NrTelefonu);
                    this.RodzajSort = RodzajSort.nrtel_asc;
                    break;
                case RodzajSort.nrtel_desc:
                    pacjent = pacjent.OrderByDescending(p => p.NrTelefonu);
                    this.RodzajSort = RodzajSort.nrtel_desc;
                    break;
                case RodzajSort.pesel_desc:
                    pacjent = pacjent.OrderByDescending(p => p.Pesel);
                    this.RodzajSort = RodzajSort.pesel_desc;
                    break;
                case RodzajSort.plec_asc:
                    pacjent = pacjent.OrderBy(p => p.Plec);
                    this.RodzajSort = RodzajSort.plec_asc;
                    break;
                case RodzajSort.plec_desc:
                    pacjent = pacjent.OrderByDescending(p => p.Plec);
                    this.RodzajSort = RodzajSort.plec_desc;
                    break;
                case RodzajSort.dataurodzenia_asc:
                    pacjent = pacjent.OrderBy(p => p.DataUrodzenia);
                    this.RodzajSort = RodzajSort.dataurodzenia_asc;
                    break;
                case RodzajSort.dataurodzenia_desc:
                    pacjent = pacjent.OrderByDescending(p => p.DataUrodzenia);
                    this.RodzajSort = RodzajSort.dataurodzenia_desc;
                    break;
                case RodzajSort.datarejestracji_asc:
                    pacjent = pacjent.OrderBy(p => p.DataRejestracji);
                    this.RodzajSort = RodzajSort.datarejestracji_asc;
                    break;
                case RodzajSort.datarejestracji_desc:
                    pacjent = pacjent.OrderByDescending(p => p.DataRejestracji);
                    this.RodzajSort = RodzajSort.datarejestracji_desc;
                    break;
                default:
                    pacjent = pacjent.OrderBy(p => p.Imie);
                    break;
            }

            ps.pageSize = 10;
            ps.pageNumber = (ps.page ?? 1);

            return pacjent.ToPagedList(ps.pageNumber,ps.pageSize);
        }
    }
}
