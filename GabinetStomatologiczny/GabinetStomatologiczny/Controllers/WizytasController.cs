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
using System.Diagnostics;
using AutoMapper;
using static GabinetStomatologiczny.Startup;
using System.Threading.Tasks;
using Nexmo.Api;
using System.Security.Claims;
using GabinetStomatologiczny.Services;

namespace GabinetStomatologiczny.Controllers
{
    public class WizytasController : BaseController
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IWizytService _wizytService;
        private readonly ISugestionService _sugestionService;
        private readonly ISendEmailService _sendEmailService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRoleService"></param>
        /// <param name="wizytService"></param>
        /// <param name="sugestionService"></param>
        public WizytasController(
            IUserRoleService userRoleService,
            IWizytService wizytService,
            ISugestionService sugestionService,
            ISendEmailService sendEmailService)
        {
            this._userRoleService = userRoleService;
            this._wizytService = wizytService;
            this._sugestionService = sugestionService;
            this._sendEmailService = sendEmailService;
        }

        // GET: Wizytas
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetEvents(string searchQuery)
        {

            var isLogedin = User.Identity.IsAuthenticated;
            var r = this._userRoleService.GetUserRole((ClaimsIdentity)User.Identity);
            var u = User.Identity.Name;

            var events = await this._wizytService.GetEvents(searchQuery, isLogedin, r, u);

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sugestion"></param>
        /// <returns></returns>
        public JsonResult Sugestions(Sugestion sugestion)
        {
            var events = _sugestionService.SugestionWizyt(sugestion);

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Sms(string to, string text)
        {
            var status = false;
            try
            {
                var results = SMS.Send(new SMS.SMSRequest
                {
                    from = Configuration.Instance.Settings["appsettings:NEXMO_FROM_NUMBER"],
                    to = to,
                    text = text

                });
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(string to, string text)
        {
            var status = false;
            try
            {
                await this._sendEmailService.SendMail(to, "Gabinet YourSmile przypomnienie o wizycie", text);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return Json(status, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveEvent(Wizyta w)
        {
            var status = await this._wizytService.SaveEvent(w, User.Identity.Name);
            return new JsonResult { Data = new { status } };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteEvent(Wizyta w)
        {
            var status = await this._wizytService.DeleteEvent(w);
            return new JsonResult { Data = new { status = status } };
        }

    }
}
