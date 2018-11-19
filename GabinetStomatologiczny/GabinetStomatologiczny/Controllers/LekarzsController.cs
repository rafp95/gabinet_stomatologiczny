using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System.IO;
using PagedList;
using GabinetStomatologiczny.Utils;
using GabinetStomatologiczny.Services;
using System.Threading.Tasks;
using GabinetStomatologiczny.Models;

namespace GabinetStomatologiczny.Controllers
{
    [Authorize]
    public class LekarzsController : BaseController
    {

        private readonly ILekarzService _lekarzservice;
        private readonly ISortLekarzService _lekarzSortService;
        private readonly ISugestionService _sugestionService;
        public LekarzsController(ILekarzService lekarzservice, ISortLekarzService lekarzSortService,ISugestionService sugestionService)
        {
            this._lekarzservice = lekarzservice;
            this._lekarzSortService = lekarzSortService;
            this._sugestionService = sugestionService;
        }

        // GET: Lekarzs
        public ActionResult Index(PaginationType lekarzSort)
        {
            ViewBag.CurrentSort = lekarzSort.sortOrder;
            if (string.IsNullOrEmpty(lekarzSort.searchString))
            {
                lekarzSort.searchString = lekarzSort.currentFilter;
            }

            ViewBag.CurrentFilter = lekarzSort.searchString;
            var lekarz = this._lekarzSortService.SortAndPaginate(lekarzSort);
            ViewBag.ActiveClass = this._lekarzSortService.RodzajSort;


            if (Request.IsAjaxRequest())
            {
                return PartialView("_LekarzsParts", lekarz);
            }
            return View(lekarz);
        }

        public ActionResult SugestionsLekarzs(Sugestion sugestion)
        {
            var list = this._sugestionService.SugestionLekarz(sugestion);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        // GET: Lekarzs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lekarz lekarz = await this._lekarzservice.Find(id);
            if (lekarz == null)
            {
                return HttpNotFound();
            }
            return View(lekarz);
        }

       
        // GET: Lekarzs/Create
        public ActionResult Create()
        {
           return View();
        }

        // POST: Lekarzs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Imie,Nazwisko,Zdjecie,Email,NrTelefonu")] Lekarz lekarz)
        {
            if (ModelState.IsValid)
            {
                this._lekarzservice.CreateLekarz(lekarz);
                return RedirectToAction("Index");
            }

            return View(lekarz);
        }

        // GET: Lekarzs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lekarz lekarz = await this._lekarzservice.Find(id);

            if (lekarz == null)
            {
                return HttpNotFound();
            }
            return View(lekarz);
        }

        // POST: Lekarzs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Imie,Nazwisko,Zdjecie,Email,NrTelefonu")] Lekarz lekarz)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase file = Request.Files["Zdjecie"];
                    UpdateModel(lekarz);
                    HttpPostedFileBase f = this._lekarzservice.SaveFile(file, lekarz);

                    if (f.ContentLength > 0)
                    {
                        f.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + lekarz.Zdjecie);
                    }
                    
                    await Task.Run(() => this._lekarzservice.Edit(lekarz));
                    TempData["editLekarz"] = "done";
                    return RedirectToAction("Index");
                }catch(Exception)
                {
                    ViewBag.Error = "Sprawdź czy poprawnie wypełniłeś wszystkie wypełniłeś dane";
                }
            }
            return View(lekarz);
        }

        // GET: Lekarzs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lekarz lekarz = await this._lekarzservice.Find(id);
            if (lekarz == null)
            {
                return HttpNotFound();
            }
            return View(lekarz);
        }

        // POST: Lekarzs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lekarz lekarz = await this._lekarzservice.Find(id);
            if(lekarz.Zdjecie != null)
            {
                string path = Request.MapPath("~/Obrazki/" + lekarz.Zdjecie);
                System.IO.File.Delete(path);
            }
            await Task.Run(() => this._lekarzservice.Remove(lekarz));
            return RedirectToAction("Index");
        }

    }
}
