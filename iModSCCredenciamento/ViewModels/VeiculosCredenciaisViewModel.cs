using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using iModSCCredenciamento.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;


namespace iModSCCredenciamento.ViewModels
{
    public class VeiculosCredenciaisViewModel : ViewModelBase
    {
        #region Inicializacao
        public VeiculosCredenciaisViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();

        }

        private void CarregaUI()
        {
            //CarregaColecaoEmpresas();
            CarregaColeçãoFormatosCredenciais();
            CarregaColecaoTiposCredenciais();
            CarregaColecaoCredenciaisStatus();
            CarregaColecaoTecnologiasCredenciais();

        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculosCredenciais;

        private ObservableCollection<ClasseVinculos.Vinculo> _Vinculos;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;

        private ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> _FormatosCredenciais;

        private ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> _EmpresasLayoutsCrachas;

        private ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> _Contratos;

        private ClasseVeiculosCredenciais.VeiculoCredencial _VeiculoCredencialSelecionado;

        private ClasseVeiculosCredenciais.VeiculoCredencial _VeiculoCredencialTemp = new ClasseVeiculosCredenciais.VeiculoCredencial();

        private List<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculosCredenciaisTemp = new List<ClasseVeiculosCredenciais.VeiculoCredencial>();

        PopupPesquisaVeiculosCredenciais popupPesquisaVeiculosCredenciais;

        private int _selectedIndex;

        private int _VeiculoSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        private string _Validade;

        private ObservableCollection<ClasseTiposCredenciais.TipoCredencial> _TiposCredenciais;

        private ObservableCollection<ClasseCredenciaisStatus.CredencialStatus> _CredenciaisStatus;

        private ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial> _TecnologiasCredenciais;

        private ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa> _VeiculosEmpresas;


        #endregion

        #region Contrutores
        public ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa> VeiculosEmpresas
        {
            get
            {
                return _VeiculosEmpresas;
            }

            set
            {
                if (_VeiculosEmpresas != value)
                {
                    _VeiculosEmpresas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseTiposCredenciais.TipoCredencial> TiposCredenciais
        {
            get
            {
                return _TiposCredenciais;
            }

            set
            {
                if (_TiposCredenciais != value)
                {
                    _TiposCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseCredenciaisStatus.CredencialStatus> CredenciaisStatus
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

        public ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial> TecnologiasCredenciais
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

        public ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> VeiculosCredenciais
        {
            get
            {
                return _VeiculosCredenciais;
            }

            set
            {
                if (_VeiculosCredenciais != value)
                {
                    _VeiculosCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseVinculos.Vinculo> Vinculos
        {
            get
            {
                return _Vinculos;
            }

            set
            {
                if (_Vinculos != value)
                {
                    _Vinculos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> EmpresasLayoutsCrachas
        {
            get
            {
                return _EmpresasLayoutsCrachas;
            }

            set
            {
                if (_EmpresasLayoutsCrachas != value)
                {
                    _EmpresasLayoutsCrachas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseVeiculosCredenciais.VeiculoCredencial VeiculoCredencialSelecionado
        {
            get
            {
                return this._VeiculoCredencialSelecionado;
            }
            set
            {
                this._VeiculoCredencialSelecionado = value;
                //base.OnPropertyChanged("SelectedItem");
                base.OnPropertyChanged();
                if (VeiculoCredencialSelecionado != null)
                {
                    //CarregaColecaoVeiculosEmpresas(VeiculoCredencialSelecionado.VeiculoID);
                    //CarregaColeçãoEmpresasLayoutsCrachas(VeiculoCredencialSelecionado.EmpresaID);
                }

            }
        }

        public ObservableCollection<ClasseEmpresas.Empresa> Empresas

        {
            get
            {
                return _Empresas;
            }

            set
            {
                if (_Empresas != value)
                {
                    _Empresas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> FormatosCredenciais

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

        public ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> Contratos
        {
            get
            {
                return _Contratos;
            }

            set
            {
                if (_Contratos != value)
                {
                    _Contratos = value;
                    OnPropertyChanged();

                }
            }
        }

        public int VeiculoSelecionadaID
        {
            get
            {
                return this._VeiculoSelecionadaID;

            }
            set
            {
                this._VeiculoSelecionadaID = value;
                base.OnPropertyChanged();
                if (VeiculoSelecionadaID != null)
                {
                    //OnEmpresaSelecionada();
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

        public string Validade
        {
            get
            {
                return this._Validade;
            }
            set
            {
                this._Validade = value;
                base.OnPropertyChanged();
            }
        }

        //public string ComboEmpresaSelecionado
        //{
        //    get
        //    {
        //        return this._ComboEmpresaSelecionado;
        //    }
        //    set
        //    {
        //        this._ComboEmpresaSelecionado = value;
        //        base.OnPropertyChanged();
        //    }
        //}
        #endregion

        #region Comandos dos Botoes

        public void OnAtualizaCommand(object _VeiculoID)
        {
            try
            {
                VeiculoSelecionadaID = Convert.ToInt32(_VeiculoID);
                Thread CarregaColecaoVeiculosCredenciais_thr = new Thread(() =>
                {
                    CarregaColecaoVeiculosEmpresas(VeiculoSelecionadaID);
                    // CarregaColeçãoEmpresasLayoutsCrachas(VeiculoCredencialSelecionado.EmpresaID);
                    CarregaColecaoVeiculosCredenciais(Convert.ToInt32(_VeiculoID));
                });
                CarregaColecaoVeiculosCredenciais_thr.Start();
                //CarregaColecaoVeiculosCredenciais(Convert.ToInt32(_VeiculoID));

            }
            catch (Exception ex)
            {

            }

        }


        public void OnEditarCommand()
        {
            try
            {

                _VeiculoCredencialTemp = VeiculoCredencialSelecionado.CriaCopia(VeiculoCredencialSelecionado);
                _selectedIndexTemp = SelectedIndex;
                //OnAtualizaCommand(VeiculoSelecionadaID);
                //CarregaColecaoContratos(_VeiculoCredencialTemp.EmpresaID);

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
                VeiculosCredenciais[_selectedIndexTemp] = _VeiculoCredencialTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {

            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {

                if (VeiculoCredencialSelecionado.CredencialStatusID == 1)
                {
                    //CarregaColeçãoVinculos(VeiculoCredencialSelecionado.VeiculoCredencialID);

                    bool _resposta = SCManager.VincularVeiculo(VeiculoCredencialSelecionado);

                    //VeiculoCredencialSelecionado.CredencialGuid = Vinculos[0].CredencialGuid;

                    //VeiculoCredencialSelecionado.CardHolderGuid = Vinculos[0].CardHolderGuid;
                }


                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosCredenciais));

                ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculoCredencialTemp = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
                ClasseVeiculosCredenciais _ClasseVeiculoerEmpresasTemp = new ClasseVeiculosCredenciais();
                _VeiculoCredencialTemp.Add(VeiculoCredencialSelecionado);
                _ClasseVeiculoerEmpresasTemp.VeiculosCredenciais = _VeiculoCredencialTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseVeiculoerEmpresasTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereVeiculoCredencialBD(xmlString);


                Thread CarregaColecaoVeiculosCredenciais_thr = new Thread(() =>
                {
                    CarregaColecaoVeiculosCredenciais(VeiculoSelecionadaID);
                });
                CarregaColecaoVeiculosCredenciais_thr.Start();

                //_ClasseEmpresasSegurosTemp = null;

                //_SegurosTemp.Clear();
                //_seguroTemp = null;


            }
            catch (Exception ex)
            {

            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                if (VeiculosCredenciais != null)
                {
                    foreach (var x in VeiculosCredenciais)
                    {
                        _VeiculosCredenciaisTemp.Add(x);
                    }

                    _selectedIndexTemp = SelectedIndex;
                    VeiculosCredenciais.Clear();
                }

                _VeiculoCredencialTemp = new ClasseVeiculosCredenciais.VeiculoCredencial();
                _VeiculoCredencialTemp.VeiculoID = VeiculoSelecionadaID;
                VeiculosCredenciais.Add(_VeiculoCredencialTemp);
                //OnAtualizaCommand(VeiculoSelecionadaID);
                CarregaColecaoVeiculosEmpresas(VeiculoSelecionadaID);
                SelectedIndex = 0;
                VeiculoCredencialSelecionado.Emissao = DateTime.Now;
                HabilitaEdicao = true;
                // SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }

        }
        //public void OnSelecionaEmpresaCommand(int _empresaID)
        //{
        //    CarregaColecaoContratos(_empresaID);
        //}
        public void OnSalvarAdicaoCommand()
        {
            try
            {
                if (VeiculoCredencialSelecionado.CredencialStatusID == 1)
                {
                    //CarregaColeçãoVinculos(VeiculoCredencialSelecionado.VeiculoCredencialID);

                    bool _resposta = SCManager.VincularVeiculo(VeiculoCredencialSelecionado);

                    //VeiculoCredencialSelecionado.CredencialGuid = Vinculos[0].CredencialGuid;

                    //VeiculoCredencialSelecionado.CardHolderGuid = Vinculos[0].CardHolderGuid;
                }

                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosCredenciais));

                ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculoCredencialPro = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
                ClasseVeiculosCredenciais _ClasseVeiculoerEmpresasPro = new ClasseVeiculosCredenciais();
                _VeiculoCredencialPro.Add(VeiculoCredencialSelecionado);
                _ClasseVeiculoerEmpresasPro.VeiculosCredenciais = _VeiculoCredencialPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseVeiculoerEmpresasPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereVeiculoCredencialBD(xmlString);





                Thread CarregaColecaoVeiculosCredenciais_thr = new Thread(() =>
                {
                    CarregaColecaoVeiculosCredenciais(VeiculoSelecionadaID);
                });
                CarregaColecaoVeiculosCredenciais_thr.Start();




                ////Thread CarregaColecaoVeiculosCredenciais_thr = new Thread(() => CarregaColecaoVeiculosCredenciais(VeiculoCredencialSelecionado.VeiculoID));
                ////CarregaColecaoVeiculosCredenciais_thr.Start();
                //_VeiculosCredenciaisTemp.Add(VeiculoCredencialSelecionado);
                //VeiculosCredenciais = null;
                //VeiculosCredenciais = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>(_VeiculosCredenciaisTemp);
                //SelectedIndex = _selectedIndexTemp;
                //_VeiculosCredenciaisTemp.Clear();



            }
            catch (Exception ex)
            {
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                VeiculosCredenciais = null;
                VeiculosCredenciais = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>(_VeiculosCredenciaisTemp);
                SelectedIndex = _selectedIndexTemp;
                _VeiculosCredenciaisTemp.Clear();
                HabilitaEdicao = false;
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
                        if (SCManager.ExcluirCredencial(VeiculoCredencialSelecionado.CredencialGuid))
                        {
                            ExcluiVeiculoCredencialBD(VeiculoCredencialSelecionado.VeiculoCredencialID);
                            VeiculosCredenciais.Remove(VeiculoCredencialSelecionado);
                        }
                        else
                        {
                            Global.PopupBox("Não foi possível excluir esta credencial. Verifique no Gerenciador de Credenciais do Controle de Acesso.", 4);
                        }

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
                popupPesquisaVeiculosCredenciais = new PopupPesquisaVeiculosCredenciais();
                popupPesquisaVeiculosCredenciais.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaVeiculosCredenciais.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            try
            {
                object vetor = popupPesquisaVeiculosCredenciais.Criterio.Split((char)(20));
                int _veiculoID = VeiculoSelecionadaID;
                string _empresaNome = ((string[])vetor)[0];
                int _status = Convert.ToInt32(((string[])vetor)[1]);
                string _validade = ((string[])vetor)[2];

                CarregaColecaoVeiculosCredenciais(_veiculoID, _empresaNome, _status, _validade);
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }

        }

        public void OnBuscarDataCommand()
        {
            try
            {
                DateTime _datavalidadeCredencial = validadeCursoContrato(VeiculoCredencialSelecionado.VeiculoID);
                VeiculosCredenciais[SelectedIndex].Validade = _datavalidadeCredencial;
                Validade = String.Format("{0:dd/MM/yyyy}", _datavalidadeCredencial);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoVeiculosCredenciais(int _veiculoID, string _empresaNome = "", int _status = 0, string _validade = "")
        {
            try
            {
                string _xml = RequisitaVeiculosCredenciais(_veiculoID, _empresaNome, _status, _validade);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVeiculosCredenciais));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseVeiculosCredenciais classeVeiculosCredenciais = new ClasseVeiculosCredenciais();
                classeVeiculosCredenciais = (ClasseVeiculosCredenciais)deserializer.Deserialize(reader);
                VeiculosCredenciais = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
                VeiculosCredenciais = classeVeiculosCredenciais.VeiculosCredenciais;
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        //private void CarregaColecaoEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", string _quantidaderegistro = "500")
        //{
        //    try
        //    {
        //        string _xml = RequisitaEmpresas(_empresaID, _nome, _apelido, _cNPJ);

        //        XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresas));

        //        XmlDocument xmldocument = new XmlDocument();
        //        xmldocument.LoadXml(_xml);

        //        TextReader reader = new StringReader(_xml);
        //        ClasseEmpresas classeEmpresas = new ClasseEmpresas();
        //        classeEmpresas = (ClasseEmpresas)deserializer.Deserialize(reader);
        //        Empresas = new ObservableCollection<ClasseEmpresas.Empresa>();
        //        Empresas = classeEmpresas.Empresas;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
        //    }
        //}
        private void CarregaColecaoEmpresas(int _empresaID = 0, string _nome = "", string _apelido = "", string _cNPJ = "", string _quantidaderegistro = "500")
        {
            try
            {
                string _xml = RequisitaEmpresas(_empresaID, _nome, _apelido, _cNPJ);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresas classeEmpresas = new ClasseEmpresas();
                classeEmpresas = (ClasseEmpresas)deserializer.Deserialize(reader);
                Empresas = new ObservableCollection<ClasseEmpresas.Empresa>();
                Empresas = classeEmpresas.Empresas;
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoVeiculosEmpresas(int _veiculoID)
        {
            try
            {
                string _xml = RequisitaVeiculosEmpresas(_veiculoID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVeiculosEmpresas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseVeiculosEmpresas classeVeiculosEmpresas = new ClasseVeiculosEmpresas();
                classeVeiculosEmpresas = (ClasseVeiculosEmpresas)deserializer.Deserialize(reader);
                VeiculosEmpresas = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>();
                VeiculosEmpresas = classeVeiculosEmpresas.VeiculosEmpresas;
                SelectedIndex = -1;
                //CarregaColeçãoEmpresasLayoutsCrachas(empresaID);
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }


        public void CarregaColeçãoEmpresasLayoutsCrachas(int _veiculoEmpresaID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaEmpresasLayoutsCrachas(_veiculoEmpresaID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasLayoutsCrachas));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseEmpresasLayoutsCrachas classeEmpresasLayoutsCrachas = new ClasseEmpresasLayoutsCrachas();
                classeEmpresasLayoutsCrachas = (ClasseEmpresasLayoutsCrachas)deserializer.Deserialize(reader);
                EmpresasLayoutsCrachas = new ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>();
                EmpresasLayoutsCrachas = classeEmpresasLayoutsCrachas.EmpresasLayoutsCrachas;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        public void CarregaColeçãoFormatosCredenciais()
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaFormatosCredenciais();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseFormatosCredenciais));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseFormatosCredenciais classeFormatosCredenciais = new ClasseFormatosCredenciais();
                classeFormatosCredenciais = (ClasseFormatosCredenciais)deserializer.Deserialize(reader);
                FormatosCredenciais = new ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial>();
                FormatosCredenciais = classeFormatosCredenciais.FormatosCredenciais;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        public void CarregaColeçãoVinculos(int _VeiculoCredencialID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaVinculos(_VeiculoCredencialID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVinculos));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseVinculos classeVinculos = new ClasseVinculos();
                classeVinculos = (ClasseVinculos)deserializer.Deserialize(reader);
                Vinculos = new ObservableCollection<ClasseVinculos.Vinculo>();
                Vinculos = classeVinculos.Vinculos;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        public void CarregaColecaoContratos(int empresaID = 0)
        {

            try
            {

                string _xml = RequisitaContratos(empresaID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasContratos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresasContratos classeContratosEmpresa = new ClasseEmpresasContratos();
                classeContratosEmpresa = (ClasseEmpresasContratos)deserializer.Deserialize(reader);
                Contratos = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>();
                Contratos = classeContratosEmpresa.EmpresasContratos;
                SelectedIndex = 0;
                CarregaColeçãoEmpresasLayoutsCrachas(empresaID);
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access

        private void CarregaColecaoTiposCredenciais()
        {
            try
            {

                string _xml = RequisitaTiposCredenciais();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposCredenciais));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseTiposCredenciais classeTiposCredenciais = new ClasseTiposCredenciais();
                classeTiposCredenciais = (ClasseTiposCredenciais)deserializer.Deserialize(reader);
                TiposCredenciais = new ObservableCollection<ClasseTiposCredenciais.TipoCredencial>();
                TiposCredenciais = classeTiposCredenciais.TiposCredenciais;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoTiposCredenciais ex: " + ex);
            }

        }

        private void CarregaColecaoTecnologiasCredenciais()
        {
            try
            {

                string _xml = RequisitaTecnologiasCredenciais();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTecnologiasCredenciais));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseTecnologiasCredenciais classeTecnologiasCredenciais = new ClasseTecnologiasCredenciais();
                classeTecnologiasCredenciais = (ClasseTecnologiasCredenciais)deserializer.Deserialize(reader);
                TecnologiasCredenciais = new ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial>();
                TecnologiasCredenciais = classeTecnologiasCredenciais.TecnologiasCredenciais;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoTecnologiasCredenciais ex: " + ex);
            }

            //try
            //{

            //    TecnologiasCredenciais = new ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial>();
            //    TecnologiasCredenciais.Add(new ClasseTecnologiasCredenciais.TecnologiaCredencial() { TecnologiaCredencialID = 1, Descricao = "Permanente" });
            //    TecnologiasCredenciais.Add(new ClasseTecnologiasCredenciais.TecnologiaCredencial() { TecnologiaCredencialID = 2, Descricao = "Temporária" });

            //}
            //catch (Exception ex)
            //{
            //    //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            //}

        }

        private void CarregaColecaoCredenciaisStatus()
        {
            try
            {

                string _xml = RequisitaCredenciaisStatus();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseCredenciaisStatus));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseCredenciaisStatus classeCredenciaisStatus = new ClasseCredenciaisStatus();
                classeCredenciaisStatus = (ClasseCredenciaisStatus)deserializer.Deserialize(reader);
                CredenciaisStatus = new ObservableCollection<ClasseCredenciaisStatus.CredencialStatus>();
                CredenciaisStatus = classeCredenciaisStatus.CredenciaisStatus;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoCredenciaisStatus ex: " + ex);
            }

        }

        //private void CarregaColecaoCredenciaisStatus()
        //{
        //    try
        //    {

        //        CredenciaisStatus = new ObservableCollection<ClasseCredenciaisStatus.CredencialStatus>();
        //        CredenciaisStatus.Add(new ClasseCredenciaisStatus.CredencialStatus() { CredencialStatusID = 1, Descricao = "Ativa" });
        //        CredenciaisStatus.Add(new ClasseCredenciaisStatus.CredencialStatus() { CredencialStatusID = 2, Descricao = "Inativa" });
        //        CredenciaisStatus.Add(new ClasseCredenciaisStatus.CredencialStatus() { CredencialStatusID = 3, Descricao = "Extraviada" });
        //        CredenciaisStatus.Add(new ClasseCredenciaisStatus.CredencialStatus() { CredencialStatusID = 4, Descricao = "Roubada" });
        //        CredenciaisStatus.Add(new ClasseCredenciaisStatus.CredencialStatus() { CredencialStatusID = 5, Descricao = "Perdida" });
        //        CredenciaisStatus.Add(new ClasseCredenciaisStatus.CredencialStatus() { CredencialStatusID = 6, Descricao = "Devolvida" });


        //    }
        //    catch (Exception ex)
        //    {
        //        //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
        //    }

        //}

        //private void CarregaColecaoTecnologiasCredenciais()
        //{
        //    try
        //    {

        //        TecnologiasCredenciais = new ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial>();
        //        TecnologiasCredenciais.Add(new ClasseTecnologiasCredenciais.TecnologiaCredencial() { TecnologiaCredencialID = 1, Descricao = "N/D" });
        //        TecnologiasCredenciais.Add(new ClasseTecnologiasCredenciais.TecnologiaCredencial() { TecnologiaCredencialID = 2, Descricao = "Cartão de Proximidade" });
        //        TecnologiasCredenciais.Add(new ClasseTecnologiasCredenciais.TecnologiaCredencial() { TecnologiaCredencialID = 3, Descricao = "Senha" });
        //        TecnologiasCredenciais.Add(new ClasseTecnologiasCredenciais.TecnologiaCredencial() { TecnologiaCredencialID = 4, Descricao = "Biometria" });

        //    }
        //    catch (Exception ex)
        //    {
        //        //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
        //    }

        //}

        //private string RequisitaVeiculosCredenciais(int _veiculoID, string _empresaNome = "", int _status = 0, string _validade = "")//Possibilidade de criar a pesquisa por Matriculatambem
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseVeiculosCredenciais = _xmlDocument.CreateElement("ClasseVeiculosCredenciais");
        //        _xmlDocument.AppendChild(_ClasseVeiculosCredenciais);

        //        XmlNode _VeiculosCredenciais = _xmlDocument.CreateElement("VeiculosCredenciais");
        //        _ClasseVeiculosCredenciais.AppendChild(_VeiculosCredenciais);

        //        string _strSql;
        //        string _statusSTR = "";

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        _empresaNome = _empresaNome == "" ? "" : " AND Nome like '%" + _empresaNome + "%' ";
        //        _statusSTR = _status == 0 ? "" : " AND CredencialStatusID = " + _status + "' ";
        //        _validade = _validade == "" ? "" : " AND _validade like '%" + _validade + "%'";

        //        _strSql = "SELECT dbo.Empresas.Nome AS EmpresaNome, dbo.EmpresasContratos.Descricao AS ContratoDescricao, dbo.LayoutsCrachas.Nome AS LayoutCrachaNome," +
        //            " dbo.FormatosCredenciais.Descricao AS FormatoCredencialDescricao, dbo.VeiculosCredenciais.NumeroCredencial, dbo.VeiculosCredenciais.FC," +
        //            " dbo.VeiculosCredenciais.Emissao, dbo.VeiculosCredenciais.Validade, dbo.VeiculosCredenciais.CredencialStatusID," +
        //            " dbo.VeiculosCredenciais.TipoCredencialID, dbo.VeiculosCredenciais.TecnologiaCredencialID, dbo.VeiculosCredenciais.FormatoCredencialID," +
        //            " dbo.VeiculosCredenciais.EmpresaID, dbo.VeiculosCredenciais.EmpresaContratoID, dbo.VeiculosCredenciais.LayoutCrachaID," +
        //            " dbo.VeiculosCredenciais.VeiculoCredencialID, dbo.VeiculosCredenciais.VeiculoID, dbo.Veiculos.Nome AS VeiculoNome" +
        //            " FROM dbo.VeiculosCredenciais INNER JOIN dbo.Empresas ON dbo.VeiculosCredenciais.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN" +
        //            " dbo.EmpresasContratos ON dbo.VeiculosCredenciais.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN" +
        //            " dbo.LayoutsCrachas ON dbo.VeiculosCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID INNER JOIN dbo.FormatosCredenciais ON" +
        //            " dbo.VeiculosCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN dbo.Veiculos ON" +
        //            " dbo.VeiculosCredenciais.VeiculoID = dbo.Veiculos.VeiculoID WHERE dbo.VeiculosCredenciais.VeiculoID = " +
        //            _veiculoID + _statusSTR + _empresaNome + _validade;

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _VeiculoCredencial = _xmlDocument.CreateElement("VeiculoCredencial");
        //            _VeiculosCredenciais.AppendChild(_VeiculoCredencial);

        //            XmlNode _VeiculoCredencialID = _xmlDocument.CreateElement("VeiculoCredencialID");
        //            _VeiculoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoCredencialID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_VeiculoCredencialID);

        //            XmlNode _VeiculoID = _xmlDocument.CreateElement("VeiculoID");
        //            _VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_VeiculoID);

        //            XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
        //            _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_EmpresaID);

        //            XmlNode _EmpresaContratoID = _xmlDocument.CreateElement("EmpresaContratoID");
        //            _EmpresaContratoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaContratoID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_EmpresaContratoID);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("ContratoDescricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContratoDescricao"].ToString())));
        //            _VeiculoCredencial.AppendChild(_Descricao);

        //            XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
        //            _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
        //            _VeiculoCredencial.AppendChild(_Empresa);

        //            XmlNode _VeiculoNome = _xmlDocument.CreateElement("VeiculoNome");
        //            _VeiculoNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoNome"].ToString())));
        //            _VeiculoCredencial.AppendChild(_VeiculoNome);

        //            XmlNode _TecnologiaCredencialID = _xmlDocument.CreateElement("TecnologiaCredencialID");
        //            _TecnologiaCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TecnologiaCredencialID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_TecnologiaCredencialID);

        //            XmlNode _TecnologiaCredencialDescricao = _xmlDocument.CreateElement("TecnologiaCredencialDescricao");
        //            var _tec =  TecnologiasCredenciais.FirstOrDefault(x => x.TecnologiaCredencialID == Convert.ToInt32(_sqlreader["TecnologiaCredencialID"].ToString()));
        //            if (_tec != null)
        //            {
        //                _TecnologiaCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tec.Descricao.ToString()));
        //                _VeiculoCredencial.AppendChild(_TecnologiaCredencialDescricao);
        //            }



        //            XmlNode _TipoCredencialID = _xmlDocument.CreateElement("TipoCredencialID");
        //            _TipoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoCredencialID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_TipoCredencialID);

        //            XmlNode _TipoCredencialDescricao = _xmlDocument.CreateElement("TipoCredencialDescricao");
        //            var _tip = TiposCredenciais.FirstOrDefault(x => x.TipoCredencialID == Convert.ToInt32(_sqlreader["TipoCredencialID"].ToString()));
        //            if (_tip != null)
        //            {
        //                _TipoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tip.Descricao.ToString()));
        //                _VeiculoCredencial.AppendChild(_TipoCredencialDescricao);
        //            }


        //            XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
        //            _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_LayoutCrachaID);

        //            XmlNode _LayoutCrachaNome = _xmlDocument.CreateElement("LayoutCrachaNome");
        //            _LayoutCrachaNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaNome"].ToString())));
        //            _VeiculoCredencial.AppendChild(_LayoutCrachaNome);

        //            XmlNode _FormatoCredencialID = _xmlDocument.CreateElement("FormatoCredencialID");
        //            _FormatoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_FormatoCredencialID);

        //            XmlNode _FormatoCredencialDescricao = _xmlDocument.CreateElement("FormatoCredencialDescricao");
        //            _FormatoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialDescricao"].ToString())));
        //            _VeiculoCredencial.AppendChild(_FormatoCredencialDescricao);

        //            XmlNode _NumeroCredencial = _xmlDocument.CreateElement("NumeroCredencial");
        //            _NumeroCredencial.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroCredencial"].ToString())));
        //            _VeiculoCredencial.AppendChild(_NumeroCredencial);

        //            XmlNode _FC = _xmlDocument.CreateElement("FC");
        //            _FC.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FC"].ToString())));
        //            _VeiculoCredencial.AppendChild(_FC);

        //            XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
        //            _Emissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Emissao"].ToString())));
        //            _VeiculoCredencial.AppendChild(_Emissao);

        //            XmlNode _Validade = _xmlDocument.CreateElement("Validade");
        //            _Validade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Validade"].ToString())));
        //            _VeiculoCredencial.AppendChild(_Validade);

        //            XmlNode _CredencialStatusID = _xmlDocument.CreateElement("CredencialStatusID");
        //            _CredencialStatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialStatusID"].ToString())));
        //            _VeiculoCredencial.AppendChild(_CredencialStatusID);

        //            XmlNode _CredencialStatusDescricao = _xmlDocument.CreateElement("CredencialStatusDescricao");
        //            var _sta = CredenciaisStatus.FirstOrDefault(x => x.CredencialStatusID == Convert.ToInt32(_sqlreader["CredencialStatusID"].ToString()));
        //            if (_sta != null)
        //            {
        //                _CredencialStatusDescricao.AppendChild(_xmlDocument.CreateTextNode(_sta.Descricao.ToString()));
        //                _VeiculoCredencial.AppendChild(_CredencialStatusDescricao);
        //            }

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
        private string RequisitaVeiculosCredenciais(int _veiculoID, string _empresaNome = "", int _status = 0, string _validade = "")//Possibilidade de criar a pesquisa por Matriculatambem
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseVeiculosCredenciais = _xmlDocument.CreateElement("ClasseVeiculosCredenciais");
                _xmlDocument.AppendChild(_ClasseVeiculosCredenciais);

                XmlNode _VeiculosCredenciais = _xmlDocument.CreateElement("VeiculosCredenciais");
                _ClasseVeiculosCredenciais.AppendChild(_VeiculosCredenciais);

                string _strSql;
                string _statusSTR = "";

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _empresaNome = _empresaNome == "" ? "" : " AND Nome like '%" + _empresaNome + "%' ";
                _statusSTR = _status == 0 ? "" : " AND CredencialStatusID = " + _status + "' ";
                _validade = _validade == "" ? "" : " AND _validade like '%" + _validade + "%'";

                //_strSql = "SELECT dbo.LayoutsCrachas.Nome AS LayoutCrachaNome, dbo.FormatosCredenciais.Descricao AS FormatoCredencialDescricao, dbo.VeiculosCredenciais.NumeroCredencial, dbo.VeiculosCredenciais.FC," +
                //    " dbo.VeiculosCredenciais.Emissao, dbo.VeiculosCredenciais.Validade, dbo.VeiculosCredenciais.CredencialStatusID, dbo.VeiculosCredenciais.VeiculoEmpresaID," +
                //    " dbo.VeiculosCredenciais.TipoCredencialID, dbo.VeiculosCredenciais.TecnologiaCredencialID, dbo.VeiculosCredenciais.FormatoCredencialID, dbo.VeiculosCredenciais.LayoutCrachaID," +
                //    " dbo.VeiculosCredenciais.VeiculoCredencialID, dbo.Veiculos.Nome AS VeiculoNome, dbo.Empresas.Nome AS EmpresaNome, dbo.EmpresasContratos.Descricao AS ContratoDescricao," +
                //    "  dbo.VeiculosEmpresas.EmpresaID, dbo.VeiculosEmpresas.VeiculoID, dbo.VeiculosCredenciais.CardHolderGUID,dbo.VeiculosCredenciais.CredencialGUID  " +
                //    " FROM dbo.VeiculosCredenciais INNER JOIN" +
                //    "  dbo.FormatosCredenciais ON dbo.VeiculosCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN" +
                //    "  dbo.VeiculosEmpresas ON dbo.VeiculosCredenciais.VeiculoEmpresaID = dbo.VeiculosEmpresas.VeiculoEmpresaID INNER JOIN" +
                //    "  dbo.Empresas ON dbo.VeiculosEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN" +
                //    "  dbo.EmpresasContratos ON dbo.VeiculosEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN" +
                //    "  dbo.Veiculos ON dbo.VeiculosEmpresas.VeiculoID = dbo.Veiculos.VeiculoID LEFT OUTER JOIN" +
                //    "  dbo.LayoutsCrachas ON dbo.VeiculosCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID" +
                //    " WHERE dbo.VeiculosEmpresas.VeiculoID =" + _veiculoID + _statusSTR + _empresaNome + _validade;


                _strSql = "SELECT dbo.LayoutsCrachas.Nome AS LayoutCrachaNome, dbo.FormatosCredenciais.Descricao AS FormatoCredencialDescricao," +
                    " dbo.VeiculosCredenciais.NumeroCredencial, dbo.VeiculosCredenciais.FC, dbo.VeiculosCredenciais.Emissao," +
                    " dbo.VeiculosCredenciais.Validade, dbo.VeiculosCredenciais.CredencialStatusID, dbo.VeiculosCredenciais.VeiculoEmpresaID, " +
                    " dbo.VeiculosCredenciais.TipoCredencialID, dbo.VeiculosCredenciais.TecnologiaCredencialID, dbo.VeiculosCredenciais.FormatoCredencialID," +
                    " dbo.VeiculosCredenciais.LayoutCrachaID, dbo.VeiculosCredenciais.VeiculoCredencialID, dbo.Veiculos.Descricao AS VeiculoDescricao," +
                    " dbo.Empresas.Nome AS EmpresaNome, dbo.EmpresasContratos.Descricao AS ContratoDescricao,dbo.VeiculosEmpresas.EmpresaID," +
                    " dbo.VeiculosEmpresas.VeiculoID, dbo.VeiculosCredenciais.CardHolderGUID, dbo.VeiculosCredenciais.CredencialGUID," +
                    " dbo.Veiculos.Foto AS VeiculoFoto, dbo.Veiculos.Placa, " +
                    " dbo.Empresas.Apelido AS EmpresaApelido, dbo.VeiculosEmpresas.Cargo, dbo.LayoutsCrachas.LayoutCrachaGUID, dbo.Empresas.CNPJ," +
                    " dbo.FormatosCredenciais.FormatIDGUID FROM dbo.VeiculosCredenciais INNER JOIN dbo.FormatosCredenciais ON" +
                    " dbo.VeiculosCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN dbo.VeiculosEmpresas ON" +
                    " dbo.VeiculosCredenciais.VeiculoEmpresaID = dbo.VeiculosEmpresas.VeiculoEmpresaID INNER JOIN dbo.Empresas ON" +
                    " dbo.VeiculosEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON " +
                    " dbo.VeiculosEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN dbo.Veiculos ON" +
                    " dbo.VeiculosEmpresas.VeiculoID = dbo.Veiculos.VeiculoID LEFT OUTER JOIN dbo.LayoutsCrachas ON" +
                    " dbo.VeiculosCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID" +
                    " WHERE dbo.VeiculosEmpresas.VeiculoID =" + _veiculoID + _statusSTR + _empresaNome + _validade;

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _VeiculoCredencial = _xmlDocument.CreateElement("VeiculoCredencial");
                    _VeiculosCredenciais.AppendChild(_VeiculoCredencial);

                    XmlNode _VeiculoCredencialID = _xmlDocument.CreateElement("VeiculoCredencialID");
                    _VeiculoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoCredencialID"].ToString())));
                    _VeiculoCredencial.AppendChild(_VeiculoCredencialID);

                    XmlNode _VeiculoEmpresaID = _xmlDocument.CreateElement("VeiculoEmpresaID");
                    _VeiculoEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoEmpresaID"].ToString())));
                    _VeiculoCredencial.AppendChild(_VeiculoEmpresaID);

                    XmlNode _VeiculoID = _xmlDocument.CreateElement("VeiculoID");
                    _VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoID"].ToString())));
                    _VeiculoCredencial.AppendChild(_VeiculoID);

                    XmlNode _CardHolderGUID = _xmlDocument.CreateElement("CardHolderGuid");
                    _CardHolderGUID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CardHolderGuid"].ToString())));
                    _VeiculoCredencial.AppendChild(_CardHolderGUID);

                    XmlNode _CredencialGUID = _xmlDocument.CreateElement("CredencialGuid");
                    _CredencialGUID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialGuid"].ToString())));
                    _VeiculoCredencial.AppendChild(_CredencialGUID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("ContratoDescricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContratoDescricao"].ToString())));
                    _VeiculoCredencial.AppendChild(_Descricao);

                    XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
                    _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
                    _VeiculoCredencial.AppendChild(_Empresa);

                    XmlNode _VeiculoNome = _xmlDocument.CreateElement("VeiculoDescricao");
                    _VeiculoNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoDescricao"].ToString())));
                    _VeiculoCredencial.AppendChild(_VeiculoNome);

                    XmlNode _TecnologiaCredencialID = _xmlDocument.CreateElement("TecnologiaCredencialID");
                    _TecnologiaCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TecnologiaCredencialID"].ToString())));
                    _VeiculoCredencial.AppendChild(_TecnologiaCredencialID);

                    XmlNode _TecnologiaCredencialDescricao = _xmlDocument.CreateElement("TecnologiaCredencialDescricao");
                    var _tec = TecnologiasCredenciais.FirstOrDefault(x => x.TecnologiaCredencialID == Convert.ToInt32(_sqlreader["TecnologiaCredencialID"].ToString()));
                    if (_tec != null)
                    {
                        _TecnologiaCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tec.Descricao.ToString()));
                        _VeiculoCredencial.AppendChild(_TecnologiaCredencialDescricao);
                    }

                    XmlNode _TipoCredencialID = _xmlDocument.CreateElement("TipoCredencialID");
                    _TipoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoCredencialID"].ToString())));
                    _VeiculoCredencial.AppendChild(_TipoCredencialID);

                    XmlNode _TipoCredencialDescricao = _xmlDocument.CreateElement("TipoCredencialDescricao");
                    var _tip = TiposCredenciais.FirstOrDefault(x => x.TipoCredencialID == Convert.ToInt32(_sqlreader["TipoCredencialID"].ToString()));
                    if (_tip != null)
                    {
                        _TipoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tip.Descricao.ToString()));
                        _VeiculoCredencial.AppendChild(_TipoCredencialDescricao);
                    }

                    XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
                    _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaID"].ToString())));
                    _VeiculoCredencial.AppendChild(_LayoutCrachaID);

                    XmlNode _LayoutCrachaNome = _xmlDocument.CreateElement("LayoutCrachaNome");
                    _LayoutCrachaNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaNome"].ToString())));
                    _VeiculoCredencial.AppendChild(_LayoutCrachaNome);

                    XmlNode _FormatoCredencialID = _xmlDocument.CreateElement("FormatoCredencialID");
                    _FormatoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialID"].ToString())));
                    _VeiculoCredencial.AppendChild(_FormatoCredencialID);

                    XmlNode _FormatoCredencialDescricao = _xmlDocument.CreateElement("FormatoCredencialDescricao");
                    _FormatoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialDescricao"].ToString())));
                    _VeiculoCredencial.AppendChild(_FormatoCredencialDescricao);

                    XmlNode _NumeroCredencial = _xmlDocument.CreateElement("NumeroCredencial");
                    _NumeroCredencial.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroCredencial"].ToString())));
                    _VeiculoCredencial.AppendChild(_NumeroCredencial);

                    XmlNode _FC = _xmlDocument.CreateElement("FC");
                    _FC.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FC"].ToString())));
                    _VeiculoCredencial.AppendChild(_FC);

                    var dateStr = (_sqlreader["Emissao"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
                        _Emissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _VeiculoCredencial.AppendChild(_Emissao);
                    }

                    dateStr = (_sqlreader["Validade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                        _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _VeiculoCredencial.AppendChild(_Validade);
                    }

                    XmlNode _CredencialStatusID = _xmlDocument.CreateElement("CredencialStatusID");
                    _CredencialStatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialStatusID"].ToString())));
                    _VeiculoCredencial.AppendChild(_CredencialStatusID);

                    XmlNode _CredencialStatusDescricao = _xmlDocument.CreateElement("CredencialStatusDescricao");
                    var _sta = CredenciaisStatus.FirstOrDefault(x => x.CredencialStatusID == Convert.ToInt32(_sqlreader["CredencialStatusID"].ToString()));
                    if (_sta != null)
                    {
                        _CredencialStatusDescricao.AppendChild(_xmlDocument.CreateTextNode(_sta.Descricao.ToString()));
                        _VeiculoCredencial.AppendChild(_CredencialStatusDescricao);
                    }


                    XmlNode _Vinculo9 = _xmlDocument.CreateElement("Placa");
                    _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Placa"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo9);

                    XmlNode _Vinculo11 = _xmlDocument.CreateElement("VeiculoFoto");
                    _Vinculo11.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoFoto"].ToString())));
                    _VeiculoCredencial.AppendChild(_Vinculo11);


                    XmlNode _Vinculo23 = _xmlDocument.CreateElement("EmpresaApelido");
                    _Vinculo23.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaApelido"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo23);

                    XmlNode _Vinculo13 = _xmlDocument.CreateElement("Cargo");
                    _Vinculo13.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo13);


                    XmlNode _Vinculo15 = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _Vinculo15.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaGUID"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo15);

                    XmlNode _Vinculo19 = _xmlDocument.CreateElement("CNPJ");
                    _Vinculo19.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo19);

                    XmlNode _Vinculo20 = _xmlDocument.CreateElement("FormatIDGUID");
                    _Vinculo20.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatIDGUID"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo20);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch (Exception ex)
            {

                return null;
            }
            return null;
        }
        private string RequisitaEmpresas(int _empresaID = 0, string _nome = "", string _apelido = "", string _cNPJ = "", int _excluida = 0, string _quantidaderegistro = "500")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresas = _xmlDocument.CreateElement("ClasseEmpresas");
                _xmlDocument.AppendChild(_ClasseEmpresas);

                XmlNode _Empresas = _xmlDocument.CreateElement("Empresas");
                _ClasseEmpresas.AppendChild(_Empresas);

                string _strSql = " [EmpresaID],[Nome],[Apelido],[CNPJ],[CEP],[Endereco]," +
                    "[Numero],[Complemento],[Bairro],[MunicipioID],[EstadoID]," +
                    "[Email1],[Contato1],[Telefone1],[Celular1],[Email2],[Contato2],[Telefone2],[Celular2]," +
                    "[Obs],[Responsavel],[InsEst],[InsMun],[Excluida],[Pendente11],[Pendente12],[Pendente13],[Pendente14]" +
                    ",[Pendente15],[Pendente16],[Pendente17]";



                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _empresaIDSTR = "";

                _empresaIDSTR = _empresaID == 0 ? "" : " AND EmpresaID = " + _empresaID;
                _nome = _nome == "" ? "" : " AND Nome like '%" + _nome + "%' ";
                _apelido = _apelido == "" ? "" : "AND Apelido like '%" + _apelido + "%' ";
                _cNPJ = _cNPJ == "" ? "" : " AND CPF like '%" + _cNPJ.RetirarCaracteresEspeciais() + "%'";

                if (_quantidaderegistro == "0")
                {
                    _strSql = "select " + _strSql + " from Empresas where Excluida = " + _excluida + _empresaIDSTR +
                       _nome + _apelido + _cNPJ + " order by EmpresaID desc";
                }
                else
                {
                    _strSql = "select Top " + _quantidaderegistro + _strSql + " from Empresas where Excluida = " + _excluida + _empresaIDSTR +
                       _nome + _apelido + _cNPJ + " order by EmpresaID desc";
                }


                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Empresa = _xmlDocument.CreateElement("Empresa");
                    _Empresas.AppendChild(_Empresa);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _Empresa.AppendChild(_EmpresaID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _Empresa.AppendChild(_Nome);

                    XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
                    _Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
                    _Empresa.AppendChild(_Apelido);

                    XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
                    _CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().FormatarCnpj())));
                    _Empresa.AppendChild(_CNPJ);

                    XmlNode _InsEst = _xmlDocument.CreateElement("InsEst");
                    _InsEst.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsEst"].ToString())));
                    _Empresa.AppendChild(_InsEst);

                    XmlNode _InsMun = _xmlDocument.CreateElement("InsMun");
                    _InsMun.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsMun"].ToString())));
                    _Empresa.AppendChild(_InsMun);

                    XmlNode _Responsavel = _xmlDocument.CreateElement("Responsavel");
                    _Responsavel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Responsavel"].ToString())));
                    _Empresa.AppendChild(_Responsavel);

                    XmlNode _CEP = _xmlDocument.CreateElement("CEP");
                    _CEP.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CEP"].ToString())));
                    _Empresa.AppendChild(_CEP);

                    XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
                    _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
                    _Empresa.AppendChild(_Endereco);

                    XmlNode _Numero = _xmlDocument.CreateElement("Numero");
                    _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
                    _Empresa.AppendChild(_Numero);

                    XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
                    _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
                    _Empresa.AppendChild(_Complemento);

                    XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
                    _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
                    _Empresa.AppendChild(_Bairro);

                    XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
                    _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    _Empresa.AppendChild(_EstadoID);

                    XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
                    _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    _Empresa.AppendChild(_MunicipioID);

                    XmlNode _Email1 = _xmlDocument.CreateElement("Email1");
                    _Email1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email1"].ToString())));
                    _Empresa.AppendChild(_Email1);

                    XmlNode _Contato1 = _xmlDocument.CreateElement("Contato1");
                    _Contato1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contato1"].ToString())));
                    _Empresa.AppendChild(_Contato1);

                    XmlNode _Tel1 = _xmlDocument.CreateElement("Telefone1");
                    _Tel1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone1"].ToString())));
                    _Empresa.AppendChild(_Tel1);

                    XmlNode _Cel1 = _xmlDocument.CreateElement("Celular1");
                    _Cel1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular1"].ToString())));
                    _Empresa.AppendChild(_Cel1);

                    XmlNode Email2 = _xmlDocument.CreateElement("Email2");
                    Email2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email2"].ToString())));
                    _Empresa.AppendChild(Email2);

                    XmlNode _Contato2 = _xmlDocument.CreateElement("Contato2");
                    _Contato2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contato2"].ToString())));
                    _Empresa.AppendChild(_Contato2);

                    XmlNode _Tel2 = _xmlDocument.CreateElement("Telefone2");
                    _Tel2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone2"].ToString())));
                    _Empresa.AppendChild(_Tel2);

                    XmlNode _Cel2 = _xmlDocument.CreateElement("Celular2");
                    _Cel2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular2"].ToString())));
                    _Empresa.AppendChild(_Cel2);
                    //////////////////////////////////////////////
                    XmlNode _Obs = _xmlDocument.CreateElement("Obs");
                    _Obs.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Obs"].ToString())));
                    _Empresa.AppendChild(_Obs);

                    XmlNode _Logo = _xmlDocument.CreateElement("Logo");
                    //_Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString())));
                    _Empresa.AppendChild(_Logo);

                    XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
                    _Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
                    _Empresa.AppendChild(_Excluida);

                    XmlNode _Pendente1 = _xmlDocument.CreateElement("Pendente11");
                    _Pendente1.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente11"])).ToString()));
                    _Empresa.AppendChild(_Pendente1);

                    XmlNode _Pendente2 = _xmlDocument.CreateElement("Pendente12");
                    _Pendente2.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente12"])).ToString()));
                    _Empresa.AppendChild(_Pendente2);

                    XmlNode _Pendente3 = _xmlDocument.CreateElement("Pendente13");
                    _Pendente3.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente13"])).ToString()));
                    _Empresa.AppendChild(_Pendente3);

                    XmlNode _Pendente4 = _xmlDocument.CreateElement("Pendente14");
                    _Pendente4.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente14"])).ToString()));
                    _Empresa.AppendChild(_Pendente4);

                    XmlNode _Pendente5 = _xmlDocument.CreateElement("Pendente15");
                    _Pendente5.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente15"])).ToString()));
                    _Empresa.AppendChild(_Pendente5);

                    XmlNode _Pendente6 = _xmlDocument.CreateElement("Pendente16");
                    _Pendente6.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente16"])).ToString()));
                    _Empresa.AppendChild(_Pendente6);

                    XmlNode _Pendente7 = _xmlDocument.CreateElement("Pendente17");
                    _Pendente7.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente17"])).ToString()));
                    _Empresa.AppendChild(_Pendente7);

                    bool _pend = false;
                    _pend = (bool)_sqlreader["Pendente11"] ||
                        (bool)_sqlreader["Pendente12"] ||
                        (bool)_sqlreader["Pendente13"] ||
                        (bool)_sqlreader["Pendente14"] ||
                        (bool)_sqlreader["Pendente15"] ||
                        (bool)_sqlreader["Pendente16"] ||
                        (bool)_sqlreader["Pendente17"];

                    XmlNode _Pendente = _xmlDocument.CreateElement("Pendente");
                    _Pendente.AppendChild(_xmlDocument.CreateTextNode(Convert.ToInt32(_pend).ToString()));
                    _Empresa.AppendChild(_Pendente);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch (Exception ex)
            {

                return null;
            }
            return null;
        }

        private string RequisitaVeiculosEmpresas(int _veiculoID)//Possibilidade de criar a pesquisa por Matriculatambem
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseVeiculosEmpresas = _xmlDocument.CreateElement("ClasseVeiculosEmpresas");
                _xmlDocument.AppendChild(_ClasseVeiculosEmpresas);

                XmlNode _VeiculosEmpresas = _xmlDocument.CreateElement("VeiculosEmpresas");
                _ClasseVeiculosEmpresas.AppendChild(_VeiculosEmpresas);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _strSql = "SELECT dbo.VeiculosEmpresas.VeiculoEmpresaID, dbo.VeiculosEmpresas.EmpresaID, dbo.Empresas.Nome AS EmpresaNome," +
                    " dbo.EmpresasContratos.NumeroContrato, dbo.EmpresasContratos.Descricao, dbo.VeiculosEmpresas.Matricula," +
                    " dbo.VeiculosEmpresas.Cargo FROM dbo.VeiculosEmpresas INNER JOIN dbo.Empresas ON" +
                    " dbo.VeiculosEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON" +
                    " dbo.VeiculosEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID" +
                    " WHERE(dbo.VeiculosEmpresas.VeiculoID =" + _veiculoID + ")" +
                    " AND(dbo.VeiculosEmpresas.Ativo = 1)  ";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _VeiculoEmpresa = _xmlDocument.CreateElement("VeiculoEmpresa");
                    _VeiculosEmpresas.AppendChild(_VeiculoEmpresa);

                    XmlNode _VeiculoEmpresaID = _xmlDocument.CreateElement("VeiculoEmpresaID");
                    _VeiculoEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoEmpresaID"].ToString())));
                    _VeiculoEmpresa.AppendChild(_VeiculoEmpresaID);

                    //XmlNode _VeiculoID = _xmlDocument.CreateElement("VeiculoID");
                    //_VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoID"].ToString())));
                    //_VeiculoEmpresa.AppendChild(_VeiculoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _VeiculoEmpresa.AppendChild(_EmpresaID);

                    //XmlNode _EmpresaContratoID = _xmlDocument.CreateElement("EmpresaContratoID");
                    //_EmpresaContratoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaContratoID"].ToString())));
                    //_VeiculoEmpresa.AppendChild(_EmpresaContratoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _VeiculoEmpresa.AppendChild(_Descricao);

                    XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
                    _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
                    _VeiculoEmpresa.AppendChild(_Empresa);

                    //XmlNode _Cargo = _xmlDocument.CreateElement("Cargo");
                    //_Cargo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString())));
                    //_VeiculoEmpresa.AppendChild(_Cargo);

                    //XmlNode _Matricula = _xmlDocument.CreateElement("Matricula");
                    //_Matricula.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Matricula"].ToString())));
                    //_VeiculoEmpresa.AppendChild(_Matricula);

                    //XmlNode _Ativo = _xmlDocument.CreateElement("Ativo");
                    //_Ativo.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativo"])).ToString()));
                    //_VeiculoEmpresa.AppendChild(_Ativo);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch (Exception ex)
            {

                return null;
            }
            return null;
        }

        //private string RequisitaEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", int _excluida = 0, string _quantidaderegistro = "500")
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseEmpresas = _xmlDocument.CreateElement("ClasseEmpresas");
        //        _xmlDocument.AppendChild(_ClasseEmpresas);

        //        XmlNode _Empresas = _xmlDocument.CreateElement("Empresas");
        //        _ClasseEmpresas.AppendChild(_Empresas);

        //        string _strSql = " [EmpresaID],[Nome],[Apelido],[CNPJ],[CEP],[Endereco]," +
        //            "[Numero],[Complemento],[Bairro],[MunicipioID],[EstadoID],[Telefone],[Contato]," +
        //            "[Celular],[Email],[Obs],[Responsavel],[InsEst],[InsMun],[Excluida]";


        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        _empresaID = "%" + _empresaID + "%";
        //        _nome = "%" + _nome + "%";
        //        _apelido = "%" + _apelido + "%";
        //        _cNPJ = "%" + _cNPJ + "%";

        //        if (_quantidaderegistro == "0")
        //        {
        //            _strSql = "select " + _strSql + " from Empresas where EmpresaID like '" + _empresaID + "' and EmpresaNome like '" + _nome + "' and Apelido like '" + _apelido + "' and CNPJ like '" + _cNPJ + "' and Excluida  = " + _excluida + " order by EmpresaID desc";
        //        }
        //        else
        //        {
        //            _strSql = "select Top " + _quantidaderegistro + _strSql + "  from Empresas where EmpresaID like '" + _empresaID + "' and Nome like '" + _nome + "' and Apelido like '" + _apelido + "' and CNPJ like '" + _cNPJ + "' and Excluida  = " + _excluida + " order by EmpresaID desc";
        //        }


        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _Empresa = _xmlDocument.CreateElement("Empresa");
        //            _Empresas.AppendChild(_Empresa);

        //            XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
        //            _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
        //            _Empresa.AppendChild(_EmpresaID);

        //            XmlNode _Nome = _xmlDocument.CreateElement("Nome");
        //            _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
        //            _Empresa.AppendChild(_Nome);

        //            XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
        //            _Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
        //            _Empresa.AppendChild(_Apelido);

        //            XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
        //            _CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString())));
        //            _Empresa.AppendChild(_CNPJ);

        //            XmlNode _InsEst = _xmlDocument.CreateElement("InsEst");
        //            _InsEst.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsEst"].ToString())));
        //            _Empresa.AppendChild(_InsEst);

        //            XmlNode _InsMun = _xmlDocument.CreateElement("InsMun");
        //            _InsMun.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsMun"].ToString())));
        //            _Empresa.AppendChild(_InsMun);

        //            XmlNode _Responsavel = _xmlDocument.CreateElement("Responsavel");
        //            _Responsavel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Responsavel"].ToString())));
        //            _Empresa.AppendChild(_Responsavel);

        //            XmlNode _CEP = _xmlDocument.CreateElement("CEP");
        //            _CEP.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CEP"].ToString())));
        //            _Empresa.AppendChild(_CEP);

        //            XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
        //            _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
        //            _Empresa.AppendChild(_Endereco);

        //            XmlNode _Numero = _xmlDocument.CreateElement("Numero");
        //            _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
        //            _Empresa.AppendChild(_Numero);

        //            XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
        //            _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
        //            _Empresa.AppendChild(_Complemento);

        //            XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
        //            _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
        //            _Empresa.AppendChild(_Bairro);

        //            XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
        //            _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
        //            _Empresa.AppendChild(_EstadoID);

        //            XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
        //            _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
        //            _Empresa.AppendChild(_MunicipioID);

        //            XmlNode _Tel = _xmlDocument.CreateElement("Telefone");
        //            _Tel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone"].ToString())));
        //            _Empresa.AppendChild(_Tel);

        //            XmlNode _Cel = _xmlDocument.CreateElement("Celular");
        //            _Cel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular"].ToString())));
        //            _Empresa.AppendChild(_Cel);

        //            XmlNode _Contato = _xmlDocument.CreateElement("Contato");
        //            _Contato.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contato"].ToString())));
        //            _Empresa.AppendChild(_Contato);

        //            XmlNode _Email = _xmlDocument.CreateElement("Email");
        //            _Email.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email"].ToString())));
        //            _Empresa.AppendChild(_Email);

        //            XmlNode _Obs = _xmlDocument.CreateElement("Obs");
        //            _Obs.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Obs"].ToString())));
        //            _Empresa.AppendChild(_Obs);

        //            XmlNode _Logo = _xmlDocument.CreateElement("Logo");
        //            //_Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString())));
        //            _Empresa.AppendChild(_Logo);

        //            XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
        //            _Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
        //            _Empresa.AppendChild(_Excluida);


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

        private string RequisitaEmpresasLayoutsCrachas(int _veiculoEmpresaID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasLayoutsCrachas = _xmlDocument.CreateElement("ClasseEmpresasLayoutsCrachas");
                _xmlDocument.AppendChild(_ClasseEmpresasLayoutsCrachas);

                XmlNode _EmpresasLayoutsCrachas = _xmlDocument.CreateElement("EmpresasLayoutsCrachas");
                _ClasseEmpresasLayoutsCrachas.AppendChild(_EmpresasLayoutsCrachas);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _SQL = "SELECT dbo.VeiculosEmpresas.VeiculoEmpresaID, dbo.EmpresasLayoutsCrachas.EmpresaID, dbo.LayoutsCrachas.Nome, dbo.EmpresasLayoutsCrachas.LayoutCrachaID " +
                    "FROM dbo.VeiculosEmpresas INNER JOIN dbo.EmpresasLayoutsCrachas ON dbo.VeiculosEmpresas.EmpresaID = dbo.EmpresasLayoutsCrachas.EmpresaID" +
                    " INNER JOIN dbo.LayoutsCrachas ON dbo.EmpresasLayoutsCrachas.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID " +
                    "WHERE dbo.VeiculosEmpresas.VeiculoEmpresaID = " + _veiculoEmpresaID;

                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _EmpresaLayoutCracha = _xmlDocument.CreateElement("EmpresaLayoutCracha");
                    _EmpresasLayoutsCrachas.AppendChild(_EmpresaLayoutCracha);

                    //XmlNode _EmpresaLayoutCrachaID = _xmlDocument.CreateElement("EmpresaLayoutCrachaID");
                    //_EmpresaLayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaLayoutCrachaID"].ToString())));
                    //_EmpresaLayoutCracha.AppendChild(_EmpresaLayoutCrachaID);

                    //XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    //_EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaID"].ToString())));
                    //_EmpresaLayoutCracha.AppendChild(_EmpresaID);

                    //XmlNode _LayoutCrachaGUID = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    //_LayoutCrachaGUID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaGUID"].ToString())));
                    //_EmpresaLayoutCracha.AppendChild(_LayoutCrachaGUID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Nome");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_Descricao);

                    XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
                    _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaID"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_LayoutCrachaID);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaEmpresasLayoutsCrachas ex: " + ex);

                return null;
            }
        }

        private string RequisitaFormatosCredenciais(int _empresaID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseFormatosCredenciais = _xmlDocument.CreateElement("ClasseFormatosCredenciais");
                _xmlDocument.AppendChild(_ClasseFormatosCredenciais);

                XmlNode _FormatosCredenciais = _xmlDocument.CreateElement("FormatosCredenciais");
                _ClasseFormatosCredenciais.AppendChild(_FormatosCredenciais);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _SQL = "select * from FormatosCredenciais";
                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _FormatoCredencial = _xmlDocument.CreateElement("FormatoCredencial");
                    _FormatosCredenciais.AppendChild(_FormatoCredencial);

                    XmlNode _FormatoCredencialID = _xmlDocument.CreateElement("FormatoCredencialID");
                    _FormatoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["FormatoCredencialID"].ToString())));
                    _FormatoCredencial.AppendChild(_FormatoCredencialID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString().Trim())));
                    _FormatoCredencial.AppendChild(_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaFormatosCredenciais ex: " + ex);

                return null;
            }
        }

        private string RequisitaVinculos(int _VeiculoCredencialID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseVinculos = _xmlDocument.CreateElement("ClasseVinculos");
                _xmlDocument.AppendChild(_ClasseVinculos);

                XmlNode _Vinculos = _xmlDocument.CreateElement("Vinculos");
                _ClasseVinculos.AppendChild(_Vinculos);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _SQL = "SELECT dbo.VeiculosCredenciais.VeiculoCredencialID,dbo.VeiculosCredenciais.CardHolderGuid,dbo.VeiculosCredenciais.CredencialGuid," +
                    " dbo.VeiculosEmpresas.VeiculoID, dbo.Veiculos.Nome AS VeiculoNome," +
                    " dbo.Veiculos.CPF,dbo.Veiculos.Apelido AS VeiculoApelido, dbo.Veiculos.Motorista, dbo.Veiculos.Foto, dbo.VeiculosEmpresas.EmpresaID," +
                    " dbo.Empresas.Nome AS EmpresaNome,dbo.Empresas.Apelido AS EmpresaApelido, dbo.Empresas.CNPJ, dbo.FormatosCredenciais.FormatIDGUID," +
                    " dbo.VeiculosCredenciais.NumeroCredencial, dbo.VeiculosCredenciais.FC,dbo.VeiculosCredenciais.Validade, dbo.LayoutsCrachas.LayoutCrachaGUID," +
                    " dbo.VeiculosEmpresas.Cargo, dbo.VeiculosEmpresas.Matricula, dbo.VeiculosEmpresas.Ativo FROM dbo.Empresas INNER JOIN" +
                    " dbo.Veiculos INNER JOIN dbo.VeiculosCredenciais INNER JOIN dbo.LayoutsCrachas ON" +
                    " dbo.VeiculosCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID INNER JOIN dbo.FormatosCredenciais ON" +
                    " dbo.VeiculosCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN dbo.VeiculosEmpresas ON" +
                    " dbo.VeiculosCredenciais.VeiculoEmpresaID = dbo.VeiculosEmpresas.VeiculoEmpresaID ON" +
                    " dbo.Veiculos.VeiculoID = dbo.VeiculosEmpresas.VeiculoID ON dbo.Empresas.EmpresaID = dbo.VeiculosEmpresas.EmpresaID " +
                    "WHERE dbo.VeiculosCredenciais.VeiculoCredencialID = " + _VeiculoCredencialID;


                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Vinculo = _xmlDocument.CreateElement("Vinculo");
                    _Vinculos.AppendChild(_Vinculo);

                    XmlNode _Vinculo24 = _xmlDocument.CreateElement("VeiculoID");
                    _Vinculo24.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["VeiculoID"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo24);

                    XmlNode _CardHolderGUID = _xmlDocument.CreateElement("CardHolderGuid");
                    _CardHolderGUID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["CardHolderGuid"].ToString())));
                    _Vinculo.AppendChild(_CardHolderGUID);

                    XmlNode _CredencialGUID = _xmlDocument.CreateElement("CredencialGuid");
                    _CredencialGUID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["CredencialGuid"].ToString())));
                    _Vinculo.AppendChild(_CredencialGUID);

