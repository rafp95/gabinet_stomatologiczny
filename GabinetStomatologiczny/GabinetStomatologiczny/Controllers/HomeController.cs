using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny.Utils;
using AutoMapper;
using GabinetStomatologiczny.Services;
using Rotativa;
using Nexmo.Api;

namespace GabinetStomatologiczny.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPacjentsService pacjentsService = null;
        public HomeController(IPacjentsService pacjentsService)
        {
            this.pacjentsService = pacjentsService;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Chart()
        {
            var about = pacjentsService.Statistic().
               Select(m => new LiczbaPacjentow { DataRejestracji = m.Key, Pacjentow = m.Count() }).
               OrderByDescending(p => p.DataRejestracji).ToList();
            
            return Json(about, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Statistic()
        {
            var about = pacjentsService.Statistic().
                Select(m => new LiczbaPacjentow { DataRejestracji = m.Key, Pacjentow = m.Count() }).
                OrderByDescending(p => p.DataRejestracji).ToList();
            return View(about);
        }

        public ActionResult Pdf()
        {
            var p = new ActionAsPdf("Statistic");
           
            return p;
        }
   
    }
}