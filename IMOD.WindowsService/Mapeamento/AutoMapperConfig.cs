// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using AutoMapper;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using ColaboradorEmpresaView = IMOD.CredenciamentoDeskTop.Views.Model.ColaboradorEmpresaView;
using EmpresaLayoutCrachaView = IMOD.Domain.EntitiesCustom.EmpresaLayoutCrachaView;
using EmpresaTipoAtividadeView = IMOD.CredenciamentoDeskTop.Views.Model.EmpresaTipoAtividadeView;
using EmpresaTipoCredencialView = IMOD.Domain.EntitiesCustom.EmpresaTipoCredencialView;
using EstadoView = IMOD.CredenciamentoDeskTop.Views.Model.EstadoView;
using VeiculoEmpresaView = IMOD.Domain.EntitiesCustom.VeiculoEmpresaView;

#endregion

namespace IMOD.WindowsService.Mapeamento
{
    public class AutoMapperConfig
    {
        #region  Metodos

        //ColaboradorEmpresaView
        public static void RegisterMappings()
        {
            Mapper.Initialize (
                m =>
                {
                    //m.CreateMap<Colaborador, ColaboradorView>().ForMember (k => k.Cpf, opt => opt.MapFrom (k => k.Cpf.FormatarCpf())).ReverseMap();
                    //m.CreateMap<ColaboradorEmpresa, ColaboradorEmpresaView>().ReverseMap();
                    //m.CreateMap<ColaboradorCurso, ColaboradorCursoView>().ReverseMap();
                    //m.CreateMap<ColaboradorCursoView, ColaboradorCurso>().ReverseMap();
                    //m.CreateMap<ColaboradorCredencial, ColaboradoresCredenciaisView>().ReverseMap();
                    //m.CreateMap<ColaboradorAnexo, ColaboradorAnexoView>().ReverseMap();
                    //m.CreateMap<CredencialView, CredencialViewCracha>()
                    //    .ForMember (n => n.Foto2, opt => opt.MapFrom (c => c.Foto == null ? null : Convert.FromBase64String (c.Foto)))
                    //    .ForMember (n => n.Logo2, opt => opt.MapFrom (c => c.Logo == null ? null : Convert.FromBase64String (c.Logo)))
                    //    .ReverseMap();
                    //m.CreateMap<VeiculoView, Veiculo>().ReverseMap();
                    //m.CreateMap<VeiculosCredenciaisView, VeiculoCredencial>().ReverseMap();
                    //m.CreateMap<VeiculoEmpresaView, VeiculoEmpresa>().ReverseMap();
                    //m.CreateMap<Views.Model.VeiculoEmpresaView, VeiculoEmpresa>().ReverseMap();
                    //m.CreateMap<VeiculoEmpresaView, Views.Model.VeiculoEmpresaView>().ReverseMap();
                    //m.CreateMap<VeiculoSeguroView, VeiculoSeguro>().ReverseMap();
                    //m.CreateMap<VeiculoAnexoView, VeiculoAnexo>().ReverseMap();
                    //m.CreateMap<Empresa, EmpresaView>()
                    //    .ForMember (k => k.Cnpj, opt => opt.MapFrom (k => k.Cnpj.FormatarCnpj()))
                    //    .ForMember (c => c.Cep, opt => opt.MapFrom (c => c.Cep.FormataCep()))
                    //    .ReverseMap();
                    //m.CreateMap<EmpresaSignatario, EmpresaSignatarioView>().ForMember (k => k.Cpf, opt => opt.MapFrom (k => k.Cpf.FormatarCpf())).ReverseMap();
                    //m.CreateMap<EmpresaContratoView, EmpresaContrato>().ReverseMap();
                    //m.CreateMap<EmpresaAnexoView, EmpresaAnexo>().ReverseMap();
                    //m.CreateMap<EmpresaTipoCredencialView, Views.Model.EmpresaTipoCredencialView>().ReverseMap();
                    //m.CreateMap<LayoutCracha, EmpresaLayoutCrachaView>().ReverseMap();
                    //m.CreateMap<Estados, EstadoView>().ReverseMap();
                    //m.CreateMap<Municipio, MunicipioView>().ReverseMap();
                    //m.CreateMap<TipoAtividade, TipoAtividadeView>().ReverseMap();
                    //m.CreateMap<Pendencia, PendenciaView>().ReverseMap();
                    //m.CreateMap<Pendencia, PendenciaView>().ReverseMap();
                    //m.CreateMap<TipoPendencia, TipoPendenciaView>().ReverseMap();
                    //m.CreateMap<EmpresaTipoAtividade, EmpresaTipoAtividadeView>().ReverseMap();
                    //m.CreateMap<Domain.EntitiesCustom.EmpresaTipoAtividadeView, EmpresaTipoAtividadeView>().ReverseMap();
                    //m.CreateMap<AreaAcesso, AreaAcessoView>().ReverseMap();
                    //m.CreateMap<LayoutCracha, LayoutCrachaView>().ReverseMap();
                    //m.CreateMap<TipoAcesso, TipoAcessoView>().ReverseMap();
                    //m.CreateMap<TipoRepresentante, TipoRepresentanteView>().ReverseMap();
                    //m.CreateMap<Domain.EntitiesCustom.ColaboradoresCredenciaisView, Views.Model.RelColaboradoresCredenciaisView>().ReverseMap();
                    //m.CreateMap<VeiculosCredenciaisView, RelVeiculosCredenciaisView>().ReverseMap();
                });
        }

        #endregion
    }
}