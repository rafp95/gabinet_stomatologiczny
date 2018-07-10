using AutoMapper;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GabinetStomatologiczny.Controllers
{
    [Authorize(Roles = "Administrator,Lekarz,Pacjent")]
    public class RejestracjaController : BaseController
    {
        private Osoba os = new Osoba();
        private SendEmail se = new SendEmail();

        // GET: Rejestracja
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "TypPracownika,Email,Haslo,PowtorzHaslo,Imie,Nazwisko,NrTelefonu")] Osoba o)
        {
            if (!o.Haslo.Equals(o.PowtorzHaslo))
            {
                ModelState.AddModelError("Haslo", "Podane hasła są różne");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    var roleUser = new RoleManager<IdentityRole>(
                                 new RoleStore<IdentityRole>(
                                 new ApplicationDbContext()));

                    var createUser = new UserManager<ApplicationUser>(
                                      new UserStore<ApplicationUser>(
                                      new ApplicationDbContext()));

                    roleUser.Create(new IdentityRole(o.TypPracownika.ToString()));


                    var user = new ApplicationUser { UserName = o.Email };
                    user.EmailConfirmed = true;
                    var pass = o.Haslo;

                    createUser.Create(user, pass);

                    createUser.AddToRole(user.Id, o.TypPracownika.ToString());

                    TempData["ok"] = o.Imie + " typ konta " + o.TypPracownika;

                    switch (o.TypPracownika)
                    {

                        case TypPracownika.Administrator:
                            {
                                var admin = Mapper.Map<Administrator>(o);
                                se.SendMail(o.Email, "Witaj, administrator " +
                                  o.Imie, "Twoje dane do logowania to \n"
                                   + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                                db.Administrator.Add(admin);
                                db.SaveChanges();
                                return RedirectToAction("Index", "Rejestracja");
                            }
                        case TypPracownika.Lekarz:
                            {
                                var lekarz = Mapper.Map<Lekarz>(o);
                                lekarz.DataDodania = DateTime.Now;
                                se.SendMail(o.Email, "Witaj, lekarz " +
                                    o.Imie, "Twoje dane do logowania to \n"
                                    + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                                db.Lekarz.Add(lekarz);
                                db.SaveChanges();
                                return RedirectToAction("Index", "Rejestracja");
                            }
                        case TypPracownika.Pacjent:
                            {
                                var pacjent = Mapper.Map<Pacjent>(o);
                                pacjent.DataRejestracji = DateTime.Now;
                                se.SendMail(o.Email, "Witaj, pacjencie " +
                                   o.Imie, "Twoje dane do logowania to \n "
                                   + "Login:  " + o.Email + "\n" + "hasło " + o.Haslo);
                                db.Pacjent.Add(pacjent);
                                db.SaveChanges();
                                return RedirectToAction("Index", "Rejestracja");
                            }
                    }
                }

                catch (InvalidOperationException)
                {
                    ModelState.AddModelError("Email", "Podany email jest już zajęty");
                }
            }
            return View(o);
        }
    }
}