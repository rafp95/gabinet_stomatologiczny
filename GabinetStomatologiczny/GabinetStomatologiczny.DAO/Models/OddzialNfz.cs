using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.DAO.Models
{
    public enum OddzialNfz
    {
        Luków,
        Warszawa,
        Siedlce,
        [Display(Name = "Radzyń Podlaski")]
        Radzyń_Podlaski
    }
}