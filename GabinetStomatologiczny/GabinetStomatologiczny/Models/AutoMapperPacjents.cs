using AutoMapper;
using GabinetStomatologiczny.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GabinetStomatologiczny.Models
{
    public class AutoMapperPacjents : Profile
    {
        public AutoMapperPacjents()
        {
            CreateMap<Osoba,Pacjent>().ForMember(src => src.ID, opt => opt.Ignore())
                                      .ForMember(src => src.Pesel, opt => opt.Ignore())
                                      .ForMember(src => src.DataUrodzenia, opt => opt.Ignore())
                                      .ForMember(src => src.Miejscowosc, opt => opt.Ignore())
                                      .ForMember(src => src.Kod, opt => opt.Ignore())
                                      .ForMember(src => src.Ulica, opt => opt.Ignore())
                                      .ForMember(src => src.Wojewodztwo, opt => opt.Ignore())
                                      .ForMember(src => src.Kraj, opt => opt.Ignore())
                                      .ForMember(src => src.OdzialNfz, opt => opt.Ignore())
                                      .ForMember(src => src.DataRejestracji, opt => opt.Ignore())
                                      .ForMember(src => src.Wizyty, opt => opt.Ignore())
                                      .ForMember(src => src.Plec, opt => opt.Ignore());
        }
    }
}