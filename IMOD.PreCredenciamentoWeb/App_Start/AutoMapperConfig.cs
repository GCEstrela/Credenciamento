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
               
                m.CreateMap<EmpresaViewModel, Empresa>().ReverseMap();

                m.CreateMap<Veiculo, VeiculoViewModel>().ReverseMap();
                m.CreateMap<VeiculoAnexo, VeiculoAnexoViewModel>().ReverseMap();
                m.CreateMap<VeiculoSeguro, VeiculoSeguroViewModel>().ReverseMap();
                m.CreateMap<VeiculoEmpresa, VeiculoEmpresaViewModel>().ReverseMap();
                m.CreateMap<VeiculoObservacao, VeiculoObservacaoViewModel>().ReverseMap();

                m.CreateMap<VeiculoWeb, VeiculoViewModel>().ReverseMap();
                m.CreateMap<VeiculoAnexoWeb, VeiculoAnexoViewModel>().ReverseMap();
                m.CreateMap<VeiculoSeguroWeb, VeiculoSeguroViewModel>().ReverseMap();
                m.CreateMap<VeiculoEmpresaWeb, VeiculoEmpresaViewModel>().ReverseMap();
            });
        }
    }
}