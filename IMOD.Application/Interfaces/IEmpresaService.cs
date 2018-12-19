// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 05 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IEmpresaService : IEmpresaRepositorio
    {
        #region  Propriedades

        /// <summary>
        /// Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }


        /// <summary>
        ///     Signatário serviços
        /// </summary>
        IEmpresaSignatarioService SignatarioService { get; }


        /// <summary>
        ///     Contrato serviços
        /// </summary>
        IEmpresaContratosService ContratoService { get; }


        /// <summary>
        ///     Anexo serviços
        /// </summary>
        IEmpresaAnexoService AnexoService { get; }


        /// <summary>
        /// Tipo de Atividade serviços
        /// </summary>
        ITipoAtividadeService TipoAtividadeService { get; }


        /// <summary>
        /// Tipo de Atividade (por Empresa) serviços
        /// </summary>
        IEmpresaTipoAtividadeService EmpresaTipoAtividadeService { get; }


        /// <summary>
        /// Empresa Layout Crachá serviços
        /// </summary>
        ILayoutCrachaService LayoutCrachaService { get; }


        /// <summary>
        /// Empresa Layout Crachá serviços
        /// </summary>
        IEmpresaLayoutCrachaService EmpresaLayoutCrachaService { get; }


        /// <summary>
        /// Area Acesso serviços
        /// </summary>
        IAreaAcessoService AreaAcessoService { get; }


        /// <summary>
        /// Empresa Area Acesso Serviços
        /// </summary>
        IEmpresaAreaAcessoService EmpresaAreaAcessoService { get; }


        /// <summary>
        /// Veículo Empresa Serviços
        /// </summary>
        IVeiculoEmpresaService VeiculoEmpresaService { get; }


        #endregion
    }
}