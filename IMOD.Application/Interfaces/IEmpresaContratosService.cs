// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IEmpresaContratosService : IEmpresaContratoRepositorio
    {
        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

        /// <summary>
        ///     AreaAceesso serviços
        /// </summary>
        IAreaAcessoService AreaAceesso { get; }

        /// <summary>
        ///     TipoAcesso serviços
        /// </summary>
        ITiposAcessoService TipoAcesso { get; }

        /// <summary>
        ///     Status serviços
        /// </summary>
        IStatusService Status { get; }

        /// <summary>
        ///     TipoCobranca serviços
        /// </summary>
        ITipoCobrancaService TipoCobranca { get; }

        /// <summary>
        ///     Estado serviços
        /// </summary>
        IEstadoService Estado { get; }

        /// <summary>
        ///     Municipio serviços
        /// </summary>
        IMunicipioService Municipio { get; }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar um contrato básico
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dataValidade">Data de Validade</param>
        /// <param name="status">Status do contrato</param>
        void CriarContratoBasico(Empresa entity, DateTime dataValidade, Status status);

        #endregion
    }
}