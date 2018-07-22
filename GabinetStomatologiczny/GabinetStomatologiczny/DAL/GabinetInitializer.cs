using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabinetStomatologiczny.DAL;
using System.Data.Entity;
using GabinetStomatologiczny.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GabinetStomatologiczny.DAO.Models;

namespace GabinetStomatologiczny.DAL
{
    public class GabinetInitializer : DropCreateDatabaseIfModelChanges<GabinetContext>
    {
        protected override void Seed(GabinetContext context)
        {

            ////Administrator

            //var roleAdministrator = new RoleManager<IdentityRole>(
            //                  new RoleStore<IdentityRole>(
            //                  new ApplicationDbContext()));

            //var userAdministrator = new UserManager<ApplicationUser>(
            //                  new UserStore<ApplicationUser>(
            //                  new ApplicationDbContext()));

            //roleAdministrator.Create(new IdentityRole("Administrator"));


            //var user = new ApplicationUser { UserName = "admin@o2.pl" };
            //var pass = "Patryk12345!";

            //userAdministrator.Create(user, pass);

            //userAdministrator.AddToRole(user.Id, "Administrator");



            //var roleAdministrator2 = new RoleManager<IdentityRole>(
            //                  new RoleStore<IdentityRole>(
            //                  new ApplicationDbContext()));

            //var userAdministrator2 = new UserManager<ApplicationUser>(
            //                  new UserStore<ApplicationUser>(
            //                  new ApplicationDbContext()));

            //roleAdministrator2.Create(new IdentityRole("Pacjent"));

            //Pacjent

            //var user2 = new ApplicationUser { UserName = "pacjent@o2.pl" };
            //var pass2 = "Patryk12345!";

            //userAdministrator2.Create(user2, pass2);

            //userAdministrator2.AddToRole(user2.Id, "Pacjent");

            //Lekarz

            //var roleAdministrator3 = new RoleManager<IdentityRole>(
            //                 new RoleStore<IdentityRole>(
            //                 new ApplicationDbContext()));

            //var userAdministrator3 = new UserManager<ApplicationUser>(
            //                  new UserStore<ApplicationUser>(
            //                  new ApplicationDbContext()));

            //roleAdministrator3.Create(new IdentityRole("Lekarz"));


            //var user3 = new ApplicationUser { UserName = "lekarz@o2.pl" };
            //var pass3 = "Patryk12345!";

            //userAdministrator3.Create(user3, pass3);

            //userAdministrator3.AddToRole(user3.Id, "Lekarz");

            //context.SaveChanges();

            //base.Seed(context);


        }

    }
}