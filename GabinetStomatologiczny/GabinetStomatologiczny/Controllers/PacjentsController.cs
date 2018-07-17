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
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Controllers
{
    [Authorize]
    public class PacjentsController : BaseController
    {

        private readonly IPacjentsService pacjentsService = null;

        public PacjentsController(IPacjentsService pacjentsService,GabinetContext db):base(db)
        {
            this.pacjentsService = pacjentsService;
        }

        // GET: Pacjents
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewBag.CurrentFilter = searchString;
            var pacjent = this.pacjentsService.getAllPacjents();

            if (!string.IsNullOrEmpty(searchString))
            {
                pacjent = pacjent.Where(s => (s.Imie + " " + s.Nazwisko + " " + s.NrTelefonu + " " + s.Email + " " + s.Plec)
                .ToLower().Contains(searchString.ToLower()));

            }
            
            switch (sortOrder)
            {
                case "name_desc":
                    {
                        pacjent =  pacjent.OrderByDescending(p => p.Imie);
                        ViewBag.ClassIsActive = "active";
                        break;
                    }
                case "name_asc":
                    {
                        pacjent =  pacjent.OrderBy(p => p.Imie);
                        break;

                    }
                case "surname_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.Nazwisko);
                        break;
                    }
                case "surname_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.Nazwisko);
                        break;

                    }
                case "pesel_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.Pesel);
                        break;
                    }
                case "pesel_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.Pesel);
                        break;
                    }
                case "plec_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.Plec);
                        break;
                    }
                case "plec_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.Plec);
                        break;
                    }
                case "dataurodzenia_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.DataUrodzenia);
                        break;
                    }
                case "dataurodzenia_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.DataUrodzenia);
                        break;
                    }
                case "nrtel_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.NrTelefonu);
                        break;
                    }
                case "nrtel_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.NrTelefonu);
                        break;
                    }
                case "datarejestracji_desc":
                    {
                        pacjent = pacjent.OrderByDescending(p => p.DataRejestracji);
                        break;
                    }
                case "datarejestracji_asc":
                    {
                        pacjent = pacjent.OrderBy(p => p.DataRejestracji);
                        break;
                    }
                default:
                    {
                        pacjent = pacjent.OrderBy(p => p.Imie);
                        break;
                    }
            }

            

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PatiensParts", pacjent.ToPagedList(pageNumber, pageSize));
            }

            return View(pacjent.ToPagedList(pageNumber, pageSize));
        }

        public async Task <ActionResult> Sugestions(string term)
        {
            var personList = await this.db.Pacjent
                .Where(p => (p.Imie + " " + p.Nazwisko).ToLower().Contains(term.ToLower())).
                Take(5).Select(p => new { label = p.Imie + " " + p.Nazwisko }).ToListAsync();

            return Json(personList, JsonRequestBehavior.AllowGet);
        }

        // GET: Pacjents/Details/5
        public async Task <ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacjent pacjent = await this.pacjentsService.Find(id);
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
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacjent pacjent = await this.pacjentsService.Find(id);
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
        public async Task <ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacjent pacjent = await this.pacjentsService.Find(id);
            if (pacjent == null)
            {
                return HttpNotFound();
            }
            return View(pacjent);
        }

        // POST: Pacjents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> DeleteConfirmed(int id)
        {
            Pacjent pacjent = await this.pacjentsService.Find(id);
            this.pacjentsService.Remove(pacjent);
            return RedirectToAction("Index");
        }

    }
}
