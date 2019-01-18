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
    public class ColaboradorViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IColaboradorService _service = new ColaboradorService();

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
        ///     True, empresa possui pendência na aba Empresas Vinculos
        /// </summary>
        public bool PendenciaEmpresasVinculos { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Treinamento
        /// </summary>
        public bool PendenciaTreinamento { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Anexo
        /// </summary>
        public bool PendenciaAnexo { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Credencial
        /// </summary>
        public bool PendenciaCredencial { get; set; }

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

        ColaboradorView EntityTmp = new ColaboradorView();
        public ColaboradorView Entity { get; set; }
        public ObservableCollection<ColaboradorView> EntityObserver { get; set; }

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
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        #endregion

        public ColaboradorViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(true, true, true, false, false);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }

        private void ListarDadosAuxiliares()
        {
            var lst3 = _auxiliaresService.EstadoService.Listar();
            Estados = Mapper.Map<List<Estados>>(lst3);
        }

        #region  Metodos

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "CPF"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Nome"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }

        private void PopularObserver(ICollection<Colaborador> list)
        {
            try
            {
                var list2 = Mapper.Map<List<ColaboradorView>>(list.OrderByDescending(n => n.ColaboradorId));
                EntityObserver = new ObservableCollection<ColaboradorView>();
                list2.ForEach(n => { EntityObserver.Add(n); });
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void Pesquisar()
        {
            try
            {
                var pesquisa = NomePesquisa;

                var num = PesquisarPor;

                //Por nome
                if (num.Key == 2)
                {
                    var l1 = _service.Listar(null, null, $"%{pesquisa}%");
                    PopularObserver(l1);
                }
                //Por cpf
                if (num.Key == 1)
                {
                    if (string.IsNullOrWhiteSpace(pesquisa))
                    {
                        pesquisa = "";
                    }

                    var l1 = _service.Listar(null, pesquisa.RetirarCaracteresEspeciais(), null);
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

        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>
        public void AtualizarDadosPendencias()
        {
            if (Entity == null)
            {
                return;
            }

            var pendencia = _service.Pendencia.ListarPorColaborador(Entity.ColaboradorId).ToList();
            //Set valores
            PendenciaGeral = false;
            PendenciaEmpresasVinculos = false;
            PendenciaTreinamento = false;
            PendenciaAnexo = false;
            PendenciaCredencial = false;
            //Buscar pendências referente aos códigos: 21;22;23;24;25
            PendenciaGeral = pendencia.Any(n => n.CodPendencia == 21);
            PendenciaEmpresasVinculos = pendencia.Any(n => n.CodPendencia == 22);
            PendenciaTreinamento = pendencia.Any(n => n.CodPendencia == 23);
            PendenciaAnexo = pendencia.Any(n => n.CodPendencia == 24);
            PendenciaCredencial = pendencia.Any(n => n.CodPendencia == 25);
            //Indica se a empresa possue pendências
            Pendencias = PendenciaGeral || PendenciaEmpresasVinculos || PendenciaTreinamento || PendenciaAnexo || PendenciaCredencial;
        }

        #endregion

        #region Regras de Negócio

        public void ValidarCpf()
        {
            if (Entity == null)
            {
                return;
            }

            var cnpj = Entity.Cpf.RetirarCaracteresEspeciais();

            //Verificar dados antes de salvar uma criação
            if (_prepareCriarCommandAcionado)
            {
                if (_service.ExisteCpf(cnpj))
                {
                    throw new Exception("CPF já cadastrado.");
                }
            }
            //Verificar dados antes de salvar uma alteraçao
            if (_prepareAlterarCommandAcionado)
            {
                var n1 = _service.BuscarPelaChave(Entity.ColaboradorId);
                if (n1 == null)
                {
                    return;
                }
                //Comparar o CNPJ antes e o depois
                if (string.Compare(n1.Cpf.RetirarCaracteresEspeciais(),
                    cnpj, StringComparison.Ordinal) != 0)
                {
                    //verificar se há cnpj exisitente
                    if (_service.ExisteCpf(cnpj))
                    {
                        throw new Exception("CPF já cadastrado.");
                    }
                }
            }
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
            EntityTmp = Entity;
            Entity = new ColaboradorView();
            IsEnableTabItem = false;
            IsEnableLstView = false;
            _prepareCriarCommandAcionado = true;
            SelectedTabIndex = 0;
            Comportamento.PrepareCriar();
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
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

                var n1 = Mapper.Map<Colaborador>(Entity);
                Validar();
                _service.Criar(n1);
                var n2 = Mapper.Map<ColaboradorView>(n1);
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

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null)
                {
                    return;
                }

                var n1 = Mapper.Map<Colaborador>(Entity);
                Validar();
                _service.Alterar(n1);
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
            try
            {
                IsEnableTabItem = true;
                IsEnableLstView = true;
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                if (Entity.ColaboradorId == 0)
                {
                    Entity = EntityTmp;
                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

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

                var n1 = Mapper.Map<Colaborador>(Entity);
                _service.Remover(n1);
                //Retirar empresa da coleção
                EntityObserver.Remove(Entity);
                IsEnableLstView = true;
                IsEnableTabItem = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        #endregion
    }
}













//public void OnAtualizaCommand(object colaboradorID)
//{
//    //EmpresaSelecionadaID = Convert.ToInt32(empresaID);
//    //Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(Convert.ToInt32(empresaID)));
//    //CarregaColecaoSeguros_thr.Start();
//    //CarregaColecaoSeguros(Convert.ToInt32(empresaID));
//}

//public void OnEditarCommand()
//{
//    try
//    {
//        ////BuscaBadges();
//        //_colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);
//        //Global.CpfEdicao = _colaboradorTemp.CPF;
//        _selectedIndexTemp = SelectedIndex;
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
//        Global.CpfEdicao = "";
//        Colaboradores[_selectedIndexTemp] = _colaboradorTemp;
//        SelectedIndex = _selectedIndexTemp;
//        HabilitaEdicao = false;
//    }
//    catch (Exception ex)
//    {

//    }
//}

//public async Task OnSalvarEdicaoCommandAsync()
//{
//    try
//    {
//        Global.CpfEdicao = "";
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
//        Global.CpfEdicao = "";
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
//        foreach (var x in Colaboradores)
//        {
//            _ColaboradoresTemp.Add(x);
//        }
//        Global.CpfEdicao = "000.000.000-00";
//        _selectedIndexTemp = SelectedIndex;
//        Colaboradores.Clear();

//        _colaboradorTemp = new ColaboradorView();
//        ////////////////////////////////////////////////////////
//        _colaboradorTemp.ColaboradorId = EmpresaSelecionadaID;  //OBS
//        ////////////////////////////////////////////////////////
//        Colaboradores.Add(_colaboradorTemp);
//        SelectedIndex = 0;
//        HabilitaEdicao = true;
//    }
//    catch (Exception ex)
//    {
//    }

//}

//public void OnCancelarAdicaoCommand()
//{
//    try
//    {
//        Global.CpfEdicao = "";
//        Colaboradores = null;
//        Colaboradores = new ObservableCollection<ColaboradorView>(_ColaboradoresTemp);
//        SelectedIndex = _selectedIndexTemp;
//        _ColaboradoresTemp.Clear();
//        HabilitaEdicao = false;
//        _atualizandoFoto = false;
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
//                var entity = Mapper.Map<Colaborador>(ColaboradorSelecionado);
//                var repositorio = new ColaboradorService();
//                repositorio.Remover(entity);

//                Colaboradores.Remove(ColaboradorSelecionado);
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
//        popupPesquisaColaborador = new PopupPesquisaColaborador();
//        popupPesquisaColaborador.EfetuarProcura += On_EfetuarProcura;
//        popupPesquisaColaborador.ShowDialog();
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//public void On_EfetuarProcura(object sender, EventArgs e)
//{
//    object vetor = popupPesquisaColaborador.Criterio.Split((char)(20));
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
//    string _apelido = ((string[])vetor)[2];
//    string _cpf = ((string[])vetor)[3];
//    CarregaColecaoColaboradores(_codigo, _nome, _apelido, _cpf);
//    SelectedIndex = 0;
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
//        frm.Inicializa(21, ColaboradorSelecionado.ColaboradorId,PendenciaTipo.Colaborador);
//        frm.ShowDialog();
//        CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorId);

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
//        frm.Inicializa(14, ColaboradorSelecionado.ColaboradorId, PendenciaTipo.Colaborador);
//        frm.ShowDialog();
//        CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorId);

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
//        frm.Inicializa(24, ColaboradorSelecionado.ColaboradorId, PendenciaTipo.Colaborador);
//        frm.ShowDialog();
//        //CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);

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
//        frm.Inicializa(25, ColaboradorSelecionado.ColaboradorId, PendenciaTipo.Colaborador);
//        frm.ShowDialog();
//        //CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);

//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}
//public void OnAbrirPendenciaTreinamento(object sender, RoutedEventArgs e)
//{
//    try
//    {
//        var frm = new PopupPendencias();
//        frm.Inicializa(23, ColaboradorSelecionado.ColaboradorId, PendenciaTipo.Colaborador);
//        frm.ShowDialog();
//        //CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);

//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//{

//internal void SalvarEdicao()
//}
//    }
//        Utils.TraceException(ex);
//    {
//    catch (Exception ex)

//    }
//        SelectedIndex = 0;
//        Colaboradores = new ObservableCollection<ColaboradorView>(_ColaboradoresTemp);
//        Colaboradores = null;

//        _ColaboradoresTemp.Add(ColaboradorSelecionado);

//        _ColaboradoresTemp.Clear();

//        ColaboradorSelecionado.ColaboradorId = id;
//        AtualizaPendencias(id);

//        var id = entity.ColaboradorId;
//        repositorio.Criar(entity);
//        var repositorio = new ColaboradorService();

//        var entity = Mapper.Map<Colaborador>(ColaboradorSelecionado);
//        ColaboradorSelecionado.Pendente25 = true;
//        ColaboradorSelecionado.Pendente24 = true;
//        ColaboradorSelecionado.Pendente23 = true;

//        ColaboradorSelecionado.Pendente22 = true;

//        ColaboradorSelecionado.Pendente21 = true;
//       // ColaboradorSelecionado.Pendente = true;
//    {
//    try
//{

#region Variaveis Privadas

////private ObservableCollection<ClasseColaboradores.Colaborador> _Colaboradores;
//private ObservableCollection<ColaboradorView> _Colaboradores;

////private ClasseColaboradores.Colaborador _ColaboradorSelecionado;
//private ColaboradorView _ColaboradorSelecionado;

////private ClasseColaboradores.Colaborador _colaboradorTemp = new ClasseColaboradores.Colaborador();
//private ColaboradorView _colaboradorTemp = new ColaboradorView();

////private List<ClasseColaboradores.Colaborador> _ColaboradoresTemp = new List<ClasseColaboradores.Colaborador>();
//private List<ColaboradorView> _ColaboradoresTemp = new List<ColaboradorView>();

//private ObservableCollection<EstadoView> _Estados;

//private ObservableCollection<MunicipioView> _municipios;

//PopupPesquisaColaborador popupPesquisaColaborador;

//PopupMensagem _PopupSalvando;

//private int _selectedIndex;

//private int _EmpresaSelecionadaID;

//private bool _HabilitaEdicao;

//private string _Criterios = "";

//private int _selectedIndexTemp;

//private bool _atualizandoFoto;

//private BitmapImage _Waiting;

//private bool _EditandoUserControl;

#endregion

#region Contrutores

//public ObservableCollection<ColaboradorView> Colaboradores
//{
//    get
//    {
//        return _Colaboradores;
//    }

//    set
//    {
//        if (_Colaboradores != value)
//        {
//            _Colaboradores = value;
//            OnPropertyChanged();

//        }
//    }
//}

//public ColaboradorView ColaboradorSelecionado
//{
//    get
//    {

//        return _ColaboradorSelecionado;
//    }
//    set
//    {
//        _ColaboradorSelecionado = value;
//        //base.OnPropertyChanged("SelectedItem");
//        //if (ColaboradorSelecionado != null)
//        //{
//        //    //BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
//        //    //string _imgstr = Conversores.IMGtoSTR(_img);
//        //    //ColaboradorSelecionado.Foto = _imgstr;
//        //    if (!_atualizandoFoto)
//        //    {
//        //        //Thread CarregaFoto_thr = new Thread(() => CarregaFoto(ColaboradorSelecionado.ColaboradorID));
//        //        //Thread CarregaFoto_thr = new Thread(() => CarregaFoto(ColaboradorSelecionado.ColaboradorId));
//        //        //CarregaFoto_thr.Start();
//        //    }

//        //    //CarregaFoto(ColaboradorSelecionado.ColaboradorID);
//        //}

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

//public bool EditandoUserControl
//{
//    get
//    {
//        return _EditandoUserControl;
//    }
//    set
//    {
//        SetProperty(ref _EditandoUserControl, value);
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

//public ObservableCollection<MunicipioView> Municipios
//{
//    get
//    {
//        return _municipios;
//    }

//    set
//    {
//        if (_municipios != value)
//        {
//            _municipios = value;
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

//internal void SalvarAdicao()

#region Carregamento das Colecoes

//private void CarregarDadosComunsEmMemoria()
//{
//    //Estados
//    var e1 = _auxiliaresService.EstadoService.Listar();
//    ObterListaEstadosFederacao = Mapper.Map<List<EstadoView>>(e1);
//    //Municipios
//    var list = _auxiliaresService.MunicipioService.Listar();
//    ObterListaListaMunicipios = Mapper.Map<List<MunicipioView>>(list);
//    ////Status
//    //var e3 = _auxiliaresService.ListarStatus();
//    //ObterListaStatus = Mapper.Map<List<ClasseStatus.Status>>(e3);
//    ////Tipos Cobrança
//    //var e4 = _auxiliaresService.ListarTiposCobranca();
//    //ObterListaTiposCobranca = Mapper.Map<List<ClasseTiposCobrancas.TipoCobranca>>(e4);
//    ////Tipo de Acesso
//    //var e5 = _auxiliaresService.ListarTiposAcessos();
//    //ObterListaTipoAcessos = Mapper.Map<List<ClasseTiposAcessos.TipoAcesso>>(e5);
//    //var e5 = _auxiliaresService.ListarTiposAcessos();
//    //ObterListaTipoAcessos = Mapper.Map<List<ClasseTiposAcessos.TipoAcesso>>(e5);

//}

//private void CarregaColecaoColaboradores(int? _ColaboradorID = 0, string nome = "", string apelido = "", string cpf = "", string _quantidaderegistro = "500")
//{
//    try
//    {

//        var service = new ColaboradorService();
//        if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
//        if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
//        if (!string.IsNullOrWhiteSpace(cpf)) cpf = $"%{cpf}%";
//        var list1 = service.Listar(_ColaboradorID, nome, apelido, cpf);

//        //var list2 = Mapper.Map<List<ClasseColaboradores.Colaborador>>(list1.OrderByDescending(n => n.ColaboradorId));
//        var list2 = Mapper.Map<List<ColaboradorView>>(list1.OrderByDescending(n => n.ColaboradorId));

//        //var observer = new ObservableCollection<ClasseColaboradores.Colaborador>();
//        var observer = new ObservableCollection<ColaboradorView>();
//        list2.ForEach(n =>
//        {
//            observer.Add(n);
//        });

//        Colaboradores = observer;

//        //Hotfix auto-selecionar registro no topo da ListView
//        var topList = observer.FirstOrDefault();
//        ColaboradorSelecionado = topList;

//        SelectedIndex = 0;
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

//        var convert = Mapper.Map<List<EstadoView>>(ObterListaEstadosFederacao);
//        Estados = new ObservableCollection<EstadoView>();
//        convert.ForEach(n => { Estados.Add(n); });

//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//public void CarregaColeçãoMunicipios(string uf)
//{

//    try
//    {

//        var list = ObterListaListaMunicipios.Where(n => n.Uf == uf).ToList();
//        Municipios = new ObservableCollection<MunicipioView>();
//        list.ForEach(n => Municipios.Add(n));

//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//private void CarregaFoto(int _ColaboradorID)
//{
//    try
//    {
//        _atualizandoFoto = true; //para que o evento de ColaboradorSelecionado não entre em looping
//        ///
//        //                BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
//        //                string _imgstr = Conversores.IMGtoSTR(_img);
//        //                ColaboradorSelecionado.Foto = _imgstr;

//        //                System.Windows.Application.Current.Dispatcher.Invoke(
//        //(Action)(() => {
//        //_colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);
//        //_selectedIndexTemp = SelectedIndex;

//        //_colaboradorTemp.Foto = _imgstr;
//        //Colaboradores[_selectedIndexTemp] = _colaboradorTemp;

//        //SelectedIndex = _selectedIndexTemp;

//        //}));

//        Application.Current.Dispatcher.Invoke(() =>
//        {
//            Waiting = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Waitng.gif", UriKind.Absolute));

//            Waiting.Freeze();
//        });

//        string _xmlstring = BuscaFoto(_ColaboradorID);

//        Application.Current.Dispatcher.Invoke(() => { Waiting = null; });

//        XmlDocument xmldocument = new XmlDocument();

//        xmldocument.LoadXml(_xmlstring);

//        XmlNode node = xmldocument.DocumentElement;

//        XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

//        if (arquivoNode.HasChildNodes)
//        {
//            Application.Current.Dispatcher.Invoke(() =>
//            {
//                //_colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);

//                _selectedIndexTemp = SelectedIndex;

//                _colaboradorTemp.Foto = arquivoNode.FirstChild.Value;

//                Colaboradores[_selectedIndexTemp] = _colaboradorTemp;

//                SelectedIndex = _selectedIndexTemp;

//            });
//        }
//        _atualizandoFoto = false;

//    }
//    catch (Exception ex)
//    {
//        _atualizandoFoto = false;
//    }
//}

#endregion

#region Data Access

//private string BuscaFoto(int colaboradorID)
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

//        SqlCommand SQCMDXML = new SqlCommand("Select * From Colaboradores Where ColaboradorID = " + colaboradorID, _Con);
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
//private void AtualizaPendencias(int _ColaboradorID)
//{
//    try
//    {

//        if (_ColaboradorID == 0)
//        {
//            return;
//        }

//        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
//        SqlCommand _sqlCmd;
//        for (int i = 21; i < 28; i++)
//        {
//            _sqlCmd = new SqlCommand("Insert into Pendencias (TipoPendenciaID,Descricao,DataLimite ,Impeditivo,ColaboradorID) values (" +
//                                              "@v1,@v2, @v3,@v4,@v5)", _Con);

//            _sqlCmd.Parameters.Add("@v1", SqlDbType.Int).Value = i;
//            _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = "Cadastro novo!";
//            _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = DateTime.Now;
//            _sqlCmd.Parameters.Add("@v4", SqlDbType.Bit).Value = 1;
//            _sqlCmd.Parameters.Add("@v5", SqlDbType.Int).Value = _ColaboradorID;
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

//    try
//    {
//        var entity = Mapper.Map<Colaborador>(ColaboradorSelecionado);
//        var repositorio = new ColaboradorService();
//        repositorio.Alterar(entity);

//        _ColaboradoresTemp.Clear();
//        _colaboradorTemp = null;

//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//    }
//}

//internal void AbrePopupSalvando()
//{

//    Application.Current.Dispatcher.Invoke(() =>
//    {
//        if (_PopupSalvando != null)
//        {
//            _PopupSalvando.ShowDialog();
//        }

//    });

//}

//public bool ConsultarCpf(string doc)
//{

//    try
//    {
//        if (string.IsNullOrWhiteSpace(doc)) throw new ArgumentNullException("Informe um CPF para pesquisar");
//        //doc = doc.RetirarCaracteresEspeciais().Replace(" ", "");

//        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
//        SqlCommand cmd = new SqlCommand("Select * From Colaboradores Where cpf = '" + doc + "'", _Con);
//        var reader = cmd.ExecuteReader(CommandBehavior.Default);
//        if (reader.Read())
//        {
//            _Con.Close();
//            return true;
//        }
//        _Con.Close();
//        return false;
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//        return false;
//    }
//}

//public void ValidarAdicao(ColaboradorView entity)
//{
//    //if (string.IsNullOrWhiteSpace(entity.CPF)) throw new InvalidOperationException("Informe CPF");
//    //if (!entity.CPF.IsValidCpf()) throw new InvalidOperationException("CPF inválido");
//    ValidarEdicao(entity);
//    if (ConsultarCpf(entity.Cpf)) throw new InvalidOperationException("CPF já cadastrado");

//}

//public void ValidarEdicao(ColaboradorView entity)
//{
//    if (string.IsNullOrWhiteSpace(entity.Cpf)) throw new InvalidOperationException("Informe CPF");
//    if (!Utils.IsValidCpf(entity.Cpf)) throw new InvalidOperationException("CPF inválido");
//    //if (ConsultarCpf(entity.CPF)) throw new InvalidOperationException("CPF já cadastrado");

//}