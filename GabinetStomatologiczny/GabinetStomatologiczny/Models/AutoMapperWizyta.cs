using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using GabinetStomatologiczny.DAO.Models;

namespace GabinetStomatologiczny.Models
{
    public class AutoMapperWizyta : Profile
    {
        public AutoMapperWizyta()
        {
            CreateMap<Wizyta, Wizyta>()
                .ForMember(dest => dest.Koniec,
               opts => opts.MapFrom(src => src.Poczatek.AddMinutes(30)))
               .ForMember(dest => dest.Lekarz,
               opts => opts.Ignore());
            CreateMap<LiczbaPacjentow, LiczbaPacjentow>();
        }

    }
}