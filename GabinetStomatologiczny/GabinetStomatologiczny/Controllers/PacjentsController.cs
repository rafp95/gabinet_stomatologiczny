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
using System.Drawing.Printing;

namespace GabinetStomatologiczny.Controllers
{
    [Authorize]
    public class PacjentsController : BaseController
    {

        private readonly IPacjentsService pacjentsService = null;
        private readonly ISortPacjentService sortPacjentService = null;
        private readonly ISugestionService sugestionService = null;

        public PacjentsController(IPacjentsService pacjentsService, 
            ISortPacjentService sortPacjentService, 
            ISugestionService sugestionService,
            GabinetContext db):base(db)
        {
            this.pacjentsService = pacjentsService;
            this.sortPacjentService = sortPacjentService;
            this.sugestionService = sugestionService;
        }

        // GET: Pacjents
        public ActionResult Index(PaginationType pacjentSort)
        {
            ViewBag.CurrentSort = pacjentSort.sortOrder;
            if (string.IsNullOrEmpty(pacjentSort.searchString))
            {
                pacjentSort.searchString = pacjentSort.currentFilter;
            }

            ViewBag.CurrentFilter = pacjentSort.searchString;
            var pacjent = this.sortPacjentService.SortAndPaginate(pacjentSort);
            ViewBag.ActiveClass = this.sortPacjentService.RodzajSort;


            if (Request.IsAjaxRequest())
            {
                return PartialView("_PatiensParts", pacjent);

            }
            return View(pacjent);
        }

        public ActionResult Sugestions(Sugestion sugestion)
        {
            var list = this.sugestionService.SugestionPacjent(sugestion);

            
            return Json(list, JsonRequestBehavior.AllowGet);
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
                TempData["editPacjent"] = "done";
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
