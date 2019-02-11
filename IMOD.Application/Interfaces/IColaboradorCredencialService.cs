// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorCredencialService : IColaboradorCredencialRepositorio
    {
        #region  Propriedades

        /// <summary>
        ///     Impressão Serviços
        /// </summary>
        IColaboradorCredencialImpressaoService ImpressaoCredencial { get; }

        /// <summary>
        ///     TecnologiaCredencial serviços
        /// </summary>
        ITecnologiaCredencialService TecnologiaCredencial { get; }

        /// <summary>
        ///     TipoCredencial serviços
        /// </summary>
        ITipoCredencialService TipoCredencial { get; }

        /// <summary>
        ///     LayoutCracha serviços
        /// </summary>
        ILayoutCrachaService LayoutCracha { get; }

        /// <summary>
        ///     FormatoCredencial serviços
        /// </summary>
        IFormatoCredencialService FormatoCredencial { get; }

        /// <summary>
        ///     CredencialStatus serviços
        /// </summary>
        ICredencialStatusService CredencialStatus { get; }

        /// <summary>
        ///     CredencialMotivo serviços
        /// </summary>
        ICredencialMotivoService CredencialMotivo { get; }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        void Criar(ColaboradorCredencial entity, int colaboradorId);

        /// <summary>
        ///     Alterar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        void Alterar(ColaboradorCredencial entity, int colaboradorId);

        #endregion
    }
}