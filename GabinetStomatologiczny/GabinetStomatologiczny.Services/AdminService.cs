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
        Task<Administrator> CreateAdmin(Administrator admin);
    }
    public class AdminService : IAdminService
    {
        private readonly GabinetContext _db;

        public AdminService(GabinetContext db)
        {
            this._db = db;
        }

        public async Task<Administrator> CreateAdmin(Administrator admin)
        {
            if (admin != null)
            {
                this._db.Administrator.Add(admin);
                await this._db.SaveChangesAsync();
            }
            return admin;
        }
    }
}
