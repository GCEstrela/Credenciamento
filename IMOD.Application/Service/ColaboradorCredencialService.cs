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
    public class ColaboradorCredencialService : IColaboradorCredencialService
    {
        #region Variaveis Globais

        private readonly IColaboradorCredencialRepositorio _repositorio = new ColaboradorCredencialRepositorio();

        #endregion

        #region  Propriedades

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        /// <summary>
        ///     Impressão Serviços
        /// </summary>
        public IColaboradorCredencialImpressaoService ImpressaoCredencial
        {
            get { return new ColaboradorCredencialImpressaoService(); }
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

        #region  Metodos

        public void ObterStatusCredencial(ColaboradorCredencial entity)
        {
            var status = CredencialStatus.BuscarPelaChave (entity.CredencialStatusId);
            entity.Ativa = status.Codigo == "1"; //Set status da credencial
        }

        /// <summary>
        ///     Set dados de um titular de cartao
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static CardHolderEntity CardHolderEntity(ColaboradoresCredenciaisView entity)
        {
            //Author: Renato Maximo
            //Data:13/03/19
            //Wrk:Adicionar um dia a credencial
            DateTime dataValidade = entity.Validade == null ? DateTime.Today.Date : (DateTime) entity.Validade; 

            var titularCartao = new CardHolderEntity
            {
                Ativo = entity.Ativa,
                Empresa = entity.EmpresaNome,
                Nome = entity.ColaboradorNome,
                Cnpj = entity.Cnpj,
                Cpf = entity.Cpf,
                Cargo = entity.Cargo,
                Identificador = entity.Cpf,
                Apelido = entity.ColaboradorApelido,
                IdentificadorCardHolderGuid = entity.CardHolderGuid,
                IdentificadorCredencialGuid = entity.CredencialGuid,
                FacilityCode = entity.Fc,
                Foto = entity.ColaboradorFoto.ConverterBase64StringToBitmap(),
                Matricula = entity.Matricula, 
                Validade = dataValidade.AddDays(1), 
                NumeroCredencial = entity.NumeroCredencial,
                IdentificadorLayoutCrachaGuid = entity.LayoutCrachaGuid
            };
            return titularCartao;
        }

        /// <summary>
        ///     Criar uma pendência impeditiva caso o motivo do credenciamento possua natureza impeditiva
        /// </summary>
        /// <param name="entity"></param>
        public void CriarPendenciaImpeditiva(ColaboradoresCredenciaisView entity)
        {
               //Criar um pendenci impeditiva ao constatar o motivo da credencial
               var pendImp = CredencialMotivo.BuscarPelaChave (entity.CredencialMotivoId);
            if (pendImp == null) throw new InvalidOperationException ("Não foi possível obter a entidade credencial motivo");
            var impeditivo = pendImp.Impeditivo & entity.DevolucaoEntregaBoId == 0;
            if (!impeditivo) return;
            //Criar uma pendencia impeditiva,caso sua natureza seja impeditiva

            #region Criar Pendência  

            var pendencia = new Pendencia();
            pendencia.EmpresaId = entity.EmpresaId;
            var motivo = CredencialMotivo.BuscarPelaChave (entity.CredencialMotivoId);
            pendencia.Descricao = $"Em {DateTime.Now}, uma pendência impeditiva foi criada pelo sistema por uma credencial ter sido {motivo.Descricao}." +
                                  $"\r\nEm nome de {entity.ColaboradorNome}" +
                                  "\r\nDeseja-se que tais pendências sejam solucionadas." +
                                  $"\r\nAutor {UsuarioLogado.Nome} {UsuarioLogado.Email}";
            pendencia.Impeditivo = true;
            //--------------------------
            pendencia.CodPendencia = 21;
            Pendencia.CriarPendenciaSistema (pendencia);

            #endregion
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
        ///     Verificar se um número credencial
        /// </summary>
        /// <param name="numColete"></param>
        /// <returns></returns>
        public bool ExisteNumeroColete(int colaboradorid,string numColete)
        {
            if (string.IsNullOrWhiteSpace(numColete)) return false;

            var doc = numColete;
            var n1 = ObterNumeroColete(colaboradorid,doc);
            return n1 != null;
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
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        public ColaboradorCredencial ObterCredencialPeloNumeroCredencial(string numCredencial)
        {
            return _repositorio.ObterCredencialPeloNumeroCredencial (numCredencial);
        }
        /// <summary>
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numColete"></param>
        /// <returns></returns>
        public ColaboradorCredencial ObterNumeroColete(int colaboradorid,string numColete)
        {
            return _repositorio.ObterNumeroColete(colaboradorid,numColete);
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
        ///     Obtem a data de validade de uma credencial
        ///     <para>
        ///         Verificar se o contrato é temporário ou permanente,
        ///         sendo permanente, então vale obter a menor data entre
        ///         um curso controlado e uma data de validade do contrato, caso contrario, será concedido prazo de 90 dias a
        ///         partir da data atual
        ///     </para>
        /// </summary>
        /// <param name="tipoCredencialId"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        public DateTime? ObterDataValidadeCredencial(int tipoCredencialId, int colaboradorId, string numContrato, ITipoCredencialRepositorio credencialRepositorio)
        {
            return _repositorio.ObterDataValidadeCredencial (tipoCredencialId, colaboradorId, numContrato, credencialRepositorio);
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
            ObterStatusCredencial (entity);
            _repositorio.Alterar (entity, colaboradorId, credencialRepositorio);
        }

        /// <summary>
        ///     Alterar um titular de cartão no  sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService"></param>
        /// <param name="entity"></param>
        /// <param name="entity2"></param>
        public void AlterarStatusTitularCartao(ICredencialService geradorCredencialService, ColaboradoresCredenciaisView entity, ColaboradorCredencial entity2)
        {
            if (geradorCredencialService == null) throw new ArgumentNullException (nameof (geradorCredencialService));
            if (entity == null) throw new ArgumentNullException (nameof (entity));
            if (entity2 == null) throw new ArgumentNullException (nameof (entity2));
            ObterStatusCredencial (entity2);
            //Alterar status de um titual do cartao
            var titularCartao = CardHolderEntity (entity); 
            titularCartao.Ativo = entity2.Ativa;

            entity.DataStatus = entity.Ativa != entity2.Ativa ? DateTime.Today.Date : entity2.DataStatus;
            entity.Ativa = entity2.Ativa; //Atulizar dados para serem exibidas na tela

            if ((!entity2.Ativa && (entity2.DevolucaoEntregaBoId != 0)) ||
                    (!entity2.Ativa && (entity2.CredencialMotivoId == 11 || entity2.CredencialMotivoId == 12)))
            {
                entity.Baixa = DateTime.Today.Date;
            }
            else
            {
                entity.Baixa = (DateTime?)null;
            }

            //Alterar dados no sub-sistema de credenciamento
            //A data da baixa está em função do status do titular do cartao e sua credencial
            entity2.DataStatus = entity.DataStatus;
            entity2.Baixa = entity.Baixa;
            Alterar (entity2);
            //Alterar o status do cartao do titular, se houver
            if (string.IsNullOrWhiteSpace (titularCartao.IdentificadorCardHolderGuid)
                & string.IsNullOrWhiteSpace (titularCartao.IdentificadorCredencialGuid)) return;

            //Alterar status do cartao
            geradorCredencialService.AlterarStatusCardHolder (titularCartao);
            //Sistema somente gerar credencial se o tipo de autenticação permitir

            //Alterar credencial
            geradorCredencialService.AlterarStatusCredencial (titularCartao);

            //Alterar status do cartao
            geradorCredencialService.AlterarStatusCardHolder(titularCartao);
            //Sistema somente gerar credencial se o tipo de autenticação permitir

        }

        /// <summary>
        ///     Criar um titular de cartão no sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService">Sub sistema de geração de credenciais de cartão de um titular</param>
        /// <param name="colaboradorService">Colaborador service</param>
        /// <param name="entity"></param>
        public void CriarTitularCartao(ICredencialService geradorCredencialService,IColaboradorService colaboradorService, ColaboradoresCredenciaisView entity)
        {
            if (geradorCredencialService == null) throw new ArgumentNullException(nameof(geradorCredencialService));
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            //Somente é permitido criar uma única vez o titular do cartão...
            //Os numeros GUIDs são indicação  de que já houve criação de credenciais no sub-sistema de credenciamento
            if (!string.IsNullOrWhiteSpace(entity.CardHolderGuid) & !string.IsNullOrWhiteSpace(entity.CredencialGuid)) return;

            var titularCartao = CardHolderEntity(entity);

            #region Setar o valor CardHolder GUID ao colaborador

            //Buscar dados do colaborador
            var co1 = colaboradorService.Empresa.BuscarPelaChave (entity.ColaboradorEmpresaId);
            if (co1 == null) throw new InvalidOperationException("Não foi possive obter um colaborador");

            if (string.IsNullOrWhiteSpace(co1.CardHolderGuid))
            {
                //Gerar titular do cartão no sub-sistema de credenciamento (Genetec)
                geradorCredencialService.CriarCardHolder(titularCartao);
                co1.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
                colaboradorService.Empresa.Alterar(co1);
            }

            titularCartao.IdentificadorCardHolderGuid = co1.CardHolderGuid;


            #endregion

            //Sistema somente gerar credencial se o tipo de autenticação permitir
            //Gerar Credencial do titular do cartão no sub-sistema de credenciamento (Genetec)
            geradorCredencialService.CriarCredencial(titularCartao);
            //Atualizar dados do identificador GUID
            entity.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
            entity.CredencialGuid = titularCartao.IdentificadorCredencialGuid;
            var n1 = BuscarPelaChave(entity.ColaboradorCredencialId);
            n1.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
            n1.CredencialGuid = titularCartao.IdentificadorCredencialGuid;

            Alterar(n1);
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

        public ColaboradorCredencial ObterNumeroColete(string numColete)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Relatório Colaborador credenciais - concedidas
        /// </summary> 
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialConcedidasView(FiltroReportColaboradoresCredenciais entity)
        {
            return _repositorio.ListarColaboradorCredencialConcedidasView(entity);
        }

        /// <summary>
        /// Relatório Colaborador credenciais - vias adicionais 
        /// </summary> 
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialViaAdicionaisView(FiltroReportColaboradoresCredenciais entity)
        {
            return _repositorio.ListarColaboradorCredencialViaAdicionaisView(entity);
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - invalidas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialInvalidasView(FiltroReportColaboradoresCredenciais entity)
        {
            return _repositorio.ListarColaboradorCredencialInvalidasView(entity);
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - impressoes 
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialImpressoesView(FiltroReportColaboradoresCredenciais entity)
        {
            return _repositorio.ListarColaboradorCredencialImpressoesView(entity);
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - permanentes ativos por área
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialPermanentePorAreaView(FiltroReportColaboradoresCredenciais entity)

        {
            return _repositorio.ListarColaboradorCredencialPermanentePorAreaView(entity);
        }

        #endregion
    }
}