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
        protected GabinetContext db;

        protected readonly Logger logger = null;
        

        public BaseController(GabinetContext db)
        {
            this.db = db;
            this.logger = LogManager.GetLogger(this.GetType().Name);
            Trace.WriteLine(this);
            logger.Info(this);
        }

        public BaseController()
        {
            this.db = new GabinetContext();
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
            //if (disposing)
            //{
            //    if (db != null)
            //    {
            //        this.db.Dispose();
            //        this._db = null;
            //    }
            //}

            base.Dispose(disposing);
        }

    }
}
