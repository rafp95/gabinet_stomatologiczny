using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GabinetStomatologiczny.DAO.Models;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace GabinetStomatologiczny.DAL
{
    public class GabinetContext : DbContext
    {

        public GabinetContext()
            :base("Gabinet Stomatologiczny")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Pacjent> Pacjent { get; set; }
        public DbSet<Zabieg> Zabieg { get; set; }
        public DbSet<Lekarz> Lekarz { get; set; }
        public DbSet<Wizyta> Wizyta { get; set; }
        public DbSet<Administrator> Administrator { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


    }
}