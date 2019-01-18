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
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels.Commands;
using iModSCCredenciamento.ViewModels.Comportamento;
using iModSCCredenciamento.Views.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradoresEmpresasViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaContratosService _empresaContratoService = new EmpresaContratoService();
        private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IColaboradorEmpresaService _service = new ColaboradorEmpresaService();
        private ColaboradorView _colaboradorView;

        #region  Propriedades

        public List<EmpresaContrato> Contratos { get; private set; }
        public List<Empresa> Empresas { get; private set; }
        public Empresa Empresa { get; set; }
        public ColaboradorEmpresaView Entity { get; set; }
        public ObservableCollection<ColaboradorEmpresaView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public ColaboradoresEmpresasViewModel()
        {
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            var lst1 = _empresaService.Listar();
            var lst2 = _empresaContratoService.Listar();
            Empresas = new List<Empresa>();
            Contratos = new List<EmpresaContrato>();
            Empresas.AddRange(lst1);
            Contratos.AddRange(lst2);
        }

        public void ListarContratos(Empresa empresa)
        {
            if (empresa == null)
            {
                return;
            }

            var lstContratos = _empresaContratoService.Listar(empresa.EmpresaId).ToList();
            Contratos.Clear();
            //Manipular concatenaçção de conrato
            lstContratos.ForEach(n =>
            {
                n.Descricao = $"{n.Descricao} - {n.NumeroContrato}";
                Contratos.Add(n);
            });
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
                if (Entity == null)
                {
                    return;
                }

                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                n1.ColaboradorId = _colaboradorView.ColaboradorId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<ColaboradorEmpresaView>(n1);
                //Adicionar o nome da empresa e o contrato
                SetDadosEmpresaContrato(n2);
                EntityObserver.Insert(0, n2);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void SetDadosEmpresaContrato(ColaboradorEmpresaView entity)
        {
            var empresa = Empresas.FirstOrDefault(n => n.EmpresaId == entity.EmpresaId);
            var contrato = Contratos.FirstOrDefault(n => n.EmpresaContratoId == entity.EmpresaContratoId);
            if (empresa != null)
            {
                entity.EmpresaNome = empresa.Nome;//Setar o nome da empresa para ser exibida na list view
            }

            if (contrato != null)
            {
                entity.Descricao = contrato.Descricao;//Setar o nome do contrato para ser exibida na list view
            }
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            Entity = new ColaboradorEmpresaView();
            Entity.Matricula = string.Format("{0:#,##0}", Global.GerarID()) + "-" + String.Format("{0:yy}", DateTime.Now);
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
            try
            {
                if (Entity == null)
                {
                    return;
                }

                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                _service.Alterar(n1);
                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
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
            try
            {
                if (Entity == null)
                {
                    return;
                }

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes)
                {
                    return;
                }

                var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                _service.Remover(n1);
                //Retirar empresa da coleção
                EntityObserver.Remove(Entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
        }

        public void AtualizarDados(ColaboradorView entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _colaboradorView = entity;
            //Obter dados
            var list1 = _service.Listar(entity.ColaboradorId);
            var list2 = Mapper.Map<List<ColaboradorEmpresaView>>(list1.OrderByDescending(n => n.ColaboradorEmpresaId));
            EntityObserver = new ObservableCollection<ColaboradorEmpresaView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
        }

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
        ///     Validar Regras de Negócio
        /// </summary>
        public void Validar()
        {
        }

        #endregion

        //private void CarregaUI()
        //{
        //    CarregaColecaoEmpresas();
        //    CarregaColecaoContratos();
        //    //CarregaColeçãoFormatosCredenciais();

        //}

        //#region Variaveis Privadas

        //private ObservableCollection<ColaboradorEmpresaView> _ColaboradoresEmpresas;

        //private ObservableCollection<ClasseVinculos.Vinculo> _Vinculos;

        //private ObservableCollection<EmpresaView> _Empresas;

        //private ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> _FormatosCredenciais;

        //private ObservableCollection<EmpresaLayoutCrachaView> _EmpresasLayoutsCrachas;

        //private ObservableCollection<EmpresaContratoView> _Contratos;

        //private ColaboradorEmpresaView _ColaboradorEmpresaSelecionado;

        //private ColaboradorEmpresaView _ColaboradorEmpresaTemp = new ColaboradorEmpresaView();

        //private List<ColaboradorEmpresaView> _ColaboradoresEmpresasTemp = new List<ColaboradorEmpresaView>();

        //PopupPesquisaColaboradoresEmpresas popupPesquisaColaboradoresEmpresas;

        //private int _selectedIndex;

        //private int _ColaboradorSelecionadaID;

        //private bool _HabilitaEdicao;

        //private string _Criterios = "";

        //private int _selectedIndexTemp;

        //private string _Validade;

        //#endregion

        //#region Contrutores
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

        //public ColaboradorEmpresaView ColaboradorEmpresaSelecionado
        //{
        //    get
        //    {
        //        return _ColaboradorEmpresaSelecionado;
        //    }
        //    set
        //    {
        //        _ColaboradorEmpresaSelecionado = value;
        //        //base.OnPropertyChanged("SelectedItem");
        //        base.OnPropertyChanged();
        //        if (ColaboradorEmpresaSelecionado != null)
        //        {
        //            //CarregaColeçãoEmpresasLayoutsCrachas(Convert.ToInt32(ColaboradorEmpresaSelecionado.EmpresaID));
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
        //        Thread CarregaColecaoColaboradoresEmpresas_thr = new Thread(() => CarregaColecaoColaboradoresEmpresas(Convert.ToInt32(_ColaboradorID)));
        //        CarregaColecaoColaboradoresEmpresas_thr.Start();
        //        //CarregaColecaoColaboradoresEmpresas(Convert.ToInt32(_ColaboradorID));

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        //public void OnEditarCommand()
        //{
        //    try
        //    {
        //        //o preenchimento de _ColaboradoresEmpresasTemp é apenas para apoiar o metodo VerificaVinculo()
        //        _ColaboradoresEmpresasTemp.Clear();
        //        foreach (var x in ColaboradoresEmpresas)
        //        {
        //            _ColaboradoresEmpresasTemp.Add(x);
        //        }

        //        //_ColaboradorEmpresaTemp = ColaboradorEmpresaSelecionado.CriaCopia(ColaboradorEmpresaSelecionado);
        //        _selectedIndexTemp = SelectedIndex;

        //        //CarregaColecaoContratos(_ColaboradorEmpresaTemp.EmpresaID);

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
        //        ColaboradoresEmpresas[_selectedIndexTemp] = _ColaboradorEmpresaTemp;
        //        SelectedIndex = _selectedIndexTemp;
        //        HabilitaEdicao = false;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public void OnSalvarAdicaoCommand()
        //{
        //    try
        //    {

        //        HabilitaEdicao = false;

        //        //IMOD.Domain.Entities.ColaboradorEmpresa ColaboradorEmpresaEntity = new IMOD.Domain.Entities.ColaboradorEmpresa();
        //        //g.TranportarDados(ColaboradorEmpresaSelecionado, 1, ColaboradorEmpresaEntity);
        //        //var repositorio = new IMOD.Infra.Repositorios.ColaboradorEmpresaRepositorio();

        //        var entity = Mapper.Map<ColaboradorEmpresa>(ColaboradorEmpresaSelecionado);
        //        var repositorio = new ColaboradorEmpresaService();
        //        repositorio.Criar(entity);
        //        var id = entity.ColaboradorEmpresaId;

        //        ColaboradorEmpresaView _ColaboradorEmpresaSelecionadoPro = new ColaboradorEmpresaView();
        //        _ColaboradorEmpresaSelecionadoPro = ColaboradorEmpresaSelecionado;
        //        _ColaboradorEmpresaSelecionadoPro.ColaboradorEmpresaId = id;
        //        ColaboradoresEmpresas = new ObservableCollection<ColaboradorEmpresaView>(_ColaboradoresEmpresasTemp);
        //        ColaboradoresEmpresas.Add(_ColaboradorEmpresaSelecionadoPro);

        //        //Thread CarregaColecaoColaboradoresEmpresas_thr = new Thread(() => CarregaColecaoColaboradoresEmpresas(ColaboradorEmpresaSelecionado.ColaboradorID));
        //        //CarregaColecaoColaboradoresEmpresas_thr.Start();

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public void OnSalvarEdicaoCommand()
        //{
        //    try
        //    {
        //        //HabilitaEdicao = false;
        //        //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresEmpresas));

        //        //ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _ColaboradorEmpresaTemp = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>();
        //        //ClasseColaboradoresEmpresas _ClasseColaboradorerEmpresasTemp = new ClasseColaboradoresEmpresas();
        //        //_ColaboradorEmpresaTemp.Add(ColaboradorEmpresaSelecionado);
        //        //_ClasseColaboradorerEmpresasTemp.ColaboradoresEmpresas = _ColaboradorEmpresaTemp;

        //        //IMOD.Domain.Entities.ColaboradorEmpresa ColaboradorEmpresaEntity = new IMOD.Domain.Entities.ColaboradorEmpresa();
        //        //g.TranportarDados(ColaboradorEmpresaSelecionado, 1, ColaboradorEmpresaEntity);

        //        //var repositorio = new IMOD.Infra.Repositorios.ColaboradorEmpresaRepositorio();

        //        var entity = Mapper.Map<ColaboradorEmpresa>(ColaboradorEmpresaSelecionado);
        //        var repositorio = new ColaboradorEmpresaService();
        //        repositorio.Alterar(entity);

        //        var id = entity.ColaboradorEmpresaId;
        //        AtualizaCredenciais(id);

        //        //string xmlString;

        //        //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
        //        //{

        //        //    using (XmlTextWriter xw = new XmlTextWriter(sw))
        //        //    {
        //        //        xw.Formatting = Formatting.Indented;
        //        //        serializer.Serialize(xw, _ClasseColaboradorerEmpresasTemp);
        //        //        xmlString = sw.ToString();
        //        //    }

        //        //}

        //        //InsereColaboradorEmpresaBD(xmlString);
        //        //if (!ColaboradorEmpresaSelecionado.Ativo)
        //        //{
        //        //    AtualizaCredenciais(ColaboradorEmpresaSelecionado.ColaboradorEmpresaID);
        //        //}

        //        // ColaboradoresEmpresas[_selectedIndexTemp] = ColaboradorEmpresaSelecionado;

        //        //Thread CarregaColecaoColaboradoresEmpresas_thr = new Thread(() => CarregaColecaoColaboradoresEmpresas(ColaboradorEmpresaSelecionado.ColaboradorID));
        //        //CarregaColecaoColaboradoresEmpresas_thr.Start();

        //        //_ClasseEmpresasSegurosTemp = null;

        //        //_SegurosTemp.Clear();
        //        //_seguroTemp = null;

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public void OnAdicionarCommand()
        //{
        //    try
        //    {
        //        _ColaboradoresEmpresasTemp.Clear();

        //        foreach (var x in ColaboradoresEmpresas)
        //        {
        //            _ColaboradoresEmpresasTemp.Add(x);
        //        }

        //        _selectedIndexTemp = SelectedIndex;
        //        ColaboradoresEmpresas.Clear();
        //        //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
        //        //_seguro.EmpresaID = EmpresaSelecionadaID;
        //        //Seguros.Add(_seguro);
        //        CarregaColecaoEmpresas();
        //        _ColaboradorEmpresaTemp = new ColaboradorEmpresaView();
        //        _ColaboradorEmpresaTemp.ColaboradorId = ColaboradorSelecionadaID;
        //        _ColaboradorEmpresaTemp.Matricula = string.Format("{0:#,##0}", Global.GerarID()) + "-" + String.Format("{0:yy}", DateTime.Now);
        //        _ColaboradorEmpresaTemp.Ativo = true;
        //        ColaboradoresEmpresas.Add(_ColaboradorEmpresaTemp);

        //        SelectedIndex = 0;
        //        HabilitaEdicao = true;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        ////public void OnSelecionaEmpresaCommand(int _empresaID)
        ////{
        ////    CarregaColecaoContratos(_empresaID);
        ////}

        //public void OnCancelarAdicaoCommand()
        //{
        //    try
        //    {
        //        ColaboradoresEmpresas = null;
        //        ColaboradoresEmpresas = new ObservableCollection<ColaboradorEmpresaView>(_ColaboradoresEmpresasTemp);
        //        SelectedIndex = _selectedIndexTemp;
        //        _ColaboradoresEmpresasTemp.Clear();
        //        HabilitaEdicao = false;
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
        //                var entity = Mapper.Map<ColaboradorEmpresa>(ColaboradorEmpresaSelecionado);
        //                var repositorio = new ColaboradorEmpresaService();
        //                repositorio.Remover(entity);

        //                ColaboradoresEmpresas.Remove(ColaboradorEmpresaSelecionado);
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
        //        popupPesquisaColaboradoresEmpresas = new PopupPesquisaColaboradoresEmpresas();
        //        popupPesquisaColaboradoresEmpresas.EfetuarProcura += On_EfetuarProcura;
        //        popupPesquisaColaboradoresEmpresas.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public void On_EfetuarProcura(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        object vetor = popupPesquisaColaboradoresEmpresas.Criterio.Split((char)(20));
        //        int _colaboradorID = ColaboradorSelecionadaID;
        //        string _matricula = ((string[])vetor)[0];
        //        string _empresaNome = ((string[])vetor)[1];
        //        string _cargo = ((string[])vetor)[2];
        //        int _ativo = Convert.ToInt32(((string[])vetor)[3]);

        //        CarregaColecaoColaboradoresEmpresas(_colaboradorID, _empresaNome, _cargo, _matricula, _ativo);
        //        SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        //#endregion

        //#region Carregamento das Colecoes
        //private void CarregaColecaoColaboradoresEmpresas(int _colaboradorID, string _empresaNome = "", string _cargo = "", string _matricula = "", int _ativo = 2)
        //{
        //    try
        //    {
        //        var service = new ColaboradorEmpresaService();
        //        if (!string.IsNullOrWhiteSpace(_cargo)) _cargo = $"%{_cargo}%";
        //        if (!string.IsNullOrWhiteSpace(_matricula)) _matricula = $"%{_matricula}%";
        //        var list1 = service.Listar(_colaboradorID, _cargo, _matricula);

        //        var list2 = Mapper.Map<List<ColaboradorEmpresaView>>(list1);
        //        var observer = new ObservableCollection<ColaboradorEmpresaView>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        ColaboradoresEmpresas = observer;

        //        //Hotfix auto-selecionar registro do topo da ListView
        //        var topList = observer.FirstOrDefault();
        //        ColaboradorEmpresaSelecionado = topList;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //private void CarregaColecaoEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", string _quantidaderegistro = "500")
        //{
        //    try
        //    {

        //        var service = new EmpresaService();
        //        if (!string.IsNullOrWhiteSpace(_nome)) _nome = $"%{_nome}%";
        //        if (!string.IsNullOrWhiteSpace(_apelido)) _apelido = $"%{_apelido}%";
        //        if (!string.IsNullOrWhiteSpace(_cNPJ)) _cNPJ = $"%{_cNPJ}%";

        //        var list1 = service.Listar(_empresaID, _nome, _apelido, _cNPJ);
        //        var list2 = Mapper.Map<List<EmpresaView>>(list1);

        //        var observer = new ObservableCollection<EmpresaView>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        Empresas = observer;
        //        SelectedIndex = 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}

        //public void CarregaColecaoContratos(int empresaID = 0)
        //{

        //    try
        //    {

        //        var service = new IMOD.Application.Service.EmpresaContratoService();
        //        var list1 = service.Listar(empresaID);

        //        var list2 = Mapper.Map<List<EmpresaContratoView>>(list1.OrderBy(n => n.EmpresaId));

        //        var observer = new ObservableCollection<EmpresaContratoView>();
        //        list2.ForEach(n =>
        //        {
        //            observer.Add(n);
        //        });

        //        this.Contratos = observer;
        //        SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
        //    }
        //}
        //#endregion

        #region Data Access

        //private string RequisitaContratos(int _empresaID)
        //{
        //    try
        //    {
        //        var _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

        //        XmlNode _ClasseContratosEmpresas = _xmlDocument.CreateElement ("ClasseEmpresasContratos");
        //        _xmlDocument.AppendChild (_ClasseContratosEmpresas);

        //        XmlNode _EmpresasContratos = _xmlDocument.CreateElement ("EmpresasContratos");
        //        _ClasseContratosEmpresas.AppendChild (_EmpresasContratos);

        //        string _strSql;

        //        var _Con = new SqlConnection (Global._connectionString);
        //        _Con.Open();

        //        _strSql = "select * from EmpresasContratos where EmpresaID = " + _empresaID + " and StatusID=1 order by EmpresaContratoID desc";

        //        var _sqlcmd = new SqlCommand (_strSql, _Con);
        //        var _sqlreader = _sqlcmd.ExecuteReader (CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {
        //            XmlNode _EmpresaContrato = _xmlDocument.CreateElement ("EmpresaContrato");
        //            _EmpresasContratos.AppendChild (_EmpresaContrato);

        //            XmlNode _EmpresaContratoID = _xmlDocument.CreateElement ("EmpresaContratoID");
        //            _EmpresaContratoID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EmpresaContratoID"].ToString()));
        //            _EmpresaContrato.AppendChild (_EmpresaContratoID);

        //            XmlNode _EmpresaID = _xmlDocument.CreateElement ("EmpresaID");
        //            _EmpresaID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EmpresaID"].ToString()));
        //            _EmpresaContrato.AppendChild (_EmpresaID);

        //            XmlNode _NumeroContrato = _xmlDocument.CreateElement ("NumeroContrato");
        //            _NumeroContrato.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["NumeroContrato"].ToString()));
        //            _EmpresaContrato.AppendChild (_NumeroContrato);

        //            XmlNode _Descricao = _xmlDocument.CreateElement ("Descricao");
        //            _Descricao.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Descricao"].ToString()));
        //            _EmpresaContrato.AppendChild (_Descricao);

        //            XmlNode _Emissao = _xmlDocument.CreateElement ("Emissao");
        //            _Emissao.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Emissao"].ToString()));
        //            _EmpresaContrato.AppendChild (_Emissao);

        //            XmlNode _Validade = _xmlDocument.CreateElement ("Validade");
        //            _Validade.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Validade"].ToString()));
        //            _EmpresaContrato.AppendChild (_Validade);

        //            XmlNode _Terceirizada = _xmlDocument.CreateElement ("Terceirizada");
        //            _Terceirizada.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Terceirizada"].ToString()));
        //            _EmpresaContrato.AppendChild (_Terceirizada);

        //            XmlNode _Contratante = _xmlDocument.CreateElement ("Contratante");
        //            _Contratante.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Contratante"].ToString()));
        //            _EmpresaContrato.AppendChild (_Contratante);

        //            XmlNode _IsencaoCobranca = _xmlDocument.CreateElement ("IsencaoCobranca");
        //            _IsencaoCobranca.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["IsencaoCobranca"].ToString()));
        //            _EmpresaContrato.AppendChild (_IsencaoCobranca);

        //            XmlNode _TipoCobrancaID = _xmlDocument.CreateElement ("TipoCobrancaID");
        //            _TipoCobrancaID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["TipoCobrancaID"].ToString()));
        //            _EmpresaContrato.AppendChild (_TipoCobrancaID);

        //            XmlNode _CobrancaEmpresaID = _xmlDocument.CreateElement ("CobrancaEmpresaID");
        //            _CobrancaEmpresaID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["CobrancaEmpresaID"].ToString()));
        //            _EmpresaContrato.AppendChild (_CobrancaEmpresaID);

        //            XmlNode _CEP = _xmlDocument.CreateElement ("CEP");
        //            _CEP.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["CEP"].ToString()));
        //            _EmpresaContrato.AppendChild (_CEP);

        //            XmlNode _Endereco = _xmlDocument.CreateElement ("Endereco");
        //            _Endereco.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Endereco"].ToString()));
        //            _EmpresaContrato.AppendChild (_Endereco);

        //            XmlNode _Complemento = _xmlDocument.CreateElement ("Complemento");
        //            _Complemento.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Complemento"].ToString()));
        //            _EmpresaContrato.AppendChild (_Complemento);

        //            XmlNode _Bairro = _xmlDocument.CreateElement ("Bairro");
        //            _Bairro.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Bairro"].ToString()));
        //            _EmpresaContrato.AppendChild (_Bairro);

        //            XmlNode _MunicipioID = _xmlDocument.CreateElement ("MunicipioID");
        //            _MunicipioID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["MunicipioID"].ToString()));
        //            _EmpresaContrato.AppendChild (_MunicipioID);

        //            XmlNode _EstadoID = _xmlDocument.CreateElement ("EstadoID");
        //            _EstadoID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EstadoID"].ToString()));
        //            _EmpresaContrato.AppendChild (_EstadoID);

        //            XmlNode _NomeResp = _xmlDocument.CreateElement ("NomeResp");
        //            _NomeResp.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["NomeResp"].ToString()));
        //            _EmpresaContrato.AppendChild (_NomeResp);

        //            XmlNode _TelefoneResp = _xmlDocument.CreateElement ("TelefoneResp");
        //            _TelefoneResp.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["TelefoneResp"].ToString()));
        //            _EmpresaContrato.AppendChild (_TelefoneResp);

        //            XmlNode _CelularResp = _xmlDocument.CreateElement ("CelularResp");
        //            _CelularResp.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["CelularResp"].ToString()));
        //            _EmpresaContrato.AppendChild (_CelularResp);

        //            XmlNode _EmailResp = _xmlDocument.CreateElement ("EmailResp");
        //            _EmailResp.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EmailResp"].ToString()));
        //            _EmpresaContrato.AppendChild (_EmailResp);

        //            XmlNode _Numero = _xmlDocument.CreateElement ("Numero");
        //            _Numero.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Numero"].ToString()));
        //            _EmpresaContrato.AppendChild (_Numero);

        //            XmlNode _StatusID = _xmlDocument.CreateElement ("StatusID");
        //            _StatusID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["StatusID"].ToString()));
        //            _EmpresaContrato.AppendChild (_StatusID);

        //            XmlNode _TipoAcessoID = _xmlDocument.CreateElement ("TipoAcessoID");
        //            _TipoAcessoID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["TipoAcessoID"].ToString()));
        //            _EmpresaContrato.AppendChild (_TipoAcessoID);

        //            XmlNode _NomeArquivo = _xmlDocument.CreateElement ("NomeArquivo");
        //            _NomeArquivo.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["NomeArquivo"].ToString()));
        //            _EmpresaContrato.AppendChild (_NomeArquivo);

        //            XmlNode _Arquivo = _xmlDocument.CreateElement ("Arquivo");
        //            //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Arquivo"].ToString())));
        //            _EmpresaContrato.AppendChild (_Arquivo);
        //        }

        //        _sqlreader.Close();

        //        _Con.Close();
        //        var _xml = _xmlDocument.InnerXml;
        //        _xmlDocument = null;
        //        return _xml;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    return null;
        //}

        //private DateTime validadeCursoContrato(int _colaborador = 0)
        //{
        //    try
        //    {
        //        //DateTime _menorDataCurso = Convert.ToDateTime("01-01-2999");
        //        //DateTime _menorDataContrato = Convert.ToDateTime("01-01-2999");

        //        var _menorDataCurso = "";
        //        var _menorDataContrato = "";

        //        var _Con = new SqlConnection (Global._connectionString);
        //        _Con.Open();

        //        var _strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome,CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) " +
        //                      "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103)) AS Dias FROM dbo.Colaboradores " +
        //                      "INNER JOIN dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID where dbo.Colaboradores.Excluida = 0 And dbo.ColaboradoresCursos.Controlado = 1 And dbo.ColaboradoresCursos.ColaboradorID = " + _colaborador + " Order By Dias";

        //        var _sqlcmd = new SqlCommand (_strSql, _Con);
        //        var _sqlreader = _sqlcmd.ExecuteReader (CommandBehavior.Default);
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
        //        //          "FROM  dbo.EmpresasContratos INNER JOIN dbo.ColaboradoresEmpresas ON dbo.EmpresasContratos.EmpresaID = dbo.ColaboradoresEmpresas.EmpresaID INNER JOIN dbo.Colaboradores " +
        //        //          "ON dbo.ColaboradoresEmpresas.ColaboradorID = dbo.Colaboradores.ColaboradorID WHERE (dbo.Colaboradores.Excluida = 0) And dbo.Colaboradores.ColaboradorID = " + _colaborador + " Order By Dias";

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
        //        return Convert.ToDateTime (_menorDataCurso);
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

        #endregion

        //    catch (Exception ex)
        //    {
        //        Utils.TraceException(ex);
        //    }
        //}
    }
}