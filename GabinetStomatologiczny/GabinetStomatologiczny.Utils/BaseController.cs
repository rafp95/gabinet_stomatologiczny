using GabinetStomatologiczny.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppLogin;

namespace GabinetStomatologiczny.Utils
{
    public abstract class BaseController : Controller
    {
        protected GabinetContext db = new GabinetContext();


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string user = string.IsNullOrEmpty(User.Identity.Name) ? " " : User.Identity.Name;
            Logger.LogInfo(" Controller " + filterContext.Controller + " "  
                +" User " + user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }

            base.Dispose(disposing);
        }

    }
}
