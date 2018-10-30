using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{
    public interface IAdminService
    {
        void CreateAdmin(Administrator admin);
    }
    public class AdminService : IAdminService
    {
        private readonly GabinetContext _db;

        public AdminService(GabinetContext db)
        {
            this._db = db;
        }

        public void CreateAdmin(Administrator admin)
        {
            this._db.Administrator.Add(admin);
            this._db.SaveChanges();
        }
    }
}
