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
using System.Windows.Forms;
using System.Windows.Input;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasContratosViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService _service = new EmpresaContratoService();
        private EmpresaView _empresaView;

        #region  Propriedades

        /// <summary>
        ///     Lista de municipios
        /// </summary>
        public List<Municipio> Municipios { get; private set; }

        public Estados Estado { get; set; }

        /// <summary>
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        /// <summary>
        ///     Lista de estados
        /// </summary>
        public List<Estados> Estados { get; private set; }

        /// <summary>
        ///     Lista de sattus
        /// </summary>
        public List<Status> Status { get; private set; }

        /// <summary>
        ///     Lista de tipos de cobrança
        /// </summary>
        public List<TipoCobranca> TiposCobranca { get; private set; }

        /// <summary>
        ///     Lista de tipos de acessos
        /// </summary>
        public List<TipoAcesso> ListaTipoAcessos { get; private set; }

        public ObservableCollection<EmpresaContratoView> EntityObserver { get; set; }
        public EmpresaContratoView Entity { get; set; }

        EmpresaContratoView EntidadeTMP = new EmpresaContratoView();

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        #endregion

        public EmpresasContratosViewModel()
        {
            ListarDadosAuxiliares();
            ItensDePesquisaConfigura();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            EntityObserver = new ObservableCollection<EmpresaContratoView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        #region  Metodos

        /// <summary>
        ///     Carregar dados auxiliares em memória
        /// </summary>
        private void ListarDadosAuxiliares()
        {
            //Estados
            var lst1 = _auxiliaresService.EstadoService.Listar();
            Estados = new List<Estados>();
            Estados.AddRange(lst1);
            //Status
            var lst2 = _auxiliaresService.StatusService.Listar();
            Status = new List<Status>();
            Status.AddRange(lst2);
            //Tipos Cobrança
            var lst3 = _auxiliaresService.TipoCobrancaService.Listar();
            TiposCobranca = new List<TipoCobranca>();
            TiposCobranca.AddRange(lst3);
            //Tipo de Acesso
            var lst4 = _auxiliaresService.TiposAcessoService.Listar();
            ListaTipoAcessos = new List<TipoAcesso>();
            ListaTipoAcessos.AddRange(lst4);
        }

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

        public void AtualizarDados(EmpresaView entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _empresaView = entity;
            //Obter dados
            var list1 = _service.Listar(entity.EmpresaId, null, null, null, null, null, null);
            var list2 = Mapper.Map<List<EmpresaContratoView>>(list1.OrderByDescending(n => n.EmpresaContratoId));
            EntityObserver = new ObservableCollection<EmpresaContratoView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Nome"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Todos"));
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
                if (Entity == null)
                {
                    return;
                }

                var n1 = Mapper.Map<EmpresaContrato>(Entity);
                n1.EmpresaId = _empresaView.EmpresaId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaContratoView>(n1);
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
            Entity = new EmpresaContratoView();
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

                var n1 = Mapper.Map<EmpresaContrato>(Entity);
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
                if (Entity == null)
                {
                    return;
                }

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes)
                {
                    return;
                }

                var n1 = Mapper.Map<EmpresaContrato>(Entity);
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
                var pesquisa = NomePesquisa;
                var num = PesquisarPor;


                //Por nome
                if (num.Key == 1)
                {
                    //Obet itens do observer
                    //var l1 = _service.Listar (Entity.EmpresaId,null, $"%{pesquisa}%");
                    if (string.IsNullOrWhiteSpace(pesquisa))
                    {
                        return;
                    }

                    var l1 = EntityObserver.Where(n => n.Descricao
                   .ToLower()
                   .Contains(pesquisa.ToLower())).ToList();
                    EntityObserver = new ObservableCollection<EmpresaContratoView>();
                    l1.ForEach(n => { EntityObserver.Add(n); });
                    //PopularObserver (l1);
                }

                if (num.Key == 2)
                {
                    //Obet itens do observer
                    var l1 = _service.Listar(Entity.EmpresaId);
                    PopularObserver(l1);
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PopularObserver(ICollection<EmpresaContrato> list)
        {
            try
            {
                var list2 = Mapper.Map<List<EmpresaContratoView>>(list);
                EntityObserver = new ObservableCollection<EmpresaContratoView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
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
}

//public ObservableCollection<EstadoView> Estados
//{
//    get { return _estados; }

//    set
//    {
//        if (_estados != value)
//        {
//            _estados = value;
//            OnPropertyChanged();
//        }
//    }
//}

//public ObservableCollection<ClasseMunicipios.Municipio> Municipios
//{
//    get { return _municipios; }

//    set
//    {
//        if (_municipios != value)
//        {
//            _municipios = value;
//            OnPropertyChanged();
//        }
//    }
//}

//public ObservableCollection<ClasseStatus.Status> Status
//{
//    get { return _statuss; }

//    set
//    {
//        if (_statuss != value)
//        {
//            _statuss = value;
//            OnPropertyChanged();
//        }
//    }
//}

//public ObservableCollection<ClasseTiposAcessos.TipoAcesso> TiposAcessos
//{
//    get { return _tiposAcessos; }

//    set
//    {
//        if (_tiposAcessos != value)
//        {
//            _tiposAcessos = value;
//            OnPropertyChanged();
//        }
//    }
//}

//public ObservableCollection<ClasseTiposCobrancas.TipoCobranca> TiposCobrancas
//{
//    get { return _tiposCobrancas; }

//    set
//    {
//        if (_tiposCobrancas != value)
//        {
//            _tiposCobrancas = value;
//            OnPropertyChanged();
//        }
//    }
//}

//public EmpresaContratoView ContratoSelecionado
//{
//    get { return _contratoSelecionado; }
//    set
//    {
//        _contratoSelecionado = value;
//        OnPropertyChanged("SelectedItem");
//    }
//}

//public int EmpresaSelecionadaId
//{
//    get { return _empresaSelecionadaId; }
//    set
//    {
//        _empresaSelecionadaId = value;
//        OnPropertyChanged();
//        if (EmpresaSelecionadaId != null)
//        {
//            //OnEmpresaSelecionada();
//        }
//    }
//}

//public int SelectedIndex
//{
//    get { return _selectedIndex; }
//    set
//    {
//        _selectedIndex = value;
//        OnPropertyChanged("SelectedIndex");
//    }
//}

//public bool HabilitaEdicao
//{
//    get { return _habilitaEdicao; }
//    set
//    {
//        _habilitaEdicao = value;
//        OnPropertyChanged();
//    }
//}

//public string Criterios
//{
//    get { return _criterios; }
//    set
//    {
//        _criterios = value;
//        OnPropertyChanged();
//    }
//}
//#region Data Access

//private void ObterContratos(int empresaId, string descricao, string numContrato)
//{
//    try
//    {
//        Contratos = new ObservableCollection<EmpresaContratoView>();
//        ICollection<EmpresaContrato> p1;

//        if (empresaId != 0)
//        {
//            p1 = _empresaContratosService.ListarPorEmpresa(empresaId);
//            var convert = Mapper.Map<List<EmpresaContratoView>>(p1);
//            convert.ForEach(n => { Contratos.Add(n); });
//            return;
//        }

//        if (!string.IsNullOrWhiteSpace(descricao))
//        {
//            p1 = _empresaContratosService.ListarPorDescricao(descricao);
//            var convert = Mapper.Map<List<EmpresaContratoView>>(p1);
//            convert.ForEach(n => { Contratos.Add(n); });
//            return;
//        }

//        if (!string.IsNullOrWhiteSpace(numContrato))
//        {
//            p1 = _empresaContratosService.ListarPorNumeroContrato(numContrato);
//            var convert = Mapper.Map<List<EmpresaContratoView>>(p1);
//            convert.ForEach(n => { Contratos.Add(n); });
//            return;
//        }

//        //Hotfix auto-selecionar registro do topo da ListView
//        var topList = Contratos.FirstOrDefault();
//        ContratoSelecionado = topList;

//        SelectedIndex = -1;
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//#endregion

#region Variaveis Privadas

//private ObservableCollection<EmpresaContratoView> _contratos;

//private EmpresaContratoView _contratoSelecionado;

//private EmpresaContratoView _contratoTemp = new EmpresaContratoView();

//private readonly List<EmpresaContratoView> _contratosTemp = new List<EmpresaContratoView>();

//private ObservableCollection<EstadoView> _estados;

//private ObservableCollection<ClasseMunicipios.Municipio> _municipios;

//private ObservableCollection<ClasseStatus.Status> _statuss;

//private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _tiposAcessos;

//private ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _tiposCobrancas;

//private PopupPesquisaContrato _popupPesquisaContrato;

//private int _selectedIndex;

//private int _empresaSelecionadaId;

//private bool _habilitaEdicao;

//private string _criterios = "";

//private int _selectedIndexTemp;

#endregion

#region Comandos dos Botoes

//public void OnAtualizaCommand(object idEmpresa)
//{
//    EmpresaSelecionadaId = Convert.ToInt32(idEmpresa);
//    ObterContratos(EmpresaSelecionadaId, "", "");
//}

//public void OnBuscarArquivoCommand()
//{
//    try
//    {
//        var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
//        var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
//        if (arq == null) return;
//        _contratoTemp.NomeArquivo = arq.Nome;
//        _contratoTemp.Arquivo = arq.FormatoBase64;
//        if (Contratos != null)
//            Contratos[0].NomeArquivo = arq.Nome;

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
//        var arquivoStr = ContratoSelecionado.Arquivo;
//        var nomeArquivo = ContratoSelecionado.NomeArquivo;
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
//        //_contratoTemp = ContratoSelecionado.CriaCopia(ContratoSelecionado);
//        _selectedIndexTemp = SelectedIndex;
//        HabilitaEdicao = true;
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
//        foreach (var x in Contratos)
//        {
//            _contratosTemp.Add(x);
//        }

//        _selectedIndexTemp = SelectedIndex;
//        Contratos.Clear();
//        _contratoTemp = new EmpresaContratoView { EmpresaId = EmpresaSelecionadaId };
//        Contratos.Add(_contratoTemp);
//        SelectedIndex = 0;
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
//        Contratos[_selectedIndexTemp] = _contratoTemp;
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
//        var entity = Mapper.Map<EmpresaContrato>(ContratoSelecionado);
//        _empresaContratosService.Alterar(entity);

//        _contratosTemp.Clear();
//        _contratoTemp = null;
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
//        var entity = Mapper.Map<EmpresaContrato>(ContratoSelecionado);
//        _empresaContratosService.Criar(entity);

//        _contratosTemp.Clear();
//        _contratoTemp = null;
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
//        Contratos = null;
//        Contratos = new ObservableCollection<EmpresaContratoView>(_contratosTemp);
//        SelectedIndex = _selectedIndexTemp;
//        _contratosTemp.Clear();
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
//                var entity = Mapper.Map<EmpresaContrato>(ContratoSelecionado);
//                _empresaContratosService.Remover(entity);

//                Contratos.Remove(ContratoSelecionado);
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
//        _popupPesquisaContrato = new PopupPesquisaContrato();
//        _popupPesquisaContrato.EfetuarProcura += On_EfetuarProcura;
//        _popupPesquisaContrato.ShowDialog();
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//public void On_EfetuarProcura(object sender, EventArgs e)
//{
//    object vetor = _popupPesquisaContrato.Criterio.Split((char)20);
//    var descricao = ((string[])vetor)[0];
//    var numContrato = ((string[])vetor)[1];
//    ObterContratos(0, descricao, numContrato);
//    SelectedIndex = 0;
//}

#endregion

#region Dados Auxiliares

//private void CarregaColecaoEstados()
//{
//    try
//    {
//        var convert = Mapper.Map<List<EstadoView>>(ListaEstadosFederacao);
//        Estados = new ObservableCollection<EstadoView>();
//        convert.ForEach(n => { Estados.Add(n); });
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//public void CarregaColecaoMunicipios(string uf)
//{
//    try
//    {
//        var list = ListaMunicipios.Where(n => n.UF == uf).ToList();
//        Municipios = new ObservableCollection<ClasseMunicipios.Municipio>();
//        list.ForEach(n => Municipios.Add(n));
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//private void CarregaColecaoStatus()
//{
//    try
//    {
//        Status = new ObservableCollection<ClasseStatus.Status>();
//        ListaStatus.ForEach(n => { Status.Add(n); });
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//private void CarregaColeçãoTiposAcessos()
//{
//    try
//    {
//        TiposAcessos = new ObservableCollection<ClasseTiposAcessos.TipoAcesso>();
//        ListaTipoAcessos.ForEach(n => { TiposAcessos.Add(n); });
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//private void CarregaColeçãoTiposCobrancas()
//{
//    try
//    {
//        TiposCobrancas = new ObservableCollection<ClasseTiposCobrancas.TipoCobranca>();
//        ListaTiposCobranca.ForEach(n => { TiposCobrancas.Add(n); });
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

#endregion