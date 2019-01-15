// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels.Commands;
using iModSCCredenciamento.ViewModels.Comportamento;
using iModSCCredenciamento.Views.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    internal class VeiculoViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IVeiculoService _service = new VeiculoService();

        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;

        #region  Propriedades

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

        /// <summary>
        ///     True, empresa possui pendências
        /// </summary>
        public bool Pendencias { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Geral
        /// </summary>
        public bool PendenciaGeral { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Represenante
        /// </summary>
        public bool PendenciaRepresentante { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Contrato
        /// </summary>
        public bool PendenciaContrato { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Anexo
        /// </summary>
        public bool PendenciaAnexo { get; set; }

        /// <summary>
        ///     Habilita abas
        /// </summary>
        public bool IsEnableTabItem { get; private set; } = true;

        /// <summary>
        ///     Seleciona o indice da tabcontrol desejada
        /// </summary>
        public short SelectedTabIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        public VeiculoView Entity { get; set; }
        public ObservableCollection<VeiculoView> EntityObserver { get; set; }

        /// <summary>
        ///     Estados
        /// </summary>
        public List<Estados> Estados { get; set; }

        public Estados Estado { get; set; }

        /// <summary>
        ///     Municipios
        /// </summary>
        public List<Municipio> Municipios { get; set; }

        /// <summary>
        ///     Tipos Combustivel
        /// </summary>
        public List<TipoCombustivel> TiposCombustiveis { get; set; }

        /// <summary>
        ///     Tipos Serviços
        /// </summary>
        public List<TipoServico> ListaTipoServico { get; set; }

        /// <summary>
        ///     Tipos Equipamentos
        /// </summary>
        public List<TipoEquipamento> ListaEquipamentos { get; set; }

        public ObservableCollection<EquipamentoVeiculoTipoServicoView> TiposEquipamentoServico { get; set; }

        /// <summary>
        ///     Tipos Serviços
        /// </summary>
        public TipoServico TipoServico { get; set; }

        /// <summary>
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        #endregion

        public VeiculoViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            TiposEquipamentoServico = new ObservableCollection<EquipamentoVeiculoTipoServicoView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Placa/Identificador"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Série/Chassi"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(3, "Código"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(4, "Descrição"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(5, "Marca"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            var lst1 = _auxiliaresService.TipoServico.Listar();
            var lst2 = _auxiliaresService.TipoCombustivelService.Listar();
            var lst3 = _auxiliaresService.EstadoService.Listar();
            var lst4 = _auxiliaresService.TipoEquipamentoService.Listar();


            ListaTipoServico = Mapper.Map<List<TipoServico>>(lst1);
            TiposCombustiveis = Mapper.Map<List<TipoCombustivel>>(lst2);
            Estados = Mapper.Map<List<Estados>>(lst3);
            ListaEquipamentos = Mapper.Map<List<TipoEquipamento>>(lst4);
        }

        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>
        /// ValidarCnpj
        public void AtualizarDadosPendencias()
        {
            if (Entity == null)
            {
                return;
            }

            var pendencia = _service.Pendencia.ListarPorVeiculo(Entity.EquipamentoVeiculoId).ToList();
            //Set valores
            PendenciaGeral = false;
            PendenciaRepresentante = false;
            PendenciaContrato = false;
            PendenciaAnexo = false;
            //Buscar pendências referente aos códigos: 21; 12;14;24
            PendenciaGeral = pendencia.Any(n => n.CodPendencia == 21);
            PendenciaRepresentante = pendencia.Any(n => n.CodPendencia == 12);
            PendenciaContrato = pendencia.Any(n => n.CodPendencia == 14);
            PendenciaAnexo = pendencia.Any(n => n.CodPendencia == 24);
            //Indica se a empresa possue pendências
            Pendencias = PendenciaGeral || PendenciaRepresentante || PendenciaContrato || PendenciaAnexo;
        }

        /// <summary>
        ///     Atualizar dados de atividade
        /// </summary>
        public void AtualizarDadosTiposServico()
        {
            if (Entity == null)
            {
                return;
            }

            TiposEquipamentoServico.Clear();
            var id = Entity.EquipamentoVeiculoId;
            var list = _service.Equipamento.ListarEquipamentoVeiculoTipoServicoView(id).ToList();
            var list2 = Mapper.Map<List<EquipamentoVeiculoTipoServicoView>>(list);
            list2.ForEach(n => TiposEquipamentoServico.Add(n));
        }

        #endregion

        #region Commands

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <param name="uf"></param>
        public void ListarMunicipios(string uf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uf))
                {
                    return;
                }

                if (Municipios == null)
                {
                    Municipios = new List<Municipio>();
                }

                if (_municipios == null)
                {
                    _municipios = new List<Municipio>();
                }

                if (Estado == null)
                {
                    return;
                }

                //Verificar se há municipios já carregados...
                var l1 = _municipios.Where(n => n.Uf == uf);
                Municipios.Clear();
                //Nao havendo municipios... obter do repositorio
                if (!l1.Any())
                {
                    var l2 = _auxiliaresService.MunicipioService.Listar(null, uf);
                    _municipios.AddRange(Mapper.Map<List<Municipio>>(l2));
                }

                var municipios = _municipios.Where(n => n.Uf == uf).ToList();
                Municipios = municipios;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase(PrepareCriar, true);

        private void PrepareCriar()
        {
            Entity = new VeiculoView();
            IsEnableTabItem = false;
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = true;
            SelectedTabIndex = 0;
            Comportamento.PrepareCriar();
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            TiposEquipamentoServico.Clear();
        }

        /// <summary>
        ///     Editar
        /// </summary>
        public ICommand PrepareAlterarCommand => new CommandBase(PrepareAlterar, true);

        /// <summary>
        ///     Cancelar
        /// </summary>
        public ICommand PrepareCancelarCommand => new CommandBase(Comportamento.PrepareCancelar, true);

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareSalvarCommand => new CommandBase(Comportamento.PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase(PrepareRemover, true);

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public void Validar()
        {
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase(Pesquisar, true);

        #endregion

        #region Salva Dados

        private void Pesquisar()
        {
            try
            {
                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Descricao
                if (num.Key == 1)
                {
                    var l1 = _service.Listar($"%{pesquisa}%", null);
                    PopularObserver(l1);
                }
                //Por Modelo
                if (num.Key == 2)
                {
                    var l1 = _service.Listar(null, $"%{pesquisa}%");
                    PopularObserver(l1);
                }

                IsEnableLstView = true;
                IsEnableTabItem = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PopularObserver(ICollection<Veiculo> list)
        {
            try
            {
                var list2 = Mapper.Map<List<VeiculoView>>(list);
                EntityObserver = new ObservableCollection<VeiculoView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                return;
            }

            Comportamento.PrepareAlterar();
            IsEnableTabItem = false;
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = false;
            SelectedTabIndex = 0;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            AtualizarDadosTiposServico();
        }

        private void PrepareRemover()
        {
            if (Entity == null)
            {
                return;
            }

            IsEnableLstView = true;
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = false;
            SelectedTabIndex = 0;
            Comportamento.PrepareRemover();
        }

        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null)
                {
                    return;
                }

                var n1 = Mapper.Map<Veiculo>(Entity);
                Validar();
                _service.Criar(n1);
                //Salvar Tipo de Servico
                SalvarTipoServico(n1.EquipamentoVeiculoId);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<VeiculoView>(n1);
                EntityObserver.Insert(0, n2);
                IsEnableTabItem = true;
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void SalvarTipoServico(int veiculoId)
        {
            //Remover
            _service.Equipamento.RemoverPorVeiculo(veiculoId);
            //Adicionar
            var lst = TiposEquipamentoServico.ToList();
            lst.ForEach(n =>
            {
                var n1 = Mapper.Map<EquipamentoVeiculoTipoServico>(n);
                n1.EquipamentoVeiculoId = veiculoId;
                _service.Equipamento.Criar(n1);
            });
        }


        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null)
                {
                    return;
                }

                Validar();
                var n1 = Mapper.Map<Veiculo>(Entity);
                _service.Alterar(n1);
                //Salvar Tipo de Servico
                SalvarTipoServico(n1.EquipamentoVeiculoId);
                IsEnableTabItem = true;
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    IsEnableTabItem = true;
            //    IsEnableLstView = true;
            //    _prepareCriarCommandAcionado = false;
            //    _prepareAlterarCommandAcionado = false;
            //    TiposAtividades.Clear();
            //    TiposLayoutCracha.Clear();
            //}
            //catch (Exception ex)
            //{
            //    Utils.TraceException(ex);
            //    WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            //}
        }

        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Entity == null) return;
                //var result = WpfHelp.MboxDialogRemove();
                //if (result != DialogResult.Yes) return;

                //var n1 = Mapper.Map<Empresa>(Entity);
                //_service.Remover(n1);
                ////Retirar empresa da coleção
                //EntityObserver.Remove(Entity);

                //IsEnableLstView = true;
                //IsEnableTabItem = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        #endregion

        #region Variaveis Privadas

        //private ObservableCollection<ClasseTiposCombustiveis.TipoCombustivel> _TiposCombustiveis;

        //private ObservableCollection<ClasseTiposEquipamentoVeiculo.TipoEquipamentoVeiculo> _TiposEquipamentoVeiculo;

        //private ObservableCollection<ClasseTiposServicos.TipoServico> _TiposServico;

        //private ObservableCollection<ClasseEquipamentoVeiculoTiposServicos.EquipamentoVeiculoServico> _EquipamentosVeiculosTiposServicos;

        //private ObservableCollection<ClasseVeiculos.Veiculo> _Veiculos;

        //private ClasseVeiculos.Veiculo _VeiculoSelecionado;

        //private ClasseVeiculos.Veiculo _veiculoTemp = new ClasseVeiculos.Veiculo();

        //private List<ClasseVeiculos.Veiculo> _VeiculosTemp = new List<ClasseVeiculos.Veiculo>();

        //private ObservableCollection<EstadoView> _Estados;

        //private ObservableCollection<ClasseMunicipios.Municipio> _Municipios;

        //PopupPesquisaVeiculos popupPesquisaVeiculos;

        //PopupMensagem _PopupSalvando;

        //private int _selectedIndex;

        //private int _EmpresaSelecionadaID;

        //private ClasseEquipamentoVeiculoTiposServicos.EquipamentoVeiculoServico _EquipamentoVeiculoTipoServicoSelecionado;

        //private bool _HabilitaEdicao;

        //private string _Criterios = "";

        //private int _selectedIndexTemp;

        //private bool _atualizandoFoto;

        //private BitmapImage _Waiting;

        ////private bool _EditandoUserControl;

        #endregion

        #region Contrutores

        //public ObservableCollection<ClasseTiposCombustiveis.TipoCombustivel> TiposCombustiveis
        //{
        //    get
        //    {
        //        return _TiposCombustiveis;
        //    }

        //    set
        //    {
        //        if (_TiposCombustiveis != value)
        //        {
        //            _TiposCombustiveis = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseTiposServicos.TipoServico> TiposServico
        //{
        //    get
        //    {
        //        return _TiposServico;
        //    }

        //    set
        //    {
        //        if (_TiposServico != value)
        //        {
        //            _TiposServico = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseTiposEquipamentoVeiculo.TipoEquipamentoVeiculo> TiposEquipamentoVeiculo
        //{
        //    get
        //    {
        //        return _TiposEquipamentoVeiculo;
        //    }

        //    set
        //    {
        //        if (_TiposEquipamentoVeiculo != value)
        //        {
        //            _TiposEquipamentoVeiculo = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseVeiculos.Veiculo> Veiculos
        //{
        //    get
        //    {
        //        return _Veiculos;
        //    }

        //    set
        //    {
        //        if (_Veiculos != value)
        //        {
        //            _Veiculos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ClasseVeiculos.Veiculo VeiculoSelecionado
        //{
        //    get
        //    {

        //        return _VeiculoSelecionado;
        //    }
        //    set
        //    {
        //        _VeiculoSelecionado = value;
        //        base.OnPropertyChanged("SelectedItem");
        //        if (VeiculoSelecionado != null)
        //        {
        //            if (!_atualizandoFoto)
        //            {
        //                Thread OnEmpresaSelecionada_thr = new Thread(() => OnVeiculoEquipamentoSelecionado());
        //                OnEmpresaSelecionada_thr.Start();
        //                Thread CarregaFoto_thr = new Thread(() => CarregaFoto(VeiculoSelecionado.EquipamentoVeiculoID));
        //                CarregaFoto_thr.Start();
        //            }

        //        }

        //    }
        //}
        //public int EmpresaSelecionadaID
        //{
        //    get
        //    {
        //        return _EmpresaSelecionadaID;
        //    }
        //    set
        //    {
        //        _EmpresaSelecionadaID = value;
        //        base.OnPropertyChanged();
        //        if (EmpresaSelecionadaID != null)
        //        {
        //            // OnEmpresaSelecionada();
        //        }

        //    }
        //}
        //public ClasseEquipamentoVeiculoTiposServicos.EquipamentoVeiculoServico EquipamentoVeiculoTipoServicoSelecionado
        //{
        //    get
        //    {
        //        return _EquipamentoVeiculoTipoServicoSelecionado;
        //    }
        //    set
        //    {
        //        _EquipamentoVeiculoTipoServicoSelecionado = value;
        //        base.OnPropertyChanged("SelectedItem");
        //        if (_EquipamentoVeiculoTipoServicoSelecionado != null)
        //        {
        //            // OnEmpresaSelecionada();
        //        }

        //    }
        //}
        //public int SelectedIndex
        //{
        //    get
        //    {
        //        return _selectedIndex;
        //    }
        //    set
        //    {
        //        _selectedIndex = value;
        //        OnPropertyChanged("SelectedIndex");
        //    }
        //}
        //public bool HabilitaEdicao
        //{
        //    get
        //    {
        //        return _HabilitaEdicao;
        //    }
        //    set
        //    {
        //        _HabilitaEdicao = value;
        //        base.OnPropertyChanged();
        //    }
        //}
        ////public bool EditandoUserControl
        ////{
        ////    get
        ////    {
        ////        return _EditandoUserControl;
        ////    }
        ////    set
        ////    {
        ////        SetProperty(ref _EditandoUserControl, value);
        ////    }
        ////}
        //public string Criterios
        //{
        //    get
        //    {
        //        return _Criterios;
        //    }
        //    set
        //    {
        //        _Criterios = value;
        //        base.OnPropertyChanged();
        //    }
        //}
        //public ObservableCollection<EstadoView> Estados
        //{
        //    get
        //    {
        //        return _Estados;
        //    }

        //    set
        //    {
        //        if (_Estados != value)
        //        {
        //            _Estados = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseMunicipios.Municipio> Municipios
        //{
        //    get
        //    {
        //        return _Municipios;
        //    }

        //    set
        //    {
        //        if (_Municipios != value)
        //        {
        //            _Municipios = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseEquipamentoVeiculoTiposServicos.EquipamentoVeiculoServico> EquipamentosVeiculosTiposServicos
        //{
        //    get
        //    {
        //        return _EquipamentosVeiculosTiposServicos;
        //    }

        //    set
        //    {
        //        if (_EquipamentosVeiculosTiposServicos != value)
        //        {
        //            _EquipamentosVeiculosTiposServicos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public BitmapImage Waiting
        //{
        //    get
        //    {
        //        return _Waiting;
        //    }
        //    set
        //    {
        //        _Waiting = value;
        //        base.OnPropertyChanged();
        //    }
        //}

        #endregion

        #region Comandos dos Botoes

        //public void OnAbrirArquivoCommand()
        //{
        //    try
        //    {
        //        var arquivoStr = VeiculoSelecionado.NomeArquivoAnexo;
        //        var nomeArquivo = VeiculoSelecionado.ArquivoAnexo;
        //        var arrBytes = Convert.FromBase64String(arquivoStr);
        //        WpfHelp.DownloadArquivoDialog(nomeArquivo, arrBytes);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }

        //}

        //public void OnEditarCommand()
        //{
        //    try
        //    {
        //        //BuscaBadges();
        //        _veiculoTemp = VeiculoSelecionado.CriaCopia(VeiculoSelecionado);
        //        _selectedIndexTemp = SelectedIndex;
        //        HabilitaEdicao = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void OnCancelarEdicaoCommand()
        //{
        //    try
        //    {
        //        Veiculos[_selectedIndexTemp] = _veiculoTemp;
        //        SelectedIndex = _selectedIndexTemp;
        //        HabilitaEdicao = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public async Task OnSalvarEdicaoCommandAsync()
        //{
        //    try
        //    {
        //        _PopupSalvando = new PopupMensagem("Aguarde, salvando ...");

        //        Thread PopupSalvando_thr = new Thread(() => AbrePopupSalvando());

        //        PopupSalvando_thr.Start();

        //        await Task.Run(() => SalvarEdicao());

        //        _PopupSalvando.Close();

        //        _PopupSalvando = null;

        //    }
        //    catch (Exception ex)
        //    {
        //        if (_PopupSalvando != null)
        //        {
        //            _PopupSalvando.Close();
        //        }
        //        Utils.TraceException(ex);
        //    }
        //}

        //public async Task OnSalvarAdicaoCommandAsync()
        //{

        //    try
        //    {
        //        _PopupSalvando = new PopupMensagem("Aguarde, salvando ...");

        //        Thread PopupSalvando_thr = new Thread(() => AbrePopupSalvando());

        //        PopupSalvando_thr.Start();

        //        await Task.Run(() => SalvarAdicao());

        //        _PopupSalvando.Close();

        //        _PopupSalvando = null;

        //    }
        //    catch (Exception ex)
        //    {
        //        if (_PopupSalvando != null)
        //        {
        //            _PopupSalvando.Close();
        //        }
        //        Utils.TraceException(ex);

        //    }
        //}

        //public void OnAdicionarCommand()
        //{
        //    try
        //    {
        //        foreach (var x in Veiculos)
        //        {
        //            _VeiculosTemp.Add(x);
        //        }

        //        _selectedIndexTemp = SelectedIndex;
        //        Veiculos.Clear();
        //        //EquipamentosVeiculosTiposServicos.Clear();

        //        _veiculoTemp = new ClasseVeiculos.Veiculo();

        //        _veiculoTemp.EquipamentoVeiculoID = EmpresaSelecionadaID;  //OBS

        //        Veiculos.Add(_veiculoTemp);
        //        Veiculos.Add(new ClasseVeiculos.Veiculo());
        //        SelectedIndex = 0;
        //        HabilitaEdicao = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }

        //}

        //public void OnCancelarAdicaoCommand()
        //{
        //    try
        //    {
        //        Veiculos = null;
        //        Veiculos = new ObservableCollection<ClasseVeiculos.Veiculo>(_VeiculosTemp);
        //        SelectedIndex = _selectedIndexTemp;
        //        _VeiculosTemp.Clear();
        //        HabilitaEdicao = false;
        //        _atualizandoFoto = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void OnExcluirCommand()
        //{
        //    try
        //    {
        //        if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
        //        {
        //            if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
        //            {
        //                var service = new VeiculoService();
        //                var emp = service.BuscarPelaChave(VeiculoSelecionado.EquipamentoVeiculoID);
        //                service.Remover(emp);

        //                Veiculos.Remove(VeiculoSelecionado);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }

        //}

        //public void OnPesquisarCommand()
        //{
        //    try
        //    {

        //        popupPesquisaVeiculos = new PopupPesquisaVeiculos();
        //        popupPesquisaVeiculos.EfetuarProcura += On_EfetuarProcura;
        //        popupPesquisaVeiculos.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void On_EfetuarProcura(object sender, EventArgs e)
        //{
        //    object vetor = popupPesquisaVeiculos.Criterio.Split((char)(20));
        //    int _codigo;
        //    if ((((string[])vetor)[0] == null) || (((string[])vetor)[0] == ""))
        //    {
        //        _codigo = 0;
        //    }
        //    else
        //    {
        //        _codigo = Convert.ToInt32(((string[])vetor)[0]);
        //    }
        //    string _nome = ((string[])vetor)[1];

        //    CarregaColecaoVeiculos(_codigo, _nome);
        //    SelectedIndex = 0;
        //}

        //public void OnAbrirPendencias(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        PopupPendencias popupPendencias = new PopupPendencias(3, ((FrameworkElement)e.OriginalSource).Tag, VeiculoSelecionado.EquipamentoVeiculoID, VeiculoSelecionado.Placa_Identificador);
        //        popupPendencias.ShowDialog();
        //        popupPendencias = null;
        //        CarregaColecaoVeiculos(VeiculoSelecionado.EquipamentoVeiculoID);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void OnAbrirPendenciaGeral(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //var popupPendencias = 
        //        //    new PopupPendencias(2, ((FrameworkElement)e.OriginalSource).Tag, ColaboradorSelecionado.ColaboradorID, ColaboradorSelecionado.Nome);
        //        //popupPendencias.ShowDialog();
        //        //popupPendencias = null;
        //        //CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);

        //        var frm = new PopupPendencias();
        //        frm.Inicializa(21, VeiculoSelecionado.EquipamentoVeiculoID, PendenciaTipo.Veiculo);
        //        frm.ShowDialog();
        //        //CarregaColecaoVeiculos(VeiculoSelecionado.EquipamentoVeiculoID);

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void OnAbrirPendenciaContratos(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var frm = new PopupPendencias();
        //        frm.Inicializa(14, VeiculoSelecionado.EquipamentoVeiculoID, PendenciaTipo.Veiculo);
        //        frm.ShowDialog();
        //        //CarregaColecaoVeiculos(VeiculoSelecionado.EquipamentoVeiculoID);

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void OnAbrirPendenciaAnexos(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var frm = new PopupPendencias();
        //        frm.Inicializa(24, VeiculoSelecionado.EquipamentoVeiculoID, PendenciaTipo.Veiculo);
        //        frm.ShowDialog();
        //        //CarregaColecaoVeiculos(VeiculoSelecionado.EquipamentoVeiculoID);

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void OnAbrirPendenciaCredenciais(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var frm = new PopupPendencias();
        //        frm.Inicializa(25, VeiculoSelecionado.EquipamentoVeiculoID, PendenciaTipo.Veiculo);
        //        frm.ShowDialog();
        //        //CarregaColecaoVeiculos(VeiculoSelecionado.EquipamentoVeiculoID);

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //public void OnAbrirPendenciaSeguro(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var frm = new PopupPendencias();
        //        frm.Inicializa(19, VeiculoSelecionado.EquipamentoVeiculoID, PendenciaTipo.Veiculo);
        //        frm.ShowDialog();
        //        //CarregaColecaoVeiculos(VeiculoSelecionado.EquipamentoVeiculoID);

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void OnInserirServicoCommand(string _TipoServicoIDstr, string _Descricao)
        //{
        //    try
        //    {
        //        int _TipoServicoID = Convert.ToInt32(_TipoServicoIDstr);
        //        ClasseEquipamentoVeiculoTiposServicos.EquipamentoVeiculoServico _EquipamentoVeiculoTiposServico = new ClasseEquipamentoVeiculoTiposServicos.EquipamentoVeiculoServico();
        //        _EquipamentoVeiculoTiposServico.EquipamentoVeiculoId = VeiculoSelecionado.EquipamentoVeiculoID;
        //        _EquipamentoVeiculoTiposServico.TipoServicoId = _TipoServicoID;
        //        _EquipamentoVeiculoTiposServico.Descricao = _Descricao;

        //        var entity = _EquipamentoVeiculoTiposServico;
        //        var entityConv = Mapper.Map<EquipamentoVeiculoTipoServico>(entity);

        //        equipVeiculoService.Criar(entityConv);

        //        CarregaColecaoVeiculoEquipTipoServicos(_EquipamentoVeiculoTiposServico.EquipamentoVeiculoId);

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void OnExcluirEquipamentoVeiculoTipoServicoCommand()
        //{
        //    try
        //    {
        //        if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
        //        {
        //            var entity = EquipamentoVeiculoTipoServicoSelecionado;
        //            var entityConv = Mapper.Map<EquipamentoVeiculoTipoServico>(entity);
        //            equipVeiculoService.Remover(entityConv);

        //            CarregaColecaoVeiculoEquipTipoServicos(VeiculoSelecionado.EquipamentoVeiculoID);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        #endregion

        #region Carregamento das Colecoes

        //private void CarregaColecaoVeiculos(int VeiculoID = 0, string nome = "", string apelido = "", string cpf = "")
        //{
        //    try
        //    {
        //        var service = new VeiculoService();
        //        if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
        //        if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
        //        if (!string.IsNullOrWhiteSpace(cpf)) cpf = $"%{cpf}%";

        //        var list1 = service.Listar(VeiculoID, nome, apelido, cpf);
        //        var list2 = Mapper.Map<List<ClasseVeiculos.Veiculo>>(list1.OrderByDescending(a => a.EquipamentoVeiculoId));

        //        var observer = new ObservableCollection<ClasseVeiculos.Veiculo>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        Veiculos = observer;

        //        SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //private void CarregaColecaoTiposServicos()
        //{
        //    try
        //    {
        //        var service = new TipoServicoService();
        //        var list1 = service.Listar();

        //        var list2 = Mapper.Map<List<ClasseTiposServicos.TipoServico>>(list1);
        //        var observer = new ObservableCollection<ClasseTiposServicos.TipoServico>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });
        //        TiposServico = observer;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void CarregaColecaoVeiculoEquipTipoServicos(int _VeiculoId = 0)
        //{
        //    try
        //    {
        //        var list1 = equipVeiculoService.ListarEquipamentoVeiculoTipoServicoView(_VeiculoId);

        //        var list2 = Mapper.Map<List<ClasseEquipamentoVeiculoTiposServicos.EquipamentoVeiculoServico>>(list1);
        //        var observer = new ObservableCollection<ClasseEquipamentoVeiculoTiposServicos.EquipamentoVeiculoServico>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });
        //        EquipamentosVeiculosTiposServicos = observer;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //private void CarregaColecaoTiposEquipamentoVeiculo()
        //{
        //    try
        //    {
        //        string _xml = RequisitaTiposEquipamentoVeiculo();

        //        XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposEquipamentoVeiculo));

        //        XmlDocument xmldocument = new XmlDocument();
        //        xmldocument.LoadXml(_xml);

        //        TextReader reader = new StringReader(_xml);
        //        ClasseTiposEquipamentoVeiculo classeTiposEquipamentoVeiculo = new ClasseTiposEquipamentoVeiculo();
        //        classeTiposEquipamentoVeiculo = (ClasseTiposEquipamentoVeiculo)deserializer.Deserialize(reader);
        //        TiposEquipamentoVeiculo = new ObservableCollection<ClasseTiposEquipamentoVeiculo.TipoEquipamentoVeiculo>();
        //        TiposEquipamentoVeiculo = classeTiposEquipamentoVeiculo.TiposEquipamentoVeiculo;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
        //    }
        //}

        //private void CarregaColecaoTiposCombustiveis()
        //{
        //    try
        //    {
        //        var service = new TipoCombustivelService();
        //        var list1 = service.Listar();

        //        var list2 = Mapper.Map<List<ClasseTiposCombustiveis.TipoCombustivel>>(list1);
        //        var observer = new ObservableCollection<ClasseTiposCombustiveis.TipoCombustivel>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });
        //        TiposCombustiveis = observer;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //private void CarregaColeçãoEstados()
        //{
        //    try
        //    {

        //        //string _xml = RequisitaEstados();

        //        //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEstados));
        //        //XmlDocument xmldocument = new XmlDocument();
        //        //xmldocument.LoadXml(_xml);
        //        //TextReader reader = new StringReader(_xml);
        //        //ClasseEstados classeEstados = new ClasseEstados();
        //        //classeEstados = (ClasseEstados)deserializer.Deserialize(reader);
        //        //Estados = new ObservableCollection<EstadoView>();
        //        //Estados = EstadoViews;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void CarregaColeçãoMunicipios(string _EstadoUF = "%")
        //{
        //    try
        //    {

        //        string _xml = RequisitaMunicipios(_EstadoUF);

        //        XmlSerializer deserializer = new XmlSerializer(typeof(ClasseMunicipios));
        //        XmlDocument DataFile = new XmlDocument();
        //        DataFile.LoadXml(_xml);
        //        TextReader reader = new StringReader(_xml);
        //        ClasseMunicipios classeMunicipios = new ClasseMunicipios();
        //        classeMunicipios = (ClasseMunicipios)deserializer.Deserialize(reader);
        //        Municipios = new ObservableCollection<ClasseMunicipios.Municipio>();
        //        Municipios = classeMunicipios.Municipios;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //private void CarregaFoto(int _VeiculoID)
        //{
        //    try
        //    {
        //        _atualizandoFoto = true; //para que o evento de VeiculoSelecionado não entre em looping
        //        ///
        //        //                BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
        //        //                string _imgstr = Conversores.IMGtoSTR(_img);
        //        //                VeiculoSelecionado.Foto = _imgstr;

        //        //                System.Windows.Application.Current.Dispatcher.Invoke(
        //        //(Action)(() => {
        //        //_veiculoTemp = VeiculoSelecionado.CriaCopia(VeiculoSelecionado);
        //        //_selectedIndexTemp = SelectedIndex;

        //        //_veiculoTemp.Foto = _imgstr;
        //        //Veiculos[_selectedIndexTemp] = _veiculoTemp;

        //        //SelectedIndex = _selectedIndexTemp;

        //        //}));

        //        Application.Current.Dispatcher.Invoke(() =>
        //        {
        //            Waiting = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Waitng.gif", UriKind.Absolute));

        //            Waiting.Freeze();
        //        });

        //        string _xmlstring = BuscaFoto(_VeiculoID);

        //        Application.Current.Dispatcher.Invoke(() => { Waiting = null; });

        //        XmlDocument xmldocument = new XmlDocument();

        //        xmldocument.LoadXml(_xmlstring);

        //        XmlNode node = xmldocument.DocumentElement;

        //        XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

        //        if (arquivoNode.HasChildNodes)
        //        {
        //            Application.Current.Dispatcher.Invoke(() =>
        //            {
        //                _veiculoTemp = VeiculoSelecionado.CriaCopia(VeiculoSelecionado);

        //                _selectedIndexTemp = SelectedIndex;

        //                _veiculoTemp.Foto = arquivoNode.FirstChild.Value;

        //                Veiculos[_selectedIndexTemp] = _veiculoTemp;

        //                SelectedIndex = _selectedIndexTemp;

        //            });
        //        }
        //        _atualizandoFoto = false;

        //    }
        //    catch (Exception ex)
        //    {
        //        _atualizandoFoto = false;
        //        Utils.TraceException(ex);
        //    }
        //}

        #endregion

        #region Data Access

        //private string RequisitaVeiculos(int _veiculoID = 0, string _Placa_Identificador = "", string _renavam = "", string _descricao = "", int _excluida = 0, string _quantidaderegistro = "500")
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseVeiculos = _xmlDocument.CreateElement("ClasseVeiculos");
        //        _xmlDocument.AppendChild(_ClasseVeiculos);

        //        XmlNode _Veiculos = _xmlDocument.CreateElement("Veiculos");
        //        _ClasseVeiculos.AppendChild(_Veiculos);

        //        string _strSql;

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        string _veiculoIDSTR = "";

        //        _veiculoIDSTR = _veiculoID == 0 ? "" : " AND EquipamentoVeiculoID = " + _veiculoID;
        //        _Placa_Identificador = _Placa_Identificador == "" ? "" : " AND Placa_Identificador like '%" + _Placa_Identificador + "%' ";
        //        _renavam = _renavam == "" ? "" : "AND Renavam like '%" + _renavam + "%' ";
        //        _descricao = _descricao == "" ? "" : " AND Descricao like '%" + _descricao + "%'";

        //        //////////////////////////////////////////////
        //        ////string ArquivoXML = "C:\\temp\\ArquivoXMLGerado.xml";

        //        //_strSql = "VeiculoID,Descricao,Placa_Identificador,Frota,Patrimonio,Marca,Modelo,Tipo,Cor,Ano,EstadoID,MunicipioID,Serie_Chassi," +
        //        //    "CombustivelID,Altura,Comprimento,Largura,Foto,Excluida,StatusID,TipoAcessoID,DescricaoAnexo,NomeArquivoAnexo,Pendente31,Pendente32,Pendente33,Pendente34 " +
        //        //     "from Veiculos where Excluida  = " + _excluida + _veiculoIDSTR + _Placa_Identificador + _renavam + _descricao + " order by VeiculoID desc";

        //        //if (_quantidaderegistro == "0")
        //        //{
        //        //    _strSql = "Select " + _strSql;
        //        //}
        //        //else
        //        //{
        //        //    _strSql = "select Top " + _quantidaderegistro + " " + _strSql;
        //        //}

        //        //SqlDataAdapter da = new SqlDataAdapter(_strSql, _Con);

        //        ////definindo o dataset
        //        //DataSet ds = new DataSet();
        //        ////da.Fill(ds,"table");
        //        ////var t = ds.GetXml();
        //        //da.Fill(ds, "Veiculo");
        //        ////preenchendo o dataset

        //        ////ds.WriteXml (ArquivoXML);
        //        ////string _xml2 = ds.GetXml();
        //        //return ds.GetXml();
        //        /////////////////////////////////////////////
        //        //_strSql = "VeiculoID,Descricao,Tipo,Marca,Modelo,Ano,Cor,Placa_Identificador,Renavam,EstadoID,MunicipioID,Foto,Excluida," +
        //        //    "StatusID,TipoAcessoID,DescricaoAnexo,NomeArquivoAnexo,Pendente31,Pendente32,Pendente33,Pendente34 " +
        //        //     "from Veiculos where Excluida  = " + _excluida + _veiculoIDSTR + _Placa_Identificador + _renavam + _descricao + " order by VeiculoID desc";

        //        _strSql = "EquipamentoVeiculoID,Descricao,Placa_Identificador,Frota,Patrimonio,Marca,Modelo,Tipo,Cor,Ano,EstadoID,MunicipioID,Serie_Chassi," +
        //            "CombustivelID,Altura,Comprimento,Largura,TipoEquipamentoVeiculoID,Foto,Excluida,StatusID,TipoAcessoID,DescricaoAnexo,NomeArquivoAnexo,Pendente31,Pendente32,Pendente33,Pendente34 " +
        //             "from Veiculos where Excluida  = " + _excluida + _veiculoIDSTR + _Placa_Identificador + _renavam + _descricao + " order by EquipamentoVeiculoID desc";

        //        if (_quantidaderegistro == "0")
        //        {
        //            _strSql = "Select " + _strSql;
        //        }
        //        else
        //        {
        //            _strSql = "select Top " + _quantidaderegistro + " " + _strSql;
        //        }

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);

        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);

        //        //string dt1;
        //        //DateTime dt2;

        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _Veiculo = _xmlDocument.CreateElement("Veiculo");
        //            _Veiculos.AppendChild(_Veiculo);

        //            XmlNode _VeiculoID = _xmlDocument.CreateElement("EquipamentoVeiculoID");
        //            _VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EquipamentoVeiculoID"].ToString())));
        //            _Veiculo.AppendChild(_VeiculoID);

        //            XmlNode _Node2 = _xmlDocument.CreateElement("Descricao");
        //            _Node2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
        //            _Veiculo.AppendChild(_Node2);

        //            XmlNode _Node3 = _xmlDocument.CreateElement("Placa_Identificador");
        //            _Node3.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Placa_Identificador"].ToString())));
        //            _Veiculo.AppendChild(_Node3);

        //            XmlNode _Node4 = _xmlDocument.CreateElement("Frota");
        //            _Node4.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Frota"].ToString())));
        //            _Veiculo.AppendChild(_Node4);

        //            XmlNode _Node5 = _xmlDocument.CreateElement("Patrimonio");
        //            _Node5.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Patrimonio"].ToString())));
        //            _Veiculo.AppendChild(_Node5);

        //            XmlNode _Node6 = _xmlDocument.CreateElement("Marca");
        //            _Node6.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Marca"].ToString())));
        //            _Veiculo.AppendChild(_Node6);

        //            XmlNode _Node7 = _xmlDocument.CreateElement("Modelo");
        //            _Node7.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Modelo"].ToString())));
        //            _Veiculo.AppendChild(_Node7);

        //            XmlNode _Node8 = _xmlDocument.CreateElement("Tipo");
        //            _Node8.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Tipo"].ToString())));
        //            _Veiculo.AppendChild(_Node8);

        //            XmlNode _Node9 = _xmlDocument.CreateElement("Cor");
        //            _Node9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cor"].ToString())));
        //            _Veiculo.AppendChild(_Node9);

        //            XmlNode _Node10 = _xmlDocument.CreateElement("Ano");
        //            _Node10.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Ano"].ToString())));
        //            _Veiculo.AppendChild(_Node10);

        //            XmlNode _Node11 = _xmlDocument.CreateElement("EstadoID");
        //            _Node10.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
        //            _Veiculo.AppendChild(_Node10);

        //            XmlNode _Node12 = _xmlDocument.CreateElement("MunicipioID");
        //            _Node12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
        //            _Veiculo.AppendChild(_Node12);

        //            XmlNode _Node13 = _xmlDocument.CreateElement("Serie_Chassi");
        //            _Node13.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Serie_Chassi"].ToString())));
        //            _Veiculo.AppendChild(_Node13);

        //            XmlNode _Node14 = _xmlDocument.CreateElement("CombustivelID");
        //            _Node14.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CombustivelID"].ToString())));
        //            _Veiculo.AppendChild(_Node14);

        //            XmlNode _Node15 = _xmlDocument.CreateElement("Altura");
        //            _Node15.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Altura"].ToString())));
        //            _Veiculo.AppendChild(_Node15);

        //            XmlNode _Node16 = _xmlDocument.CreateElement("Comprimento");
        //            _Node16.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Comprimento"].ToString())));
        //            _Veiculo.AppendChild(_Node16);

        //            XmlNode _Node17 = _xmlDocument.CreateElement("Largura");
        //            _Node17.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Largura"].ToString())));
        //            _Veiculo.AppendChild(_Node17);

        //            XmlNode _Node18 = _xmlDocument.CreateElement("Foto");
        //            //_Node12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Foto"].ToString())));
        //            _Veiculo.AppendChild(_Node18);

        //            XmlNode _Node19 = _xmlDocument.CreateElement("Excluida");
        //            _Node19.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
        //            _Veiculo.AppendChild(_Node19);

        //            XmlNode _Node20 = _xmlDocument.CreateElement("StatusID");
        //            _Node20.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
        //            _Veiculo.AppendChild(_Node20);

        //            XmlNode _Node21 = _xmlDocument.CreateElement("TipoAcessoID");
        //            _Node21.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
        //            _Veiculo.AppendChild(_Node21);

        //            XmlNode _Node22 = _xmlDocument.CreateElement("TipoEquipamentoVeiculoID");
        //            _Node22.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoEquipamentoVeiculoID"].ToString())));
        //            _Veiculo.AppendChild(_Node22);

        //            XmlNode _DescricaoAnexo = _xmlDocument.CreateElement("DescricaoAnexo");
        //            _DescricaoAnexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DescricaoAnexo"].ToString()).Trim()));
        //            _Veiculo.AppendChild(_DescricaoAnexo);

        //            XmlNode _NomeArquivoAnexo = _xmlDocument.CreateElement("NomeArquivoAnexo");
        //            _NomeArquivoAnexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivoAnexo"].ToString()).Trim()));
        //            _Veiculo.AppendChild(_NomeArquivoAnexo);

        //            XmlNode _ArquivoAnexo = _xmlDocument.CreateElement("ArquivoAnexo");
        //            //_ArquivoAnexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ArquivoAnexo"].ToString())));
        //            _Veiculo.AppendChild(_ArquivoAnexo);

        //            XmlNode _Pendente1 = _xmlDocument.CreateElement("Pendente31");
        //            _Pendente1.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente31"])).ToString()));
        //            _Veiculo.AppendChild(_Pendente1);

        //            XmlNode _Pendente2 = _xmlDocument.CreateElement("Pendente32");
        //            _Pendente2.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente32"])).ToString()));
        //            _Veiculo.AppendChild(_Pendente2);

        //            XmlNode _Pendente3 = _xmlDocument.CreateElement("Pendente33");
        //            _Pendente3.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente33"])).ToString()));
        //            _Veiculo.AppendChild(_Pendente3);

        //            XmlNode _Pendente4 = _xmlDocument.CreateElement("Pendente34");
        //            _Pendente4.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente34"])).ToString()));
        //            _Veiculo.AppendChild(_Pendente4);

        //            bool _pend = false;
        //            _pend = (bool)_sqlreader["Pendente31"] ||
        //                (bool)_sqlreader["Pendente32"] ||
        //                (bool)_sqlreader["Pendente33"] ||
        //                (bool)_sqlreader["Pendente34"];

        //            XmlNode _Pendente = _xmlDocument.CreateElement("Pendente");
        //            _Pendente.AppendChild(_xmlDocument.CreateTextNode(Convert.ToInt32(_pend).ToString()));
        //            _Veiculo.AppendChild(_Pendente);

        //        }

        //        _sqlreader.Close();

        //        _Con.Close();
        //        string _xml = _xmlDocument.InnerXml;
        //        _xmlDocument = null;
        //        //InsereVeiculosBD("");

        //        return _xml;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //    return null;
        //}
        //private string RequisitaEstados()
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseEstados = _xmlDocument.CreateElement("ClasseEstados");
        //        _xmlDocument.AppendChild(_ClasseEstados);

        //        XmlNode _Estados = _xmlDocument.CreateElement("Estados");
        //        _ClasseEstados.AppendChild(_Estados);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
        //        SqlCommand _sqlcmd = new SqlCommand("select * from Estados", _Con);
        //        SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
        //        while (_sqldatareader.Read())
        //        {
        //            XmlNode _Estado = _xmlDocument.CreateElement("Estado");
        //            _Estados.AppendChild(_Estado);

        //            XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
        //            _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EstadoID"].ToString())));
        //            _Estado.AppendChild(_EstadoID);

        //            XmlNode _Nome = _xmlDocument.CreateElement("Nome");
        //            _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString())));
        //            _Estado.AppendChild(_Nome);

        //            XmlNode _UF = _xmlDocument.CreateElement("UF");
        //            _UF.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["UF"].ToString())));
        //            _Estado.AppendChild(_UF);
        //        }
        //        _sqldatareader.Close();
        //        _Con.Close();
        //        return _xmlDocument.InnerXml;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void RequisitaEstados ex: " + ex);

        //        return null;
        //    }
        //}
        //private string RequisitaMunicipios(string _EstadoUF = "%")
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseMunicipios = _xmlDocument.CreateElement("ClasseMunicipios");
        //        _xmlDocument.AppendChild(_ClasseMunicipios);

        //        XmlNode _Municipios = _xmlDocument.CreateElement("Municipios");
        //        _ClasseMunicipios.AppendChild(_Municipios);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
        //        SqlCommand _sqlcmd = new SqlCommand("select * from Municipios where UF Like '" + _EstadoUF + "'", _Con);
        //        SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
        //        while (_sqldatareader.Read())
        //        {
        //            XmlNode _Municipio = _xmlDocument.CreateElement("Municipio");
        //            _Municipios.AppendChild(_Municipio);

        //            XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
        //            _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["MunicipioID"].ToString())));
        //            _Municipio.AppendChild(_MunicipioID);

        //            XmlNode _Nome = _xmlDocument.CreateElement("Nome");
        //            _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString())));
        //            _Municipio.AppendChild(_Nome);

        //            XmlNode _UF = _xmlDocument.CreateElement("UF");
        //            _UF.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["UF"].ToString())));
        //            _Municipio.AppendChild(_UF);
        //        }
        //        _sqldatareader.Close();
        //        _Con.Close();
        //        return _xmlDocument.InnerXml;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void RequisitaMunicipios ex: " + ex);

        //        return null;
        //    }
        //}
        //private string RequisitaTiposServicos()
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseTiposServicos = _xmlDocument.CreateElement("ClasseTiposServicos");
        //        _xmlDocument.AppendChild(_ClasseTiposServicos);

        //        XmlNode _TiposServicos = _xmlDocument.CreateElement("TiposServicos");
        //        _ClasseTiposServicos.AppendChild(_TiposServicos);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
        //        SqlCommand _sqlcmd = new SqlCommand("select * from TipoServico order by TipoServicoID", _Con);
        //        SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
        //        while (_sqldatareader.Read())
        //        {
        //            XmlNode _TipoServico = _xmlDocument.CreateElement("TipoServico");
        //            _TiposServicos.AppendChild(_TipoServico);

        //            XmlNode _TipoServicoID = _xmlDocument.CreateElement("TipoServicoID");
        //            _TipoServicoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoServicoID"].ToString())));
        //            _TipoServico.AppendChild(_TipoServicoID);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
        //            _TipoServico.AppendChild(_Descricao);

        //        }
        //        _sqldatareader.Close();
        //        _Con.Close();
        //        return _xmlDocument.InnerXml;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void RequisitaTiposAtividades ex: " + ex);

        //        return null;
        //    }
        //}
        //private string RequisitaTiposEquipamentoVeiculo()
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseTiposEquipamentoVeiculo = _xmlDocument.CreateElement("ClasseTiposEquipamentoVeiculo");
        //        _xmlDocument.AppendChild(_ClasseTiposEquipamentoVeiculo);

        //        XmlNode _TiposEquipamentoVeiculo = _xmlDocument.CreateElement("TiposEquipamentoVeiculo");
        //        _ClasseTiposEquipamentoVeiculo.AppendChild(_TiposEquipamentoVeiculo);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
        //        SqlCommand _sqlcmd = new SqlCommand("select * from TipoEquipamentoVeiculo order by TipoEquipamentoViculoID", _Con);
        //        SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
        //        while (_sqldatareader.Read())
        //        {
        //            XmlNode _TipoEquipamentoVeiculo = _xmlDocument.CreateElement("TipoEquipamentoVeiculo");
        //            _TiposEquipamentoVeiculo.AppendChild(_TipoEquipamentoVeiculo);

        //            XmlNode _TipoEquipamentoViculoID = _xmlDocument.CreateElement("TipoEquipamentoViculoID");
        //            _TipoEquipamentoViculoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoEquipamentoViculoID"].ToString())));
        //            _TipoEquipamentoVeiculo.AppendChild(_TipoEquipamentoViculoID);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
        //            _TipoEquipamentoVeiculo.AppendChild(_Descricao);

        //        }
        //        _sqldatareader.Close();
        //        _Con.Close();
        //        return _xmlDocument.InnerXml;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void RequisitaTiposAtividades ex: " + ex);

        //        return null;
        //    }
        //}
        //private string RequisitaTiposCombustiveis()
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseTiposCombustiveis = _xmlDocument.CreateElement("ClasseTiposCombustiveis");
        //        _xmlDocument.AppendChild(_ClasseTiposCombustiveis);

        //        XmlNode _TiposCombustiveis = _xmlDocument.CreateElement("TiposCombustiveis");
        //        _ClasseTiposCombustiveis.AppendChild(_TiposCombustiveis);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
        //        SqlCommand _sqlcmd = new SqlCommand("select * from TipoCombustivel order by TipoCombustivelID", _Con);
        //        SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
        //        while (_sqldatareader.Read())
        //        {
        //            XmlNode _TipoCombustivel = _xmlDocument.CreateElement("TipoCombustivel");
        //            _TiposCombustiveis.AppendChild(_TipoCombustivel);

        //            XmlNode _TipoCombustivelID = _xmlDocument.CreateElement("TipoCombustivelID");
        //            _TipoCombustivelID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoCombustivelID"].ToString())));
        //            _TipoCombustivel.AppendChild(_TipoCombustivelID);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
        //            _TipoCombustivel.AppendChild(_Descricao);

        //        }
        //        _sqldatareader.Close();
        //        _Con.Close();
        //        return _xmlDocument.InnerXml;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void RequisitaTiposAtividades ex: " + ex);

        //        return null;
        //    }
        //}
        //private int InsereVeiculosBD(string xmlString)
        //{
        //    try
        //    {
        //        int _novID = 0;

        //        System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

        //        _xmlDoc.LoadXml(xmlString);
        //        // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
        //        ClasseVeiculos.Veiculo _Veiculo = new ClasseVeiculos.Veiculo();
        //        //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
        //        //{
        //        int i = 0;
        //        _Veiculo.EquipamentoVeiculoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EquipamentoVeiculoID")[i].InnerText);
        //        _Veiculo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
        //        _Veiculo.Placa_Identificador = _xmlDoc.GetElementsByTagName("Placa_Identificador")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Placa_Identificador")[i].InnerText;
        //        _Veiculo.Frota = _xmlDoc.GetElementsByTagName("Frota")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Frota")[i].InnerText;
        //        _Veiculo.Patrimonio = _xmlDoc.GetElementsByTagName("Patrimonio")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Patrimonio")[i].InnerText;
        //        _Veiculo.Marca = _xmlDoc.GetElementsByTagName("Marca")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Marca")[i].InnerText;
        //        _Veiculo.Modelo = _xmlDoc.GetElementsByTagName("Modelo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Modelo")[i].InnerText;
        //        _Veiculo.Tipo = _xmlDoc.GetElementsByTagName("Tipo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Tipo")[i].InnerText;
        //        _Veiculo.Cor = _xmlDoc.GetElementsByTagName("Cor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Cor")[i].InnerText;
        //        _Veiculo.Ano = _xmlDoc.GetElementsByTagName("Ano")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Ano")[i].InnerText;
        //        //_Veiculo.Renavam = _xmlDoc.GetElementsByTagName("Renavam")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Renavam")[i].InnerText;
        //        _Veiculo.EstadoID = _xmlDoc.GetElementsByTagName("EstadoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);
        //        _Veiculo.MunicipioID = _xmlDoc.GetElementsByTagName("MunicipioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);

        //        _Veiculo.Serie_Chassi = _xmlDoc.GetElementsByTagName("Serie_Chassi")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Serie_Chassi")[i].InnerText;
        //        _Veiculo.CombustivelID = _xmlDoc.GetElementsByTagName("CombustivelID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CombustivelID")[i].InnerText);
        //        _Veiculo.Altura = _xmlDoc.GetElementsByTagName("Altura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Altura")[i].InnerText;
        //        _Veiculo.Comprimento = _xmlDoc.GetElementsByTagName("Comprimento")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Comprimento")[i].InnerText;
        //        _Veiculo.Largura = _xmlDoc.GetElementsByTagName("Largura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Largura")[i].InnerText;

        //        _Veiculo.Foto = _xmlDoc.GetElementsByTagName("Foto")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Foto")[i].InnerText;
        //        _Veiculo.Excluida = _xmlDoc.GetElementsByTagName("Excluida")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("Excluida")[i].InnerText);
        //        _Veiculo.StatusID = _xmlDoc.GetElementsByTagName("StatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
        //        _Veiculo.TipoAcessoID = _xmlDoc.GetElementsByTagName("TipoAcessoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);

        //        _Veiculo.NomeArquivoAnexo = _xmlDoc.GetElementsByTagName("NomeArquivoAnexo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivoAnexo")[i].InnerText;
        //        _Veiculo.DescricaoAnexo = _xmlDoc.GetElementsByTagName("DescricaoAnexo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("DescricaoAnexo")[i].InnerText;
        //        _Veiculo.ArquivoAnexo = _veiculoTemp.ArquivoAnexo == null ? "" : _veiculoTemp.ArquivoAnexo;
        //        _Veiculo.TipoEquipamentoVeiculoID = _xmlDoc.GetElementsByTagName("TipoEquipamentoVeiculoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoEquipamentoVeiculoID")[i].InnerText);

        //        bool _controlado1, _controlado2, _controlado3, _controlado4;
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente31")[i].InnerText, out _controlado1);
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente32")[i].InnerText, out _controlado2);
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente33")[i].InnerText, out _controlado3);
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente34")[i].InnerText, out _controlado4);

        //        //_Con.Close();
        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        SqlCommand _sqlCmd;
        //        if (_Veiculo.EquipamentoVeiculoID == 0)
        //        {
        //            _sqlCmd = new SqlCommand("Insert into Veiculos(Descricao,Tipo,Marca,Modelo,Ano,Cor,Placa_Identificador," +
        //                "EstadoID,MunicipioID,Foto,Excluida,StatusID,TipoAcessoID,NomeArquivoAnexo,DescricaoAnexo,ArquivoAnexo," +
        //                "Pendente31,Pendente32,Pendente33,Pendente34,Frota,Patrimonio,Serie_Chassi,CombustivelID,Altura,Comprimento,Largura,TipoEquipamentoVeiculoID) " +
        //                " values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v9,@v10,@v11,@v12,@v13,@v14,@v15" +
        //                 ",@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26,@v27,@v28,@v29);SELECT SCOPE_IDENTITY();", _Con);

        //            _sqlCmd.Parameters.Add("@v1", SqlDbType.VarChar).Value = _Veiculo.Descricao;
        //            _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = _Veiculo.Tipo;
        //            _sqlCmd.Parameters.Add("@v3", SqlDbType.VarChar).Value = _Veiculo.Marca;
        //            _sqlCmd.Parameters.Add("@v4", SqlDbType.VarChar).Value = _Veiculo.Modelo;
        //            _sqlCmd.Parameters.Add("@v5", SqlDbType.VarChar).Value = _Veiculo.Ano;
        //            _sqlCmd.Parameters.Add("@v6", SqlDbType.VarChar).Value = _Veiculo.Cor;
        //            _sqlCmd.Parameters.Add("@v7", SqlDbType.VarChar).Value = _Veiculo.Placa_Identificador;
        //            //_sqlCmd.Parameters.Add("@v8", SqlDbType.VarChar).Value = _Veiculo.Renavam;
        //            _sqlCmd.Parameters.Add("@v9", SqlDbType.Int).Value = _Veiculo.EstadoID;
        //            _sqlCmd.Parameters.Add("@v10", SqlDbType.Int).Value = _Veiculo.MunicipioID;
        //            _sqlCmd.Parameters.Add("@v11", SqlDbType.VarChar).Value = _Veiculo.Foto;
        //            _sqlCmd.Parameters.Add("@v12", SqlDbType.Int).Value = _Veiculo.Excluida;
        //            _sqlCmd.Parameters.Add("@v13", SqlDbType.Int).Value = _Veiculo.StatusID;
        //            _sqlCmd.Parameters.Add("@v14", SqlDbType.Int).Value = _Veiculo.TipoAcessoID;
        //            _sqlCmd.Parameters.Add("@v15", SqlDbType.VarChar).Value = _Veiculo.NomeArquivoAnexo;
        //            _sqlCmd.Parameters.Add("@v16", SqlDbType.VarChar).Value = _Veiculo.DescricaoAnexo;
        //            _sqlCmd.Parameters.Add("@v17", SqlDbType.VarChar).Value = _Veiculo.ArquivoAnexo;
        //            _sqlCmd.Parameters.Add("@v18", SqlDbType.Bit).Value = _controlado1;
        //            _sqlCmd.Parameters.Add("@v19", SqlDbType.Bit).Value = _controlado2;
        //            _sqlCmd.Parameters.Add("@v20", SqlDbType.Bit).Value = _controlado3;
        //            _sqlCmd.Parameters.Add("@v21", SqlDbType.Bit).Value = _controlado4;

        //            _sqlCmd.Parameters.Add("@v22", SqlDbType.VarChar).Value = _Veiculo.Frota;
        //            _sqlCmd.Parameters.Add("@v23", SqlDbType.VarChar).Value = _Veiculo.Patrimonio;
        //            _sqlCmd.Parameters.Add("@v24", SqlDbType.VarChar).Value = _Veiculo.Serie_Chassi;
        //            _sqlCmd.Parameters.Add("@v25", SqlDbType.Int).Value = _Veiculo.CombustivelID;
        //            _sqlCmd.Parameters.Add("@v26", SqlDbType.VarChar).Value = _Veiculo.Altura;
        //            _sqlCmd.Parameters.Add("@v27", SqlDbType.VarChar).Value = _Veiculo.Comprimento;
        //            _sqlCmd.Parameters.Add("@v28", SqlDbType.VarChar).Value = _Veiculo.Largura;
        //            _sqlCmd.Parameters.Add("@v29", SqlDbType.Int).Value = _Veiculo.TipoEquipamentoVeiculoID;

        //            _novID = Convert.ToInt32(_sqlCmd.ExecuteScalar());
        //            _Con.Close();

        //        }

        //        return _novID;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void InsereVeiculosBD ex: " + ex);
        //        return 0;

        //    }
        //}

        //private void AtualizaVeiculosBD(string xmlString)
        //{
        //    try
        //    {

        //        System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

        //        _xmlDoc.LoadXml(xmlString);
        //        // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
        //        ClasseVeiculos.Veiculo _Veiculo = new ClasseVeiculos.Veiculo();
        //        //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
        //        //{
        //        int i = 0;

        //        _Veiculo.EquipamentoVeiculoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EquipamentoVeiculoID")[i].InnerText);
        //        _Veiculo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
        //        _Veiculo.Placa_Identificador = _xmlDoc.GetElementsByTagName("Placa_Identificador")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Placa_Identificador")[i].InnerText;
        //        _Veiculo.Frota = _xmlDoc.GetElementsByTagName("Frota")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Frota")[i].InnerText;
        //        _Veiculo.Patrimonio = _xmlDoc.GetElementsByTagName("Patrimonio")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Patrimonio")[i].InnerText;
        //        _Veiculo.Marca = _xmlDoc.GetElementsByTagName("Marca")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Marca")[i].InnerText;
        //        _Veiculo.Modelo = _xmlDoc.GetElementsByTagName("Modelo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Modelo")[i].InnerText;
        //        _Veiculo.Tipo = _xmlDoc.GetElementsByTagName("Tipo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Tipo")[i].InnerText;
        //        _Veiculo.Cor = _xmlDoc.GetElementsByTagName("Cor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Cor")[i].InnerText;
        //        _Veiculo.Ano = _xmlDoc.GetElementsByTagName("Ano")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Ano")[i].InnerText;
        //        //_Veiculo.Renavam = _xmlDoc.GetElementsByTagName("Renavam")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Renavam")[i].InnerText;
        //        _Veiculo.EstadoID = _xmlDoc.GetElementsByTagName("EstadoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);
        //        _Veiculo.MunicipioID = _xmlDoc.GetElementsByTagName("MunicipioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);

        //        _Veiculo.Serie_Chassi = _xmlDoc.GetElementsByTagName("Serie_Chassi")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Serie_Chassi")[i].InnerText;
        //        _Veiculo.CombustivelID = _xmlDoc.GetElementsByTagName("CombustivelID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CombustivelID")[i].InnerText);
        //        _Veiculo.Altura = _xmlDoc.GetElementsByTagName("Altura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Altura")[i].InnerText;
        //        _Veiculo.Comprimento = _xmlDoc.GetElementsByTagName("Comprimento")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Comprimento")[i].InnerText;
        //        _Veiculo.Largura = _xmlDoc.GetElementsByTagName("Largura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Largura")[i].InnerText;
        //        _Veiculo.TipoEquipamentoVeiculoID = _xmlDoc.GetElementsByTagName("TipoEquipamentoVeiculoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoEquipamentoVeiculoID")[i].InnerText);

        //        _Veiculo.Foto = _xmlDoc.GetElementsByTagName("Foto")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Foto")[i].InnerText;
        //        _Veiculo.Excluida = _xmlDoc.GetElementsByTagName("Excluida")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("Excluida")[i].InnerText);
        //        _Veiculo.StatusID = _xmlDoc.GetElementsByTagName("StatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
        //        _Veiculo.TipoAcessoID = _xmlDoc.GetElementsByTagName("TipoAcessoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);

        //        _Veiculo.NomeArquivoAnexo = _xmlDoc.GetElementsByTagName("NomeArquivoAnexo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivoAnexo")[i].InnerText;
        //        _Veiculo.DescricaoAnexo = _xmlDoc.GetElementsByTagName("DescricaoAnexo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("DescricaoAnexo")[i].InnerText;
        //        _Veiculo.ArquivoAnexo = _veiculoTemp.ArquivoAnexo == null ? "" : _veiculoTemp.ArquivoAnexo;

        //        bool _controlado1, _controlado2, _controlado3, _controlado4;
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente31")[i].InnerText, out _controlado1);
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente32")[i].InnerText, out _controlado2);
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente33")[i].InnerText, out _controlado3);
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente34")[i].InnerText, out _controlado4);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        SqlCommand _sqlCmd;
        //        if (_Veiculo.EquipamentoVeiculoID != 0)
        //        {
        //            _sqlCmd = new SqlCommand("Update Veiculos Set" +
        //                " Descricao= @v1" +
        //                ",Tipo= @v2" +
        //                ",Marca= @v3" +
        //                ",Modelo= @v4" +
        //                ",Ano= @v5" +
        //                ",Cor= @v6" +
        //                ",Placa_Identificador= @v7" +
        //                //",Renavam= @v8" +
        //                ",EstadoID= @v9" +
        //                ",MunicipioID= @v10" +
        //                ",Foto= @v11" +
        //                ",Excluida= @v12" +
        //                ",StatusID= @v13" +
        //                ",TipoAcessoID= @v14" +
        //                ",NomeArquivoAnexo= @v15" +
        //                ",DescricaoAnexo= @v16" +
        //                ",ArquivoAnexo= @v17" +
        //                ",Pendente31= @v18" +
        //                ",Pendente32= @v19" +
        //                ",Pendente33= @v20" +
        //                ",Pendente34= @v21" +
        //                ",Frota= @v22" +
        //                ",Patrimonio= @v23" +
        //                ",Serie_Chassi= @v24" +
        //                ",CombustivelID= @v25" +
        //                ",Altura= @v26" +
        //                ",Comprimento= @v27" +
        //                ",Largura= @v28" +
        //                ",TipoEquipamentoVeiculoID= @v29" +
        //                " Where EquipamentoVeiculoID = @v0", _Con);
        //            //,,,,,,
        //            _sqlCmd.Parameters.Add("@v0", SqlDbType.VarChar).Value = _Veiculo.EquipamentoVeiculoID;
        //            _sqlCmd.Parameters.Add("@v1", SqlDbType.VarChar).Value = _Veiculo.Descricao;
        //            _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = _Veiculo.Tipo;
        //            _sqlCmd.Parameters.Add("@v3", SqlDbType.VarChar).Value = _Veiculo.Marca;
        //            _sqlCmd.Parameters.Add("@v4", SqlDbType.VarChar).Value = _Veiculo.Modelo;
        //            _sqlCmd.Parameters.Add("@v5", SqlDbType.VarChar).Value = _Veiculo.Ano;
        //            _sqlCmd.Parameters.Add("@v6", SqlDbType.VarChar).Value = _Veiculo.Cor;
        //            _sqlCmd.Parameters.Add("@v7", SqlDbType.VarChar).Value = _Veiculo.Placa_Identificador;
        //            //_sqlCmd.Parameters.Add("@v8", SqlDbType.VarChar).Value = _Veiculo.Renavam;
        //            _sqlCmd.Parameters.Add("@v9", SqlDbType.Int).Value = _Veiculo.EstadoID;
        //            _sqlCmd.Parameters.Add("@v10", SqlDbType.Int).Value = _Veiculo.MunicipioID;
        //            _sqlCmd.Parameters.Add("@v11", SqlDbType.VarChar).Value = _Veiculo.Foto;
        //            _sqlCmd.Parameters.Add("@v12", SqlDbType.Int).Value = _Veiculo.Excluida;
        //            _sqlCmd.Parameters.Add("@v13", SqlDbType.Int).Value = _Veiculo.StatusID;
        //            _sqlCmd.Parameters.Add("@v14", SqlDbType.Int).Value = _Veiculo.TipoAcessoID;
        //            _sqlCmd.Parameters.Add("@v15", SqlDbType.VarChar).Value = _Veiculo.NomeArquivoAnexo;
        //            _sqlCmd.Parameters.Add("@v16", SqlDbType.VarChar).Value = _Veiculo.DescricaoAnexo;
        //            _sqlCmd.Parameters.Add("@v17", SqlDbType.VarChar).Value = _Veiculo.ArquivoAnexo;
        //            _sqlCmd.Parameters.Add("@v18", SqlDbType.Bit).Value = _controlado1;
        //            _sqlCmd.Parameters.Add("@v19", SqlDbType.Bit).Value = _controlado2;
        //            _sqlCmd.Parameters.Add("@v20", SqlDbType.Bit).Value = _controlado3;
        //            _sqlCmd.Parameters.Add("@v21", SqlDbType.Bit).Value = _controlado4;

        //            _sqlCmd.Parameters.Add("@v22", SqlDbType.VarChar).Value = _Veiculo.Frota;
        //            _sqlCmd.Parameters.Add("@v23", SqlDbType.VarChar).Value = _Veiculo.Patrimonio;
        //            _sqlCmd.Parameters.Add("@v24", SqlDbType.VarChar).Value = _Veiculo.Serie_Chassi;
        //            _sqlCmd.Parameters.Add("@v25", SqlDbType.Int).Value = _Veiculo.CombustivelID;
        //            _sqlCmd.Parameters.Add("@v26", SqlDbType.VarChar).Value = _Veiculo.Altura;
        //            _sqlCmd.Parameters.Add("@v27", SqlDbType.VarChar).Value = _Veiculo.Comprimento;
        //            _sqlCmd.Parameters.Add("@v28", SqlDbType.VarChar).Value = _Veiculo.Largura;
        //            _sqlCmd.Parameters.Add("@v29", SqlDbType.Int).Value = _Veiculo.TipoEquipamentoVeiculoID;

        //            _sqlCmd.ExecuteNonQuery();
        //            _Con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void AtualizaVeiculoBD ex: " + ex);

        //    }
        //}

        //private void ExcluiVeiculoBD(int _VeiculoID) // alterar para xml
        //{
        //    try
        //    {

        //        //_Con.Close();
        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        SqlCommand _sqlCmd;
        //        _sqlCmd = new SqlCommand("Delete from Veiculos where VeiculoID=" + _VeiculoID, _Con);
        //        _sqlCmd.ExecuteNonQuery();

        //        _Con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void ExcluiSeguroBD ex: " + ex);

        //    }
        //}

        //private string BuscaFoto(int veiculoID)
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseArquivosImagens = _xmlDocument.CreateElement("ClasseArquivosImagens");
        //        _xmlDocument.AppendChild(_ClasseArquivosImagens);

        //        XmlNode _ArquivosImagens = _xmlDocument.CreateElement("ArquivosImagens");
        //        _ClasseArquivosImagens.AppendChild(_ArquivosImagens);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        SqlCommand SQCMDXML = new SqlCommand("Select * From Veiculos Where VeiculoID = " + veiculoID, _Con);
        //        SqlDataReader SQDR_XML;
        //        SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
        //        while (SQDR_XML.Read())
        //        {
        //            XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
        //            _ArquivosImagens.AppendChild(_ArquivoImagem);

        //            XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
        //            _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Foto"].ToString())));
        //            _ArquivoImagem.AppendChild(_Arquivo);

        //        }
        //        SQDR_XML.Close();

        //        _Con.Close();
        //        return _xmlDocument.InnerXml;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void CriaXmlImagem ex: " + ex);
        //        return null;
        //    }
        //}

        //private void AtualizaPendencias(int _VeiculoID)
        //{
        //    try
        //    {

        //        if (_VeiculoID == 0)
        //        {
        //            return;
        //        }

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
        //        SqlCommand _sqlCmd;
        //        for (int i = 31; i < 35; i++)
        //        {
        //            _sqlCmd = new SqlCommand("Insert into Pendencias (TipoPendenciaID,Descricao,DataLimite ,Impeditivo,VeiculoID) values (" +
        //                                              "@v1,@v2, @v3,@v4,@v5)", _Con);

        //            _sqlCmd.Parameters.Add("@v1", SqlDbType.Int).Value = i;
        //            _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = "Cadastro novo!";
        //            _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = DateTime.Now;
        //            _sqlCmd.Parameters.Add("@v4", SqlDbType.Bit).Value = 1;
        //            _sqlCmd.Parameters.Add("@v5", SqlDbType.Int).Value = _VeiculoID;
        //            _sqlCmd.ExecuteNonQuery();
        //            //Thread.Sleep(200);
        //        }

        //        _Con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void AtualizaPendencias ex: " + ex);

        //    }

        //}

        #endregion

        //        {
        //            _PopupSalvando.ShowDialog();
        //        }
        //    });
        //}

        //private void OnVeiculoEquipamentoSelecionado()
        //{
        //    try
        //    {
        //        CarregaColecaoVeiculoEquipTipoServicos(VeiculoSelecionado.EquipamentoVeiculoID);

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }

        //}
    }
}