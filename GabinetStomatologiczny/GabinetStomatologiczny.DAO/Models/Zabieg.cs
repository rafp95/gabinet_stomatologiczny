using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.DAO.Models
{
    public class Zabieg
    {
        public int ID { get; set; }
        public int WizytaID { get; set; }

        [Display(Name = "Typ zabiegu")]
        public TypZabiegu TypZabiegu { get; set; }

        [Display(Name = "Cena zabiegu")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\,\d{1,2})?\s*$", ErrorMessage = "Pole musi być liczbą")]
        [DataType(DataType.Currency)]
        public int CenaZabiegu { get; set; }
        public DateTime? ZabiegDate { get; set; }

        public virtual Wizyta Wizyta { get; set; }
        public virtual ICollection<Zab> Zeby { get; set; }

        [NotMapped]
        public string SearchQuery { get; set; }

    }
}