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

namespace GabinetStomatologiczny.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPacjentsService pacjentsService = null;

        public HomeController() : base()
        {
            this.pacjentsService = new PacjentsService(this.db);
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var about = Mapper.Map<IEnumerable<LiczbaPacjentow>>(this.pacjentsService.Statistic().
                Select(m => new LiczbaPacjentow { DataRejestracji = m.Key, Pacjentow = m.Count() }).ToList());
            return View(about);
        }

    }
}