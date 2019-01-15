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
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPacjentsService pacjentsService = null;
        private readonly INotyficationService ns = null;
        public HomeController(IPacjentsService pacjentsService, INotyficationService ns)
        {
            this.pacjentsService = pacjentsService;
            this.ns = ns;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public  async Task<JsonResult> GetNotyficationWizytas()
        {
            var notyficationTime = Session["LastUpdated"] != null ? 
                (DateTime)Session["LastUpdated"] : DateTime.Now ;
           
            var list = await ns.GetWizyts(notyficationTime);
            //update session for get only wizytas
            Session["LastUpdated"] = DateTime.Now;
            return new JsonResult { Data = list,JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult About()
        {
            return View();
        }

        public async Task<ActionResult> Chart()
        {
            var about = await pacjentsService.Statistic();
            var statistic = about.Select(m => new LiczbaPacjentow { DataRejestracji = m.Key, Pacjentow = m.Count() }).
               OrderByDescending(p => p.DataRejestracji);
            
            return Json(statistic, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Statistic()
        {
            var about = await pacjentsService.Statistic();
            var statistic = about.Select(m => new LiczbaPacjentow { DataRejestracji = m.Key, Pacjentow = m.Count() }).
                OrderByDescending(p => p.DataRejestracji).ToList();
            return View(statistic);
        }

        public ActionResult Pdf()
        {
            var p = new ActionAsPdf("Statistic");
           
            return p;
        }
        
    }
}