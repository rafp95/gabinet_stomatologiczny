using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GabinetStomatologiczny.Services
{

    public interface ILekarzService
    {
        IEnumerable<Lekarz> getAllLekarz();
        Lekarz About(int? id);
        void Edit(Lekarz lekarz);
        Lekarz Find(int? id);
        void Remove(Lekarz lekarz);
        Task<Lekarz> CreateLekarz(Lekarz lekarz);
        HttpPostedFileBase SaveFile(HttpPostedFileBase file, Lekarz lekarz);
    }

    public class LekarzsService : ILekarzService
    {
        private readonly GabinetContext _db;

        public LekarzsService(GabinetContext db)
        {
            this._db = db;
        }

        public IEnumerable<Lekarz> getAllLekarz()
        {
            return this._db.Lekarz.ToList();
        }

        public Lekarz About(int? id)
        {
            return this._db.Lekarz.Find(id);
        }

        public void Edit(Lekarz lekarz)
        {
            lekarz.DataDodania = DateTime.Now;
            this._db.Entry(lekarz).State = EntityState.Modified;
            this._db.SaveChanges();
        }

        public void Remove(Lekarz lekarz)
        {
            if (lekarz != null)
            {
                this._db.Lekarz.Remove(lekarz);
                this._db.SaveChanges();
            }
        }

        public Lekarz Find(int? id)
        {
            return this._db.Lekarz.Find(id);
        }

        public async Task<Lekarz> CreateLekarz(Lekarz lekarz)
        {
            if (lekarz != null)
            {
                this._db.Lekarz.Add(lekarz);
                await this._db.SaveChangesAsync();
            }
            return lekarz;
        }

        public HttpPostedFileBase SaveFile(HttpPostedFileBase file, Lekarz lekarz)
        {

            if (file != null && file.ContentLength > 0)
            {
                lekarz.Zdjecie = System.Guid.NewGuid().ToString();
                lekarz.Zdjecie = file.FileName;
            }
            return file;
        }
    }
}
