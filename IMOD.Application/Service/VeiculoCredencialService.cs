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

        #region  Propriedades

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

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        public IPendenciaService Pendencia
        {
            get { return new PendenciaService(); }
        }

        #endregion

        #region  Metodos

        private void ObterStatusCredencial(VeiculoCredencial entity)
        {
            try
            {
                var status = CredencialStatus.BuscarPelaChave(entity.CredencialStatusId);
                entity.Ativa = status.Codigo == "1"; //Set status da credencial
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        ///     Set dados de um titular de cartao
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static CardHolderEntity CardHolderEntity(VeiculosCredenciaisView entity)
        {
            //Autor: Valnei Filho
            //Data:13/03/19
            //Wrk:Adicionar um dia a credencial
            DateTime dataValidade = entity.Validade == null ? DateTime.Today.Date : (DateTime)entity.Validade;

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
                Validade = dataValidade.AddDays(1),
                NumeroCredencial = entity.NumeroCredencial,
                IdentificadorLayoutCrachaGuid = entity.LayoutCrachaGuid
            };
            return titularCartao;
        }

        public VeiculosCredenciaisView BuscarCredencialPelaChave(int veiculoCredencialId)
        {
            try
            {
                return _repositorio.BuscarCredencialPelaChave(veiculoCredencialId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        public VeiculoCredencial ObterCredencialPeloNumeroCredencial(string numCredencial)
        {
            try
            {
                return _repositorio.ObterCredencialPeloNumeroCredencial(numCredencial);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Obter credencial
        /// </summary>
        /// <param name="veiculoCredencialId"></param>
        /// <returns></returns>
        public AutorizacaoView ObterCredencialView(int veiculoCredencialId)
        {
            try
            {
                return _repositorio.ObterCredencialView(veiculoCredencialId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
        /// <param name="equiapmentoVeiculoId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        public DateTime? ObterDataValidadeCredencial(int tipoCredencialId, int equiapmentoVeiculoId, string numContrato, ITipoCredencialRepositorio credencialRepositorio)
        {
            try
            {
                return _repositorio.ObterDataValidadeCredencial(tipoCredencialId, equiapmentoVeiculoId, numContrato, credencialRepositorio);
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
            try
            {
                ObterStatusCredencial(entity);
                _repositorio.Criar(entity, colaboradorId, TipoCredencial);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Alterar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="veiculoId"></param>
        /// <param name="credencialRepositorio"></param>
        public void Alterar(VeiculoCredencial entity, int veiculoId, ITipoCredencialRepositorio credencialRepositorio)
        {
            try
            {
                _repositorio.Alterar(entity, veiculoId, credencialRepositorio);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Verificar se um número credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        public bool ExisteNumeroCredencial(string numCredencial)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(numCredencial)) return false;

                var doc = numCredencial.RetirarCaracteresEspeciais();
                var n1 = ObterCredencialPeloNumeroCredencial(doc);
                return n1 != null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Criar uma pendência impeditiva caso o motivo do credenciamento possua natureza impeditiva
        /// </summary>
        /// <param name="entity"></param>
        public void CriarPendenciaImpeditiva(VeiculosCredenciaisView entity)
        {
            try
            {
                //Criar um pendenci impeditiva ao constatar o motivo da credencial
                var pendImp = CredencialMotivo.BuscarPelaChave(entity.CredencialMotivoId);
                if (pendImp == null) throw new InvalidOperationException("Não foi possível obter a entidade credencial motivo");
                var impeditivo = pendImp.Impeditivo & !entity.DevolucaoEntregaBo;
                if (!impeditivo) return;
                //Criar uma pendencia impeditiva,caso sua natureza seja impeditiva

                #region Criar Pendência  

                var pendencia = new Pendencia();
                pendencia.EmpresaId = entity.EmpresaId;
                var motivo = CredencialMotivo.BuscarPelaChave(entity.CredencialMotivoId);
                pendencia.Descricao = $"Em {DateTime.Now}, uma pendência impeditiva foi criada pelo sistema por uma autorização ter sido {motivo.Descricao}." +
                                      $"\r\nO Equipamento/Veículo de Placa/Identificador {entity.PlacaIdentificador}" +
                                      "\r\nDeseja-se que tais pendências sejam solucionadas." +
                                      $"\r\nAutor {UsuarioLogado.Nome} {UsuarioLogado.Email}";
                pendencia.Impeditivo = true;
                //--------------------------
                pendencia.CodPendencia = 21;
                Pendencia.CriarPendenciaSistema(pendencia);

                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoCredencial entity)
        {
            try
            {
                CredencialMotivo motivo = new CredencialMotivo();
                if (entity.CredencialMotivoId > 0)
                {
                    motivo = CredencialMotivo.BuscarPelaChave((int)entity.CredencialMotivoId);

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

                ObterStatusCredencial(entity);
                _repositorio.Alterar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoCredencial BuscarPelaChave(int id)
        {
            try
            {
                return _repositorio.BuscarPelaChave(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoCredencial entity)
        {
            try
            {
                ObterStatusCredencial(entity);
                _repositorio.Criar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoCredencial> Listar(params object[] objects)
        {
            try
            {
                return _repositorio.Listar(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Listar Veículos e suas credenciais
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ICollection<VeiculosCredenciaisView> ListarView(params object[] objects)
        {
            try
            {
                return _repositorio.ListarView(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Alterar um titular de cartão no  sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService"></param>
        /// <param name="entity"></param>
        /// <param name="entity2"></param>
        public void AlterarStatusTitularCartao(ICredencialService geradorCredencialService, VeiculosCredenciaisView entity, VeiculoCredencial entity2)
        {
            try
            {
                if (geradorCredencialService == null) throw new ArgumentNullException(nameof(geradorCredencialService));
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                if (entity2 == null) throw new ArgumentNullException(nameof(entity2));
                ObterStatusCredencial(entity2);
                //Alterar status de um titual do cartao
                var titularCartao = CardHolderEntity(entity);
                titularCartao.Ativo = entity2.Ativa;

                entity.DataStatus = entity.Ativa != entity2.Ativa ? DateTime.Today.Date : entity2.DataStatus;
                entity.Ativa = entity2.Ativa; //Atulizar dados para serem exibidas na tela

                CredencialMotivo motivo = new CredencialMotivo();
                if (entity.CredencialMotivoId > 0)
                {
                    motivo = CredencialMotivo.BuscarPelaChave((int)entity.CredencialMotivoId);

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
                entity2.Baixa = entity.Baixa;
                Alterar(entity2);
                //Alterar o status do cartao do titular, se houver
                if (string.IsNullOrWhiteSpace(titularCartao.IdentificadorCardHolderGuid)
                    & string.IsNullOrWhiteSpace(titularCartao.IdentificadorCredencialGuid)) return;

                //Alterar status do cartao
                geradorCredencialService.AlterarStatusCardHolder(titularCartao);
                //Alterar credencial
                geradorCredencialService.AlterarStatusCredencial(titularCartao);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Criar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        public void Criar(VeiculoCredencial entity, int colaboradorId)
        {
            try
            {
                Criar(entity, colaboradorId, TipoCredencial);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Alterar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        public void Alterar(VeiculoCredencial entity, int colaboradorId)
        {
            try
            {
                Alterar(entity, colaboradorId, TipoCredencial);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Criar um titular de cartão no sub-sistema de credenciamento (Genetec)
        /// </summary>
        /// <param name="geradorCredencialService"> Sub sistema de geração de credenciais de cartão de um titular</param>
        /// <param name="entity"></param>
        public void CriarTitularCartao(ICredencialService geradorCredencialService, IVeiculoService veiculoService, VeiculosCredenciaisView entity)
        {
            try
            {
                if (geradorCredencialService == null) throw new ArgumentNullException(nameof(geradorCredencialService));
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                //Somente é permitido criar uma única vez o titular do cartão...
                //Os numeros GUIDs são indicação  de que já houve criação de credenciais no sub-sistema de credenciamento
                if (!string.IsNullOrWhiteSpace(entity.CardHolderGuid) & !string.IsNullOrWhiteSpace(entity.CredencialGuid)) return;
                var titularCartao = CardHolderEntity(entity);




                #region Setar o valor CardHolder GUID ao colaborador

                //Buscar dados do colaborador
                var co1 = veiculoService.Empresa.BuscarPelaChave(entity.VeiculoCredencialId);
                if (co1 == null) throw new InvalidOperationException("Não foi possive obter um Veiculo/Equipamento");

                if (string.IsNullOrWhiteSpace(co1.CardHolderGuid))
                {
                    //Gerar titular do cartão no sub-sistema de credenciamento (Genetec)
                    geradorCredencialService.CriarCardHolder(titularCartao);
                    co1.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
                    veiculoService.Empresa.Alterar(co1);
                }

                titularCartao.IdentificadorCardHolderGuid = co1.CardHolderGuid;


                #endregion







                //Gerar titular do cartão no sub-sistema de credenciamento (Genetec)
                //geradorCredencialService.CriarCardHolder (titularCartao);
                //Gerar Credencial do titular do cartão no sub-sistema de credenciamento (Genetec)
                geradorCredencialService.CriarCredencial(titularCartao);
                //Atualizar dados do identificador GUID
                entity.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
                entity.CredencialGuid = titularCartao.IdentificadorCredencialGuid;
                var n1 = BuscarPelaChave(entity.VeiculoCredencialId);
                n1.CardHolderGuid = titularCartao.IdentificadorCardHolderGuid;
                n1.CredencialGuid = titularCartao.IdentificadorCredencialGuid;

                Alterar(n1);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(VeiculoCredencial entity)
        {
            try
            {
                _repositorio.Remover(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ICollection<AutorizacaoView> ListarAutorizacaoView(params object[] objects)
        {
            try
            {
                return _repositorio.ListarAutorizacaoView(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<VeiculosCredenciaisView> ListarVeiculoCredencialConcedidasView(FiltroReportVeiculoCredencial entity)
        {
            try
            {
                return _repositorio.ListarVeiculoCredencialConcedidasView(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<VeiculosCredenciaisView> ListarVeiculoCredencialViaAdicionaisView(FiltroReportVeiculoCredencial entity)
        {
            try
            {
                return _repositorio.ListarVeiculoCredencialViaAdicionaisView(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<VeiculosCredenciaisView> ListarVeiculoCredencialInvalidasView(FiltroReportVeiculoCredencial entity)
        {
            try
            {
                return _repositorio.ListarVeiculoCredencialInvalidasView(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<VeiculosCredenciaisView> ListarVeiculoCredencialImpressoesView(FiltroReportVeiculoCredencial entity)
        {
            try
            {
                return _repositorio.ListarVeiculoCredencialImpressoesView(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<VeiculosCredenciaisView> ListarVeiculoCredencialPermanentePorAreaView(FiltroReportVeiculoCredencial entity)
        {
            try
            {
                return _repositorio.ListarVeiculoCredencialPermanentePorAreaView(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<VeiculosCredenciaisView> ListarVeiculoCredencialDestruidasView(FiltroReportVeiculoCredencial entity)
        {
            try
            {
                return _repositorio.ListarVeiculoCredencialDestruidasView(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<VeiculosCredenciaisView> ListarVeiculoCredencialExtraviadasView(FiltroReportVeiculoCredencial entity)
        {
            try
            {
                return _repositorio.ListarVeiculoCredencialExtraviadasView(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}