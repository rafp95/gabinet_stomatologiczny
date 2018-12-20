using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GabinetStomatologiczny.DAO.Infrastucture
{
    public class DatePacjentAttribute : ValidationAttribute
    {
        public DatePacjentAttribute() {
            ErrorMessage = "Data musi być wcześniejsza";
        }
        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return true;
            }
            DateTime date = Convert.ToDateTime(value);
            return date > DateTime.UtcNow ? false : true;
        }
    }
}
