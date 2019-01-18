﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using System;
using AutoMapper;
//using iModSCCredenciamento.Models;
using iModSCCredenciamento.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using iModSCCredenciamento.Views.Model;
using Colaborador = IMOD.Domain.Entities.Colaborador;
using EmpresaLayoutCrachaView = IMOD.Domain.EntitiesCustom.EmpresaLayoutCrachaView;
using VeiculoEmpresaView = IMOD.Domain.EntitiesCustom.VeiculoEmpresaView;
using EstadoView = iModSCCredenciamento.Views.Model.EstadoView;
using EmpresaTipoAtividadeView = iModSCCredenciamento.Views.Model.EmpresaTipoAtividadeView;

#endregion

namespace iModSCCredenciamento.Mapeamento
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
                       //m.CreateMap<ColaboradorView, Colaborador>().ReverseMap();
                       m.CreateMap<ColaboradorCurso, ColaboradorCursoView>().ReverseMap();
                       m.CreateMap<ColaboradorCursoView, ColaboradorCurso>().ReverseMap();

                       m.CreateMap<ColaboradorCredencial, ColaboradoresCredenciaisView>().ReverseMap();

                       m.CreateMap<VeiculosCredenciaisView, VeiculoCredencial>().ReverseMap();
                       m.CreateMap<VeiculoEmpresaView, VeiculoEmpresa>().ReverseMap();
                       m.CreateMap<VeiculoEmpresa, VeiculoEmpresa>().ReverseMap();
                       m.CreateMap<IMOD.Domain.EntitiesCustom.VeiculosCredenciaisView, iModSCCredenciamento.Views.Model.VeiculoCredencialView>().ReverseMap();

                       m.CreateMap<Empresa, EmpresaView>().ForMember(k => k.Cnpj, opt => opt.MapFrom(k => k.Cnpj.FormatarCnpj())).ReverseMap();
                       m.CreateMap<EmpresaSignatario, EmpresaSignatarioView>().ForMember(k => k.Cpf, opt => opt.MapFrom(k => k.Cpf.FormatarCpf())).ReverseMap();
                       m.CreateMap<EmpresaContratoView, EmpresaContrato>().ReverseMap();
                       m.CreateMap<IMOD.Domain.EntitiesCustom.EmpresaTipoCredencialView, iModSCCredenciamento.Views.Model.EmpresaTipoCredencialView>().ReverseMap();

                       m.CreateMap<LayoutCracha, EmpresaLayoutCrachaView>().ReverseMap();
                       m.CreateMap<Estados, EstadoView>().ReverseMap();
                       m.CreateMap<Municipio, MunicipioView>().ReverseMap();


                       m.CreateMap<TipoAtividade, TipoAtividadeView>().ReverseMap();

                       m.CreateMap<Pendencia, PendenciaView>().ReverseMap();
                       m.CreateMap<Pendencia, PendenciaView>().ReverseMap();
                       m.CreateMap<TipoPendencia, TipoPendenciaView>().ReverseMap();

                       m.CreateMap<EmpresaTipoAtividade, EmpresaTipoAtividadeView>().ReverseMap();

                       m.CreateMap<IMOD.Domain.EntitiesCustom.EmpresaTipoAtividadeView, EmpresaTipoAtividadeView>()
                       .ReverseMap();

                       m.CreateMap<AreaAcesso, AreaAcessoView>().ReverseMap();


                       m.CreateMap<LayoutCracha, LayoutCrachaView>().ReverseMap();

                       m.CreateMap<TipoAcesso, TipoAcessoView>().ReverseMap();

                       //m.CreateMap<EmpresaContrato, ClasseEmpresasContratos.EmpresaContrato>().ReverseMap();

                       m.CreateMap<IMOD.Domain.EntitiesCustom.CredencialView, Views.Model.CredencialView>()
                           .ForMember(k => k.Foto, opt => opt.MapFrom(k => Convert.FromBase64String(k.Foto)))
                           .ForMember(k => k.Logo, opt => opt.MapFrom(k => Convert.FromBase64String(k.Logo)))
                           .ReverseMap();

                   });
        }

        #endregion
    }
}