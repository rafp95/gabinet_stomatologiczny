using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.DAO.Models
{
    public class LekarzSort
    {
        public RodzajSort sortOrder { get; set; }
        public string currentFilter { get; set; }
        public string searchString { get; set; }
        public int? page { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }
}
