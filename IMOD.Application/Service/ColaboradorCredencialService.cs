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
                Identificacao1 = entity.Identificacao1,
                Identificacao2 = entity.Identificacao2,
                Apelido = entity.ColaboradorApelido,
                IdentificadorCardHolderGuid = entity.CardHolderGuid,
                IdentificadorCredencialGuid = entity.CredencialGuid,
                FacilityCode = entity.Fc,
                Foto = entity.ColaboradorFoto.ConverterBase64StringToBitmap(),
                Matricula = entity.Matricula,
                Validade = dataValidade.AddDays(0),
                NumeroCredencial = entity.NumeroCredencial,
                IdentificadorLayoutCrachaGuid = entity.LayoutCrachaGuid,
                FormatoCredencial = entity.FormatoCredencialDescricao.Trim(),
                TecnologiaCredencialId = entity.TecnologiaCredencialId,
                FormatoCredencialId = entity.FormatoCredencialId,
                Fc = entity.Fc,
                Regras = entity.Regras,
                GrupoPadrao = entity.GrupoPadrao
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
            var impeditivo = pendImp.Impeditivo & !entity.DevolucaoEntregaBo;
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
        public ColaboradorCredencial ExisteNumeroColete(int colaboradorid,string numColete)
        {
            if (string.IsNullOrWhiteSpace(numColete)) return null;

            var doc = numColete;
            var n1 = ObterNumeroColete(colaboradorid,doc);
            return n1;
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param> 
        public void Alterar(ColaboradorCredencial entity)
        {

            CredencialMotivo motivo = new CredencialMotivo();
            if (entity.CredencialMotivoId > 0)
            {
                motivo = CredencialMotivo.BuscarPelaChave(entity.CredencialMotivoId);

                if (entity.CredencialStatusId == 1) 
                {
                    entity.Baixa = (DateTime?)null; 
                }
                else if (entity.CredencialStatusId == 2 && (!entity.DevolucaoEntregaBo) && motivo.Impeditivo)
                {
                    entity.Baixa = (DateTime?)null; 
                }
                else if (entity.CredencialStatusId == 2 && !motivo.Impeditivo)
                {
                    entity.Baixa = DateTime.Today.Date; 
                }
                else if (entity.CredencialStatusId == 2 && (entity.DevolucaoEntregaBo) && motivo.Impeditivo)
                {
                    entity.Baixa = DateTime.Today.Date; 
                }
            }

            _repositorio.Alterar(entity);
            //////comentado pois a busca não está retornando resultador e anulando a entity
            //var n1 = BuscarPelaChave(entity.ColaboradorCredencialId);
            
            //if (n1 == null) return;
            //ObterStatusCredencial(n1);
            //n1.CardHolderGuid = entity.CardHolderGuid;
            //n1.CredencialGuid = entity.CredencialGuid;
            //_repositorio.Alterar(n1);
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
            

            entity.DataStatus = entity.Ativa != entity2.Ativa ? DateTime.Today.Date : entity2.DataStatus;
            entity.Ativa = entity2.Ativa; //Atulizar dados para serem exibidas na tela

            CredencialMotivo motivo = new CredencialMotivo();
            if (entity.CredencialMotivoId > 0)
            {
                motivo = CredencialMotivo.BuscarPelaChave(entity.CredencialMotivoId);

                if (entity.CredencialStatusId == 1)
                {
                    entity.Baixa = (DateTime?)null;
                }
                else if (entity.CredencialStatusId == 2 && (!entity.DevolucaoEntregaBo) && motivo.Impeditivo)
                {
                    entity.Baixa = (DateTime?)null;
                } 
                else if (entity.CredencialStatusId == 2 && !motivo.Impeditivo) 
                {
                    entity.Baixa = DateTime.Today.Date;
                } 
                else if (entity.CredencialStatusId == 2 && (entity.DevolucaoEntregaBo) && motivo.Impeditivo)
                {
                    entity.Baixa = DateTime.Today.Date;
                }
            }

            //Alterar dados no sub-sistema de credenciamento
            //A data da baixa está em função do status do titular do cartao e sua credencial
            entity2.DataStatus = entity.DataStatus;
            entity2.Baixa = entity.Baixa;
            //Alterar (entity2);

            //entity = BuscarPelaChave(entity.ColaboradorCredencialId);
            entity = BuscarCredencialPelaChave(entity.ColaboradorCredencialId);
            //Alterar status de um titual do cartao
            var titularCartao = CardHolderEntity(entity);
            ////Alterar status de um titual do cartao
            //var titularCartao = CardHolderEntity (entity); 
            titularCartao.Ativo = entity2.Ativa;
            //titularCartao = CardHolderEntity(entity);
            //Alterar o status do cartao do titular, se houver
            if (string.IsNullOrWhiteSpace (titularCartao.IdentificadorCardHolderGuid)
                & string.IsNullOrWhiteSpace (titularCartao.IdentificadorCredencialGuid)) return;

            //Alterar status do cartao
            geradorCredencialService.AlterarStatusCardHolder (titularCartao);
            //Sistema somente gerar credencial se o tipo de autenticação permitir

            //Alterar credencial
            geradorCredencialService.AlterarStatusCredencial (titularCartao);

            ////Alterar status do cartao
            geradorCredencialService.AlterarStatusCardHolder(titularCartao);
            ////Sistema somente gerar credencial se o tipo de autenticação permitir

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
            //if (!string.IsNullOrWhiteSpace(entity.CardHolderGuid) & !string.IsNullOrWhiteSpace(entity.CredencialGuid)) return;

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
            //n1.Identificacao1 = titularCartao.Identificacao1;
            //n1.Identificacao2 = titularCartao.Identificacao2;
            n1.CredencialGuid = titularCartao.IdentificadorCredencialGuid;
            n1.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
            //n1.TecnologiaCredencialId = entity.TecnologiaCredencialId;
            //n1.FormatoCredencialId = entity.FormatoCredencialId;
            //n1.Fc = entity.Fc;
            n1.NumeroCredencial = entity.NumeroCredencial;

            //Alterar(n1);
        }
        /// <summary>
        ///     Remove Regra de acesso do cardHolder sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService">Sub sistema de geração de credenciais de cartão de um titular</param>
        /// <param name="colaboradorService">Colaborador service</param>
        /// <param name="entity"></param>
        public void RemoverRegrasCardHolder(ICredencialService geradorCredencialService, IColaboradorService colaboradorService, ColaboradoresCredenciaisView entity)
        {
            try
            {
                if (geradorCredencialService == null) throw new ArgumentNullException(nameof(geradorCredencialService));
                if (entity == null) throw new ArgumentNullException(nameof(entity));


                var titularCartao = CardHolderEntity(entity);
                geradorCredencialService.RemoverRegrasCardHolder(titularCartao);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        /// <summary>
        ///     Remove Regra de acesso do cardHolder sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService">Sub sistema de geração de credenciais de cartão de um titular</param>
        /// <param name="colaboradorService">Colaborador service</param>
        /// <param name="entity"></param>
        public void RemoverCredencial(ICredencialService geradorCredencialService, IColaboradorService colaboradorService, ColaboradoresCredenciaisView entity)
        {
            try
            {
                if (geradorCredencialService == null) throw new ArgumentNullException(nameof(geradorCredencialService));
                if (entity == null) throw new ArgumentNullException(nameof(entity));


                var titularCartao = CardHolderEntity(entity);
                geradorCredencialService.RemoverCredencial(titularCartao);
                geradorCredencialService.AlterarStatusCardHolder(titularCartao);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        /// <summary>
        ///     Remove Regra de acesso do cardHolder sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService">Sub sistema de geração de credenciais de cartão de um titular</param>
        /// <param name="colaboradorService">Colaborador service</param>
        /// <param name="entity"></param>
        public void RemoverRegrasCardHolder(ICredencialService geradorCredencialService, CardHolderEntity entity)
        {
            try
            {                
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                
                geradorCredencialService.RemoverRegrasCardHolder(entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        public void ExisteCardHolder(ICredencialService geradorCredencialService, CardHolderEntity entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                geradorCredencialService.ExisteCardHolder(entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        /// <summary>
        ///     Remove uma Credential (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService">Sub sistema de geração de credenciais de cartão de um titular</param>
        /// <param name="colaboradorService">Colaborador service</param>
        /// <param name="entity"></param>
        public void RemoverCredencial(ICredencialService geradorCredencialService, CardHolderEntity entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                geradorCredencialService.RemoverCredencial(entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
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

        /// <summary>
        ///    Listar Colaboradores credenciais - destruídas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialDestruidasView(FiltroReportColaboradoresCredenciais entity)
        {
            return _repositorio.ListarColaboradorCredencialDestruidasView(entity);
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - extraviadas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialExtraviadasView(FiltroReportColaboradoresCredenciais entity)
        {
            return _repositorio.ListarColaboradorCredencialExtraviadasView(entity);
        }

        public void DisparaAlarme(ICredencialService geradorCredencialService, CardHolderEntity entity)
        {
            throw new NotImplementedException();
        }

        public void RetornarGrupos()
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}