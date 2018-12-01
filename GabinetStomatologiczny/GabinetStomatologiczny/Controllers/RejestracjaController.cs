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
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GabinetStomatologiczny.Controllers
{
    [Authorize(Roles = "Administrator,Lekarz")]
    public class RejestracjaController : BaseController
    {
        private readonly IRejestracjaService _rs;

        public RejestracjaController(IRejestracjaService rs)
        {
            this._rs = rs;
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
                if (await this._rs.UserExist(o))
                {
                    ModelState.AddModelError("", "Osoba o podanym imieniu i nazwisku istnieje już w bazie");
                }
                else if (!o.Haslo.Equals(o.PowtorzHaslo))
                {
                    ModelState.AddModelError("Haslo", "Podane hasła są różne");
                }
                else if (ModelState.IsValid)
                {

                    await this._rs.CreateUserInIdentity(o);
                    TempData["ok"] = o.Imie + " typ konta " + o.RoleType;
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