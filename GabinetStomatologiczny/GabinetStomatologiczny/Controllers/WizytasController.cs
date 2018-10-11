using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Utils;
using System.Diagnostics;
using AutoMapper;
using static GabinetStomatologiczny.Startup;
using System.Threading.Tasks;
using Nexmo.Api;
using System.Security.Claims;
using GabinetStomatologiczny.Services;

namespace GabinetStomatologiczny.Controllers
{
    public class WizytasController : BaseController
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IWizytService _wizytService;
        private readonly ISugestionService _sugestionService;
        public WizytasController(GabinetContext db,
            IUserRoleService userRoleService, 
            IWizytService wizytService,
            ISugestionService sugestionService) : base(db)
        {
            this._userRoleService = userRoleService;
            this._wizytService = wizytService;
            this._sugestionService = sugestionService;
        }

        // GET: Wizytas
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetEvents(string searchQuery)
        {

            var isLogedin = User.Identity.IsAuthenticated;
            var r = this._userRoleService.GetUserRole((ClaimsIdentity)User.Identity);
            var u =  User.Identity.Name;

            var events = await this._wizytService.GetEvents(searchQuery,isLogedin,r,u);
            
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Sugestions(Sugestion sugestion)
        {
            var events = _sugestionService.SugestionWizyt(sugestion);

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Sms(string to, string text)
        {
            var status = false;
            var results = SMS.Send(new SMS.SMSRequest
            {
                from = Configuration.Instance.Settings["appsettings:NEXMO_FROM_NUMBER"],
                to = to,
                text = text
              
            });
            status = true;
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> SaveEvent(Wizyta w)
        {
            var status = await this._wizytService.SaveEvent(w, User.Identity.Name);
            return new JsonResult { Data = new { status } };
        }

        [HttpPost]
        public async Task<JsonResult> DeleteEvent(Wizyta w)
        {
            var status = await this._wizytService.DeleteEvent(w);
            return new JsonResult { Data = new { status = status } };
        }


        // GET: Wizytas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wizyta wizyta = db.Wizyta.Find(id);
            if (wizyta == null)
            {
                return HttpNotFound();
            }
            return View(wizyta);
        }

        // GET: Wizytas/Create
        public ActionResult Create()
        {
            ViewBag.LekarzID = new SelectList(db.Lekarz, "ID", "Imie");
            ViewBag.PacjentID = new SelectList(db.Pacjent, "ID", "Imie");
            return View();
        }

        // POST: Wizytas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LekarzID,PacjentID,Data")] Wizyta wizyta)
        {
            if (ModelState.IsValid)
            {
                db.Wizyta.Add(wizyta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LekarzID = new SelectList(db.Lekarz, "ID", "Imie", wizyta.LekarzID);
            ViewBag.PacjentID = new SelectList(db.Pacjent, "ID", "Imie", wizyta.PacjentID);
            return View(wizyta);
        }

        // GET: Wizytas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wizyta wizyta = db.Wizyta.Find(id);
            if (wizyta == null)
            {
                return HttpNotFound();
            }
            ViewBag.LekarzID = new SelectList(db.Lekarz, "ID", "Imie", wizyta.LekarzID);
            ViewBag.PacjentID = new SelectList(db.Pacjent, "ID", "Imie", wizyta.PacjentID);
            return View(wizyta);
        }

        // POST: Wizytas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LekarzID,PacjentID,Data")] Wizyta wizyta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wizyta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LekarzID = new SelectList(db.Lekarz, "ID", "Imie", wizyta.LekarzID);
            ViewBag.PacjentID = new SelectList(db.Pacjent, "ID", "Imie", wizyta.PacjentID);
            return View(wizyta);
        }

        // GET: Wizytas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wizyta wizyta = db.Wizyta.Find(id);
            if (wizyta == null)
            {
                return HttpNotFound();
            }
            return View(wizyta);
        }

        // POST: Wizytas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wizyta wizyta = db.Wizyta.Find(id);
            db.Wizyta.Remove(wizyta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
