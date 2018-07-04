using GabinetStomatologiczny.DAL;
using NLog;
using NLog.Fluent;
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
        private GabinetContext _db;

        protected GabinetContext db
        {
            get
            {
                    if(_db == null)
                        {
                            _db = new GabinetContext();
                        }
                    return _db;
            }
           
        }
        
        private Logger logger = LogManager.GetCurrentClassLogger();
        

        public BaseController()
        {
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
                    this._db = null;
                }
            }

            base.Dispose(disposing);
        }

    }
}
