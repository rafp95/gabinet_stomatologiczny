using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny.Utils;
using AutoMapper;

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
            return View(Mapper.Map<IEnumerable<LiczbaPacjentow>>(db.Pacjent.ToList().GroupBy(m => m.DataRejestracji.Date).
                Select(m => new LiczbaPacjentow { DataRejestracji = m.Key, Pacjentow = m.Count() }).ToList()));
        }

    }
}