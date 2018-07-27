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
        public ActionResult Index(LekarzSort lekarzSort)
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
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lekarz lekarz = this._lekarzservice.Find(id);
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lekarz lekarz = this._lekarzservice.Find(id);

            if (lekarz == null)
            {
                return HttpNotFound();
            }else if ((User.Identity.IsAuthenticated && User.Identity.Name == lekarz.Email) || User.IsInRole("Administrator"))
            {
                return View(lekarz);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Lekarzs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Imie,Nazwisko,Zdjecie,Email,NrTelefonu")] Lekarz lekarz)
        {
            
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["Zdjecie"];
                UpdateModel(lekarz);
                if (file != null && file.ContentLength > 0)
                {
                    lekarz.Zdjecie = System.Guid.NewGuid().ToString();
                    lekarz.Zdjecie = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + lekarz.Zdjecie);
                }

                this._lekarzservice.Edit(lekarz);
                return RedirectToAction("Index");
            }
            return View(lekarz);
        }

        // GET: Lekarzs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lekarz lekarz = this._lekarzservice.Find(id);
            if (lekarz == null)
            {
                return HttpNotFound();
            }
            else if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(lekarz);
        }

        // POST: Lekarzs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lekarz lekarz = db.Lekarz.Find(id);
            if(lekarz.Zdjecie != null)
            {
                string path = Request.MapPath("~/Obrazki/" + lekarz.Zdjecie);
                System.IO.File.Delete(path);
            }
            this._lekarzservice.Remove(lekarz);
            return RedirectToAction("Index");
        }

    }
}
