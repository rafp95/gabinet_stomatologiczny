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

        [Required(ErrorMessage = "Pole numer telefonu jest wymagane")]
        [Display(Name = "Numer telefonu")]
        [Phone]
        [RegularExpression(@"([\+]){0,1}([0-9]{2})?[\-\s]?[-]?([0-9]{3})\-?[-\s]?([0-9]{3})[-\s]\-?([0-9]{3})$",
            ErrorMessage = "Numer musi być zapisany w formacie 123-123-123")]
        [StringLength(11)]
        public string NrTelefonu { get; set; }

        [Display(Name = "Typ pracownika")]
        public RoleType RoleType { get; set; }


        public  virtual ICollection<Wizyta> Wiztyty { get; set; }
    }
}