                    XmlNode _Vinculo9 = _xmlDocument.CreateElement("CPF");
                    _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["CPF"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo9);

                    XmlNode _Vinculo10 = _xmlDocument.CreateElement("VeiculoNome");
                    _Vinculo10.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["VeiculoNome"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo10);

                    XmlNode _Vinculo22 = _xmlDocument.CreateElement("Motorista");
                    _Vinculo22.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqldatareader["Motorista"])).ToString()));
                    _Vinculo.AppendChild(_Vinculo22);

                    XmlNode _Vinculo21 = _xmlDocument.CreateElement("VeiculoApelido");
                    _Vinculo21.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["VeiculoApelido"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo21);

                    XmlNode _Vinculo11 = _xmlDocument.CreateElement("VeiculoFoto");
                    _Vinculo11.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Foto"].ToString())));
                    _Vinculo.AppendChild(_Vinculo11);

                    XmlNode _Vinculo12 = _xmlDocument.CreateElement("EmpresaNome");
                    _Vinculo12.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaNome"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo12);


                    XmlNode _Vinculo23 = _xmlDocument.CreateElement("EmpresaApelido");
                    _Vinculo23.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaApelido"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo23);

                    XmlNode _Vinculo13 = _xmlDocument.CreateElement("Cargo");
                    _Vinculo13.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Cargo"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo13);

                    XmlNode _Vinculo14 = _xmlDocument.CreateElement("Matricula");
                    _Vinculo14.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Matricula"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo14);

                    XmlNode _Vinculo15 = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _Vinculo15.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaGUID"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo15);

                    XmlNode _Vinculo16 = _xmlDocument.CreateElement("NumeroCredencial");
                    _Vinculo16.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["NumeroCredencial"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo16);

                    XmlNode _Vinculo17 = _xmlDocument.CreateElement("FC");
                    _Vinculo17.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["FC"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo17);

                    var dateStr = (_sqldatareader["Validade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt1 = Convert.ToDateTime(dateStr);
                        XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                        //format valid for XML W3C yyyy-MM-ddTHH:mm:ss
                        _Validade.AppendChild(_xmlDocument.CreateTextNode(dt1.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Vinculo.AppendChild(_Validade);
                    }

                    XmlNode _Vinculo19 = _xmlDocument.CreateElement("CNPJ");
                    _Vinculo19.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["CNPJ"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo19);

                    XmlNode _Vinculo20 = _xmlDocument.CreateElement("FormatIDGUID");
                    _Vinculo20.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["FormatIDGUID"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo20);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaVinculos ex: " + ex);

                return null;
            }
        }

        private string RequisitaContratos(int _empresaID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseContratosEmpresas = _xmlDocument.CreateElement("ClasseEmpresasContratos");
                _xmlDocument.AppendChild(_ClasseContratosEmpresas);

                XmlNode _EmpresasContratos = _xmlDocument.CreateElement("EmpresasContratos");
                _ClasseContratosEmpresas.AppendChild(_EmpresasContratos);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _strSql = "SELECT dbo.VeiculosEmpresas.VeiculoID, dbo.VeiculosEmpresas.EmpresaID, dbo.Empresas.Nome AS EmpresaNome," +
                    " dbo.VeiculosEmpresas.Ativo, dbo.EmpresasContratos.Descricao, dbo.VeiculosEmpresas.EmpresaContratoID FROM dbo.VeiculosEmpresas INNER JOIN dbo.Empresas" +
                    " ON dbo.VeiculosEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON" +
                    " dbo.VeiculosEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID WHERE(dbo.VeiculosEmpresas.Ativo = 1)" +
                    " AND(dbo.VeiculosEmpresas.VeiculoID =" + VeiculoSelecionadaID + ")AND (dbo.VeiculosEmpresas.EmpresaID =" + _empresaID + ")";


                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _EmpresaContrato = _xmlDocument.CreateElement("EmpresaContrato");
                    _EmpresasContratos.AppendChild(_EmpresaContrato);

                    XmlNode _EmpresaContratoID = _xmlDocument.CreateElement("EmpresaContratoID");
                    _EmpresaContratoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaContratoID"].ToString())));
                    _EmpresaContrato.AppendChild(_EmpresaContratoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _EmpresaContrato.AppendChild(_EmpresaID);

                    //XmlNode _NumeroContrato = _xmlDocument.CreateElement("NumeroContrato");
                    //_NumeroContrato.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroContrato"].ToString())));
                    //_EmpresaContrato.AppendChild(_NumeroContrato);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _EmpresaContrato.AppendChild(_Descricao);


                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch
            {

                return null;
            }
            return null;
        }

        private string RequisitaTiposCredenciais()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposCredenciais = _xmlDocument.CreateElement("ClasseTiposCredenciais");
                _xmlDocument.AppendChild(_ClasseTiposCredenciais);

                XmlNode _TiposCredenciais = _xmlDocument.CreateElement("TiposCredenciais");
                _ClasseTiposCredenciais.AppendChild(_TiposCredenciais);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _strSql = "select * from TiposCredenciais ";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _TipoCredencial = _xmlDocument.CreateElement("TipoCredencial");
                    _TiposCredenciais.AppendChild(_TipoCredencial);

                    XmlNode _TipoCredencialID = _xmlDocument.CreateElement("TipoCredencialID");
                    _TipoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoCredencialID"].ToString())));
                    _TipoCredencial.AppendChild(_TipoCredencialID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _TipoCredencial.AppendChild(_Descricao);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch
            {

                return null;
            }

        }

        private string RequisitaTecnologiasCredenciais()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTecnologiasCredenciais = _xmlDocument.CreateElement("ClasseTecnologiasCredenciais");
                _xmlDocument.AppendChild(_ClasseTecnologiasCredenciais);

                XmlNode _TecnologiasCredenciais = _xmlDocument.CreateElement("TecnologiasCredenciais");
                _ClasseTecnologiasCredenciais.AppendChild(_TecnologiasCredenciais);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _strSql = "select * from TecnologiasCredenciais ";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _TecnologiaCredencial = _xmlDocument.CreateElement("TecnologiaCredencial");
                    _TecnologiasCredenciais.AppendChild(_TecnologiaCredencial);

                    XmlNode _TecnologiaCredencialID = _xmlDocument.CreateElement("TecnologiaCredencialID");
                    _TecnologiaCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TecnologiaCredencialID"].ToString())));
                    _TecnologiaCredencial.AppendChild(_TecnologiaCredencialID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _TecnologiaCredencial.AppendChild(_Descricao);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch
            {

                return null;
            }

        }

        private string RequisitaCredenciaisStatus()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseCredenciaisStatus = _xmlDocument.CreateElement("ClasseCredenciaisStatus");
                _xmlDocument.AppendChild(_ClasseCredenciaisStatus);

                XmlNode _CredenciaisStatus = _xmlDocument.CreateElement("CredenciaisStatus");
                _ClasseCredenciaisStatus.AppendChild(_CredenciaisStatus);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _strSql = "select * from CredenciaisStatus ";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _CredencialStatus = _xmlDocument.CreateElement("CredencialStatus");
                    _CredenciaisStatus.AppendChild(_CredencialStatus);

                    XmlNode _CredencialStatusID = _xmlDocument.CreateElement("CredencialStatusID");
                    _CredencialStatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialStatusID"].ToString())));
                    _CredencialStatus.AppendChild(_CredencialStatusID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _CredencialStatus.AppendChild(_Descricao);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch
            {

                return null;
            }

        }
        private void InsereVeiculoCredencialBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseVeiculosCredenciais.VeiculoCredencial _VeiculoCredencial = new ClasseVeiculosCredenciais.VeiculoCredencial();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _VeiculoCredencial.VeiculoCredencialID = _xmlDoc.GetElementsByTagName("VeiculoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoCredencialID")[i].InnerText);
                //_VeiculoCredencial.VeiculoID = _xmlDoc.GetElementsByTagName("VeiculoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoID")[i].InnerText);
                //_VeiculoCredencial.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
                _VeiculoCredencial.VeiculoEmpresaID = _xmlDoc.GetElementsByTagName("VeiculoEmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoEmpresaID")[i].InnerText);
                //_VeiculoCredencial.EmpresaContratoID = _xmlDoc.GetElementsByTagName("EmpresaContratoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaContratoID")[i].InnerText);
                _VeiculoCredencial.TecnologiaCredencialID = _xmlDoc.GetElementsByTagName("TecnologiaCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TecnologiaCredencialID")[i].InnerText);
                _VeiculoCredencial.TipoCredencialID = _xmlDoc.GetElementsByTagName("TipoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoCredencialID")[i].InnerText);
                _VeiculoCredencial.LayoutCrachaID = _xmlDoc.GetElementsByTagName("LayoutCrachaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("LayoutCrachaID")[i].InnerText);
                _VeiculoCredencial.FormatoCredencialID = _xmlDoc.GetElementsByTagName("FormatoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FormatoCredencialID")[i].InnerText);
                _VeiculoCredencial.NumeroCredencial = _xmlDoc.GetElementsByTagName("NumeroCredencial")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroCredencial")[i].InnerText;
                _VeiculoCredencial.FC = _xmlDoc.GetElementsByTagName("FC")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FC")[i].InnerText);
                //var teste = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText;
                _VeiculoCredencial.Emissao = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Emissao")[i].InnerText);
                _VeiculoCredencial.Validade = _xmlDoc.GetElementsByTagName("Validade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Validade")[i].InnerText);
                _VeiculoCredencial.CredencialStatusID = _xmlDoc.GetElementsByTagName("CredencialStatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CredencialStatusID")[i].InnerText);
                _VeiculoCredencial.CardHolderGuid = _xmlDoc.GetElementsByTagName("CardHolderGuid")[i].InnerText == "" ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(_xmlDoc.GetElementsByTagName("CardHolderGuid")[i].InnerText);
                _VeiculoCredencial.CredencialGuid = _xmlDoc.GetElementsByTagName("CredencialGuid")[i].InnerText == "" ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(_xmlDoc.GetElementsByTagName("CredencialGuid")[i].InnerText);



                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_VeiculoCredencial.VeiculoCredencialID != 0)
                {

                    _sqlCmd = new SqlCommand("Update VeiculosCredenciais Set " +
                            //" VeiculoID=@v1" +
                            "CardHolderGUID=@v2" +
                            ",CredencialGUID=@v3" +
                            ",TipoCredencialID=@v4" +
                            ",TecnologiaCredencialID=@v5" +
                            ",LayoutCrachaID=@v6" +
                            ",FormatoCredencialID=@v7" +
                            ",NumeroCredencial=@v8" +
                            ",FC=@v9" +
                            ",Emissao=@v10" +
                            ",Validade=@v11" +
                            ",CredencialStatusID=@v12" +
                            ",VeiculoEmpresaID=@v13" +
                            " Where VeiculoCredencialID = @v0", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoCredencialID;
                    //_sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.UniqueIdentifier).Value = _VeiculoCredencial.CardHolderGuid;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.UniqueIdentifier).Value = _VeiculoCredencial.CredencialGuid;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.Int).Value = _VeiculoCredencial.TipoCredencialID;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _VeiculoCredencial.TecnologiaCredencialID;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _VeiculoCredencial.LayoutCrachaID;
                    _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _VeiculoCredencial.FormatoCredencialID;
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _VeiculoCredencial.NumeroCredencial;
                    _sqlCmd.Parameters.Add("@V9", SqlDbType.Int).Value = _VeiculoCredencial.FC;
                    if (_VeiculoCredencial.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _VeiculoCredencial.Emissao;
                    }

                    if (_VeiculoCredencial.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _VeiculoCredencial.Validade;
                    }

                    _sqlCmd.Parameters.Add("@V12", SqlDbType.Int).Value = _VeiculoCredencial.CredencialStatusID;
                    _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoEmpresaID;
                }
                else
                {
                    //VeiculoID,EmpresaID,EmpresaContratoID,
                    _sqlCmd = new SqlCommand("Insert into VeiculosCredenciais(CardHolderGUID,CredencialGUID,TipoCredencialID,TecnologiaCredencialID,LayoutCrachaID,FormatoCredencialID,NumeroCredencial,FC," +
                            "Emissao,Validade,CredencialStatusID,VeiculoEmpresaID) " +
                            "values (@V2,@V3@V4,@V5,@V6,@V7,@V8,@V9,@V10,@V11,@V12,@v13)", _Con);

                    //_sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.UniqueIdentifier).Value = _VeiculoCredencial.CardHolderGuid;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.UniqueIdentifier).Value = _VeiculoCredencial.CredencialGuid;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.Int).Value = _VeiculoCredencial.TipoCredencialID;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _VeiculoCredencial.TecnologiaCredencialID;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _VeiculoCredencial.LayoutCrachaID;
                    _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _VeiculoCredencial.FormatoCredencialID;
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _VeiculoCredencial.NumeroCredencial;
                    _sqlCmd.Parameters.Add("@V9", SqlDbType.Int).Value = _VeiculoCredencial.FC;
                    if (_VeiculoCredencial.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _VeiculoCredencial.Emissao;
                    }

                    if (_VeiculoCredencial.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _VeiculoCredencial.Validade;
                    }

                    _sqlCmd.Parameters.Add("@V12", SqlDbType.Int).Value = _VeiculoCredencial.CredencialStatusID;
                    _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoEmpresaID;
                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereVeiculoCredencialBD ex: " + ex);


            }
        }

        private void InsereImpressaoDB(int veiculoCredencialID)
        {
            try
            {
                if (veiculoCredencialID != 0)
                {
                    SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                    SqlCommand _sqlCmd;


                    _sqlCmd = new SqlCommand("Insert into VeiculosCredenciaisImpressoes(VeiculoCredencialID) values (@V1)", _Con);

                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = veiculoCredencialID;

                    _sqlCmd.ExecuteNonQuery();

                    _Con.Close();
                }
            }
            catch (Exception ex)
            {

                Global.Log("Erro na void InsereImpressaoDB ex: " + ex);
            }
        }

        private void ExcluiVeiculoCredencialBD(int _VeiculoCredencialID) // alterar para xml
        {
            try
            {


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from VeiculosCredenciais where VeiculoCredencialID=" + _VeiculoCredencialID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiVeiculoCredencialBD ex: " + ex);


            }
        }

        private DateTime validadeCursoContrato(int _veiculo = 0)
        {
            try
            {

                //DateTime _menorDataCurso = Convert.ToDateTime("01-01-2999");
                //DateTime _menorDataContrato = Convert.ToDateTime("01-01-2999");

                string _menorDataCurso = "";
                string _menorDataContrato = "";

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _strSql = "SELECT dbo.Veiculos.VeiculoID, dbo.Veiculos.Nome,CONVERT(datetime, dbo.VeiculosCursos.Validade, 103) " +
                                 "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.VeiculosCursos.Validade, 103)) AS Dias FROM dbo.Veiculos " +
                                 "INNER JOIN dbo.VeiculosCursos ON dbo.Veiculos.VeiculoID = dbo.VeiculosCursos.VeiculoID where dbo.Veiculos.Excluida = 0 And dbo.VeiculosCursos.Controlado = 1 And dbo.VeiculosCursos.VeiculoID = " + _veiculo + " Order By Dias";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                if (_sqlreader.Read())
                {

                    //if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
                    //{
                    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _menorDataCurso = _sqlreader["ValidadeCurso"].ToString();
                    // break;
                    //}

                }
                _sqlreader.Close();


                //_strSql = "SELECT dbo.Veiculos.VeiculoID, dbo.Veiculos.Nome, dbo.EmpresasContratos.EmpresaID, dbo.EmpresasContratos.NumeroContrato, " +
                //          "CONVERT(datetime,dbo.EmpresasContratos.Validade,103) as DataContrato, DATEDIFF ( DAY , GETDATE(),  CONVERT(datetime, dbo.EmpresasContratos.Validade,103))  AS Dias " +
                //          "FROM  dbo.EmpresasContratos INNER JOIN dbo.VeiculosCredenciais ON dbo.EmpresasContratos.EmpresaID = dbo.VeiculosCredenciais.EmpresaID INNER JOIN dbo.Veiculos " +
                //          "ON dbo.VeiculosCredenciais.VeiculoID = dbo.Veiculos.VeiculoID WHERE (dbo.Veiculos.Excluida = 0) And dbo.Veiculos.VeiculoID = " + _veiculo + " Order By Dias";

                //_sqlcmd = new SqlCommand(_strSql, _Con);
                //_sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                //if (_sqlreader.Read())
                //{

                //    // if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
                //    //{
                //    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    _menorDataContrato = _sqlreader["DataContrato"].ToString();
                //    // break;
                //    //}

                //}
                //_sqlreader.Close();



                //if (Convert.ToDateTime(_menorDataCurso) < Convert.ToDateTime(_menorDataContrato))
                //{
                return Convert.ToDateTime(_menorDataCurso);
                //}
                //else if (Convert.ToDateTime(_menorDataCurso) > Convert.ToDateTime(_menorDataContrato))
                //{
                //    return Convert.ToDateTime(_menorDataContrato);
                //}

                //return DateTime.Now;

            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }
        #endregion

        #region Metodos Privados
        public void OnVincularCommand()
        {
            try
            {
                //CarregaColeçãoVinculos(VeiculoCredencialSelecionado.VeiculoCredencialID);

                //bool _resposta = SCManager.Vincular(Vinculos[0]);
                //if (_resposta)
                //{
                //    Global.PopupBox("Vinculo Efetuado com Sucesso!", 1);
                //}
            }
            catch (Exception ex)
            {
            }
        }


        public void OnImprimirCommand()
        {
            try
            {

                if (!Global.PopupBox("Confirma Impressão da Credencial para " + VeiculoCredencialSelecionado.VeiculoNome, 2))
                {
                    return;
                }

                //CarregaColeçãoVinculos(VeiculoCredencialSelecionado.VeiculoCredencialID);

                bool _resposta = SCManager.ImprimirCredencialVeiculo(VeiculoCredencialSelecionado);
                if (_resposta)
                {
                    InsereImpressaoDB(VeiculoCredencialSelecionado.VeiculoCredencialID);
                    Global.PopupBox("Impressão Efetuada com Sucesso!", 1);

                }

            }
            catch (Exception ex)
            {
            }
        }


        #endregion
    }
}
