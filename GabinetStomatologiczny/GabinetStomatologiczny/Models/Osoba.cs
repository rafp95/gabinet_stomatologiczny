using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class Osoba
    {
        [Display(Name = "Wybierz rodzaj osoby")]
        public TypPracownika TypPracownika { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Podaj poprawy adres email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Haslo { get; set; }

        [Display(Name = "Powtórz hasło")]
        [Required]
        [DataType(DataType.Password)]
        public string PowtorzHaslo { get; set; }

        [Required]
        public string Imie { get; set; }

        [Required]
        public string Nazwisko { get; set; }

        [Display(Name = "Numer telefonu")]
        [Required]
        [Phone]
        [RegularExpression(@"([\+]){0,1}([0-9]{2})?[\-\s]?[-]?([0-9]{3})\-?[-\s]?([0-9]{3})[-\s]\-?([0-9]{3})$",
            ErrorMessage = "Numer musi być zapisany w formacie 123-123-123")]
        public string NrTelefonu { get; set; }
    }
}