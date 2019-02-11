// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class ColaboradorCredencialService : IColaboradorCredencialService
    {
        #region Variaveis Globais

        private readonly IColaboradorCredencialRepositorio _repositorio = new ColaboradorCredencialRepositorio();

        #endregion

        #region  Metodos

        private void ObterStatusCredencial(ColaboradorCredencial entity)
        {
            var status = CredencialStatus.BuscarPelaChave (entity.CredencialStatusId);
            entity.Ativa = status.Codigo == "1"; //Set status da credencial
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorCredencial entity)
        {
            ObterStatusCredencial (entity);
            _repositorio.Alterar (entity);
        }

        public ColaboradoresCredenciaisView BuscarCredencialPelaChave(int colaboradorCredencialId)
        {
            return _repositorio.BuscarCredencialPelaChave (colaboradorCredencialId);
        }

        /// <summary>
        ///     Obter credencial
        /// </summary>
        /// <param name="colaboradorCredencialId"></param>
        /// <returns></returns>
        public CredencialView ObterCredencialView(int colaboradorCredencialId)
        {
            return _repositorio.ObterCredencialView (colaboradorCredencialId);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ColaboradorCredencial BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(ColaboradorCredencial entity)
        {
            ObterStatusCredencial (entity);
            _repositorio.Criar (entity);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<ColaboradorCredencial> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     Listar Colaboradores e suas credenciais
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ICollection<ColaboradoresCredenciaisView> ListarView(params object[] o)
        {
            return _repositorio.ListarView (o);
        }
         

        /// <summary>
        ///     Obtem a data de validade de uma credencial
        ///     <para>
        ///         Verificar se o contrato é temporário ou permanente,
        ///         sendo permanente, então vale obter a menor data entre
        ///         um curso controlado e uma data de validade do contrato, caso contrario, será concedido prazo de 90 dias a
        ///         partir da data atual
        ///     </para>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        public DateTime? ObterDataValidadeCredencial(ColaboradorCredencial entity, int colaboradorId, string numContrato, ITipoCredencialRepositorio credencialRepositorio)
        {
            return _repositorio.ObterDataValidadeCredencial (entity, colaboradorId, numContrato, credencialRepositorio);
        }

        /// <summary>
        ///     Criar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        /// <param name="credencialService"></param>
        public void Criar(ColaboradorCredencial entity, int colaboradorId,
            ITipoCredencialRepositorio credencialService)
        {
            ObterStatusCredencial (entity);
            _repositorio.Criar (entity, colaboradorId, TipoCredencial);
        }

        /// <summary>
        ///     Alterar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="credencialRepositorio"></param>
        public void Alterar(ColaboradorCredencial entity, int colaboradorId, ITipoCredencialRepositorio credencialRepositorio)
        {
            _repositorio.Alterar (entity, colaboradorId, credencialRepositorio);
        }

        /// <summary>
        ///     Criar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        public void Criar(ColaboradorCredencial entity, int colaboradorId)
        {
            Criar (entity, colaboradorId, TipoCredencial);
        }

        /// <summary>
        ///     Alterar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        public void Alterar(ColaboradorCredencial entity, int colaboradorId)
        {
            Alterar (entity, colaboradorId, TipoCredencial);
        }

        /// <summary>
        ///     Listar contratos
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresaView> ListarContratos(params object[] o)
        {
            return _repositorio.ListarContratos (o);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(ColaboradorCredencial entity)
        {
            _repositorio.Remover (entity);
        }

        #endregion

        #region Construtores

        /// <summary>
        ///     Impressão Serviços
        /// </summary>
        public IColaboradorCredencialImpressaoService ImpressaoCredencial
        {
            get { return new ColaboradorCredencialImpressaoService();}
        }

        /// <summary>
        ///     TecnologiaCredencial serviços
        /// </summary>
        public ITecnologiaCredencialService TecnologiaCredencial
        {
            get { return new TecnologiaCredencialService(); }
        }

        /// <summary>
        ///     TipoCredencial serviços
        /// </summary>
        public ITipoCredencialService TipoCredencial
        {
            get { return new TipoCredencialService(); }
        }

        /// <summary>
        ///     LayoutCracha serviços
        /// </summary>
        public ILayoutCrachaService LayoutCracha
        {
            get { return new LayoutCrachaService(); }
        }

        /// <summary>
        ///     FormatoCredencial serviços
        /// </summary>
        public IFormatoCredencialService FormatoCredencial
        {
            get { return new FormatoCredencialService(); }
        }

        /// <summary>
        ///     CredencialStatus serviços
        /// </summary>
        public ICredencialStatusService CredencialStatus
        {
            get { return new CredencialStatusService(); }
        }

        /// <summary>
        ///     CredencialMotivo serviços
        /// </summary>
        public ICredencialMotivoService CredencialMotivo
        {
            get { return new CredencialMotivoService(); }
        }

        #endregion
    }
}