using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.ViewModel
{
    public class PacjentViewModelHistory
    {
        public int ID { get; set; }
        public string Tytul { get; set; }
        public TypZabiegu TypZabiegu { get; set; }
        public int CenaZabiegu { get; set; }
        public DateTime? ZabiegDate { get; set; }
    }
}
