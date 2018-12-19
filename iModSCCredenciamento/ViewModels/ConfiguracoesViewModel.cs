﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

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
            CarregaColecaoTiposEquipamentos();
            CarregaColecaoTiposAtividades();
            CarregaColecaoTiposCobrancas();
            CarregaColecaoStatus();
            CarregaColecaoCursos();
            CarregaColecaoAreasAcessos();
            CarregaColecaoTiposAcessos();
            CarregaColecaoRelatorios();
            CarregaColecaoRelatoriosGerenciais();
            CarregaColecaoLayoutsCrachas();
        }
        #endregion

        #region Variaveis privadas
        private SynchronizationContext MainThread;

        private ObservableCollection<ClasseRelatorios.Relatorio> _Relatorios;
        private ObservableCollection<ClasseRelatoriosGerenciais.RelatorioGerencial> _RelatoriosGerenciais;


        private ClasseRelatorios.Relatorio _RelatorioTemp = new ClasseRelatorios.Relatorio();
        private List<ClasseRelatorios.Relatorio> _RelatoriosTemp = new List<ClasseRelatorios.Relatorio>();

        private ClasseRelatoriosGerenciais.RelatorioGerencial _RelatorioGerencialTemp = new ClasseRelatoriosGerenciais.RelatorioGerencial();
        private List<ClasseRelatoriosGerenciais.RelatorioGerencial> _RelatoriosGerenciaisTemp = new List<ClasseRelatoriosGerenciais.RelatorioGerencial>();

        private ClasseLayoutsCrachas.LayoutCracha _LayoutCrachaTemp = new ClasseLayoutsCrachas.LayoutCracha();
        private List<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachasTemp = new List<ClasseLayoutsCrachas.LayoutCracha>();

        private ClasseRelatorios.Relatorio _RelatorioSelecionado;
        private ClasseRelatoriosGerenciais.RelatorioGerencial _RelatorioGerencialSelecionado;


        private int _relatorioSelectedIndex;
        private int _relatorioGerencialSelectedIndex;

        private ObservableCollection<ClasseTiposEquipamento.TipoEquipamento> _TiposEquipamentos;
        private List<ClasseTiposEquipamento.TipoEquipamento> _TiposEquipamentosTemp = new List<ClasseTiposEquipamento.TipoEquipamento>();
        private ClasseTiposEquipamento.TipoEquipamento _TipoEquipamentoSelecionado;
        private int _tipoEquipamentoSelectedIndex;

        private ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _TiposCobrancas;
        private List<ClasseTiposCobrancas.TipoCobranca> _TiposCobrancasTemp = new List<ClasseTiposCobrancas.TipoCobranca>();
        private ClasseTiposCobrancas.TipoCobranca _CobrancaSelecionada;
        private int _tipoCobrancaSelectedIndex;

        private ObservableCollection<ClasseTiposAtividades.TipoAtividade> _TiposAtividade;
        private List<ClasseTiposAtividades.TipoAtividade> _TiposAtividadesTemp = new List<ClasseTiposAtividades.TipoAtividade>();
        private ClasseTiposAtividades.TipoAtividade _AtividadeSelecionada;
        private int _tipoAtividadeSelectedIndex;

        private ObservableCollection<ClasseCursos.Curso> _Cursos;
        private List<ClasseCursos.Curso> _cursosTemp = new List<ClasseCursos.Curso>();
        private ClasseCursos.Curso _CursosSelecionado;
        private int _cursoSelectedIndex;


        private ObservableCollection<ClasseStatus.Status> _TiposStatus;
        private List<ClasseStatus.Status> _StatusTemp = new List<ClasseStatus.Status>();
        private ClasseStatus.Status _StatusSelecionado;
        private int _tipoStatusSelectedIndex;

        private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _TiposAcessos;
        private List<ClasseTiposAcessos.TipoAcesso> _TiposAcessosTemp = new List<ClasseTiposAcessos.TipoAcesso>();
        private ClasseTiposAcessos.TipoAcesso _TiposAcessoSelecionado;
        private int _tipoAcessoSelectedIndex;

        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreaAcessos;
        private ClasseAreasAcessos.AreaAcesso _AcessoAreaSelecionada;
        private int _areaAcessoSelectedIndex;

        private int _selectedAcessoIndex;

        private int _selectedIndex;
        private int _selectedIndexTemp = 0;
        private ClasseLayoutsCrachas.LayoutCracha _LayoutCrachaSelecionado;
        private ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachas;
        private int _LayoutCrachaSelectedIndex;

        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IRelatorioService _relatorioService = new RelatorioService();
        private readonly IRelatorioGerencialService _relatorioGerencialService = new RelatorioGerencialService();

        private Relatorios relatorio = new Relatorios();
        private RelatoriosGerenciais relatorioGerencial = new RelatoriosGerenciais();
        private LayoutCracha layoutCracha = new LayoutCracha();


        #endregion

        #region Contrutores
        public ObservableCollection<ClasseTiposEquipamento.TipoEquipamento> TiposEquipamentos
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

        public ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> LayoutsCrachas
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
        public ClasseTiposEquipamento.TipoEquipamento TipoEquipamentoSelecionado
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

        public ObservableCollection<ClasseRelatorios.Relatorio> Relatorios
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

        public ObservableCollection<ClasseRelatoriosGerenciais.RelatorioGerencial> RelatoriosGerenciais
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

        public ClasseRelatorios.Relatorio RelatorioSelecionado
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

        public ClasseLayoutsCrachas.LayoutCracha LayoutCrachaSelecionado
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

        public ClasseRelatoriosGerenciais.RelatorioGerencial RelatorioGerencialSelecionado
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

        public ObservableCollection<ClasseTiposAtividades.TipoAtividade> TiposAtividades
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
        public ClasseTiposAtividades.TipoAtividade TipoAtividadeSelecionada
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

        public ObservableCollection<ClasseTiposCobrancas.TipoCobranca> TiposCobrancas
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
        public ClasseTiposCobrancas.TipoCobranca TipoCobrancaSelecionado
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

        public ObservableCollection<ClasseAreasAcessos.AreaAcesso> AreasAcessos
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
        public ClasseAreasAcessos.AreaAcesso AreaAcessoSelecionada
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

        public ObservableCollection<ClasseStatus.Status> TiposStatus
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
        public ClasseStatus.Status TipoStatusSelecionado
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

        public ObservableCollection<ClasseCursos.Curso> Cursos
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
        public ClasseCursos.Curso CursoSelecionado
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

        public ObservableCollection<ClasseTiposAcessos.TipoAcesso> TiposAcessos
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

        public ClasseTiposAcessos.TipoAcesso TipoAcessoSelecionado
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

        #endregion

        #region Comandos dos Botoes

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
                ClasseTiposEquipamento.TipoEquipamento _tipoEquipamento = new ClasseTiposEquipamento.TipoEquipamento();
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

                if (TipoEquipamentoSelecionado.TipoEquipamentoID != 0)
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

                _RelatorioTemp = new ClasseRelatorios.Relatorio();
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
                var entity = RelatorioSelecionado;
                var entityConv = Mapper.Map<Relatorios>(entity);

                if (RelatorioSelecionado.RelatorioID != 0)
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
                if (arq == null) return;
                _RelatorioTemp.NomeArquivoRPT = arq.Nome;
                _RelatorioTemp.ArquivoRPT = arq.FormatoBase64;
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
                var id = RelatorioSelecionado.RelatorioID;
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

                _RelatorioGerencialTemp = new ClasseRelatoriosGerenciais.RelatorioGerencial();
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
                var entity = RelatorioGerencialSelecionado;
                var entityConv = Mapper.Map<RelatoriosGerenciais>(entity);

                if (RelatorioGerencialSelecionado.RelatorioID != 0)
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
                if (arq == null) return;
                _RelatorioGerencialTemp.Nome = arq.Nome;
                _RelatorioGerencialTemp.ArquivoRPT = arq.FormatoBase64;

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
                var id = RelatorioGerencialSelecionado.RelatorioID;
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

                _LayoutCrachaTemp = new ClasseLayoutsCrachas.LayoutCracha();
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
                var entity = LayoutCrachaSelecionado;
                var entityConv = Mapper.Map<LayoutCracha>(entity);

                if (LayoutCrachaSelecionado.LayoutCrachaID != 0)
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
                if (arq == null) return;
                _LayoutCrachaTemp.Nome = arq.Nome;
                _LayoutCrachaTemp.LayoutRPT = arq.FormatoBase64;
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
                var id = LayoutCrachaSelecionado.LayoutCrachaID;

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
                ClasseTiposAtividades.TipoAtividade _atividade = new ClasseTiposAtividades.TipoAtividade();
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

                if (TipoAtividadeSelecionada.TipoAtividadeID != 0)
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
                ClasseTiposCobrancas.TipoCobranca _cobranca = new ClasseTiposCobrancas.TipoCobranca();
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

                if (TipoCobrancaSelecionado.TipoCobrancaID != 0)
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
                ClasseTiposAcessos.TipoAcesso _acesso = new ClasseTiposAcessos.TipoAcesso();
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

                if (TipoAcessoSelecionado.TipoAcessoID != 0)
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
                ClasseStatus.Status _status = new ClasseStatus.Status();
                TiposStatus.Add(_status);

                TipoStatusSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnAdicionarCommand_TiposStatus ex: " + ex);
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

                if (TipoStatusSelecionado.StatusID != 0)
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
                ClasseCursos.Curso _cursos = new ClasseCursos.Curso();
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

                if (CursoSelecionado.CursoID != 0)
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

        #region Comandos dos Botoes AreaAcesso
        public void OnAdicionarCommand_AreaAcesso()
        {
            try
            {
                foreach (var x in Cursos)
                {
                    _cursosTemp.Add(x);
                }

                _areaAcessoSelectedIndex = AreaAcessoSelectedIndex;
                AreasAcessos.Clear();
                ClasseAreasAcessos.AreaAcesso _areaAcesso = new ClasseAreasAcessos.AreaAcesso();
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

                if (AreaAcessoSelecionada.AreaAcessoID != 0)
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

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoTiposEquipamentos()
        {
            try
            {
                var service = new TipoEquipamentoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseTiposEquipamento.TipoEquipamento>>(list1);
                var observer = new ObservableCollection<ClasseTiposEquipamento.TipoEquipamento>();
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

        private void CarregaColecaoTiposAtividades()
        {
            try
            {
                var service = new TipoAtividadeService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseTiposAtividades.TipoAtividade>>(list1);
                var observer = new ObservableCollection<ClasseTiposAtividades.TipoAtividade>();
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

        private void CarregaColecaoTiposCobrancas()
        {
            try
            {
                var service = new TipoCobrancaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseTiposCobrancas.TipoCobranca>>(list1);
                var observer = new ObservableCollection<ClasseTiposCobrancas.TipoCobranca>();
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

        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                var service = new AreaAcessoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseAreasAcessos.AreaAcesso>>(list1);
                var observer = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
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

        private void CarregaColecaoStatus()
        {
            try
            {
                var service = new StatusService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseStatus.Status>>(list1);
                var observer = new ObservableCollection<ClasseStatus.Status>();
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

        public void CarregaColecaoCursos()
        {
            try
            {
                var service = new CursoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseCursos.Curso>>(list1);
                var observer = new ObservableCollection<ClasseCursos.Curso>();
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

        public void CarregaColecaoLayoutsCrachas()
        {
            try
            {
                var service = new LayoutCrachaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseLayoutsCrachas.LayoutCracha>>(list1);
                var observer = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();
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

        private void CarregaColecaoTiposAcessos()
        {
            try
            {
                var service = new TipoAcessoService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseTiposAcessos.TipoAcesso>>(list1);
                var observer = new ObservableCollection<ClasseTiposAcessos.TipoAcesso>();
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

        private void CarregaColecaoRelatorios()
        {
            try
            {
                var service = new RelatorioService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseRelatorios.Relatorio>>(list1);
                var observer = new ObservableCollection<ClasseRelatorios.Relatorio>();
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

                var list2 = Mapper.Map<List<ClasseRelatoriosGerenciais.RelatorioGerencial>>(list1);
                var observer = new ObservableCollection<ClasseRelatoriosGerenciais.RelatorioGerencial>();
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


        #endregion

        #region Metodos privados (vazia)

        #endregion


    }
}
