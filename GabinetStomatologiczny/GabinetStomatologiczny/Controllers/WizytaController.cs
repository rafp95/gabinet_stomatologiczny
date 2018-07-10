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

namespace GabinetStomatologiczny.Controllers
{
    public class WizytaController : BaseController
    {

        // GET: Wizytas
        public ActionResult Index()
        {
            ViewBag.User = User.Identity.Name;
            var wizyta = db.Wizyta.Include(w => w.Lekarz).Include(w => w.Pacjent);
            return View(wizyta.ToList());
        }

        public JsonResult GetEvents(string searchQuery)
        {
            var events = db.Wizyta.ToList();
            if (!User.Identity.IsAuthenticated)
            {
                foreach (var item in events)
                {
                    item.Tytul = "*";
                }
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                events.Where(p => p.Tytul.Contains(searchQuery)).ToList();
            }
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult Sugestions(string term)
        {
            var events = db.Wizyta.ToList().
                Where(p => p.Tytul.ToLower().Contains(term.ToLower()))
                .Take(5).Select(p => new { label = p.Tytul + " " + p.Poczatek.ToString("dd.MMM.yyyy, H:mm") });

            if (events.Count() == 0)
            {
                events = db.Wizyta.DefaultIfEmpty().Select(p => new { label = "Nie jesteś zapisany" }).Take(1);
            }

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEvent(Wizyta w)
        {
            var status = false;

            if(w.ID > 0)
            {
                var v = db.Wizyta.Where(a => a.ID == w.ID).FirstOrDefault();
                if(v != null)
                {
                    w.LekarzID = v.LekarzID;
                    w.PacjentID = v.PacjentID;
                    var m = Mapper.Map<Wizyta, Wizyta>(w,v);
                    db.Entry(m).State = EntityState.Modified;
                }

            }else
            {
                w.PacjentID = db.Pacjent.Where(p => p.Email == User.Identity.Name).Select(p => p.ID).FirstOrDefault();
                w.LekarzID = db.Lekarz.Select(p => p.ID).FirstOrDefault();
                var m = Mapper.Map<Wizyta, Wizyta>(w);
                db.Wizyta.Add(m);
                
            }

            db.Database.Log = message => Trace.WriteLine(message);
            db.SaveChanges();
            status = true;
            return new JsonResult { Data = new { status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(Wizyta w)
        {
            var status = false;

            var v = db.Wizyta.Where(a => a.ID == w.ID).FirstOrDefault();
            if(v != null)
            {
                db.Wizyta.Remove(v);
                db.SaveChanges();
                status = true;
            }

            return new JsonResult { Data = new {status = status} };
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
