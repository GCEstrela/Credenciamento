// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.Infra.Servicos;
using CrystalDecisions.CrystalReports.Engine;
using IMOD.CredenciamentoDeskTop.Funcoes;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.Domain.Constantes;
using Genetec.Sdk;
using IMOD.Infra.Repositorios;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Entities;
using System.Data;
using System.IO;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{

    public class ConfiguracoesViewModel : ViewModelBase
    {

        #region Inicializacao

        public ConfiguracoesViewModel()
        {
            var carregaUiThr = new Thread(() => CarregaUi());
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
            CarregaConfiguracaoSistema();
            CarregaTipoCracha();
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

        //Configuracao
        private readonly IConfiguraSistemaService _serviceConfiguracoesSistema = new ConfiguraSistemaService();
        private ObservableCollection<ConfiguraSistemaView> _congiracaoSistema;
        private ConfiguraSistemaView _configuracaosistemaSelecionado;
        private int _configuracaosistemaSelectedIndex;
        public ConfiguraSistemaView Entity { get; set; }
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
                OnPropertyChanged("SelectedIndex");
            }
        }
        public ObservableCollection<ConfiguraSistemaView> CongiracaoSistema
        {
            get { return _congiracaoSistema; }

            set
            {
                if (_congiracaoSistema != value)
                {
                    _congiracaoSistema = value;
                    OnPropertyChanged();
                }
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("RelatorioSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("RelatorioGerencialSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("LayoutCrachaSelectedIndex");
            }
        }

        /// <summary>
        ///     Tipo Layout Crachá
        /// </summary>
        public IEnumerable<object> TipoLayoutCracha { get; set; }

        public TipoLayoutCracha TipoLayoutCrachaSelecionado { get; set; }



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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("TipoEquipamentoSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("TipoAcessoSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("AreaAcessoSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("TipoAtividadeSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("TipoServicoSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("TecnologiaCredencialSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("TipoCobrancaSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("CursoSelectedIndex");
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
        public ConfiguraSistemaView ConfiguraSistemaSelecionado
        {
            get { return _configuracaosistemaSelecionado; }

            set
            {
                _configuracaosistemaSelecionado = value;
                OnPropertyChanged("SelectedItem");
                if (_configuracaosistemaSelecionado != null)
                {
                }
            }
        }
        public TipoCombustivelView TipoCombustivelSelecionado
        {
            get { return _tipoCombustivelSelecionado; }

            set
            {
                _tipoCombustivelSelecionado = value;
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("TipoCombustivelSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("TipoStatusSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
                if (_credencialStatusSelecionado != null)
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
                OnPropertyChanged("CredencialStatusSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("CredencialMotivoSelectedIndex");
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
                OnPropertyChanged("SelectedItem");
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
                OnPropertyChanged("FormatoCredencialSelectedIndex");
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
                    _relatoriosTemp.Add(x);
                }

                _relatorioSelectedIndex = RelatorioSelectedIndex;
                Relatorios.Clear();

                RelatorioTemp = new RelatorioView();
                Relatorios.Add(RelatorioTemp);

                RelatorioSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarRelatorioCommand()
        {
            try
            {
                //Atualiza arquivo Byte[] (.rpt)
                RelatorioSelecionado.ArquivoRpt = RelatorioTemp.ArquivoRpt;

                var entity = RelatorioSelecionado;
                var entityConv = Mapper.Map<Relatorios>(entity);

                if (RelatorioSelecionado.RelatorioId != 0)
                {
                    _auxiliaresService.RelatorioService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.RelatorioService.Criar(entityConv);
                }

                CarregaColecaoRelatorios();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<Relatorios>(entity);
                _auxiliaresService.RelatorioService.Remover(entityConv);

                Relatorios.Remove(RelatorioSelecionado);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnBuscarRelatorioCommand()
        {
            try
            {
                var filtro = "Crystal Report Files|*.rpt";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 0);
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
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        public void OnAbrirRelatorioCommand()
        {
            try
            {
                var id = RelatorioSelecionado.RelatorioId;
                _relatorio = _relatorioService.BuscarPelaChave(id);

                var arrayFile = Convert.FromBase64String(_relatorio.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "Relatorio " + id, "", "");
            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
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
                    _relatoriosGerenciaisTemp.Add(x);
                }

                _relatorioGerencialSelectedIndex = RelatorioGerencialSelectedIndex;
                RelatoriosGerenciais.Clear();

                RelatorioGerencialTemp = new RelatorioGerencialView();
                RelatoriosGerenciais.Add(RelatorioGerencialTemp);

                RelatorioGerencialSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarRelatorioGerencialCommand()
        {
            try
            {
                //Atualiza arquivo Byte[] (.rpt)
                RelatorioGerencialSelecionado.ArquivoRpt = RelatorioGerencialTemp.ArquivoRpt;

                var entity = RelatorioGerencialSelecionado;
                var entityConv = Mapper.Map<RelatoriosGerenciais>(entity);

                if (RelatorioGerencialSelecionado.RelatorioId != 0)
                {
                    _auxiliaresService.RelatorioGerencialService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.RelatorioGerencialService.Criar(entityConv);
                }
                CarregaColecaoRelatoriosGerenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<RelatoriosGerenciais>(entity);
                _auxiliaresService.RelatorioGerencialService.Remover(entityConv);

                RelatoriosGerenciais.Remove(RelatorioGerencialSelecionado);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnBuscarRelatorioGerencialCommand()
        {
            try
            {
                var filtro = "Crystal Report Files|*.rpt";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 0);
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
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        public void OnAbrirRelatorioGerencialCommand()
        {
            try
            {
                var id = RelatorioGerencialSelecionado.RelatorioId;
                _relatorioGerencial = _relatorioGerencialService.BuscarPelaChave(id);

                var arrayFile = Convert.FromBase64String(_relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioGerencial " + id, "", "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #region Comandos dos Botoes LayoutCrachas

        public void OnAdicionarLayoutCrachaCommand()
        {
            //try
            //{
            //    foreach (var x in LayoutsCrachas)
            //    {
            //        _layoutsCrachasTemp.Add (x);
            //    }

            //    _layoutCrachaSelectedIndex = LayoutCrachaSelectedIndex;
            //    LayoutsCrachas.Clear();

            //    LayoutCrachaTemp = new LayoutCrachaView();
            //    LayoutsCrachas.Add (LayoutCrachaTemp);

            //    LayoutCrachaSelectedIndex = 0;
            //}
            //catch (Exception ex)
            //{
            //    Utils.TraceException (ex);
            //}
        }

        public void OnSalvarLayoutCrachaCommand()
        {
            try
            {

                //Atualiza arquivo Byte[] (.rpt)
                LayoutCrachaSelecionado.LayoutRpt = LayoutCrachaTemp.LayoutRpt;
                //LayoutCrachaSelecionado.TipoCracha = TipoLayoutCracha.

                if (LayoutCrachaSelecionado.TipoCracha <= 0) return;

                var entity = LayoutCrachaSelecionado;
                var entityConv = Mapper.Map<LayoutCracha>(entity);

                if (LayoutCrachaSelecionado.LayoutCrachaId != 0)
                {
                    _auxiliaresService.LayoutCrachaService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.LayoutCrachaService.Criar(entityConv);
                }

                CarregaColecaoLayoutsCrachas();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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

                _auxiliaresService.LayoutCrachaService.Remover(entityConv);
                //_auxiliaresService.LayoutCrachaService.Remover(entityConv);

                LayoutsCrachas.Remove(LayoutCrachaSelecionado);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnBuscarLayoutCrachaCommand()
        {
            try
            {
                var filtro = "Crystal Report Files|*.rpt";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 0);
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
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        public void OnAbrirLayoutCrachaCommand()
        {
            try
            {
                var id = LayoutCrachaSelecionado.LayoutCrachaId;

                _layoutCracha = _auxiliaresService.LayoutCrachaService.BuscarPelaChave(id);

                var arrayFile = Convert.FromBase64String(_layoutCracha.LayoutRpt);
                WpfHelp.ShowRelatorio(arrayFile, "LayoutCracha " + id, "", "");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _tiposEquipamentosTemp.Add(x);
                }

                _tipoEquipamentoSelectedIndex = TipoEquipamentoSelectedIndex;
                TiposEquipamentos.Clear();
                var tipoEquipamento = new TipoEquipamentoView();
                TiposEquipamentos.Add(tipoEquipamento);
                TipoEquipamentoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposEquipamentos()
        {
            try
            {
                var entity = TipoEquipamentoSelecionado;
                var entityConv = Mapper.Map<TipoEquipamento>(entity);

                if (TipoEquipamentoSelecionado.TipoEquipamentoId != 0)
                {
                    _auxiliaresService.TipoEquipamentoService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.TipoEquipamentoService.Criar(entityConv);
                }

                CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<TipoEquipamento>(entity);
                _auxiliaresService.TipoEquipamentoService.Remover(entityConv);

                TiposEquipamentos.Remove(TipoEquipamentoSelecionado);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _tiposAcessosTemp.Add(x);
                }

                _tipoAcessoSelectedIndex = TipoAcessoSelectedIndex;
                TiposAcessos.Clear();
                var acesso = new TipoAcessoView();
                TiposAcessos.Add(acesso);

                TipoAcessoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposAcesso()
        {
            try
            {
                var entity = TipoAcessoSelecionado;
                var entityConv = Mapper.Map<TipoAcesso>(entity);

                if (TipoAcessoSelecionado.TipoAcessoId != 0)
                {
                    _auxiliaresService.TiposAcessoService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.TiposAcessoService.Criar(entityConv);
                }

                CarregaColecaoTiposAcessos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<TipoAcesso>(entity);
                _auxiliaresService.TiposAcessoService.Remover(entityConv);

                CarregaColecaoTiposAcessos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _areaAcessoTemp.Add(x);
                }

                _areaAcessoSelectedIndex = AreaAcessoSelectedIndex;
                AreasAcessos.Clear();
                var areaAcesso = new AreaAcessoView();
                AreasAcessos.Add(areaAcesso);

                AreaAcessoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_AreaAcesso()
        {
            try
            {

                var entity = AreaAcessoSelecionada;
                var entityConv = Mapper.Map<AreaAcesso>(entity);

                if (AreaAcessoSelecionada.AreaAcessoId != 0)
                {
                    _auxiliaresService.AreaAcessoService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.AreaAcessoService.Criar(entityConv);
                }

                CarregaColecaoAreasAcessos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<AreaAcesso>(entity);
                _auxiliaresService.AreaAcessoService.Remover(entityConv);

                AreasAcessos.Remove(AreaAcessoSelecionada);

                CarregaColecaoAreasAcessos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _tiposAtividadesTemp.Add(x);
                }

                _tipoAtividadeSelectedIndex = TipoAtividadeSelectedIndex;
                TiposAtividades.Clear();
                var atividade = new TipoAtividadeView();
                TiposAtividades.Add(atividade);

                TipoAtividadeSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void OnSalvarEdicaoCommand_ConfiguracoesSistema()
        {
            try
            {
                var entity = Entity;
                var entityConv = Mapper.Map<ConfiguraSistema>(entity);
                EstrelaEncryparDecrypitar.Variavel.key = Constante.CRIPTO_KEY;
                EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
                entityConv.EmailSenha = ESTRELA_EMCRYPTAR.EstrelaEncrypt(entity.EmailSenha);
                _serviceConfiguracoesSistema.Alterar(entityConv);

                CarregaConfiguracaoSistema();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void OnExcluirRegistroLogCommand_ConfiguracoesSistema()
        {
            try
            {
                var entity = Entity;
                var entityConv = Mapper.Map<ConfiguraSistema>(entity);                
                _serviceConfiguracoesSistema.Remover(entityConv);

                CarregaConfiguracaoSistema();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private IEngine _sdknew;
        public void CredencialGenetecService(IEngine sdk)
        {
            _sdknew = sdk;
        }
        public void AtualizarCustomFieldCarHolderExistentes()
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                ColaboradorEmpresaRepositorio colaboradorEmpresaService = new ColaboradorEmpresaRepositorio();
                CredencialGenetecService(Main.Engine);
                EntityConfigurationQuery query;
                QueryCompletedEventArgs result;
                query = _sdknew.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
                query.EntityTypeFilter.Add(EntityType.Cardholder);
                query.NameSearchMode = StringSearchMode.StartsWith;
                result = query.Query();
                SystemConfiguration systemConfiguration = _sdknew.GetEntity(SdkGuids.SystemConfiguration) as SystemConfiguration;
                var service = systemConfiguration.CustomFieldService;
                var cardHolderList = new List<Cardholder>();
                if (result.Success)
                {
                    foreach (DataRow dr in result.Data.Rows)
                    {
                        Cardholder cardholder = _sdknew.GetEntity((Guid)dr[0]) as Cardholder;
                        cardHolderList.Add(cardholder);
                    }
                }

                var cardHolderListDuplicado = new List<string>();
                //ColaboradorEmpresa cardholderBanco;
                var cardholderBancoList = colaboradorEmpresaService.BuscarListaIntegracao();
                foreach (ColaboradorEmpresa colaboradorEmpresa in cardholderBancoList)
                {
                    string nomeDB = colaboradorEmpresa.ColaboradorNome;
                    var cardholderEncontrados = (List<Cardholder>)cardHolderList.Where(c => c.FirstName == nomeDB).ToList();
                    if (cardholderEncontrados.Count == 1)
                    {
                        cardholderEncontrados[0].CustomFields["Matricula"] = colaboradorEmpresa.Matricula;
                        //cardholderEncontrados[0].CustomFields["Cpf"] = colaboradorEmpresa.Cpf;
                        cardholderEncontrados[0].CustomFields["Cargo"] = colaboradorEmpresa.Cargo;
                        //cardholderEncontrados[0].CustomFields["Empresa"] = colaboradorEmpresa.Empresa;
                        //cardholderEncontrados[0].CustomFields["Identificador"] = colaboradorEmpresa.Identificador;
                    }
                    else
                    {
                        //cardHolderListDuplicado.Add(cardholderEncontrados[0].FirstName);
                        CriarLog("Nome..: " + cardholderEncontrados[0].FirstName + "Quantidade..:" + cardholderEncontrados.Count);


                    }


                    //_sdknew.TransactionManager.CreateTransaction();
                    //Cardholder cardholderNew = _sdknew.CreateEntity(nomeDB,EntityType.Cardholder) as Cardholder;
                    //cardholderNew.FirstName = nomeDB;
                    ////cardholderNew.LastName = "";
                    //_sdknew.TransactionManager.CommitTransaction();

                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                throw ex;
            }
        }
        private void CriarLog(object state)
        {
            try
            {
                StreamWriter vWriter = new StreamWriter(@"C:\Tmp\logCardHolderDuplicado.txt", true);
                vWriter.WriteLine(state.ToString());
                vWriter.Flush();
                vWriter.Close();
            }
            catch { }

        }
        public void OnSalvarEdicaoCommand_TiposAtividades()
        {
            try
            {
                var entity = TipoAtividadeSelecionada;
                var entityConv = Mapper.Map<TipoAtividade>(entity);

                if (TipoAtividadeSelecionada.TipoAtividadeId != 0)
                {
                    _auxiliaresService.TipoAtividadeService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.TipoAtividadeService.Criar(entityConv);
                }

                CarregaColecaoTiposAtividades();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<TipoAtividade>(entity);
                _auxiliaresService.TipoAtividadeService.Remover(entityConv);

                TiposAtividades.Remove(TipoAtividadeSelecionada);
                CarregaColecaoTiposAtividades();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _tipoServicoTemp.Add(x);
                }

                _tipoServicoSelectedIndex = TipoServicoSelectedIndex;
                TipoServico.Clear();
                var status = new TipoServicoView();
                TipoServico.Add(status);

                TipoServicoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_TipoServico()
        {
            try
            {
                var entity = TipoServicoSelecionado;
                var entityConv = Mapper.Map<TipoServico>(entity);

                if (TipoServicoSelecionado.TipoServicoId != 0)
                {
                    _auxiliaresService.TipoServicoService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.TipoServicoService.Criar(entityConv);
                }

                CarregaColecaoTipoServico();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<TipoServico>(entity);
                _auxiliaresService.TipoServicoService.Remover(entityConv);

                TipoServico.Remove(TipoServicoSelecionado);

                CarregaColecaoTipoServico();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _tecnologiaCredencialTemp.Add(x);
                }

                _tecnologiaCredencialSelectedIndex = TecnologiaCredencialSelectedIndex;
                TecnologiasCredenciais.Clear();
                var tecnologiaCredencial = new TecnologiaCredencialView();
                TecnologiasCredenciais.Add(tecnologiaCredencial);

                TecnologiaCredencialSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_TecnologiasCredenciais()
        {
            try
            {
                var entity = TecnologiaCredencialSelecionada;
                var entityConv = Mapper.Map<TecnologiaCredencial>(entity);

                if (TecnologiaCredencialSelecionada.TecnologiaCredencialId != 0)
                {
                    _auxiliaresService.TecnologiaCredencialService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.TecnologiaCredencialService.Criar(entityConv);
                }

                CarregaColecaoTecnologiasCredenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<TecnologiaCredencial>(entity);
                _auxiliaresService.TecnologiaCredencialService.Remover(entityConv);

                TecnologiasCredenciais.Remove(TecnologiaCredencialSelecionada);

                CarregaColecaoTecnologiasCredenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _tiposCobrancasTemp.Add(x);
                }

                _tipoCobrancaSelectedIndex = TipoCobrancaSelectedIndex;
                TiposCobrancas.Clear();
                var cobranca = new TipoCobrancaView();
                TiposCobrancas.Add(cobranca);

                TipoCobrancaSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposCobrancas()
        {
            try
            {
                var entity = TipoCobrancaSelecionado;
                var entityConv = Mapper.Map<TipoCobranca>(entity);

                if (TipoCobrancaSelecionado.TipoCobrancaId != 0)
                {
                    _auxiliaresService.TipoCobrancaService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.TipoCobrancaService.Criar(entityConv);
                }

                CarregaColecaoTiposCobrancas();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<TipoCobranca>(entity);
                _auxiliaresService.TipoCobrancaService.Remover(entityConv);

                TiposCobrancas.Remove(TipoCobrancaSelecionado);

                CarregaColecaoTiposCobrancas();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _cursosTemp.Add(x);
                }

                _cursoSelectedIndex = CursoSelectedIndex;
                Cursos.Clear();
                var cursos = new CursoView();
                Cursos.Add(cursos);

                CursoSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposCursos()
        {
            try
            {
                var entity = CursoSelecionado;
                var entityConv = Mapper.Map<Curso>(entity);

                if (CursoSelecionado.CursoId != 0)
                {
                    _auxiliaresService.CursoService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.CursoService.Criar(entityConv);
                }

                CarregaColecaoCursos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<Curso>(entity);
                _auxiliaresService.CursoService.Remover(entityConv);
                Cursos.Remove(CursoSelecionado);

                CarregaColecaoCursos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _tiposCombustiveisTemp.Add(x);
                }

                _tipoCombustivelSelectedIndex = TipoCombustivelSelectedIndex;

                TiposCombustiveis.Clear();
                var tipoCombustivel = new TipoCombustivelView();
                TiposCombustiveis.Add(tipoCombustivel);

                TipoCombustivelSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposCombustiveis()
        {
            try
            {
                var entity = TipoCombustivelSelecionado;
                var entityConv = Mapper.Map<TipoCombustivel>(entity);

                if (TipoCombustivelSelecionado.TipoCombustivelId != 0)
                {
                    _auxiliaresService.TipoCombustivelService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.TipoCombustivelService.Criar(entityConv);
                }

                CarregaColecaoTipoCombustiveis();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<TipoCombustivel>(entity);
                _auxiliaresService.TipoCombustivelService.Remover(entityConv);

                TiposCombustiveis.Remove(TipoCombustivelSelecionado);

                CarregaColecaoTipoCombustiveis();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _statusTemp.Add(x);
                }

                _tipoStatusSelectedIndex = TipoStatusSelectedIndex;
                TiposStatus.Clear();
                var status = new StatusView();
                TiposStatus.Add(status);

                TipoStatusSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_TiposStatus()
        {
            try
            {
                var entity = TipoStatusSelecionado;
                var entityConv = Mapper.Map<Status>(entity);

                if (TipoStatusSelecionado.StatusId != 0)
                {
                    _auxiliaresService.StatusService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.StatusService.Criar(entityConv);
                }

                CarregaColecaoStatus();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<Status>(entity);
                _auxiliaresService.StatusService.Remover(entityConv);

                TiposStatus.Remove(TipoStatusSelecionado);

                CarregaColecaoStatus();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _credencialStatusTemp.Add(x);
                }

                _credencialStatusSelectedIndex = CredencialStatusSelectedIndex;
                CredenciaisStatus.Clear();
                var credencialStatus = new CredencialStatusView();
                CredenciaisStatus.Add(credencialStatus);

                CredencialStatusSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_CredenciaisStatus()
        {
            try
            {
                var entity = CredenciaisStatusSelecionado;
                if (entity == null) return;
                var entityConv = Mapper.Map<CredencialStatus>(entity);

                if (CredenciaisStatusSelecionado.CredencialStatusId != 0)
                {
                    _auxiliaresService.CredencialStatusService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.CredencialStatusService.Criar(entityConv);
                }

                CarregaColecaoCredenciaisStatus();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<CredencialStatus>(entity);
                _auxiliaresService.CredencialStatusService.Remover(entityConv);

                CredenciaisStatus.Remove(CredenciaisStatusSelecionado);

                CarregaColecaoCredenciaisStatus();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _credencialMotivoTemp.Add(x);
                }

                _credencialMotivoSelectedIndex = CredencialMotivoSelectedIndex;
                CredenciaisMotivos.Clear();
                var credencialMotivo = new CredencialMotivoView();
                CredenciaisMotivos.Add(credencialMotivo);

                CredencialMotivoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_CredenciaisMotivos()
        {
            try
            {
                var entity = CredencialMotivoSelecionado;
                var entityConv = Mapper.Map<CredencialMotivo>(entity);

                if (CredencialMotivoSelecionado.CredencialMotivoId != 0)
                {
                    _auxiliaresService.CredencialMotivoService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.CredencialMotivoService.Criar(entityConv);
                }

                CarregaColecaoCredenciaisMotivos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<CredencialStatus>(entity);
                _auxiliaresService.CredencialStatusService.Remover(entityConv);

                CredenciaisStatus.Remove(CredenciaisStatusSelecionado);

                CarregaColecaoCredenciaisMotivos();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                    _formatoCredencialTemp.Add(x);
                }

                _formatoCredencialSelectedIndex = FormatoCredencialSelectedIndex;
                FormatosCredenciais.Clear();
                var formatoCredencial = new FormatoCredencialView();
                FormatosCredenciais.Add(formatoCredencial);

                FormatoCredencialSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand_FormatosCredenciais()
        {
            try
            {
                var entity = FormatoCredencialSelecionado;
                var entityConv = Mapper.Map<FormatoCredencial>(entity);

                if (FormatoCredencialSelecionado.FormatoCredencialId != 0)
                {
                    _auxiliaresService.FormatoCredencialService.Alterar(entityConv);
                }
                else
                {
                    _auxiliaresService.FormatoCredencialService.Criar(entityConv);
                }

                CarregaColecaoFormatosCredenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                var entityConv = Mapper.Map<FormatoCredencial>(entity);
                _auxiliaresService.FormatoCredencialService.Remover(entityConv);
                FormatosCredenciais.Remove(FormatoCredencialSelecionado);
                CarregaColecaoFormatosCredenciais();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #endregion

        #region Carregamento das Colecoes

        public void CarregaConfiguracaoSistema()
        {
            try
            {

                var service = new ConfiguraSistemaService();
                var list1 = service.Listar().ToList().FirstOrDefault();
                var list2 = Mapper.Map<ConfiguraSistemaView>(list1);
                Entity = list2;

                EstrelaEncryparDecrypitar.Variavel.key = Constante.CRIPTO_KEY;
                EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
                if (Entity != null)
                {
                    Entity.EmailSenha = ESTRELA_EMCRYPTAR.EstrelaDecrypt(Entity.EmailSenha);
                }

                var serviceSC = new CredencialGenetecService(Main.Engine);
                var grupos = serviceSC.RetornarGrupos();
                grupos.ForEach(n => { Entity.Grupos.Add(n.Name); });
                this.Entity.GrupoPadrao = list2.GrupoPadrao.Trim();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoRelatorios()
        {
            try
            {
                var service = new RelatorioService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<RelatorioView>>(list1);
                var observer = new ObservableCollection<RelatorioView>();
                list2.ForEach(n => { observer.Add(n); });

                Relatorios = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoRelatoriosGerenciais()
        {
            try
            {
                var service = new RelatorioGerencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<RelatorioGerencialView>>(list1);
                var observer = new ObservableCollection<RelatorioGerencialView>();
                list2.ForEach(n => { observer.Add(n); });

                RelatoriosGerenciais = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoLayoutsCrachas()
        {
            try
            {
                var service = new LayoutCrachaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<LayoutCrachaView>>(list1);
                var observer = new ObservableCollection<LayoutCrachaView>();
                list2.ForEach(n => { observer.Add(n); });

                LayoutsCrachas = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void BuscarAnexo(int LayoutCrachaID)
        {
            try
            {
                var service = new LayoutCrachaService();
                var anexo = service.BuscarPelaChave(LayoutCrachaID);
                //Entity.Arquivo = anexo.Arquivo;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
        public void CarregaColecaoTiposEquipamentos()
        {
            try
            {
                var service = new TipoEquipamentoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoEquipamentoView>>(list1);
                var observer = new ObservableCollection<TipoEquipamentoView>();
                list2.ForEach(n => { observer.Add(n); });

                TiposEquipamentos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoTiposAcessos()
        {
            try
            {
                var service = new TipoAcessoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoAcessoView>>(list1);
                var observer = new ObservableCollection<TipoAcessoView>();
                list2.ForEach(n => { observer.Add(n); });

                TiposAcessos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoAreasAcessos()
        {
            try
            {
                var service = new AreaAcessoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<AreaAcessoView>>(list1);
                var observer = new ObservableCollection<AreaAcessoView>();
                list2.ForEach(n => { observer.Add(n); });

                AreasAcessos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoTiposAtividades()
        {
            try
            {
                var service = new TipoAtividadeService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoAtividadeView>>(list1);
                var observer = new ObservableCollection<TipoAtividadeView>();
                list2.ForEach(n => { observer.Add(n); });

                TiposAtividades = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoTipoServico()
        {
            try
            {
                var service = new TipoServicoService();

                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoServicoView>>(list1);
                var observer = new ObservableCollection<TipoServicoView>();
                list2.ForEach(n => { observer.Add(n); });

                TipoServico = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoTecnologiasCredenciais()
        {
            try
            {
                var service = new TecnologiaCredencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TecnologiaCredencialView>>(list1);
                var observer = new ObservableCollection<TecnologiaCredencialView>();
                list2.ForEach(n => { observer.Add(n); });

                TecnologiasCredenciais = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoTiposCobrancas()
        {
            try
            {
                var service = new TipoCobrancaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoCobrancaView>>(list1);
                var observer = new ObservableCollection<TipoCobrancaView>();
                list2.ForEach(n => { observer.Add(n); });

                TiposCobrancas = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoCursos()
        {
            try
            {
                var service = new CursoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<CursoView>>(list1);
                var observer = new ObservableCollection<CursoView>();
                list2.ForEach(n => { observer.Add(n); });

                Cursos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoTipoCombustiveis()
        {
            try
            {
                var service = new TipoCombustivelService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoCombustivelView>>(list1);
                var observer = new ObservableCollection<TipoCombustivelView>();
                list2.ForEach(n => { observer.Add(n); });

                TiposCombustiveis = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoStatus()
        {
            try
            {
                var service = new StatusService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<StatusView>>(list1);
                var observer = new ObservableCollection<StatusView>();
                list2.ForEach(n => { observer.Add(n); });

                TiposStatus = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoCredenciaisStatus()
        {
            try
            {
                var service = new CredencialStatusService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<CredencialStatusView>>(list1);
                var observer = new ObservableCollection<CredencialStatusView>();
                list2.ForEach(n => { observer.Add(n); });

                CredenciaisStatus = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoCredenciaisMotivos()
        {
            try
            {
                var service = new CredencialMotivoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<CredencialMotivoView>>(list1);
                var observer = new ObservableCollection<CredencialMotivoView>();
                list2.ForEach(n => { observer.Add(n); });

                CredenciaisMotivos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoFormatosCredenciais()
        {
            try
            {
                var service = new FormatoCredencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<FormatoCredencialView>>(list1);
                var observer = new ObservableCollection<FormatoCredencialView>();
                list2.ForEach(n => { observer.Add(n); });

                FormatosCredenciais = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaTipoCracha()
        {
            var lstResultado = Helper.EnumToListObject<TipoLayoutCracha>();

            TipoLayoutCracha = lstResultado;
        }
        private Domain.Entities.ConfiguraSistema _configuraSistema;
        public void VisualizarCrach(LayoutCrachaView rpt)
        {
            try
            {
                var layoutCracha = _auxiliaresService.LayoutCrachaService.BuscarPelaChave(rpt.LayoutCrachaId);
                if (string.IsNullOrEmpty(layoutCracha.LayoutRpt))
                {
                    WpfHelp.PopupBox("Layout do Crachá não Encontrado !", 1);
                    return;
                }

                if (Constante.CREDENCIAL.Equals(layoutCracha.TipoCracha))
                {
                    GerarCredencialModelo(rpt, layoutCracha);
                }
                else
                {
                    GerarAutorizacaoModelo(rpt, layoutCracha);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GerarCredencialModelo(LayoutCrachaView rpt, LayoutCracha layoutCracha)
        {
            IColaboradorCredencialService _service = new ColaboradorCredencialService();
            var arquivoStr = rpt.LayoutRpt;
            var arrBytes = Convert.FromBase64String(arquivoStr);
            var relatorio = WpfHelp.ShowRelatorioCrystalReport(arrBytes, layoutCracha.Nome);
            IMOD.Domain.EntitiesCustom.ColaboradoresCredenciaisView colaboradorcrdencialteste = new IMOD.Domain.EntitiesCustom.ColaboradoresCredenciaisView();
            //var colaboradorcrdencialteste =_service.BuscarCredencialPelaChave(5081);// 5081
            colaboradorcrdencialteste.ColaboradorCredencialId = 0000;
            colaboradorcrdencialteste.ColaboradorNome = "Colaborador teste";
            colaboradorcrdencialteste.ColaboradorPrivilegio1Id = 1;
            colaboradorcrdencialteste.ColaboradorPrivilegio2Id = 2;
            colaboradorcrdencialteste.Colete = "COLETE";
            TextObject txtRNE = (TextObject)relatorio.ReportDefinition.ReportObjects["Text1"];
            txtRNE.Text = "RG:";
            TextObject txt_RG_RNE = (TextObject)relatorio.ReportDefinition.ReportObjects["obj_RG_RNE"];
            txt_RG_RNE.Text = "9999999999";

            var colaboradorCursosCracha = _auxiliaresService.ColaboradorCursoService.ListarView(null, null, true);
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
            _configuraSistema = ObterConfiguracao();

            var lst = new List<CredencialViewCracha>();
            var c1 = new CredencialViewCracha();
            c1.CrachaCursos = _cursosCracha;
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
            c1.ColaboradorNome = "COLABORADOR NOME";
            c1.Matricula = "0000";
            c1.ColaboradorApelido = "APELIDO";
            c1.EmpresaNome = "ESTRELA SISTEMAS ELETRÔNICOS";
            c1.EmpresaApelido = "ESTRELA";
            c1.TelefoneEmergencia = "(71) 9999-9999";
            c1.RG = "";
            c1.RG = "9999999999";
            c1.CPF = "999.999.999-99";
            c1.RGOrgUF = "BA";
            c1.Colete = "COLETE";
            c1.Motorista = true;
            c1.OperadorPonteEmbarque = true;
            c1.ManuseioBagagem = true;
            c1.FlagCcam = true;
            c1.CNHCategoria = "B";
            c1.Cargo = "CARGO DO COLABORADOR";
            c1.Identificacao1 = "R1";
            c1.Identificacao2 = "T";
            c1.Validade = DateTime.Now.Date;
            c1.Foto2 = obterFoto();
            c1.Logo2 = obterLogo();

            lst.Add(c1);
            relatorio.SetDataSource(lst);

            var objCode = new QrCode();
            string querySistema = _configuraSistema.UrlSistema?.Trim().ToString() + "/Colaborador/Credential/"
                                            + Helpers.Helper.Encriptar(c1.ColaboradorCredencialID.ToString());

            var pathImagem = objCode.GerarQrCode(querySistema, "QrCodeAutorizacao" + c1.ColaboradorCredencialID.ToString() + ".png");
            relatorio.SetParameterValue("PathImgQrCode", pathImagem);

            //IDENTIFICACAO
            var popupCredencial = new PopupCredencial(relatorio, _service, colaboradorcrdencialteste, layoutCracha, false);
            popupCredencial.ShowDialog();
        }

        private void GerarAutorizacaoModelo(LayoutCrachaView rpt, LayoutCracha layoutCracha)
        {
            IVeiculoCredencialService _service = new VeiculoCredencialService();
            var arquivoStr = rpt.LayoutRpt;
            var arrBytes = Convert.FromBase64String(arquivoStr);
            var relatorio = WpfHelp.ShowRelatorioCrystalReport(arrBytes, layoutCracha.Nome);
            IMOD.Domain.EntitiesCustom.VeiculosCredenciaisView veiculoCredencialModelo = new IMOD.Domain.EntitiesCustom.VeiculosCredenciaisView();
            veiculoCredencialModelo.VeiculoCredencialId = 0000;
            veiculoCredencialModelo.VeiculoNome = "Veículo Modelo";
            veiculoCredencialModelo.VeiculoPrivilegio1Id = 1;
            veiculoCredencialModelo.VeiculoPrivilegio2Id = 2;
            veiculoCredencialModelo.Colete = "COLETE";
                        
            
            _configuraSistema = ObterConfiguracao();

            var lst = new List<AutorizacaoView>();
            //var credencialViewPP = _service.ObterCredencialView(5081);//5081
            //CredencialView credencialView = new CredencialView();                

            var a1 = new AutorizacaoView();
            a1.AreaManobra = true;
            a1.Categoria = "B";
            a1.Cor = "Preta";
            a1.Emissao = DateTime.Now.Date.AddDays(-1);
            a1.EmpresaNome = "ESTRELA LTDA.";
            a1.Frota = "1234-6";
            a1.Identificacao1 = "R3";
            a1.Identificacao2 = "T";
            a1.Lacre = "B032254";
            a1.Marca = "FIAT";
            a1.Modelo = "DOBLÔ";
            a1.PlacaIdentificador = "ABC 1324";
            a1.Portao = "1 - 3 - 7";
            a1.SerieChassi = "45566332211";
            a1.Validade = DateTime.Now.Date.AddDays(3);
            a1.VeiculoCredencialId = 0000;
            a1.Registro = "1234-5";
            a1.TipoServico = "Transporte Interno";
            a1.Logo2 = obterLogo();
            lst.Add(a1);
            relatorio.SetDataSource(lst);

            var objCode = new QrCode();
            string querySistema = _configuraSistema.UrlSistema?.Trim().ToString() + "/Colaborador/Credential/"
                                            + Helpers.Helper.Encriptar(a1.VeiculoCredencialId.ToString());

            var pathImagem = objCode.GerarQrCode(querySistema, "QrCodeAutorizacao" + a1.VeiculoCredencialId.ToString() + ".png");
            relatorio.SetParameterValue("PathImgQrCode", pathImagem);

            //IDENTIFICACAO
            var popupCredencial = new PopupAutorizacao(relatorio, _service, veiculoCredencialModelo, layoutCracha);
            popupCredencial.ShowDialog();
        }

        private byte[] obterFoto()
        {
            byte[] foto = Convert.FromBase64String("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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=");
            return foto;
        }
        private byte[] obterLogo()
        {
            byte[] logo = Convert.FromBase64String("Qk1mwgEAAAAAADYAAAAoAAAAxAAAAMQAAAABABgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEABAAAAAAAAAEAJQMKTwcUDwASBb3zBsT/DBU2QgIONwMNAgADAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAAABwAANwMNSgAPFgAZAMj/Bcf9AMb/AMr8AAgqRgEMQwIRBwAAAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAAACwAAQwIRSAMOIQUjAsf/Asf/Asf/AMf/AMf/CMf+EB5CSQcSRwMODwAEAQEBAAEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABQAAIwEISgAPQgQUK36kAMX/Asf/Asf/Asf/AMf/AMf/AMj9D8f/RLzmNwAQRgEOLAIJBwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwEBAQEBAwAERQIRTgUPQgAaJsHyAsf/Asf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/A8b/Hsb2EAASPgQPUAAQDgACAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEgACNgUNRAMMSAMYG09+AMH5Asn6Asf/Asf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/B8b/AMf/AMX9PJvCPgAWUAAROwMQDQAFBQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAwEBAAEAEAAAMAUOUQAPQwQOCwYlCcPzAMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/BcX/As/8CSBGTwYORgEQOgQREwACAAEBAAIBBQABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGwAEPwEPRgEQRgEOFgMkD8T1D8L/AMn/A8b/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/B8b/AMn/BM/8CMf/BwMgSgAVPgQPPQYPIAAEBQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAgAAAAEACAABIgAGPwQSSwAORwAKAAgpIMj3A8j/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/Asf/Asf/Asf/BcHyEDBTRQMPQAAGRQESJQMKCAABAAIBAwEBAwEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFgADQwIRRwAQQgEQSAASI36dDMT6AcT/AMj7AMf/A8b/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/Asf/Asf/Asf/A8b/A8b/Bcj/QprDOgAYSQARRwAQRAINGQACBQMDAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAIAAAIBDQACMAIORQIXRgEQSwAOIgMgO8jtCsf6A8b/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/Asf/Asf/Asf/Psz1BgEeQwENTQAOSQARLgMOCgABAAEBAAAABQABBQABAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAABwAACgACMAEKRgANSAAPPwERQQQYJZC1A8L5AsX/AMv5Bcf9AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/Asf/Asf/Asf/Bcf9AMj9AMb9ULrfOwAaRwIRPwIQSAERLwQPDwACAAEAAAIBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwEBAwEBAAIBAwEBBwAAJgUMRwAURwAQRAALRAEcHVJzEMXyBsX+AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/B8b/AMb1MXqaNgUbRgQQUAAORwAQOAEQFwACBQABAAIAAwEABgEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAABGQAEOgIPRwAQSwARVAIPNAEVIV6AAML3AMj/B8b/B8f8AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMj9AMj9A8b/AMT/Ccb5LniaLQAePAMMRAEQRgEQOwMOHgAGAAABBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAgAAAwEBAAEBAAAAAAMALgANQQMPTQAQRAIOTgYTJAAdOIarAsTzAMT9AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/B8X/AMj9F8L2RZ/VGQgdQwILPQYPUAAOPgMSHgUJDAAAAAAAAgAAAwEBAwEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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RQESPwERTgARRQAPJQAKEAAAAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAADgAAIgAKQgEQRwAQSQAQCgAYLc31EMT/AMn8AMv8B8f8AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/A8b/AMf/B8b9AMf8E8j1AQMiOQAMSwAQRAEQKQMJCAABAwEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAACIgEIRwIPSwARRAMTQgEQPwMOAgICAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABQABCwAARwEOOwMODAABAAAABgABAAEAAwEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAAABQABAAAABQAAOgINSgAWQgQQLQAYMcfvAMX+AcT/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMf/AMj/DMj/McLwGgIgRAEQQgMNQQMVDQABAAIBBQABBQABCAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABgEAAwEAAAMABwABMwQNQQAPEgABBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAABwAAJwIMRwAQQwcSLAIbQb/oAMb+CcX8Asf/Asf/Asf/AMf/AMf/A8f9Bcf9AMf/Ksv4HQAXRgEQRgEQLAEKBQAAAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABgABAgAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBBQABBAAAGQELSAESRAEQLQATLsjzAsf/Asf/Asf/AMf/AMf/DcH/EsbwDQAeUAINRAATKwIJCQAAAwEBAwEBBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAgAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQEBBgABKAIISQEORQIRDAwwAMj/BMj4CsT/AMX5ACdNRAMMQAIOMAEJCQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAAAAAEABQABKgMLSQAUPwADNpjGP8bsPwIWTwAOLQMVAgABAAICAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGAAERAEQSgERRwARRwIPIQAFAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBBgABKQULNgQOBQAAAAAAAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            return logo;
        }

        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }
        #endregion
    }
}