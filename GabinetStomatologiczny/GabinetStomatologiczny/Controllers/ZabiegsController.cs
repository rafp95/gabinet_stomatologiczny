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

namespace GabinetStomatologiczny.Controllers
{
    public class ZabiegsController : BaseController
    {

        // GET: Zabiegs
        public ActionResult Index()
        {
            var zabieg = db.Zabieg.Include(z => z.Wizyta);
            return View(zabieg.ToList());
        }

        // GET: Zabiegs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zabieg zabieg = db.Zabieg.Find(id);
            if (zabieg == null)
            {
                return HttpNotFound();
            }
            return View(zabieg);
        }

        // GET: Zabiegs/Create
        public ActionResult Create()
        {
            ViewBag.WizytaID = new SelectList(db.Wizyta.Include(p => p.Pacjent), "ID", "ID");
            return View();
        }

        // POST: Zabiegs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,WizytaID,TypZabiegu,CenaZabiegu,SearchQuery")] Zabieg zabieg)
        {
            if (string.IsNullOrEmpty(zabieg.SearchQuery))
            {
                TempData["error"] = "Pole szukaj pacjęta nie może być puste";
            }
            else if (ModelState.IsValid)
            {
                var pacjet = db.Pacjent.ToList().Where(p => (p.Imie + " " + p.Nazwisko).ToLower().
                Contains(zabieg.SearchQuery.ToLower())).Select(p => p.ID).FirstOrDefault();

                var wizyta = db.Wizyta.ToList().
                    Where(p => p.PacjentID == pacjet).Select(p => p.ID).FirstOrDefault();

                zabieg.WizytaID = wizyta;
                db.Zabieg.Add(zabieg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WizytaID = new SelectList(db.Wizyta, "ID", "ID", zabieg.WizytaID);
            return View(zabieg);
        }

        // GET: Zabiegs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Zabieg zabieg = db.Zabieg.Find(id);
            
            if (zabieg == null)
            {
                return HttpNotFound();
            }
            ViewBag.WizytaID = new SelectList(db.Wizyta, "ID", "ID", zabieg.WizytaID);
            return View(zabieg);
        }

        // POST: Zabiegs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WizytaID,TypZabiegu,CenaZabiegu")] Zabieg zabieg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zabieg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WizytaID = new SelectList(db.Wizyta, "ID", "ID", zabieg.WizytaID);
            return View(zabieg);
        }

        // GET: Zabiegs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zabieg zabieg = db.Zabieg.Find(id);
            if (zabieg == null)
            {
                return HttpNotFound();
            }
            return View(zabieg);
        }

        // POST: Zabiegs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Zabieg zabieg = db.Zabieg.Find(id);
            db.Zabieg.Remove(zabieg);
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
