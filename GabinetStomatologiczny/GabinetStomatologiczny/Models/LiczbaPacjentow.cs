using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class LiczbaPacjentow
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DataRejestracji { get; set; }
        public int Pacjentow { get; set; }
    }
}