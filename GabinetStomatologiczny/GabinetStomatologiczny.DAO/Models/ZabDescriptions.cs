using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.DAO.Models
{
    public class ZabDescriptions
    {
        public int ID { get; set; }
        public int ZabID { get; set; }
        public string Description { get; set; }

        public virtual Zab Zab { get; set; }
    }
}
