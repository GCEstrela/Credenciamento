using iModSCCredenciamento.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using iModSCCredenciamento.Funcoes;
using System.Windows.Forms;
using iModSCCredenciamento.Windows;
using System.Threading;
using System.Linq;
using System.Reflection;
using iModSCCredenciamento.Views;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Infra.Repositorios;
using MessageBox = System.Windows.MessageBox;
using IMOD.Application.Service;
using IMOD.CrossCutting;

//using IMOD.Application.Service;

//using IMOD.Application.Service;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradorViewModel : ViewModelBase
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        /// <summary>
        /// Lista de municipios
        /// </summary>
        public List<ClasseMunicipios.Municipio> ObterListaListaMunicipios { get; private set; }
        /// <summary>
        /// Lista de estados
        /// </summary>
        public List<ClasseEstados.Estado> ObterListaEstadosFederacao { get; private set; }
        //private IColaboradorService _colaboradorService = new ColaboradorService();
        public IMOD.Domain.Entities.Colaborador Colaborador { get; set; }
        
        #region Inicializacao
        public ColaboradorViewModel()
        {


            //CarregaUI();
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
            //CarregaColecaoColaboradores();
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

        private ObservableCollection<ClasseColaboradores.Colaborador> _Colaboradores;

        private ClasseColaboradores.Colaborador _ColaboradorSelecionado;

        private ClasseColaboradores.Colaborador _colaboradorTemp = new ClasseColaboradores.Colaborador();

        private List<ClasseColaboradores.Colaborador> _ColaboradoresTemp = new List<ClasseColaboradores.Colaborador>();

        private ObservableCollection<ClasseEstados.Estado> _Estados;

        private ObservableCollection<ClasseMunicipios.Municipio> _municipios;

        PopupPesquisaColaborador popupPesquisaColaborador;

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;

        //private bool _EditandoUserControl;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseColaboradores.Colaborador> Colaboradores
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

        public ClasseColaboradores.Colaborador ColaboradorSelecionado
        {
            get
            {

                return this._ColaboradorSelecionado;
            }
            set
            {
                this._ColaboradorSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (ColaboradorSelecionado != null)
                {
                    //BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                    //string _imgstr = Conversores.IMGtoSTR(_img);
                    //ColaboradorSelecionado.Foto = _imgstr;
                    if (!_atualizandoFoto)
                    {
                        Thread CarregaFoto_thr = new Thread(() => CarregaFoto(ColaboradorSelecionado.ColaboradorID));
                        CarregaFoto_thr.Start();
                    }

                    //CarregaFoto(ColaboradorSelecionado.ColaboradorID);
                }

            }
        }

        public int EmpresaSelecionadaID
        {
            get
            {
                return this._EmpresaSelecionadaID;
            }
            set
            {
                this._EmpresaSelecionadaID = value;
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
                return this._HabilitaEdicao;
            }
            set
            {
                this._HabilitaEdicao = value;
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
                return this._Criterios;
            }
            set
            {
                this._Criterios = value;
                base.OnPropertyChanged();
            }
        }

        public ObservableCollection<ClasseEstados.Estado> Estados
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

        public ObservableCollection<ClasseMunicipios.Municipio> Municipios
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
                return this._Waiting;
            }
            set
            {
                this._Waiting = value;
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
        public void OnBuscarArquivoCommand()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog _arquivoPDF = new System.Windows.Forms.OpenFileDialog();
                string _sql;
                string _nomecompletodoarquivo;
                string _arquivoSTR;
                _arquivoPDF.InitialDirectory = "c:\\\\";
                _arquivoPDF.Filter = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                _arquivoPDF.RestoreDirectory = true;
                _arquivoPDF.ShowDialog();
                //if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
                //{
                _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
                //_seguroTemp.NomeArquivo = _nomecompletodoarquivo;
                //_seguroTemp.Arquivo = _arquivoSTR;
                //InsereArquivoBD(Convert.ToInt32(empresaID), _nomecompletodoarquivo, _arquivoSTR);

                //AtualizaListaAnexos(_resp);

                //}
            }
            catch (Exception ex)
            {

            }
        }

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                _colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);
                Global.CpfEdicao = _colaboradorTemp.CPF;
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

            }
        }

        public void OnSalvarEdicaoCommand2()
        {
            //var colab = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
            //_colaboradorService.Alterar(colab);
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
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _colaboradorTemp = new ClasseColaboradores.Colaborador();
                ////////////////////////////////////////////////////////
                _colaboradorTemp.ColaboradorID = EmpresaSelecionadaID;  //OBS
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
                Colaboradores = new ObservableCollection<ClasseColaboradores.Colaborador>(_ColaboradoresTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresTemp.Clear();
                HabilitaEdicao = false;
                _atualizandoFoto = false;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnExcluirCommand2()
        {
            try
            {

                //var colab = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
                //_colaboradorService.Remover(colab);

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
                        //IMOD.Domain.Entities.Colaborador ColaboradorEntity = new IMOD.Domain.Entities.Colaborador();
                        //g.TranportarDados(ColaboradorSelecionado, 1, ColaboradorEntity);

                        //var repositorio = new IMOD.Infra.Repositorios.ColaboradorRepositorio();

                        var entity = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
                        var repositorio = new IMOD.Application.Service.ColaboradorService();
                        repositorio.Remover(entity);
                       
                        Colaboradores.Remove(ColaboradorSelecionado);
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }

        public void OnPesquisarCommand()
        {
            try
            {

                popupPesquisaColaborador = new PopupPesquisaColaborador();
                popupPesquisaColaborador.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaColaborador.ShowDialog();
            }
            catch (Exception ex)
            {
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

        public void OnAbrirPendencias(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupPendencias popupPendencias = new PopupPendencias(2, ((System.Windows.FrameworkElement)e.OriginalSource).Tag, ColaboradorSelecionado.ColaboradorID, ColaboradorSelecionado.Nome);
                popupPendencias.ShowDialog();
                popupPendencias = null;
                CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);


            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregarDadosComunsEmMemoria()
        {
            //Estados
            var e1 = _auxiliaresService.ListarEstadosFederacao();
            ObterListaEstadosFederacao = Mapper.Map<List<ClasseEstados.Estado>>(e1);
            //Municipios
            var list = _auxiliaresService.ListarMunicipios();
            ObterListaListaMunicipios = Mapper.Map<List<ClasseMunicipios.Municipio>>(list);
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

                var service = new IMOD.Application.Service.ColaboradorService();
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(cpf)) cpf = $"%{cpf}%";
                var list1 = service.Listar(_ColaboradorID, nome, apelido, cpf);

                var list2 = Mapper.Map<List<ClasseColaboradores.Colaborador>>(list1.OrderBy (n => n.ColaboradorId ));

                var observer = new ObservableCollection<ClasseColaboradores.Colaborador>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });
                                
                this.Colaboradores = observer;
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
            }
        }

        private void CarregaColeçãoEstados()
        {
            try
            {

                var convert = Mapper.Map<List<ClasseEstados.Estado>>(ObterListaEstadosFederacao);
                Estados = new ObservableCollection<ClasseEstados.Estado>();
                convert.ForEach(n => { Estados.Add(n); });

            }
            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
            }
        }

        public void CarregaColeçãoMunicipios(string uf )
        {

            try
            {

                var list = ObterListaListaMunicipios.Where(n => n.UF == uf).ToList();
                Municipios = new ObservableCollection<ClasseMunicipios.Municipio>();
                list.ForEach(n => Municipios.Add(n));

            }
            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
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

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    Waiting = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Waitng.gif", UriKind.Absolute));

                    Waiting.Freeze();
                }));

                string _xmlstring = BuscaFoto(_ColaboradorID);

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => { Waiting = null; }));

                XmlDocument xmldocument = new XmlDocument();

                xmldocument.LoadXml(_xmlstring);

                XmlNode node = (XmlNode)xmldocument.DocumentElement;

                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                if (arquivoNode.HasChildNodes)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        _colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);

                        _selectedIndexTemp = SelectedIndex;

                        _colaboradorTemp.Foto = arquivoNode.FirstChild.Value;

                        Colaboradores[_selectedIndexTemp] = _colaboradorTemp;

                        SelectedIndex = _selectedIndexTemp;

                    }));
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

        
        //Global g = new Global();

        internal void SalvarAdicao()
        {
            try
            {

                ColaboradorSelecionado.Pendente = true;
                ColaboradorSelecionado.Pendente21 = true;
                ColaboradorSelecionado.Pendente22 = true;
                ColaboradorSelecionado.Pendente23 = true;
                ColaboradorSelecionado.Pendente24 = true;
                ColaboradorSelecionado.Pendente25 = true;


                var entity = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorService();
                repositorio.Criar(entity);

                var id = entity.ColaboradorId;
                AtualizaPendencias(id);

                ColaboradorSelecionado.ColaboradorID = id;

                _ColaboradoresTemp.Clear();

                _ColaboradoresTemp.Add(ColaboradorSelecionado);
                Colaboradores = null;
                Colaboradores = new ObservableCollection<ClasseColaboradores.Colaborador>(_ColaboradoresTemp);
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Alteração iniciada por Mihai & Máximo (28/11/2018)
        /// TranportarDados
        /// </summary>
        internal void SalvarEdicao()
        {
            try
            {
                

                var entity = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorService();
                repositorio.Alterar(entity);

                //_ClasseColaboradoresTemp = null;

                this._ColaboradoresTemp.Clear();
                _colaboradorTemp = null;


            }
            catch (Exception ex)
            {

            }
        }

        internal void AbrePopupSalvando()
        {

            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (_PopupSalvando != null)
                {
                    _PopupSalvando.ShowDialog();
                }


            }));

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
                return false;
            }
        }

        public void ValidarAdicao(ClasseColaboradores.Colaborador entity)
        {
            //if (string.IsNullOrWhiteSpace(entity.CPF)) throw new InvalidOperationException("Informe CPF");
            //if (!entity.CPF.IsValidCpf()) throw new InvalidOperationException("CPF inválido");
            ValidarEdicao(entity);
            if (ConsultarCpf(entity.CPF)) throw new InvalidOperationException("CPF já cadastrado");


        }

        public void ValidarEdicao(ClasseColaboradores.Colaborador entity)
        {
            if (string.IsNullOrWhiteSpace(entity.CPF)) throw new InvalidOperationException("Informe CPF");
            if (!entity.CPF.IsValidCpf()) throw new InvalidOperationException("CPF inválido");
            //if (ConsultarCpf(entity.CPF)) throw new InvalidOperationException("CPF já cadastrado");


        }

        #endregion

        #region Testes

        #endregion

    }

}
