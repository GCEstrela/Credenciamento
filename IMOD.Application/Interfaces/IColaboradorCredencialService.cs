// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Application.Service;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Servicos;
using IMOD.Infra.Servicos.Entities;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorCredencialService : IColaboradorCredencialRepositorio
    {
        #region  Propriedades 
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

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
        ///     Criar uma pendência impeditiva caso o motivo do credenciamento possua natureza impeditiva
        /// </summary>
        /// <param name="entity"></param>
        void CriarPendenciaImpeditiva(ColaboradoresCredenciaisView entity);

        /// <summary>
        /// Verificar se um número credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        bool ExisteNumeroCredencial(string numCredencial);
        /// <summary>
        /// Verificar se um número credencial
        /// </summary>
        /// <param name="numColete"></param>
        /// <returns></returns>
        ColaboradorCredencial ExisteNumeroColete(int colavoradorid,string numColete);
        ///// <summary>
        /////     Criar um titular de cartão no sub-sistema de credenciamento (Genetec)
        ///// </summary>
        ///// <param name="geradorCredencialService"> Sub sistema de geração de credenciais de cartão de um titular</param>
        ///// <param name="entity"></param>
        //void CriarTitularCartao(ICredencialService geradorCredencialService, ColaboradoresCredenciaisView entity);

        /// <summary>
        ///     Criar um titular de cartão no sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService">Sub sistema de geração de credenciais de cartão de um titular</param>
        /// <param name="colaboradorService">Colaborador service</param>
        /// <param name="entity"></param>
        void CriarTitularCartao(ICredencialService geradorCredencialService, IColaboradorService colaboradorService, ColaboradoresCredenciaisView entity);

        /// <summary>
        ///     Alterar o status de um titular de cartão no  sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService"></param>
        /// <param name="entity"></param>
        /// <param name="entity2"></param>
        void AlterarStatusTitularCartao(ICredencialService geradorCredencialService, ColaboradoresCredenciaisView entity, ColaboradorCredencial entity2);

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
        /// <summary>
        ///     REmove as REgras de Acesso de um Cardholder
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        void RemoverRegrasCardHolder(ICredencialService geradorCredencialService, IColaboradorService colaboradorService, ColaboradoresCredenciaisView entity);
        /// <summary>
        ///     REmove as REgras de Acesso de um Cardholder
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        void DisparaAlarme(ICredencialService geradorCredencialService, CardHolderEntity entity);


        /// <summary>
        ///     REmove as REgras de Acesso de um Cardholder
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        void RemoverRegrasCardHolder(ICredencialService geradorCredencialService, CardHolderEntity entity);
        #endregion
    }
}