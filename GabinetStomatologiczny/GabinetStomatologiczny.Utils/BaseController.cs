using GabinetStomatologiczny.DAL;
using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                if (_db == null)
                {
                    _db = new GabinetContext();
                }
                return _db;
            }
        }

        protected readonly Logger logger = null;
        

        public BaseController()
        {
            this.logger = LogManager.GetLogger(this.GetType().Name);
            Trace.WriteLine(this);
            logger.Info(this);
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            logger.Info(filterContext.Exception);
            Trace.WriteLine(filterContext.Exception);
            base.OnException(filterContext);
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
