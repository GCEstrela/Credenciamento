// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Forms;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    
    public class ConfiguracoesViewModel : ViewModelBase
    {
       
        #region Inicializacao

        public ConfiguracoesViewModel()
        {
            var carregaUiThr = new Thread (() => CarregaUi());
            carregaUiThr.Start();
        }

        private void CarregaUi()
        {
            CarregaColecaoRelatorios();
            CarregaColecaoRelatoriosGerenciais();
            CarregaColecaoLayoutsCrachas();
            CarregaColecaoTiposEquipamentos();
            CarregaColecaoTiposAcessos();
            CarregaColecaoAreasAcessos();
            CarregaColecaoTiposAtividades();
            CarregaColecaoTipoServico();
            CarregaColecaoTecnologiasCredenciais();
            CarregaColecaoTiposCobrancas();
            CarregaColecaoCursos();
            CarregaColecaoTipoCombustiveis();
            CarregaColecaoStatus();
            CarregaColecaoCredenciaisStatus();
            CarregaColecaoFormatosCredenciais();
        }

        #endregion

        #region Variáveis Públicas

        public RelatorioView RelatorioTemp = new RelatorioView();
        public RelatorioGerencialView RelatorioGerencialTemp = new RelatorioGerencialView();
        public LayoutCrachaView LayoutCrachaTemp = new LayoutCrachaView();

        #endregion

        #region Variaveis privadas

        //private SynchronizationContext _mainThread;

        private int _selectedIndex;
       // private int _selectedIndexTemp = 0;

        //Relatórios
        private ObservableCollection<RelatorioView> _relatorios;
        private readonly List<RelatorioView> _relatoriosTemp = new List<RelatorioView>();
        private RelatorioView _relatorioSelecionado;
        private int _relatorioSelectedIndex;

        //Relatórios Gerenciais
        private ObservableCollection<RelatorioGerencialView> _relatoriosGerenciais;
        private readonly List<RelatorioGerencialView> _relatoriosGerenciaisTemp = new List<RelatorioGerencialView>();
        private RelatorioGerencialView _relatorioGerencialSelecionado;
        private int _relatorioGerencialSelectedIndex;

        //Layouts Crachás
        private ObservableCollection<LayoutCrachaView> _layoutsCrachas;
        private readonly List<LayoutCrachaView> _layoutsCrachasTemp = new List<LayoutCrachaView>();
        private LayoutCrachaView _layoutCrachaSelecionado;
        private int _layoutCrachaSelectedIndex;

        //TipoEquipamento
        private ObservableCollection<TipoEquipamentoView> _tiposEquipamentos;
        private readonly List<TipoEquipamentoView> _tiposEquipamentosTemp = new List<TipoEquipamentoView>();
        private TipoEquipamentoView _tipoEquipamentoSelecionado;
        private int _tipoEquipamentoSelectedIndex;

        //Tipos Acessos
        private ObservableCollection<TipoAcessoView> _tiposAcessos;
        private readonly List<TipoAcessoView> _tiposAcessosTemp = new List<TipoAcessoView>();
        private TipoAcessoView _tiposAcessoSelecionado;
        private int _tipoAcessoSelectedIndex;

        //Áreas Acessos
        private ObservableCollection<AreaAcessoView> _areaAcessos;
        private readonly List<AreaAcessoView> _areaAcessoTemp = new List<AreaAcessoView>();
        private AreaAcessoView _acessoAreaSelecionada;
        private int _areaAcessoSelectedIndex;
        private int _selectedAcessoIndex;

        //Tipos Atividades
        private ObservableCollection<TipoAtividadeView> _tiposAtividade;
        private readonly List<TipoAtividadeView> _tiposAtividadesTemp = new List<TipoAtividadeView>();
        private TipoAtividadeView _atividadeSelecionada;
        private int _tipoAtividadeSelectedIndex;

        //Tipos Servico
        private ObservableCollection<TipoServicoView> _tipoServico;
        private readonly List<TipoServicoView> _tipoServicoTemp = new List<TipoServicoView>();
        private TipoServicoView _tipoServicoSelecionado;
        private int _tipoServicoSelectedIndex;

        //Tecnologias Credenciais
        private ObservableCollection<TecnologiaCredencialView> _tecnologiasCredenciais;
        private readonly List<TecnologiaCredencialView> _tecnologiaCredencialTemp = new List<TecnologiaCredencialView>();
        private TecnologiaCredencialView _tecnologiaCredencialSelecionada;
        private int _tecnologiaCredencialSelectedIndex;

        //Tipos Cobranças
        private ObservableCollection<TipoCobrancaView> _tiposCobrancas;
        private readonly List<TipoCobrancaView> _tiposCobrancasTemp = new List<TipoCobrancaView>();
        private TipoCobrancaView _cobrancaSelecionada;
        private int _tipoCobrancaSelectedIndex;

        //Cursos
        private ObservableCollection<CursoView> _cursos;
        private readonly List<CursoView> _cursosTemp = new List<CursoView>();
        private CursoView _cursosSelecionado;
        private int _cursoSelectedIndex;

        //TipoCombustível
        private ObservableCollection<TipoCombustivelView> _tiposCombustiveis;
        private readonly List<TipoCombustivelView> _tiposCombustiveisTemp = new List<TipoCombustivelView>();
        private TipoCombustivelView _tipoCombustivelSelecionado;
        private int _tipoCombustivelSelectedIndex;

        //Tipos Status
        private ObservableCollection<StatusView> _tiposStatus;
        private readonly List<StatusView> _statusTemp = new List<StatusView>();
        private StatusView _statusSelecionado;
        private int _tipoStatusSelectedIndex;

        //Credenciais Status
        private ObservableCollection<CredencialStatusView> _credenciaisStatus;
        private readonly List<CredencialStatusView> _credencialStatusTemp = new List<CredencialStatusView>();
        private CredencialStatusView _credencialStatusSelecionado;
        private int _credencialStatusSelectedIndex;

        //Credenciais Motivos
        private ObservableCollection<CredencialMotivoView> _credenciaisMotivos;
        private readonly List<CredencialMotivoView> _credencialMotivoTemp = new List<CredencialMotivoView>();
        private CredencialMotivoView _credencialMotivoSelecionado;
        private int _credencialMotivoSelectedIndex;

        //Formatos Credenciais
        private ObservableCollection<FormatoCredencialView> _formatosCredenciais;
        private readonly List<FormatoCredencialView> _formatoCredencialTemp = new List<FormatoCredencialView>();
        private FormatoCredencialView _formatoCredencialSelecionado;
        private int _formatoCredencialSelectedIndex;

        //Serviços
        private readonly IEmpresaLayoutCrachaService _serviceEmpresasCracha = new EmpresaLayoutCrachaService();
        private readonly IEmpresaService _serviceEmpresas = new EmpresaService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IRelatorioService _relatorioService = new RelatorioService();
        private readonly IRelatorioGerencialService _relatorioGerencialService = new RelatorioGerencialService();

        private Relatorios _relatorio = new Relatorios();
        private RelatoriosGerenciais _relatorioGerencial = new RelatoriosGerenciais();
        private LayoutCracha _layoutCracha = new LayoutCracha();

        #endregion

        #region Contrutores

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged ("SelectedIndex");
            }
        }

        //Relatórios
        public ObservableCollection<RelatorioView> Relatorios
        {
            get { return _relatorios; }

            set
            {
                if (_relatorios != value)
                {
                    _relatorios = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelatorioView RelatorioSelecionado
        {
            get { return _relatorioSelecionado; }
            set
            {
                _relatorioSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (_relatorioSelecionado != null)
                {
                }
            }
        }

        public int RelatorioSelectedIndex
        {
            get { return _relatorioSelectedIndex; }
            set
            {
                _relatorioSelectedIndex = value;
                OnPropertyChanged ("RelatorioSelectedIndex");
            }
        }

        //Relatórios Gerenciais
        public ObservableCollection<RelatorioGerencialView> RelatoriosGerenciais
        {
            get { return _relatoriosGerenciais; }

            set
            {
                if (_relatoriosGerenciais != value)
                {
                    _relatoriosGerenciais = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelatorioGerencialView RelatorioGerencialSelecionado
        {
            get { return _relatorioGerencialSelecionado; }
            set
            {
                _relatorioGerencialSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (_relatorioGerencialSelecionado != null)
                {
                }
            }
        }

        public int RelatorioGerencialSelectedIndex
        {
            get { return _relatorioGerencialSelectedIndex; }
            set
            {
                _relatorioGerencialSelectedIndex = value;
                OnPropertyChanged ("RelatorioGerencialSelectedIndex");
            }
        }

        //Layouts Crachás
        public ObservableCollection<LayoutCrachaView> LayoutsCrachas
        {
            get { return _layoutsCrachas; }

            set
            {
                if (_layoutsCrachas != value)
                {
                    _layoutsCrachas = value;
                    OnPropertyChanged();
                }
            }
        }

        public LayoutCrachaView LayoutCrachaSelecionado
        {
            get { return _layoutCrachaSelecionado; }
            set
            {
                _layoutCrachaSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (_layoutCrachaSelecionado != null)
                {
                }
            }
        }

        public int LayoutCrachaSelectedIndex
        {
            get { return _layoutCrachaSelectedIndex; }
            set
            {
                _layoutCrachaSelectedIndex = value;
                OnPropertyChanged ("LayoutCrachaSelectedIndex");
            }
        }

        //Tipos Equipamentos
        public ObservableCollection<TipoEquipamentoView> TiposEquipamentos
        {
            get { return _tiposEquipamentos; }

            set
            {
                if (_tiposEquipamentos != value)
                {
                    _tiposEquipamentos = value;
                    OnPropertyChanged();
                }
            }
        }

        public TipoEquipamentoView TipoEquipamentoSelecionado
        {
            get { return _tipoEquipamentoSelecionado; }
            set
            {
                _tipoEquipamentoSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (TipoEquipamentoSelecionado != null)
                {
                }
            }
        }

        public int TipoEquipamentoSelectedIndex
        {
            get { return _tipoEquipamentoSelectedIndex; }
            set
            {
                _tipoEquipamentoSelectedIndex = value;
                OnPropertyChanged ("TipoEquipamentoSelectedIndex");
            }
        }

        //Tipos Acessos
        public ObservableCollection<TipoAcessoView> TiposAcessos
        {
            get { return _tiposAcessos; }

            set
            {
                if (_tiposAcessos != value)
                {
                    _tiposAcessos = value;
                    OnPropertyChanged();
                }
            }
        }

        public TipoAcessoView TipoAcessoSelecionado
        {
            get { return _tiposAcessoSelecionado; }
            set
            {
                _tiposAcessoSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (TipoAcessoSelecionado != null)
                {
                }
            }
        }

        public int TipoAcessoSelectedIndex
        {
            get { return _tipoAcessoSelectedIndex; }
            set
            {
                _tipoAcessoSelectedIndex = value;
                OnPropertyChanged ("TipoAcessoSelectedIndex");
            }
        }

        //Tipos Áreas Acessos
        public ObservableCollection<AreaAcessoView> AreasAcessos
        {
            get { return _areaAcessos; }

            set
            {
                if (_areaAcessos != value)
                {
                    _areaAcessos = value;
                    OnPropertyChanged();
                }
            }
        }

        public AreaAcessoView AreaAcessoSelecionada
        {
            get { return _acessoAreaSelecionada; }
            set
            {
                _acessoAreaSelecionada = value;
                OnPropertyChanged ("SelectedItem");
                if (AreaAcessoSelecionada != null)
                {
                }
            }
        }

        public int AreaAcessoSelectedIndex
        {
            get { return _areaAcessoSelectedIndex; }
            set
            {
                _areaAcessoSelectedIndex = value;
                OnPropertyChanged ("AreaAcessoSelectedIndex");
            }
        }

        //Tipos Atividades
        public ObservableCollection<TipoAtividadeView> TiposAtividades
        {
            get { return _tiposAtividade; }

            set
            {
                if (_tiposAtividade != value)
                {
                    _tiposAtividade = value;
                    OnPropertyChanged();
                }
            }
        }

        public TipoAtividadeView TipoAtividadeSelecionada
        {
            get { return _atividadeSelecionada; }
            set
            {
                _atividadeSelecionada = value;
                OnPropertyChanged ("SelectedItem");
                if (TipoAtividadeSelecionada != null)
                {
                }
            }
        }

        public int TipoAtividadeSelectedIndex
        {
            get { return _tipoAtividadeSelectedIndex; }
            set
            {
                _tipoAtividadeSelectedIndex = value;
                OnPropertyChanged ("TipoAtividadeSelectedIndex");
            }
        }

        //Tipos Serviços
        public ObservableCollection<TipoServicoView> TipoServico
        {
            get { return _tipoServico; }

            set
            {
                if (_tipoServico != value)
                {
                    _tipoServico = value;
                    OnPropertyChanged();
                }
            }
        }

        public TipoServicoView TipoServicoSelecionado
        {
            get { return _tipoServicoSelecionado; }
            set
            {
                _tipoServicoSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (TipoServicoSelecionado != null)
                {
                }
            }
        }

        public int TipoServicoSelectedIndex
        {
            get { return _tipoServicoSelectedIndex; }
            set
            {
                _tipoServicoSelectedIndex = value;
                OnPropertyChanged ("TipoServicoSelectedIndex");
            }
        }

        //Tecnologias Credenciais
        public ObservableCollection<TecnologiaCredencialView> TecnologiasCredenciais
        {
            get { return _tecnologiasCredenciais; }
            set
            {
                if (_tecnologiasCredenciais != value)
                {
                    _tecnologiasCredenciais = value;
                    OnPropertyChanged();
                }
            }
        }

        public TecnologiaCredencialView TecnologiaCredencialSelecionada
        {
            get { return _tecnologiaCredencialSelecionada; }
            set
            {
                _tecnologiaCredencialSelecionada = value;
                OnPropertyChanged ("SelectedItem");
                if (_tecnologiaCredencialSelecionada != null)
                {
                }
            }
        }

        public int TecnologiaCredencialSelectedIndex
        {
            get { return _tecnologiaCredencialSelectedIndex; }
            set
            {
                _tecnologiaCredencialSelectedIndex = value;
                OnPropertyChanged ("TecnologiaCredencialSelectedIndex");
            }
        }

        //Tipos Cobranças
        public ObservableCollection<TipoCobrancaView> TiposCobrancas
        {
            get { return _tiposCobrancas; }

            set
            {
                if (_tiposCobrancas != value)
                {
                    _tiposCobrancas = value;
                    OnPropertyChanged();
                }
            }
        }

        public TipoCobrancaView TipoCobrancaSelecionado
        {
            get { return _cobrancaSelecionada; }
            set
            {
                _cobrancaSelecionada = value;
                OnPropertyChanged ("SelectedItem");
                if (TipoCobrancaSelecionado != null)
                {
                }
            }
        }

        public int TipoCobrancaSelectedIndex
        {
            get { return _tipoCobrancaSelectedIndex; }
            set
            {
                _tipoCobrancaSelectedIndex = value;
                OnPropertyChanged ("TipoCobrancaSelectedIndex");
            }
        }

        //Cursos
        public ObservableCollection<CursoView> Cursos
        {
            get { return _cursos; }

            set
            {
                if (_cursos != value)
                {
                    _cursos = value;
                    OnPropertyChanged();
                }
            }
        }

        public CursoView CursoSelecionado
        {
            get { return _cursosSelecionado; }
            set
            {
                _cursosSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (CursoSelecionado != null)
                {
                }
            }
        }

        public int CursoSelectedIndex
        {
            get { return _cursoSelectedIndex; }
            set
            {
                _cursoSelectedIndex = value;
                OnPropertyChanged ("CursoSelectedIndex");
            }
        }

        //Tipos Combustíveis
        public ObservableCollection<TipoCombustivelView> TiposCombustiveis
        {
            get { return _tiposCombustiveis; }

            set
            {
                if (_tiposCombustiveis != value)
                {
                    _tiposCombustiveis = value;
                    OnPropertyChanged();
                }
            }
        }

        public TipoCombustivelView TipoCombustivelSelecionado
        {
            get { return _tipoCombustivelSelecionado; }

            set
            {
                _tipoCombustivelSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (_tipoCombustivelSelecionado != null)
                {
                }
            }
        }

        public int TipoCombustivelSelectedIndex
        {
            get { return _tipoCombustivelSelectedIndex; }
            set
            {
                _tipoCombustivelSelectedIndex = value;
                OnPropertyChanged ("TipoCombustivelSelectedIndex");
            }
        }

        //Tipos Status
        public ObservableCollection<StatusView> TiposStatus
        {
            get { return _tiposStatus; }

            set
            {
                if (_tiposStatus != value)
                {
                    _tiposStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public StatusView TipoStatusSelecionado
        {
            get { return _statusSelecionado; }
            set
            {
                _statusSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (TipoStatusSelecionado != null)
                {
                }
            }
        }

        public int TipoStatusSelectedIndex
        {
            get { return _tipoStatusSelectedIndex; }
            set
            {
                _tipoStatusSelectedIndex = value;
                OnPropertyChanged ("TipoStatusSelectedIndex");
            }
        }

        //Credenciais Status
        public ObservableCollection<CredencialStatusView> CredenciaisStatus
        {
            get { return _credenciaisStatus; }

            set
            {
                if (_credenciaisStatus != value)
                {
                    _credenciaisStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public CredencialStatusView CredenciaisStatusSelecionado
        {
            get { return _credencialStatusSelecionado; }
            set
            {
                _credencialStatusSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (TipoStatusSelecionado != null)
                {
                }
            }
        }

        public int CredencialStatusSelectedIndex
        {
            get { return _credencialStatusSelectedIndex; }
            set
            {
                _credencialStatusSelectedIndex = value;
                OnPropertyChanged ("CredencialStatusSelectedIndex");
            }
        }

        //Credenciais Motivos
        public ObservableCollection<CredencialMotivoView> CredenciaisMotivos
        {
            get { return _credenciaisMotivos; }

            set
            {
                if (_credenciaisMotivos != value)
                {
                    _credenciaisMotivos = value;
                    OnPropertyChanged();
                }
            }
        }

        public CredencialMotivoView CredencialMotivoSelecionado
        {
            get { return _credencialMotivoSelecionado; }
            set
            {
                _credencialMotivoSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (TipoStatusSelecionado != null)
                {
                }
            }
        }

        public int CredencialMotivoSelectedIndex
        {
            get { return _credencialMotivoSelectedIndex; }
            set
            {
                _credencialMotivoSelectedIndex = value;
                OnPropertyChanged ("CredencialMotivoSelectedIndex");
            }
        }

        //Formatos Credenciais
        public ObservableCollection<FormatoCredencialView> FormatosCredenciais
        {
            get { return _formatosCredenciais; }

            set
            {
                if (_formatosCredenciais != value)
                {
                    _formatosCredenciais = value;
                    OnPropertyChanged();
                }
            }
        }

        public FormatoCredencialView FormatoCredencialSelecionado
        {
            get { return _formatoCredencialSelecionado; }
            set
            {
                _formatoCredencialSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (_formatoCredencialSelecionado != null)
                {
                }
            }
        }

        public int FormatoCredencialSelectedIndex
        {
            get { return _formatoCredencialSelectedIndex; }
            set
            {
                _formatoCredencialSelectedIndex = value;
                OnPropertyChanged ("FormatoCredencialSelectedIndex");
            }
        }

        #endregion

        #region Comandos dos Botoes

        #region Comandos dos Botoes Relatorios

        public void OnAdicionarRelatorioCommand()
        {
            try
            {
                foreach (var x in Relatorios)
                {
                    _relatoriosTemp.Add (x);
                }

                _relatorioSelectedIndex = RelatorioSelectedIndex;
                Relatorios.Clear();

                RelatorioTemp = new RelatorioView();
                Relatorios.Add (RelatorioTemp);

                RelatorioSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarRelatorioCommand()
        {
            try
            {
                //Atualiza arquivo Byte[] (.rpt)
                RelatorioSelecionado.ArquivoRpt = RelatorioTemp.ArquivoRpt;

                var entity = RelatorioSelecionado;
                var entityConv = Mapper.Map<Relatorios> (entity);

                if (RelatorioSelecionado.RelatorioId != 0)
                {
                    _auxiliaresService.RelatorioService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.RelatorioService.Criar (entityConv);
                }

                CarregaColecaoRelatorios();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirRelatorioCommand()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove(); 
                if (result != DialogResult.Yes) return; 
                var entity = RelatorioSelecionado; 
                if (entity == null) return; 
                var entityConv = Mapper.Map<Relatorios> (entity);
                _auxiliaresService.RelatorioService.Remover (entityConv);

                Relatorios.Remove (RelatorioSelecionado);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnBuscarRelatorioCommand()
        {
            try
            {
                var filtro = "Crystal Report Files|*.rpt";
                var arq = WpfHelp.UpLoadArquivoDialog (filtro, 0);
                if (arq == null)
                {
                    return;
                }

                RelatorioTemp.RelatorioId = RelatorioSelecionado.RelatorioId;
                RelatorioSelecionado.Nome = arq.Nome;

                RelatorioTemp.Nome = RelatorioSelecionado.Nome;
                RelatorioTemp.NomeArquivoRpt = arq.Nome;
                RelatorioTemp.ArquivoRpt = arq.FormatoBase64;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox (ex.Message);
                Utils.TraceException (ex);
            }
        }

        public void OnAbrirRelatorioCommand()
        {
            try
            {
                var id = RelatorioSelecionado.RelatorioId;
                _relatorio = _relatorioService.BuscarPelaChave (id);

                var arrayFile = Convert.FromBase64String (_relatorio.ArquivoRpt);
                WpfHelp.ShowRelatorio (arrayFile, "Relatorio " + id, "", "");
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox (ex.Message);
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes Relatorios Gerenciais

        public void OnAdicionarRelatorioGerencialCommand()
        {
            try
            {
                foreach (var x in RelatoriosGerenciais)
                {
                    _relatoriosGerenciaisTemp.Add (x);
                }

                _relatorioGerencialSelectedIndex = RelatorioGerencialSelectedIndex;
                RelatoriosGerenciais.Clear();

                RelatorioGerencialTemp = new RelatorioGerencialView();
                RelatoriosGerenciais.Add (RelatorioGerencialTemp);

                RelatorioGerencialSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarRelatorioGerencialCommand()
        {
            try
            {
                //Atualiza arquivo Byte[] (.rpt)
                RelatorioGerencialSelecionado.ArquivoRpt = RelatorioGerencialTemp.ArquivoRpt;

                var entity = RelatorioGerencialSelecionado;
                var entityConv = Mapper.Map<RelatoriosGerenciais> (entity);

                if (RelatorioGerencialSelecionado.RelatorioId != 0)
                {
                    _auxiliaresService.RelatorioGerencialService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.RelatorioGerencialService.Criar (entityConv);
                }
                CarregaColecaoRelatoriosGerenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirRelatorioGerencialCommand()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = RelatorioGerencialSelecionado; 
                if (entity == null) return;
                var entityConv = Mapper.Map<RelatoriosGerenciais> (entity); 
                _auxiliaresService.RelatorioGerencialService.Remover (entityConv);

                RelatoriosGerenciais.Remove (RelatorioGerencialSelecionado);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnBuscarRelatorioGerencialCommand()
        {
            try
            {
                var filtro = "Crystal Report Files|*.rpt";
                var arq = WpfHelp.UpLoadArquivoDialog (filtro, 0);
                if (arq == null)
                {
                    return;
                }

                RelatorioGerencialTemp.RelatorioId = RelatorioGerencialSelecionado.RelatorioId;
                RelatorioGerencialSelecionado.Nome = arq.Nome;
                RelatorioGerencialTemp.Nome = RelatorioGerencialSelecionado.Nome;
                RelatorioGerencialTemp.NomeArquivoRpt = arq.Nome;
                RelatorioGerencialTemp.ArquivoRpt = arq.FormatoBase64;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox (ex.Message);
                Utils.TraceException (ex);
            }
        }

        public void OnAbrirRelatorioGerencialCommand()
        {
            try
            {
                var id = RelatorioGerencialSelecionado.RelatorioId;
                _relatorioGerencial = _relatorioGerencialService.BuscarPelaChave (id);

                var arrayFile = Convert.FromBase64String (_relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio (arrayFile, "RelatorioGerencial " + id, "", "");
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes LayoutCrachas

        public void OnAdicionarLayoutCrachaCommand()
        {
            try
            {
                foreach (var x in LayoutsCrachas)
                {
                    _layoutsCrachasTemp.Add (x);
                }

                _layoutCrachaSelectedIndex = LayoutCrachaSelectedIndex;
                LayoutsCrachas.Clear();

                LayoutCrachaTemp = new LayoutCrachaView();
                LayoutsCrachas.Add (LayoutCrachaTemp);

                LayoutCrachaSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarLayoutCrachaCommand()
        {
            try
            {
                //Atualiza arquivo Byte[] (.rpt)
                LayoutCrachaSelecionado.LayoutRpt = LayoutCrachaTemp.LayoutRpt;

                var entity = LayoutCrachaSelecionado;
                var entityConv = Mapper.Map<LayoutCracha> (entity);

                if (LayoutCrachaSelecionado.LayoutCrachaId != 0)
                {
                    _auxiliaresService.LayoutCrachaService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.LayoutCrachaService.Criar (entityConv);
                }
               
                CarregaColecaoLayoutsCrachas();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirLayoutCrachaCommand()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var entity = LayoutCrachaSelecionado; 
                if (entity == null) return; 
                var entityConv = Mapper.Map<LayoutCracha>(entity); 
                
                var listaCracha = _serviceEmpresasCracha.ListarLayoutCrachaView(null, entity.LayoutCrachaId);
                if (listaCracha.Count > 0)
                {
                    WpfHelp.Mbox("LayoutCracha não pode ser deletato, ele esta sendo utilizado por Empresa(s).");
                    return;
                }

                _auxiliaresService.LayoutCrachaService.Remover (entityConv);
                //_auxiliaresService.LayoutCrachaService.Remover(entityConv);

                LayoutsCrachas.Remove (LayoutCrachaSelecionado);
                
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnBuscarLayoutCrachaCommand()
        {
            try
            {
                var filtro = "Crystal Report Files|*.rpt";
                var arq = WpfHelp.UpLoadArquivoDialog (filtro, 0);
                if (arq == null)
                {
                    return;
                }

                LayoutCrachaTemp.LayoutCrachaId = LayoutCrachaSelecionado.LayoutCrachaId;
                LayoutCrachaSelecionado.Nome = arq.Nome;

                LayoutCrachaTemp.Nome = LayoutCrachaSelecionado.Nome;
                LayoutCrachaTemp.LayoutRpt = arq.FormatoBase64;
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox (ex.Message);
                Utils.TraceException (ex);
            }
        }

        public void OnAbrirLayoutCrachaCommand()
        {
            try
            {
                var id = LayoutCrachaSelecionado.LayoutCrachaId;

                _layoutCracha = _auxiliaresService.LayoutCrachaService.BuscarPelaChave (id);

                var arrayFile = Convert.FromBase64String (_layoutCracha.LayoutRpt);
                WpfHelp.ShowRelatorio (arrayFile, "LayoutCracha " + id, "", "");
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes TiposEquipamentos

        public void OnAdicionarCommand_TiposEquipamentos()
        {
            try
            {
                foreach (var x in TiposEquipamentos)
                {
                    _tiposEquipamentosTemp.Add (x);
                }

                _tipoEquipamentoSelectedIndex = TipoEquipamentoSelectedIndex;
                TiposEquipamentos.Clear();
                var tipoEquipamento = new TipoEquipamentoView();
                TiposEquipamentos.Add (tipoEquipamento);
                TipoEquipamentoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposEquipamentos()
        {
            try
            {
                var entity = TipoEquipamentoSelecionado;
                var entityConv = Mapper.Map<TipoEquipamento> (entity);

                if (TipoEquipamentoSelecionado.TipoEquipamentoId != 0)
                {
                    _auxiliaresService.TipoEquipamentoService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.TipoEquipamentoService.Criar (entityConv);
                }

                CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TiposEquipamentos()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = TipoEquipamentoSelecionado;
                if (entity == null) return; 
                var entityConv = Mapper.Map<TipoEquipamento> (entity);
                _auxiliaresService.TipoEquipamentoService.Remover (entityConv);

                TiposEquipamentos.Remove (TipoEquipamentoSelecionado);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes TiposAcesso

        public void OnAdicionarCommand_TiposAcesso()
        {
            try
            {
                foreach (var x in TiposAcessos)
                {
                    _tiposAcessosTemp.Add (x);
                }

                _tipoAcessoSelectedIndex = TipoAcessoSelectedIndex;
                TiposAcessos.Clear();
                var acesso = new TipoAcessoView();
                TiposAcessos.Add (acesso);

                TipoAcessoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposAcesso()
        {
            try
            {
                var entity = TipoAcessoSelecionado;
                var entityConv = Mapper.Map<TipoAcesso> (entity);

                if (TipoAcessoSelecionado.TipoAcessoId != 0)
                {
                    _auxiliaresService.TiposAcessoService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.TiposAcessoService.Criar (entityConv);
                }

                CarregaColecaoTiposAcessos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TiposAcesso()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = TipoAcessoSelecionado; 
                if (entity == null) return; 
                var entityConv = Mapper.Map<TipoAcesso> (entity);
                _auxiliaresService.TiposAcessoService.Remover (entityConv);

                CarregaColecaoTiposAcessos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes AreaAcesso

        public void OnAdicionarCommand_AreaAcesso()
        {
            try
            {
                foreach (var x in AreasAcessos)
                {
                    _areaAcessoTemp.Add (x);
                }

                _areaAcessoSelectedIndex = AreaAcessoSelectedIndex;
                AreasAcessos.Clear();
                var areaAcesso = new AreaAcessoView();
                AreasAcessos.Add (areaAcesso);

                AreaAcessoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_AreaAcesso()
        {
            try
            {               

                var entity = AreaAcessoSelecionada;
                var entityConv = Mapper.Map<AreaAcesso> (entity);

                if (AreaAcessoSelecionada.AreaAcessoId != 0)
                {
                    _auxiliaresService.AreaAcessoService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.AreaAcessoService.Criar (entityConv);
                }

                CarregaColecaoAreasAcessos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_AreaAcesso()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove(); 
                if (result != DialogResult.Yes) return; 
                var entity = AreaAcessoSelecionada; 
                if (entity == null) return; 
                var entityConv = Mapper.Map<AreaAcesso> (entity);
                _auxiliaresService.AreaAcessoService.Remover (entityConv);

                AreasAcessos.Remove (AreaAcessoSelecionada);

                CarregaColecaoAreasAcessos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes TiposAtividades

        public void OnAdicionarCommand_TiposAtividades()
        {
            try
            {
                foreach (var x in TiposAtividades)
                {
                    _tiposAtividadesTemp.Add (x);
                }

                _tipoAtividadeSelectedIndex = TipoAtividadeSelectedIndex;
                TiposAtividades.Clear();
                var atividade = new TipoAtividadeView();
                TiposAtividades.Add (atividade);

                TipoAtividadeSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposAtividades()
        {
            try
            {
                var entity = TipoAtividadeSelecionada;
                var entityConv = Mapper.Map<TipoAtividade> (entity);

                if (TipoAtividadeSelecionada.TipoAtividadeId != 0)
                {
                    _auxiliaresService.TipoAtividadeService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.TipoAtividadeService.Criar (entityConv);
                }
               
                CarregaColecaoTiposAtividades();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TiposAtividades()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = TipoAtividadeSelecionada;
                if (entity == null) return;
                var entityConv = Mapper.Map<TipoAtividade> (entity);
                _auxiliaresService.TipoAtividadeService.Remover (entityConv);

                TiposAtividades.Remove (TipoAtividadeSelecionada);
                CarregaColecaoTiposAtividades();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes TiposStatus

        public void OnAdicionarCommand_TipoServico()
        {
            try
            {
                foreach (var x in TipoServico)
                {
                    _tipoServicoTemp.Add (x);
                }

                _tipoServicoSelectedIndex = TipoServicoSelectedIndex;
                TipoServico.Clear();
                var status = new TipoServicoView();
                TipoServico.Add (status);

                TipoServicoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TipoServico()
        {
            try
            {
                var entity = TipoServicoSelecionado;
                var entityConv = Mapper.Map<TipoServico> (entity);

                if (TipoServicoSelecionado.TipoServicoId != 0)
                {
                    _auxiliaresService.TipoServicoService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.TipoServicoService.Criar (entityConv);
                }

                CarregaColecaoTipoServico();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TipoServico()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = TipoServicoSelecionado;
                if (entity == null) return;
                var entityConv = Mapper.Map<TipoServico> (entity);
                _auxiliaresService.TipoServicoService.Remover (entityConv);

                TipoServico.Remove (TipoServicoSelecionado);

                CarregaColecaoTipoServico();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comando dos Botoes TecnologiasCredenciais

        public void OnAdicionarCommand_TecnologiasCredenciais()
        {
            try
            {
                foreach (var x in TecnologiasCredenciais)
                {
                    _tecnologiaCredencialTemp.Add (x);
                }

                _tecnologiaCredencialSelectedIndex = TecnologiaCredencialSelectedIndex;
                TecnologiasCredenciais.Clear();
                var tecnologiaCredencial = new TecnologiaCredencialView();
                TecnologiasCredenciais.Add (tecnologiaCredencial);

                TecnologiaCredencialSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TecnologiasCredenciais()
        {
            try
            {
                var entity = TecnologiaCredencialSelecionada;
                var entityConv = Mapper.Map<TecnologiaCredencial> (entity);

                if (TecnologiaCredencialSelecionada.TecnologiaCredencialId != 0)
                {
                    _auxiliaresService.TecnologiaCredencialService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.TecnologiaCredencialService.Criar (entityConv);
                }

                CarregaColecaoTecnologiasCredenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TecnologiasCredenciais()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return; 
                var entity = TecnologiaCredencialSelecionada; 
                if (entity == null) return; 
                var entityConv = Mapper.Map<TecnologiaCredencial> (entity);
                _auxiliaresService.TecnologiaCredencialService.Remover (entityConv);

                TecnologiasCredenciais.Remove (TecnologiaCredencialSelecionada);

                CarregaColecaoTecnologiasCredenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes TiposCobrancas

        public void OnAdicionarCommand_TiposCobrancas()
        {
            try
            {
                foreach (var x in TiposCobrancas)
                {
                    _tiposCobrancasTemp.Add (x);
                }

                _tipoCobrancaSelectedIndex = TipoCobrancaSelectedIndex;
                TiposCobrancas.Clear();
                var cobranca = new TipoCobrancaView();
                TiposCobrancas.Add (cobranca);

                TipoCobrancaSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposCobrancas()
        {
            try
            {
                var entity = TipoCobrancaSelecionado;
                var entityConv = Mapper.Map<TipoCobranca> (entity);

                if (TipoCobrancaSelecionado.TipoCobrancaId != 0)
                {
                    _auxiliaresService.TipoCobrancaService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.TipoCobrancaService.Criar (entityConv);
                }

                CarregaColecaoTiposCobrancas();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TiposCobrancas()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = TipoCobrancaSelecionado;
                if (entity == null) return; 
                var entityConv = Mapper.Map<TipoCobranca> (entity);
                _auxiliaresService.TipoCobrancaService.Remover (entityConv);

                TiposCobrancas.Remove (TipoCobrancaSelecionado);

                CarregaColecaoTiposCobrancas();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes TiposCursos

        public void OnAdicionarCommand_TiposCursos()
        {
            try
            {
                foreach (var x in Cursos)
                {
                    _cursosTemp.Add (x);
                }

                _cursoSelectedIndex = CursoSelectedIndex;
                Cursos.Clear();
                var cursos = new CursoView();
                Cursos.Add (cursos);

                CursoSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposCursos()
        {
            try
            {
                var entity = CursoSelecionado;
                var entityConv = Mapper.Map<Curso> (entity);

                if (CursoSelecionado.CursoId != 0)
                {
                    _auxiliaresService.CursoService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.CursoService.Criar (entityConv);
                }

                CarregaColecaoCursos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TiposCursos()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = CursoSelecionado; 
                if (entity == null) return;
                var entityConv = Mapper.Map<Curso> (entity);
                _auxiliaresService.CursoService.Remover (entityConv);
                Cursos.Remove (CursoSelecionado);

                CarregaColecaoCursos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comando dos Botões TiposCombustíveis

        public void OnAdicionarCommand_TiposCombustiveis()
        {
            try
            {
                foreach (var x in TiposCombustiveis)
                {
                    _tiposCombustiveisTemp.Add (x);
                }

                _tipoCombustivelSelectedIndex = TipoCombustivelSelectedIndex;

                TiposCombustiveis.Clear();
                var tipoCombustivel = new TipoCombustivelView();
                TiposCombustiveis.Add (tipoCombustivel);

                TipoCombustivelSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposCombustiveis()
        {
            try
            {
                var entity = TipoCombustivelSelecionado;
                var entityConv = Mapper.Map<TipoCombustivel> (entity);

                if (TipoCombustivelSelecionado.TipoCombustivelId != 0)
                {
                    _auxiliaresService.TipoCombustivelService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.TipoCombustivelService.Criar (entityConv);
                }

                CarregaColecaoTipoCombustiveis();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TiposCombustiveis()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = TipoCombustivelSelecionado;
                if (entity == null) return;
                var entityConv = Mapper.Map<TipoCombustivel> (entity);
                _auxiliaresService.TipoCombustivelService.Remover (entityConv);

                TiposCombustiveis.Remove (TipoCombustivelSelecionado);

                CarregaColecaoTipoCombustiveis();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes TiposStatus

        public void OnAdicionarCommand_TiposStatus()
        {
            try
            {
                foreach (var x in TiposStatus)
                {
                    _statusTemp.Add (x);
                }

                _tipoStatusSelectedIndex = TipoStatusSelectedIndex;
                TiposStatus.Clear();
                var status = new StatusView();
                TiposStatus.Add (status);

                TipoStatusSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposStatus()
        {
            try
            {
                var entity = TipoStatusSelecionado;
                var entityConv = Mapper.Map<Status> (entity);

                if (TipoStatusSelecionado.StatusId != 0)
                {
                    _auxiliaresService.StatusService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.StatusService.Criar (entityConv);
                }

                CarregaColecaoStatus();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_TiposStatus()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = TipoStatusSelecionado; 
                if (entity == null) return; 
                var entityConv = Mapper.Map<Status> (entity);
                _auxiliaresService.StatusService.Remover (entityConv);

                TiposStatus.Remove (TipoStatusSelecionado);

                CarregaColecaoStatus();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes CredenciaisStatus

        public void OnAdicionarCommand_CredenciaisStatus()
        {
            try
            {
                foreach (var x in CredenciaisStatus)
                {
                    _credencialStatusTemp.Add (x);
                }

                _credencialStatusSelectedIndex = CredencialStatusSelectedIndex;
                CredenciaisStatus.Clear();
                var credencialStatus = new CredencialStatusView();
                CredenciaisStatus.Add (credencialStatus);

                TipoStatusSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_CredenciaisStatus()
        {
            try
            {
                var entity = CredenciaisStatusSelecionado;
                var entityConv = Mapper.Map<CredencialStatus> (entity);

                if (CredenciaisStatusSelecionado.CredencialStatusId != 0)
                {
                    _auxiliaresService.CredencialStatusService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.CredencialStatusService.Criar (entityConv);
                }

                CarregaColecaoCredenciaisStatus();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_CredenciaisStatus()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = CredenciaisStatusSelecionado;
                if (entity == null) return; 
                var entityConv = Mapper.Map<CredencialStatus> (entity);
                _auxiliaresService.CredencialStatusService.Remover (entityConv);

                CredenciaisStatus.Remove (CredenciaisStatusSelecionado);

                CarregaColecaoCredenciaisStatus();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes CredenciaisMotivos

        public void OnAdicionarCommand_CredenciaisMotivos()
        {
            try
            {
                foreach (var x in CredenciaisMotivos)
                {
                    _credencialMotivoTemp.Add (x);
                }

                _credencialMotivoSelectedIndex = CredencialMotivoSelectedIndex;
                CredenciaisMotivos.Clear();
                var credencialMotivo = new CredencialMotivoView();
                CredenciaisMotivos.Add (credencialMotivo);

                CredencialMotivoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_CredenciaisMotivos()
        {
            try
            {
                var entity = CredencialMotivoSelecionado;
                var entityConv = Mapper.Map<CredencialMotivo> (entity);

                if (CredencialMotivoSelecionado.CredencialMotivoId != 0)
                {
                    _auxiliaresService.CredencialMotivoService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.CredencialMotivoService.Criar (entityConv);
                }

                CarregaColecaoCredenciaisMotivos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_CredenciaisMotivos()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = CredencialMotivoSelecionado;
                if (entity == null) return;
                var entityConv = Mapper.Map<CredencialStatus> (entity);
                _auxiliaresService.CredencialStatusService.Remover (entityConv);

                CredenciaisStatus.Remove (CredenciaisStatusSelecionado);

                CarregaColecaoCredenciaisMotivos();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Comandos dos Botoes FormatosCredenciais

        public void OnAdicionarCommand_FormatosCredenciais()
        {
            try
            {
                foreach (var x in FormatosCredenciais)
                {
                    _formatoCredencialTemp.Add (x);
                }

                _formatoCredencialSelectedIndex = FormatoCredencialSelectedIndex;
                FormatosCredenciais.Clear();
                var formatoCredencial = new FormatoCredencialView();
                FormatosCredenciais.Add (formatoCredencial);

                FormatoCredencialSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand_FormatosCredenciais()
        {
            try
            {
                var entity = FormatoCredencialSelecionado;
                var entityConv = Mapper.Map<FormatoCredencial> (entity);

                if (FormatoCredencialSelecionado.FormatoCredencialId != 0)
                {
                    _auxiliaresService.FormatoCredencialService.Alterar (entityConv);
                }
                else
                {
                    _auxiliaresService.FormatoCredencialService.Criar (entityConv);
                }

                CarregaColecaoFormatosCredenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCommand_FormatosCredenciais()
        {
            try
            {
                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;
                var entity = FormatoCredencialSelecionado; 
                if (entity == null) return; 
                var entityConv = Mapper.Map<FormatoCredencial> (entity);
                _auxiliaresService.FormatoCredencialService.Remover (entityConv);
                FormatosCredenciais.Remove (FormatoCredencialSelecionado);
                CarregaColecaoFormatosCredenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #endregion

        #region Carregamento das Colecoes

        public void CarregaColecaoRelatorios()
        {
            try
            {
                var service = new RelatorioService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<RelatorioView>> (list1);
                var observer = new ObservableCollection<RelatorioView>();
                list2.ForEach (n => { observer.Add (n); });

                Relatorios = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoRelatoriosGerenciais()
        {
            try
            {
                var service = new RelatorioGerencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<RelatorioGerencialView>> (list1);
                var observer = new ObservableCollection<RelatorioGerencialView>();
                list2.ForEach (n => { observer.Add (n); });

                RelatoriosGerenciais = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoLayoutsCrachas()
        {
            try
            {
                var service = new LayoutCrachaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<LayoutCrachaView>> (list1);
                var observer = new ObservableCollection<LayoutCrachaView>();
                list2.ForEach (n => { observer.Add (n); });

                LayoutsCrachas = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoTiposEquipamentos()
        {
            try
            {
                var service = new TipoEquipamentoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoEquipamentoView>> (list1);
                var observer = new ObservableCollection<TipoEquipamentoView>();
                list2.ForEach (n => { observer.Add (n); });

                TiposEquipamentos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoTiposAcessos()
        {
            try
            {
                var service = new TipoAcessoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoAcessoView>> (list1);
                var observer = new ObservableCollection<TipoAcessoView>();
                list2.ForEach (n => { observer.Add (n); });

                TiposAcessos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoAreasAcessos()
        {
            try
            {
                var service = new AreaAcessoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<AreaAcessoView>> (list1);
                var observer = new ObservableCollection<AreaAcessoView>();
                list2.ForEach (n => { observer.Add (n); });

                AreasAcessos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoTiposAtividades()
        {
            try
            {
                var service = new TipoAtividadeService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoAtividadeView>> (list1);
                var observer = new ObservableCollection<TipoAtividadeView>();
                list2.ForEach (n => { observer.Add (n); });

                TiposAtividades = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoTipoServico()
        {
            try
            {
                var service = new TipoServicoService();
               
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoServicoView>> (list1);
                var observer = new ObservableCollection<TipoServicoView>();
                list2.ForEach (n => { observer.Add (n); });

                TipoServico = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoTecnologiasCredenciais()
        {
            try
            {
                var service = new TecnologiaCredencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TecnologiaCredencialView>> (list1);
                var observer = new ObservableCollection<TecnologiaCredencialView>();
                list2.ForEach (n => { observer.Add (n); });

                TecnologiasCredenciais = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoTiposCobrancas()
        {
            try
            {
                var service = new TipoCobrancaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoCobrancaView>> (list1);
                var observer = new ObservableCollection<TipoCobrancaView>();
                list2.ForEach (n => { observer.Add (n); });

                TiposCobrancas = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoCursos()
        {
            try
            {
                var service = new CursoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<CursoView>> (list1);
                var observer = new ObservableCollection<CursoView>();
                list2.ForEach (n => { observer.Add (n); });

                Cursos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoTipoCombustiveis()
        {
            try
            {
                var service = new TipoCombustivelService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoCombustivelView>> (list1);
                var observer = new ObservableCollection<TipoCombustivelView>();
                list2.ForEach (n => { observer.Add (n); });

                TiposCombustiveis = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoStatus()
        {
            try
            {
                var service = new StatusService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<StatusView>> (list1);
                var observer = new ObservableCollection<StatusView>();
                list2.ForEach (n => { observer.Add (n); });

                TiposStatus = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoCredenciaisStatus()
        {
            try
            {
                var service = new CredencialStatusService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<CredencialStatusView>> (list1);
                var observer = new ObservableCollection<CredencialStatusView>();
                list2.ForEach (n => { observer.Add (n); });

                CredenciaisStatus = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoCredenciaisMotivos()
        {
            try
            {
                var service = new CredencialMotivoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<CredencialMotivoView>> (list1);
                var observer = new ObservableCollection<CredencialMotivoView>();
                list2.ForEach (n => { observer.Add (n); });

                CredenciaisMotivos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoFormatosCredenciais()
        {
            try
            {
                var service = new FormatoCredencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<FormatoCredencialView>> (list1);
                var observer = new ObservableCollection<FormatoCredencialView>();
                list2.ForEach (n => { observer.Add (n); });

                FormatosCredenciais = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion
    }
}