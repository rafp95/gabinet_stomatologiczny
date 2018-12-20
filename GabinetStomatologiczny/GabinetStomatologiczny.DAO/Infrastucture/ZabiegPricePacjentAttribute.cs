using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.DAO.Infrastucture
{
    public class ZabiegPricePacjentAttribute : ValidationAttribute
    {
        public ZabiegPricePacjentAttribute()
        {
            ErrorMessage = "Pole musi być liczbą";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            return value.GetType() == typeof(Int32) && (int)value > 0 ? true : false;
        }
    }
}
