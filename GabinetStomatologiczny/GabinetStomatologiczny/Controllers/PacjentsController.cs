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
using PagedList;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.Utils;
using GabinetStomatologiczny.Services;

namespace GabinetStomatologiczny.Controllers
{
    [Authorize]
    public class PacjentsController : BaseController
    {

        private readonly IPacjentsService pacjentsService = null;

        public PacjentsController(IPacjentsService pacjentsService)
        {
            this.pacjentsService = pacjentsService;
        }

        // GET: Pacjents
        public ActionResult Index(string sortOrder,string currentFilter,string searchString,int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
            }else
            {
                searchString = currentFilter;
            }
   

            ViewBag.CurrentFilter = searchString;
            var pacjent = this.pacjentsService.getAllPacjents();

            if (!string.IsNullOrEmpty(searchString))
            {
                pacjent = pacjent.Where(s => (s.Imie + " " + s.Nazwisko + " " + s.NrTelefonu + " " + s.Email + " " + s.Plec)
                .ToLower().Contains(searchString.ToLower())).ToList();

            }
            switch (sortOrder)
            {
                case "name_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.Imie).ToList();
                        break;
                    }
                case "name_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.Imie).ToList();
                        break;

                    }
                case "surname_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.Nazwisko).ToList();
                        break;
                    }
                case "surname_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.Nazwisko).ToList();
                        break;

                    }
                case "pesel_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.Pesel).ToList();
                        break;
                    }
                case "pesel_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.Pesel).ToList();
                        break;
                    }
                case "plec_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.Plec).ToList();
                        break;
                    }
                case "plec_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.Plec).ToList();
                        break;
                    }
                case "dataurodzenia_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.DataUrodzenia).ToList();
                        break;
                    }
                case "dataurodzenia_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.DataUrodzenia).ToList();
                        break;
                    }
                case "nrtel_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.NrTelefonu).ToList();
                        break;
                    }
                case "nrtel_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.NrTelefonu).ToList();
                        break;
                    }
                case "datarejestracji_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.DataRejestracji).ToList();
                        break;
                    }
                case "datarejestracji_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.DataRejestracji).ToList();
                        break;
                    }
                default:
                    {
                        pacjent = pacjent.OrderBy(p => p.Imie).ToList();
                        break;
                    }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PatiensParts", pacjent.ToPagedList(pageNumber, pageSize));
            }

            return View(pacjent.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Sugestions(string term)
        {
            var personList = this.pacjentsService.getAllPacjents().ToList().Where(p => (p.Imie + " " + p.Nazwisko).ToLower().Contains(term.ToLower())).Take(5).Select(p => new { label = p.Imie + " " + p.Nazwisko });

            if(personList.Count() == 0)
            {
                personList = this.pacjentsService.getAllPacjents().Select(p => new { label = "Nie znaleziono"}).Take(1);
            }

            return Json(personList, JsonRequestBehavior.AllowGet);
        }

        // GET: Pacjents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacjent pacjent = this.pacjentsService.About(id);
            if (pacjent == null)
            {
                return HttpNotFound();
            }
            return View(pacjent);
        }

        // GET: Pacjents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pacjents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Imie,DrugieImie,Nazwisko,Pesel,Plec,DataUrodzenia,NrTelefonu,Email,Miejscowosc,Kod,Ulica,Wojewodztwo,Kraj,OdzialNfz,DataRejestracji")] Pacjent pacjent)
        {
            if (ModelState.IsValid)
            {
                pacjent.DataRejestracji = DateTime.Now;
                this.pacjentsService.CreatePacjnt(pacjent);
                return RedirectToAction("Index");
            }

            return View(pacjent);
        }

        // GET: Pacjents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacjent pacjent = this.pacjentsService.Find(id);
            if (pacjent == null)
            {
                return HttpNotFound();
            }
            return View(pacjent);
        }

        // POST: Pacjents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Imie,DrugieImie,Nazwisko,Pesel,Plec,DataUrodzenia,NrTelefonu,Email,Miejscowosc,Kod,Ulica,Wojewodztwo,Kraj,OdzialNfz,DataRejestracji")] Pacjent pacjent)
        {
            if (ModelState.IsValid)
            {
                this.pacjentsService.Edit(pacjent);
                return RedirectToAction("Index");
            }
            return View(pacjent);
        }

        // GET: Pacjents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacjent pacjent = this.pacjentsService.Find(id);
            if (pacjent == null)
            {
                return HttpNotFound();
            }
            return View(pacjent);
        }

        // POST: Pacjents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pacjent pacjent = this.pacjentsService.Find(id);
            this.pacjentsService.Remove(pacjent);
            return RedirectToAction("Index");
        }

    }
}
