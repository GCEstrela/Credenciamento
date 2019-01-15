using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using iModSCCredenciamento.Views.Model;
using IMOD.Application.Interfaces;
using iModSCCredenciamento.ViewModels.Comportamento;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using iModSCCredenciamento.ViewModels.Commands;

namespace iModSCCredenciamento.ViewModels
{
    class VeiculosSegurosViewModel : ViewModelBase
    {

        #region  Propriedades

        private readonly IVeiculoSeguroService _service = new VeiculoSeguroService();
        private VeiculoView _veiculoView;

        public VeiculoSeguroView Entity { get; set; }
        public ObservableCollection<VeiculoSeguroView> EntityObserver { get; set; }

        VeiculoSeguroView EntidadeTMP = new VeiculoSeguroView();

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        #region Inicializacao

        public void AtualizarDados(VeiculoView entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _veiculoView = entity;
            //Obter dados

            var list1 = _service.Listar(entity.EquipamentoVeiculoId, null, null);
            var list2 = Mapper.Map<List<VeiculoSeguroView>>(list1);
            var observer = new ObservableCollection<VeiculoSeguro>();

            EntityObserver = new ObservableCollection<VeiculoSeguroView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
        }

        public VeiculosSegurosViewModel()
        {
            ItensDePesquisaConfigura();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #endregion

        #region Metódos


        /// <summary>
        ///     Relação dos itens de pesquisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Seguro"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Apólice"));
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
                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
                n1.VeiculoId = _veiculoView.EquipamentoVeiculoId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<VeiculoSeguroView>(n1);
                EntityObserver.Insert(0, n2);
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
            EntidadeTMP = Entity;
            Entity = new VeiculoSeguroView();
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
                if (Entity == null) return;
                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
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
                Entity = EntidadeTMP;

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

                var n1 = Mapper.Map<VeiculoSeguro>(Entity);
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

        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
            try
            {
                if (_veiculoView == null) return;

                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Por nome
                if (num.Key == 1)
                {
                    var l1 = _service.Listar(_veiculoView.EquipamentoVeiculoId, $"%{pesquisa}%");
                    PopularObserver(l1);
                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PopularObserver(ICollection<VeiculoSeguro> list)
        {
            try
            {
                var list2 = Mapper.Map<List<VeiculoSeguroView>>(list.OrderBy(n => n.NomeArquivo));
                EntityObserver = new ObservableCollection<VeiculoSeguroView>();
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
            throw new NotImplementedException();
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
    }

    //#region Variaveis Privadas

    //private ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro> _Seguros;

    //private ClasseVeiculosSeguros.VeiculoSeguro _SeguroSelecionado;

    //private ClasseVeiculosSeguros.VeiculoSeguro _seguroTemp = new ClasseVeiculosSeguros.VeiculoSeguro();

    //private List<ClasseVeiculosSeguros.VeiculoSeguro> _SegurosTemp = new List<ClasseVeiculosSeguros.VeiculoSeguro>();

    //PopupPesquisaSeguro popupPesquisaSeguro;

    //private int _selectedIndex;

    //private int _VeiculoSelecionadaID;

    //private bool _HabilitaEdicao;

    //private string _Criterios = "";

    //private int _selectedIndexTemp;

    //#endregion

    //#region Contrutores
    //public ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro> Seguros
    //{
    //    get
    //    {
    //        return _Seguros;
    //    }

    //    set
    //    {
    //        if (_Seguros != value)
    //        {
    //            _Seguros = value;
    //            OnPropertyChanged();

    //        }
    //    }
    //}

    //public ClasseVeiculosSeguros.VeiculoSeguro SeguroSelecionado
    //{
    //    get
    //    {
    //        return _SeguroSelecionado;
    //    }
    //    set
    //    {
    //        _SeguroSelecionado = value;
    //        base.OnPropertyChanged("SelectedItem");
    //        if (SeguroSelecionado != null)
    //        {
    //            //OnVeiculoSelecionada();
    //        }

    //    }
    //}

    //public int VeiculoSelecionadaID
    //{
    //    get
    //    {
    //        return _VeiculoSelecionadaID;
    //    }
    //    set
    //    {
    //        _VeiculoSelecionadaID = value;
    //        base.OnPropertyChanged();
    //        if (VeiculoSelecionadaID != null)
    //        {
    //            //OnVeiculoSelecionada();
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
    //#endregion

    //#region Comandos dos Botoes
    //public void OnAtualizaCommand(object veiculoID)
    //{
    //    VeiculoSelecionadaID = Convert.ToInt32(veiculoID);
    //    Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(Convert.ToInt32(veiculoID)));
    //    CarregaColecaoSeguros_thr.Start();
    //}

    //public void OnBuscarArquivoCommand()
    //{
    //    try
    //    {
    //        var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
    //        var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
    //        if (arq == null) return;
    //        _seguroTemp.NomeArquivo = arq.Nome;
    //        _seguroTemp.Arquivo = arq.FormatoBase64;
    //        if (Seguros != null)
    //            Seguros[0].NomeArquivo = arq.Nome;

    //    }
    //    catch (Exception ex)
    //    {
    //        WpfHelp.Mbox(ex.Message);
    //        Utils.TraceException(ex);
    //    }

    //}

    //public void OnAbrirArquivoCommand()
    //{
    //    try
    //    {
    //        var arquivoStr = SeguroSelecionado.Arquivo;
    //        var nomeArquivo = SeguroSelecionado.NomeArquivo;
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
    //        _seguroTemp = SeguroSelecionado.CriaCopia(SeguroSelecionado);
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
    //        Seguros[_selectedIndexTemp] = _seguroTemp;
    //        SelectedIndex = _selectedIndexTemp;
    //        HabilitaEdicao = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        Utils.TraceException(ex);
    //    }
    //}

    //public void OnSalvarEdicaoCommand()
    //{
    //    try
    //    {
    //        HabilitaEdicao = false;

    //        var entity = Mapper.Map<VeiculoSeguro>(SeguroSelecionado);
    //        var repositorio = new VeiculoSeguroService();
    //        repositorio.Alterar(entity);

    //    }
    //    catch (Exception ex)
    //    {
    //        Utils.TraceException(ex);
    //    }
    //}

    //public void OnAdicionarCommand()
    //{
    //    try
    //    {
    //        foreach (var x in Seguros)
    //        {
    //            _SegurosTemp.Add(x);
    //        }

    //        _selectedIndexTemp = SelectedIndex;
    //        Seguros.Clear();

    //        _seguroTemp = new ClasseVeiculosSeguros.VeiculoSeguro();
    //        _seguroTemp.VeiculoID = VeiculoSelecionadaID;
    //        Seguros.Add(_seguroTemp);
    //        SelectedIndex = 0;
    //        HabilitaEdicao = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Utils.TraceException(ex);
    //    }

    //}

    //public void OnSalvarAdicaoCommand()
    //{
    //    try
    //    {
    //        HabilitaEdicao = false;

    //        var entity = Mapper.Map<VeiculoSeguro>(SeguroSelecionado);
    //        var repositorio = new VeiculoSeguroService();
    //        repositorio.Criar(entity);

    //        Thread CarregaColecaoAnexos_thr = new Thread(() => CarregaColecaoSeguros(SeguroSelecionado.VeiculoID));
    //        CarregaColecaoAnexos_thr.Start();

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
    //        Seguros = null;
    //        Seguros = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>(_SegurosTemp);
    //        SelectedIndex = _selectedIndexTemp;
    //        _SegurosTemp.Clear();
    //        HabilitaEdicao = false;
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
    //                var entity = Mapper.Map<VeiculoSeguro>(SeguroSelecionado);
    //                var repositorio = new VeiculoSeguroService();
    //                repositorio.Remover(entity);

    //                Seguros.Remove(SeguroSelecionado);
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
    //        popupPesquisaSeguro = new PopupPesquisaSeguro();
    //        popupPesquisaSeguro.EfetuarProcura += On_EfetuarProcura;
    //        popupPesquisaSeguro.ShowDialog();
    //    }
    //    catch (Exception ex)
    //    {
    //        Utils.TraceException(ex);
    //    }
    //}

    //public void On_EfetuarProcura(object sender, EventArgs e)
    //{
    //    object vetor = popupPesquisaSeguro.Criterio.Split((char)(20));
    //    int _veiculoID = VeiculoSelecionadaID;
    //    string _seguradora = ((string[])vetor)[0];
    //    string _numeroapolice = ((string[])vetor)[1];
    //    CarregaColecaoSeguros(_veiculoID, _seguradora, _numeroapolice);
    //    SelectedIndex = 0;
    //}

    //#endregion

    //#region Carregamento das Colecoes
    //private void CarregaColecaoSeguros(int veiculoID, string _seguradora = "", string _numeroapolice = "")
    //{
    //    try
    //    {
    //        var service = new VeiculoSeguroService();
    //        var list1 = service.Listar(veiculoID, null, null);

    //        var list2 = Mapper.Map<List<ClasseVeiculosSeguros.VeiculoSeguro>>(list1);
    //        var observer = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>();
    //        list2.ForEach(n =>
    //        {
    //            observer.Add(n);
    //        });

    //        Seguros = observer;

    //        //Hotfix auto-selecionar registro do topo da ListView
    //        var topList = observer.FirstOrDefault();
    //        SeguroSelecionado = topList;

    //    }
    //    catch (Exception ex)
    //    {
    //        Utils.TraceException(ex);
    //    }
    //}
    //#endregion


}
