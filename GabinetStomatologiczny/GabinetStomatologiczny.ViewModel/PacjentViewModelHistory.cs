using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.ViewModel
{
    public class PacjentViewModelHistory
    {
        public int ID { get; set; }
        public string Tytul { get; set; }
        [Display(Name ="Typ zabiegu")]
        public TypZabiegu TypZabiegu { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Cena zabiegu")]
        public int CenaZabiegu { get; set; }
        [Display(Name = "Data zabiegu")]
        public DateTime? ZabiegDate { get; set; }
    }
}
