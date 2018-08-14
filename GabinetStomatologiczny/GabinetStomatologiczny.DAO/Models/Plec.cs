using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.DAO.Models
{
    public enum Plec
    {
        [Display(Name = " -- Wybierz płeć --")]
        __________,
        Mężczyzna,
        Kobieta
    }
}