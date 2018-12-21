using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using AutoMapper;
using iModSCCredenciamento.Enums;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using Colaborador = IMOD.Domain.Entities.Colaborador;
using iModSCCredenciamento.Views.Model;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradorViewModel : ViewModelBase
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        /// <summary>
        /// Lista de municipios
        /// </summary>
        public List<MunicipioView> ObterListaListaMunicipios { get; private set; }
        /// <summary>
        /// Lista de estados
        /// </summary>
        public List<EstadoView> ObterListaEstadosFederacao { get; private set; }
        //private IColaboradorService _colaboradorService = new ColaboradorService();
        public Colaborador Colaborador { get; set; }

        #region Inicializacao
        public ColaboradorViewModel()
        {//CarregaUI();
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
        }
        private void CarregaUI()
        {
            CarregarDadosComunsEmMemoria();

            CarregaColecaoColaboradores();
            CarregaColeçãoEstados();
            //CarregaColecaoTiposAtividades();
            //CarregaColecaoAreasAcessos();
            //CarregaColecaoLayoutsCrachas();
        }
        #endregion

        #region Variaveis Privadas

        //private ObservableCollection<ClasseColaboradores.Colaborador> _Colaboradores;
        private ObservableCollection<ColaboradorView> _Colaboradores;

        //private ClasseColaboradores.Colaborador _ColaboradorSelecionado;
        private ColaboradorView _ColaboradorSelecionado;

        //private ClasseColaboradores.Colaborador _colaboradorTemp = new ClasseColaboradores.Colaborador();
        private ColaboradorView _colaboradorTemp = new ColaboradorView();

        //private List<ClasseColaboradores.Colaborador> _ColaboradoresTemp = new List<ClasseColaboradores.Colaborador>();
        private List<ColaboradorView> _ColaboradoresTemp = new List<ColaboradorView>();

        private ObservableCollection<EstadoView> _Estados;

        private ObservableCollection<MunicipioView> _municipios;

        PopupPesquisaColaborador popupPesquisaColaborador;

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        private bool _atualizandoFoto;

        private BitmapImage _Waiting;

        //private bool _EditandoUserControl;

        #endregion

        #region Contrutores
        public ObservableCollection<ColaboradorView> Colaboradores
        {
            get
            {
                return _Colaboradores;
            }

            set
            {
                if (_Colaboradores != value)
                {
                    _Colaboradores = value;
                    OnPropertyChanged();

                }
            }
        }

        public ColaboradorView ColaboradorSelecionado
        {
            get
            {

                return _ColaboradorSelecionado;
            }
            set
            {
                _ColaboradorSelecionado = value;
                //base.OnPropertyChanged("SelectedItem");
                //if (ColaboradorSelecionado != null)
                //{
                //    //BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                //    //string _imgstr = Conversores.IMGtoSTR(_img);
                //    //ColaboradorSelecionado.Foto = _imgstr;
                //    if (!_atualizandoFoto)
                //    {
                //        //Thread CarregaFoto_thr = new Thread(() => CarregaFoto(ColaboradorSelecionado.ColaboradorID));
                //        //Thread CarregaFoto_thr = new Thread(() => CarregaFoto(ColaboradorSelecionado.ColaboradorId));
                //        //CarregaFoto_thr.Start();
                //    }

                //    //CarregaFoto(ColaboradorSelecionado.ColaboradorID);
                //}

            }
        }

        public int EmpresaSelecionadaID
        {
            get
            {
                return _EmpresaSelecionadaID;
            }
            set
            {
                _EmpresaSelecionadaID = value;
                base.OnPropertyChanged();
                if (EmpresaSelecionadaID != null)
                {
                    // OnEmpresaSelecionada();
                }

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

        public bool HabilitaEdicao
        {
            get
            {
                return _HabilitaEdicao;
            }
            set
            {
                _HabilitaEdicao = value;
                base.OnPropertyChanged();
            }
        }

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


        public string Criterios
        {
            get
            {
                return _Criterios;
            }
            set
            {
                _Criterios = value;
                base.OnPropertyChanged();
            }
        }

        public ObservableCollection<EstadoView> Estados
        {
            get
            {
                return _Estados;
            }

            set
            {
                if (_Estados != value)
                {
                    _Estados = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<MunicipioView> Municipios
        {
            get
            {
                return _municipios;
            }

            set
            {
                if (_municipios != value)
                {
                    _municipios = value;
                    OnPropertyChanged();

                }
            }
        }

        public BitmapImage Waiting
        {
            get
            {
                return _Waiting;
            }
            set
            {
                _Waiting = value;
                base.OnPropertyChanged();
            }
        }

        #endregion

        #region Comandos dos Botoes
        public void OnAtualizaCommand(object colaboradorID)
        {
            //EmpresaSelecionadaID = Convert.ToInt32(empresaID);
            //Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(Convert.ToInt32(empresaID)));
            //CarregaColecaoSeguros_thr.Start();
            //CarregaColecaoSeguros(Convert.ToInt32(empresaID));
        }

        public void OnEditarCommand()
        {
            try
            {
                ////BuscaBadges();
                //_colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);
                //Global.CpfEdicao = _colaboradorTemp.CPF;
                _selectedIndexTemp = SelectedIndex;
                HabilitaEdicao = true;
            }
            catch (Exception)
            {
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                Global.CpfEdicao = "";
                Colaboradores[_selectedIndexTemp] = _colaboradorTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task OnSalvarEdicaoCommandAsync()
        {
            try
            {
                Global.CpfEdicao = "";
                _PopupSalvando = new PopupMensagem("Aguarde, salvando ...");

                Thread PopupSalvando_thr = new Thread(() => AbrePopupSalvando());

                PopupSalvando_thr.Start();

                await Task.Run(() => SalvarEdicao());

                _PopupSalvando.Close();

                _PopupSalvando = null;

            }
            catch (Exception ex)
            {
                if (_PopupSalvando != null)
                {
                    _PopupSalvando.Close();
                }
                Utils.TraceException(ex);
            }
        }

        public async Task OnSalvarAdicaoCommandAsync()
        {

            try
            {
                Global.CpfEdicao = "";
                _PopupSalvando = new PopupMensagem("Aguarde, salvando ...");

                Thread PopupSalvando_thr = new Thread(() => AbrePopupSalvando());

                PopupSalvando_thr.Start();

                await Task.Run(() => SalvarAdicao());

                _PopupSalvando.Close();

                _PopupSalvando = null;

            }
            catch (Exception ex)
            {
                if (_PopupSalvando != null)
                {
                    _PopupSalvando.Close();
                }
                Utils.TraceException(ex);
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Colaboradores)
                {
                    _ColaboradoresTemp.Add(x);
                }
                Global.CpfEdicao = "000.000.000-00";
                _selectedIndexTemp = SelectedIndex;
                Colaboradores.Clear();

                _colaboradorTemp = new ColaboradorView();
                ////////////////////////////////////////////////////////
                _colaboradorTemp.ColaboradorId = EmpresaSelecionadaID;  //OBS
                ////////////////////////////////////////////////////////
                Colaboradores.Add(_colaboradorTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
            }

        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Global.CpfEdicao = "";
                Colaboradores = null;
                Colaboradores = new ObservableCollection<ColaboradorView>(_ColaboradoresTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresTemp.Clear();
                HabilitaEdicao = false;
                _atualizandoFoto = false;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnExcluirCommand()
        {
            try
            {

                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        var entity = Mapper.Map<Colaborador>(ColaboradorSelecionado);
                        var repositorio = new ColaboradorService();
                        repositorio.Remover(entity);

                        Colaboradores.Remove(ColaboradorSelecionado);
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        public void OnPesquisarCommand()
        {
            try
            {
                popupPesquisaColaborador = new PopupPesquisaColaborador();
                popupPesquisaColaborador.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaColaborador.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaColaborador.Criterio.Split((char)(20));
            int _codigo;
            if ((((string[])vetor)[0] == null) || (((string[])vetor)[0] == ""))
            {
                _codigo = 0;
            }
            else
            {
                _codigo = Convert.ToInt32(((string[])vetor)[0]);
            }
            string _nome = ((string[])vetor)[1];
            string _apelido = ((string[])vetor)[2];
            string _cpf = ((string[])vetor)[3];
            CarregaColecaoColaboradores(_codigo, _nome, _apelido, _cpf);
            SelectedIndex = 0;
        }

        public void OnAbrirPendenciaGeral(object sender, RoutedEventArgs e)
        {
            try
            {
                //var popupPendencias = 
                //    new PopupPendencias(2, ((FrameworkElement)e.OriginalSource).Tag, ColaboradorSelecionado.ColaboradorID, ColaboradorSelecionado.Nome);
                //popupPendencias.ShowDialog();
                //popupPendencias = null;
                //CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);

                var frm = new PopupPendencias();
                frm.Inicializa(21, ColaboradorSelecionado.ColaboradorId,PendenciaTipo.Colaborador);
                frm.ShowDialog();
                CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorId);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void OnAbrirPendenciaContratos(object sender, RoutedEventArgs e)
        {
            try
            { 
                var frm = new PopupPendencias();
                frm.Inicializa(14, ColaboradorSelecionado.ColaboradorId, PendenciaTipo.Colaborador);
                frm.ShowDialog();
                CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorId);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void OnAbrirPendenciaAnexos(object sender, RoutedEventArgs e)
        {
            try
            {
                var frm = new PopupPendencias();
                frm.Inicializa(24, ColaboradorSelecionado.ColaboradorId, PendenciaTipo.Colaborador);
                frm.ShowDialog();
                //CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void OnAbrirPendenciaCredenciais(object sender, RoutedEventArgs e)
        {
            try
            {
                var frm = new PopupPendencias();
                frm.Inicializa(25, ColaboradorSelecionado.ColaboradorId, PendenciaTipo.Colaborador);
                frm.ShowDialog();
                //CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void OnAbrirPendenciaTreinamento(object sender, RoutedEventArgs e)
        {
            try
            {
                var frm = new PopupPendencias();
                frm.Inicializa(23, ColaboradorSelecionado.ColaboradorId, PendenciaTipo.Colaborador);
                frm.ShowDialog();
                //CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }




        #endregion

        #region Carregamento das Colecoes
        private void CarregarDadosComunsEmMemoria()
        {
            //Estados
            var e1 = _auxiliaresService.EstadoService.Listar();
            ObterListaEstadosFederacao = Mapper.Map<List<EstadoView>>(e1);
            //Municipios
            var list = _auxiliaresService.MunicipioService.Listar();
            ObterListaListaMunicipios = Mapper.Map<List<MunicipioView>>(list);
            ////Status
            //var e3 = _auxiliaresService.ListarStatus();
            //ObterListaStatus = Mapper.Map<List<ClasseStatus.Status>>(e3);
            ////Tipos Cobrança
            //var e4 = _auxiliaresService.ListarTiposCobranca();
            //ObterListaTiposCobranca = Mapper.Map<List<ClasseTiposCobrancas.TipoCobranca>>(e4);
            ////Tipo de Acesso
            //var e5 = _auxiliaresService.ListarTiposAcessos();
            //ObterListaTipoAcessos = Mapper.Map<List<ClasseTiposAcessos.TipoAcesso>>(e5);
            //var e5 = _auxiliaresService.ListarTiposAcessos();
            //ObterListaTipoAcessos = Mapper.Map<List<ClasseTiposAcessos.TipoAcesso>>(e5);

        }

        private void CarregaColecaoColaboradores(int? _ColaboradorID = 0, string nome = "", string apelido = "", string cpf = "", string _quantidaderegistro = "500")
        {
            try
            {

                var service = new ColaboradorService();
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(cpf)) cpf = $"%{cpf}%";
                var list1 = service.Listar(_ColaboradorID, nome, apelido, cpf);

                //var list2 = Mapper.Map<List<ClasseColaboradores.Colaborador>>(list1.OrderByDescending(n => n.ColaboradorId));
                var list2 = Mapper.Map<List<ColaboradorView>>(list1.OrderByDescending(n => n.ColaboradorId));

                //var observer = new ObservableCollection<ClasseColaboradores.Colaborador>();
                var observer = new ObservableCollection<ColaboradorView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Colaboradores = observer;

                //Hotfix auto-selecionar registro no topo da ListView
                var topList = observer.FirstOrDefault();
                ColaboradorSelecionado = topList;

                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColeçãoEstados()
        {
            try
            {

                var convert = Mapper.Map<List<EstadoView>>(ObterListaEstadosFederacao);
                Estados = new ObservableCollection<EstadoView>();
                convert.ForEach(n => { Estados.Add(n); });

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColeçãoMunicipios(string uf)
        {

            try
            {

                var list = ObterListaListaMunicipios.Where(n => n.Uf == uf).ToList();
                Municipios = new ObservableCollection<MunicipioView>();
                list.ForEach(n => Municipios.Add(n));

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaFoto(int _ColaboradorID)
        {
            try
            {
                _atualizandoFoto = true; //para que o evento de ColaboradorSelecionado não entre em looping
                ///
                //                BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                //                string _imgstr = Conversores.IMGtoSTR(_img);
                //                ColaboradorSelecionado.Foto = _imgstr;

                //                System.Windows.Application.Current.Dispatcher.Invoke(
                //(Action)(() => {
                //_colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);
                //_selectedIndexTemp = SelectedIndex;

                //_colaboradorTemp.Foto = _imgstr;
                //Colaboradores[_selectedIndexTemp] = _colaboradorTemp;

                //SelectedIndex = _selectedIndexTemp;



                //}));

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Waiting = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Waitng.gif", UriKind.Absolute));

                    Waiting.Freeze();
                });

                string _xmlstring = BuscaFoto(_ColaboradorID);

                Application.Current.Dispatcher.Invoke(() => { Waiting = null; });

                XmlDocument xmldocument = new XmlDocument();

                xmldocument.LoadXml(_xmlstring);

                XmlNode node = xmldocument.DocumentElement;

                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                if (arquivoNode.HasChildNodes)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        //_colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);

                        _selectedIndexTemp = SelectedIndex;

                        _colaboradorTemp.Foto = arquivoNode.FirstChild.Value;

                        Colaboradores[_selectedIndexTemp] = _colaboradorTemp;

                        SelectedIndex = _selectedIndexTemp;

                    });
                }
                _atualizandoFoto = false;

            }
            catch (Exception ex)
            {
                _atualizandoFoto = false;
            }
        }
        #endregion

        #region Data Access
        private string BuscaFoto(int colaboradorID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseArquivosImagens = _xmlDocument.CreateElement("ClasseArquivosImagens");
                _xmlDocument.AppendChild(_ClasseArquivosImagens);

                XmlNode _ArquivosImagens = _xmlDocument.CreateElement("ArquivosImagens");
                _ClasseArquivosImagens.AppendChild(_ArquivosImagens);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand SQCMDXML = new SqlCommand("Select * From Colaboradores Where ColaboradorID = " + colaboradorID, _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Foto"].ToString())));
                    _ArquivoImagem.AppendChild(_Arquivo);

                }
                SQDR_XML.Close();

                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CriaXmlImagem ex: " + ex);
                return null;
            }
        }
        private void AtualizaPendencias(int _ColaboradorID)
        {
            try
            {

                if (_ColaboradorID == 0)
                {
                    return;
                }

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlCmd;
                for (int i = 21; i < 28; i++)
                {
                    _sqlCmd = new SqlCommand("Insert into Pendencias (TipoPendenciaID,Descricao,DataLimite ,Impeditivo,ColaboradorID) values (" +
                                                      "@v1,@v2, @v3,@v4,@v5)", _Con);

                    _sqlCmd.Parameters.Add("@v1", SqlDbType.Int).Value = i;
                    _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = "Cadastro novo!";
                    _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = DateTime.Now;
                    _sqlCmd.Parameters.Add("@v4", SqlDbType.Bit).Value = 1;
                    _sqlCmd.Parameters.Add("@v5", SqlDbType.Int).Value = _ColaboradorID;
                    _sqlCmd.ExecuteNonQuery();
                    //Thread.Sleep(200);
                }

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void AtualizaPendencias ex: " + ex);


            }

        }
        #endregion

        #region Metodos privados


        internal void SalvarAdicao()
        {
            try
            {
               // ColaboradorSelecionado.Pendente = true;
                ColaboradorSelecionado.Pendente21 = true;
                ColaboradorSelecionado.Pendente22 = true;
                ColaboradorSelecionado.Pendente23 = true;
                ColaboradorSelecionado.Pendente24 = true;
                ColaboradorSelecionado.Pendente25 = true;


                var entity = Mapper.Map<Colaborador>(ColaboradorSelecionado);
                var repositorio = new ColaboradorService();
                repositorio.Criar(entity);

                var id = entity.ColaboradorId;
                AtualizaPendencias(id);

                ColaboradorSelecionado.ColaboradorId = id;

                _ColaboradoresTemp.Clear();

                _ColaboradoresTemp.Add(ColaboradorSelecionado);
                Colaboradores = null;
                Colaboradores = new ObservableCollection<ColaboradorView>(_ColaboradoresTemp);
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        internal void SalvarEdicao()
        {
            try
            {
                var entity = Mapper.Map<Colaborador>(ColaboradorSelecionado);
                var repositorio = new ColaboradorService();
                repositorio.Alterar(entity);


                _ColaboradoresTemp.Clear();
                _colaboradorTemp = null;


            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        internal void AbrePopupSalvando()
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_PopupSalvando != null)
                {
                    _PopupSalvando.ShowDialog();
                }


            });

        }

        public bool ConsultarCpf(string doc)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(doc)) throw new ArgumentNullException("Informe um CPF para pesquisar");
                //doc = doc.RetirarCaracteresEspeciais().Replace(" ", "");

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand cmd = new SqlCommand("Select * From Colaboradores Where cpf = '" + doc + "'", _Con);
                var reader = cmd.ExecuteReader(CommandBehavior.Default);
                if (reader.Read())
                {
                    _Con.Close();
                    return true;
                }
                _Con.Close();
                return false;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                return false;
            }
        }

        public void ValidarAdicao(ColaboradorView entity)
        {
            //if (string.IsNullOrWhiteSpace(entity.CPF)) throw new InvalidOperationException("Informe CPF");
            //if (!entity.CPF.IsValidCpf()) throw new InvalidOperationException("CPF inválido");
            ValidarEdicao(entity);
            if (ConsultarCpf(entity.Cpf)) throw new InvalidOperationException("CPF já cadastrado");


        }

        public void ValidarEdicao(ColaboradorView entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Cpf)) throw new InvalidOperationException("Informe CPF");
            if (!Utils.IsValidCpf(entity.Cpf)) throw new InvalidOperationException("CPF inválido");
            //if (ConsultarCpf(entity.CPF)) throw new InvalidOperationException("CPF já cadastrado");


        }



        #endregion

        #region Testes

        #endregion

    }

}
