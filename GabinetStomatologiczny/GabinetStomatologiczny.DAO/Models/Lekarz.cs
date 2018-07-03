using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.DAO.Models
{
    public class Lekarz
    {
        public int ID { get; set; }
        public string  Imie { get; set; }
        public string Nazwisko { get; set; }

        [EmailAddress(ErrorMessage = "Podaj poprawy adres email")]
        public string Email { get; set; }
        public string Zdjecie { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "datetime2")]
        [Display(Name = "Data dodania")]
        public DateTime DataDodania { get; set; }

        [Display(Name = "Numer telefonu")]
        public string NrTelefonu { get; set; }

        [Display(Name = "Typ pracownika")]
        public TypPracownika TypPracownika { get; set; }


        public  virtual ICollection<Wizyta> Wiztyty { get; set; }
    }
}