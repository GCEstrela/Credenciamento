// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels.Commands;
using iModSCCredenciamento.ViewModels.Comportamento;
using iModSCCredenciamento.Views.Model;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
//using IMOD.Domain.EntitiesCustom;
//using ColaboradoresCredenciaisView = IMOD.Domain.EntitiesCustom.ColaboradoresCredenciaisView;
//using EmpresaView = iModSCCredenciamento.Views.Model.EmpresaView;
using ColaboradorEmpresaView = iModSCCredenciamento.Views.Model.ColaboradorEmpresaView;
using EmpresaLayoutCrachaView = iModSCCredenciamento.Views.Model.EmpresaLayoutCrachaView;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradoresCredenciaisViewModel : ViewModelBase, IComportamento
    {
        //private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        //private readonly IColaboradorService _service = new ColaboradorService();
        private readonly IColaboradorCredencialService _service = new ColaboradorCredencialService();

        private readonly IColaboradorEmpresaService _ColaboradorEmpresaService = new ColaboradorEmpresaService();
        private readonly IEmpresaLayoutCrachaService _EmpresaLayoutCrachaService = new EmpresaLayoutCrachaService();

        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaLayoutCrachaService _empresaLayoutCracha = new EmpresaLayoutCrachaService();


        private ColaboradorView _colaboradorView;
        private ColaboradorEmpresaView _colaboradorEmpresaView;
        private ColaboradorCredencial _colaboradorCredencial;

        #region  Propriedades

        public List<CredencialStatus> CredencialStatus { get; set; }
        public List<CredencialMotivo> CredencialMotivo { get; set; }
        public List<FormatoCredencial> FormatoCredencial { get; set; }
        public List<TipoCredencial> TipoCredencial { get; set; }
        public List<EmpresaLayoutCracha> EmpresaLayoutCracha { get; set; }
        public List<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public List<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; }
        public List<AreaAcesso> ColaboradorPrivilegio { get; set; }

        //public EmpresaLayoutCrachaView EntityEmpresasLayoutsCrachas { get; set; }
        //public LayoutCrachaView EntityLayoutCrachaView { get; set; }
        //public FormatoCredencialView EntityFormatoCredencialView { get; set; }
        //public EmpresaContratoView EntityEmpresaContratoView { get; set; }

        public ColaboradoresCredenciaisView Entity { get; set; }
        public ObservableCollection<ColaboradoresCredenciaisView> EntityObserver { get; set; }

        //TODO: EntityCustom ColaboradoresCredenciais
        //public ColaboradoresCredenciaisView EntityCustom { get; set; }
        //public ObservableCollection<ColaboradoresCredenciaisView> EntityCustomObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; set; } = true;

        #endregion 
        #region Inicializacao

        public ColaboradoresCredenciaisViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();

            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        //TODO:Listar ColaboradoresCredenciais
        public void AtualizarDados(ColaboradorView entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _colaboradorView = entity;
            ////Obter dados
            var list1 = _service.ListarView(null, null, null, null, entity.ColaboradorId).ToList();
            var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>>(list1);
            EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
            list2.ForEach(n =>
            {
                EntityObserver.Add(n);
            });
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
                var n1 = Mapper.Map<ColaboradorCredencial>(Entity);
                
                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;

                _service.Criar(n1);
                //////Adicionar no inicio da lista um item a coleção
                //var n2 = Mapper.Map<ColaboradorCredencialView>(n1);
                //EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
                //Entity.ColaboradorCredencialId = n1.ColaboradorCredencialId;
                
                var list1 = _service.ListarView(null, null, null, null, _colaboradorView.ColaboradorId).ToList();
                var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>>(list1);
                EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
                list2.ForEach(n =>
                {
                    EntityObserver.Add(n);
                });


                //EntityObserver.Insert(0, Entity);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            //Entity = new ColaboradorCredencialView();
            Entity = new ColaboradoresCredenciaisView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
        }

        /// <summary>
        ///     Editar dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (Entity == null) return;
            //    var n1 = Mapper.Map<ColaboradorCredencial>(Entity);
            //    _service.Alterar(n1);
            //    IsEnableLstView = true;
            //}
            //catch (Exception ex)
            //{
            //    Utils.TraceException(ex);
            //    WpfHelp.PopupBox(ex);
            //}
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
                IsEnableLstView = true;
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
            //try
            //{
            //    if (Entity == null) return;
            //    var result = WpfHelp.MboxDialogRemove();
            //    if (result != DialogResult.Yes) return;

            //    var n1 = Mapper.Map<ColaboradorCredencial>(Entity);
            //    _service.Remover(n1);
            //    //Retirar empresa da coleção
            //    EntityObserver.Remove(Entity);
            //}
            //catch (Exception ex)
            //{
            //    Utils.TraceException(ex);
            //    WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            //}
        }

        /// <summary>
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
            //try
            //{
            //    if (_colaboradorView == null) return;

            //    var pesquisa = NomePesquisa;

            //    var num = PesquisarPor;

            //    //Por nome
            //    if (num.Key == 1)
            //    {
            //        var l1 = _service.Listar(_colaboradorView.EmpresaId, $"%{pesquisa}%");
            //        PopularObserver(l1);
            //    }
            //    //Por CPF
            //    if (num.Key == 2)
            //    {
            //        var l1 = _service.Listar(_colaboradorView.EmpresaId, null, $"%{pesquisa}%", null, null, null);
            //        PopularObserver(l1);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utils.TraceException(ex);
            //}
        }

        private void PopularObserver(ICollection<ColaboradorCredencial> list)
        {
            try
            {
                var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>>(list.OrderBy(n => n.ColaboradorCredencialId));
                EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
                //Empresas = observer;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public void Validar()
        {
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
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase(Pesquisar, true);

        #endregion

        private void CarregaUI()
        {
            //CarregaColecaoEmpresas();
            //CarregaColecaoAreasAcessos();
            //CarregaColecaoCredenciaisStatus();
            //CarregaColecaoTiposCredenciais();
            //CarregaColecaoTecnologiasCredenciais();
            //CarregaColeçãoFormatosCredenciais();
            //CarregaColecaoCredenciaisMotivos();

            //CarregaColecaoColaboradoresPrivilegios();
        }

        #endregion

        #region Variaveis Privadas

        public List<ColaboradorEmpresaView> ColaboradorEmpresa { get; private set; }



        #endregion

        #region  Metodos
        private void ListarDadosAuxiliares()
        {
            var lst0 = _auxiliaresService.CredencialStatusService.Listar();
            CredencialStatus = new List<CredencialStatus>();
            CredencialStatus.AddRange(lst0);

            var lst1 = _auxiliaresService.CredencialMotivoService.Listar();
            CredencialMotivo = new List<CredencialMotivo>();
            CredencialMotivo.AddRange(lst1);

            var lst2 = _auxiliaresService.FormatoCredencialService.Listar();
            FormatoCredencial = new List<FormatoCredencial>();
            FormatoCredencial.AddRange(lst2);

            var lst3 = _auxiliaresService.TipoCredencialService.Listar();
            TipoCredencial = new List<TipoCredencial>();
            TipoCredencial.AddRange(lst3);

            var lst4 = _empresaLayoutCracha.Listar();
            EmpresaLayoutCracha = new List<EmpresaLayoutCracha>();
            EmpresaLayoutCracha.AddRange(lst4);

            var lst5 = _auxiliaresService.TecnologiaCredencialService.Listar();
            TecnologiasCredenciais = new List<TecnologiaCredencial>();
            TecnologiasCredenciais.AddRange(lst5);

            var lst6 = _ColaboradorEmpresaService.Listar();
            ColaboradoresEmpresas = new List<ColaboradorEmpresa>();
            ColaboradoresEmpresas.AddRange(lst6);

            var lst7 = _auxiliaresService.AreaAcessoService.Listar();
            ColaboradorPrivilegio = new List<AreaAcesso>();
            ColaboradorPrivilegio.AddRange(lst7);

        }

        #endregion

        //#region Contrutores
        //public ObservableCollection<ClasseAreasAcessos.AreaAcesso> AreasAcessos
        //{
        //    get
        //    {
        //        return _AreasAcessos;
        //    }

        //    set
        //    {
        //        if (_AreasAcessos != value)
        //        {
        //            _AreasAcessos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ColaboradorEmpresaView> ColaboradoresEmpresas
        //{
        //    get
        //    {
        //        return _ColaboradoresEmpresas;
        //    }

        //    set
        //    {
        //        if (_ColaboradoresEmpresas != value)
        //        {
        //            _ColaboradoresEmpresas = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseClaboradoresPrivilegios.ColaboradorPrivilegio> ColaboradoresPrivilegios
        //{
        //    get
        //    {
        //        return _ClaboradoresPrivilegios;
        //    }

        //    set
        //    {
        //        if (_ClaboradoresPrivilegios != value)
        //        {
        //            _ClaboradoresPrivilegios = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseTiposCredenciais.TipoCredencial> TiposCredenciais
        //{
        //    get
        //    {
        //        return _TiposCredenciais;
        //    }

        //    set
        //    {
        //        if (_TiposCredenciais != value)
        //        {
        //            _TiposCredenciais = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseCredenciaisStatus.CredencialStatus> CredenciaisStatus
        //{
        //    get
        //    {
        //        return _CredenciaisStatus;
        //    }

        //    set
        //    {
        //        if (_CredenciaisStatus != value)
        //        {
        //            _CredenciaisStatus = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial> TecnologiasCredenciais
        //{
        //    get
        //    {
        //        return _TecnologiasCredenciais;
        //    }

        //    set
        //    {
        //        if (_TecnologiasCredenciais != value)
        //        {
        //            _TecnologiasCredenciais = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial> ColaboradoresCredenciais
        //{
        //    get
        //    {
        //        return _ColaboradoresCredenciais;
        //    }

        //    set
        //    {
        //        if (_ColaboradoresCredenciais != value)
        //        {
        //            _ColaboradoresCredenciais = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseVinculos.Vinculo> Vinculos
        //{
        //    get
        //    {
        //        return _Vinculos;
        //    }

        //    set
        //    {
        //        if (_Vinculos != value)
        //        {
        //            _Vinculos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<EmpresaLayoutCrachaView> EmpresasLayoutsCrachas
        //{
        //    get
        //    {
        //        return _EmpresasLayoutsCrachas;
        //    }

        //    set
        //    {
        //        if (_EmpresasLayoutsCrachas != value)
        //        {
        //            _EmpresasLayoutsCrachas = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ClasseColaboradoresCredenciais.ColaboradorCredencial ColaboradorCredencialSelecionado
        //{
        //    get
        //    {
        //        return _ColaboradorCredencialSelecionado;
        //    }
        //    set
        //    {
        //        _ColaboradorCredencialSelecionado = value;
        //        //base.OnPropertyChanged("SelectedItem");
        //        base.OnPropertyChanged();
        //        if (ColaboradorCredencialSelecionado != null)
        //        {
        //            //CarregaColecaoColaboradoresEmpresas(ColaboradorCredencialSelecionado.ColaboradorID);
        //            //CarregaColeçãoEmpresasLayoutsCrachas(ColaboradorCredencialSelecionado.EmpresaID);
        //        }

        //    }
        //}

        //public ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo> CredenciaisMotivos
        //{
        //    get
        //    {
        //        return _CredenciaisMotivos;
        //    }
        //    set
        //    {
        //        if (_CredenciaisMotivos != value)
        //        {
        //            _CredenciaisMotivos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<EmpresaView> Empresas

        //{
        //    get
        //    {
        //        return _Empresas;
        //    }

        //    set
        //    {
        //        if (_Empresas != value)
        //        {
        //            _Empresas = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> FormatosCredenciais

        //{
        //    get
        //    {
        //        return _FormatosCredenciais;
        //    }

        //    set
        //    {
        //        if (_FormatosCredenciais != value)
        //        {
        //            _FormatosCredenciais = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public ObservableCollection<EmpresaContratoView> Contratos
        //{
        //    get
        //    {
        //        return _Contratos;
        //    }

        //    set
        //    {
        //        if (_Contratos != value)
        //        {
        //            _Contratos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        //public int ColaboradorSelecionadaID
        //{
        //    get
        //    {
        //        return _ColaboradorSelecionadaID;

        //    }
        //    set
        //    {
        //        _ColaboradorSelecionadaID = value;
        //        base.OnPropertyChanged();
        //        if (ColaboradorSelecionadaID != null)
        //        {
        //            //OnEmpresaSelecionada();
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
        //public string Validade
        //{
        //    get
        //    {
        //        return _Validade;
        //    }
        //    set
        //    {
        //        _Validade = value;
        //        base.OnPropertyChanged();
        //    }
        //}
        ////public string ComboEmpresaSelecionado
        ////{
        ////    get
        ////    {
        ////        return this._ComboEmpresaSelecionado;
        ////    }
        ////    set
        ////    {
        ////        this._ComboEmpresaSelecionado = value;
        ////        base.OnPropertyChanged();
        ////    }
        ////}
        //#endregion

        //#region Comandos dos Botoes

        //public void OnAtualizaCommand(object _ColaboradorID)
        //{
        //    try
        //    {
        //        ColaboradorSelecionadaID = Convert.ToInt32(_ColaboradorID);
        //        Thread CarregaColecaoColaboradoresCredenciais_thr = new Thread(() =>
        //        {
        //            CarregaColecaoColaboradoresEmpresas(ColaboradorSelecionadaID);
        //            // CarregaColeçãoEmpresasLayoutsCrachas(ColaboradorCredencialSelecionado.EmpresaID);
        //            CarregaColecaoColaboradoresCredenciais(Convert.ToInt32(_ColaboradorID));
        //        });
        //        CarregaColecaoColaboradoresCredenciais_thr.Start();
        //        //CarregaColecaoColaboradoresCredenciais(Convert.ToInt32(_ColaboradorID));

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        //public void OnBuscarArquivoCommand()
        //{
        //    try
        //    {
        //        //System.Windows.Forms.OpenFileDialog _arquivoPDF = new System.Windows.Forms.OpenFileDialog();
        //        //string _sql;
        //        //string _nomecompletodoarquivo;
        //        //string _arquivoSTR;
        //        //_arquivoPDF.InitialDirectory = "c:\\\\";
        //        //_arquivoPDF.Filter = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
        //        //_arquivoPDF.RestoreDirectory = true;
        //        //_arquivoPDF.ShowDialog();
        //        ////if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
        //        ////{
        //        //_nomecompletodoarquivo = _arquivoPDF.SafeFileName;
        //        //_arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
        //        //_ColaboradorCredencialTemp.Cargo = _nomecompletodoarquivo;
        //        //_ColaboradorCredencialTemp.Arquivo = _arquivoSTR;
        //        ////InsereArquivoBD(Convert.ToInt32(empresaID), _nomecompletodoarquivo, _arquivoSTR);

        //        ////AtualizaListaAnexos(_resp);

        //        ////}
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public void OnEditarCommand()
        //{
        //    try
        //    {
        //        _ColaboradoresEmpresasTemp.Clear();
        //        foreach (var y in ColaboradoresEmpresas)
        //        {
        //            _ColaboradoresEmpresasTemp.Add(y);
        //        }
        //        //if (ColaboradorCredencialSelecionado.Ativa)
        //        //{
        //        //    List<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _Temp = ColaboradoresEmpresas.Where(x => x.Ativo == true).ToList();
        //        //    //foreach (var _member in toRemove)
        //        //    //{
        //        //    //    ColaboradoresEmpresas.Remove(_member);
        //        //    //}

        //        //    ColaboradoresEmpresas.Clear();

        //        //    ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>(_Temp);
        //        //}

        //        _ColaboradorCredencialTemp = ColaboradorCredencialSelecionado.CriaCopia(ColaboradorCredencialSelecionado);
        //        _selectedIndexTemp = SelectedIndex;

        //        //OnAtualizaCommand(ColaboradorSelecionadaID);
        //        //CarregaColecaoContratos(_ColaboradorCredencialTemp.EmpresaID);

        //        HabilitaEdicao = true;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //public void OnCancelarEdicaoCommand()
        //{
        //    try
        //    {

        //        ColaboradoresCredenciais[_selectedIndexTemp] = _ColaboradorCredencialTemp;
        //        SelectedIndex = _selectedIndexTemp;
        //        HabilitaEdicao = false;
        //        ColaboradoresEmpresas = new ObservableCollection<ColaboradorEmpresaView>(_ColaboradoresEmpresasTemp);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public void OnAdicionarCommand()
        //{
        //    try
        //    {
        //        if (ColaboradoresCredenciais != null)
        //        {
        //            foreach (var x in ColaboradoresCredenciais)
        //            {
        //                _ColaboradoresCredenciaisTemp.Add(x);
        //            }

        //            _selectedIndexTemp = SelectedIndex;
        //            ColaboradoresCredenciais.Clear();
        //        }

        //        _ColaboradoresEmpresasTemp.Clear();
        //        foreach (var y in ColaboradoresEmpresas)
        //        {
        //            _ColaboradoresEmpresasTemp.Add(y);
        //        }

        //        List<ColaboradorEmpresaView> _Temp = ColaboradoresEmpresas.Where(x => x.Ativo).ToList();
        //        //foreach (var _member in toRemove)
        //        //{
        //        //    ColaboradoresEmpresas.Remove(_member);
        //        //}

        //        ColaboradoresEmpresas.Clear();

        //        ColaboradoresEmpresas = new ObservableCollection<ColaboradorEmpresaView>(_Temp);

        //        _ColaboradorCredencialTemp = new ClasseColaboradoresCredenciais.ColaboradorCredencial();
        //        _ColaboradorCredencialTemp.ColaboradorID = ColaboradorSelecionadaID;
        //        _ColaboradorCredencialTemp.CredencialStatusID = 1;
        //        ColaboradoresCredenciais.Add(_ColaboradorCredencialTemp);

        //        //CarregaColecaoColaboradoresEmpresas(ColaboradorSelecionadaID);
        //        SelectedIndex = 0;
        //        ColaboradorCredencialSelecionado.Emissao = DateTime.Now;
        //        HabilitaEdicao = true;
        //        // SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        ////public void OnSalvarEdicaoCommand()
        ////{
        ////    try
        ////    {

        ////        if (ColaboradorCredencialSelecionado.CredencialStatusID == 1)
        ////        {

        ////            ColaboradorCredencialSelecionado.Validade = (DateTime?)VerificarMenorData(ColaboradorCredencialSelecionado.ColaboradorID);
        ////            bool _resposta = SCManager.Vincular(ColaboradorCredencialSelecionado);

        ////        }

        ////        HabilitaEdicao = false;
        ////        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresCredenciais));

        ////        ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial> _ColaboradorCredencialTemp = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>();
        ////        ClasseColaboradoresCredenciais _ClasseColaboradorerEmpresasTemp = new ClasseColaboradoresCredenciais();
        ////        _ColaboradorCredencialTemp.Add(ColaboradorCredencialSelecionado);
        ////        _ClasseColaboradorerEmpresasTemp.ColaboradoresCredenciais = _ColaboradorCredencialTemp;

        ////        string xmlString;

        ////        using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
        ////        {

        ////            using (XmlTextWriter xw = new XmlTextWriter(sw))
        ////            {
        ////                xw.Formatting = Formatting.Indented;
        ////                serializer.Serialize(xw, _ClasseColaboradorerEmpresasTemp);
        ////                xmlString = sw.ToString();
        ////            }

        ////        }

        ////        InsereColaboradorCredencialBD(xmlString);

        ////        Thread CarregaColecaoColaboradoresCredenciais_thr = new Thread(() =>
        ////        {
        ////            CarregaColecaoColaboradoresCredenciais(ColaboradorSelecionadaID);
        ////        });
        ////        CarregaColecaoColaboradoresCredenciais_thr.Start();

        ////        //_ClasseEmpresasSegurosTemp = null;

        ////        //_SegurosTemp.Clear();
        ////        //_seguroTemp = null;

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////}

        //public void OnSalvarAdicaoCommand()
        //{
        //    try
        //    {

        //        //string _xml = RequisitaColaboradoresCredenciaisNovos(ColaboradorCredencialSelecionado.ColaboradorEmpresaID);

        //        //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradoresCredenciais));

        //        //XmlDocument xmldocument = new XmlDocument();
        //        //xmldocument.LoadXml(_xml);

        //        //TextReader reader = new StringReader(_xml);
        //        //ClasseColaboradoresCredenciais classeColaboradoresCredenciais = new ClasseColaboradoresCredenciais();
        //        //classeColaboradoresCredenciais = (ClasseColaboradoresCredenciais)deserializer.Deserialize(reader);
        //        //ColaboradorCredencialSelecionado.Cargo = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].Cargo;
        //        //ColaboradorCredencialSelecionado.CNPJ = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].CNPJ;
        //        //ColaboradorCredencialSelecionado.ColaboradorApelido = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorApelido;
        //        //ColaboradorCredencialSelecionado.ColaboradorEmpresaID = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorEmpresaID;
        //        //ColaboradorCredencialSelecionado.ColaboradorFoto = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorFoto;
        //        //ColaboradorCredencialSelecionado.ColaboradorID = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorID;
        //        //ColaboradorCredencialSelecionado.ColaboradorNome = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorNome;
        //        //ColaboradorCredencialSelecionado.ContratoDescricao = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ContratoDescricao;
        //        //ColaboradorCredencialSelecionado.CPF = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].CPF;
        //        //ColaboradorCredencialSelecionado.EmpresaApelido = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].EmpresaApelido;
        //        //ColaboradorCredencialSelecionado.EmpresaID = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].EmpresaID;
        //        //ColaboradorCredencialSelecionado.EmpresaLogo = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].EmpresaLogo;
        //        //ColaboradorCredencialSelecionado.EmpresaNome = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].EmpresaNome;
        //        //ColaboradorCredencialSelecionado.Motorista = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].Motorista;

        //        //ColaboradorCredencialSelecionado.FormatoCredencialDescricao = FormatosCredenciais.First(i => i.FormatoCredencialID == ColaboradorCredencialSelecionado.FormatoCredencialID).Descricao;
        //        //ColaboradorCredencialSelecionado.FormatIDGUID = FormatosCredenciais.First(i => i.FormatoCredencialID == ColaboradorCredencialSelecionado.FormatoCredencialID).FormatIDGUID.ToString();

        //        ////ColaboradorCredencialSelecionado.LayoutCrachaGUID = EmpresasLayoutsCrachas.First(i => i.LayoutCrachaID == ColaboradorCredencialSelecionado.LayoutCrachaID).LayoutCrachaGUID;

        //        //ColaboradorCredencialSelecionado.PrivilegioDescricao1 = ColaboradoresPrivilegios.First(i => i.ColaboradorPrivilegioID == ColaboradorCredencialSelecionado.ColaboradorPrivilegio1ID).Descricao;
        //        //ColaboradorCredencialSelecionado.PrivilegioDescricao2 = ColaboradoresPrivilegios.First(i => i.ColaboradorPrivilegioID == ColaboradorCredencialSelecionado.ColaboradorPrivilegio2ID).Descricao;

        //        //ColaboradorCredencialSelecionado.Validade = (DateTime?)VerificarMenorData(ColaboradorCredencialSelecionado.ColaboradorID);

        //        //var _index = SelectedIndex;

        //        //bool _resposta = SCManager.Vincular(ColaboradorCredencialSelecionado);

        //        //if (!ColaboradorCredencialSelecionado.Ativa)
        //        //{
        //        //    ColaboradorCredencialSelecionado.Baixa = DateTime.Now;
        //        //}
        //        //else
        //        //{
        //        //    ColaboradorCredencialSelecionado.Baixa = null;
        //        //}

        //        //HabilitaEdicao = false;
        //        //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresCredenciais));

        //        //ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial> _ColaboradorCredencialPro = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>();
        //        //ClasseColaboradoresCredenciais _ClasseColaboradorerEmpresasPro = new ClasseColaboradoresCredenciais();
        //        //_ColaboradorCredencialPro.Add(ColaboradorCredencialSelecionado);
        //        //_ClasseColaboradorerEmpresasPro.ColaboradoresCredenciais = _ColaboradorCredencialPro;

        //        //IMOD.Domain.Entities.ColaboradorCredencial ColaboradorEntity = new IMOD.Domain.Entities.ColaboradorCredencial();
        //        //g.TranportarDados(ColaboradorCredencialSelecionado, 1, ColaboradorEntity);

        //        var entity = Mapper.Map<ColaboradorCredencial>(ColaboradorCredencialSelecionado);
        //        var repositorio = new ColaboradorCredencialService();
        //        if (ColaboradorCredencialSelecionado.ColaboradorCredencialID == 0)
        //        {
        //            _repositorio.Credencial.Criar(entity);
        //        }
        //        else
        //        {
        //            _repositorio.Credencial.Alterar(entity);
        //        }

        //        var id = ColaboradorCredencialSelecionado.ColaboradorID;
        //        //string xmlString;

        //        //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
        //        //{

        //        //    using (XmlTextWriter xw = new XmlTextWriter(sw))
        //        //    {
        //        //        xw.Formatting = Formatting.Indented;
        //        //        serializer.Serialize(xw, _ClasseColaboradorerEmpresasPro);
        //        //        xmlString = sw.ToString();
        //        //    }

        //        //}
        //        ////int _colaboradorCredencialID = InsereColaboradorCredencialBD(xmlString);

        //        //int _colaboradorCredencialID = InsereColaboradorCredencialBD(xmlString);

        //        CarregaColecaoColaboradoresCredenciais(id);

        //        //SelectedIndex = _index;

        //        ColaboradoresEmpresas = new ObservableCollection<ColaboradorEmpresaView>(_ColaboradoresEmpresasTemp);

        //        //if (ColaboradorCredencialSelecionado.CredencialStatusID == 1)
        //        //{

        //        //    ColaboradorCredencialSelecionado.Validade = (DateTime?)VerificarMenorData(ColaboradorCredencialSelecionado.ColaboradorID);
        //        //    bool _resposta = SCManager.Vincular(ColaboradorCredencialSelecionado);

        //        //}

        //        //Thread CarregaColecaoColaboradoresCredenciais_thr = new Thread(() =>
        //        //{
        //        //    //CarregaColecaoColaboradoresCredenciais(ColaboradorSelecionadaID);

        //        //    //System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => { SelectedIndex = 0; }));

        //        //    if (ColaboradorCredencialSelecionado.CredencialStatusID == 1)
        //        //    {

        //        //        bool _resposta = SCManager.Vincular(ColaboradorCredencialSelecionado);

        //        //    }

        //        //});
        //        //CarregaColecaoColaboradoresCredenciais_thr.Start();

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public void OnCancelarAdicaoCommand()
        //{
        //    try
        //    {
        //        ColaboradoresCredenciais = null;
        //        ColaboradoresCredenciais = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>(_ColaboradoresCredenciaisTemp);
        //        SelectedIndex = _selectedIndexTemp;
        //        _ColaboradoresCredenciaisTemp.Clear();
        //        HabilitaEdicao = false;
        //        ColaboradoresEmpresas = new ObservableCollection<ColaboradorEmpresaView>(_ColaboradoresEmpresasTemp);

        //    }
        //    catch (Exception ex)
        //    {
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
        //                //if (SCManager.ExcluirCredencial(ColaboradorCredencialSelecionado.CredencialGuid))
        //                //{

        //                var entity = Mapper.Map<ColaboradorCredencial>(ColaboradorCredencialSelecionado);
        //                var repositorio = new ColaboradorCredencialService();
        //                repositorio.Remover(entity);

        //                ColaboradoresCredenciais.Remove(ColaboradorCredencialSelecionado);
        //                //}
        //                //else
        //                //{
        //                //    Global.PopupBox("Não foi possível excluir esta credencial. Verifique no Gerenciador de Credenciais do Controle de Acesso.",4);
        //                //}

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        //public void OnPesquisarCommand()
        //{
        //    try
        //    {
        //        popupPesquisaColaboradoresCredenciais = new PopupPesquisaColaboradoresCredenciais();
        //        popupPesquisaColaboradoresCredenciais.EfetuarProcura += On_EfetuarProcura;
        //        popupPesquisaColaboradoresCredenciais.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public void On_EfetuarProcura(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        object vetor = popupPesquisaColaboradoresCredenciais.Criterio.Split((char)(20));
        //        int _colaboradorID = ColaboradorSelecionadaID;
        //        string _empresaNome = ((string[])vetor)[0];
        //        int _tipoCredencialID = Convert.ToInt32(((string[])vetor)[1]);
        //        int _credencialStatusID = Convert.ToInt32(((string[])vetor)[2]);

        //        CarregaColecaoColaboradoresCredenciais(_colaboradorID, _empresaNome, _tipoCredencialID, _credencialStatusID);
        //        SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        //public void OnBuscarDataCommand()
        //{
        //    try
        //    {
        //        DateTime _datavalidadeCredencial = validadeCursoContrato(ColaboradorCredencialSelecionado.ColaboradorID);
        //        ColaboradoresCredenciais[SelectedIndex].Validade = _datavalidadeCredencial;
        //        Validade = String.Format("{0:dd/MM/yyyy}", _datavalidadeCredencial);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //#endregion

        #region Carregamento das Colecoes

        //private void CarregaColecaoColaboradoresCredenciais(int colaboradorID, string _empresaNome = "", int _tipoCredencialID = 0, int _credencialStatusID = 0)
        //{
        //    try
        //    {

        //        var list = _repositorio.ListarColaboradores(0, "", 0, 0, colaboradorID).ToList();
        //        var list2 = Mapper.Map<List<ClasseColaboradoresCredenciais.ColaboradorCredencial>>(list.OrderBy(n => n.ColaboradorId));
        //        var observer = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>();

        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        ColaboradoresCredenciais = observer;

        //        //Hotfix auto-selecionar registro no topo da ListView
        //        var topList = observer.FirstOrDefault();
        //        ColaboradorCredencialSelecionado = topList;

        //        SelectedIndex = 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //private void CarregaColecaoColaboradoresPrivilegios()
        //{
        //    try
        //    {
        //        ColaboradoresPrivilegios = new ObservableCollection<ClasseClaboradoresPrivilegios.ColaboradorPrivilegio>();
        //        foreach (ClasseAreasAcessos.AreaAcesso _areaaAcesso in AreasAcessos)
        //        {
        //            ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio { ColaboradorPrivilegioID = _areaaAcesso.AreaAcessoID, Descricao = _areaaAcesso.Identificacao });

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
        //private void CarregaColecaoAreasAcessos()
        //{
        //    try
        //    {

        //        var list1 = _auxiliaresService.AreaAcessoService.Listar();
        //        var list2 = Mapper.Map<List<ClasseAreasAcessos.AreaAcesso>>(list1);

        //        var observer = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        AreasAcessos = observer;
        //        SelectedIndex = 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //private void CarregaColecaoColaboradoresEmpresas(int _colaboradorID, int _ativo = 2)
        //{
        //    try
        //    {
        //        var service = new ColaboradorEmpresaService();
        //        var list1 = service.Listar(_colaboradorID, _ativo);

        //        var list2 = Mapper.Map<List<ColaboradorEmpresaView>>(list1);

        //        var observer = new ObservableCollection<ColaboradorEmpresaView>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        this.ColaboradoresEmpresas = observer;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void CarregaColeçãoEmpresasLayoutsCrachas(int _colaboradorEmpresaID = 0)
        //{
        //    try
        //    {
        //        var list1 = _auxiliaresService.LayoutCrachaService.Listar();
        //        var list2 = Mapper.Map<List<EmpresaLayoutCrachaView>>(list1);

        //        var observer = new ObservableCollection<EmpresaLayoutCrachaView>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        EmpresasLayoutsCrachas = observer;
        //        SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void CarregaColeçãoFormatosCredenciais()
        //{
        //    try
        //    {
        //        var list1 = _auxiliaresService.FormatoCredencialService.Listar();
        //        var list2 = Mapper.Map<List<ClasseFormatosCredenciais.FormatoCredencial>>(list1);

        //        var observer = new ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        FormatosCredenciais = observer;
        //        SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //private void CarregaColecaoTiposCredenciais()
        //{
        //    try
        //    {
        //        var list1 = _auxiliaresService.TipoCredencialService.Listar();
        //        var list2 = Mapper.Map<List<ClasseTiposCredenciais.TipoCredencial>>(list1);

        //        var observer = new ObservableCollection<ClasseTiposCredenciais.TipoCredencial>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        TiposCredenciais = observer;
        //        SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }

        //}

        //private void CarregaColecaoTecnologiasCredenciais()
        //{
        //    try
        //    {
        //        var list1 = _auxiliaresService.TecnologiaCredencialService.Listar();
        //        var list2 = Mapper.Map<List<ClasseTecnologiasCredenciais.TecnologiaCredencial>>(list1);

        //        var observer = new ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        TecnologiasCredenciais = observer;
        //        SelectedIndex = 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //private void CarregaColecaoCredenciaisStatus()
        //{
        //    try
        //    {
        //        var list1 = _auxiliaresService.CredencialStatusService.Listar();
        //        var list2 = Mapper.Map<List<ClasseCredenciaisStatus.CredencialStatus>>(list1);

        //        var observer = new ObservableCollection<ClasseCredenciaisStatus.CredencialStatus>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        CredenciaisStatus = observer;
        //        SelectedIndex = 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }

        //}

        //public void CarregaColecaoCredenciaisMotivos(int tipo = 0)
        //{
        //    try
        //    {
        //        var list1 = _auxiliaresService.CredencialMotivoService.Listar();
        //        var list2 = Mapper.Map<List<ClasseCredenciaisMotivos.CredencialMotivo>>(list1);

        //        var observer = new ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        CredenciaisMotivos = observer;
        //        SelectedIndex = 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }

        //}

        #endregion

        ////        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        ////        XmlNode _ClasseCredencial = _xmlDocument.CreateElement("ClasseCredencial");
        ////        _xmlDocument.AppendChild(_ClasseCredencial);

        ////        string _strSql;

        ////        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        ////        _strSql = "SELECT dbo.ColaboradoresCredenciais.ColaboradorCredencialID, dbo.ColaboradoresCredenciais.Colete, dbo.ColaboradoresCredenciais.Emissao," +
        ////            " dbo.ColaboradoresCredenciais.Validade, dbo.ColaboradoresEmpresas.Matricula, dbo.ColaboradoresEmpresas.Cargo, dbo.Empresas.Nome AS EmpresaNome," +
        ////            " dbo.Empresas.Apelido AS EmpresaApelido, dbo.Empresas.CNPJ, dbo.Empresas.Sigla, dbo.Empresas.Logo, dbo.Colaboradores.Nome AS ColaboradorNome," +
        ////            " dbo.Colaboradores.Apelido AS ColaboradorApelido, dbo.Colaboradores.CPF, dbo.Colaboradores.RG, dbo.Colaboradores.RGOrgLocal," +
        ////            " dbo.Colaboradores.RGOrgUF, dbo.Colaboradores.TelefoneEmergencia, dbo.Colaboradores.Foto, AreasAcessos_1.Identificacao AS Identificacao1," +
        ////            " dbo.AreasAcessos.Identificacao AS Identificacao2, dbo.Colaboradores.CNHCategoria,dbo.LayoutsCrachas.LayoutRPT " +
        ////            " FROM  dbo.AreasAcessos AS AreasAcessos_1 INNER JOIN dbo.ColaboradoresCredenciais ON" +
        ////            " AreasAcessos_1.AreaAcessoID = dbo.ColaboradoresCredenciais.ColaboradorPrivilegio1ID INNER JOIN dbo.AreasAcessos ON" +
        ////            " dbo.ColaboradoresCredenciais.ColaboradorPrivilegio2ID = dbo.AreasAcessos.AreaAcessoID INNER JOIN dbo.Colaboradores INNER JOIN" +
        ////            " dbo.ColaboradoresEmpresas ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresEmpresas.ColaboradorID INNER JOIN " +
        ////            "dbo.Empresas ON dbo.ColaboradoresEmpresas.EmpresaID = dbo.Empresas.EmpresaID ON" +
        ////            " dbo.ColaboradoresCredenciais.ColaboradorEmpresaID = dbo.ColaboradoresEmpresas.ColaboradorEmpresaID INNER JOIN dbo.LayoutsCrachas ON" +
        ////            " dbo.ColaboradoresCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID" +
        ////            " WHERE(dbo.ColaboradoresCredenciais.ColaboradorCredencialID =" + _colaboradorCredencialID + ")";

        ////        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        ////        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        ////        while (_sqlreader.Read())
        ////        {

        ////            XmlNode _ColaboradorCredencialID = _xmlDocument.CreateElement("ColaboradorCredencialID");
        ////            _ColaboradorCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCredencialID"].ToString())));
        ////            _ClasseCredencial.AppendChild(_ColaboradorCredencialID);

        ////            XmlNode _Colete = _xmlDocument.CreateElement("Colete");
        ////            _Colete.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Colete"].ToString())));
        ////            _ClasseCredencial.AppendChild(_Colete);

        ////            var dateStr = (_sqlreader["Emissao"].ToString());
        ////            if (!string.IsNullOrWhiteSpace(dateStr))
        ////            {
        ////                var dt2 = Convert.ToDateTime(dateStr);
        ////                XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
        ////                _Emissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        ////                _ClasseCredencial.AppendChild(_Emissao);
        ////            }

        ////            dateStr = (_sqlreader["Validade"].ToString());
        ////            if (!string.IsNullOrWhiteSpace(dateStr))
        ////            {
        ////                var dt2 = Convert.ToDateTime(dateStr);
        ////                XmlNode _Validade = _xmlDocument.CreateElement("Validade");
        ////                _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        ////                _ClasseCredencial.AppendChild(_Validade);
        ////            }

        ////            XmlNode _Matricula = _xmlDocument.CreateElement("Matricula");
        ////            _Matricula.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Matricula"].ToString())));
        ////            _ClasseCredencial.AppendChild(_Matricula);

        ////            XmlNode _Cargo = _xmlDocument.CreateElement("Cargo");
        ////            _Cargo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString())));
        ////            _ClasseCredencial.AppendChild(_Cargo);

        ////            XmlNode _EmpresaNome = _xmlDocument.CreateElement("EmpresaNome");
        ////            _EmpresaNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
        ////            _ClasseCredencial.AppendChild(_EmpresaNome);

        ////            XmlNode _EmpresaApelido = _xmlDocument.CreateElement("EmpresaApelido");
        ////            _EmpresaApelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaApelido"].ToString())));
        ////            _ClasseCredencial.AppendChild(_EmpresaApelido);

        ////            XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
        ////            _CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim().FormatarCnpj())));
        ////            _ClasseCredencial.AppendChild(_CNPJ);

        ////            XmlNode _Sigla = _xmlDocument.CreateElement("Sigla");
        ////            _Sigla.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Sigla"].ToString())));
        ////            _ClasseCredencial.AppendChild(_Sigla);

        ////            XmlNode _Logo = _xmlDocument.CreateElement("Logo");
        ////            _Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString().Trim())));
        ////            _ClasseCredencial.AppendChild(_Logo);

        ////            XmlNode _ColaboradorNome = _xmlDocument.CreateElement("ColaboradorNome");
        ////            _ColaboradorNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorNome"].ToString())));
        ////            _ClasseCredencial.AppendChild(_ColaboradorNome);

        ////            XmlNode _ColaboradorApelido = _xmlDocument.CreateElement("ColaboradorApelido");
        ////            _ColaboradorApelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorApelido"].ToString())));
        ////            _ClasseCredencial.AppendChild(_ColaboradorApelido);

        ////            XmlNode _CPF = _xmlDocument.CreateElement("CPF");
        ////            _CPF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().Trim().FormatarCpf())));
        ////            _ClasseCredencial.AppendChild(_CPF);

        ////            XmlNode _RG = _xmlDocument.CreateElement("RG");
        ////            _RG.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RG"].ToString().Trim())));
        ////            _ClasseCredencial.AppendChild(_RG);

        ////            XmlNode _RGOrgLocal = _xmlDocument.CreateElement("RGOrgLocal");
        ////            _RGOrgLocal.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgLocal"].ToString())));
        ////            _ClasseCredencial.AppendChild(_RGOrgLocal);

        ////            XmlNode _RGOrgUF = _xmlDocument.CreateElement("RGOrgUF");
        ////            _RGOrgUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgUF"].ToString())));
        ////            _ClasseCredencial.AppendChild(_RGOrgUF);

        ////            XmlNode _TelefoneEmergencia = _xmlDocument.CreateElement("TelefoneEmergencia");
        ////            _TelefoneEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneEmergencia"].ToString())));
        ////            _ClasseCredencial.AppendChild(_TelefoneEmergencia);

        ////            XmlNode _Foto = _xmlDocument.CreateElement("Foto");
        ////            _Foto.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Foto"].ToString().Trim())));
        ////            _ClasseCredencial.AppendChild(_Foto);

        ////            XmlNode _Identificacao1 = _xmlDocument.CreateElement("Identificacao1");
        ////            _Identificacao1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Identificacao1"].ToString())));
        ////            _ClasseCredencial.AppendChild(_Identificacao1);

        ////            XmlNode _Identificacao2 = _xmlDocument.CreateElement("Identificacao2");
        ////            _Identificacao2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Identificacao2"].ToString().Trim())));
        ////            _ClasseCredencial.AppendChild(_Identificacao2);

        ////            XmlNode _CNHCategoria = _xmlDocument.CreateElement("CNHCategoria");
        ////            _CNHCategoria.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHCategoria"].ToString().Trim())));
        ////            _ClasseCredencial.AppendChild(_CNHCategoria);

        ////            XmlNode _LayoutRPT = _xmlDocument.CreateElement("LayoutRPT");
        ////            _LayoutRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutRPT"].ToString().Trim())));
        ////            _ClasseCredencial.AppendChild(_LayoutRPT);

        ////        }

        ////        _sqlreader.Close();

        ////        _Con.Close();
        ////        string _xml = _xmlDocument.InnerXml;
        ////        _xmlDocument = null;
        ////        return _xml;
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        return null;
        ////    }

        ////}

        //private string RequisitaColaboradoresCredenciais(int _colaboradorID, string _empresaNome = "", int _tipoCredencialID = 0, int _credencialStatusID = 0)//Possibilidade de criar a pesquisa por Matriculatambem
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseColaboradoresCredenciais = _xmlDocument.CreateElement("ClasseColaboradoresCredenciais");
        //        _xmlDocument.AppendChild(_ClasseColaboradoresCredenciais);

        //        XmlNode _ColaboradoresCredenciais = _xmlDocument.CreateElement("ColaboradoresCredenciais");
        //        _ClasseColaboradoresCredenciais.AppendChild(_ColaboradoresCredenciais);

        //        string _strSql;
        //        string _credencialStatusSTR = "";
        //        string _tipoCredencialIDSTR = "";

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        _empresaNome = _empresaNome == "" ? "" : " AND dbo.Empresas.Nome like '%" + _empresaNome + "%' ";
        //        _credencialStatusSTR = _credencialStatusID == 0 ? "" : " AND CredencialStatusID = " + _credencialStatusID;
        //        _tipoCredencialIDSTR = _tipoCredencialID == 0 ? "" : " AND TipoCredencialID = " + _tipoCredencialID;

        //        //_validade = _validade == "" ? "" : " AND _validade like '%" + _validade + "%'";

        //        _strSql = "SELECT dbo.LayoutsCrachas.Nome AS LayoutCrachaNome, dbo.FormatosCredenciais.Descricao AS FormatoCredencialDescricao," +
        //            " dbo.ColaboradoresCredenciais.NumeroCredencial, dbo.ColaboradoresCredenciais.FC, dbo.ColaboradoresCredenciais.Emissao,dbo.ColaboradoresCredenciais.Impressa," +
        //            " dbo.ColaboradoresCredenciais.Validade,dbo.ColaboradoresCredenciais.Baixa, dbo.ColaboradoresCredenciais.Ativa,dbo.ColaboradoresCredenciais.ColaboradorPrivilegio1ID," +
        //            " dbo.ColaboradoresCredenciais.ColaboradorPrivilegio2ID,dbo.ColaboradoresCredenciais.Colete,dbo.ColaboradoresCredenciais.CredencialMotivoID," +
        //            " dbo.ColaboradoresCredenciais.CredencialStatusID, dbo.ColaboradoresCredenciais.ColaboradorEmpresaID, " +
        //            " dbo.ColaboradoresCredenciais.TipoCredencialID, dbo.ColaboradoresCredenciais.TecnologiaCredencialID, dbo.ColaboradoresCredenciais.FormatoCredencialID," +
        //            " dbo.ColaboradoresCredenciais.LayoutCrachaID, dbo.ColaboradoresCredenciais.ColaboradorCredencialID, dbo.Colaboradores.Nome AS ColaboradorNome," +
        //            " dbo.Empresas.Nome AS EmpresaNome, dbo.EmpresasContratos.Descricao AS ContratoDescricao,dbo.ColaboradoresEmpresas.EmpresaID," +
        //            " dbo.ColaboradoresEmpresas.ColaboradorID, dbo.ColaboradoresCredenciais.CardHolderGUID, dbo.ColaboradoresCredenciais.CredencialGUID," +
        //            " dbo.Colaboradores.Foto AS ColaboradorFoto, dbo.Colaboradores.CPF, dbo.Colaboradores.Motorista, dbo.Colaboradores.Apelido AS ColaboradorApelido," +
        //            " dbo.Colaboradores.Nome AS ColaboradorNome, dbo.Empresas.Logo AS EmpresaLogo,dbo.Empresas.Sigla AS EmpresaSigla," +
        //            " dbo.Empresas.Apelido AS EmpresaApelido, dbo.ColaboradoresEmpresas.Cargo, dbo.LayoutsCrachas.LayoutCrachaGUID, dbo.Empresas.CNPJ," +
        //            " dbo.FormatosCredenciais.FormatIDGUID FROM dbo.ColaboradoresCredenciais INNER JOIN dbo.FormatosCredenciais ON" +
        //            " dbo.ColaboradoresCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN dbo.ColaboradoresEmpresas ON" +
        //            " dbo.ColaboradoresCredenciais.ColaboradorEmpresaID = dbo.ColaboradoresEmpresas.ColaboradorEmpresaID INNER JOIN dbo.Empresas ON" +
        //            " dbo.ColaboradoresEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON " +
        //            " dbo.ColaboradoresEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN dbo.Colaboradores ON" +
        //            " dbo.ColaboradoresEmpresas.ColaboradorID = dbo.Colaboradores.ColaboradorID LEFT OUTER JOIN dbo.LayoutsCrachas ON" +
        //            " dbo.ColaboradoresCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID" +
        //            " WHERE dbo.ColaboradoresEmpresas.ColaboradorID =" + _colaboradorID + _credencialStatusSTR + _empresaNome + _tipoCredencialIDSTR +
        //            " ORDER BY dbo.ColaboradoresCredenciais.ColaboradorCredencialID DESC";

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _ColaboradorCredencial = _xmlDocument.CreateElement("ColaboradorCredencial");
        //            _ColaboradoresCredenciais.AppendChild(_ColaboradorCredencial);

        //            XmlNode _ColaboradorCredencialID = _xmlDocument.CreateElement("ColaboradorCredencialID");
        //            _ColaboradorCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCredencialID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorCredencialID);

        //            XmlNode _Ativa = _xmlDocument.CreateElement("Ativa");
        //            _Ativa.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativa"])).ToString()));
        //            _ColaboradorCredencial.AppendChild(_Ativa);

        //            XmlNode ColaboradorPrivilegio1ID = _xmlDocument.CreateElement("ColaboradorPrivilegio1ID");
        //            ColaboradorPrivilegio1ID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorPrivilegio1ID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(ColaboradorPrivilegio1ID);

        //            XmlNode ColaboradorPrivilegio2ID = _xmlDocument.CreateElement("ColaboradorPrivilegio2ID");
        //            ColaboradorPrivilegio2ID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorPrivilegio2ID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(ColaboradorPrivilegio2ID);

        //            XmlNode _ColaboradorEmpresaID = _xmlDocument.CreateElement("ColaboradorEmpresaID");
        //            _ColaboradorEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorEmpresaID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorEmpresaID);

        //            XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
        //            _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorID);

        //            XmlNode _CardHolderGUID = _xmlDocument.CreateElement("CardHolderGuid");
        //            _CardHolderGUID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CardHolderGuid"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_CardHolderGUID);

        //            XmlNode _CredencialGUID = _xmlDocument.CreateElement("CredencialGuid");
        //            _CredencialGUID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialGuid"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_CredencialGUID);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("ContratoDescricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContratoDescricao"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_Descricao);

        //            XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
        //            _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_Empresa);

        //            XmlNode _ColaboradorNome = _xmlDocument.CreateElement("ColaboradorNome");
        //            _ColaboradorNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorNome"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorNome);

        //            XmlNode _TecnologiaCredencialID = _xmlDocument.CreateElement("TecnologiaCredencialID");
        //            _TecnologiaCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TecnologiaCredencialID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_TecnologiaCredencialID);

        //            XmlNode _TecnologiaCredencialDescricao = _xmlDocument.CreateElement("TecnologiaCredencialDescricao");
        //            var _tec = TecnologiasCredenciais.FirstOrDefault(x => x.TecnologiaCredencialID == Convert.ToInt32(_sqlreader["TecnologiaCredencialID"].ToString()));
        //            if (_tec != null)
        //            {
        //                _TecnologiaCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tec.Descricao));
        //                _ColaboradorCredencial.AppendChild(_TecnologiaCredencialDescricao);
        //            }

        //            XmlNode _TipoCredencialID = _xmlDocument.CreateElement("TipoCredencialID");
        //            _TipoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoCredencialID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_TipoCredencialID);

        //            XmlNode _TipoCredencialDescricao = _xmlDocument.CreateElement("TipoCredencialDescricao");
        //            var _tip = TiposCredenciais.FirstOrDefault(x => x.TipoCredencialID == Convert.ToInt32(_sqlreader["TipoCredencialID"].ToString()));
        //            if (_tip != null)
        //            {
        //                _TipoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tip.Descricao));
        //                _ColaboradorCredencial.AppendChild(_TipoCredencialDescricao);
        //            }

        //            XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
        //            _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_LayoutCrachaID);

        //            XmlNode _LayoutCrachaNome = _xmlDocument.CreateElement("LayoutCrachaNome");
        //            _LayoutCrachaNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaNome"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_LayoutCrachaNome);

        //            XmlNode _FormatoCredencialID = _xmlDocument.CreateElement("FormatoCredencialID");
        //            _FormatoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_FormatoCredencialID);

        //            XmlNode _FormatoCredencialDescricao = _xmlDocument.CreateElement("FormatoCredencialDescricao");
        //            _FormatoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialDescricao"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_FormatoCredencialDescricao);

        //            XmlNode _NumeroCredencial = _xmlDocument.CreateElement("NumeroCredencial");
        //            _NumeroCredencial.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroCredencial"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_NumeroCredencial);

        //            XmlNode _FC = _xmlDocument.CreateElement("FC");
        //            _FC.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FC"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_FC);

        //            var dateStr = (_sqlreader["Emissao"].ToString());
        //            if (!string.IsNullOrWhiteSpace(dateStr))
        //            {
        //                var dt2 = Convert.ToDateTime(dateStr);
        //                XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
        //                _Emissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        //                _ColaboradorCredencial.AppendChild(_Emissao);
        //            }

        //            dateStr = (_sqlreader["Validade"].ToString());
        //            if (!string.IsNullOrWhiteSpace(dateStr))
        //            {
        //                var dt2 = Convert.ToDateTime(dateStr);
        //                XmlNode _Validade = _xmlDocument.CreateElement("Validade");
        //                _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        //                _ColaboradorCredencial.AppendChild(_Validade);
        //            }

        //            XmlNode _CredencialStatusID = _xmlDocument.CreateElement("CredencialStatusID");
        //            _CredencialStatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialStatusID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_CredencialStatusID);

        //            XmlNode _CredencialStatusDescricao = _xmlDocument.CreateElement("CredencialStatusDescricao");
        //            var _sta = CredenciaisStatus.FirstOrDefault(x => x.CredencialStatusID == Convert.ToInt32(_sqlreader["CredencialStatusID"].ToString()));
        //            if (_sta != null)
        //            {
        //                _CredencialStatusDescricao.AppendChild(_xmlDocument.CreateTextNode(_sta.Descricao));
        //                _ColaboradorCredencial.AppendChild(_CredencialStatusDescricao);
        //            }

        //            XmlNode _Vinculo9 = _xmlDocument.CreateElement("CPF");
        //            _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo9);

        //            XmlNode _Vinculo22 = _xmlDocument.CreateElement("Motorista");
        //            _Vinculo22.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Motorista"])).ToString()));
        //            _ColaboradorCredencial.AppendChild(_Vinculo22);

        //            XmlNode _Vinculo21 = _xmlDocument.CreateElement("ColaboradorApelido");
        //            _Vinculo21.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorApelido"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo21);

        //            XmlNode _Vinculo11 = _xmlDocument.CreateElement("ColaboradorFoto");
        //            _Vinculo11.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorFoto"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo11);

        //            XmlNode _Vinculo12 = _xmlDocument.CreateElement("EmpresaLogo");
        //            _Vinculo12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaLogo"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo12);

        //            XmlNode _Vinculo23 = _xmlDocument.CreateElement("EmpresaApelido");
        //            _Vinculo23.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaApelido"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo23);

        //            XmlNode _Vinculo13 = _xmlDocument.CreateElement("Cargo");
        //            _Vinculo13.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo13);

        //            XmlNode _Vinculo15 = _xmlDocument.CreateElement("LayoutCrachaGUID");
        //            _Vinculo15.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaGUID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo15);

        //            XmlNode _Vinculo19 = _xmlDocument.CreateElement("CNPJ");
        //            _Vinculo19.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo19);

        //            XmlNode _Vinculo20 = _xmlDocument.CreateElement("FormatIDGUID");
        //            _Vinculo20.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatIDGUID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo20);

        //            XmlNode _Colete = _xmlDocument.CreateElement("Colete");
        //            _Colete.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Colete"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Colete);

        //            XmlNode _EmpresaSigla = _xmlDocument.CreateElement("EmpresaSigla");
        //            _EmpresaSigla.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaSigla"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_EmpresaSigla);

        //            XmlNode _CredencialMotivoID = _xmlDocument.CreateElement("CredencialMotivoID");
        //            _CredencialMotivoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialMotivoID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_CredencialMotivoID);

        //            dateStr = (_sqlreader["Baixa"].ToString());
        //            if (!string.IsNullOrWhiteSpace(dateStr))
        //            {
        //                var dt2 = Convert.ToDateTime(dateStr);
        //                XmlNode _Baixa = _xmlDocument.CreateElement("Baixa");
        //                _Baixa.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        //                _ColaboradorCredencial.AppendChild(_Baixa);

        //            }

        //            XmlNode _Impressa = _xmlDocument.CreateElement("Impressa");
        //            _Impressa.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Impressa"])).ToString().Trim()));
        //            _ColaboradorCredencial.AppendChild(_Impressa);
        //        }
        //        _sqlreader.Close();

        //        _Con.Close();
        //        string _xml = _xmlDocument.InnerXml;
        //        _xmlDocument = null;
        //        return _xml;
        //    }

        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //    return null;
        //}

        //private void InsereImpressaoDB(int colaboradorCredencialID)
        //{
        //    try
        //    {
        //        if (colaboradorCredencialID != 0)
        //        {
        //            SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //            SqlCommand _sqlCmd;

        //            _sqlCmd = new SqlCommand("Insert into ColaboradoresCredenciaisImpressoes(ColaboradorCredencialID) values (@V1)", _Con);

        //            _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = colaboradorCredencialID;

        //            _sqlCmd.ExecuteNonQuery();

        //            _sqlCmd = new SqlCommand("Update ColaboradoresCredenciais Set Impressa=@v1" +
        //                    " Where ColaboradorCredencialID = @v0", _Con);

        //            _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = colaboradorCredencialID;

        //            _sqlCmd.Parameters.Add("@V1", SqlDbType.Bit).Value = 1;

        //            _sqlCmd.ExecuteNonQuery();

        //            _Con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Global.Log("Erro na void InsereImpressaoDB ex: " + ex);
        //    }
        //}

        //private DateTime validadeCursoContrato(int _colaborador = 0)
        //{
        //    try
        //    {
        //        //DateTime _menorDataCurso = Convert.ToDateTime("01-01-2999");
        //        //DateTime _menorDataContrato = Convert.ToDateTime("01-01-2999");

        //        string _menorDataCurso = "";
        //        string _menorDataContrato = "";

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        string _strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome,CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) " +
        //                         "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103)) AS Dias FROM dbo.Colaboradores " +
        //                         "INNER JOIN dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID where dbo.Colaboradores.Excluida = 0 And dbo.ColaboradoresCursos.Controlado = 1 And dbo.ColaboradoresCursos.ColaboradorID = " + _colaborador + " Order By Dias";

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        if (_sqlreader.Read())
        //        {

        //            //if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
        //            //{
        //            //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            _menorDataCurso = _sqlreader["ValidadeCurso"].ToString();
        //            // break;
        //            //}

        //        }
        //        _sqlreader.Close();

        //        //_strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome, dbo.EmpresasContratos.EmpresaID, dbo.EmpresasContratos.NumeroContrato, " +
        //        //          "CONVERT(datetime,dbo.EmpresasContratos.Validade,103) as DataContrato, DATEDIFF ( DAY , GETDATE(),  CONVERT(datetime, dbo.EmpresasContratos.Validade,103))  AS Dias " +
        //        //          "FROM  dbo.EmpresasContratos INNER JOIN dbo.ColaboradoresCredenciais ON dbo.EmpresasContratos.EmpresaID = dbo.ColaboradoresCredenciais.EmpresaID INNER JOIN dbo.Colaboradores " +
        //        //          "ON dbo.ColaboradoresCredenciais.ColaboradorID = dbo.Colaboradores.ColaboradorID WHERE (dbo.Colaboradores.Excluida = 0) And dbo.Colaboradores.ColaboradorID = " + _colaborador + " Order By Dias";

        //        //_sqlcmd = new SqlCommand(_strSql, _Con);
        //        //_sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        //if (_sqlreader.Read())
        //        //{

        //        //    // if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
        //        //    //{
        //        //    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        //    _menorDataContrato = _sqlreader["DataContrato"].ToString();
        //        //    // break;
        //        //    //}

        //        //}
        //        //_sqlreader.Close();

        //        //if (Convert.ToDateTime(_menorDataCurso) < Convert.ToDateTime(_menorDataContrato))
        //        //{
        //        return Convert.ToDateTime(_menorDataCurso);
        //        //}
        //        //else if (Convert.ToDateTime(_menorDataCurso) > Convert.ToDateTime(_menorDataContrato))
        //        //{
        //        //    return Convert.ToDateTime(_menorDataContrato);
        //        //}

        //        //return DateTime.Now;

        //    }
        //    catch (Exception ex)
        //    {
        //        return DateTime.Now;
        //    }
        //}
        //#endregion

        //public void OnImprimirCommand()
        //{
        //    try
        //    {
        //        if (ColaboradorCredencialSelecionado.Validade == null || !ColaboradorCredencialSelecionado.Ativa ||
        //            ColaboradorCredencialSelecionado.LayoutCrachaID == 0)
        //        {
        //            Global.PopupBox("Não é possível imprimir esta credencial!", 4);
        //            return;
        //        }

        //        //if (!Global.PopupBox("Confirma Impressão da Credencial para " + ColaboradorCredencialSelecionado.ColaboradorNome, 2))
        //        //{
        //        //    return;
        //        //}

        //        string _xml = RequisitaCredencial(ColaboradorCredencialSelecionado.ColaboradorCredencialID);

        //        XmlSerializer deserializer = new XmlSerializer(typeof(ClasseCredencial));

        //        XmlDocument xmldocument = new XmlDocument();
        //        xmldocument.LoadXml(_xml);

        //        TextReader reader = new StringReader(_xml);
        //        ClasseCredencial Credencial = new ClasseCredencial();
        //        Credencial = (ClasseCredencial)deserializer.Deserialize(reader);

        //        string _ArquivoRPT = Path.GetRandomFileName();
        //        _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;
        //        _ArquivoRPT = Path.ChangeExtension(_ArquivoRPT, ".rpt");
        //        byte[] buffer = Convert.FromBase64String(Credencial.LayoutRPT.Trim());

        //        File.WriteAllBytes(_ArquivoRPT, buffer);

        //        ReportDocument reportDocument = new ReportDocument();
        //        //reportDocument.Load("D:\\Meus Documentos\\CrachaModelo - Motorista.rpt");
        //        reportDocument.Load(_ArquivoRPT);

        //        //var report = new Cracha();
        //        var x = new List<ClasseCredencial>();
        //        x.Add(Credencial);
        //        reportDocument.SetDataSource(x);

        //        //Thread CarregaCracha_thr = new Thread(() =>
        //        //{

        //        //    System.Windows.Application.Current.Dispatcher.Invoke(() =>
        //        //    {

        //        PopupCredencial _popupCredencial = new PopupCredencial(reportDocument);
        //        _popupCredencial.ShowDialog();

        //        bool _result = _popupCredencial.Result;
        //        //    });

        //        //}
        //        //);

        //        //CarregaCracha_thr.Start();

        //        // GenericReportViewer.ViewerCore.ReportSource = reportDocument;

        //        //bool _resposta = SCManager.ImprimirCredencial(ColaboradorCredencialSelecionado);
        //        //if (_resposta)
        //        //{
        //        if (_result)
        //        {

        //            InsereImpressaoDB(ColaboradorCredencialSelecionado.ColaboradorCredencialID);
        //            // Global.PopupBox("Impressão Efetuada com Sucesso!", 1);
        //            ColaboradorCredencialSelecionado.Impressa = true;
        //            int _selectindex = SelectedIndex;
        //            CarregaColecaoColaboradoresCredenciais(ColaboradorCredencialSelecionado.ColaboradorID); //revisar a necessidade do carregamento
        //            SelectedIndex = _selectindex;
        //        }
        //        File.Delete(_ArquivoRPT);
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private DateTime VerificarMenorData(int _colaborador)
        //{
        //    try
        //    {

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        // Dim _dataValidade As DateTime = Date.Now
        //        DateTime _menorData = DateTime.Now;
        //        // Dim _menorDataContrato As String = ""

        //        var sqlSelectCurso = @"SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome, dbo.ColaboradoresCursos.Validade, dbo.ColaboradoresCursos.Controlado
        //               FROM dbo.Colaboradores INNER JOIN
        //                 dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID
        //               WHERE (dbo.Colaboradores.ColaboradorID = " + _colaborador + ") And (dbo.ColaboradoresCursos.Controlado = 1)";
        //        int idx = 0;
        //        // ---------------------------------------------------
        //        SqlCommand sqlcmd = new SqlCommand(sqlSelectCurso, _Con);
        //        SqlDataReader _sqlreader = sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        var list = new List<Colaborador>();
        //        while (_sqlreader.Read()) // Popular dados
        //        {
        //            list.Add(new Colaborador { Id = idx, ColaboradorId = Convert.ToInt32(_sqlreader["ColaboradorID"].ToString()), DataValidade = Convert.ToDateTime(_sqlreader["Validade"].ToString()) });
        //            idx = idx + 1;
        //        }

        //        var sqlSelectContrato = @"SELECT dbo.EmpresasContratos.EmpresaID, dbo.ColaboradoresEmpresas.EmpresaContratoID, dbo.ColaboradoresEmpresas.ColaboradorID,
        //                            dbo.ColaboradoresEmpresas.Cargo, dbo.ColaboradoresEmpresas.Matricula, dbo.ColaboradoresCredenciais.NumeroCredencial,
        //                            dbo.EmpresasContratos.Validade AS ValidadeContrato, dbo.ColaboradoresCredenciais.CredencialGUID, dbo.ColaboradoresEmpresas.Ativo
        //                            FROM dbo.EmpresasContratos RIGHT OUTER JOIN dbo.ColaboradoresEmpresas ON dbo.EmpresasContratos.EmpresaContratoID = dbo.ColaboradoresEmpresas.EmpresaContratoID
        //                            FULL OUTER JOIN dbo.ColaboradoresCredenciais ON dbo.ColaboradoresEmpresas.ColaboradorEmpresaID = dbo.ColaboradoresCredenciais.ColaboradorEmpresaID 
        //                            WHERE dbo.ColaboradoresEmpresas.Ativo = 1 AND dbo.ColaboradoresEmpresas.ColaboradorID = " + _colaborador;

        //        SqlCommand sqlcmd2 = new SqlCommand(sqlSelectContrato, _Con);
        //        SqlDataReader _sqlreader2 = sqlcmd2.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader2.Read()) // Popular dados
        //        {
        //            list.Add(new Colaborador { Id = idx, CredencialGuidId = _sqlreader2["CredencialGUID"].ToString(), ColaboradorId = Convert.ToInt32(_sqlreader2["ColaboradorID"].ToString()), DataValidade = Convert.ToDateTime(_sqlreader2["ValidadeContrato"].ToString()) });
        //            idx = idx + 1;
        //        }

        //        // group by colaboradores
        //        var colaboradoresGroup = list.GroupBy(n => n.ColaboradorId).Select(n => n.First()).ToList();
        //        // ----------------------------------------------------
        //        // Itera entre os colaboradores e retorna o item com a menor data
        //        foreach (Colaborador item in colaboradoresGroup)
        //        {
        //            var d1 = (from d in list
        //                      where d.ColaboradorId == item.ColaboradorId
        //                      select d.DataValidade).Min();
        //            var dados = list.Where(x => x.DataValidade.Equals(d1) & x.ColaboradorId.Equals(item.ColaboradorId)).FirstOrDefault();
        //            _menorData = dados.DataValidade;
        //        }
        //        // ----------------------------------------------------
        //        return _menorData;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return DateTime.Now;
        //}
    }
}