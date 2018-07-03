using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.DAO.Models
{
    public class Administrator
    {
        public int ID { get; set; }

        [Display(Name = "Typ pracownika")]
        public TypPracownika TypPracownika { get; set; }
        public string Email { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        [Display(Name = "Numer telefonu")]
        public string NrTelefonu { get; set; }

    }
}