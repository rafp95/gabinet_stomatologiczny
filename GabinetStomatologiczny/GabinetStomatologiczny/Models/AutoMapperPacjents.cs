using AutoMapper;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class AutoMapperPacjents<T,T1> : Profile
    {
        public AutoMapperPacjents()
        {
            CreateMap<Osoba,Pacjent>();
        }
    }
}