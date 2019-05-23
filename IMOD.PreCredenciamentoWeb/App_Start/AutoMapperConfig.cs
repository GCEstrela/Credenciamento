using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.PreCredenciamentoWeb.Models;

namespace IMOD.PreCredenciamentoWeb.Mapeamento
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(
            m =>
            {

                m.CreateMap<ColaboradorViewModel, Colaborador>().ReverseMap();
                m.CreateMap<Colaborador, ColaboradorViewModel>().ReverseMap();

                m.CreateMap<Veiculo, VeiculoViewModel>().ReverseMap();
                m.CreateMap<EmpresaViewModel, Empresa>().ReverseMap();
            });
        }
    }
}