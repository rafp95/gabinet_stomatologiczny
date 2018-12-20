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

    public interface INotyficatorComponnets
    {
        void RegisterNotyfication(DateTime currentTime);
    }

    public class NotyficatorComponnets : INotyficatorComponnets
    {

        public NotyficatorComponnets()
        {

        }

        public void RegisterNotyfication(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["NotyficationConnection"].ConnectionString;
            string sqlCommand = @"SELECT [PacjentID] FROM [dbo].[Wizyta] Where [Poczatek] >= @Poczatek";
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

                RegisterNotyfication(DateTime.UtcNow);


            }
        }

    }
}