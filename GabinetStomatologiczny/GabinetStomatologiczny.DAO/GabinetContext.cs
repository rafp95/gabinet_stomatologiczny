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
            :base("DefaultConnection")
        {
          
        }

        public virtual DbSet<Pacjent> Pacjent { get; set; }
        public virtual DbSet<ZabDescriptions> ZabDescriptions { get; set; }
        public virtual DbSet<Zabieg> Zabieg { get; set; }
        public virtual DbSet<Lekarz> Lekarz { get; set; }
        public virtual DbSet<Wizyta> Wizyta { get; set; }
        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Zab> Zab { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}