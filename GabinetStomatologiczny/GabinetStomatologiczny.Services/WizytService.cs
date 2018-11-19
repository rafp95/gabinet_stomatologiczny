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
using AutoMapper;

namespace GabinetStomatologiczny.Services
{
    public interface IWizytService
    {
        Task<IEnumerable<object>> GetEvents(string searchQuery, bool isLogedin, string r, string u);
        Task <bool> SaveEvent(Wizyta w,string user);
        Task<bool> DeleteEvent(Wizyta w);
    }
    public class WizytServie : IWizytService
    {
        private readonly GabinetContext db;

        public WizytServie(GabinetContext db)
        {
            this.db = db;
        }

        public async Task<bool> DeleteEvent(Wizyta w)
        {
            var status = false;

            var v = db.Wizyta.Where(a => a.ID == w.ID).FirstOrDefault();
            if (v != null)
            {
                db.Wizyta.Remove(v);
                await db.SaveChangesAsync();
                status = true;
            }
            return status;
        }

        public async Task<IEnumerable<object>> GetEvents(string searchQuery, bool isLogedin, string r, string u)
        {
            var eventsQuery = db.Wizyta.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                eventsQuery = eventsQuery.Where(p => p.Tytul.Contains(searchQuery));
            }
            var events = await eventsQuery.Select(_ =>
            new
            {
                _.ID,
                _.Koniec,
                _.Poczatek,
                Tytul = isLogedin ? _.Tytul : "*",
                p = isLogedin ?
                    new
                    {
                        _.Pacjent.NrTelefonu,
                        _.Pacjent.Email,
                        _.PacjentID,
                        activeUser = u,
                        role = r
                    } :
                    null,
            }).ToListAsync();

            return events;
        }

        /// <summary>
        /// Save event
        /// </summary>
        /// <param name="w"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        #region Save event
        public async Task<bool> SaveEvent(Wizyta w, string user)
        {
            var status = false;
            if(!this.db.Wizyta.Any(_ => _.Poczatek.CompareTo(w.Poczatek) == 0))
            {
                try
                {
                    if (w.ID > 0)
                    {
                        var v = await db.Wizyta.Where(a => a.ID == w.ID).FirstOrDefaultAsync();
                        if (v != null)
                        {
                            w.LekarzID = v.LekarzID;
                            w.PacjentID = v.PacjentID;
                            if (!string.IsNullOrEmpty(v.Tytul))
                            {
                                w.Tytul = v.Tytul;
                            }
                            var m = Mapper.Map<Wizyta, Wizyta>(w, v);
                            m.DataDodania = DateTime.Now;
                            if (string.IsNullOrEmpty(m.Tytul))
                            {
                                m.Tytul =await db.Pacjent.Where(p => p.Email == user).
                                    Select(p => p.Imie + " " + p.Nazwisko).FirstOrDefaultAsync();
                            }

                            db.Entry(m).State = EntityState.Modified;
                        }

                    }
                    else
                    {
                        w.PacjentID = await db.Pacjent.Where(p => p.Email == user).Select(p => p.ID).FirstOrDefaultAsync();
                        w.LekarzID = await db.Lekarz.Select(p => p.ID).FirstOrDefaultAsync();
                        var m = Mapper.Map<Wizyta, Wizyta>(w);

                        if (m.Tytul != null)
                        {
                            var pacjent = await db.Pacjent.Where(_ => (_.Imie + " " + _.Nazwisko).Equals(w.Tytul)).FirstOrDefaultAsync();
                            m.PacjentID = pacjent.ID;
                        }
                        else
                        {
                            m.Tytul = await db.Pacjent.Where(p => p.Email == user).Select(p => p.Imie + " " + p.Nazwisko).FirstOrDefaultAsync();
                        }

                        m.DataDodania = DateTime.Now;
                        db.Wizyta.Add(m);

                    }
                    status = true;
                    await this.db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    status = false;
                }
            }

            return status;
        }

        #endregion
    }
}
