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
using System.Security.Claims;

namespace GabinetStomatologiczny.Services
{
    public interface IWizytService
    {
        Task<IEnumerable<object>> GetEvents(string searchQuery, bool isLogedin, string r, string u);
        Task<bool> SaveEvent(Wizyta w, ClaimsIdentity user);
        Task<bool> DeleteEvent(Wizyta w);
    }
    public class WizytServie : IWizytService
    {
        private readonly GabinetContext db;
        private readonly IUserRoleService uR;

        public WizytServie(GabinetContext db, IUserRoleService uR)
        {
            this.db = db;
            this.uR = uR;
        }

        public async Task<bool> DeleteEvent(Wizyta w)
        {
            var status = false;

            var v = await db.Wizyta.Where(a => a.ID == w.ID).FirstOrDefaultAsync();
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
                eventsQuery = eventsQuery.Where(p => (p.Pacjent.Imie + " " + p.Pacjent.Nazwisko).ToLower().Contains(searchQuery.ToLower()));
            }
            var events = await eventsQuery.Select(_ =>
            new
            {
                _.ID,
                _.Koniec,
                _.Poczatek,
                Tytul = isLogedin ? _.Pacjent.Imie + " " + _.Pacjent.Nazwisko : "*",
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
        public async Task<bool> SaveEvent(Wizyta w, ClaimsIdentity user)
        {
            var status = true;
            if (await this.db.Wizyta.AnyAsync(_ => _.Poczatek.CompareTo(w.Poczatek) == 0 ))
            {
                status = false;
            }
            else
            {
                if (w.ID > 0)
                {
                    var v = await db.Wizyta.Where(a => a.ID == w.ID).FirstOrDefaultAsync();
                    if (v != null)
                    {
                        w.LekarzID = v.LekarzID;
                        w.PacjentID = v.PacjentID;

                        var m = Mapper.Map<Wizyta, Wizyta>(w, v);
                        m.DataDodania = DateTime.Now;

                        db.Entry(m).State = EntityState.Modified;
                    }

                }
                else
                {
                    w.LekarzID = await db.Lekarz.Select(p => p.ID).FirstOrDefaultAsync();
                    var pacjentQuery = db.Pacjent.AsQueryable();
                    var m = Mapper.Map<Wizyta, Wizyta>(w);
                    m.PacjentID = this.uR.GetUserRole(user) == "Pacjent" ?
                        await pacjentQuery.Where(_ => _.Email == user.Name).Select(_ => _.ID).FirstOrDefaultAsync() :
                        await pacjentQuery.Where(p => (p.Imie + " " + p.Nazwisko).
                    Contains(w.Pacjent.Imie + " " + w.Pacjent.Nazwisko)).Select(p => p.ID).FirstOrDefaultAsync();
                    m.DataDodania = DateTime.Now;
                    db.Wizyta.Add(m);
                }

                await this.db.SaveChangesAsync();
                status = true;

            }

            return status;
        }

        #endregion
    }
}
