using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Views.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace iModSCCredenciamento.ViewModels
{
    public class ConfiguracoesViewModel : ViewModelBase
    {
        #region Inicializacao
        public ConfiguracoesViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
        }

        private void CarregaUI()
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

        public RelatorioView _RelatorioTemp = new RelatorioView();
        public RelatorioGerencialView _RelatorioGerencialTemp = new RelatorioGerencialView();
        public LayoutCrachaView _LayoutCrachaTemp = new LayoutCrachaView();

        #endregion

        #region Variaveis privadas
        private SynchronizationContext MainThread;

        private int _selectedIndex;
        private int _selectedIndexTemp = 0;

        //Relatórios
        private ObservableCollection<RelatorioView> _Relatorios;
        private List<RelatorioView> _RelatoriosTemp = new List<RelatorioView>();
        private RelatorioView _RelatorioSelecionado;
        private int _relatorioSelectedIndex;

        //Relatórios Gerenciais
        private ObservableCollection<RelatorioGerencialView> _RelatoriosGerenciais;
        private List<RelatorioGerencialView> _RelatoriosGerenciaisTemp = new List<RelatorioGerencialView>();
        private RelatorioGerencialView _RelatorioGerencialSelecionado;
        private int _relatorioGerencialSelectedIndex;

        //Layouts Crachás
        private ObservableCollection<LayoutCrachaView> _LayoutsCrachas;
        private List<LayoutCrachaView> _LayoutsCrachasTemp = new List<LayoutCrachaView>();
        private LayoutCrachaView _LayoutCrachaSelecionado;
        private int _LayoutCrachaSelectedIndex;

        //TipoEquipamento
        private ObservableCollection<TipoEquipamentoView> _TiposEquipamentos;
        private List<TipoEquipamentoView> _TiposEquipamentosTemp = new List<TipoEquipamentoView>();
        private TipoEquipamentoView _TipoEquipamentoSelecionado;
        private int _tipoEquipamentoSelectedIndex;

        //Tipos Acessos
        private ObservableCollection<TipoAcessoView> _TiposAcessos;
        private List<TipoAcessoView> _TiposAcessosTemp = new List<TipoAcessoView>();
        private TipoAcessoView _TiposAcessoSelecionado;
        private int _tipoAcessoSelectedIndex;

        //Áreas Acessos
        private ObservableCollection<AreaAcessoView> _AreaAcessos;
        private List<AreaAcessoView> _areaAcessoTemp = new List<AreaAcessoView>();
        private AreaAcessoView _AcessoAreaSelecionada;
        private int _areaAcessoSelectedIndex;
        private int _selectedAcessoIndex;

        //Tipos Atividades
        private ObservableCollection<TipoAtividadeView> _TiposAtividade;
        private List<TipoAtividadeView> _TiposAtividadesTemp = new List<TipoAtividadeView>();
        private TipoAtividadeView _AtividadeSelecionada;
        private int _tipoAtividadeSelectedIndex;

        //Tipos Servico
        private ObservableCollection<TipoServicoView> _TipoServico;
        private List<TipoServicoView> _TipoServicoTemp = new List<TipoServicoView>();
        private TipoServicoView _TipoServicoSelecionado;
        private int _tipoServicoSelectedIndex;

        //Tecnologias Credenciais
        private ObservableCollection<TecnologiaCredencialView> _TecnologiasCredenciais;
        private List<TecnologiaCredencialView> _TecnologiaCredencialTemp = new List<TecnologiaCredencialView>();
        private TecnologiaCredencialView _TecnologiaCredencialSelecionada;
        private int _tecnologiaCredencialSelectedIndex;

        //Tipos Cobranças
        private ObservableCollection<TipoCobrancaView> _TiposCobrancas;
        private List<TipoCobrancaView> _TiposCobrancasTemp = new List<TipoCobrancaView>();
        private TipoCobrancaView _CobrancaSelecionada;
        private int _tipoCobrancaSelectedIndex;

        //Cursos
        private ObservableCollection<CursoView> _Cursos;
        private List<CursoView> _cursosTemp = new List<CursoView>();
        private CursoView _CursosSelecionado;
        private int _cursoSelectedIndex;

        //TipoCombustível
        private ObservableCollection<TipoCombustivelView> _TiposCombustiveis;
        private List<TipoCombustivelView> _TiposCombustiveisTemp = new List<TipoCombustivelView>();
        private TipoCombustivelView _TipoCombustivelSelecionado;
        private int _tipoCombustivelSelectedIndex;

        //Tipos Status
        private ObservableCollection<StatusView> _TiposStatus;
        private List<StatusView> _StatusTemp = new List<StatusView>();
        private StatusView _StatusSelecionado;
        private int _tipoStatusSelectedIndex;

        //Credenciais Status
        private ObservableCollection<CredencialStatusView> _CredenciaisStatus;
        private List<CredencialStatusView> _CredencialStatusTemp = new List<CredencialStatusView>();
        private CredencialStatusView _CredencialStatusSelecionado;
        private int _credencialStatusSelectedIndex;

        //Credenciais Motivos
        private ObservableCollection<CredencialMotivoView> _CredenciaisMotivos;
        private List<CredencialMotivoView> _CredencialMotivoTemp = new List<CredencialMotivoView>();
        private CredencialMotivoView _CredencialMotivoSelecionado;
        private int _credencialMotivoSelectedIndex;

        //Formatos Credenciais
        private ObservableCollection<FormatoCredencialView> _FormatosCredenciais;
        private List<FormatoCredencialView> _FormatoCredencialTemp = new List<FormatoCredencialView>();
        private FormatoCredencialView _FormatoCredencialSelecionado;
        private int _formatoCredencialSelectedIndex;

        //Serviços
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IRelatorioService _relatorioService = new RelatorioService();
        private readonly IRelatorioGerencialService _relatorioGerencialService = new RelatorioGerencialService();

        private Relatorios relatorio = new Relatorios();
        private RelatoriosGerenciais relatorioGerencial = new RelatoriosGerenciais();
        private LayoutCracha layoutCracha = new LayoutCracha();


        #endregion

        #region Contrutores

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        //Relatórios
        public ObservableCollection<RelatorioView> Relatorios
        {
            get
            {
                return _Relatorios;
            }

            set
            {
                if (_Relatorios != value)
                {
                    _Relatorios = value;
                    OnPropertyChanged();

                }
            }
        }
        public RelatorioView RelatorioSelecionado
        {
            get
            {
                return _RelatorioSelecionado;
            }
            set
            {
                _RelatorioSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (_RelatorioSelecionado != null)
                {

                }

            }
        }
        public int RelatorioSelectedIndex
        {
            get
            {
                return _relatorioSelectedIndex;
            }
            set
            {
                _relatorioSelectedIndex = value;
                OnPropertyChanged("RelatorioSelectedIndex");
            }
        }

        //Relatórios Gerenciais
        public ObservableCollection<RelatorioGerencialView> RelatoriosGerenciais
        {
            get
            {
                return _RelatoriosGerenciais;
            }

            set
            {
                if (_RelatoriosGerenciais != value)
                {
                    _RelatoriosGerenciais = value;
                    OnPropertyChanged();

                }
            }
        }
        public RelatorioGerencialView RelatorioGerencialSelecionado
        {
            get
            {
                return _RelatorioGerencialSelecionado;
            }
            set
            {
                _RelatorioGerencialSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (_RelatorioGerencialSelecionado != null)
                {

                }

            }
        }
        public int RelatorioGerencialSelectedIndex
        {
            get
            {
                return _relatorioGerencialSelectedIndex;
            }
            set
            {
                _relatorioGerencialSelectedIndex = value;
                OnPropertyChanged("RelatorioGerencialSelectedIndex");
            }
        }

        //Layouts Crachás
        public ObservableCollection<LayoutCrachaView> LayoutsCrachas
        {
            get
            {
                return _LayoutsCrachas;
            }

            set
            {
                if (_LayoutsCrachas != value)
                {
                    _LayoutsCrachas = value;
                    OnPropertyChanged();

                }
            }
        }
        public LayoutCrachaView LayoutCrachaSelecionado
        {
            get
            {
                return _LayoutCrachaSelecionado;
            }
            set
            {
                _LayoutCrachaSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (_LayoutCrachaSelecionado != null)
                {

                }

            }
        }
        public int LayoutCrachaSelectedIndex
        {
            get
            {
                return _LayoutCrachaSelectedIndex;
            }
            set
            {
                _LayoutCrachaSelectedIndex = value;
                OnPropertyChanged("LayoutCrachaSelectedIndex");
            }
        }

        //Tipos Equipamentos
        public ObservableCollection<TipoEquipamentoView> TiposEquipamentos
        {
            get
            {
                return _TiposEquipamentos;
            }

            set
            {
                if (_TiposEquipamentos != value)
                {
                    _TiposEquipamentos = value;
                    OnPropertyChanged();

                }
            }
        }
        public TipoEquipamentoView TipoEquipamentoSelecionado
        {
            get
            {
                return _TipoEquipamentoSelecionado;
            }
            set
            {
                _TipoEquipamentoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoEquipamentoSelecionado != null)
                {

                }

            }
        }
        public int TipoEquipamentoSelectedIndex
        {
            get
            {
                return _tipoEquipamentoSelectedIndex;
            }
            set
            {
                _tipoEquipamentoSelectedIndex = value;
                OnPropertyChanged("TipoEquipamentoSelectedIndex");
            }
        }

        //Tipos Acessos
        public ObservableCollection<TipoAcessoView> TiposAcessos
        {
            get
            {
                return _TiposAcessos;
            }

            set
            {
                if (_TiposAcessos != value)
                {
                    _TiposAcessos = value;
                    OnPropertyChanged();

                }
            }
        }
        public TipoAcessoView TipoAcessoSelecionado
        {
            get
            {
                return _TiposAcessoSelecionado;
            }
            set
            {
                _TiposAcessoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoAcessoSelecionado != null)
                {

                }

            }
        }
        public int TipoAcessoSelectedIndex
        {
            get
            {
                return _tipoAcessoSelectedIndex;
            }
            set
            {
                _tipoAcessoSelectedIndex = value;
                OnPropertyChanged("TipoAcessoSelectedIndex");
            }
        }

        //Tipos Áreas Acessos
        public ObservableCollection<AreaAcessoView> AreasAcessos
        {
            get
            {
                return _AreaAcessos;
            }

            set
            {
                if (_AreaAcessos != value)
                {
                    _AreaAcessos = value;
                    OnPropertyChanged();

                }
            }
        }
        public AreaAcessoView AreaAcessoSelecionada
        {
            get
            {
                return _AcessoAreaSelecionada;
            }
            set
            {
                _AcessoAreaSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (AreaAcessoSelecionada != null)
                {

                }

            }
        }
        public int AreaAcessoSelectedIndex
        {
            get
            {
                return _areaAcessoSelectedIndex;
            }
            set
            {
                _areaAcessoSelectedIndex = value;
                OnPropertyChanged("AreaAcessoSelectedIndex");
            }
        }

        //Tipos Atividades
        public ObservableCollection<TipoAtividadeView> TiposAtividades
        {
            get
            {
                return _TiposAtividade;
            }

            set
            {
                if (_TiposAtividade != value)
                {
                    _TiposAtividade = value;
                    OnPropertyChanged();

                }
            }
        }
        public TipoAtividadeView TipoAtividadeSelecionada
        {
            get
            {
                return _AtividadeSelecionada;
            }
            set
            {
                _AtividadeSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoAtividadeSelecionada != null)
                {

                }

            }
        }
        public int TipoAtividadeSelectedIndex
        {
            get
            {
                return _tipoAtividadeSelectedIndex;
            }
            set
            {
                _tipoAtividadeSelectedIndex = value;
                OnPropertyChanged("TipoAtividadeSelectedIndex");
            }
        }

        //Tipos Serviços
        public ObservableCollection<TipoServicoView> TipoServico
        {
            get
            {
                return _TipoServico;
            }

            set
            {
                if (_TipoServico != value)
                {
                    _TipoServico = value;
                    OnPropertyChanged();

                }
            }
        }


        public TipoServicoView TipoServicoSelecionado
        {
            get
            {
                return _TipoServicoSelecionado;
            }
            set
            {
                _TipoServicoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoServicoSelecionado != null)
                {

                }

            }
        }

        public int TipoServicoSelectedIndex
        {
            get
            {
                return _tipoServicoSelectedIndex;
            }
            set
            {
                _tipoServicoSelectedIndex = value;
                OnPropertyChanged("TipoServicoSelectedIndex");
            }
        }






        //Tecnologias Credenciais
        public ObservableCollection<TecnologiaCredencialView> TecnologiasCredenciais
        {
            get
            {
                return _TecnologiasCredenciais;

            }
            set
            {
                if (_TecnologiasCredenciais != value)
                {
                    _TecnologiasCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }
        public TecnologiaCredencialView TecnologiaCredencialSelecionada
        {
            get
            {
                return _TecnologiaCredencialSelecionada;

            }
            set
            {
                _TecnologiaCredencialSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (_TecnologiaCredencialSelecionada != null)
                {

                }
            }
        }
        public int TecnologiaCredencialSelectedIndex
        {
            get
            {
                return _tecnologiaCredencialSelectedIndex;

            }
            set
            {
                _tecnologiaCredencialSelectedIndex = value;
                OnPropertyChanged("TecnologiaCredencialSelectedIndex");
            }
        }

        //Tipos Cobranças
        public ObservableCollection<TipoCobrancaView> TiposCobrancas
        {
            get
            {
                return _TiposCobrancas;
            }

            set
            {
                if (_TiposCobrancas != value)
                {
                    _TiposCobrancas = value;
                    OnPropertyChanged();

                }
            }
        }
        public TipoCobrancaView TipoCobrancaSelecionado
        {
            get
            {
                return _CobrancaSelecionada;
            }
            set
            {
                _CobrancaSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoCobrancaSelecionado != null)
                {

                }

            }
        }
        public int TipoCobrancaSelectedIndex
        {
            get
            {
                return _tipoCobrancaSelectedIndex;
            }
            set
            {
                _tipoCobrancaSelectedIndex = value;
                OnPropertyChanged("TipoCobrancaSelectedIndex");
            }
        }

        //Cursos
        public ObservableCollection<CursoView> Cursos
        {
            get
            {
                return _Cursos;
            }

            set
            {
                if (_Cursos != value)
                {
                    _Cursos = value;
                    OnPropertyChanged();
                }
            }
        }
        public CursoView CursoSelecionado
        {
            get
            {
                return _CursosSelecionado;
            }
            set
            {
                _CursosSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (CursoSelecionado != null)
                {

                }

            }
        }
        public int CursoSelectedIndex
        {
            get
            {
                return _cursoSelectedIndex;
            }
            set
            {
                _cursoSelectedIndex = value;
                OnPropertyChanged("CursoSelectedIndex");
            }
        }

        //Tipos Combustíveis
        public ObservableCollection<TipoCombustivelView> TiposCombustiveis
        {
            get
            {
                return _TiposCombustiveis;
            }

            set
            {
                if (_TiposCombustiveis != value)
                {
                    _TiposCombustiveis = value;
                    OnPropertyChanged();

                }
            }
        }
        public TipoCombustivelView TipoCombustivelSelecionado
        {
            get
            {
                return _TipoCombustivelSelecionado;
            }

            set
            {
                _TipoCombustivelSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (_TipoCombustivelSelecionado != null)
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
            get
            {
                return _TiposStatus;
            }

            set
            {
                if (_TiposStatus != value)
                {
                    _TiposStatus = value;
                    OnPropertyChanged();

                }
            }
        }
        public StatusView TipoStatusSelecionado
        {
            get
            {
                return _StatusSelecionado;
            }
            set
            {
                _StatusSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoStatusSelecionado != null)
                {

                }

            }
        }
        public int TipoStatusSelectedIndex
        {
            get
            {
                return _tipoStatusSelectedIndex;
            }
            set
            {
                _tipoStatusSelectedIndex = value;
                OnPropertyChanged("TipoStatusSelectedIndex");
            }
        }

        //Credenciais Status
        public ObservableCollection<CredencialStatusView> CredenciaisStatus
        {
            get
            {
                return _CredenciaisStatus;
            }

            set
            {
                if (_CredenciaisStatus != value)
                {
                    _CredenciaisStatus = value;
                    OnPropertyChanged();

                }
            }
        }
        public CredencialStatusView CredenciaisStatusSelecionado
        {
            get
            {
                return _CredencialStatusSelecionado;
            }
            set
            {
                _CredencialStatusSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoStatusSelecionado != null)
                {

                }

            }
        }
        public int CredencialStatusSelectedIndex
        {
            get
            {
                return _credencialStatusSelectedIndex;
            }
            set
            {
                _credencialStatusSelectedIndex = value;
                OnPropertyChanged("CredencialStatusSelectedIndex");
            }
        }


        //Credenciais Motivos
        public ObservableCollection<CredencialMotivoView> CredenciaisMotivos
        {
            get
            {
                return _CredenciaisMotivos;
            }

            set
            {
                if (_CredenciaisMotivos != value)
                {
                    _CredenciaisMotivos = value;
                    OnPropertyChanged();

                }
            }
        }
        public CredencialMotivoView CredencialMotivoSelecionado
        {
            get
            {
                return _CredencialMotivoSelecionado;
            }
            set
            {
                _CredencialMotivoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoStatusSelecionado != null)
                {

                }

            }
        }
        public int CredencialMotivoSelectedIndex
        {
            get
            {
                return _credencialMotivoSelectedIndex;
            }
            set
            {
                _credencialMotivoSelectedIndex = value;
                OnPropertyChanged("CredencialMotivoSelectedIndex");
            }
        }


        //Formatos Credenciais
        public ObservableCollection<FormatoCredencialView> FormatosCredenciais
        {
            get
            {
                return _FormatosCredenciais;
            }

            set
            {
                if (_FormatosCredenciais != value)
                {
                    _FormatosCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }
        public FormatoCredencialView FormatoCredencialSelecionado
        {
            get
            {
                return _FormatoCredencialSelecionado;
            }
            set
            {
                _FormatoCredencialSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (_FormatoCredencialSelecionado != null)
                {

                }

            }
        }
        public int FormatoCredencialSelectedIndex
        {
            get
            {
                return _formatoCredencialSelectedIndex;
            }
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
                    _RelatoriosTemp.Add(x);
                }

                _relatorioSelectedIndex = RelatorioSelectedIndex;
                Relatorios.Clear();

                _RelatorioTemp = new RelatorioView();
                Relatorios.Add(_RelatorioTemp);

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
                RelatorioSelecionado.ArquivoRpt = _RelatorioTemp.ArquivoRpt;

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = RelatorioSelecionado;
                    var entityConv = Mapper.Map<Relatorios>(entity);
                    _auxiliaresService.RelatorioService.Remover(entityConv);

                    Relatorios.Remove(RelatorioSelecionado);
                }
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

                _RelatorioTemp.RelatorioId = RelatorioSelecionado.RelatorioId;
                RelatorioSelecionado.Nome = arq.Nome;

                _RelatorioTemp.Nome = RelatorioSelecionado.Nome;
                _RelatorioTemp.NomeArquivoRpt = arq.Nome;
                _RelatorioTemp.ArquivoRpt = arq.FormatoBase64;

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
                relatorio = _relatorioService.BuscarPelaChave(id);

                var arrayFile = Convert.FromBase64String(relatorio.ArquivoRpt);
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
                    _RelatoriosGerenciaisTemp.Add(x);
                }

                _relatorioGerencialSelectedIndex = RelatorioGerencialSelectedIndex;
                RelatoriosGerenciais.Clear();

                _RelatorioGerencialTemp = new RelatorioGerencialView();
                RelatoriosGerenciais.Add(_RelatorioGerencialTemp);

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
                RelatorioGerencialSelecionado.ArquivoRpt = _RelatorioGerencialTemp.ArquivoRpt;

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = RelatorioGerencialSelecionado;
                    var entityConv = Mapper.Map<RelatoriosGerenciais>(entity);
                    _auxiliaresService.RelatorioGerencialService.Remover(entityConv);

                    RelatoriosGerenciais.Remove(RelatorioGerencialSelecionado);
                }
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

                _RelatorioGerencialTemp.RelatorioId = RelatorioGerencialSelecionado.RelatorioId;
                RelatorioGerencialSelecionado.Nome = arq.Nome;
                _RelatorioGerencialTemp.Nome = RelatorioGerencialSelecionado.Nome;
                _RelatorioGerencialTemp.NomeArquivoRpt = arq.Nome;
                _RelatorioGerencialTemp.ArquivoRpt = arq.FormatoBase64;

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
                relatorioGerencial = _relatorioGerencialService.BuscarPelaChave(id);

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
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
            try
            {
                foreach (var x in LayoutsCrachas)
                {
                    _LayoutsCrachasTemp.Add(x);
                }

                _LayoutCrachaSelectedIndex = LayoutCrachaSelectedIndex;
                LayoutsCrachas.Clear();

                _LayoutCrachaTemp = new LayoutCrachaView();
                LayoutsCrachas.Add(_LayoutCrachaTemp);

                LayoutCrachaSelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }
        public void OnSalvarLayoutCrachaCommand()
        {
            try
            {
                //Atualiza arquivo Byte[] (.rpt)
                LayoutCrachaSelecionado.LayoutRpt = _LayoutCrachaTemp.LayoutRpt;

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    var entity = LayoutCrachaSelecionado;
                    var entityConv = Mapper.Map<LayoutCracha>(entity);
                    _auxiliaresService.LayoutCrachaService.Remover(entityConv);
                    LayoutsCrachas.Remove(LayoutCrachaSelecionado);
                }
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

                _LayoutCrachaTemp.LayoutCrachaId = LayoutCrachaSelecionado.LayoutCrachaId;
                LayoutCrachaSelecionado.Nome = arq.Nome;

                _LayoutCrachaTemp.Nome = LayoutCrachaSelecionado.Nome;
                _LayoutCrachaTemp.LayoutRpt = arq.FormatoBase64;

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

                layoutCracha = _auxiliaresService.LayoutCrachaService.BuscarPelaChave(id);

                var arrayFile = Convert.FromBase64String(layoutCracha.LayoutRpt);
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
                    _TiposEquipamentosTemp.Add(x);
                }

                _tipoEquipamentoSelectedIndex = TipoEquipamentoSelectedIndex;
                TiposEquipamentos.Clear();
                TipoEquipamentoView _tipoEquipamento = new TipoEquipamentoView();
                TiposEquipamentos.Add(_tipoEquipamento);
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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        var entity = TipoEquipamentoSelecionado;
                        var entityConv = Mapper.Map<TipoEquipamento>(entity);
                        _auxiliaresService.TipoEquipamentoService.Remover(entityConv);

                        TiposEquipamentos.Remove(TipoEquipamentoSelecionado);
                    }
                }
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
                    _TiposAcessosTemp.Add(x);
                }

                _tipoAcessoSelectedIndex = TipoAcessoSelectedIndex;
                TiposAcessos.Clear();
                TipoAcessoView _acesso = new TipoAcessoView();
                TiposAcessos.Add(_acesso);

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = TipoAcessoSelecionado;
                    var entityConv = Mapper.Map<TipoAcesso>(entity);
                    _auxiliaresService.TiposAcessoService.Remover(entityConv);

                    CarregaColecaoTiposAcessos();
                }
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
                AreaAcessoView _areaAcesso = new AreaAcessoView();
                AreasAcessos.Add(_areaAcesso);

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = AreaAcessoSelecionada;
                    var entityConv = Mapper.Map<AreaAcesso>(entity);
                    _auxiliaresService.AreaAcessoService.Remover(entityConv);

                    AreasAcessos.Remove(AreaAcessoSelecionada);

                    CarregaColecaoAreasAcessos();
                }
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
                    _TiposAtividadesTemp.Add(x);
                }

                _tipoAtividadeSelectedIndex = TipoAtividadeSelectedIndex;
                TiposAtividades.Clear();
                TipoAtividadeView _atividade = new TipoAtividadeView();
                TiposAtividades.Add(_atividade);

                TipoAtividadeSelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = TipoAtividadeSelecionada;
                    var entityConv = Mapper.Map<TipoAtividade>(entity);
                    _auxiliaresService.TipoAtividadeService.Remover(entityConv);

                    TiposAtividades.Remove(TipoAtividadeSelecionada);
                    CarregaColecaoTiposAtividades();
                }
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
                    _TipoServicoTemp.Add(x);
                }

                _tipoServicoSelectedIndex = TipoServicoSelectedIndex;
                TipoServico.Clear();
                TipoServicoView _status = new TipoServicoView();
                TipoServico.Add(_status);

                TipoServicoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
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
                Global.Log("Erro na void OnSalvarEdicaoCommand_TiposStatus ex: " + ex);
                Utils.TraceException(ex);
                throw;
            }
        }
        public void OnExcluirCommand_TipoServico()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = TipoServicoSelecionado;
                    var entityConv = Mapper.Map<TipoServico>(entity);
                    _auxiliaresService.TipoServicoService.Remover(entityConv);

                    TipoServico.Remove(TipoServicoSelecionado);

                    CarregaColecaoTipoServico();
                }
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnExcluirCommand_TiposStatus ex: " + ex);
                Utils.TraceException(ex);
                throw;
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
                    _TecnologiaCredencialTemp.Add(x);
                }

                _tecnologiaCredencialSelectedIndex = TecnologiaCredencialSelectedIndex;
                TecnologiasCredenciais.Clear();
                TecnologiaCredencialView _tecnologiaCredencial = new TecnologiaCredencialView();
                TecnologiasCredenciais.Add(_tecnologiaCredencial);

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = TecnologiaCredencialSelecionada;
                    var entityConv = Mapper.Map<TecnologiaCredencial>(entity);
                    _auxiliaresService.TecnologiaCredencialService.Remover(entityConv);

                    TecnologiasCredenciais.Remove(TecnologiaCredencialSelecionada);

                    CarregaColecaoTecnologiasCredenciais();
                }
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
                    _TiposCobrancasTemp.Add(x);
                }

                _tipoCobrancaSelectedIndex = TipoCobrancaSelectedIndex;
                TiposCobrancas.Clear();
                TipoCobrancaView _cobranca = new TipoCobrancaView();
                TiposCobrancas.Add(_cobranca);

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = TipoCobrancaSelecionado;
                    var entityConv = Mapper.Map<TipoCobranca>(entity);
                    _auxiliaresService.TipoCobrancaService.Remover(entityConv);

                    TiposCobrancas.Remove(TipoCobrancaSelecionado);

                    CarregaColecaoTiposCobrancas();
                }
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
                CursoView _cursos = new CursoView();
                Cursos.Add(_cursos);

                CursoSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnAdicionarCommand_TiposCursos ex: " + ex);
                Utils.TraceException(ex);
                throw;
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
                Global.Log("Erro na void OnSalvarEdicaoCommand_TiposCursos ex: " + ex);
                Utils.TraceException(ex);
                throw;
            }
        }
        public void OnExcluirCommand_TiposCursos()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = CursoSelecionado;
                    var entityConv = Mapper.Map<Curso>(entity);
                    _auxiliaresService.CursoService.Remover(entityConv);
                    Cursos.Remove(CursoSelecionado);

                    CarregaColecaoCursos();
                }
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnSalvarEdicaoCommand_TiposCursos ex: " + ex);
                Utils.TraceException(ex);
                throw;
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
                    _TiposCombustiveisTemp.Add(x);
                }

                _tipoCombustivelSelectedIndex = TipoCombustivelSelectedIndex;


                TiposCombustiveis.Clear();
                TipoCombustivelView _tipoCombustivel = new TipoCombustivelView();
                TiposCombustiveis.Add(_tipoCombustivel);

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
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = TipoCombustivelSelecionado;
                    var entityConv = Mapper.Map<TipoCombustivel>(entity);
                    _auxiliaresService.TipoCombustivelService.Remover(entityConv);

                    TiposCombustiveis.Remove(TipoCombustivelSelecionado);

                    CarregaColecaoTipoCombustiveis();
                }
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
                    _StatusTemp.Add(x);
                }

                _tipoStatusSelectedIndex = TipoStatusSelectedIndex;
                TiposStatus.Clear();
                StatusView _status = new StatusView();
                TiposStatus.Add(_status);

                TipoStatusSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
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
                Global.Log("Erro na void OnSalvarEdicaoCommand_TiposStatus ex: " + ex);
                Utils.TraceException(ex);
                throw;
            }
        }
        public void OnExcluirCommand_TiposStatus()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = TipoStatusSelecionado;
                    var entityConv = Mapper.Map<Status>(entity);
                    _auxiliaresService.StatusService.Remover(entityConv);

                    TiposStatus.Remove(TipoStatusSelecionado);

                    CarregaColecaoStatus();
                }
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnExcluirCommand_TiposStatus ex: " + ex);
                Utils.TraceException(ex);
                throw;
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
                    _CredencialStatusTemp.Add(x);
                }

                _credencialStatusSelectedIndex = CredencialStatusSelectedIndex;
                CredenciaisStatus.Clear();
                CredencialStatusView _credencialStatus = new CredencialStatusView();
                CredenciaisStatus.Add(_credencialStatus);

                TipoStatusSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }

        }
        public void OnSalvarEdicaoCommand_CredenciaisStatus()
        {
            try
            {
                var entity = CredenciaisStatusSelecionado;
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
                throw;
            }
        }
        public void OnExcluirCommand_CredenciaisStatus()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = CredenciaisStatusSelecionado;
                    var entityConv = Mapper.Map<CredencialStatus>(entity);
                    _auxiliaresService.CredencialStatusService.Remover(entityConv);

                    CredenciaisStatus.Remove(CredenciaisStatusSelecionado);

                    CarregaColecaoCredenciaisStatus();
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
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
                    _CredencialMotivoTemp.Add(x);
                }

                _credencialMotivoSelectedIndex = CredencialMotivoSelectedIndex;
                CredenciaisMotivos.Clear();
                CredencialMotivoView _credencialMotivo = new CredencialMotivoView();
                CredenciaisMotivos.Add(_credencialMotivo);

                CredencialMotivoSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
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
                throw;
            }
        }
        public void OnExcluirCommand_CredenciaisMotivos()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = CredencialMotivoSelecionado;
                    var entityConv = Mapper.Map<CredencialStatus>(entity);
                    _auxiliaresService.CredencialStatusService.Remover(entityConv);

                    CredenciaisStatus.Remove(CredenciaisStatusSelecionado);

                    CarregaColecaoCredenciaisMotivos();
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
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
                    _FormatoCredencialTemp.Add(x);
                }

                _formatoCredencialSelectedIndex = FormatoCredencialSelectedIndex;
                FormatosCredenciais.Clear();
                FormatoCredencialView _formatoCredencial = new FormatoCredencialView();
                FormatosCredenciais.Add(_formatoCredencial);

                FormatoCredencialSelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
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
                throw;
            }
        }
        public void OnExcluirCommand_FormatosCredenciais()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = FormatoCredencialSelecionado;
                    var entityConv = Mapper.Map<FormatoCredencial>(entity);
                    _auxiliaresService.FormatoCredencialService.Remover(entityConv);

                    FormatosCredenciais.Remove(FormatoCredencialSelecionado);

                    CarregaColecaoFormatosCredenciais();
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }

        }
        #endregion





        #endregion

        #region Carregamento das Colecoes

        private void CarregaColecaoRelatorios()
        {
            try
            {
                var service = new RelatorioService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<RelatorioView>>(list1);
                var observer = new ObservableCollection<RelatorioView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Relatorios = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void CarregaColecaoRelatoriosGerenciais()
        {
            try
            {
                var service = new RelatorioGerencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<RelatorioGerencialView>>(list1);
                var observer = new ObservableCollection<RelatorioGerencialView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

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
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                LayoutsCrachas = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }
        private void CarregaColecaoTiposEquipamentos()
        {
            try
            {
                var service = new TipoEquipamentoService();
                var list1 = service.Listar();


                var list2 = Mapper.Map<List<TipoEquipamentoView>>(list1);
                var observer = new ObservableCollection<TipoEquipamentoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                TiposEquipamentos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void CarregaColecaoTiposAcessos()
        {
            try
            {
                var service = new TipoAcessoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoAcessoView>>(list1);
                var observer = new ObservableCollection<TipoAcessoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                TiposAcessos = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                var service = new AreaAcessoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<AreaAcessoView>>(list1);
                var observer = new ObservableCollection<AreaAcessoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                AreasAcessos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void CarregaColecaoTiposAtividades()
        {
            try
            {
                var service = new TipoAtividadeService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoAtividadeView>>(list1);
                var observer = new ObservableCollection<TipoAtividadeView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                TiposAtividades = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        private void CarregaColecaoTipoServico()
        {
            try
            {
                var service = new TipoServicoService(); ;
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoServicoView>>(list1);
                var observer = new ObservableCollection<TipoServicoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                TipoServico = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        private void CarregaColecaoTecnologiasCredenciais()
        {
            try
            {
                var service = new TecnologiaCredencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TecnologiaCredencialView>>(list1);
                var observer = new ObservableCollection<TecnologiaCredencialView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                TecnologiasCredenciais = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void CarregaColecaoTiposCobrancas()
        {
            try
            {
                var service = new TipoCobrancaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoCobrancaView>>(list1);
                var observer = new ObservableCollection<TipoCobrancaView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

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
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Cursos = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void CarregaColecaoTipoCombustiveis()
        {
            try
            {
                var service = new TipoCombustivelService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<TipoCombustivelView>>(list1);
                var observer = new ObservableCollection<TipoCombustivelView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                TiposCombustiveis = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoStatus()
        {
            try
            {
                var service = new StatusService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<StatusView>>(list1);
                var observer = new ObservableCollection<StatusView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                TiposStatus = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoCredenciaisStatus()
        {
            try
            {
                var service = new CredencialStatusService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<CredencialStatusView>>(list1);
                var observer = new ObservableCollection<CredencialStatusView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                CredenciaisStatus = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoCredenciaisMotivos()
        {
            try
            {
                var service = new CredencialMotivoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<CredencialMotivoView>>(list1);
                var observer = new ObservableCollection<CredencialMotivoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                CredenciaisMotivos = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoFormatosCredenciais()
        {
            try
            {
                var service = new FormatoCredencialService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<FormatoCredencialView>>(list1);
                var observer = new ObservableCollection<FormatoCredencialView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                FormatosCredenciais = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

    }
}
