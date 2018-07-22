using AutoMapper;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class AutoMapperAdmin : Profile
    {
        public AutoMapperAdmin()
        {
            CreateMap<Osoba, Administrator>()
                .ForMember(src => src.ID, opt => opt.Ignore());
        }
    }
}