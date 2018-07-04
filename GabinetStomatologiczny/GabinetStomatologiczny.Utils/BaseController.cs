using GabinetStomatologiczny.DAL;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace GabinetStomatologiczny.Utils
{
    public abstract class BaseController : Controller
    {
        protected GabinetContext db = new GabinetContext();
        private Logger logger = null;

        public BaseController()
        {
            logger = LogManager.GetLogger("fileLogger");
            logger.Info(this);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string user = User.Identity.Name;
            if (!string.IsNullOrEmpty(user))
            {
                logger.Info("User " + user);
            }
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
