using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny.Utils;

namespace GabinetStomatologiczny.Controllers
{
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var pacjenci = db.Pacjent.ToList().GroupBy(m => m.DataRejestracji.Date).
                Select(m => new LiczbaPacjentow { DataRejestracji = m.Key, Pacjentow = m.Count() });
            return View(pacjenci.ToList());
        }

    }
}