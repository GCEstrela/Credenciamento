// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using IMOD.Infra.Servicos;
using IMOD.Infra.Servicos.Entities;

#endregion

namespace IMOD.Application.Service
{
    public class VeiculoCredencialService : IVeiculoCredencialService
    {
        #region Variaveis Globais

        private readonly IVeiculoCredencialRepositorio _repositorio = new VeiculoCredencialRepositorio();

        #endregion

        #region  Metodos

        private void ObterStatusCredencial(VeiculoCredencial entity)
        {
            var status = CredencialStatus.BuscarPelaChave (entity.CredencialStatusId);
            entity.Ativa = status.Codigo == "1"; //Set status da credencial
        }

        /// <summary>
        ///     Set dados de um titular de cartao
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static CardHolderEntity CardHolderEntity(VeiculosCredenciaisView entity)
        {
            var titularCartao = new CardHolderEntity
            {
                Ativo = entity.Ativa,
                Empresa = entity.EmpresaNome,
                Nome = entity.VeiculoNome,
                Identificador = entity.PlacaIdentificador, 
                IdentificadorCardHolderGuid = entity.CardHolderGuid,
                IdentificadorCredencialGuid = entity.CredencialGuid,
                FacilityCode = entity.Fc,
                Foto = entity.VeiculoFoto.ConverterBase64StringToBitmap(),
                Matricula = entity.PlacaIdentificador,
                Validade = entity.Validade ?? DateTime.Today.Date,
                NumeroCredencial = entity.NumeroCredencial,
                IdentificadorLayoutCrachaGuid = entity.LayoutCrachaGuid
            };
            return titularCartao;

        }

        public VeiculosCredenciaisView BuscarCredencialPelaChave(int veiculoCredencialId)
        {
            return _repositorio.BuscarCredencialPelaChave (veiculoCredencialId);
        }

        /// <summary>
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        public VeiculoCredencial ObterCredencialPeloNumeroCredencial(string numCredencial)
        {
            return _repositorio.ObterCredencialPeloNumeroCredencial (numCredencial);
        }

