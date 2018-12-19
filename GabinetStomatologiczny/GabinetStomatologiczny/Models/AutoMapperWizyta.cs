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
               opts => opts.MapFrom(src => src.Poczatek.AddMinutes(15)))
               .ForMember(dest => dest.Lekarz,
               opts => opts.Ignore())
               .ForPath(dest => dest.Pacjent.Imie , opts => opts.Ignore())
               .ForPath(dest => dest.Pacjent.Nazwisko, opts => opts.Ignore());

            CreateMap<LiczbaPacjentow, LiczbaPacjentow>();
            CreateMap<ZabDescriptions,ZabDescriptions>().
                ForMember(dest => dest.ZabID,opts => opts.MapFrom(src => src.ZabID)).
                ForMember(dest => dest.ZabDateDescription ,opts => opts.MapFrom(src => src.ZabDateDescription));
     
        }

    }
}