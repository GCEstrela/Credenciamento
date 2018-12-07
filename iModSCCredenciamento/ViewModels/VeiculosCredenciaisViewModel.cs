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
using CrystalDecisions.CrystalReports.Engine;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using AutoMapper;

namespace iModSCCredenciamento.ViewModels
{
    public class VeiculosCredenciaisViewModel : ViewModelBase
    {
        private readonly IVeiculoCredencialService _repositorio = new VeiculoCredencialService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        #region Inicializacao
        public VeiculosCredenciaisViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();

        }

        private void CarregaUI()
        {
            //CarregaColecaoEmpresas();
            CarregaColecaoAreasAcessos();
            CarregaColeçãoFormatosCredenciais();
            CarregaColecaoTiposCredenciais();
            CarregaColecaoCredenciaisStatus();
            CarregaColecaoTecnologiasCredenciais();
            CarregaColecaoCredenciaisMotivos();

            CarregaColecaoVeiculosPrivilegios();

             CarregaColecaoVeiculosCredenciais();
        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseTiposCombustiveis.TipoCombustivel> _TiposCombustiveis;
        private ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculosCredenciais;

        private ObservableCollection<ClasseVinculos.Vinculo> _Vinculos;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;

        private ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> _FormatosCredenciais;

        private ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> _EmpresasLayoutsCrachas;

        private ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> _Contratos;

        private ClasseVeiculosCredenciais.VeiculoCredencial _VeiculoCredencialSelecionado;

        private ClasseVeiculosCredenciais.VeiculoCredencial _VeiculoCredencialTemp = new ClasseVeiculosCredenciais.VeiculoCredencial();

        private List<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculosCredenciaisTemp = new List<ClasseVeiculosCredenciais.VeiculoCredencial>();

        private ObservableCollection<ClasseVeiculosPrivilegios.VeiculoPrivilegio> _VeiculosPrivilegios;

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
        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;
        private ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo> _CredenciaisMotivos;

        #endregion

        #region Contrutores
       
        public ObservableCollection<ClasseAreasAcessos.AreaAcesso> AreasAcessos
        {
            get
            {
                return _AreasAcessos;
            }

            set
            {
                if (_AreasAcessos != value)
                {
                    _AreasAcessos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo> CredenciaisMotivos
        {
            get
            {
                return this._CredenciaisMotivos;
            }
            set
            {
                if (_CredenciaisMotivos != value)
                {
                    _CredenciaisMotivos = value;
                    OnPropertyChanged();

                }
            }
        }
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
                    CarregaColecaoVeiculosEmpresas(VeiculoCredencialSelecionado.VeiculoID);
                    CarregaColeçãoEmpresasLayoutsCrachas(VeiculoCredencialSelecionado.EmpresaID);
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
        public ObservableCollection<ClasseVeiculosPrivilegios.VeiculoPrivilegio> VeiculosPrivilegios
        {
            get
            {
                return _VeiculosPrivilegios;
            }

            set
            {
                if (_VeiculosPrivilegios != value)
                {
                    _VeiculosPrivilegios = value;
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
                    CarregaColecaoEmpresas(VeiculoSelecionadaID);
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
                CarregaColecaoEmpresas(VeiculoSelecionadaID);
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
            //try
            //{

            //    string _xml = RequisitaVeiculosCredenciaisNovos(VeiculoCredencialSelecionado.VeiculoEmpresaID);

            //    XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradoresCredenciais));

            //    XmlDocument xmldocument = new XmlDocument();
            //    xmldocument.LoadXml(_xml);

            //    TextReader reader = new StringReader(_xml);
            //    ClasseVeiculosCredenciais classeVeiculosCredenciais = new ClasseVeiculosCredenciais();
            //    classeVeiculosCredenciais = (ClasseVeiculosCredenciais)deserializer.Deserialize(reader);
            //    VeiculoCredencialSelecionado.Cargo = classeVeiculosCredenciais.VeiculosCredenciais[0].Cargo;
            //    VeiculoCredencialSelecionado.CNPJ = classeVeiculosCredenciais.VeiculosCredenciais[0].CNPJ;
            //    VeiculoCredencialSelecionado.ColaboradorApelido = classeVeiculosCredenciais.VeiculosCredenciais[0].ColaboradorApelido;
            //    VeiculoCredencialSelecionado.VeiculoEmpresaID = classeVeiculosCredenciais.VeiculosCredenciais[0].VeiculoEmpresaID;
            //    VeiculoCredencialSelecionado.VeiculoFoto = classeVeiculosCredenciais.VeiculosCredenciais[0].VeiculoFoto;
            //    VeiculoCredencialSelecionado.VeiculoID = classeVeiculosCredenciais.VeiculosCredenciais[0].VeiculoID;
            //    VeiculoCredencialSelecionado.VeiculoNome = classeVeiculosCredenciais.VeiculosCredenciais[0].VeiculoNome;
            //    VeiculoCredencialSelecionado.ContratoDescricao = classeVeiculosCredenciais.VeiculosCredenciais[0].ContratoDescricao;
            //    VeiculoCredencialSelecionado.Placa = classeVeiculosCredenciais.VeiculosCredenciais[0].Placa;
            //    VeiculoCredencialSelecionado.EmpresaApelido = classeVeiculosCredenciais.VeiculosCredenciais[0].EmpresaApelido;
            //    VeiculoCredencialSelecionado.EmpresaID = classeVeiculosCredenciais.VeiculosCredenciais[0].EmpresaID;
            //    VeiculoCredencialSelecionado.EmpresaLogo = classeVeiculosCredenciais.VeiculosCredenciais[0].EmpresaLogo;
            //    VeiculoCredencialSelecionado.EmpresaNome = classeVeiculosCredenciais.VeiculosCredenciais[0].EmpresaNome;
            //    VeiculoCredencialSelecionado.Motorista = classeVeiculosCredenciais.VeiculosCredenciais[0].Motorista;

            //    VeiculoCredencialSelecionado.FormatoCredencialDescricao = FormatosCredenciais.First(i => i.FormatoCredencialID == VeiculoCredencialSelecionado.FormatoCredencialID).Descricao;
            //    VeiculoCredencialSelecionado.LayoutCrachaGUID = EmpresasLayoutsCrachas.First(i => i.LayoutCrachaID == VeiculoCredencialSelecionado.LayoutCrachaID).LayoutCrachaGUID;

            //    VeiculoCredencialSelecionado.PrivilegioDescricao1 = VeiculosPrivilegios.First(i => i.VeiculoPrivilegioID == VeiculoCredencialSelecionado.VeiculoPrivilegio1ID).Descricao;
            //    VeiculoCredencialSelecionado.PrivilegioDescricao2 = VeiculosPrivilegios.First(i => i.VeiculoPrivilegioID == VeiculoCredencialSelecionado.VeiculoPrivilegio2ID).Descricao;



            //    VeiculoCredencialSelecionado.Validade = (DateTime?)VerificarMenorData(VeiculoCredencialSelecionado.VeiculoID);

            //    var _index = SelectedIndex;

            //    //bool _resposta = SCManager.Vincular(VeiculoCredencialSelecionado);


            //    if (!VeiculoCredencialSelecionado.Ativa)
            //    {
            //        VeiculoCredencialSelecionado.Baixa = DateTime.Now;
            //    }
            //    else
            //    {
            //        VeiculoCredencialSelecionado.Baixa = null;
            //    }


            //    HabilitaEdicao = false;
            //    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosCredenciais));

            //    ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial> _VeiculoCredencialPro = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
            //    ClasseVeiculosCredenciais _ClasseVeiculosEmpresasPro = new ClasseVeiculosCredenciais();
            //    _VeiculoCredencialPro.Add(VeiculoCredencialSelecionado);
            //    _ClasseVeiculosEmpresasPro.VeiculosCredenciais = _VeiculoCredencialPro;



            //    string xmlString;

            //    using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
            //    {

            //        using (XmlTextWriter xw = new XmlTextWriter(sw))
            //        {
            //            xw.Formatting = Formatting.Indented;
            //            serializer.Serialize(xw, _ClasseVeiculosEmpresasPro);
            //            xmlString = sw.ToString();
            //        }

            //    }
            //    //int _colaboradorCredencialID = InsereColaboradorCredencialBD(xmlString);

            //    int _colaboradorCredencialID = InsereVeiculoCredencialBD(xmlString);

            //    CarregaColecaoVeiculosCredenciais(VeiculoSelecionadaID);

            //    SelectedIndex = _index;

            //}
            //catch (Exception ex)
            //{
            //}
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
                    //if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    //{
                    //    if (SCManager.ExcluirCredencial(VeiculoCredencialSelecionado.CredencialGuid))
                    //    {
                    //        ExcluiVeiculoCredencialBD(VeiculoCredencialSelecionado.VeiculoCredencialID);
                    //        VeiculosCredenciais.Remove(VeiculoCredencialSelecionado);
                    //    }
                    //    else
                    //    {
                    //        Global.PopupBox("Não foi possível excluir esta credencial. Verifique no Gerenciador de Credenciais do Controle de Acesso.", 4);
                    //    }

                    //}
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
        private void CarregaColecaoVeiculosCredenciais(int veiculoID = 0, string empresaNome = "", int status = 0, string _validade = "")
        {
            try
            {
                //string _xml = RequisitaVeiculosCredenciais(veiculoID, empresaNome, status, _validade);

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVeiculosCredenciais));

                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);

                //TextReader reader = new StringReader(_xml);
                //ClasseVeiculosCredenciais classeVeiculosCredenciais = new ClasseVeiculosCredenciais();
                //classeVeiculosCredenciais = (ClasseVeiculosCredenciais)deserializer.Deserialize(reader);
                //VeiculosCredenciais = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
                //VeiculosCredenciais = classeVeiculosCredenciais.VeiculosCredenciais;
                //SelectedIndex = -1;

                ////////////////////////////////////////

                var service = new IMOD.Application.Service.VeiculoCredencialService();
                if (!string.IsNullOrWhiteSpace(empresaNome)) empresaNome = $"%{empresaNome}%";
                //if (!int.IsNullOrWhiteSpace(status)) status = $"%{status}%";
                //if (!string.IsNullOrWhiteSpace(_validade)) _validade = $"%{_validade}%";
                var list1 = service.Listar(veiculoID, 0, empresaNome, status, _validade);

                var list2 = Mapper.Map<List<ClasseVeiculosCredenciais.VeiculoCredencial>>(list1);

                var observer = new ObservableCollection<ClasseVeiculosCredenciais.VeiculoCredencial>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.VeiculosCredenciais = observer;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        private void CarregaColecaoVeiculosPrivilegios()
        {
            try
            {
                VeiculosPrivilegios = new ObservableCollection<ClasseVeiculosPrivilegios.VeiculoPrivilegio>();
                foreach (ClasseAreasAcessos.AreaAcesso _areaaAcesso in AreasAcessos)
                {
                    VeiculosPrivilegios.Add(new ClasseVeiculosPrivilegios.VeiculoPrivilegio() { VeiculoPrivilegioID = _areaaAcesso.AreaAcessoID, Descricao = _areaaAcesso.Identificacao });

                }

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                var list1 = _auxiliaresService.AreaAcessoService.Listar();
                var list2 = Mapper.Map<List<ClasseAreasAcessos.AreaAcesso>>(list1);

                var observer = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.AreasAcessos = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        private void CarregaColecaoEmpresas(int idEmpresa = 0, string nome = "", string apelido = "", string cnpj = "", string _quantidaderegistro = "500")
        {
            try
            {
                var service = new IMOD.Application.Service.EmpresaService();
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(cnpj)) cnpj = $"%{cnpj}%";

                var list1 = service.Listar(idEmpresa, nome, apelido, cnpj);
                var list2 = Mapper.Map<List<ClasseEmpresas.Empresa>>(list1);

                var observer = new ObservableCollection<ClasseEmpresas.Empresa>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Empresas = observer;
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        private void CarregaColecaoVeiculosEmpresas(int veiculoID)
        {
            try
            {
                //string _xml = RequisitaVeiculosEmpresas(_veiculoID);

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVeiculosEmpresas));

                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);

                //TextReader reader = new StringReader(_xml);
                //ClasseVeiculosEmpresas classeVeiculosEmpresas = new ClasseVeiculosEmpresas();
                //classeVeiculosEmpresas = (ClasseVeiculosEmpresas)deserializer.Deserialize(reader);
                //VeiculosEmpresas = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>();
                //VeiculosEmpresas = classeVeiculosEmpresas.VeiculosEmpresas;
                //SelectedIndex = -1;
                ////CarregaColeçãoEmpresasLayoutsCrachas(empresaID);

                var service = new IMOD.Application.Service.EmpresaService();
                //if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                //if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                //if (!string.IsNullOrWhiteSpace(cnpj)) cnpj = $"%{cnpj}%";

                var list1 = service.Listar(veiculoID);
                var list2 = Mapper.Map<List<ClasseVeiculosEmpresas.VeiculoEmpresa>>(list1);

                var observer = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.VeiculosEmpresas = observer;
                SelectedIndex = 0;


            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void CarregaColeçãoEmpresasLayoutsCrachas(int _colaboradorEmpresaID = 0)
        {

            try
            {
                var list1 = _auxiliaresService.LayoutCrachaService.Listar();
                var list2 = Mapper.Map<List<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>>(list1);

                var observer = new ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.EmpresasLayoutsCrachas = observer;
                SelectedIndex = 0;
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
                var list1 = _auxiliaresService.FormatoCredencialService.Listar();
                var list2 = Mapper.Map<List<ClasseFormatosCredenciais.FormatoCredencial>>(list1);

                var observer = new ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.FormatosCredenciais = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        public void CarregaColeçãoVinculos(int _ColaboradorCredencialID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaVinculos(_ColaboradorCredencialID);

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

        private void CarregaColecaoTiposCredenciais()
        {
            try
            {

                var list1 = _auxiliaresService.TipoCredencialService.Listar();
                var list2 = Mapper.Map<List<ClasseTiposCredenciais.TipoCredencial>>(list1);

                var observer = new ObservableCollection<ClasseTiposCredenciais.TipoCredencial>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.TiposCredenciais = observer;
                SelectedIndex = 0;

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

                var list1 = _auxiliaresService.TecnologiaCredencialService.Listar();
                var list2 = Mapper.Map<List<ClasseTecnologiasCredenciais.TecnologiaCredencial>>(list1);

                var observer = new ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.TecnologiasCredenciais = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoTecnologiasCredenciais ex: " + ex);
            }

        }

        private void CarregaColecaoCredenciaisStatus()
        {
            try
            {

                var list1 = _auxiliaresService.CredencialStatusService.Listar();
                var list2 = Mapper.Map<List<ClasseCredenciaisStatus.CredencialStatus>>(list1);

                var observer = new ObservableCollection<ClasseCredenciaisStatus.CredencialStatus>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.CredenciaisStatus = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoCredenciaisStatus ex: " + ex);
            }

        }

        public void CarregaColecaoCredenciaisMotivos(int tipo = 0)
        {
            try
            {

                var list1 = _auxiliaresService.CredencialMotivoService.Listar();
                var list2 = Mapper.Map<List<ClasseCredenciaisMotivos.CredencialMotivo>>(list1);

                var observer = new ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.CredenciaisMotivos = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoCredenciaisStatus ex: " + ex);
            }

        }


        #endregion


        //#region Data Access
        //private string RequisitaCredencial(int _colaboradorCredencialID)
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseCredencial = _xmlDocument.CreateElement("ClasseCredencial");
        //        _xmlDocument.AppendChild(_ClasseCredencial);


        //        string _strSql;

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        _strSql = "SELECT dbo.ColaboradoresCredenciais.ColaboradorCredencialID, dbo.ColaboradoresCredenciais.Colete, dbo.ColaboradoresCredenciais.Emissao," +
        //            " dbo.ColaboradoresCredenciais.Validade, dbo.ColaboradoresEmpresas.Matricula, dbo.ColaboradoresEmpresas.Cargo, dbo.Empresas.Nome AS EmpresaNome," +
        //            " dbo.Empresas.Apelido AS EmpresaApelido, dbo.Empresas.CNPJ, dbo.Empresas.Sigla, dbo.Empresas.Logo, dbo.Colaboradores.Nome AS ColaboradorNome," +
        //            " dbo.Colaboradores.Apelido AS ColaboradorApelido, dbo.Colaboradores.CPF, dbo.Colaboradores.RG, dbo.Colaboradores.RGOrgLocal," +
        //            " dbo.Colaboradores.RGOrgUF, dbo.Colaboradores.TelefoneEmergencia, dbo.Colaboradores.Foto, AreasAcessos_1.Identificacao AS Identificacao1," +
        //            " dbo.AreasAcessos.Identificacao AS Identificacao2, dbo.Colaboradores.CNHCategoria,dbo.LayoutsCrachas.LayoutRPT " +
        //            " FROM  dbo.AreasAcessos AS AreasAcessos_1 INNER JOIN dbo.ColaboradoresCredenciais ON" +
        //            " AreasAcessos_1.AreaAcessoID = dbo.ColaboradoresCredenciais.ColaboradorPrivilegio1ID INNER JOIN dbo.AreasAcessos ON" +
        //            " dbo.ColaboradoresCredenciais.ColaboradorPrivilegio2ID = dbo.AreasAcessos.AreaAcessoID INNER JOIN dbo.Colaboradores INNER JOIN" +
        //            " dbo.ColaboradoresEmpresas ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresEmpresas.ColaboradorID INNER JOIN " +
        //            "dbo.Empresas ON dbo.ColaboradoresEmpresas.EmpresaID = dbo.Empresas.EmpresaID ON" +
        //            " dbo.ColaboradoresCredenciais.ColaboradorEmpresaID = dbo.ColaboradoresEmpresas.ColaboradorEmpresaID INNER JOIN dbo.LayoutsCrachas ON" +
        //            " dbo.ColaboradoresCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID" +
        //            " WHERE(dbo.ColaboradoresCredenciais.ColaboradorCredencialID =" + _colaboradorCredencialID + ")";

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {


        //            XmlNode _ColaboradorCredencialID = _xmlDocument.CreateElement("ColaboradorCredencialID");
        //            _ColaboradorCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCredencialID"].ToString())));
        //            _ClasseCredencial.AppendChild(_ColaboradorCredencialID);


        //            XmlNode _Colete = _xmlDocument.CreateElement("Colete");
        //            _Colete.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Colete"].ToString())));
        //            _ClasseCredencial.AppendChild(_Colete);

        //            var dateStr = (_sqlreader["Emissao"].ToString());
        //            if (!string.IsNullOrWhiteSpace(dateStr))
        //            {
        //                var dt2 = Convert.ToDateTime(dateStr);
        //                XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
        //                _Emissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        //                _ClasseCredencial.AppendChild(_Emissao);
        //            }

        //            dateStr = (_sqlreader["Validade"].ToString());
        //            if (!string.IsNullOrWhiteSpace(dateStr))
        //            {
        //                var dt2 = Convert.ToDateTime(dateStr);
        //                XmlNode _Validade = _xmlDocument.CreateElement("Validade");
        //                _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        //                _ClasseCredencial.AppendChild(_Validade);
        //            }

        //            XmlNode _Matricula = _xmlDocument.CreateElement("Matricula");
        //            _Matricula.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Matricula"].ToString())));
        //            _ClasseCredencial.AppendChild(_Matricula);

        //            XmlNode _Cargo = _xmlDocument.CreateElement("Cargo");
        //            _Cargo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString())));
        //            _ClasseCredencial.AppendChild(_Cargo);

        //            XmlNode _EmpresaNome = _xmlDocument.CreateElement("EmpresaNome");
        //            _EmpresaNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
        //            _ClasseCredencial.AppendChild(_EmpresaNome);

        //            XmlNode _EmpresaApelido = _xmlDocument.CreateElement("EmpresaApelido");
        //            _EmpresaApelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaApelido"].ToString())));
        //            _ClasseCredencial.AppendChild(_EmpresaApelido);

        //            XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
        //            _CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim().FormatarCnpj())));
        //            _ClasseCredencial.AppendChild(_CNPJ);

        //            XmlNode _Sigla = _xmlDocument.CreateElement("Sigla");
        //            _Sigla.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Sigla"].ToString())));
        //            _ClasseCredencial.AppendChild(_Sigla);

        //            XmlNode _Logo = _xmlDocument.CreateElement("Logo");
        //            _Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString())));
        //            _ClasseCredencial.AppendChild(_Logo);

        //            XmlNode _ColaboradorNome = _xmlDocument.CreateElement("ColaboradorNome");
        //            _ColaboradorNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorNome"].ToString())));
        //            _ClasseCredencial.AppendChild(_ColaboradorNome);

        //            XmlNode _ColaboradorApelido = _xmlDocument.CreateElement("ColaboradorApelido");
        //            _ColaboradorApelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorApelido"].ToString())));
        //            _ClasseCredencial.AppendChild(_ColaboradorApelido);

        //            XmlNode _CPF = _xmlDocument.CreateElement("CPF");
        //            _CPF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().Trim().FormatarCpf())));
        //            _ClasseCredencial.AppendChild(_CPF);

        //            XmlNode _RG = _xmlDocument.CreateElement("RG");
        //            _RG.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RG"].ToString().Trim())));
        //            _ClasseCredencial.AppendChild(_RG);

        //            XmlNode _RGOrgLocal = _xmlDocument.CreateElement("RGOrgLocal");
        //            _RGOrgLocal.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgLocal"].ToString())));
        //            _ClasseCredencial.AppendChild(_RGOrgLocal);

        //            XmlNode _RGOrgUF = _xmlDocument.CreateElement("RGOrgUF");
        //            _RGOrgUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgUF"].ToString())));
        //            _ClasseCredencial.AppendChild(_RGOrgUF);

        //            XmlNode _TelefoneEmergencia = _xmlDocument.CreateElement("TelefoneEmergencia");
        //            _TelefoneEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneEmergencia"].ToString())));
        //            _ClasseCredencial.AppendChild(_TelefoneEmergencia);

        //            XmlNode _Foto = _xmlDocument.CreateElement("Foto");
        //            _Foto.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Foto"].ToString())));
        //            _ClasseCredencial.AppendChild(_Foto);

        //            XmlNode _Identificacao1 = _xmlDocument.CreateElement("Identificacao1");
        //            _Identificacao1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Identificacao1"].ToString())));
        //            _ClasseCredencial.AppendChild(_Identificacao1);

        //            XmlNode _Identificacao2 = _xmlDocument.CreateElement("Identificacao2");
        //            _Identificacao2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Identificacao2"].ToString().Trim())));
        //            _ClasseCredencial.AppendChild(_Identificacao2);

        //            XmlNode _CNHCategoria = _xmlDocument.CreateElement("CNHCategoria");
        //            _CNHCategoria.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHCategoria"].ToString().Trim())));
        //            _ClasseCredencial.AppendChild(_CNHCategoria);


        //            XmlNode _LayoutRPT = _xmlDocument.CreateElement("LayoutRPT");
        //            _LayoutRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutRPT"].ToString().Trim())));
        //            _ClasseCredencial.AppendChild(_LayoutRPT);


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

        //}

        private string RequisitaVeiculosCredenciais(int _veiculosID, string _empresaNome = "", int _status = 0, string _validade = "")//Possibilidade de criar a pesquisa por Matriculatambem
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


                _strSql = "SELECT dbo.LayoutsCrachas.Nome AS LayoutCrachaNome, dbo.FormatosCredenciais.Descricao AS FormatoCredencialDescricao, dbo.VeiculosCredenciais.NumeroCredencial, " +
                    "dbo.VeiculosCredenciais.FC, dbo.VeiculosCredenciais.Emissao, dbo.VeiculosCredenciais.Impressa, dbo.VeiculosCredenciais.Validade, dbo.VeiculosCredenciais.Baixa, " +
                    "dbo.VeiculosCredenciais.Ativa, dbo.VeiculosCredenciais.Colete, dbo.VeiculosCredenciais.CredencialmotivoID, dbo.VeiculosCredenciais.CredencialStatusID, " +
                    "dbo.VeiculosCredenciais.VeiculoEmpresaID, dbo.VeiculosCredenciais.TipoCredencialID, dbo.VeiculosCredenciais.TecnologiaCredencialID, dbo.VeiculosCredenciais.FormatoCredencialID, " +
                    "dbo.VeiculosCredenciais.LayoutCrachaID, dbo.VeiculosCredenciais.VeiculoCredencialID, dbo.Veiculos.Descricao AS VeiculoNome, dbo.Empresas.Nome AS EmpresaNome, " +
                    "dbo.EmpresasContratos.Descricao AS ContratoDescricao, dbo.VeiculosEmpresas.EmpresaID, dbo.VeiculosEmpresas.VeiculoID, dbo.VeiculosCredenciais.CardHolderGUID, " +
                    "dbo.VeiculosCredenciais.CredencialGUID, dbo.Veiculos.Foto AS VeiculoFoto, dbo.Veiculos.Placa_Identificador, dbo.LayoutsCrachas.LayoutCrachaGUID, dbo.FormatosCredenciais.FormatIDGUID, " +
                    "dbo.VeiculosCredenciais.VeiculoPrivilegio1ID, dbo.VeiculosCredenciais.VeiculoPrivilegio2ID, dbo.Empresas.Logo AS EmpresaLogo, dbo.Empresas.CNPJ, dbo.Empresas.Sigla AS EmpresaSigla, " +
                    "dbo.Empresas.Apelido AS EmpresaApelido FROM dbo.Empresas INNER JOIN dbo.VeiculosEmpresas ON dbo.Empresas.EmpresaID = dbo.VeiculosEmpresas.EmpresaID INNER JOIN dbo.EmpresasContratos " +
                    "ON dbo.VeiculosEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN dbo.VeiculosCredenciais INNER JOIN dbo.FormatosCredenciais ON " +
                    "dbo.VeiculosCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID ON dbo.VeiculosEmpresas.VeiculoEmpresaID = dbo.VeiculosCredenciais.VeiculoEmpresaID INNER JOIN " +
                    "dbo.Veiculos ON dbo.VeiculosEmpresas.VeiculoID = dbo.Veiculos.EquipamentoVeiculoID LEFT OUTER JOIN dbo.LayoutsCrachas ON dbo.VeiculosCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID " +
                    "WHERE dbo.VeiculosEmpresas.VeiculoID =" + _veiculosID + _statusSTR + _empresaNome + _validade +
                    " ORDER BY dbo.VeiculosCredenciais.VeiculoCredencialID DESC";

                //_strSql = "SELECT dbo.LayoutsCrachas.Nome AS LayoutCrachaNome, dbo.FormatosCredenciais.Descricao AS FormatoCredencialDescricao, dbo.Empresas.Nome AS EmpresaNome, " +
                //    " dbo.EmpresasContratos.Descricao AS ContratoDescricao, dbo.Empresas.Logo AS EmpresaLogo, dbo.Empresas.Sigla AS EmpresaSigla, dbo.Empresas.Apelido AS EmpresaApelido, " +
                //    " dbo.LayoutsCrachas.LayoutCrachaGUID, dbo.Empresas.CNPJ, dbo.FormatosCredenciais.FormatIDGUID dbo.Empresas INNER JOIN" +
                //    " dbo.VeiculosEmpresas ON dbo.Empresas.EmpresaID = dbo.VeiculosEmpresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON " +
                //    " dbo.VeiculosEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN dbo.VeiculosCredenciais INNER JOIN" +
                //    " dbo.FormatosCredenciais ON dbo.VeiculosCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID ON" +
                //    " dbo.VeiculosEmpresas.VeiculoEmpresaID = dbo.VeiculosCredenciais.VeiculoEmpresaID INNER JOIN dbo.Veiculos ON dbo.VeiculosEmpresas.VeiculoID = " +
                //    " dbo.Veiculos.VeiculoID LEFT OUTER JOIN dbo.LayoutsCrachas ON dbo.VeiculosCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID " +
                //    " WHERE dbo.VeiculosEmpresas.VeiculoID =" + _VeiculoID + _statusSTR + _empresaNome + _validade +
                //    " ORDER BY dbo.VeiculosCredenciais.VeiculoCredencialID DESC";




                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _VeiculoCredencial = _xmlDocument.CreateElement("VeiculoCredencial");
                    _VeiculosCredenciais.AppendChild(_VeiculoCredencial);

                    XmlNode _VeiculoCredencialID = _xmlDocument.CreateElement("VeiculoCredencialID");
                    _VeiculoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoCredencialID"].ToString())));
                    _VeiculoCredencial.AppendChild(_VeiculoCredencialID);

                    XmlNode _Ativa = _xmlDocument.CreateElement("Ativa");
                    _Ativa.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativa"])).ToString()));
                    _VeiculoCredencial.AppendChild(_Ativa);

                    XmlNode VeiculoPrivilegio1ID = _xmlDocument.CreateElement("VeiculoPrivilegio1ID");
                    VeiculoPrivilegio1ID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoPrivilegio1ID"].ToString())));
                    _VeiculoCredencial.AppendChild(VeiculoPrivilegio1ID);

                    XmlNode VeiculoPrivilegio2ID = _xmlDocument.CreateElement("VeiculoPrivilegio2ID");
                    VeiculoPrivilegio2ID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoPrivilegio2ID"].ToString())));
                    _VeiculoCredencial.AppendChild(VeiculoPrivilegio2ID);

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

                    XmlNode _VeiculoNome = _xmlDocument.CreateElement("VeiculoNome");
                    _VeiculoNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoNome"].ToString())));
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


                    XmlNode _Vinculo9 = _xmlDocument.CreateElement("Placa_Identificador");
                    _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Placa_Identificador"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo9);

                    //XmlNode _Vinculo22 = _xmlDocument.CreateElement("Motorista");
                    //_Vinculo22.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Motorista"])).ToString()));
                    //_VeiculoCredencial.AppendChild(_Vinculo22);

                    //XmlNode _Vinculo21 = _xmlDocument.CreateElement("VeiculoApelido");
                    //_Vinculo21.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoApelido"].ToString().Trim())));
                    //_VeiculoCredencial.AppendChild(_Vinculo21);

                    XmlNode _Vinculo11 = _xmlDocument.CreateElement("VeiculoFoto");
                    _Vinculo11.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoFoto"].ToString())));
                    _VeiculoCredencial.AppendChild(_Vinculo11);


                    XmlNode _Vinculo12 = _xmlDocument.CreateElement("EmpresaLogo");
                    _Vinculo12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaLogo"].ToString())));
                    _VeiculoCredencial.AppendChild(_Vinculo12);

                    XmlNode _Vinculo23 = _xmlDocument.CreateElement("EmpresaApelido");
                    _Vinculo23.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaApelido"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo23);

                    //XmlNode _Vinculo13 = _xmlDocument.CreateElement("Cargo");
                    //_Vinculo13.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString().Trim())));
                    //_VeiculoCredencial.AppendChild(_Vinculo13);


                    XmlNode _Vinculo15 = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _Vinculo15.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaGUID"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo15);

                    XmlNode _Vinculo19 = _xmlDocument.CreateElement("CNPJ");
                    _Vinculo19.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo19);

                    XmlNode _Vinculo20 = _xmlDocument.CreateElement("FormatIDGUID");
                    _Vinculo20.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatIDGUID"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Vinculo20);

                    XmlNode _Colete = _xmlDocument.CreateElement("Colete");
                    _Colete.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Colete"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_Colete);

                    XmlNode _EmpresaSigla = _xmlDocument.CreateElement("EmpresaSigla");
                    _EmpresaSigla.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaSigla"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_EmpresaSigla);

                    XmlNode _CredencialMotivoID = _xmlDocument.CreateElement("CredencialMotivoID");
                    _CredencialMotivoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialMotivoID"].ToString().Trim())));
                    _VeiculoCredencial.AppendChild(_CredencialMotivoID);

                    dateStr = (_sqlreader["Baixa"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Baixa = _xmlDocument.CreateElement("Baixa");
                        _Baixa.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _VeiculoCredencial.AppendChild(_Baixa);

                    }

                    XmlNode _Impressa = _xmlDocument.CreateElement("Impressa");
                    _Impressa.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Impressa"])).ToString().Trim()));
                    _VeiculoCredencial.AppendChild(_Impressa);
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

        //private string RequisitaVeiculosCredenciaisNovos(int _colaboradorEmpresaID)
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

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        _strSql = "SELECT dbo.Colaboradores.Nome AS ColaboradorNome, dbo.Empresas.Nome AS EmpresaNome, dbo.EmpresasContratos.Descricao AS ContratoDescricao," +
        //            " dbo.ColaboradoresEmpresas.EmpresaID, dbo.ColaboradoresEmpresas.ColaboradorID, dbo.Colaboradores.Foto AS ColaboradorFoto, dbo.Colaboradores.CPF," +
        //            " dbo.Colaboradores.Motorista, dbo.Colaboradores.Apelido AS ColaboradorApelido, dbo.Colaboradores.Nome, dbo.Empresas.Logo AS EmpresaLogo," +
        //            " dbo.Empresas.Apelido AS EmpresaApelido, dbo.ColaboradoresEmpresas.Cargo, dbo.Empresas.CNPJ, dbo.ColaboradoresEmpresas.ColaboradorEmpresaID" +
        //            " FROM dbo.Empresas INNER JOIN dbo.ColaboradoresEmpresas ON" +
        //            " dbo.Empresas.EmpresaID = dbo.ColaboradoresEmpresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON" +
        //            " dbo.ColaboradoresEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN dbo.Colaboradores ON" +
        //            " dbo.ColaboradoresEmpresas.ColaboradorID = dbo.Colaboradores.ColaboradorID WHERE dbo.ColaboradoresEmpresas.Ativo = 1 AND" +
        //            " dbo.ColaboradoresEmpresas.ColaboradorEmpresaID = " + _colaboradorEmpresaID;

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _ColaboradorCredencial = _xmlDocument.CreateElement("ColaboradorCredencial");
        //            _ColaboradoresCredenciais.AppendChild(_ColaboradorCredencial);

        //            //XmlNode _ColaboradorCredencialID = _xmlDocument.CreateElement("ColaboradorCredencialID");
        //            //_ColaboradorCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCredencialID"].ToString())));
        //            //_ColaboradorCredencial.AppendChild(_ColaboradorCredencialID);

        //            XmlNode _ColaboradorEmpresaID = _xmlDocument.CreateElement("ColaboradorEmpresaID");
        //            _ColaboradorEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorEmpresaID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorEmpresaID);

        //            XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
        //            _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorID);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("ContratoDescricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContratoDescricao"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_Descricao);

        //            XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
        //            _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_Empresa);

        //            XmlNode _ColaboradorNome = _xmlDocument.CreateElement("ColaboradorNome");
        //            _ColaboradorNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorNome"].ToString())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorNome);

        //            //XmlNode _CredencialStatusID = _xmlDocument.CreateElement("CredencialStatusID");
        //            //_CredencialStatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialStatusID"].ToString())));
        //            //_ColaboradorCredencial.AppendChild(_CredencialStatusID);

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

        //            XmlNode _Vinculo19 = _xmlDocument.CreateElement("CNPJ");
        //            _Vinculo19.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo19);

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



        private string RequisitaVinculos(int _ColaboradorCredencialID = 0)
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

                string _SQL = "SELECT dbo.ColaboradoresCredenciais.ColaboradorCredencialID,dbo.ColaboradoresCredenciais.CardHolderGuid,dbo.ColaboradoresCredenciais.CredencialGuid," +
                    " dbo.ColaboradoresEmpresas.ColaboradorID, dbo.Colaboradores.Nome AS ColaboradorNome," +
                    " dbo.Colaboradores.CPF,dbo.Colaboradores.Apelido AS ColaboradorApelido, dbo.Colaboradores.Motorista, dbo.Colaboradores.Foto, dbo.ColaboradoresEmpresas.EmpresaID," +
                    " dbo.Empresas.Nome AS EmpresaNome,dbo.Empresas.Apelido AS EmpresaApelido, dbo.Empresas.CNPJ, dbo.FormatosCredenciais.FormatIDGUID," +
                    " dbo.ColaboradoresCredenciais.NumeroCredencial, dbo.ColaboradoresCredenciais.FC,dbo.ColaboradoresCredenciais.Validade, dbo.LayoutsCrachas.LayoutCrachaGUID," +
                    " dbo.ColaboradoresEmpresas.Cargo, dbo.ColaboradoresEmpresas.Matricula, dbo.ColaboradoresEmpresas.Ativo FROM dbo.Empresas INNER JOIN" +
                    " dbo.Colaboradores INNER JOIN dbo.ColaboradoresCredenciais INNER JOIN dbo.LayoutsCrachas ON" +
                    " dbo.ColaboradoresCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID INNER JOIN dbo.FormatosCredenciais ON" +
                    " dbo.ColaboradoresCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN dbo.ColaboradoresEmpresas ON" +
                    " dbo.ColaboradoresCredenciais.ColaboradorEmpresaID = dbo.ColaboradoresEmpresas.ColaboradorEmpresaID ON" +
                    " dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresEmpresas.ColaboradorID ON dbo.Empresas.EmpresaID = dbo.ColaboradoresEmpresas.EmpresaID " +
                    "WHERE dbo.ColaboradoresCredenciais.ColaboradorCredencialID = " + _ColaboradorCredencialID;


                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Vinculo = _xmlDocument.CreateElement("Vinculo");
                    _Vinculos.AppendChild(_Vinculo);

                    XmlNode _Vinculo24 = _xmlDocument.CreateElement("ColaboradorID");
                    _Vinculo24.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["ColaboradorID"].ToString().Trim())));
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

                    XmlNode _Vinculo10 = _xmlDocument.CreateElement("ColaboradorNome");
                    _Vinculo10.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["ColaboradorNome"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo10);

                    XmlNode _Vinculo22 = _xmlDocument.CreateElement("Motorista");
                    _Vinculo22.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqldatareader["Motorista"])).ToString()));
                    _Vinculo.AppendChild(_Vinculo22);

                    XmlNode _Vinculo21 = _xmlDocument.CreateElement("ColaboradorApelido");
                    _Vinculo21.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["ColaboradorApelido"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo21);

                    XmlNode _Vinculo11 = _xmlDocument.CreateElement("ColaboradorFoto");
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


                _strSql = "SELECT dbo.ColaboradoresEmpresas.ColaboradorID, dbo.ColaboradoresEmpresas.EmpresaID, dbo.Empresas.Nome AS EmpresaNome," +
                    " dbo.ColaboradoresEmpresas.Ativo, dbo.EmpresasContratos.Descricao, dbo.ColaboradoresEmpresas.EmpresaContratoID FROM dbo.ColaboradoresEmpresas INNER JOIN dbo.Empresas" +
                    " ON dbo.ColaboradoresEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON" +
                    " dbo.ColaboradoresEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID WHERE(dbo.ColaboradoresEmpresas.Ativo = 1)" +
                    " AND(dbo.ColaboradoresEmpresas.ColaboradorID =" + VeiculoSelecionadaID + ") AND (dbo.ColaboradoresEmpresas.EmpresaID =" + _empresaID + ")";


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

       
 
   

        private int InsereVeiculoCredencialBD(string xmlString)
        {
            return 0;
            //int _novID = 0;
            //try
            //{


            //    System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

            //    _xmlDoc.LoadXml(xmlString);
            //    // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
            //    ClasseVeiculosCredenciais.VeiculoCredencial _VeiculoCredencial = new ClasseVeiculosCredenciais.VeiculoCredencial();
            //    //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
            //    //{
            //    int i = 0;

            //    _VeiculoCredencial.VeiculoCredencialID = _xmlDoc.GetElementsByTagName("VeiculoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoCredencialID")[i].InnerText);
            //    //_ColaboradorCredencial.ColaboradorID = _xmlDoc.GetElementsByTagName("ColaboradorID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorID")[i].InnerText);
            //    //_ColaboradorCredencial.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
            //    _VeiculoCredencial.VeiculoEmpresaID = _xmlDoc.GetElementsByTagName("VeiculoEmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoEmpresaID")[i].InnerText);
            //    //_ColaboradorCredencial.EmpresaContratoID = _xmlDoc.GetElementsByTagName("EmpresaContratoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaContratoID")[i].InnerText);
            //    _VeiculoCredencial.TecnologiaCredencialID = _xmlDoc.GetElementsByTagName("TecnologiaCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TecnologiaCredencialID")[i].InnerText);
            //    _VeiculoCredencial.TipoCredencialID = _xmlDoc.GetElementsByTagName("TipoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoCredencialID")[i].InnerText);
            //    _VeiculoCredencial.LayoutCrachaID = _xmlDoc.GetElementsByTagName("LayoutCrachaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("LayoutCrachaID")[i].InnerText);
            //    _VeiculoCredencial.FormatoCredencialID = _xmlDoc.GetElementsByTagName("FormatoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FormatoCredencialID")[i].InnerText);
            //    _VeiculoCredencial.NumeroCredencial = _xmlDoc.GetElementsByTagName("NumeroCredencial")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroCredencial")[i].InnerText;
            //    _VeiculoCredencial.FC = _xmlDoc.GetElementsByTagName("FC")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FC")[i].InnerText);
            //    //var teste = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText;
            //    _VeiculoCredencial.Emissao = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Emissao")[i].InnerText);
            //    _VeiculoCredencial.Validade = _xmlDoc.GetElementsByTagName("Validade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Validade")[i].InnerText);
            //    _VeiculoCredencial.CredencialStatusID = _xmlDoc.GetElementsByTagName("CredencialStatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CredencialStatusID")[i].InnerText);
            //    _VeiculoCredencial.CardHolderGuid = _xmlDoc.GetElementsByTagName("CardHolderGuid")[i].InnerText == "" ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(_xmlDoc.GetElementsByTagName("CardHolderGuid")[i].InnerText);
            //    _VeiculoCredencial.CredencialGuid = _xmlDoc.GetElementsByTagName("CredencialGuid")[i].InnerText == "" ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(_xmlDoc.GetElementsByTagName("CredencialGuid")[i].InnerText);
            //    _VeiculoCredencial.VeiculoPrivilegio1ID = _xmlDoc.GetElementsByTagName("VeiculoPrivilegio1ID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoPrivilegio1ID")[i].InnerText);
            //    _VeiculoCredencial.VeiculoPrivilegio2ID = _xmlDoc.GetElementsByTagName("VeiculoPrivilegio2ID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoPrivilegio2ID")[i].InnerText);
            //    bool _ativa;
            //    Boolean.TryParse(_xmlDoc.GetElementsByTagName("Ativa")[i].InnerText, out _ativa);
            //    _VeiculoCredencial.Ativa = _xmlDoc.GetElementsByTagName("Ativa")[i] == null ? false : _ativa;
            //    bool _Impressa;
            //    Boolean.TryParse(_xmlDoc.GetElementsByTagName("Impressa")[i].InnerText, out _Impressa);
            //    _VeiculoCredencial.Impressa = _xmlDoc.GetElementsByTagName("Impressa")[i] == null ? false : _Impressa;
            //    _VeiculoCredencial.Colete = _xmlDoc.GetElementsByTagName("Colete")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Colete")[i].InnerText;
            //    _VeiculoCredencial.CredencialMotivoID = _xmlDoc.GetElementsByTagName("CredencialMotivoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CredencialMotivoID")[i].InnerText);
            //    _VeiculoCredencial.Baixa = _xmlDoc.GetElementsByTagName("Baixa")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Baixa")[i].InnerText);

            //    SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

            //    SqlCommand _sqlCmd;
            //    if (_VeiculoCredencial.VeiculoCredencialID != 0)
            //    {

            //        _sqlCmd = new SqlCommand("Update VeiculosCredenciais Set " +
            //                " VeiculoPrivilegio1ID=@v1" +
            //                ",CardHolderGUID=@v2" +
            //                ",CredencialGUID=@v3" +
            //                ",TipoCredencialID=@v4" +
            //                ",TecnologiaCredencialID=@v5" +
            //                ",LayoutCrachaID=@v6" +
            //                ",FormatoCredencialID=@v7" +
            //                ",NumeroCredencial=@v8" +
            //                ",FC=@v9" +
            //                ",Emissao=@v10" +
            //                ",Validade=@v11" +
            //                ",CredencialStatusID=@v12" +
            //                ",VeiculoEmpresaID=@v13" +
            //                ",Ativa=@v14" +
            //                ",VeiculoPrivilegio2ID=@v15" +
            //                ",Colete=@v16" +
            //                ",CredencialMotivoID=@v17" +
            //                ",Baixa=@v18" +
            //                ",Impressa=@v19" +
            //                " Where VeiculoCredencialID = @v0", _Con);

            //        _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoCredencialID;
            //        _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoPrivilegio1ID;
            //        _sqlCmd.Parameters.Add("@V2", SqlDbType.UniqueIdentifier).Value = _VeiculoCredencial.CardHolderGuid;
            //        _sqlCmd.Parameters.Add("@V3", SqlDbType.UniqueIdentifier).Value = _VeiculoCredencial.CredencialGuid;
            //        _sqlCmd.Parameters.Add("@V4", SqlDbType.Int).Value = _VeiculoCredencial.TipoCredencialID;
            //        _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _VeiculoCredencial.TecnologiaCredencialID;
            //        _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _VeiculoCredencial.LayoutCrachaID;
            //        _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _VeiculoCredencial.FormatoCredencialID;
            //        _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _VeiculoCredencial.NumeroCredencial;
            //        _sqlCmd.Parameters.Add("@V9", SqlDbType.Int).Value = _VeiculoCredencial.FC;
            //        if (_VeiculoCredencial.Emissao == null)
            //        {
            //            _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
            //        }
            //        else
            //        {
            //            _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _VeiculoCredencial.Emissao;
            //        }

            //        if (_VeiculoCredencial.Validade == null)
            //        {
            //            _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
            //        }
            //        else
            //        {
            //            _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _VeiculoCredencial.Validade;
            //        }

            //        _sqlCmd.Parameters.Add("@V12", SqlDbType.Int).Value = _VeiculoCredencial.CredencialStatusID;
            //        _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoEmpresaID;
            //        _sqlCmd.Parameters.Add("@V14", SqlDbType.Bit).Value = _VeiculoCredencial.Ativa;
            //        _sqlCmd.Parameters.Add("@V15", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoPrivilegio2ID;
            //        _sqlCmd.Parameters.Add("@V16", SqlDbType.NVarChar).Value = _VeiculoCredencial.Colete;
            //        _sqlCmd.Parameters.Add("@V17", SqlDbType.Int).Value = _VeiculoCredencial.CredencialMotivoID;
            //        if (_VeiculoCredencial.Baixa == null)
            //        {
            //            _sqlCmd.Parameters.Add("@V18", SqlDbType.DateTime).Value = DBNull.Value;
            //        }
            //        else
            //        {
            //            _sqlCmd.Parameters.Add("@V18", SqlDbType.DateTime).Value = _VeiculoCredencial.Baixa;
            //        }
            //        _sqlCmd.Parameters.Add("@V19", SqlDbType.Bit).Value = _VeiculoCredencial.Impressa;

            //        _sqlCmd.ExecuteNonQuery();
            //        _novID = _VeiculoCredencial.VeiculoCredencialID;
            //    }
            //    else
            //    {
            //        //ColaboradorID,EmpresaID,EmpresaContratoID,
            //        _sqlCmd = new SqlCommand("Insert into VeiculosCredenciais(VeiculoPrivilegio1ID,CardHolderGUID,CredencialGUID," +
            //            "TipoCredencialID,TecnologiaCredencialID,LayoutCrachaID,FormatoCredencialID,NumeroCredencial,FC," +
            //                "Emissao,Validade,CredencialStatusID,VeiculoEmpresaID,Ativa,VeiculoPrivilegio2ID," +
            //                "Colete,CredencialMotivoID,Baixa,Impressa) " +
            //                "values (@V1,@V2,@V3,@V4,@V5,@V6,@V7,@V8,@V9,@V10,@V11,@V12,@v13,@V14,@V15,@V16,@V17,@V18,@V19);SELECT SCOPE_IDENTITY();", _Con);

            //        _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoPrivilegio1ID;
            //        _sqlCmd.Parameters.Add("@V2", SqlDbType.UniqueIdentifier).Value = _VeiculoCredencial.CardHolderGuid;
            //        _sqlCmd.Parameters.Add("@V3", SqlDbType.UniqueIdentifier).Value = _VeiculoCredencial.CredencialGuid;
            //        _sqlCmd.Parameters.Add("@V4", SqlDbType.Int).Value = _VeiculoCredencial.TipoCredencialID;
            //        _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _VeiculoCredencial.TecnologiaCredencialID;
            //        _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _VeiculoCredencial.LayoutCrachaID;
            //        _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _VeiculoCredencial.FormatoCredencialID;
            //        _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _VeiculoCredencial.NumeroCredencial;
            //        _sqlCmd.Parameters.Add("@V9", SqlDbType.Int).Value = _VeiculoCredencial.FC;
            //        if (_VeiculoCredencial.Emissao == null)
            //        {
            //            _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
            //        }
            //        else
            //        {
            //            _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _VeiculoCredencial.Emissao;
            //        }

            //        if (_VeiculoCredencial.Validade == null)
            //        {
            //            _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
            //        }
            //        else
            //        {
            //            _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _VeiculoCredencial.Validade;
            //        }

            //        _sqlCmd.Parameters.Add("@V12", SqlDbType.Int).Value = _VeiculoCredencial.CredencialStatusID;
            //        _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoEmpresaID;
            //        _sqlCmd.Parameters.Add("@V14", SqlDbType.Bit).Value = _VeiculoCredencial.Ativa;
            //        _sqlCmd.Parameters.Add("@V15", SqlDbType.Int).Value = _VeiculoCredencial.VeiculoPrivilegio2ID;
            //        _sqlCmd.Parameters.Add("@V16", SqlDbType.VarChar).Value = _VeiculoCredencial.Colete;
            //        _sqlCmd.Parameters.Add("@V17", SqlDbType.Int).Value = _VeiculoCredencial.CredencialMotivoID;

            //        if (_VeiculoCredencial.Baixa == null)
            //        {
            //            _sqlCmd.Parameters.Add("@V18", SqlDbType.DateTime).Value = DBNull.Value;
            //        }
            //        else
            //        {
            //            _sqlCmd.Parameters.Add("@V18", SqlDbType.DateTime).Value = _VeiculoCredencial.Baixa;
            //        }
            //        _sqlCmd.Parameters.Add("@V19", SqlDbType.Bit).Value = _VeiculoCredencial.Impressa;

            //        _novID = Convert.ToInt32(_sqlCmd.ExecuteScalar());
            //    }

            //    _Con.Close();


            //}
            //catch (Exception ex)
            //{
            //    Global.Log("Erro na void InsereColaboradorCredencialBD ex: " + ex);
            //}

            //return _novID;
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

                    _sqlCmd = new SqlCommand("Update VeiculosCredenciais Set Impressa=@v1" +
                            " Where VeiculoCredencialID = @v0", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = veiculoCredencialID;

                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Bit).Value = 1;

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
                _sqlCmd = new SqlCommand("Delete from VeiculosCredenciaisImpressoes where VeiculoCredencialID=" + _VeiculoCredencialID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiVeiculoCredencialBD ex: " + ex);


            }
        }

        private DateTime validadeCursoContrato(int _colaborador = 0)
        {
            try
            {

                //DateTime _menorDataCurso = Convert.ToDateTime("01-01-2999");
                //DateTime _menorDataContrato = Convert.ToDateTime("01-01-2999");

                string _menorDataCurso = "";
                string _menorDataContrato = "";

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome,CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) " +
                                 "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103)) AS Dias FROM dbo.Colaboradores " +
                                 "INNER JOIN dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID where dbo.Colaboradores.Excluida = 0 And dbo.ColaboradoresCursos.Controlado = 1 And dbo.ColaboradoresCursos.ColaboradorID = " + _colaborador + " Order By Dias";

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


                //_strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome, dbo.EmpresasContratos.EmpresaID, dbo.EmpresasContratos.NumeroContrato, " +
                //          "CONVERT(datetime,dbo.EmpresasContratos.Validade,103) as DataContrato, DATEDIFF ( DAY , GETDATE(),  CONVERT(datetime, dbo.EmpresasContratos.Validade,103))  AS Dias " +
                //          "FROM  dbo.EmpresasContratos INNER JOIN dbo.ColaboradoresCredenciais ON dbo.EmpresasContratos.EmpresaID = dbo.ColaboradoresCredenciais.EmpresaID INNER JOIN dbo.Colaboradores " +
                //          "ON dbo.ColaboradoresCredenciais.ColaboradorID = dbo.Colaboradores.ColaboradorID WHERE (dbo.Colaboradores.Excluida = 0) And dbo.Colaboradores.ColaboradorID = " + _colaborador + " Order By Dias";

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
            //try
            //{
            //    if (VeiculoCredencialSelecionado.Validade == null || !VeiculoCredencialSelecionado.Ativa ||
            //        VeiculoCredencialSelecionado.LayoutCrachaID == 0)
            //    {
            //        Global.PopupBox("Não é possível imprimir esta credencial!", 4);
            //        return;
            //    }

            //    //if (!Global.PopupBox("Confirma Impressão da Credencial para " + ColaboradorCredencialSelecionado.ColaboradorNome, 2))
            //    //{
            //    //    return;
            //    //}

            //    string _xml = RequisitaCredencial(VeiculoCredencialSelecionado.VeiculoCredencialID);

            //    XmlSerializer deserializer = new XmlSerializer(typeof(ClasseCredencial));

            //    XmlDocument xmldocument = new XmlDocument();
            //    xmldocument.LoadXml(_xml);

            //    TextReader reader = new StringReader(_xml);
            //    ClasseCredencial Credencial = new ClasseCredencial();
            //    Credencial = (ClasseCredencial)deserializer.Deserialize(reader);

            //    ;
            //    string _ArquivoRPT = System.IO.Path.GetRandomFileName();
            //    _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;
            //    _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
            //    byte[] buffer = Convert.FromBase64String(Credencial.LayoutRPT.Trim());

            //    System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

            //    ReportDocument reportDocument = new ReportDocument();

            //    reportDocument.Load(_ArquivoRPT);

            //    //var report = new Cracha();
            //    var x = new List<ClasseCredencial>();
            //    x.Add(Credencial);
            //    reportDocument.SetDataSource(x);

            //    //Thread CarregaCracha_thr = new Thread(() =>
            //    //{

            //    //    System.Windows.Application.Current.Dispatcher.Invoke(() =>
            //    //    {

            //    PopupCredencial _popupCredencial = new PopupCredencial(reportDocument);
            //    _popupCredencial.ShowDialog();

            //    bool _result = _popupCredencial.Result;
            //    //    });

            //    //}
            //    //);

            //    //CarregaCracha_thr.Start();

            //    // GenericReportViewer.ViewerCore.ReportSource = reportDocument;




            //    //bool _resposta = SCManager.ImprimirCredencial(ColaboradorCredencialSelecionado);
            //    //if (_resposta)
            //    //{
            //    if (_result)
            //    {
            //        System.IO.File.Delete(_ArquivoRPT);
            //        InsereImpressaoDB(VeiculoCredencialSelecionado.VeiculoCredencialID);
            //        // Global.PopupBox("Impressão Efetuada com Sucesso!", 1);
            //        VeiculoCredencialSelecionado.Impressa = true;
            //        int _selectindex = SelectedIndex;
            //        CarregaColecaoVeiculosCredenciais(VeiculoCredencialSelecionado.VeiculoID); //revisar a necessidade do carregamento
            //        SelectedIndex = _selectindex;
            //    }

            //    //}

            //}
            //catch (Exception ex)
            //{
            //}
        }

        private DateTime VerificarMenorData(int _colaborador)
        {
            try
            {

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                // Dim _dataValidade As DateTime = Date.Now
                DateTime _menorData = DateTime.Now;
                // Dim _menorDataContrato As String = ""


                var sqlSelectCurso = @"SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome, dbo.ColaboradoresCursos.Validade, dbo.ColaboradoresCursos.Controlado
                       FROM dbo.Colaboradores INNER JOIN
                         dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID
                       WHERE (dbo.Colaboradores.ColaboradorID = " + _colaborador + ") And (dbo.ColaboradoresCursos.Controlado = 1)";
                int idx = 0;
                // ---------------------------------------------------
                SqlCommand sqlcmd = new SqlCommand(sqlSelectCurso, _Con);
                SqlDataReader _sqlreader = sqlcmd.ExecuteReader(CommandBehavior.Default);
                var list = new List<Colaborador>();
                while (_sqlreader.Read()) // Popular dados
                {
                    list.Add(new Colaborador() { Id = idx, ColaboradorId = Convert.ToInt32(_sqlreader["ColaboradorID"].ToString()), DataValidade = Convert.ToDateTime(_sqlreader["Validade"].ToString()) });
                    idx = idx + 1;
                }

                var sqlSelectContrato = @"SELECT dbo.EmpresasContratos.EmpresaID, dbo.ColaboradoresEmpresas.EmpresaContratoID, dbo.ColaboradoresEmpresas.ColaboradorID,
                                    dbo.ColaboradoresEmpresas.Cargo, dbo.ColaboradoresEmpresas.Matricula, dbo.ColaboradoresCredenciais.NumeroCredencial,
                                    dbo.EmpresasContratos.Validade AS ValidadeContrato, dbo.ColaboradoresCredenciais.CredencialGUID, dbo.ColaboradoresEmpresas.Ativo
                                    FROM dbo.EmpresasContratos RIGHT OUTER JOIN dbo.ColaboradoresEmpresas ON dbo.EmpresasContratos.EmpresaContratoID = dbo.ColaboradoresEmpresas.EmpresaContratoID
                                    FULL OUTER JOIN dbo.ColaboradoresCredenciais ON dbo.ColaboradoresEmpresas.ColaboradorEmpresaID = dbo.ColaboradoresCredenciais.ColaboradorEmpresaID 
                                    WHERE dbo.ColaboradoresEmpresas.Ativo = 1 AND dbo.ColaboradoresEmpresas.ColaboradorID = " + _colaborador;

                SqlCommand sqlcmd2 = new SqlCommand(sqlSelectContrato, _Con);
                SqlDataReader _sqlreader2 = sqlcmd2.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader2.Read()) // Popular dados
                {
                    list.Add(new Colaborador() { Id = idx, CredencialGuidId = _sqlreader2["CredencialGUID"].ToString(), ColaboradorId = Convert.ToInt32(_sqlreader2["ColaboradorID"].ToString()), DataValidade = Convert.ToDateTime(_sqlreader2["ValidadeContrato"].ToString()) });
                    idx = idx + 1;
                }

                // group by colaboradores
                var colaboradoresGroup = list.GroupBy(n => n.ColaboradorId).Select(n => n.First()).ToList();
                // ----------------------------------------------------
                // Itera entre os colaboradores e retorna o item com a menor data
                foreach (Colaborador item in colaboradoresGroup)
                {
                    var d1 = (from d in list
                              where d.ColaboradorId == item.ColaboradorId
                              select d.DataValidade).Min();
                    var dados = list.Where(x => x.DataValidade.Equals(d1) & x.ColaboradorId.Equals(item.ColaboradorId)).FirstOrDefault();
                    _menorData = dados.DataValidade;
                }
                // ----------------------------------------------------
                return _menorData;
            }
            catch (Exception ex)
            {

            }

            return DateTime.Now;
        }
        #endregion
    }
}
