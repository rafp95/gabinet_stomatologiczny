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
    [Authorize(Roles = "Administrator,Lekarz")]
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
                    var roleAdministrator = new RoleManager<IdentityRole>(
                                 new RoleStore<IdentityRole>(
                                 new ApplicationDbContext()));

                    var userAdministrator = new UserManager<ApplicationUser>(
                                      new UserStore<ApplicationUser>(
                                      new ApplicationDbContext()));

                    roleAdministrator.Create(new IdentityRole(o.TypPracownika.ToString()));


                    var user = new ApplicationUser { UserName = o.Email };
                    user.EmailConfirmed = true;
                    var pass = o.Haslo;

                    userAdministrator.Create(user, pass);

                    userAdministrator.AddToRole(user.Id, o.TypPracownika.ToString());

                    TempData["ok"] = o.Imie + " typ konta " + o.TypPracownika;

                    switch (o.TypPracownika)
                    {

                        case TypPracownika.Administrator:
                            {
                                Administrator admin = new Administrator()
                                {
                                    Email = o.Email,
                                    Imie = o.Imie,
                                    Nazwisko = o.Nazwisko,
                                    NrTelefonu = o.NrTelefonu

                                };
                                se.SendMail(o.Email, "Witaj, administrator " +
                                  o.Imie, "Twoje dane do logowania to \n"
                                   + "Login:  " + o.Email + "\n" + "hasło " + pass);
                                db.Administrator.Add(admin);
                                db.SaveChanges();
                                return RedirectToAction("Index", "Rejestracja");
                            }
                        case TypPracownika.Lekarz:
                            {
                                Lekarz lekarz = new Lekarz()
                                {
                                    Imie = o.Imie,
                                    NrTelefonu = o.NrTelefonu,
                                    DataDodania = DateTime.Now,
                                    Nazwisko = o.Nazwisko,
                                    Email = o.Email

                                };
                                se.SendMail(o.Email, "Witaj, lekarz " +
                                    o.Imie, "Twoje dane do logowania to \n"
                                    + "Login:  " + o.Email + "\n" + "hasło " + pass);
                                db.Lekarz.Add(lekarz);
                                db.SaveChanges();
                                return RedirectToAction("Index", "Rejestracja");
                            }
                        case TypPracownika.Pacjent:
                            {
                                Pacjent pacjent = new Pacjent()
                                {
                                    Imie = o.Imie,
                                    Nazwisko = o.Nazwisko,
                                    NrTelefonu = o.NrTelefonu,
                                    Email = o.Email,
                                    DataRejestracji = DateTime.Now,
                                };
                                se.SendMail(o.Email, "Witaj, pacjencie " +
                                   o.Imie, "Twoje dane do logowania to \n "
                                   + "Login:  " + o.Email + "\n" + "hasło " + pass);
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