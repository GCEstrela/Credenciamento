// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Modulo;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Infra.Servicos;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;
using IMOD.CredenciamentoDeskTop.Enums;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using IMOD.CredenciamentoDeskTop.Funcoes;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradoresCredenciaisViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IColaboradorEmpresaService _colaboradorEmpresaService = new ColaboradorEmpresaService();

        private readonly IEmpresaContratosService _contratosService = new EmpresaContratoService();
        private readonly IColaboradorCredencialService _service = new ColaboradorCredencialService();
        private ColaboradorView _colaboradorView;
        /// <summary>
        ///     Lista de todos os contratos disponíveis
        /// </summary>
        private readonly List<ColaboradorEmpresa> _todosContratosEmpresas = new List<ColaboradorEmpresa>();

        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();

        private readonly IEmpresaService serviceEmpresa = new EmpresaService();

        private ConfiguraSistema _configuraSistema;

        private Boolean verificarcredencialAtiva = false; // Testa se é prara verificar a exectencia de credencial ativa para o colaborador.
        private int _count;
        public int? _viaAdicional;
        private List<CredencialMotivo> _credencialMotivo;
        private bool _coleteEnabled;

        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;

        private ColaboradorViewModel _viewModelParent;

        #region  Propriedades

        /// <summary>
        ///     Habilitar Controles
        /// </summary>
        public bool Habilitar { get; set; }
        /// <summary>
        ///     Habilitar Controles de Vias Adicionais
        /// </summary>
        public string HabilitarVias { get; set; }
        /// <summary>
        /// Permite criar uma credencial, se não existir.
        /// </summary>
        public bool HabilitarOpcoesCredencial { get; set; }
        public string ExcluirVisivel { get; set; }
        public bool AlterarDataValidade { get; set; }
        public Boolean ColeteEnabled
        {
            get
            {
                if (_coleteEnabled)
                {
                    return Habilitar;
                }

                return _coleteEnabled;
            }
            set { _coleteEnabled = value; }
        }
        public CredencialStatus StatusCredencial { get; set; }
        public List<CredencialStatus> CredencialStatus { get; set; }

        public List<CredencialMotivo> CredenciaisMotivo
        {
            get
            {
                if (StatusCredencial == null) return _credencialMotivo;
                var lst = _credencialMotivo.Where(n => n.CodigoStatus == StatusCredencial.Codigo);
                return lst.ToList();
            }
            set { _credencialMotivo = value; }
        }

        public List<FormatoCredencial> FormatoCredencial { get; set; }
        public List<TipoCredencial> TipoCredencial { get; set; }
        public List<EmpresaLayoutCracha> EmpresaLayoutCracha { get; set; }
        public List<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public ObservableCollection<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; }
        public ColaboradorEmpresa ColaboradorEmpresa { get; set; }
        public List<AreaAcesso> ColaboradorPrivilegio { get; set; }
        public ColaboradoresCredenciaisView Entity { get; set; }
        public List<Curso> Cursos { get; private set; }
        public List<CredencialMotivo> ColaboradorMotivoViaAdcional { get; set; }

        /// <summary>
        ///     Mensagem de alerta
        /// </summary>
        public string MensagemAlerta { get; private set; }
        /// <summary>
        ///     Mensagem de alerta
        /// </summary>
        public string ContentImprimir { get; set; }
        /// <summary>
        ///     Habilitar impressao de credencial com base no status da credencial
        ///     e condição de pendencia impeditiva
        /// </summary>
        public bool HabilitaImpressao { get; set; }

        public ObservableCollection<ColaboradoresCredenciaisView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; set; } = true;

        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }
        ///// <summary>
        /////     Habilita Concatenação da Combo Empresa e Contratos
        ///// </summary>
        public bool IsEnableComboContrato { get; set; } = true;
        /// <summary>
        ///     Habilita Combo de Contratos
        /// </summary>
        public bool IsEnableColete { get; set; } = true;
        public int WidthColete { get; set; } = 48;
        public int indTecnologiaCredencial { get; set; } = 1;

        #endregion

        public ColaboradoresCredenciaisViewModel()
        {
            try
            {
                AlterarDataValidade = UsuarioLogado.Adm;
                if (!UsuarioLogado.Adm)
                {
                    ExcluirVisivel = "Collapsed";
                }
                else
                {
                    ExcluirVisivel = "Visible";
                }


                HabilitarOpcoesCredencial = true;
                ItensDePesquisaConfigura();
                ListarDadosAuxiliares();
                Comportamento = new ComportamentoBasico(false, true, false, false, false);
                EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
                Comportamento.SalvarAdicao += OnSalvarAdicao;
                Comportamento.SalvarEdicao += OnSalvarEdicao;
                Comportamento.Remover += OnRemover;
                Comportamento.Cancelar += OnCancelar;
                PropertyChanged += OnEntityChanged;
                SelectListViewIndex = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region  Metodos

        private void AtualizarMensagem(ColaboradoresCredenciaisView entity)
        {
            MensagemAlerta = string.Empty;
            if (entity == null) return;
            if (entity.ColaboradorCredencialId <= 0) return;

            Entity.NumeroColete = Entity.Colete.Length > 3 && !String.IsNullOrEmpty(Entity.Colete) ? Entity.Colete.Substring(3) : String.Empty;
            //Habilita a possibilidade de gerar uma nova ou alterar credencial
            HabilitarOpcoesCredencial = true;
            if (Entity.CredencialGuid != null)
            {
                HabilitarOpcoesCredencial = false;
                Habilitar = false;
            }
            //////////////////////////////////////////////////////////////////
            #region Habilitar botão de impressao e mensagem ao usuario
            bool impeditivaEmpresa = false;
            string mensagemPendencias = "";
            var pendenciaImpeditivaEmpresa = serviceEmpresa.Pendencia.ListarPorEmpresa(entity.EmpresaId).Where(n => n.Impeditivo == true && n.Ativo == true && n.DataLimite <= DateTime.Now).ToList();
            if (pendenciaImpeditivaEmpresa != null && pendenciaImpeditivaEmpresa.Count > 0)
            {
                impeditivaEmpresa = true;
                ContentImprimir = impeditivaEmpresa ? "Visualizar Credencial " : "Imprimir Credencial";
                foreach (Pendencia elemento in pendenciaImpeditivaEmpresa)
                {
                    mensagemPendencias = mensagemPendencias + elemento.DescricaoPendencia.ToString() + " - ";
                }
                if (mensagemPendencias.Length > 0)
                    mensagemPendencias = mensagemPendencias.Substring(0, mensagemPendencias.Length - 3);
            }

            bool impeditivaColaborador = false;
            string mensagemPendenciasColaborador = "";
            var pendenciaImpeditivaColaborador = serviceEmpresa.Pendencia.ListarPorColaborador(entity.ColaboradorId).Where(n => n.Impeditivo == true && n.Ativo == true && n.DataLimite <= DateTime.Now).ToList();
            if (pendenciaImpeditivaColaborador != null && pendenciaImpeditivaColaborador.Count > 0)
            {
                impeditivaColaborador = true;
                ContentImprimir = impeditivaColaborador ? "Visualizar Credencial " : "Imprimir Credencial";
                foreach (Pendencia elemento in pendenciaImpeditivaColaborador)
                {
                    mensagemPendenciasColaborador = mensagemPendenciasColaborador + elemento.DescricaoPendencia.ToString() + " - ";
                }
                if (mensagemPendenciasColaborador.Length > 0)
                {
                    mensagemPendenciasColaborador = mensagemPendenciasColaborador.Substring(0, mensagemPendenciasColaborador.Length - 3);

                }

            }

            if (!UsuarioLogado.Adm)   //Esta alteração esta sendo feita para permitir que o Usuario ADM posso continuar com a data alterada
            {
                HabilitaImpressao = entity.Ativa && !entity.PendenciaImpeditiva && !entity.Impressa && (entity.ColaboradorCredencialId > 0) && entity.Validade >= DateTime.Now.Date && (pendenciaImpeditivaEmpresa.Count <= 0) && (pendenciaImpeditivaColaborador.Count <= 0);
            }
            else
            {
                HabilitaImpressao = entity.Ativa && !entity.PendenciaImpeditiva && !entity.Impressa && (entity.ColaboradorCredencialId > 0) && (pendenciaImpeditivaEmpresa.Count <= 0) && (pendenciaImpeditivaColaborador.Count <= 0);
            }



            //Verificar se a empresa esta impedida 
            var mensagem1 = "";
            var mensagem2 = "";
            var mensagem3 = "";
            var mensagem4 = "";
            var mensagem5 = "";
            var n1 = _service.BuscarCredencialPelaChave(entity.ColaboradorCredencialId);
            if (n1 != null)
            {
                if (!UsuarioLogado.Adm)    //Esta alteração esta sendo feita para permitir que o Usuario ADM posso continuar com a data alterada
                {
                    HabilitaImpressao = n1.Ativa && !n1.PendenciaImpeditiva && !n1.Impressa && (entity.ColaboradorCredencialId > 0) && entity.Validade >= DateTime.Now.Date && (pendenciaImpeditivaEmpresa.Count <= 0) && (pendenciaImpeditivaColaborador.Count <= 0);
                }
                else
                {
                    HabilitaImpressao = n1.Ativa && !n1.PendenciaImpeditiva && !n1.Impressa && (entity.ColaboradorCredencialId > 0) && (pendenciaImpeditivaEmpresa.Count <= 0) && (pendenciaImpeditivaColaborador.Count <= 0);
                }

                mensagem1 = !n1.Ativa ? "Credencial Inativa" : string.Empty;
                //ContentImprimir = !n1.Ativa ? "Visualizar Credencial " : "Imprimir Credencial";
                //mensagem2 = n1.PendenciaImpeditiva ? "Pendência Impeditiva (consultar dados da empresa na aba Geral)" : string.Empty;
                //mensagem2 = n1.PendenciaImpeditiva ? "Pendência(s) Impeditiva(s) dados da empresa aba(s): " + mensagemPendencias : string.Empty;
                mensagem2 = n1.PendenciaImpeditiva ? " Pendência(s) para a EMPRESA: " + mensagemPendencias : string.Empty;
                mensagem3 = n1.Impressa ? "Credencial já foi emitida " : string.Empty;
                mensagem4 = (entity.Validade < DateTime.Now.Date) ? "Credencial vencida. " : string.Empty;
                //ContentImprimir = (entity.Validade < DateTime.Now.Date) ? "Visualizar Credencial " : "Imprimir Credencial";

                //if (mensagemPendenciasColaborador.Length > 0)
                //mensagem5 = n1.PendenciaImpeditiva ? " Pendência(s) para a COLABORADOR: " + mensagemPendenciasColaborador : string.Empty;
                mensagem5 = impeditivaColaborador ? " Pendência(s) para a COLABORADOR: " + mensagemPendenciasColaborador : string.Empty;
                Entity.DevolucaoEntregaBo = n1.DevolucaoEntregaBo;
            }



            CollectionViewSource.GetDefaultView(EntityObserver).Refresh();
            //Exibir mensagem de impressao de credencial, esta tem prioridade sobre as demais regras            
            //if (n1.Impressa) return; 

            if (!string.IsNullOrEmpty(mensagem1 + mensagem2 + mensagem3 + mensagem4 + mensagem5))
            {
                //MensagemAlerta = $"A credencial não poderá ser impressa pelo seguinte motivo: ";
                MensagemAlerta = $"A credencial não pode ser impressa. Motivo(s): ";
                if (!string.IsNullOrEmpty(mensagem1))
                {
                    MensagemAlerta += mensagem1;
                }
                else if (!string.IsNullOrEmpty(mensagem2))
                {
                    MensagemAlerta += mensagem2;
                }
                else if (!string.IsNullOrEmpty(mensagem3))
                {
                    MensagemAlerta += mensagem3;
                }
                else if (!string.IsNullOrEmpty(mensagem4))
                {
                    MensagemAlerta += mensagem4;
                }
                else if (!string.IsNullOrEmpty(mensagem5))
                {
                    MensagemAlerta += mensagem5;
                }
            }

            //================================================================================
            #endregion
        }

        #region Regras de Negócio

        public bool ExisteNumeroCredencial()
        {
            if (Entity == null) return false;
            var numCredencial = Entity.NumeroCredencial.RetirarCaracteresEspeciais();

            //Verificar dados antes de salvar uma criação
            if (_prepareCriarCommandAcionado)
                if (_service.ExisteNumeroCredencial(numCredencial))
                    return true;
            //Verificar dados antes de salvar uma alteraçao
            if (!_prepareAlterarCommandAcionado) return false;
            var n1 = _service.BuscarPelaChave(Entity.ColaboradorCredencialId);
            if (n1 == null) return false;
            //Comparar o CNPJ antes e o depois
            //Verificar se há cnpj exisitente
            return string.Compare(n1.NumeroCredencial.RetirarCaracteresEspeciais(),
                       numCredencial, StringComparison.Ordinal) != 0 && _service.ExisteNumeroCredencial(numCredencial);
        }
        //
        public ColaboradorCredencial ExisteNumeroColete()
        {
            try
            {
                var numColete = "";
                if (Entity == null) return null;
                if (Entity.EmpresaSigla == null || Entity.EmpresaSigla == "")
                {
                    Entity.EmpresaSigla = "---";
                }
                numColete = Entity.EmpresaSigla + Entity.NumeroColete;

                //Verificar dados antes de salvar uma criação
                var statusID = _auxiliaresService.CredencialStatusService.Listar(null, "ATIVA").FirstOrDefault();
                if (Entity.CredencialStatusId == statusID.CredencialStatusId)
                {
                    return _service.ExisteNumeroColete(_colaboradorView.ColaboradorId, numColete);
                }
                return null;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                return null;
            }
        }
        #endregion

        private void ListarDadosAuxiliares()
        {
            try
            {
                var lst0 = _auxiliaresService.CredencialStatusService.Listar();
                CredencialStatus = new List<CredencialStatus>();
                CredencialStatus.AddRange(lst0.OrderBy(n => n.Descricao));

                var lst2 = _auxiliaresService.FormatoCredencialService.Listar();
                FormatoCredencial = new List<FormatoCredencial>();
                FormatoCredencial.AddRange(lst2.OrderBy(n => n.FormatoCredencialId));

                var lst3 = _auxiliaresService.TipoCredencialService.Listar();
                TipoCredencial = new List<TipoCredencial>();
                TipoCredencial.AddRange(lst3.OrderBy(n => n.TipoCredencialId));

                var lst5 = _auxiliaresService.TecnologiaCredencialService.Listar();
                TecnologiasCredenciais = new List<TecnologiaCredencial>();
                TecnologiasCredenciais.AddRange(lst5.OrderBy(n => n.TecnologiaCredencialId));

                ColaboradoresEmpresas = new ObservableCollection<ColaboradorEmpresa>();

                var lst7 = _auxiliaresService.AreaAcessoService.Listar();
                ColaboradorPrivilegio = new List<AreaAcesso>();
                ColaboradorPrivilegio.AddRange(lst7.OrderBy(n => n.Identificacao));

                var lst8 = _auxiliaresService.CredencialMotivoService.Listar();
                _credencialMotivo = new List<CredencialMotivo>();
                //_credencialMotivo = new List<CredencialMotivo>(lst8.OrderBy(n => n.Descricao));
                _credencialMotivo.AddRange(lst8.OrderBy(n => n.Descricao));

                var lst9 = _auxiliaresService.CredencialMotivoService.Listar(null, null, null, true);
                ColaboradorMotivoViaAdcional = new List<CredencialMotivo>();
                ColaboradorMotivoViaAdcional.AddRange(lst9.OrderBy(n => n.Descricao));

                _configuraSistema = ObterConfiguracao();
                if (_configuraSistema.Contrato) //Se contrato for automático for true a combo sera removida do formulário
                {
                    IsEnableComboContrato = false;
                }
                if (!_configuraSistema.Colete) //Se Cole não for automático false
                {
                    IsEnableColete = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

        }


        /// <summary>
        /// </summary>
        /// <param name="empresaId"></param>
        public void ListarCracha(int empresaId, int codigoTipoValidade)
        {
            try
            {
                EmpresaLayoutCracha = new List<EmpresaLayoutCracha>();
                var service = new EmpresaLayoutCrachaService();
                var list1 = service.ListarLayoutCrachaView(empresaId, null, null, null, 1, codigoTipoValidade);
                var list2 = Mapper.Map<List<EmpresaLayoutCracha>>(list1);
                EmpresaLayoutCracha = list2;

                //_todosContratosEmpresas.ForEach(n => { ColaboradoresEmpresas.Add(n); });
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Entity")
            {
                Comportamento.IsEnableEditar = Entity != null;
                Comportamento.isEnableRemover = Entity != null;
                AtualizarMensagem(Entity);
            }
        }

        public void ObterValidade()
        {
            if (!_prepareCriarCommandAcionado) return;
            if (Entity == null) return;
            var empContratoId = ColaboradorEmpresa.EmpresaContratoId;
            var contrato = _contratosService.BuscarPelaChave(empContratoId);
            var data = _service.ObterDataValidadeCredencial(Entity.TipoCredencialId,
                _colaboradorView.ColaboradorId, contrato.NumeroContrato, _service.TipoCredencial);


            DateTime dataEncontrada;
            TimeSpan diferenca = Convert.ToDateTime(data) - DateTime.Now.Date;
            int credencialDias = int.Parse(diferenca.Days.ToString());
            if (credencialDias > 730)
            {
                if (Entity.Emissao == null)
                {
                    dataEncontrada = DateTime.Now.AddDays(730);
                }
                else
                {
                    DateTime dataEmissao = (DateTime)Entity.Emissao;
                    dataEncontrada = dataEmissao.AddDays(730);
                }
                if (Entity.Validade > dataEncontrada)
                    Entity.Validade = dataEncontrada;

                Entity.Validade = dataEncontrada;
                OnPropertyChanged("Entity");
            }
            else
            {
                //Comentado por Renato Maximo em 28-06-2019
                Entity.Validade = credencialDias > (Constantes.Constantes.diasPorAno * 2) ? DateTime.Now.Date.AddDays((Constantes.Constantes.diasPorAno * 2)) : data;
                OnPropertyChanged("Entity");
            }


        }
        public void ObterValidadeAlteracao()
        {
            //if (!_prepareCriarCommandAcionado) return;
            if (Entity == null) return;
            var empContratoId = ColaboradorEmpresa.EmpresaContratoId;
            var contrato = _contratosService.BuscarPelaChave(empContratoId);
            var data = _service.ObterDataValidadeCredencial(Entity.TipoCredencialId,
                _colaboradorView.ColaboradorId, contrato.NumeroContrato, _service.TipoCredencial);


            DateTime dataEncontrada;
            TimeSpan diferenca;
            if (Entity.Emissao == null)
            {
                diferenca = Convert.ToDateTime(data) - DateTime.Now.Date;
            }
            else
            {
                diferenca = Convert.ToDateTime(data) - (DateTime)Entity.Emissao;
            }
            int credencialDias = int.Parse(diferenca.Days.ToString());
            if (credencialDias > 730)
            {
                if (Entity.Emissao == null)
                {
                    dataEncontrada = DateTime.Now.AddDays(730);
                }
                else
                {
                    DateTime dataEmissao = (DateTime)Entity.Emissao;
                    dataEncontrada = dataEmissao.AddDays(730);
                }
                if (Entity.Validade > dataEncontrada)
                    Entity.Validade = dataEncontrada;

                OnPropertyChanged("Entity");
            }
            else if (!UsuarioLogado.Adm)    //Esta alteração esta sendo feita para permitir que o Usuario ADM posso continuar com a data alterada
            {

                if (Entity.Validade > data)
                    //Entity.Validade = data;   //Esta alteração esta sendo feita para permitir que o Usuario ADM posso continuar com a data alterada

                    OnPropertyChanged("Entity");
            }

        }
        public void AtualizarDados(ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {

            verificarcredencialAtiva = false;
            EntityObserver.Clear();
            if (entity == null) return; // throw new ArgumentNullException(nameof(entity));
            _colaboradorView = entity;
            _viewModelParent = viewModelParent;
            ListarColaboradoresCredenciais(entity);
            //Listar dados de contratos
            if (_count == 0) ObterContratos();
            _count++;
            //ListarTodosContratos();
            OnAtualizarDadosContratosAtivos();
            MensagemAlerta = "";
            OnPropertyChanged("Entity");
            CollectionViewSource.GetDefaultView(EntityObserver).Refresh();

        }

        private void ListarColaboradoresCredenciais(ColaboradorView entity)
        {
            try
            {
                var list1 = _service.ListarView(null, null, null, null, entity.ColaboradorId).ToList();
                var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>>(list1.OrderByDescending(n => n.ColaboradorCredencialId));
                //Comportamento.IsEnableCriar = !list2.Any(c => c.Ativa);

                EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
                list2.ForEach(n => { EntityObserver.Add(n); });

                //Atualizar observer
                OnPropertyChanged("Entity");
                CollectionViewSource.GetDefaultView(EntityObserver).Refresh();
            }
            catch (Exception)
            {

            }
        }


        public void HabilitaCriar(ColaboradorEmpresa entity, ColaboradoresCredenciaisViewModel viewModel)
        {
            var list1 = _service.ListarView(null, entity.EmpresaNome, null, null, entity.ColaboradorId).ToList();
            if (list1.Any())
            {
                if (list1.Any(c => c.Ativa && c.ColaboradorEmpresaId == entity.ColaboradorEmpresaId && c.ColaboradorCredencialId != viewModel.Entity.ColaboradorCredencialId))
                {
                    viewModel.Entity.SetMessageErro("ColaboradorEmpresaId", "Já existe uma credencial ativa o vinculo selecionado. Não é possivel criar uma nova.");
                    //entity.ColaboradorEmpresaId = 0;                    
                }
                else
                {
                    viewModel.Entity.ClearMessageErro();
                }
            }
            else
            {
                viewModel.Entity.ClearMessageErro();
            }

        }


        /// <summary>
        ///     Listar dados de empresa e contratos
        /// </summary>
        private void ObterContratos()
        {
            try
            {
                var l2 = _colaboradorEmpresaService.Listar().ToList();
                _todosContratosEmpresas.Clear();
                _todosContratosEmpresas.AddRange(l2);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ListarTodosContratos()
        {
            ColaboradoresEmpresas.Clear();
            _todosContratosEmpresas.ForEach(n => { ColaboradoresEmpresas.Add(n); });
        }

        /// <summary>
        ///     Obter novos dados de contratos ativos
        /// </summary>
        private void OnAtualizarDadosContratosAtivos()
        {
            //Obter todos os contratos vinculados ao colaborador...
            ObterContratos();
            ListarTodosContratoPorColaboradorAtivos(_colaboradorView.ColaboradorId);
        }

        /// <summary>
        ///     Listar contratos ativos
        /// </summary>
        /// <param name="colaboradorId"></param>
        private void ListarTodosContratoPorColaboradorAtivos(int colaboradorId)
        {

            ColaboradoresEmpresas.Clear();
            var lst2 = _todosContratosEmpresas.Where(n => (n.ColaboradorId == colaboradorId) & n.Ativo).ToList();
            lst2.ForEach(n => { ColaboradoresEmpresas.Add(n); });


        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Nome"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "CPF"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }

        /// <summary>
        ///     Acionado antes de remover
        /// </summary>
        private void PrepareRemover()
        {
            HabilitarOpcoesCredencial = true;
            verificarcredencialAtiva = false;
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = false;
            Comportamento.PrepareRemover();
        }

        /// <summary>
        ///     Criar Dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                //Alterado por Máximo em 28-06-2019
                //ObterValidade();

                var n1 = Mapper.Map<ColaboradorCredencial>(Entity);
                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;
                n1.ColaboradorPrivilegio1Id = Entity.ColaboradorPrivilegio1Id;
                n1.ColaboradorPrivilegio2Id = Entity.ColaboradorPrivilegio2Id;
                n1.Identificacao1 = Entity.Identificacao1;
                n1.Identificacao2 = Entity.Identificacao2;
                n1.Validade = Entity.Validade.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);  //Sempre Add 23:59:59 horas à credencial nova.
                n1.CredencialmotivoViaAdicionalID = Entity.CredencialmotivoViaAdicionalID;
                n1.CredencialmotivoIDanterior = Entity.CredencialMotivoId;
                n1.listadeGrupos = Entity.listadeGrupos;
                n1.Usuario = UsuarioLogado.Nome;

                //_configuraSistema = ObterConfiguracao();
                n1.Regras = _configuraSistema.Regras;
                Entity.Regras = _configuraSistema.Regras;
                if (n1.Validade <= DateTime.Now)
                {
                    WpfHelp.Mbox("Data de Validade da Credencial é inferior à data atual.", MessageBoxIcon.Information);
                    MensagemAlerta = "";

                    Comportamento.PrepareCriarSegundaTentativa();
                    return;
                }

                if (_configuraSistema.Colete)
                {
                    Entity.NumeroColete = Convert.ToString(_colaboradorView.ColaboradorId);
                    n1.Colete = Entity.EmpresaSigla != "---" && !String.IsNullOrEmpty(Entity.EmpresaSigla) && !String.IsNullOrEmpty(Entity.NumeroColete) ? Entity.EmpresaSigla + Entity.NumeroColete : String.Empty;
                }
                else
                {
                    if (Entity.EmpresaSigla == null)
                    {
                        Entity.EmpresaSigla = "---";
                    }
                    n1.Colete = Entity.EmpresaSigla != "---" && !String.IsNullOrEmpty(Entity.EmpresaSigla) && !String.IsNullOrEmpty(Entity.NumeroColete) ? Entity.EmpresaSigla + Entity.NumeroColete : String.Empty;
                }

                
                //Criar registro no banco de dados e setar uma data de validade
                _prepareCriarCommandAcionado = false;
                _service.Criar(n1);
                IsEnableLstView = true;
                SelectListViewIndex = 0;

                #region Verificar se pode gerar CardHolder
                var tecCredencial = _auxiliaresService.TecnologiaCredencialService.BuscarPelaChave(Entity.TecnologiaCredencialId);
                if (tecCredencial.PodeGerarCardHolder)
                {
                    var entity = _service.BuscarCredencialPelaChave(n1.ColaboradorCredencialId);
                    entity.listadeGrupos = n1.listadeGrupos;
                    GerarCardHolder(n1.ColaboradorCredencialId, entity);
                }
                #endregion



                /// Atualiza Observer
                ListarColaboradoresCredenciais(_colaboradorView);

                //var list1 = _service.ListarView(null, null, null, null, _colaboradorView.ColaboradorId).ToList();                
                //var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>>(list1.OrderByDescending(n => n.ColaboradorCredencialId));
                //Comportamento.IsEnableCriar = !list2.Any(c => c.Ativa == true);
                //EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
                //list2.ForEach(n => { EntityObserver.Add(n); });

                MensagemAlerta = "";
                Entity = null;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        /// <summary>
        ///     Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }

        private void PrepararCancelar()
        {
            verificarcredencialAtiva = false;
            Comportamento.PrepareCancelar();
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            try
            {
                HabilitarOpcoesCredencial = true;
                verificarcredencialAtiva = true;
                Entity = new ColaboradoresCredenciaisView();

                //if (!HabilitaCriar(_colaboradorView.ColaboradorId)) throw new InvalidOperationException("Não é possivel criar credencial, pois existe uma credencial ativa para o colaborador no contrato.");

                Entity.NumeroColete = "";
                _configuraSistema = ObterConfiguracao();
                if (_configuraSistema.Colete)
                {
                    Entity.NumeroColete = Convert.ToString(_colaboradorView.ColaboradorId);
                }


                var statusCred = CredencialStatus.FirstOrDefault(n => n.Codigo == "1"); //Status ativa
                if (statusCred == null) throw new InvalidOperationException("O status da credencial é requerida.");
                StatusCredencial = statusCred;

                var tipoCredencial = TipoCredencial.FirstOrDefault(n => n.CredPermanente);
                if (tipoCredencial != null) Entity.TipoCredencialId = tipoCredencial.TipoCredencialId;

                Entity.Ativa = true;
                Comportamento.PrepareCriar();
                _prepareCriarCommandAcionado = true;
                _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
                IsEnableLstView = false;
                Habilitar = true;
                MensagemAlerta = "";

                //indTecnologiaCredencial = 1;
                //Entity.TecnologiaCredencialId = indTecnologiaCredencial;

                //Listar Colaboradores Ativos
                OnAtualizarDadosContratosAtivos();
                _viewModelParent.HabilitaControleTabControls(false, false, false, false, false, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        /// <summary>
        ///     Editar dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Entity == null) return;     //IdentificacaoDescricao = null
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                //Alterado por Maximo em 28/06/2019
                //ObterValidadeAlteracao();

                var n1 = Mapper.Map<ColaboradorCredencial>(Entity);
                n1.ColaboradorPrivilegio1Id = Entity.ColaboradorPrivilegio1Id;
                n1.ColaboradorPrivilegio2Id = Entity.ColaboradorPrivilegio2Id;
                n1.Usuario = UsuarioLogado.Nome;
                Entity.Usuario= UsuarioLogado.Nome;

                if (Entity.ColaboradorPrivilegio1Id != 0 && Entity.ColaboradorPrivilegio1Id != 41)
                {
                    var areaAcesso1 = _auxiliaresService.AreaAcessoService.Listar(Entity.ColaboradorPrivilegio1Id).FirstOrDefault();
                    Entity.Identificacao1 = areaAcesso1.Identificacao;
                    n1.Identificacao1 = areaAcesso1.Identificacao;
                }
                else
                {
                    n1.Identificacao1 = null;
                }

                if (Entity.ColaboradorPrivilegio2Id != 0 && Entity.ColaboradorPrivilegio2Id != 41)
                {
                    var areaAcesso2 = _auxiliaresService.AreaAcessoService.Listar(Entity.ColaboradorPrivilegio2Id).FirstOrDefault();
                    Entity.Identificacao2 = areaAcesso2.Identificacao;
                    n1.Identificacao2 = areaAcesso2.Identificacao;
                }
                else
                {
                    n1.Identificacao2 = null;
                }

                n1.NumeroCredencial = Entity.NumeroCredencial;
                n1.Validade = Entity.Validade.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59); //Sempre Add 23:59:59 horas à credencial nova.
                n1.CredencialVia = Entity.CredencialVia;
                n1.CredencialmotivoViaAdicionalID = Entity.CredencialmotivoViaAdicionalID;
                if (_configuraSistema.Colete)
                {
                    Entity.NumeroColete = Convert.ToString(_colaboradorView.ColaboradorId);

                    n1.Colete = Entity.EmpresaSigla != "---" && !String.IsNullOrEmpty(Entity.EmpresaSigla) && !String.IsNullOrEmpty(Entity.NumeroColete) ? Entity.EmpresaSigla + Entity.NumeroColete : String.Empty;
                }
                else
                {
                    if (Entity.EmpresaSigla == null)
                    {
                        Entity.EmpresaSigla = "---";
                    }
                    n1.Colete = Entity.EmpresaSigla != "---" && !String.IsNullOrEmpty(Entity.EmpresaSigla) && !String.IsNullOrEmpty(Entity.NumeroColete) ? Entity.EmpresaSigla + Entity.NumeroColete : String.Empty;
                }
                if (n1.Ativa)
                {
                    if (n1.Validade < DateTime.Now)
                    {
                        //WpfHelp.Mbox("Data de Validate não pode ser inferior à data do dia.", MessageBoxIcon.Information);
                        //return;
                    }
                }
                if(n1.CredencialStatusId == 2)
                {
                    n1.DataStatus = DateTime.Today.Date;
                }
                // _configuraSistema = ObterConfiguracao();
                n1.Regras = _configuraSistema.Regras;
                Entity.Regras = _configuraSistema.Regras;
                //Atualizar dados a serem exibidas na tela de empresa
                if (Entity == null) return;
                _service.CriarPendenciaImpeditiva(Entity);
                var view = new ViewSingleton().EmpresaView;
                var dados = view.DataContext as IAtualizarDados;
                dados.AtualizarDadosPendencias();

                if (Entity.FormatoCredencialId != 0)
                {
                    #region Verificar se pode gerar CardHolder
                    //Alterar o status do titular do cartão
                    if(n1.Validade > DateTime.Now || n1.CredencialStatusId == 1)
                    {
                        GerarCardHolder(n1.ColaboradorCredencialId, Entity);

                        var entity = _service.BuscarCredencialPelaChave(n1.ColaboradorCredencialId);
                        n1.CardHolderGuid = entity.CardHolderGuid;
                        n1.CredencialGuid = entity.CredencialGuid;

                        _service.AlterarStatusTitularCartao(new CredencialGenetecService(Main.Engine), Entity, n1);
                    }
                    else
                    {
                        _service.Alterar(n1);
                    }

                    #endregion
                }
                else
                {
                    _service.Alterar(n1);
                }
                
                
                ////Atualizar Observer
                ListarColaboradoresCredenciais(_colaboradorView);

                IsEnableLstView = true;
                AtualizarMensagem(Entity);

                Entity = null;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
                Utils.TraceException(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Criar CardHolder e Credencial do usuario
        /// <para>Criar um card holder caso o usuario nao o possua</para>
        /// </summary>
        /// <param name="colaboradorCredencialId">Identificador</param>
        private void GerarCardHolder(int colaboradorCredencialId, ColaboradoresCredenciaisView entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (Entity.ColaboradorPrivilegio1Id != 0 && Entity.ColaboradorPrivilegio1Id != 41)
            {
                var areaAcesso1 = _auxiliaresService.AreaAcessoService.Listar(Entity.ColaboradorPrivilegio1Id).FirstOrDefault();
                entity.Identificacao1 = areaAcesso1.Identificacao;
            }

            if (Entity.ColaboradorPrivilegio2Id != 0 && Entity.ColaboradorPrivilegio2Id != 41)
            {
                var areaAcesso2 = _auxiliaresService.AreaAcessoService.Listar(Entity.ColaboradorPrivilegio2Id).FirstOrDefault();
                entity.Identificacao2 = areaAcesso2.Identificacao;
            }
            _configuraSistema = ObterConfiguracao();
            entity.Regras = _configuraSistema.Regras;
            entity.GrupoPadrao = _configuraSistema.GrupoPadrao;

            _service.CriarTitularCartao(new CredencialGenetecService(Main.Engine), new ColaboradorService(), entity);

        }
        /// <summary>
        /// Criar CardHolder e Credencial do usuario
        /// <para>Criar um card holder caso o usuario nao o possua</para>
        /// </summary>
        /// <param name="colaboradorCredencialId">Identificador</param>
        private void RemoverRegrasCardHolder(int colaboradorCredencialId, ColaboradoresCredenciaisView entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var n1 = _service.BuscarCredencialPelaChave(colaboradorCredencialId);

            var tecCredencial = _auxiliaresService.TecnologiaCredencialService.BuscarPelaChave(entity.TecnologiaCredencialId);
            if (tecCredencial.PodeGerarCardHolder)
                _service.RemoverRegrasCardHolder(new CredencialGenetecService(Main.Engine), new ColaboradorService(), n1);

        }
        /// <summary>
        /// Criar CardHolder e Credencial do usuario
        /// <para>Criar um card holder caso o usuario nao o possua</para>
        /// </summary>
        /// <param name="colaboradorCredencialId">Identificador</param>
        private void RemoverCredencial(int colaboradorCredencialId, ColaboradoresCredenciaisView entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var n1 = _service.BuscarCredencialPelaChave(colaboradorCredencialId);

            var tecCredencial = _auxiliaresService.TecnologiaCredencialService.BuscarPelaChave(entity.TecnologiaCredencialId);
            if (tecCredencial.PodeGerarCardHolder)
                _service.RemoverCredencial(new CredencialGenetecService(Main.Engine), new ColaboradorService(), n1);

        }

        /// <summary>
        ///     Cancelar operação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                
                this.HabilitarOpcoesCredencial = false;
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                IsEnableLstView = true;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
                ListarColaboradoresCredenciais(_colaboradorView);                
                if (Entity != null) Entity.ClearMessageErro();
                Entity = null;                
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        ///     Remover dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<ColaboradorCredencial>(Entity);

                //Remover as regras do CardHorlder
                try
                {
                    RemoverRegrasCardHolder(n1.ColaboradorCredencialId, Entity);
                    RemoverCredencial(n1.ColaboradorCredencialId, Entity);
                    ///////////////////
                }
                catch (Exception)
                {

                }
                _service.Remover(n1);
                //Retirar empresa da coleção
                EntityObserver.Remove(Entity);

                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        ///     Imprimir Credencial
        /// </summary>
        private void OnImprimirCredencial()
        {
            try
            {

                //if (Entity == null) return;
                if (Entity == null) throw new InvalidOperationException("Selecione uma credencial para impressão.");
                //if (!Entity.Ativa) throw new InvalidOperationException("Não é possível imprimir uma credencial não ativa.");
                if (Entity.Validade == null) throw new InvalidOperationException("Não é possível imprimir uma credencial sem data de validade.");

                var layoutCracha = _auxiliaresService.LayoutCrachaService.BuscarPelaChave(Entity.LayoutCrachaId);
                if (layoutCracha == null) throw new InvalidOperationException("Não é possível imprimir uma credencial sem ter sido definida um layout do crachá.");
                if (string.IsNullOrWhiteSpace(layoutCracha.LayoutRpt)) throw new InvalidOperationException("Não é possível imprimir uma credencial sem ter sido definida um layout do crachá.");

                ObterValidadeAlteracao();

                Cursor.Current = Cursors.WaitCursor;

                var arrayBytes = WpfHelp.ConverterBase64(layoutCracha.LayoutRpt, "Layout Cracha");
                var relatorio = WpfHelp.ShowRelatorioCrystalReport(arrayBytes, layoutCracha.Nome);

                if (Entity.Estrangeiro)
                {
                    TextObject txtRNE = (TextObject)relatorio.ReportDefinition.ReportObjects["Text1"];
                    txtRNE.Text = "RNE:";

                    TextObject txt_RG_RNE = (TextObject)relatorio.ReportDefinition.ReportObjects["obj_RG_RNE"];
                    txt_RG_RNE.Text = Entity.RNE.ToString();
                }
                else
                {
                    TextObject txtRNE = (TextObject)relatorio.ReportDefinition.ReportObjects["Text1"];
                    txtRNE.Text = "RG:";

                    TextObject txt_RG_RNE = (TextObject)relatorio.ReportDefinition.ReportObjects["obj_RG_RNE"];
                    txt_RG_RNE.Text = Entity.Rg.ToString();
                }
                try
                {
                    //FieldObject txtRG1 = (FieldObject)relatorio.ReportDefinition.ReportObjects["RG1"];

                    // txtRG1.DataSource.FormulaName = "{IMOD_CredenciamentoDeskTop_Views_Model_CredencialViewCracha.RG}"; 
                }
                catch (Exception ex)
                {

                    throw;
                }


                var colaboradorCursosCracha = _auxiliaresService.ColaboradorCursoService.ListarView(Entity.ColaboradorId, null, true);
                string _cursosCracha = "";

                foreach (ColaboradorCurso element in colaboradorCursosCracha)
                {
                    if (_cursosCracha == "")
                    {
                        _cursosCracha = !String.IsNullOrEmpty(element.Descricao) ? " - " + element.Descricao?.ToString() : "";
                    }
                    else
                    {
                        _cursosCracha = _cursosCracha + Environment.NewLine + " - " + element.Descricao.ToString();
                    }
                }

                var lst = new List<CredencialViewCracha>();
                var credencialView = _service.ObterCredencialView(Entity.ColaboradorCredencialId);
                var c1 = Mapper.Map<CredencialViewCracha>(credencialView);
                c1.CrachaCursos = _cursosCracha;

                //se o tipo for diferente de via adicional ou o motivo form "DANO" não exibe no crachá;
                if (c1.CredencialMotivoID != 2 || c1.CredencialmotivoViaAdicionalID == 22)
                {
                    c1.ImpressaoMotivo = "";
                }
                else
                {
                    c1.ImpressaoMotivo = c1.CredencialVia + "ª " + c1.ImpressaoMotivo;
                }
                c1.TelefoneEmergencia = "EMERGÊNCIA " + _configuraSistema.TelefoneEmergencia;
                c1.EmpresaNome = c1.EmpresaNome + (!string.IsNullOrEmpty(c1.TerceirizadaNome?.Trim()) ? " / " + c1.TerceirizadaNome?.Trim() : string.Empty);
                c1.EmpresaApelido = (!string.IsNullOrEmpty(c1.TerceirizadaNome) ? c1.TerceirizadaNome?.Trim() : c1.EmpresaApelido?.Trim());
                c1.Emissao = DateTime.Now;
                lst.Add(c1);
                relatorio.SetDataSource(lst);

                var objCode = new QrCode();
                string querySistema = _configuraSistema.UrlSistema?.Trim().ToString() + "/Colaborador/Credential/"
                                                + Helpers.Helper.Encriptar(c1.ColaboradorCredencialID.ToString());

                var pathImagem = objCode.GerarQrCode(querySistema, "QrCodeAutorizacao" + c1.ColaboradorCredencialID.ToString() + ".png");
                relatorio.SetParameterValue("PathImgQrCode", pathImagem);

                //IDENTIFICACAO
                var popupCredencial = new PopupCredencial(relatorio, _service, Entity, layoutCracha, HabilitaImpressao);
                popupCredencial.ShowDialog();

                //Atualizar observer
                OnPropertyChanged("Entity");
                CollectionViewSource.GetDefaultView(EntityObserver).Refresh();

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            //Listar Colaboradores Ativos
            //OnAtualizarDadosContratosAtivos();

            verificarcredencialAtiva = true;
            if (Entity == null)
            {
                WpfHelp.PopupBox("Selecione um item da lista", 1);
                return;
            }

            Comportamento.PrepareAlterar();
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            IsEnableLstView = false;

            //Habilitar controles somente se a credencial não estiver sido impressa
            ColeteEnabled = true;
            Habilitar = !Entity.Impressa;
            if (Entity.CardHolderGuid == null)
            {
                Habilitar = !Entity.Impressa;
            }


            _viewModelParent.HabilitaControleTabControls(false, false, false, false, false, true);
        }

        private void PrepareSalvar()
        {
            try
            {
                verificarcredencialAtiva = false;
                // 
                if (Entity.NumeroColete == "" & HabilitaImpressao == true)
                {
                    if (ColeteEnabled) // Se o campo Nº do colete estiver IsEnabled=false a menssagem não será exibiba
                    {
                        var podeCobrarResult = WpfHelp.MboxDialogYesNo($"Nº do Colete esta em branco, Continua.", true);
                        if (podeCobrarResult == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                    }

                }

                if (Validar()) return;
                Comportamento.PrepareSalvar();
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex);
            }
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        public void Motivacao_Select()
        {
            try
            {
                HabilitarVias = "Collapsed";
                //if (!Habilitar) return;
                if (Entity == null) return;
                if (Entity.CredencialMotivoId == 2)
                {
                    Entity.CredencialVia = null;
                    var listCeredenciais = _service.Listar(null, null, null, null, null, null, null, null, null, null, Entity.ColaboradorEmpresaId).OrderByDescending(c => c.ColaboradorCredencialId).ToList();
                    var ultimacredencial = listCeredenciais.Where(c => c.ColaboradorCredencialId != Entity.ColaboradorCredencialId).FirstOrDefault();

                    if (ultimacredencial == null || ultimacredencial.CredencialVia == null || ultimacredencial.CredencialVia == 0)
                    {
                        Entity.CredencialVia = 2;
                    }
                    else
                    {
                        Entity.CredencialVia = ultimacredencial.CredencialVia + 1;
                    }

                    HabilitarVias = "Visible";
                }
                else
                {
                    if (Constantes.Constantes.ATIVO.Equals(Entity.CredencialStatusId))
                    {
                        Entity.CredencialVia = null;
                        Entity.CredencialmotivoViaAdicionalID = null;
                    }
                }
                _viaAdicional = Entity.CredencialVia;
                CollectionViewSource.GetDefaultView(EntityObserver).Refresh();

            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        /// <summary>
        ///     Carregar Caracteres Colete
        /// </summary>
        public void CarregarCaracteresColete(ColaboradorEmpresa colaboradorEmpresa)
        {
            if (Entity == null) return;
            if (colaboradorEmpresa.EmpresaSigla == null) return;
            Entity.EmpresaSigla = colaboradorEmpresa.EmpresaSigla?.Trim();
            _configuraSistema = ObterConfiguracao();
            if (_configuraSistema.Colete)
            {
                //if (Entity == null || colaboradorEmpresa == null || colaboradorEmpresa.EmpresaSigla == null) return;
                //Entity.Colete = colaboradorEmpresa.EmpresaSigla.Trim() + Convert.ToString(_colaboradorView.ColaboradorId);
                IsEnableColete = false;
            }
            else
            {
                //if (Entity == null || colaboradorEmpresa == null || colaboradorEmpresa.EmpresaSigla == null) return;
                //Entity.Colete = colaboradorEmpresa.EmpresaSigla.Trim(); //+ Convert.ToString(_colaboradorView.ColaboradorId);
                IsEnableColete = true;
            }

        }
        /// <summary>
        ///     Carregar Vinculos Ativos do Colaborador
        /// </summary>
        public void CarregarVinculosAtivos(int colaboradorId, int empresaId)
        {
            try
            {
                if (!verificarcredencialAtiva) return;
                if (colaboradorId == 0) return;
                if (empresaId == 0) return;
                var n1 = _service.Listar(null, null, null, null, colaboradorId, empresaId, true);
                if (n1.Count >= 1)
                {
                    verificarcredencialAtiva = false;
                    Comportamento.PrepareCancelar();
                    WpfHelp.Mbox("Já existe credencial ativa para esse contrato.");
                    return;
                }
                verificarcredencialAtiva = false;
                if (Entity != null)
                {
                    if (Entity.EmpresaSigla == null || Entity.EmpresaSigla == "")
                    {
                        ColeteEnabled = false;
                    }
                    else
                    {
                        ColeteEnabled = true;
                    }
                }
            }
            catch
            {

            }
        }
        /// <summary>
        ///     Carregar Vinculos Ativos do Colaborador
        /// </summary>
        public void CarregarVinculosAtivosOutrasCredenciais(int colaboradorId, int empresaId)
        {
            try
            {
                if (!verificarcredencialAtiva) return;
                if (colaboradorId == 0) return;
                if (empresaId == 0) return;
                var n1 = _service.Listar(null, null, null, null, colaboradorId, empresaId, true, null, Entity.ColaboradorCredencialId);
                if (n1.Count >= 1)
                {
                    verificarcredencialAtiva = false;
                    Comportamento.PrepareCancelar();
                    WpfHelp.Mbox("Já existe credencial ativa para esse contrato.");
                    return;
                }
                verificarcredencialAtiva = false;
            }
            catch
            {

            }
        }
        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public bool Validar()
        {
            if (Entity == null) return true;
            Entity.Validate();
            var hasErros = Entity.HasErrors;

            int rangefc = 0;
            if(!ValidaFC(Entity.TipoCredencialId, Entity.FormatoCredencialId, Entity.Fc, out rangefc))
            {
                WpfHelp.PopupBox("Para o formato selecionado o valor [FC] deve estar entre 0 e " + rangefc, 1);
                return true;
            }

            long rangecredencial = 0;
            if (!ValidaNumeroCredencial(Entity.TipoCredencialId, Entity.FormatoCredencialId, Entity.NumeroCredencial, out rangecredencial))
            {
                WpfHelp.PopupBox("Para o formato selecionado o valor [Número] deve estar entre 0 e " + rangecredencial, 1);
                return true;
            }
            //retirar o espaço entre a numeração obtida do cartão
            if (Entity.CredencialStatusId == 1)
            {
                if (Entity.Validade.Value.AddHours(23).AddMinutes(59).AddSeconds(59) < DateTime.Now)
                {
                    WpfHelp.PopupBox("Data de Validade não pode ser menor que a data atual. Não é possível continua essa ação.", 1);
                    return true;
                }
            }

            if (!string.IsNullOrEmpty(Entity.NumeroContrato))
            {
                Entity.NumeroCredencial = Regex.Replace(Entity.NumeroCredencial, @"\s", "");
            }

            if (hasErros) return true;

            if (ExisteNumeroCredencial())
            {
                Entity.SetMessageErro("NumeroCredencial", "Número de credencial já existente.");
                return true;
            }
            if (Entity.NumeroColete != "")
            {
                var colaboradorcredencial = ExisteNumeroColete();
                if (colaboradorcredencial != null)
                {
                    Entity.SetMessageErro("Colete", "Número do colete já existente.");
                    WpfHelp.PopupBox("Número do colete já cadastrado para o colaborador  " + colaboradorcredencial.ColaboradorNome.ToString() + " ",1);
                    return true;
                }
            }

            TimeSpan diferenca = Convert.ToDateTime(Entity.Validade) - DateTime.Now.Date;
            int credencialDias = int.Parse(diferenca.Days.ToString());
            if (Entity.TipoCredencialId == 1)
            {
                if (credencialDias > 730)
                {
                    System.Windows.MessageBox.Show("Validade da credencial PERMANENTE, não pode ser superior a 2 anos!");
                    return true;
                }

            }
            if (Entity.TipoCredencialId == 2)
            {
                if (credencialDias > 90)
                {
                    WpfHelp.PopupBox("Validade da credencial TEMPORÁRIA, não pode ser superior a 90 dias!",1);
                    return true;
                }
            }

            if (Entity.TecnologiaCredencialId != 0)
            {
                //_configuraSistema = ObterConfiguracao();
                //if (_configuraSistema.Regras)
                //{
                //    if (Entity.ColaboradorPrivilegio1Id == 0 && Entity.ColaboradorPrivilegio2Id == 0)
                //    {
                //        //System.Windows.MessageBox.Show("REgras não informadas");
                //        WpfHelp.Mbox("Para a Autenticação selecionada é necessário o preenchimento dos Privilégios.", MessageBoxIcon.Information);
                //        return true;
                //    }
                //}
                if (Entity.FormatoCredencialId == 0)
                {
                    //System.Windows.MessageBox.Show("Formato da credencial não informada");
                    WpfHelp.PopupBox("Para a Autenticação selecionada é necessário o preenchimento do formato da credencial.", 1);
                    return true;
                }
                if (Entity.NumeroCredencial == null || Entity.NumeroCredencial == "")
                {
                    //System.Windows.MessageBox.Show("Nº da credencial não informado");
                    WpfHelp.PopupBox("O nº da credencial é obrigatório para esta ação. Não é possível criar uma credencial sem essa infrmação", 1);
                    return true;
                }
            }
            return Entity.HasErrors;
        }
        public bool ValidaFC(int tipocreencial,int formatocredencial,int numerofc, out int rengefc)
        {
            if (tipocreencial > 0)
            {
                switch (formatocredencial)
                {
                    case 1:
                        rengefc = 255;
                        return numerofc <= 255;
                    case 2:
                        rengefc = 65535;
                        return numerofc <= 65535;
                    case 3:
                        rengefc = 0;
                        return true;
                    case 4:
                        rengefc = 65535;
                        return numerofc <= 65535;
                    case 5:
                        rengefc = 4095;
                        return numerofc <= 4095;
                    case 6:
                        rengefc = 4194303;
                        return numerofc <= 4194303;
                    default:
                        rengefc = 0;
                        return true;
                }
            }
            else
            {
                rengefc = 0;
                return true;
            }
        }
        public bool ValidaNumeroCredencial(int tipocreencial, int formatocredencial, string numerocredencial, out long rengecrecencial)
        {
            long NumeroCredencialvalidade = 0;
            long.TryParse(numerocredencial, out NumeroCredencialvalidade);
            if (tipocreencial > 0)
            {
                switch (formatocredencial)
                {
                    case 1:
                        rengecrecencial = 65535;
                        return NumeroCredencialvalidade <= 65535;
                    case 2:
                        rengecrecencial = 65535;
                        return NumeroCredencialvalidade <= 65535;
                    case 3:
                        rengecrecencial = 34359738637;
                        return NumeroCredencialvalidade <= 34359738637;
                    case 4:
                        rengecrecencial = 524287;
                        return NumeroCredencialvalidade <= 524287;
                    case 5:
                        rengecrecencial = 1048575;
                        return NumeroCredencialvalidade <= 1048575;
                    case 6:
                        rengecrecencial = 8388607;
                        return NumeroCredencialvalidade <= 8388607;
                    case 7:
                        rengecrecencial = 4294967295;
                        return NumeroCredencialvalidade <= 4294967295;
                    default:
                        rengecrecencial = 0;
                        return true;
                }
            }
            else
            {
                rengecrecencial = 0;
                return true;
            }
        }


        #endregion

        #region Propriedade de Pesquisa

        /// <summary>
        ///     String contendo o nome a pesquisa;
        /// </summary>
        public string NomePesquisa { get; set; }

        /// <summary>
        ///     Opções de pesquisa
        /// </summary>
        public List<KeyValuePair<int, string>> ListaPesquisa { get; private set; }

        /// <summary>
        ///     Pesquisar por
        /// </summary>
        public KeyValuePair<int, string> PesquisarPor { get; set; }

        #endregion

        #region Propriedade Commands

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase(PrepareCriar, true);

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Editar
        /// </summary>
        public ICommand PrepareAlterarCommand => new CommandBase(PrepareAlterar, true);

        /// <summary>
        ///     Cancelar
        /// </summary>
        public ICommand PrepareCancelarCommand => new CommandBase(PrepararCancelar, true);

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareSalvarCommand => new CommandBase(PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase(PrepareRemover, true);

        /// <summary>
        ///     Pesquisar
        /// </summary>
        //public ICommand PesquisarCommand => new CommandBase(Motivacao_Select, true);

        public ICommand ImprimirCommand => new CommandBase(OnImprimirCredencial, true);



        #endregion
    }
}