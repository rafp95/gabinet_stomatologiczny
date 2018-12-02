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
using Rotativa;
using GabinetStomatologiczny.Services;
using System.Threading.Tasks;
using System.Security.Claims;

namespace GabinetStomatologiczny.Controllers
{
    [Authorize]
    public class ZabiegsController : BaseController
    {

        private IGenerateZabiegsPdf _generatePdf;
        private ISortZabiegsService _sortZabiegService;
        private ISugestionService _sugestionService;
        private IZabiegService _zabiegService;
        private IPacjentsService _pacejntService;

        public ZabiegsController(GabinetContext db,
            IGenerateZabiegsPdf generatePdf,
            ISortZabiegsService sortZabiegService,
            ISugestionService sugestionService,
            IZabiegService zabiegService,
            IPacjentsService pacejntService) : base(db)
        {
            _generatePdf = generatePdf;
            _sortZabiegService = sortZabiegService;
            _sugestionService = sugestionService;
            _zabiegService = zabiegService;
            _pacejntService = pacejntService;
        }

        // GET: Zabiegs
        public ActionResult Index(PaginationType zabiegSort)
        {

            ViewBag.CurrentSort = zabiegSort.sortOrder;
            var zabieg = this._sortZabiegService.SortAndPaginate(zabiegSort, (ClaimsIdentity)User.Identity);
            ViewBag.CurrentFilter = zabiegSort.searchString;
            ViewBag.ActiveClass = this._sortZabiegService.RodzajSort;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ZabiegsPartial", zabieg);

            }
            return View(zabieg);
        }

        public ActionResult Sugestions(Sugestion sugestion)
        {
            var list = _sugestionService.SugestionZabeg(sugestion);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> PdfGenerate(string user)
        {
            return View(await _generatePdf.GetPacjents(user));
        }

        [AllowAnonymous]
        public ActionResult RaportPdf(string user)
        {
            var p = new ActionAsPdf("PdfGenerate", new { user = User.Identity.Name });
            return p;
        }

        // GET: Zabiegs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zabieg zabieg = await _zabiegService.FindZabieg(id);// db.Zabieg.Include(w => w.Wizyta).First(_ => _.ID == id);
            if (zabieg == null)
            {
                return HttpNotFound();
            }
            return View(zabieg);
        }

        // GET: Zabiegs/Create
        public async Task<ActionResult> Create(PaginationType search)
        {
            var user = await _pacejntService.Find(search.ID);
            ViewBag.name = await db.Pacjent.Where(_ => _.ID == search.ID).Select(_ => _.Imie + " " + _.Nazwisko).FirstOrDefaultAsync();
            Session["user"] = user.ID;
            ViewBag.WizytaID = new SelectList(db.Wizyta.Include(p => p.Pacjent), "ID", "ID");
            return View();
        }

        // POST: Zabiegs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,WizytaID,TypZabiegu,CenaZabiegu,SearchQuery")] Zabieg zabieg)
        {
            try
            {
                if (string.IsNullOrEmpty(zabieg.SearchQuery))
                {
                    TempData["error"] = "Pole szukaj pacjęta nie może być puste";
                }
                else if (ModelState.IsValid)
                {
                    await _zabiegService.ZabiegCreate(zabieg);
                    return RedirectToAction("ToothModel");
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Osoba nie była zapisana na wizytę";
            }

            ViewBag.WizytaID = new SelectList(db.Wizyta, "ID", "ID", zabieg.WizytaID);
            return View(zabieg);
        }

        public async Task<ActionResult> ToothModel(PaginationType pt)
        {
            int id = (int)Session["user"];
            Pacjent p = await _pacejntService.GetPacjentById(id);
            ViewBag.User = p.Imie + " " + p.Nazwisko;
            var pacejnt = await _zabiegService.PacejntZabiegHistory(pt, id);
            return View(pacejnt);
        }


        public async Task<ActionResult> GetAllTooth()
        {
            int id = (int)Session["user"];
            //var idWizyta = db.Wizyta.First(_ => _.Pacjent.ID == id).ID;
            //var tooth = db.Zabieg.Where(_ => _.WizytaID == idWizyta && _.Wizyta.PacjentID == id).Select(_ => _.Zeby);
            var tooth = await _zabiegService.GetAllTooth(id);
            return new JsonResult { Data = tooth, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTooth(Zab zab, ZabDescriptions zdesc)
        {

            bool status = false;
            int id = (int)Session["user"];
            if (!string.IsNullOrEmpty(zdesc.Description))
            {
                await _zabiegService.ZabDescription(zab, zdesc, id);
                status = true;
            }
            else if (await db.Zab.AnyAsync(_ => _.IdTooth == zab.IdTooth && _.Zabieg.Wizyta.PacjentID == id) == false)
            {
                await _zabiegService.SaveTooth(zab, id);
                status = true;
            }

            return new JsonResult { Data = new { status }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<ActionResult> GetHistory(Zab zab)
        {
            int idU = (int)Session["user"];
            var data = await this._zabiegService.GetHistoryToothDescr(zab.IdTooth, idU);

            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<ActionResult> DeleteTooth(Zab zab)
        {

            bool status = false;
            int id = (int)Session["user"];
            if (await db.Zab.AnyAsync(_ => _.IdTooth == zab.IdTooth && _.Zabieg.Wizyta.PacjentID == id))
            {
                await _zabiegService.DeleteTooth(zab, id);
                status = true;
            }
            return new JsonResult { Data = new { status }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<ActionResult> DeleteToothDescr(int id)
        {
            var status = await _zabiegService.DeleteZabDescr(id);
            return new JsonResult { Data = new {status}, JsonRequestBehavior = JsonRequestBehavior.AllowGet  };
        }


        // GET: Zabiegs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Zabieg zabieg = await _zabiegService.FindZabieg(id);
            Session["user"] = zabieg.Wizyta.PacjentID;
            ViewBag.UserName = zabieg.Wizyta.Tytul;
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,WizytaID,TypZabiegu,CenaZabiegu,ZabiegDate")] Zabieg zabieg)
        {
            if (ModelState.IsValid)
            {
                await _zabiegService.EditZabeg(zabieg);
                var z = await _zabiegService.FindZabieg(zabieg.ID);
                TempData["zabiegOperationSucess"] = "Pomyślnie edytowano " + z.Wizyta.Tytul;
                return RedirectToAction("Index");
            }
            ViewBag.WizytaID = new SelectList(db.Wizyta, "ID", "ID", zabieg.WizytaID);
            return View(zabieg);
        }

        // GET: Zabiegs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zabieg zabieg = await _zabiegService.FindZabieg(id); //db.Zabieg.Include(_ => _.Wizyta).First(_ => _.ID == id);
            if (zabieg == null)
            {
                return HttpNotFound();
            }
            return View(zabieg);
        }

        // POST: Zabiegs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var z = await _zabiegService.FindZabieg(id);
            TempData["zabiegOperationSucess"] = "Pomyślnie skasowano " + z.Wizyta.Tytul;
            await _zabiegService.DeleteZabieg(id);
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
