using AutoMapper;
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
        Task<IEnumerable<Lekarz>> getAllLekarz();
        Task <Lekarz> About(int? id);
        Task <Lekarz> Edit(Lekarz lekarz);
        Task<Lekarz> Find(int? id);
        Task <Lekarz> Remove(Lekarz lekarz);
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

        public async Task<IEnumerable<Lekarz>> getAllLekarz()
        {
            return await this._db.Lekarz.ToListAsync();
        }

        public async Task<Lekarz> About(int? id)
        {
            return await this._db.Lekarz.Where(_ => _.ID == id).FirstOrDefaultAsync();
        }

        public async Task<Lekarz> Edit(Lekarz lekarz)
        {
            var lekarzBuf = await _db.Lekarz.Where(_ => _.Email == lekarz.Email).FirstOrDefaultAsync();
            lekarz.DataDodania = DateTime.Now;
            if (string.IsNullOrEmpty(lekarz.Zdjecie))
            {
                lekarz.Zdjecie = lekarzBuf.Zdjecie;
            }
            var l = Mapper.Map<Lekarz, Lekarz>(lekarz, lekarzBuf);
            this._db.Entry(l).State = EntityState.Modified;
            await this._db.SaveChangesAsync();
            return lekarzBuf;
        }

        public async Task<Lekarz> Remove(Lekarz lekarz)
        {
            if (lekarz != null)
            {
                this._db.Lekarz.Remove(lekarz);
                await this._db.SaveChangesAsync();
            }
            return lekarz;
        }

        public async Task<Lekarz> Find(int? id)
        {
            return await this._db.Lekarz.Where(_ => _.ID == id).FirstOrDefaultAsync();
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
