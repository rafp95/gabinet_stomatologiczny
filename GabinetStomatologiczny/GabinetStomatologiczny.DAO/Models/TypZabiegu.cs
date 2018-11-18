using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.DAO.Models
{
    public enum TypZabiegu
    {
        [Display(Name ="Usługa stomatologiczna")]
        Usługa,
        [Display(Name = "Konsultacja stomatologiczna")]
        Konsultacja
    }
}