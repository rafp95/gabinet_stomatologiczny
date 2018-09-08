using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.DAO.Models
{
    public enum OddzialNfz
    {
        [Display(Name = " -- Wybierz oddział NFZ --")]
        __________,
        Luków,
        Warszawa,
        Siedlce,
        [Display(Name = "Radzyń Podlaski")]
        Radzyń_Podlaski
    }
}