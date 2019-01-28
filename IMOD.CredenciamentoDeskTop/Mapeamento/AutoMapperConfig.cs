// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using System;
using AutoMapper;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
//using iModSCCredenciamento.Models;
using Colaborador = IMOD.Domain.Entities.Colaborador;
using CredencialView = IMOD.CredenciamentoDeskTop.Views.Model.CredencialView;
using EmpresaLayoutCrachaView = IMOD.Domain.EntitiesCustom.EmpresaLayoutCrachaView;
using VeiculoEmpresaView = IMOD.Domain.EntitiesCustom.VeiculoEmpresaView;
using EstadoView = IMOD.CredenciamentoDeskTop.Views.Model.EstadoView;
using EmpresaTipoAtividadeView = IMOD.CredenciamentoDeskTop.Views.Model.EmpresaTipoAtividadeView;
using EmpresaTipoCredencialView = IMOD.CredenciamentoDeskTop.Views.Model.EmpresaTipoCredencialView;

#endregion

namespace IMOD.CredenciamentoDeskTop.Mapeamento
{
    public class AutoMapperConfig
    {
        #region  Metodos
        //ColaboradorEmpresaView
        public static void RegisterMappings()
        {
            Mapper.Initialize(
                   m =>
                   {
                       m.CreateMap<Colaborador, ColaboradorView>().ForMember(k => k.Cpf, opt => opt.MapFrom(k => k.Cpf.FormatarCpf())).ReverseMap();
                       m.CreateMap<ColaboradorEmpresa, Views.Model.ColaboradorEmpresaView>().ReverseMap();
                       m.CreateMap<ColaboradorCurso, ColaboradorCursoView>().ReverseMap();
                       m.CreateMap<ColaboradorCursoView, ColaboradorCurso>().ReverseMap();
                       m.CreateMap<ColaboradorCredencial, ColaboradoresCredenciaisView>().ReverseMap();
                       m.CreateMap<VeiculosCredenciaisView, VeiculoCredencial>().ReverseMap();
                       m.CreateMap<VeiculoEmpresaView, VeiculoEmpresa>().ReverseMap();
                       m.CreateMap<VeiculoEmpresa, VeiculoEmpresa>().ReverseMap();
                       m.CreateMap<IMOD.Domain.EntitiesCustom.VeiculosCredenciaisView, VeiculoCredencialView>().ReverseMap();
                       m.CreateMap<Empresa, EmpresaView>().ForMember(k => k.Cnpj, opt => opt.MapFrom(k => k.Cnpj.FormatarCnpj())).ReverseMap();
                       m.CreateMap<EmpresaSignatario, EmpresaSignatarioView>().ForMember(k => k.Cpf, opt => opt.MapFrom(k => k.Cpf.FormatarCpf())).ReverseMap();
                       m.CreateMap<EmpresaContratoView, EmpresaContrato>().ReverseMap();
                       m.CreateMap<EmpresaAnexoView, EmpresaAnexo>().ReverseMap();
                       m.CreateMap<Domain.EntitiesCustom.EmpresaTipoCredencialView, EmpresaTipoCredencialView>().ReverseMap();
                       m.CreateMap<LayoutCracha, EmpresaLayoutCrachaView>().ReverseMap();
                       m.CreateMap<Estados, EstadoView>().ReverseMap();
                       m.CreateMap<Municipio, MunicipioView>().ReverseMap();
                       m.CreateMap<TipoAtividade, TipoAtividadeView>().ReverseMap();
                       m.CreateMap<Pendencia, PendenciaView>().ReverseMap();
                       m.CreateMap<Pendencia, PendenciaView>().ReverseMap();
                       m.CreateMap<TipoPendencia, TipoPendenciaView>().ReverseMap();
                       m.CreateMap<EmpresaTipoAtividade, EmpresaTipoAtividadeView>().ReverseMap();
                       m.CreateMap<Domain.EntitiesCustom.EmpresaTipoAtividadeView, EmpresaTipoAtividadeView>().ReverseMap();
                       m.CreateMap<AreaAcesso, AreaAcessoView>().ReverseMap();
                       m.CreateMap<LayoutCracha, LayoutCrachaView>().ReverseMap();
                       m.CreateMap<TipoAcesso, TipoAcessoView>().ReverseMap();
                       m.CreateMap<Domain.EntitiesCustom.CredencialView, CredencialView>()
                           .ForMember(k => k.Foto, opt => opt.MapFrom(k => Convert.FromBase64String(k.Foto)))
                           .ForMember(k => k.Logo, opt => opt.MapFrom(k => Convert.FromBase64String(k.Logo)))
                           .ReverseMap();

                   });
        }

        #endregion
    }
}