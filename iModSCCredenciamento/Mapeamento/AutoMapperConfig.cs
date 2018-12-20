// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using AutoMapper;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using Colaborador = IMOD.Domain.Entities.Colaborador;
using EmpresaLayoutCrachaView = IMOD.Domain.EntitiesCustom.EmpresaLayoutCrachaView;
using VeiculoEmpresaView = IMOD.Domain.EntitiesCustom.VeiculoEmpresaView;

#endregion

namespace iModSCCredenciamento.Mapeamento
{
    public class AutoMapperConfig
    {
        #region  Metodos

        public static void RegisterMappings()
        {
            Mapper.Initialize(
                   m =>
                   { 
                        m.CreateMap<Colaborador, ClasseColaboradores.Colaborador>()
                       .ForMember(k => k.CPF, opt => opt.MapFrom(k => k.Cpf.FormatarCpf())).ReverseMap();
                       m.CreateMap<ColaboradorCurso, ClasseColaboradoresCursos.ColaboradorCurso>().ReverseMap();
                       m.CreateMap<ClasseColaboradoresCredenciais, ClasseColaboradoresCredenciais.ColaboradorCredencial>().ReverseMap();
                       m.CreateMap<ColaboradoresCredenciaisView, ClasseColaboradoresCredenciais.ColaboradorCredencial>().ReverseMap();
                       m.CreateMap<ColaboradorEmpresa, ClasseColaboradoresEmpresas.ColaboradorEmpresa>().ReverseMap();
                       m.CreateMap<VeiculoEmpresa, ClasseVeiculosEmpresas.VeiculoEmpresa>().ReverseMap();
                       m.CreateMap<VeiculoCredencial, ClasseVeiculosCredenciais.VeiculoCredencial>().ReverseMap();
                       m.CreateMap<VeiculosCredenciaisView, ClasseVeiculosCredenciais.VeiculoCredencial>().ReverseMap();
                       m.CreateMap<VeiculoEmpresa, ClasseVeiculosEmpresas.VeiculoEmpresa>().ReverseMap();
                       m.CreateMap<VeiculoEmpresaView, ClasseVeiculosEmpresas.VeiculoEmpresa>().ReverseMap();
                       m.CreateMap<Empresa, ClasseEmpresas.Empresa>()
                      .ForMember(k => k.CNPJ, opt => opt.MapFrom(k => k.Cnpj.FormatarCnpj())).ReverseMap();
                       m.CreateMap<LayoutCracha, ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>().ReverseMap();
                       m.CreateMap<Estados, ClasseEstados.Estado>().ReverseMap();
                       m.CreateMap<Municipio, ClasseMunicipios.Municipio>().ReverseMap();
                       m.CreateMap<EmpresaSignatario, ClasseEmpresasSignatarios.EmpresaSignatario>().ReverseMap();
                       m.CreateMap<EmpresaContrato, ClasseEmpresasContratos.EmpresaContrato>().ReverseMap();
                       m.CreateMap<TipoAtividade, ClasseTiposAtividades.TipoAtividade>().ReverseMap();
                       m.CreateMap<TipoEquipamento, ClasseTiposEquipamento.TipoEquipamento>().ReverseMap();

                       m.CreateMap<Pendencia, ClassePendencias.Pendencia>().ReverseMap();
                       m.CreateMap<Pendencia, PendenciaView>().ReverseMap();
                       m.CreateMap<TipoPendencia, TipoPendenciaView>().ReverseMap();

                       m.CreateMap<EmpresaTipoAtividade, ClasseEmpresasTiposAtividades.EmpresaTipoAtividade>().ReverseMap();
                       m.CreateMap<AreaAcesso, ClasseAreasAcessos.AreaAcesso>().ReverseMap();
                       m.CreateMap<LayoutCracha, ClasseLayoutsCrachas.LayoutCracha>().ReverseMap();
                       m.CreateMap<EmpresaLayoutCrachaView, ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>().ReverseMap();
                       m.CreateMap<EmpresaContrato, ClasseEmpresasContratos.EmpresaContrato>().ReverseMap();

                   });
        }

        #endregion
    }
}