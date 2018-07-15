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

namespace GabinetStomatologiczny.Controllers
{
    [Authorize]
    public class LekarzsController : BaseController
    {

        private readonly ILekarzService _lekarzservice;
        public LekarzsController(ILekarzService lekarzservice)
        {
            this._lekarzservice = lekarzservice;
        }

        // GET: Lekarzs
        public ActionResult Index(string sortOrder,string currentFilter,string searchString,int? page)
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

            var lekarz = this._lekarzservice.getAllLekarz();
            if (!string.IsNullOrEmpty(searchString))
            {
                lekarz = lekarz.Where(l => (l.Imie + " "  + l.Nazwisko).ToLower().Contains(searchString.ToLower())).ToList();
            }


            switch (sortOrder)
            {
                case "name_desc":
                    {
                        lekarz = lekarz.OrderByDescending(p => p.Imie).ToList();
                        break;
                    }
                case "name_asc":
                    {
                        lekarz = lekarz.OrderBy(p => p.Imie).ToList();
                        break;

                    }
                case "surname_desc":
                    {
                        lekarz = lekarz.OrderByDescending(p => p.Nazwisko).ToList();
                        break;
                    }
                case "surname_asc":
                    {
                        lekarz = lekarz.OrderBy(p => p.Nazwisko).ToList();
                        break;

                    }
                case "nrtel_desc":
                    {
                        lekarz = lekarz.OrderByDescending(p => p.NrTelefonu).ToList();
                        break;
                    }
                case "nrtel_asc":
                    {
                        lekarz = lekarz.OrderBy(p => p.NrTelefonu).ToList();
                        break;
                        
                    }
                case "email_desc":
                    {
                        lekarz = lekarz.OrderByDescending(p => p.NrTelefonu).ToList();
                        break;
                    }
                case "email_asc":
                    {
                        lekarz = lekarz.OrderBy(p => p.NrTelefonu).ToList();
                        break;
                    }


                default:
                    {
                        lekarz = lekarz.OrderBy(p => p.Imie).ToList();
                        break;
                    }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

           if (Request.IsAjaxRequest())
            {
                return PartialView("_LekarzsParts",lekarz.ToPagedList(pageNumber,pageSize));
            }

            return View(lekarz.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult SugestionsLekarzs(string term)
        {
            var personList = this._lekarzservice.getAllLekarz().ToList().Where(p => (p.Imie + " " + p.Nazwisko).ToLower().Contains(term.ToLower())).Take(5).Select(p => new { label = p.Imie + " " + p.Nazwisko });

            return Json(personList, JsonRequestBehavior.AllowGet);
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
            if(User.Identity.IsAuthenticated || User.IsInRole("Administrator") || User.IsInRole("Lekarz"))
            {
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
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
