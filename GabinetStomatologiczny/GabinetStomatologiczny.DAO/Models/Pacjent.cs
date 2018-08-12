using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabinetStomatologiczny.DAO.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabinetStomatologiczny.DAO.Models
{
    public class Pacjent
    {
        public int ID { get; set; }

        [Required (ErrorMessage = "Pole imię jest wymagane")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Pole nazwisko jest wymagane")]
        public string Nazwisko { get; set; }

        [RegularExpression(@"^\d{11}$",ErrorMessage = "Pesel nieprawidłowy")]
        public string Pesel { get; set; }
        public Plec Plec { get; set; }

        [DataType(DataType.Date)]
        [UIHint("DateTimePicker")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime2")]
        [Display(Name = "Data urodzenia")]
        public DateTime? DataUrodzenia { get; set; }

        [Required(ErrorMessage = "Pole numer telefonu jest wymagane")]
        [Display (Name = "Numer telefonu")]
        [Phone]
        [RegularExpression(@"([\+]){0,1}([0-9]{2})?[\-\s]?[-]?([0-9]{3})\-?[-\s]?([0-9]{3})[-\s]\-?([0-9]{3})$",
            ErrorMessage = "Numer musi być zapisany w formacie 123-123-123")]
        [StringLength(11)]
        public string NrTelefonu { get; set; }

        [Required(ErrorMessage = "Pole email jest wymagane")]
        [EmailAddress(ErrorMessage = "Podaj poprawy adres email")]
        public string Email { get; set; }
        public string Miejscowosc { get; set; }

        [RegularExpression(@"^\d{2}-\d{3}$",ErrorMessage = "Podany kod pocztowy jest nie poprawny")]
        public string Kod { get; set; }
        public string Ulica { get; set; }
        public Wojewodztwo Wojewodztwo { get; set; }
        public string Kraj { get; set; }

        [Display(Name = "Odział NFZ")]
        public OddzialNfz OdzialNfz { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data rejestracji")]
        public DateTime DataRejestracji { get; set; }

        [Display(Name = "Typ pracownika")]
        public TypPracownika TypPracownika { get; set; }

        public virtual ICollection<Wizyta> Wizyty { get; set; }


    }
}