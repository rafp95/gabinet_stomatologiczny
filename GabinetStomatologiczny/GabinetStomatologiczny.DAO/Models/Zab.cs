using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.DAO.Models
{
    public class Zab
    {
        public int ID { get; set; }
        public int ZabiegID { get; set; }
        public string IdTooth { get; set; }

        public virtual Zabieg Zabieg{ get; set; }
    }
}
