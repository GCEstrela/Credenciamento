// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 10 - 2018
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
        ///     Pendência serviços
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

        IEmpresaTipoAtividadeService Atividade { get; }

        /// <summary>
        ///     Empresa Layout Crachá serviços
        /// </summary>
        IEmpresaLayoutCrachaService CrachaService { get; }

        /// <summary>
        ///     Empresa Area Acesso Serviços
        /// </summary>
        IEmpresaAreaAcessoService AreaAcessoService { get; }

        /// <summary>
        ///     Veículo Empresa Serviços
        /// </summary>
        IVeiculoEmpresaService VeiculoService { get; }

        /// <summary>
        /// Verificar se existe CNPJ cadastrado
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        bool ExisteCnpj(string cnpj);

        #endregion
    }
}