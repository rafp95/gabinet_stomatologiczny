using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class Zabieg
    {
        public int ID { get; set; }
        public int WizytaID { get; set; }

        [Display(Name = "Typ zabiegu")]
        public TypZabiegu TypZabiegu { get; set; }

        [Display(Name = "Cena zabiegu")]
        public decimal CenaZabiegu { get; set; }

        public virtual Wizyta Wizyta { get; set; }

    }
}