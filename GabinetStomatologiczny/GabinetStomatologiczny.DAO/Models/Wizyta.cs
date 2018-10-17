using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.DAO.Models
{
    public class Wizyta
    {
        public int ID { get; set; }
        public int LekarzID { get; set; }
        public int PacjentID { get; set; }
        public string Tytul { get; set; }

        [UIHint("DateTimePicker")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "datetime2")]
        public DateTime Poczatek { get; set; }

        [UIHint("DateTimePicker")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "datetime2")]
        public DateTime Koniec { get; set; }
        public DateTime DataDodania { get; set; }
        public virtual Lekarz Lekarz { get; set; }
        public virtual Pacjent Pacjent { get; set; }
        public virtual ICollection<Zabieg> Zabiegi { get; set; }

    }
}