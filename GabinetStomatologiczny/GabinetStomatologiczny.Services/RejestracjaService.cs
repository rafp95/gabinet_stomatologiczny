using AutoMapper;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface IRejestracjaService
    {
        Task<Osoba> CreateUserInIdentity(Osoba o);
        Task<bool> UserExist(Osoba o);
    }
    public class RejestracjaService : IRejestracjaService
    {
        private readonly GabinetContext _db;
        private readonly ISendEmailService se;
        private readonly IPacjentsService ps;
        private readonly ILekarzService ls;
        private readonly IAdminService adminS;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="se"></param>
        /// <param name="ps"></param>
        /// <param name="ls"></param>
        /// <param name="adminS"></param>
        public RejestracjaService(GabinetContext db, ISendEmailService se, IPacjentsService ps, ILekarzService ls,IAdminService adminS)
        {
            this._db = db;
            this.se = se;
            this.ps = ps;
            this.ls = ls;
            this.adminS = adminS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public async Task<Osoba> CreateUserInIdentity(Osoba o)
        {
            //rejestrajcja identity
            var roleUser = new RoleManager<IdentityRole>(
             new RoleStore<IdentityRole>(
             new ApplicationDbContext()));

            var createUser = new UserManager<ApplicationUser>(
                              new UserStore<ApplicationUser>(
                              new ApplicationDbContext()));

            await roleUser.CreateAsync(new IdentityRole(o.RoleType.ToString()));


            var user = new ApplicationUser { UserName = o.Email, Email = o.Email, EmailConfirmed = true };

            var pass = o.Haslo;

            await createUser.CreateAsync(user, pass);

            await createUser.AddToRoleAsync(user.Id, o.RoleType.ToString());
            // end rejestracja identiyy

            //
            switch (o.RoleType)
            {

                case RoleType.Administrator:
                    {
                        var admin = Mapper.Map<Administrator>(o);
                        await se.SendMail(o.Email, "Witaj, administrator " +
                          o.Imie, "Twoje dane do logowania to \n"
                           + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                        await this.adminS.CreateAdmin(admin);
                        break;
                    }
                case RoleType.Lekarz:
                    {
                        var lekarz = Mapper.Map<Lekarz>(o);
                        lekarz.DataDodania = DateTime.UtcNow;
                        await se.SendMail(o.Email, "Witaj, lekarz " +
                            o.Imie, "Twoje dane do logowania to \n"
                            + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                        await this.ls.CreateLekarz(lekarz);
                        break;
                    }
                case RoleType.Pacjent:
                    {
                        var pacjent = Mapper.Map<Pacjent>(o);
                        pacjent.DataRejestracji = DateTime.UtcNow;
                        await se.SendMail(o.Email, "Witaj, pacjencie " +
                           o.Imie, "Twoje dane do logowania to \n "
                           + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                        await ps.CreatePacjnt(pacjent);
                        break;
                    }
            }
            //

            return o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public async Task<bool> UserExist(Osoba o)
        {
            return await _db.Pacjent.AnyAsync(_ =>_.Imie == o.Imie  && _.Nazwisko == o.Nazwisko);
        }
    }
}
