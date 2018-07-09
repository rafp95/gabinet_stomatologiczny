using AutoMapper;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class AutoMapperLekarz : Profile
    {
        public AutoMapperLekarz()
        {
            CreateMap<Osoba, Lekarz>()
                .ForMember(src => src.Wiztyty , opt => opt.Ignore())
                .ForMember(src => src.ID, opt => opt.Ignore())
                .ForMember(src => src.Zdjecie, opt => opt.Ignore())
                .ForMember(src => src.DataDodania, opt => opt.Ignore());
        }
    }
}