        /// <summary>
        ///     Obter credencial
        /// </summary>
        /// <param name="veiculoCredencialId"></param>
        /// <returns></returns>
        public AutorizacaoView ObterCredencialView(int veiculoCredencialId)
        {
            return _repositorio.ObterCredencialView (veiculoCredencialId);
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
        /// <param name="equiapmentoVeiculoId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        public DateTime? ObterDataValidadeCredencial(VeiculoCredencial entity, int equiapmentoVeiculoId, string numContrato, ITipoCredencialRepositorio credencialRepositorio)
        {
            return _repositorio.ObterDataValidadeCredencial (entity, equiapmentoVeiculoId, numContrato, credencialRepositorio);
        }

        /// <summary>
        ///     Criar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        /// <param name="credencialService"></param>
        public void Criar(VeiculoCredencial entity, int colaboradorId,
            ITipoCredencialRepositorio credencialService)
        {
            ObterStatusCredencial (entity);
            _repositorio.Criar (entity, colaboradorId, TipoCredencial);
        }

        /// <summary>
        ///     Alterar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="veiculoId"></param>
        /// <param name="credencialRepositorio"></param>
        public void Alterar(VeiculoCredencial entity, int veiculoId, ITipoCredencialRepositorio credencialRepositorio)
        {
            _repositorio.Alterar (entity, veiculoId, credencialRepositorio);
        }

        /// <summary>
        ///     Verificar se um número credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        public bool ExisteNumeroCredencial(string numCredencial)
        {
            if (string.IsNullOrWhiteSpace (numCredencial)) return false;

            var doc = numCredencial.RetirarCaracteresEspeciais();
            var n1 = ObterCredencialPeloNumeroCredencial (doc);
            return n1 != null;
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoCredencial entity)
        {
            ObterStatusCredencial (entity);
            _repositorio.Alterar (entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoCredencial BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave (id);
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoCredencial entity)
        {
            ObterStatusCredencial (entity);
            _repositorio.Criar (entity);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoCredencial> Listar(params object[] objects)
        {
            return _repositorio.Listar (objects);
        }

        /// <summary>
        ///     Listar Veículos e suas credenciais
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ICollection<VeiculosCredenciaisView> ListarView(params object[] objects)
        {
            return _repositorio.ListarView (objects);
        }

        /// <summary>
        ///     Alterar um titular de cartão no  sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService"></param>
        /// <param name="entity"></param>
        /// <param name="entity2"></param>
        public void AlterarStatusTitularCartao(ICredencialService geradorCredencialService, VeiculosCredenciaisView entity, VeiculoCredencial entity2)
        {
            if (geradorCredencialService == null) throw new ArgumentNullException (nameof (geradorCredencialService));
            if (entity == null) throw new ArgumentNullException (nameof (entity));
            if (entity2 == null) throw new ArgumentNullException (nameof (entity2));
            ObterStatusCredencial (entity2);
            //Alterar status de um titual do cartao
            var titularCartao = CardHolderEntity (entity);
            titularCartao.Ativo = entity2.Ativa;
            entity.Ativa = entity2.Ativa; //Atulizar dados para serem exibidas na tela
            entity.Baixa = entity2.Ativa ? (DateTime?) null : DateTime.Today.Date; //Atulizar dados para serem exibidas na tela
            //Alterar dados no sub-sistema de credenciamento
            //A data da baixa está em função do status do titular do cartao e sua credencial
            entity2.Baixa = entity.Baixa;
            Alterar (entity2);
            //Alterar o status do cartao do titular, se houver
            if (string.IsNullOrWhiteSpace (titularCartao.IdentificadorCardHolderGuid)
                & string.IsNullOrWhiteSpace (titularCartao.IdentificadorCredencialGuid)) return;

            //Alterar status do cartao
            geradorCredencialService.AlterarStatusCardHolder (titularCartao);
            //Alterar credencial
            geradorCredencialService.AlterarStatusCredencial (titularCartao);
        }

        /// <summary>
        ///     Criar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        public void Criar(VeiculoCredencial entity, int colaboradorId)
        {
            Criar (entity, colaboradorId, TipoCredencial);
        }

        /// <summary>
        ///     Alterar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        public void Alterar(VeiculoCredencial entity, int colaboradorId)
        {
            Alterar (entity, colaboradorId, TipoCredencial);
        }

        /// <summary>
        ///     Criar um titular de cartão no sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService"> Sub sistema de geração de credenciais de cartão de um titular</param>
        /// <param name="entity"></param>
        public void CriarTitularCartao(ICredencialService geradorCredencialService, VeiculosCredenciaisView entity)
        {
            if (geradorCredencialService == null) throw new ArgumentNullException (nameof (geradorCredencialService));
            if (entity == null) throw new ArgumentNullException (nameof (entity));
            //Somente é permitido criar uma única vez o titular do cartão...
            //Os numeros GUIDs são indicação  de que já houve criação de credenciais no sub-sistema de credenciamento
            if (!string.IsNullOrWhiteSpace (entity.CardHolderGuid) & !string.IsNullOrWhiteSpace (entity.CredencialGuid)) return;

            var titularCartao = CardHolderEntity (entity);
            //Gerar titular do cartão no sub-sistema de credenciamento (Genetec)
            geradorCredencialService.CriarCardHolder (titularCartao);
            //Gerar Credencial do titular do cartão no sub-sistema de credenciamento (Genetec)
            geradorCredencialService.CriarCredencial (titularCartao);
            //Atualizar dados do identificador GUID
            entity.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
            entity.CredencialGuid = titularCartao.IdentificadorCredencialGuid;
            var n1 = BuscarPelaChave (entity.VeiculoCredencialId);
            n1.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
            n1.CredencialGuid = titularCartao.IdentificadorCredencialGuid;

            Alterar (n1);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(VeiculoCredencial entity)
        {
            _repositorio.Remover (entity);
        }

        public ICollection<AutorizacaoView> ListarAutorizacaoView(params object[] objects)
        {
            return _repositorio.ListarAutorizacaoView (objects);
        }
        
        #endregion

        

        /// <summary>
        ///     Impressão Serviços
        /// </summary>
        public IVeiculoCredencialimpressaoService ImpressaoCredencial
        {
            get { return new VeiculoCredencialimpressaoService(); }
        }

        public ITecnologiaCredencialService TecnologiaCredencial
        {
            get { return new TecnologiaCredencialService(); }
        }

        public ITipoCredencialService TipoCredencial
        {
            get { return new TipoCredencialService(); }
        }

        public ILayoutCrachaService LayoutCracha
        {
            get { return new LayoutCrachaService(); }
        }

        public IFormatoCredencialService FormatoCredencial
        {
            get { return new FormatoCredencialService(); }
        }

        public ICredencialStatusService CredencialStatus
        {
            get { return new CredencialStatusService(); }
        }

        public ICredencialMotivoService CredencialMotivo
        {
            get { return new CredencialMotivoService(); }
        }

        
    }
}