using GabinetStomatologiczny.DAL;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using GabinetStomatologiczny;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GabinetStomatologiczny
{
    public class NotyficatorComponnets
    {

        public NotyficatorComponnets()
        {

        }

        public void RegisterNotyfication(DateTime currentTime)
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ASPNET_GABINETSTOMATOLOGICZNY2_dabb89d6696748e496a6d3b4a582a5a0;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlCommand = @"SELECT [Tytul] FROM [dbo].[Wizyta]";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@Poczatek", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //read notyfications
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                   
                }

            }
        }

        private void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                //send message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotyficationsHub>();
                notificationHub.Clients.All.notify("added");

                RegisterNotyfication(DateTime.Now);


            }
        }

    }
}