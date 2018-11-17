using AutoMapper;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny.Services;
using GabinetStomatologiczny.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GabinetStomatologiczny.Controllers
{
    [Authorize(Roles = "Administrator,Lekarz")]
    public class RejestracjaController : BaseController
    {
        private readonly new GabinetContext db;
        private readonly ISendEmailService se;
        private readonly IPacjentsService ps;
        private readonly ILekarzService ls;
        private readonly IAdminService adminS;

        public RejestracjaController(ISendEmailService se, IPacjentsService ps, ILekarzService ls, GabinetContext db, IAdminService adminS)
        {
            this.se = se;
            this.db = db;
            this.ps = ps;
            this.ls = ls;
            this.adminS = adminS;
        }

        // GET: Rejestracja
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Rejestracja użytkownika
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        #region
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Osoba o)
        {
            try
            {
                if (!o.Haslo.Equals(o.PowtorzHaslo))
                {
                    ModelState.AddModelError("Haslo", "Podane hasła są różne");
                }
                else if (ModelState.IsValid)
                {
                    var roleUser = new RoleManager<IdentityRole>(
                                 new RoleStore<IdentityRole>(
                                 new ApplicationDbContext()));

                    var createUser = new UserManager<ApplicationUser>(
                                      new UserStore<ApplicationUser>(
                                      new ApplicationDbContext()));

                    roleUser.Create(new IdentityRole(o.TypPracownika.ToString()));


                    var user = new ApplicationUser { UserName = o.Email, Email = o.Email, EmailConfirmed = true };

                    var pass = o.Haslo;

                    createUser.Create(user, pass);

                    createUser.AddToRole(user.Id, o.TypPracownika.ToString());

                    TempData["ok"] = o.Imie + " typ konta " + o.TypPracownika;

                    switch (o.TypPracownika)
                    {

                        case TypPracownika.Administrator:
                            {
                                var admin = Mapper.Map<DAO.Models.Administrator>(o);
                                await se.SendMail(o.Email, "Witaj, administrator " +
                                  o.Imie, "Twoje dane do logowania to \n"
                                   + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                                await this.adminS.CreateAdmin(admin);
                                break;
                            }
                        case TypPracownika.Lekarz:
                            {
                                var lekarz = Mapper.Map<Lekarz>(o);
                                lekarz.DataDodania = DateTime.Now;
                                await se.SendMail(o.Email, "Witaj, lekarz " +
                                    o.Imie, "Twoje dane do logowania to \n"
                                    + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                                await this.ls.CreateLekarz(lekarz);
                                break;
                            }
                        case TypPracownika.Pacjent:
                            {
                                var pacjent = Mapper.Map<Pacjent>(o);
                                pacjent.DataRejestracji = DateTime.Now;
                                await se.SendMail(o.Email, "Witaj, pacjencie " +
                                   o.Imie, "Twoje dane do logowania to \n "
                                   + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                                await ps.CreatePacjnt(pacjent);
                                break;
                            }
                    }
                    return RedirectToAction("Index", "Rejestracja");
                }
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Email", "Podany email jest już zajęty");
            }

            return View(o);
        }
        #endregion
    }
}