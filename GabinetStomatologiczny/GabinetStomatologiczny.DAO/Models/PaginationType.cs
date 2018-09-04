using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class PaginationType
    {
        public RodzajSort sortOrder { get; set; }
        public string currentFilter { get; set; }
        public string searchString { get; set; }
        public int? page { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int ID { get; set; }
    }
}