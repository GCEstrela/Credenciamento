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
using IMOD.Application.Service;
using IMOD.Application.Interfaces;
using IMOD.Domain.EntitiesCustom;
using AutoMapper;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradoresCredenciaisViewModel : ViewModelBase
    {
        
        private readonly IColaboradorService _repositorio = new ColaboradorService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        //private readonly ColaboradoresCredenciaisView _repositorio = new ColaboradoresCredenciaisViewService();
        Global g = new Global();

        #region Inicializacao
        public ColaboradoresCredenciaisViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();

        }

        private void CarregaUI()
        {
            //CarregaColecaoEmpresas();
            CarregaColecaoAreasAcessos();
            CarregaColecaoCredenciaisStatus();
            CarregaColecaoTiposCredenciais();
            CarregaColecaoTecnologiasCredenciais();
            CarregaColeçãoFormatosCredenciais();
            CarregaColecaoCredenciaisMotivos();

            CarregaColecaoColaboradoresPrivilegios();

        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial> _ColaboradoresCredenciais;

        private ObservableCollection<ClasseVinculos.Vinculo> _Vinculos;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;

        private ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> _FormatosCredenciais;

        private ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> _EmpresasLayoutsCrachas;

        private ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> _Contratos;

        private ClasseColaboradoresCredenciais.ColaboradorCredencial _ColaboradorCredencialSelecionado;

        private ClasseColaboradoresCredenciais.ColaboradorCredencial _ColaboradorCredencialTemp = new ClasseColaboradoresCredenciais.ColaboradorCredencial();

        private List<ClasseColaboradoresCredenciais.ColaboradorCredencial> _ColaboradoresCredenciaisTemp = new List<ClasseColaboradoresCredenciais.ColaboradorCredencial>();

        PopupPesquisaColaboradoresCredenciais popupPesquisaColaboradoresCredenciais;

        private int _selectedIndex;

        private int _ColaboradorSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        private string _Validade;

        private ObservableCollection<ClasseTiposCredenciais.TipoCredencial> _TiposCredenciais;

        private ObservableCollection<ClasseCredenciaisStatus.CredencialStatus> _CredenciaisStatus;

        private ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial> _TecnologiasCredenciais;

        private ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _ColaboradoresEmpresas;

        private List<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _ColaboradoresEmpresasTemp = new List<ClasseColaboradoresEmpresas.ColaboradorEmpresa>();


        private ObservableCollection<ClasseClaboradoresPrivilegios.ColaboradorPrivilegio> _ClaboradoresPrivilegios;

        private ObservableCollection<ClasseCredenciaisMotivos.CredencialMotivo> _CredenciaisMotivos;
        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;


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

        public ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa> ColaboradoresEmpresas
        {
            get
            {
                return _ColaboradoresEmpresas;
            }

            set
            {
                if (_ColaboradoresEmpresas != value)
                {
                    _ColaboradoresEmpresas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseClaboradoresPrivilegios.ColaboradorPrivilegio> ColaboradoresPrivilegios
        {
            get
            {
                return _ClaboradoresPrivilegios;
            }

            set
            {
                if (_ClaboradoresPrivilegios != value)
                {
                    _ClaboradoresPrivilegios = value;
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

        public ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial> ColaboradoresCredenciais
        {
            get
            {
                return _ColaboradoresCredenciais;
            }

            set
            {
                if (_ColaboradoresCredenciais != value)
                {
                    _ColaboradoresCredenciais = value;
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

        public ClasseColaboradoresCredenciais.ColaboradorCredencial ColaboradorCredencialSelecionado
        {
            get
            {
                return this._ColaboradorCredencialSelecionado;
            }
            set
            {
                this._ColaboradorCredencialSelecionado = value;
                //base.OnPropertyChanged("SelectedItem");
                base.OnPropertyChanged();
                if (ColaboradorCredencialSelecionado != null)
                {
                    //CarregaColecaoColaboradoresEmpresas(ColaboradorCredencialSelecionado.ColaboradorID);
                    //CarregaColeçãoEmpresasLayoutsCrachas(ColaboradorCredencialSelecionado.EmpresaID);
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

        public int ColaboradorSelecionadaID
        {
            get
            {
                return this._ColaboradorSelecionadaID;

            }
            set
            {
                this._ColaboradorSelecionadaID = value;
                base.OnPropertyChanged();
                if (ColaboradorSelecionadaID != null)
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

        public void OnAtualizaCommand(object _ColaboradorID)
        {
            try
            {
                ColaboradorSelecionadaID = Convert.ToInt32(_ColaboradorID);
                Thread CarregaColecaoColaboradoresCredenciais_thr = new Thread(() =>
                {
                    CarregaColecaoColaboradoresEmpresas(ColaboradorSelecionadaID);
                    // CarregaColeçãoEmpresasLayoutsCrachas(ColaboradorCredencialSelecionado.EmpresaID);
                    CarregaColecaoColaboradoresCredenciais(Convert.ToInt32(_ColaboradorID));
                });
                CarregaColecaoColaboradoresCredenciais_thr.Start();
                //CarregaColecaoColaboradoresCredenciais(Convert.ToInt32(_ColaboradorID));

            }
            catch (Exception ex)
            {

            }

        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                //System.Windows.Forms.OpenFileDialog _arquivoPDF = new System.Windows.Forms.OpenFileDialog();
                //string _sql;
                //string _nomecompletodoarquivo;
                //string _arquivoSTR;
                //_arquivoPDF.InitialDirectory = "c:\\\\";
                //_arquivoPDF.Filter = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                //_arquivoPDF.RestoreDirectory = true;
                //_arquivoPDF.ShowDialog();
                ////if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
                ////{
                //_nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                //_arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
                //_ColaboradorCredencialTemp.Cargo = _nomecompletodoarquivo;
                //_ColaboradorCredencialTemp.Arquivo = _arquivoSTR;
                ////InsereArquivoBD(Convert.ToInt32(empresaID), _nomecompletodoarquivo, _arquivoSTR);

                ////AtualizaListaAnexos(_resp);

                ////}
            }
            catch (Exception ex)
            {

            }
        }

        public void OnEditarCommand()
        {
            try
            {
                _ColaboradoresEmpresasTemp.Clear();
                foreach (var y in ColaboradoresEmpresas)
                {
                    _ColaboradoresEmpresasTemp.Add(y);
                }
                //if (ColaboradorCredencialSelecionado.Ativa)
                //{
                //    List<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _Temp = ColaboradoresEmpresas.Where(x => x.Ativo == true).ToList();
                //    //foreach (var _member in toRemove)
                //    //{
                //    //    ColaboradoresEmpresas.Remove(_member);
                //    //}

                //    ColaboradoresEmpresas.Clear();

                //    ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>(_Temp);
                //}




                _ColaboradorCredencialTemp = ColaboradorCredencialSelecionado.CriaCopia(ColaboradorCredencialSelecionado);
                _selectedIndexTemp = SelectedIndex;
                
                //OnAtualizaCommand(ColaboradorSelecionadaID);
                //CarregaColecaoContratos(_ColaboradorCredencialTemp.EmpresaID);

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
                
                ColaboradoresCredenciais[_selectedIndexTemp] = _ColaboradorCredencialTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
                ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>(_ColaboradoresEmpresasTemp);
            }
            catch (Exception ex)
            {

            }
        }




        public void OnAdicionarCommand()
        {
            try
            {
                if (ColaboradoresCredenciais != null)
                {
                    foreach (var x in ColaboradoresCredenciais)
                    {
                        _ColaboradoresCredenciaisTemp.Add(x);
                    }

                    _selectedIndexTemp = SelectedIndex;
                    ColaboradoresCredenciais.Clear();
                }

                _ColaboradoresEmpresasTemp.Clear();
                foreach (var y in ColaboradoresEmpresas)
                {
                    _ColaboradoresEmpresasTemp.Add(y);
                }

                List<ClasseColaboradoresEmpresas.ColaboradorEmpresa> _Temp = ColaboradoresEmpresas.Where(x => x.Ativo == true).ToList();
                //foreach (var _member in toRemove)
                //{
                //    ColaboradoresEmpresas.Remove(_member);
                //}

                ColaboradoresEmpresas.Clear();

                ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>(_Temp);

                _ColaboradorCredencialTemp = new ClasseColaboradoresCredenciais.ColaboradorCredencial();
                _ColaboradorCredencialTemp.ColaboradorID = ColaboradorSelecionadaID;
                _ColaboradorCredencialTemp.CredencialStatusID = 1;
                ColaboradoresCredenciais.Add(_ColaboradorCredencialTemp);

                //CarregaColecaoColaboradoresEmpresas(ColaboradorSelecionadaID);
                SelectedIndex = 0;
                ColaboradorCredencialSelecionado.Emissao = DateTime.Now;
                HabilitaEdicao = true;
               // SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }

        }

        //public void OnSalvarEdicaoCommand()
        //{
        //    try
        //    {

        //        if (ColaboradorCredencialSelecionado.CredencialStatusID == 1)
        //        {

        //            ColaboradorCredencialSelecionado.Validade = (DateTime?)VerificarMenorData(ColaboradorCredencialSelecionado.ColaboradorID);
        //            bool _resposta = SCManager.Vincular(ColaboradorCredencialSelecionado);


        //        }


        //        HabilitaEdicao = false;
        //        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresCredenciais));

        //        ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial> _ColaboradorCredencialTemp = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>();
        //        ClasseColaboradoresCredenciais _ClasseColaboradorerEmpresasTemp = new ClasseColaboradoresCredenciais();
        //        _ColaboradorCredencialTemp.Add(ColaboradorCredencialSelecionado);
        //        _ClasseColaboradorerEmpresasTemp.ColaboradoresCredenciais = _ColaboradorCredencialTemp;

        //        string xmlString;

        //        using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
        //        {

        //            using (XmlTextWriter xw = new XmlTextWriter(sw))
        //            {
        //                xw.Formatting = Formatting.Indented;
        //                serializer.Serialize(xw, _ClasseColaboradorerEmpresasTemp);
        //                xmlString = sw.ToString();
        //            }

        //        }

        //        InsereColaboradorCredencialBD(xmlString);


        //        Thread CarregaColecaoColaboradoresCredenciais_thr = new Thread(() =>
        //        {
        //            CarregaColecaoColaboradoresCredenciais(ColaboradorSelecionadaID);
        //        });
        //        CarregaColecaoColaboradoresCredenciais_thr.Start();

        //        //_ClasseEmpresasSegurosTemp = null;

        //        //_SegurosTemp.Clear();
        //        //_seguroTemp = null;


        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public void OnSalvarAdicaoCommand()
        {
            try
            {

                //string _xml = RequisitaColaboradoresCredenciaisNovos(ColaboradorCredencialSelecionado.ColaboradorEmpresaID);

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradoresCredenciais));

                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);

                //TextReader reader = new StringReader(_xml);
                //ClasseColaboradoresCredenciais classeColaboradoresCredenciais = new ClasseColaboradoresCredenciais();
                //classeColaboradoresCredenciais = (ClasseColaboradoresCredenciais)deserializer.Deserialize(reader);
                //ColaboradorCredencialSelecionado.Cargo = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].Cargo;
                //ColaboradorCredencialSelecionado.CNPJ = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].CNPJ;
                //ColaboradorCredencialSelecionado.ColaboradorApelido = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorApelido;
                //ColaboradorCredencialSelecionado.ColaboradorEmpresaID = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorEmpresaID;
                //ColaboradorCredencialSelecionado.ColaboradorFoto = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorFoto;
                //ColaboradorCredencialSelecionado.ColaboradorID = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorID;
                //ColaboradorCredencialSelecionado.ColaboradorNome = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ColaboradorNome;
                //ColaboradorCredencialSelecionado.ContratoDescricao = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].ContratoDescricao;
                //ColaboradorCredencialSelecionado.CPF = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].CPF;
                //ColaboradorCredencialSelecionado.EmpresaApelido = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].EmpresaApelido;
                //ColaboradorCredencialSelecionado.EmpresaID = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].EmpresaID;
                //ColaboradorCredencialSelecionado.EmpresaLogo = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].EmpresaLogo;
                //ColaboradorCredencialSelecionado.EmpresaNome = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].EmpresaNome;
                //ColaboradorCredencialSelecionado.Motorista = classeColaboradoresCredenciais.ColaboradoresCredenciais[0].Motorista;

                //ColaboradorCredencialSelecionado.FormatoCredencialDescricao = FormatosCredenciais.First(i => i.FormatoCredencialID == ColaboradorCredencialSelecionado.FormatoCredencialID).Descricao;
                //ColaboradorCredencialSelecionado.FormatIDGUID = FormatosCredenciais.First(i => i.FormatoCredencialID == ColaboradorCredencialSelecionado.FormatoCredencialID).FormatIDGUID.ToString();

                ////ColaboradorCredencialSelecionado.LayoutCrachaGUID = EmpresasLayoutsCrachas.First(i => i.LayoutCrachaID == ColaboradorCredencialSelecionado.LayoutCrachaID).LayoutCrachaGUID;

                //ColaboradorCredencialSelecionado.PrivilegioDescricao1 = ColaboradoresPrivilegios.First(i => i.ColaboradorPrivilegioID == ColaboradorCredencialSelecionado.ColaboradorPrivilegio1ID).Descricao;
                //ColaboradorCredencialSelecionado.PrivilegioDescricao2 = ColaboradoresPrivilegios.First(i => i.ColaboradorPrivilegioID == ColaboradorCredencialSelecionado.ColaboradorPrivilegio2ID).Descricao;



                //ColaboradorCredencialSelecionado.Validade = (DateTime?)VerificarMenorData(ColaboradorCredencialSelecionado.ColaboradorID);

                //var _index = SelectedIndex;

                //bool _resposta = SCManager.Vincular(ColaboradorCredencialSelecionado);


                //if (!ColaboradorCredencialSelecionado.Ativa)
                //{
                //    ColaboradorCredencialSelecionado.Baixa = DateTime.Now;
                //}
                //else
                //{
                //    ColaboradorCredencialSelecionado.Baixa = null;
                //}


                //HabilitaEdicao = false;
                //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresCredenciais));

                //ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial> _ColaboradorCredencialPro = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>();
                //ClasseColaboradoresCredenciais _ClasseColaboradorerEmpresasPro = new ClasseColaboradoresCredenciais();
                //_ColaboradorCredencialPro.Add(ColaboradorCredencialSelecionado);
                //_ClasseColaboradorerEmpresasPro.ColaboradoresCredenciais = _ColaboradorCredencialPro;


                
                //IMOD.Domain.Entities.ColaboradorCredencial ColaboradorEntity = new IMOD.Domain.Entities.ColaboradorCredencial();
                //g.TranportarDados(ColaboradorCredencialSelecionado, 1, ColaboradorEntity);

                var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorCredencial>(ColaboradorCredencialSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorCredencialService();
                if (ColaboradorCredencialSelecionado.ColaboradorCredencialID == 0)
                {
                    _repositorio.Credencial.Criar(entity);
                }
                else
                {
                    _repositorio.Credencial.Alterar(entity);
                }
                
                var id = ColaboradorCredencialSelecionado.ColaboradorID;
                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseColaboradorerEmpresasPro);
                //        xmlString = sw.ToString();
                //    }

                //}
                ////int _colaboradorCredencialID = InsereColaboradorCredencialBD(xmlString);

                //int _colaboradorCredencialID = InsereColaboradorCredencialBD(xmlString);

                CarregaColecaoColaboradoresCredenciais(id);

                //SelectedIndex = _index;

                ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>(_ColaboradoresEmpresasTemp);


                //if (ColaboradorCredencialSelecionado.CredencialStatusID == 1)
                //{

                //    ColaboradorCredencialSelecionado.Validade = (DateTime?)VerificarMenorData(ColaboradorCredencialSelecionado.ColaboradorID);
                //    bool _resposta = SCManager.Vincular(ColaboradorCredencialSelecionado);

                //}

                //Thread CarregaColecaoColaboradoresCredenciais_thr = new Thread(() =>
                //{
                //    //CarregaColecaoColaboradoresCredenciais(ColaboradorSelecionadaID);

                //    //System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => { SelectedIndex = 0; }));


                //    if (ColaboradorCredencialSelecionado.CredencialStatusID == 1)
                //    {


                //        bool _resposta = SCManager.Vincular(ColaboradorCredencialSelecionado);

                //    }

                //});
                //CarregaColecaoColaboradoresCredenciais_thr.Start();




            }
            catch (Exception ex)
            {
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                ColaboradoresCredenciais = null;
                ColaboradoresCredenciais = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>(_ColaboradoresCredenciaisTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresCredenciaisTemp.Clear();
                HabilitaEdicao = false;
                ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>(_ColaboradoresEmpresasTemp);

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
                        if (SCManager.ExcluirCredencial(ColaboradorCredencialSelecionado.CredencialGuid))
                        {
                           
                            var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorCredencial>(ColaboradorCredencialSelecionado);
                            var repositorio = new IMOD.Application.Service.ColaboradorCredencialService();
                            repositorio.Remover(entity);

                            ColaboradoresCredenciais.Remove(ColaboradorCredencialSelecionado);
                        }
                        else
                        {
                            Global.PopupBox("Não foi possível excluir esta credencial. Verifique no Gerenciador de Credenciais do Controle de Acesso.",4);
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
                popupPesquisaColaboradoresCredenciais = new PopupPesquisaColaboradoresCredenciais();
                popupPesquisaColaboradoresCredenciais.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaColaboradoresCredenciais.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            try
            {
                object vetor = popupPesquisaColaboradoresCredenciais.Criterio.Split((char)(20));
                int _colaboradorID = ColaboradorSelecionadaID;
                string _empresaNome = ((string[])vetor)[0];
                int _tipoCredencialID = Convert.ToInt32(((string[])vetor)[1]);
                int _credencialStatusID = Convert.ToInt32(((string[])vetor)[2]);

                CarregaColecaoColaboradoresCredenciais(_colaboradorID, _empresaNome, _tipoCredencialID, _credencialStatusID);
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
                DateTime _datavalidadeCredencial = validadeCursoContrato(ColaboradorCredencialSelecionado.ColaboradorID);
                ColaboradoresCredenciais[SelectedIndex].Validade = _datavalidadeCredencial;
                Validade = String.Format("{0:dd/MM/yyyy}", _datavalidadeCredencial);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Carregamento das Colecoes



        private void CarregaColecaoColaboradoresCredenciais(int colaboradorID, string _empresaNome = "",  int _tipoCredencialID = 0,int _credencialStatusID = 0)
        {
            try
            {
                //string _xml = RequisitaColaboradoresCredenciais(_colaboradorID, _empresaNome, _tipoCredencialID, _credencialStatusID);

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradoresCredenciais));

                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);

                //TextReader reader = new StringReader(_xml);
                //ClasseColaboradoresCredenciais classeColaboradoresCredenciais = new ClasseColaboradoresCredenciais();
                //classeColaboradoresCredenciais = (ClasseColaboradoresCredenciais)deserializer.Deserialize(reader);
                //ColaboradoresCredenciais = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>();
                //ColaboradoresCredenciais = classeColaboradoresCredenciais.ColaboradoresCredenciais;
                //SelectedIndex = -1;

                ////_repositorio.Credencial.
                ///////////////////////////////////////////////////////

                //var service = new ColaboradorService();
                //if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                //if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                //if (!string.IsNullOrWhiteSpace(cpf)) cpf = $"%{cpf}%";
                //var list1 = service.Listar(_colaboradorID); //, nome, apelido, cpf);

                //var list2 = Mapper.Map<List<ClasseColaboradoresCredenciais.ColaboradorCredencial>>(list1.OrderBy(n => n.ColaboradorId));
                //var list2 = Mapper.Map<List<ClasseColaboradores.Colaborador>>(list1.OrderBy(n => n.ColaboradorId));
                
                var list = _repositorio.ListarColaboradores(0,"",0,0,colaboradorID).ToList();
                var list2 = Mapper.Map<List<ClasseColaboradoresCredenciais.ColaboradorCredencial>>(list.OrderBy(n=>n.ColaboradorId));
                var observer = new ObservableCollection<ClasseColaboradoresCredenciais.ColaboradorCredencial>();

                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.ColaboradoresCredenciais = observer;
                SelectedIndex = 0;



            }
            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoColaboradoresPrivilegios()
        {
            try
            {
                ColaboradoresPrivilegios = new ObservableCollection<ClasseClaboradoresPrivilegios.ColaboradorPrivilegio>();
                foreach(ClasseAreasAcessos.AreaAcesso _areaaAcesso in AreasAcessos)
                {
                    ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = _areaaAcesso.AreaAcessoID, Descricao = _areaaAcesso.Identificacao });

                }

                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 1, Descricao = "A" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 2, Descricao = "A1" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 3, Descricao = "A2" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 4, Descricao = "A3" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 5, Descricao = "A4" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 6, Descricao = "C" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 7, Descricao = "C1" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 8, Descricao = "C4" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 9, Descricao = "C6" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 10, Descricao = "C8" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 11, Descricao = "R" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 12, Descricao = "R1" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 13, Descricao = "R2" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 14, Descricao = "R3" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 15, Descricao = "R4" });
                //ColaboradoresPrivilegios.Add(new ClasseClaboradoresPrivilegios.ColaboradorPrivilegio() { ColaboradorPrivilegioID = 16, Descricao = "AR" });

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
        private void CarregaColecaoEmpresas(int? idEmpresa = null, string nome = null, string apelido = null, string cnpj = null, string _quantidaderegistro = "500")
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

        private void CarregaColecaoColaboradoresEmpresas(int _colaboradorID, int _ativo = 2)
        {
            try
            {
                string _xml = RequisitaColaboradoresEmpresas(_colaboradorID, _ativo);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradoresEmpresas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseColaboradoresEmpresas classeColaboradoresEmpresas = new ClasseColaboradoresEmpresas();
                classeColaboradoresEmpresas = (ClasseColaboradoresEmpresas)deserializer.Deserialize(reader);
                ColaboradoresEmpresas = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>();
                ColaboradoresEmpresas = classeColaboradoresEmpresas.ColaboradoresEmpresas;
                SelectedIndex = -1;
                //CarregaColeçãoEmpresasLayoutsCrachas(empresaID);

                //var service = new IMOD.Application.Service.ColaboradorEmpresaService();
                ////if (!string.IsNullOrWhiteSpace(_cargo)) _cargo = $"%{_cargo}%";
                ////if (!string.IsNullOrWhiteSpace(_matricula)) _matricula = $"%{_matricula}%";
                //var list1 = service.Listar(_colaboradorID, _ativo);

                //var list2 = Mapper.Map<List<ClasseColaboradoresEmpresas.ColaboradorEmpresa>>(list1);

                //var observer = new ObservableCollection<ClasseColaboradoresEmpresas.ColaboradorEmpresa>();
                //list2.ForEach(n =>
                //{
                //    observer.Add(n);
                //});

                //this.ColaboradoresEmpresas = observer;


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
                ////this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                //string _xml = RequisitaFormatosCredenciais();

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseFormatosCredenciais));
                //XmlDocument DataFile = new XmlDocument();
                //DataFile.LoadXml(_xml);
                //TextReader reader = new StringReader(_xml);
                //ClasseFormatosCredenciais classeFormatosCredenciais = new ClasseFormatosCredenciais();
                //classeFormatosCredenciais = (ClasseFormatosCredenciais)deserializer.Deserialize(reader);
                //FormatosCredenciais = new ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial>();
                //FormatosCredenciais = classeFormatosCredenciais.FormatosCredenciais;
                
                ////this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));


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

                //string _xml = RequisitaTiposCredenciais();

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposCredenciais));
                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);
                //TextReader reader = new StringReader(_xml);
                //ClasseTiposCredenciais classeTiposCredenciais = new ClasseTiposCredenciais();
                //classeTiposCredenciais = (ClasseTiposCredenciais)deserializer.Deserialize(reader);
                //TiposCredenciais = new ObservableCollection<ClasseTiposCredenciais.TipoCredencial>();
                //TiposCredenciais = classeTiposCredenciais.TiposCredenciais;

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

                //string _xml = RequisitaTecnologiasCredenciais();

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTecnologiasCredenciais));
                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);
                //TextReader reader = new StringReader(_xml);
                //ClasseTecnologiasCredenciais classeTecnologiasCredenciais = new ClasseTecnologiasCredenciais();
                //classeTecnologiasCredenciais = (ClasseTecnologiasCredenciais)deserializer.Deserialize(reader);
                //TecnologiasCredenciais = new ObservableCollection<ClasseTecnologiasCredenciais.TecnologiaCredencial>();
                //TecnologiasCredenciais = classeTecnologiasCredenciais.TecnologiasCredenciais;

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

        #region Data Access
        private string RequisitaCredencial(int _colaboradorCredencialID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseCredencial = _xmlDocument.CreateElement("ClasseCredencial");
                _xmlDocument.AppendChild(_ClasseCredencial);


                string _strSql;
                
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _strSql = "SELECT dbo.ColaboradoresCredenciais.ColaboradorCredencialID, dbo.ColaboradoresCredenciais.Colete, dbo.ColaboradoresCredenciais.Emissao," +
                    " dbo.ColaboradoresCredenciais.Validade, dbo.ColaboradoresEmpresas.Matricula, dbo.ColaboradoresEmpresas.Cargo, dbo.Empresas.Nome AS EmpresaNome," +
                    " dbo.Empresas.Apelido AS EmpresaApelido, dbo.Empresas.CNPJ, dbo.Empresas.Sigla, dbo.Empresas.Logo, dbo.Colaboradores.Nome AS ColaboradorNome," +
                    " dbo.Colaboradores.Apelido AS ColaboradorApelido, dbo.Colaboradores.CPF, dbo.Colaboradores.RG, dbo.Colaboradores.RGOrgLocal," +
                    " dbo.Colaboradores.RGOrgUF, dbo.Colaboradores.TelefoneEmergencia, dbo.Colaboradores.Foto, AreasAcessos_1.Identificacao AS Identificacao1," +
                    " dbo.AreasAcessos.Identificacao AS Identificacao2, dbo.Colaboradores.CNHCategoria,dbo.LayoutsCrachas.LayoutRPT " +
                    " FROM  dbo.AreasAcessos AS AreasAcessos_1 INNER JOIN dbo.ColaboradoresCredenciais ON" +
                    " AreasAcessos_1.AreaAcessoID = dbo.ColaboradoresCredenciais.ColaboradorPrivilegio1ID INNER JOIN dbo.AreasAcessos ON" +
                    " dbo.ColaboradoresCredenciais.ColaboradorPrivilegio2ID = dbo.AreasAcessos.AreaAcessoID INNER JOIN dbo.Colaboradores INNER JOIN" +
                    " dbo.ColaboradoresEmpresas ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresEmpresas.ColaboradorID INNER JOIN " +
                    "dbo.Empresas ON dbo.ColaboradoresEmpresas.EmpresaID = dbo.Empresas.EmpresaID ON" +
                    " dbo.ColaboradoresCredenciais.ColaboradorEmpresaID = dbo.ColaboradoresEmpresas.ColaboradorEmpresaID INNER JOIN dbo.LayoutsCrachas ON" +
                    " dbo.ColaboradoresCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID" +
                    " WHERE(dbo.ColaboradoresCredenciais.ColaboradorCredencialID =" + _colaboradorCredencialID + ")";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {


                    XmlNode _ColaboradorCredencialID = _xmlDocument.CreateElement("ColaboradorCredencialID");
                    _ColaboradorCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCredencialID"].ToString())));
                    _ClasseCredencial.AppendChild(_ColaboradorCredencialID);


                    XmlNode _Colete = _xmlDocument.CreateElement("Colete");
                    _Colete.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Colete"].ToString())));
                    _ClasseCredencial.AppendChild(_Colete);

                    var dateStr = (_sqlreader["Emissao"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
                        _Emissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _ClasseCredencial.AppendChild(_Emissao);
                    }

                    dateStr = (_sqlreader["Validade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                        _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _ClasseCredencial.AppendChild(_Validade);
                    }

                    XmlNode _Matricula = _xmlDocument.CreateElement("Matricula");
                    _Matricula.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Matricula"].ToString())));
                    _ClasseCredencial.AppendChild(_Matricula);

                    XmlNode _Cargo = _xmlDocument.CreateElement("Cargo");
                    _Cargo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString())));
                    _ClasseCredencial.AppendChild(_Cargo);

                    XmlNode _EmpresaNome = _xmlDocument.CreateElement("EmpresaNome");
                    _EmpresaNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
                    _ClasseCredencial.AppendChild(_EmpresaNome);

                    XmlNode _EmpresaApelido = _xmlDocument.CreateElement("EmpresaApelido");
                    _EmpresaApelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaApelido"].ToString())));
                    _ClasseCredencial.AppendChild(_EmpresaApelido);

                    XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
                    _CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim().FormatarCnpj())));
                    _ClasseCredencial.AppendChild(_CNPJ);

                    XmlNode _Sigla = _xmlDocument.CreateElement("Sigla");
                    _Sigla.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Sigla"].ToString())));
                    _ClasseCredencial.AppendChild(_Sigla);

                    XmlNode _Logo = _xmlDocument.CreateElement("Logo");
                    _Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString().Trim())));
                    _ClasseCredencial.AppendChild(_Logo);

                    XmlNode _ColaboradorNome = _xmlDocument.CreateElement("ColaboradorNome");
                    _ColaboradorNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorNome"].ToString())));
                    _ClasseCredencial.AppendChild(_ColaboradorNome);

                    XmlNode _ColaboradorApelido = _xmlDocument.CreateElement("ColaboradorApelido");
                    _ColaboradorApelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorApelido"].ToString())));
                    _ClasseCredencial.AppendChild(_ColaboradorApelido);

                    XmlNode _CPF = _xmlDocument.CreateElement("CPF");
                    _CPF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().Trim().FormatarCpf())));
                    _ClasseCredencial.AppendChild(_CPF);

                    XmlNode _RG = _xmlDocument.CreateElement("RG");
                    _RG.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RG"].ToString().Trim())));
                    _ClasseCredencial.AppendChild(_RG);

                    XmlNode _RGOrgLocal = _xmlDocument.CreateElement("RGOrgLocal");
                    _RGOrgLocal.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgLocal"].ToString())));
                    _ClasseCredencial.AppendChild(_RGOrgLocal);

                    XmlNode _RGOrgUF = _xmlDocument.CreateElement("RGOrgUF");
                    _RGOrgUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgUF"].ToString())));
                    _ClasseCredencial.AppendChild(_RGOrgUF);

                    XmlNode _TelefoneEmergencia = _xmlDocument.CreateElement("TelefoneEmergencia");
                    _TelefoneEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneEmergencia"].ToString())));
                    _ClasseCredencial.AppendChild(_TelefoneEmergencia);

                    XmlNode _Foto = _xmlDocument.CreateElement("Foto");
                    _Foto.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Foto"].ToString().Trim())));
                    _ClasseCredencial.AppendChild(_Foto);

                    XmlNode _Identificacao1 = _xmlDocument.CreateElement("Identificacao1");
                    _Identificacao1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Identificacao1"].ToString())));
                    _ClasseCredencial.AppendChild(_Identificacao1);

                    XmlNode _Identificacao2 = _xmlDocument.CreateElement("Identificacao2");
                    _Identificacao2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Identificacao2"].ToString().Trim())));
                    _ClasseCredencial.AppendChild(_Identificacao2);

                    XmlNode _CNHCategoria = _xmlDocument.CreateElement("CNHCategoria");
                    _CNHCategoria.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHCategoria"].ToString().Trim())));
                    _ClasseCredencial.AppendChild(_CNHCategoria);


                    XmlNode _LayoutRPT = _xmlDocument.CreateElement("LayoutRPT");
                    _LayoutRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutRPT"].ToString().Trim())));
                    _ClasseCredencial.AppendChild(_LayoutRPT);


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
            
        }

        private string RequisitaColaboradoresCredenciais(int _colaboradorID, string _empresaNome = "", int _tipoCredencialID = 0, int _credencialStatusID = 0)//Possibilidade de criar a pesquisa por Matriculatambem
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseColaboradoresCredenciais = _xmlDocument.CreateElement("ClasseColaboradoresCredenciais");
                _xmlDocument.AppendChild(_ClasseColaboradoresCredenciais);

                XmlNode _ColaboradoresCredenciais = _xmlDocument.CreateElement("ColaboradoresCredenciais");
                _ClasseColaboradoresCredenciais.AppendChild(_ColaboradoresCredenciais);

                string _strSql;
                string _credencialStatusSTR = "";
                string _tipoCredencialIDSTR = "";

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _empresaNome = _empresaNome == "" ? "" : " AND dbo.Empresas.Nome like '%" + _empresaNome + "%' ";
                _credencialStatusSTR = _credencialStatusID == 0 ? "" : " AND CredencialStatusID = " + _credencialStatusID ;
                _tipoCredencialIDSTR = _tipoCredencialID == 0 ? "" : " AND TipoCredencialID = " + _tipoCredencialID;

                //_validade = _validade == "" ? "" : " AND _validade like '%" + _validade + "%'";


                _strSql = "SELECT dbo.LayoutsCrachas.Nome AS LayoutCrachaNome, dbo.FormatosCredenciais.Descricao AS FormatoCredencialDescricao," +
                    " dbo.ColaboradoresCredenciais.NumeroCredencial, dbo.ColaboradoresCredenciais.FC, dbo.ColaboradoresCredenciais.Emissao,dbo.ColaboradoresCredenciais.Impressa," +
                    " dbo.ColaboradoresCredenciais.Validade,dbo.ColaboradoresCredenciais.Baixa, dbo.ColaboradoresCredenciais.Ativa,dbo.ColaboradoresCredenciais.ColaboradorPrivilegio1ID," +
                    " dbo.ColaboradoresCredenciais.ColaboradorPrivilegio2ID,dbo.ColaboradoresCredenciais.Colete,dbo.ColaboradoresCredenciais.CredencialMotivoID," +
                    " dbo.ColaboradoresCredenciais.CredencialStatusID, dbo.ColaboradoresCredenciais.ColaboradorEmpresaID, " +
                    " dbo.ColaboradoresCredenciais.TipoCredencialID, dbo.ColaboradoresCredenciais.TecnologiaCredencialID, dbo.ColaboradoresCredenciais.FormatoCredencialID," +
                    " dbo.ColaboradoresCredenciais.LayoutCrachaID, dbo.ColaboradoresCredenciais.ColaboradorCredencialID, dbo.Colaboradores.Nome AS ColaboradorNome," +
                    " dbo.Empresas.Nome AS EmpresaNome, dbo.EmpresasContratos.Descricao AS ContratoDescricao,dbo.ColaboradoresEmpresas.EmpresaID," +
                    " dbo.ColaboradoresEmpresas.ColaboradorID, dbo.ColaboradoresCredenciais.CardHolderGUID, dbo.ColaboradoresCredenciais.CredencialGUID," +
                    " dbo.Colaboradores.Foto AS ColaboradorFoto, dbo.Colaboradores.CPF, dbo.Colaboradores.Motorista, dbo.Colaboradores.Apelido AS ColaboradorApelido," +
                    " dbo.Colaboradores.Nome AS ColaboradorNome, dbo.Empresas.Logo AS EmpresaLogo,dbo.Empresas.Sigla AS EmpresaSigla," +
                    " dbo.Empresas.Apelido AS EmpresaApelido, dbo.ColaboradoresEmpresas.Cargo, dbo.LayoutsCrachas.LayoutCrachaGUID, dbo.Empresas.CNPJ," +
                    " dbo.FormatosCredenciais.FormatIDGUID FROM dbo.ColaboradoresCredenciais INNER JOIN dbo.FormatosCredenciais ON" +
                    " dbo.ColaboradoresCredenciais.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN dbo.ColaboradoresEmpresas ON" +
                    " dbo.ColaboradoresCredenciais.ColaboradorEmpresaID = dbo.ColaboradoresEmpresas.ColaboradorEmpresaID INNER JOIN dbo.Empresas ON" +
                    " dbo.ColaboradoresEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON " +
                    " dbo.ColaboradoresEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN dbo.Colaboradores ON" +
                    " dbo.ColaboradoresEmpresas.ColaboradorID = dbo.Colaboradores.ColaboradorID LEFT OUTER JOIN dbo.LayoutsCrachas ON" +
                    " dbo.ColaboradoresCredenciais.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID" +
                    " WHERE dbo.ColaboradoresEmpresas.ColaboradorID =" + _colaboradorID + _credencialStatusSTR + _empresaNome + _tipoCredencialIDSTR +
                    " ORDER BY dbo.ColaboradoresCredenciais.ColaboradorCredencialID DESC";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _ColaboradorCredencial = _xmlDocument.CreateElement("ColaboradorCredencial");
                    _ColaboradoresCredenciais.AppendChild(_ColaboradorCredencial);

                    XmlNode _ColaboradorCredencialID = _xmlDocument.CreateElement("ColaboradorCredencialID");
                    _ColaboradorCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCredencialID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_ColaboradorCredencialID);

                    XmlNode _Ativa = _xmlDocument.CreateElement("Ativa");
                    _Ativa.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativa"])).ToString()));
                    _ColaboradorCredencial.AppendChild(_Ativa);

                    XmlNode ColaboradorPrivilegio1ID = _xmlDocument.CreateElement("ColaboradorPrivilegio1ID");
                    ColaboradorPrivilegio1ID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorPrivilegio1ID"].ToString())));
                    _ColaboradorCredencial.AppendChild(ColaboradorPrivilegio1ID);

                    XmlNode ColaboradorPrivilegio2ID = _xmlDocument.CreateElement("ColaboradorPrivilegio2ID");
                    ColaboradorPrivilegio2ID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorPrivilegio2ID"].ToString())));
                    _ColaboradorCredencial.AppendChild(ColaboradorPrivilegio2ID);

                    XmlNode _ColaboradorEmpresaID = _xmlDocument.CreateElement("ColaboradorEmpresaID");
                    _ColaboradorEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorEmpresaID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_ColaboradorEmpresaID);

                    XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
                    _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_ColaboradorID);

                    XmlNode _CardHolderGUID = _xmlDocument.CreateElement("CardHolderGuid");
                    _CardHolderGUID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CardHolderGuid"].ToString())));
                    _ColaboradorCredencial.AppendChild(_CardHolderGUID);

                    XmlNode _CredencialGUID = _xmlDocument.CreateElement("CredencialGuid");
                    _CredencialGUID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialGuid"].ToString())));
                    _ColaboradorCredencial.AppendChild(_CredencialGUID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("ContratoDescricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContratoDescricao"].ToString())));
                    _ColaboradorCredencial.AppendChild(_Descricao);

                    XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
                    _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
                    _ColaboradorCredencial.AppendChild(_Empresa);

                    XmlNode _ColaboradorNome = _xmlDocument.CreateElement("ColaboradorNome");
                    _ColaboradorNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorNome"].ToString())));
                    _ColaboradorCredencial.AppendChild(_ColaboradorNome);

                    XmlNode _TecnologiaCredencialID = _xmlDocument.CreateElement("TecnologiaCredencialID");
                    _TecnologiaCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TecnologiaCredencialID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_TecnologiaCredencialID);

                    XmlNode _TecnologiaCredencialDescricao = _xmlDocument.CreateElement("TecnologiaCredencialDescricao");
                    var _tec = TecnologiasCredenciais.FirstOrDefault(x => x.TecnologiaCredencialID == Convert.ToInt32(_sqlreader["TecnologiaCredencialID"].ToString()));
                    if (_tec != null)
                    {
                        _TecnologiaCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tec.Descricao.ToString()));
                        _ColaboradorCredencial.AppendChild(_TecnologiaCredencialDescricao);
                    }

                    XmlNode _TipoCredencialID = _xmlDocument.CreateElement("TipoCredencialID");
                    _TipoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoCredencialID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_TipoCredencialID);

                    XmlNode _TipoCredencialDescricao = _xmlDocument.CreateElement("TipoCredencialDescricao");
                    var _tip = TiposCredenciais.FirstOrDefault(x => x.TipoCredencialID == Convert.ToInt32(_sqlreader["TipoCredencialID"].ToString()));
                    if (_tip != null)
                    {
                        _TipoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tip.Descricao.ToString()));
                        _ColaboradorCredencial.AppendChild(_TipoCredencialDescricao);
                    }

                    XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
                    _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_LayoutCrachaID);

                    XmlNode _LayoutCrachaNome = _xmlDocument.CreateElement("LayoutCrachaNome");
                    _LayoutCrachaNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaNome"].ToString())));
                    _ColaboradorCredencial.AppendChild(_LayoutCrachaNome);

                    XmlNode _FormatoCredencialID = _xmlDocument.CreateElement("FormatoCredencialID");
                    _FormatoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_FormatoCredencialID);

                    XmlNode _FormatoCredencialDescricao = _xmlDocument.CreateElement("FormatoCredencialDescricao");
                    _FormatoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialDescricao"].ToString())));
                    _ColaboradorCredencial.AppendChild(_FormatoCredencialDescricao);

                    XmlNode _NumeroCredencial = _xmlDocument.CreateElement("NumeroCredencial");
                    _NumeroCredencial.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroCredencial"].ToString())));
                    _ColaboradorCredencial.AppendChild(_NumeroCredencial);

                    XmlNode _FC = _xmlDocument.CreateElement("FC");
                    _FC.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FC"].ToString())));
                    _ColaboradorCredencial.AppendChild(_FC);

                    var dateStr = (_sqlreader["Emissao"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
                        _Emissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _ColaboradorCredencial.AppendChild(_Emissao);
                    }

                    dateStr = (_sqlreader["Validade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                        _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _ColaboradorCredencial.AppendChild(_Validade);
                    }

                    XmlNode _CredencialStatusID = _xmlDocument.CreateElement("CredencialStatusID");
                    _CredencialStatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialStatusID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_CredencialStatusID);

                    XmlNode _CredencialStatusDescricao = _xmlDocument.CreateElement("CredencialStatusDescricao");
                    var _sta = CredenciaisStatus.FirstOrDefault(x => x.CredencialStatusID == Convert.ToInt32(_sqlreader["CredencialStatusID"].ToString()));
                    if (_sta != null)
                    {
                        _CredencialStatusDescricao.AppendChild(_xmlDocument.CreateTextNode(_sta.Descricao.ToString()));
                        _ColaboradorCredencial.AppendChild(_CredencialStatusDescricao);
                    }


                    XmlNode _Vinculo9 = _xmlDocument.CreateElement("CPF");
                    _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo9);

                    XmlNode _Vinculo22 = _xmlDocument.CreateElement("Motorista");
                    _Vinculo22.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Motorista"])).ToString()));
                    _ColaboradorCredencial.AppendChild(_Vinculo22);

                    XmlNode _Vinculo21 = _xmlDocument.CreateElement("ColaboradorApelido");
                    _Vinculo21.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorApelido"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo21);

                    XmlNode _Vinculo11 = _xmlDocument.CreateElement("ColaboradorFoto");
                    _Vinculo11.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorFoto"].ToString())));
                    _ColaboradorCredencial.AppendChild(_Vinculo11);


                    XmlNode _Vinculo12 = _xmlDocument.CreateElement("EmpresaLogo");
                    _Vinculo12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaLogo"].ToString())));
                    _ColaboradorCredencial.AppendChild(_Vinculo12);

                    XmlNode _Vinculo23 = _xmlDocument.CreateElement("EmpresaApelido");
                    _Vinculo23.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaApelido"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo23);

                    XmlNode _Vinculo13 = _xmlDocument.CreateElement("Cargo");
                    _Vinculo13.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo13);


                    XmlNode _Vinculo15 = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _Vinculo15.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaGUID"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo15);

                    XmlNode _Vinculo19 = _xmlDocument.CreateElement("CNPJ");
                    _Vinculo19.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo19);

                    XmlNode _Vinculo20 = _xmlDocument.CreateElement("FormatIDGUID");
                    _Vinculo20.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatIDGUID"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo20);

                    XmlNode _Colete = _xmlDocument.CreateElement("Colete");
                    _Colete.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Colete"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Colete);

                    XmlNode _EmpresaSigla = _xmlDocument.CreateElement("EmpresaSigla");
                    _EmpresaSigla.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaSigla"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_EmpresaSigla);

                    XmlNode _CredencialMotivoID = _xmlDocument.CreateElement("CredencialMotivoID");
                    _CredencialMotivoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialMotivoID"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_CredencialMotivoID);

                    dateStr = (_sqlreader["Baixa"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Baixa = _xmlDocument.CreateElement("Baixa");
                        _Baixa.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _ColaboradorCredencial.AppendChild(_Baixa);

                    }

                    XmlNode _Impressa = _xmlDocument.CreateElement("Impressa");
                    _Impressa.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Impressa"])).ToString().Trim()));
                    _ColaboradorCredencial.AppendChild(_Impressa);
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


        //private string RequisitaColaboradoresCredenciais(int _colaboradorID, string _empresaNome = "", int _status = 0, string _validade = "")//Possibilidade de criar a pesquisa por Matriculatambem
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
        //        string _statusSTR = "";

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        _empresaNome = _empresaNome == "" ? "" : " AND Nome like '%" + _empresaNome + "%' ";
        //        _statusSTR = _status == 0 ? "" : " AND CredencialStatusID = " + _status + "' ";
        //        _validade = _validade == "" ? "" : " AND _validade like '%" + _validade + "%'";


        //        _strSql = "SELECT dbo.LayoutsCrachas.Nome AS LayoutCrachaNome, dbo.FormatosCredenciais.Descricao AS FormatoCredencialDescricao," +
        //            " dbo.ColaboradoresCredenciais.NumeroCredencial, dbo.ColaboradoresCredenciais.FC, dbo.ColaboradoresCredenciais.Emissao,dbo.ColaboradoresCredenciais.Impressa," +
        //            " dbo.ColaboradoresCredenciais.Validade, dbo.ColaboradoresCredenciais.Ativa,dbo.ColaboradoresCredenciais.ColaboradorPrivilegio1ID," +
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
        //            " WHERE dbo.ColaboradoresEmpresas.ColaboradorID =" + _colaboradorID + _statusSTR + _empresaNome + _validade +
        //            " ORDER BY dbo.ColaboradoresCredenciais.ColaboradorCredencialID DESC";

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _ColaboradorCredencial = _xmlDocument.CreateElement("ColaboradorCredencial");
        //            _ColaboradoresCredenciais.AppendChild(_ColaboradorCredencial);

        //            XmlNode _ColaboradorCredencialID = _xmlDocument.CreateElement("ColaboradorCredencialID");
        //            _ColaboradorCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCredencialID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorCredencialID);

        //            XmlNode _Ativa = _xmlDocument.CreateElement("Ativa");
        //            _Ativa.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativa"])).ToString().Trim()));
        //            _ColaboradorCredencial.AppendChild(_Ativa);

        //            XmlNode ColaboradorPrivilegio1ID = _xmlDocument.CreateElement("ColaboradorPrivilegio1ID");
        //            ColaboradorPrivilegio1ID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorPrivilegio1ID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(ColaboradorPrivilegio1ID);

        //            XmlNode ColaboradorPrivilegio2ID = _xmlDocument.CreateElement("ColaboradorPrivilegio2ID");
        //            ColaboradorPrivilegio2ID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorPrivilegio2ID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(ColaboradorPrivilegio2ID);

        //            XmlNode _ColaboradorEmpresaID = _xmlDocument.CreateElement("ColaboradorEmpresaID");
        //            _ColaboradorEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorEmpresaID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorEmpresaID);

        //            XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
        //            _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorID);

        //            XmlNode _CardHolderGUID = _xmlDocument.CreateElement("CardHolderGuid");
        //            _CardHolderGUID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CardHolderGuid"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_CardHolderGUID);

        //            XmlNode _CredencialGUID = _xmlDocument.CreateElement("CredencialGuid");
        //            _CredencialGUID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialGuid"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_CredencialGUID);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("ContratoDescricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContratoDescricao"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Descricao);

        //            XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
        //            _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Empresa);

        //            XmlNode _ColaboradorNome = _xmlDocument.CreateElement("ColaboradorNome");
        //            _ColaboradorNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorNome"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_ColaboradorNome);

        //            XmlNode _TecnologiaCredencialID = _xmlDocument.CreateElement("TecnologiaCredencialID");
        //            _TecnologiaCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TecnologiaCredencialID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_TecnologiaCredencialID);

        //            XmlNode _TecnologiaCredencialDescricao = _xmlDocument.CreateElement("TecnologiaCredencialDescricao");
        //            var _tec = TecnologiasCredenciais.FirstOrDefault(x => x.TecnologiaCredencialID == Convert.ToInt32(_sqlreader["TecnologiaCredencialID"].ToString().Trim()));
        //            if (_tec != null)
        //            {
        //                _TecnologiaCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tec.Descricao.ToString().Trim()));
        //                _ColaboradorCredencial.AppendChild(_TecnologiaCredencialDescricao);
        //            }

        //            XmlNode _TipoCredencialID = _xmlDocument.CreateElement("TipoCredencialID");
        //            _TipoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoCredencialID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_TipoCredencialID);

        //            XmlNode _TipoCredencialDescricao = _xmlDocument.CreateElement("TipoCredencialDescricao");
        //            var _tip = TiposCredenciais.FirstOrDefault(x => x.TipoCredencialID == Convert.ToInt32(_sqlreader["TipoCredencialID"].ToString().Trim()));
        //            if (_tip != null)
        //            {
        //                _TipoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode(_tip.Descricao.ToString().Trim()));
        //                _ColaboradorCredencial.AppendChild(_TipoCredencialDescricao);
        //            }

        //            XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
        //            _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_LayoutCrachaID);

        //            XmlNode _LayoutCrachaNome = _xmlDocument.CreateElement("LayoutCrachaNome");
        //            _LayoutCrachaNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaNome"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_LayoutCrachaNome);

        //            XmlNode _FormatoCredencialID = _xmlDocument.CreateElement("FormatoCredencialID");
        //            _FormatoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_FormatoCredencialID);

        //            XmlNode _FormatoCredencialDescricao = _xmlDocument.CreateElement("FormatoCredencialDescricao");
        //            _FormatoCredencialDescricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialDescricao"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_FormatoCredencialDescricao);

        //            XmlNode _NumeroCredencial = _xmlDocument.CreateElement("NumeroCredencial");
        //            _NumeroCredencial.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroCredencial"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_NumeroCredencial);

        //            XmlNode _FC = _xmlDocument.CreateElement("FC");
        //            _FC.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FC"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_FC);

        //            var dateStr = (_sqlreader["Emissao"].ToString().Trim());
        //            if (!string.IsNullOrWhiteSpace(dateStr))
        //            {
        //                var dt2 = Convert.ToDateTime(dateStr);
        //                XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
        //                _Emissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        //                _ColaboradorCredencial.AppendChild(_Emissao);
        //            }

        //            dateStr = (_sqlreader["Validade"].ToString().Trim());
        //            if (!string.IsNullOrWhiteSpace(dateStr))
        //            {
        //                var dt2 = Convert.ToDateTime(dateStr);
        //                XmlNode _Validade = _xmlDocument.CreateElement("Validade");
        //                _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
        //                _ColaboradorCredencial.AppendChild(_Validade);
        //            }

        //            XmlNode _CredencialStatusID = _xmlDocument.CreateElement("CredencialStatusID");
        //            _CredencialStatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialStatusID"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_CredencialStatusID);

        //            XmlNode _CredencialStatusDescricao = _xmlDocument.CreateElement("CredencialStatusDescricao");
        //            var _sta = CredenciaisStatus.FirstOrDefault(x => x.CredencialStatusID == Convert.ToInt32(_sqlreader["CredencialStatusID"].ToString().Trim()));
        //            if (_sta != null)
        //            {
        //                _CredencialStatusDescricao.AppendChild(_xmlDocument.CreateTextNode(_sta.Descricao.ToString().Trim()));
        //                _ColaboradorCredencial.AppendChild(_CredencialStatusDescricao);
        //            }


        //            XmlNode _Vinculo9 = _xmlDocument.CreateElement("CPF");
        //            _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo9);

        //            XmlNode _Vinculo22 = _xmlDocument.CreateElement("Motorista");
        //            _Vinculo22.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Motorista"])).ToString().Trim()));
        //            _ColaboradorCredencial.AppendChild(_Vinculo22);

        //            XmlNode _Vinculo21 = _xmlDocument.CreateElement("ColaboradorApelido");
        //            _Vinculo21.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorApelido"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo21);

        //            XmlNode _Vinculo11 = _xmlDocument.CreateElement("ColaboradorFoto");
        //            _Vinculo11.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorFoto"].ToString().Trim())));
        //            _ColaboradorCredencial.AppendChild(_Vinculo11);


        //            XmlNode _Vinculo12 = _xmlDocument.CreateElement("EmpresaLogo");
        //            _Vinculo12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaLogo"].ToString().Trim())));
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

        private string RequisitaColaboradoresCredenciaisNovos(int _colaboradorEmpresaID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseColaboradoresCredenciais = _xmlDocument.CreateElement("ClasseColaboradoresCredenciais");
                _xmlDocument.AppendChild(_ClasseColaboradoresCredenciais);

                XmlNode _ColaboradoresCredenciais = _xmlDocument.CreateElement("ColaboradoresCredenciais");
                _ClasseColaboradoresCredenciais.AppendChild(_ColaboradoresCredenciais);

                string _strSql;

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _strSql = "SELECT dbo.Colaboradores.Nome AS ColaboradorNome, dbo.Empresas.Nome AS EmpresaNome, dbo.EmpresasContratos.Descricao AS ContratoDescricao," +
                    " dbo.ColaboradoresEmpresas.EmpresaID, dbo.ColaboradoresEmpresas.ColaboradorID, dbo.Colaboradores.Foto AS ColaboradorFoto, dbo.Colaboradores.CPF," +
                    " dbo.Colaboradores.Motorista, dbo.Colaboradores.Apelido AS ColaboradorApelido, dbo.Colaboradores.Nome, dbo.Empresas.Logo AS EmpresaLogo," +
                    " dbo.Empresas.Apelido AS EmpresaApelido, dbo.ColaboradoresEmpresas.Cargo, dbo.Empresas.CNPJ, dbo.ColaboradoresEmpresas.ColaboradorEmpresaID" +
                    " FROM dbo.Empresas INNER JOIN dbo.ColaboradoresEmpresas ON" +
                    " dbo.Empresas.EmpresaID = dbo.ColaboradoresEmpresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON" +
                    " dbo.ColaboradoresEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID INNER JOIN dbo.Colaboradores ON" +
                    " dbo.ColaboradoresEmpresas.ColaboradorID = dbo.Colaboradores.ColaboradorID WHERE dbo.ColaboradoresEmpresas.Ativo = 1 AND" +
                    " dbo.ColaboradoresEmpresas.ColaboradorEmpresaID = " + _colaboradorEmpresaID;

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _ColaboradorCredencial = _xmlDocument.CreateElement("ColaboradorCredencial");
                    _ColaboradoresCredenciais.AppendChild(_ColaboradorCredencial);

                    //XmlNode _ColaboradorCredencialID = _xmlDocument.CreateElement("ColaboradorCredencialID");
                    //_ColaboradorCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCredencialID"].ToString())));
                    //_ColaboradorCredencial.AppendChild(_ColaboradorCredencialID);

                    XmlNode _ColaboradorEmpresaID = _xmlDocument.CreateElement("ColaboradorEmpresaID");
                    _ColaboradorEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorEmpresaID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_ColaboradorEmpresaID);

                    XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
                    _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
                    _ColaboradorCredencial.AppendChild(_ColaboradorID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("ContratoDescricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContratoDescricao"].ToString())));
                    _ColaboradorCredencial.AppendChild(_Descricao);

                    XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
                    _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
                    _ColaboradorCredencial.AppendChild(_Empresa);

                    XmlNode _ColaboradorNome = _xmlDocument.CreateElement("ColaboradorNome");
                    _ColaboradorNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorNome"].ToString())));
                    _ColaboradorCredencial.AppendChild(_ColaboradorNome);

                    //XmlNode _CredencialStatusID = _xmlDocument.CreateElement("CredencialStatusID");
                    //_CredencialStatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialStatusID"].ToString())));
                    //_ColaboradorCredencial.AppendChild(_CredencialStatusID);

                    XmlNode _Vinculo9 = _xmlDocument.CreateElement("CPF");
                    _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo9);

                    XmlNode _Vinculo22 = _xmlDocument.CreateElement("Motorista");
                    _Vinculo22.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Motorista"])).ToString()));
                    _ColaboradorCredencial.AppendChild(_Vinculo22);

                    XmlNode _Vinculo21 = _xmlDocument.CreateElement("ColaboradorApelido");
                    _Vinculo21.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorApelido"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo21);

                    XmlNode _Vinculo11 = _xmlDocument.CreateElement("ColaboradorFoto");
                    _Vinculo11.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorFoto"].ToString())));
                    _ColaboradorCredencial.AppendChild(_Vinculo11);


                    XmlNode _Vinculo12 = _xmlDocument.CreateElement("EmpresaLogo");
                    _Vinculo12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaLogo"].ToString())));
                    _ColaboradorCredencial.AppendChild(_Vinculo12);

                    XmlNode _Vinculo23 = _xmlDocument.CreateElement("EmpresaApelido");
                    _Vinculo23.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaApelido"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo23);

                    XmlNode _Vinculo13 = _xmlDocument.CreateElement("Cargo");
                    _Vinculo13.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo13);

                    XmlNode _Vinculo19 = _xmlDocument.CreateElement("CNPJ");
                    _Vinculo19.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().Trim())));
                    _ColaboradorCredencial.AppendChild(_Vinculo19);

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

        private string RequisitaColaboradoresEmpresas(int _colaboradorID, int _ativo = 2)//Possibilidade de criar a pesquisa por Matriculatambem
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseColaboradoresEmpresas = _xmlDocument.CreateElement("ClasseColaboradoresEmpresas");
                _xmlDocument.AppendChild(_ClasseColaboradoresEmpresas);

                XmlNode _ColaboradoresEmpresas = _xmlDocument.CreateElement("ColaboradoresEmpresas");
                _ClasseColaboradoresEmpresas.AppendChild(_ColaboradoresEmpresas);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                if (_ativo == 1)
                {
                    _strSql = "SELECT dbo.ColaboradoresEmpresas.ColaboradorEmpresaID, dbo.ColaboradoresEmpresas.EmpresaID, dbo.Empresas.Nome AS EmpresaNome," +
                        " dbo.EmpresasContratos.NumeroContrato, dbo.EmpresasContratos.Descricao, dbo.ColaboradoresEmpresas.Matricula," +
                        " dbo.ColaboradoresEmpresas.Ativo FROM dbo.ColaboradoresEmpresas INNER JOIN dbo.Empresas ON" +
                        " dbo.ColaboradoresEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON" +
                        " dbo.ColaboradoresEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID" +
                        " WHERE(dbo.ColaboradoresEmpresas.ColaboradorID =" + _colaboradorID + ")" +
                        " AND(dbo.ColaboradoresEmpresas.Ativo = 1)  ";
                }
                else
                {
                    _strSql = "SELECT dbo.ColaboradoresEmpresas.ColaboradorEmpresaID, dbo.ColaboradoresEmpresas.EmpresaID, dbo.Empresas.Nome AS EmpresaNome," +
                        " dbo.EmpresasContratos.NumeroContrato, dbo.EmpresasContratos.Descricao, dbo.ColaboradoresEmpresas.Matricula," +
                        " dbo.ColaboradoresEmpresas.Ativo FROM dbo.ColaboradoresEmpresas INNER JOIN dbo.Empresas ON" +
                        " dbo.ColaboradoresEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.EmpresasContratos ON" +
                        " dbo.ColaboradoresEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID" +
                        " WHERE(dbo.ColaboradoresEmpresas.ColaboradorID =" + _colaboradorID + ")";
                }


                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _ColaboradorEmpresa = _xmlDocument.CreateElement("ColaboradorEmpresa");
                    _ColaboradoresEmpresas.AppendChild(_ColaboradorEmpresa);

                    XmlNode _ColaboradorEmpresaID = _xmlDocument.CreateElement("ColaboradorEmpresaID");
                    _ColaboradorEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorEmpresaID"].ToString())));
                    _ColaboradorEmpresa.AppendChild(_ColaboradorEmpresaID);

                    //XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
                    //_ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
                    //_ColaboradorEmpresa.AppendChild(_ColaboradorID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _ColaboradorEmpresa.AppendChild(_EmpresaID);

                    //XmlNode _EmpresaContratoID = _xmlDocument.CreateElement("EmpresaContratoID");
                    //_EmpresaContratoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaContratoID"].ToString())));
                    //_ColaboradorEmpresa.AppendChild(_EmpresaContratoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _ColaboradorEmpresa.AppendChild(_Descricao);

                    XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
                    _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaNome"].ToString())));
                    _ColaboradorEmpresa.AppendChild(_Empresa);

                    //XmlNode _Cargo = _xmlDocument.CreateElement("Cargo");
                    //_Cargo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString())));
                    //_ColaboradorEmpresa.AppendChild(_Cargo);

                    //XmlNode _Matricula = _xmlDocument.CreateElement("Matricula");
                    //_Matricula.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Matricula"].ToString())));
                    //_ColaboradorEmpresa.AppendChild(_Matricula);

                    XmlNode _Ativo = _xmlDocument.CreateElement("Ativo");
                    _Ativo.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativo"])).ToString()));
                    _ColaboradorEmpresa.AppendChild(_Ativo);

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

        private string RequisitaEmpresasLayoutsCrachas(int _colaboradorEmpresaID = 0)
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

                //string _SQL = "SELECT dbo.ColaboradoresEmpresas.ColaboradorEmpresaID, dbo.EmpresasLayoutsCrachas.EmpresaID, dbo.LayoutsCrachas.Nome, dbo.EmpresasLayoutsCrachas.LayoutCrachaID " +
                //    "FROM dbo.ColaboradoresEmpresas INNER JOIN dbo.EmpresasLayoutsCrachas ON dbo.ColaboradoresEmpresas.EmpresaID = dbo.EmpresasLayoutsCrachas.EmpresaID" +
                //    " INNER JOIN dbo.LayoutsCrachas ON dbo.EmpresasLayoutsCrachas.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID " +
                //    "WHERE dbo.ColaboradoresEmpresas.ColaboradorEmpresaID = " + _colaboradorEmpresaID ;

                string _SQL = "SELECT dbo.ColaboradoresEmpresas.ColaboradorEmpresaID, dbo.EmpresasLayoutsCrachas.EmpresaLayoutCrachaID, dbo.LayoutsCrachas.LayoutCrachaGUID," +
                    " dbo.LayoutsCrachas.Nome, dbo.EmpresasLayoutsCrachas.LayoutCrachaID" +
                    " FROM dbo.LayoutsCrachas INNER JOIN dbo.EmpresasLayoutsCrachas ON dbo.LayoutsCrachas.LayoutCrachaID = dbo.EmpresasLayoutsCrachas.LayoutCrachaID INNER JOIN" +
                    " dbo.ColaboradoresEmpresas ON dbo.EmpresasLayoutsCrachas.EmpresaID = dbo.ColaboradoresEmpresas.EmpresaID " +
                    "WHERE dbo.ColaboradoresEmpresas.ColaboradorEmpresaID = " + _colaboradorEmpresaID ;

                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _EmpresaLayoutCracha = _xmlDocument.CreateElement("EmpresaLayoutCracha");
                    _EmpresasLayoutsCrachas.AppendChild(_EmpresaLayoutCracha);

                    XmlNode _EmpresaLayoutCrachaID = _xmlDocument.CreateElement("EmpresaLayoutCrachaID");
                    _EmpresaLayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaLayoutCrachaID"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_EmpresaLayoutCrachaID);

                    //XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    //_EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaID"].ToString())));
                    //_EmpresaLayoutCracha.AppendChild(_EmpresaID);

                    XmlNode _LayoutCrachaGUID = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _LayoutCrachaGUID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaGUID"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_LayoutCrachaGUID);

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

                    XmlNode _FormatIDGUID = _xmlDocument.CreateElement("FormatIDGUID");
                    _FormatIDGUID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["FormatIDGUID"].ToString().Trim())));
                    _FormatoCredencial.AppendChild(_FormatIDGUID);
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
                    " AND(dbo.ColaboradoresEmpresas.ColaboradorID =" + ColaboradorSelecionadaID +")AND (dbo.ColaboradoresEmpresas.EmpresaID =" + _empresaID + ")";


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

        private string RequisitaCredenciaisMotivos(int tipo = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseCredenciaisMotivos = _xmlDocument.CreateElement("ClasseCredenciaisMotivos");
                _xmlDocument.AppendChild(_ClasseCredenciaisMotivos);

                XmlNode _CredenciaisMotivos = _xmlDocument.CreateElement("CredenciaisMotivos");
                _ClasseCredenciaisMotivos.AppendChild(_CredenciaisMotivos);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                if (tipo == 0)
                {
                    _strSql = "select * from CredenciaisMotivos ";
                }
                else
                {
                    _strSql = "select * from CredenciaisMotivos where Tipo= " + tipo;
                }
                

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _CredencialMotivo = _xmlDocument.CreateElement("CredencialMotivo");
                    _CredenciaisMotivos.AppendChild(_CredencialMotivo);

                    XmlNode _CredencialMotivoID = _xmlDocument.CreateElement("CredencialMotivoID");
                    _CredencialMotivoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CredencialMotivoID"].ToString().Trim())));
                    _CredencialMotivo.AppendChild(_CredencialMotivoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString().Trim())));
                    _CredencialMotivo.AppendChild(_Descricao);


                    XmlNode _Tipo = _xmlDocument.CreateElement("Tipo");
                    _Tipo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Tipo"].ToString().Trim())));
                    _CredencialMotivo.AppendChild(_Tipo);

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

        private string RequisitaAreasAcessos()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseAreasAcessos = _xmlDocument.CreateElement("ClasseAreasAcessos");
                _xmlDocument.AppendChild(_ClasseAreasAcessos);

                XmlNode _AreasAcessos = _xmlDocument.CreateElement("AreasAcessos");
                _ClasseAreasAcessos.AppendChild(_AreasAcessos);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from AreasAcessos order by AreaAcessoID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _AreaAcesso = _xmlDocument.CreateElement("AreaAcesso");
                    _AreasAcessos.AppendChild(_AreaAcesso);

                    XmlNode _AreaAcessoID = _xmlDocument.CreateElement("AreaAcessoID");
                    _AreaAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["AreaAcessoID"].ToString())));
                    _AreaAcesso.AppendChild(_AreaAcessoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _AreaAcesso.AppendChild(_Descricao);

                    XmlNode _Identificacao = _xmlDocument.CreateElement("Identificacao");
                    _Identificacao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Identificacao"].ToString())));
                    _AreaAcesso.AppendChild(_Identificacao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaAreasAcessos ex: " + ex);

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

        private int InsereColaboradorCredencialBD(string xmlString)
        {
            int _novID = 0;
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseColaboradoresCredenciais.ColaboradorCredencial _ColaboradorCredencial = new ClasseColaboradoresCredenciais.ColaboradorCredencial();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _ColaboradorCredencial.ColaboradorCredencialID = _xmlDoc.GetElementsByTagName("ColaboradorCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorCredencialID")[i].InnerText);
                //_ColaboradorCredencial.ColaboradorID = _xmlDoc.GetElementsByTagName("ColaboradorID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorID")[i].InnerText);
                //_ColaboradorCredencial.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
                _ColaboradorCredencial.ColaboradorEmpresaID = _xmlDoc.GetElementsByTagName("ColaboradorEmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorEmpresaID")[i].InnerText);
                //_ColaboradorCredencial.EmpresaContratoID = _xmlDoc.GetElementsByTagName("EmpresaContratoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaContratoID")[i].InnerText);
                _ColaboradorCredencial.TecnologiaCredencialID = _xmlDoc.GetElementsByTagName("TecnologiaCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TecnologiaCredencialID")[i].InnerText);
                _ColaboradorCredencial.TipoCredencialID = _xmlDoc.GetElementsByTagName("TipoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoCredencialID")[i].InnerText);
                _ColaboradorCredencial.LayoutCrachaID = _xmlDoc.GetElementsByTagName("LayoutCrachaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("LayoutCrachaID")[i].InnerText);
                _ColaboradorCredencial.FormatoCredencialID = _xmlDoc.GetElementsByTagName("FormatoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FormatoCredencialID")[i].InnerText);
                _ColaboradorCredencial.NumeroCredencial = _xmlDoc.GetElementsByTagName("NumeroCredencial")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroCredencial")[i].InnerText;
                _ColaboradorCredencial.FC = _xmlDoc.GetElementsByTagName("FC")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FC")[i].InnerText);
                //var teste = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText;
                _ColaboradorCredencial.Emissao = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Emissao")[i].InnerText);
                _ColaboradorCredencial.Validade = _xmlDoc.GetElementsByTagName("Validade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Validade")[i].InnerText);
                _ColaboradorCredencial.CredencialStatusID = _xmlDoc.GetElementsByTagName("CredencialStatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CredencialStatusID")[i].InnerText);
                _ColaboradorCredencial.CardHolderGuid = _xmlDoc.GetElementsByTagName("CardHolderGuid")[i].InnerText == "" ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(_xmlDoc.GetElementsByTagName("CardHolderGuid")[i].InnerText);
                _ColaboradorCredencial.CredencialGuid = _xmlDoc.GetElementsByTagName("CredencialGuid")[i].InnerText == "" ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(_xmlDoc.GetElementsByTagName("CredencialGuid")[i].InnerText);
                _ColaboradorCredencial.ColaboradorPrivilegio1ID = _xmlDoc.GetElementsByTagName("ColaboradorPrivilegio1ID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorPrivilegio1ID")[i].InnerText);
                _ColaboradorCredencial.ColaboradorPrivilegio2ID = _xmlDoc.GetElementsByTagName("ColaboradorPrivilegio2ID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorPrivilegio2ID")[i].InnerText);
                bool _ativa;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Ativa")[i].InnerText, out _ativa);
                _ColaboradorCredencial.Ativa = _xmlDoc.GetElementsByTagName("Ativa")[i] == null ? false : _ativa;
                bool _Impressa;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Impressa")[i].InnerText, out _Impressa);
                _ColaboradorCredencial.Impressa = _xmlDoc.GetElementsByTagName("Impressa")[i] == null ? false : _Impressa;
                _ColaboradorCredencial.Colete = _xmlDoc.GetElementsByTagName("Colete")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Colete")[i].InnerText;
                _ColaboradorCredencial.CredencialMotivoID = _xmlDoc.GetElementsByTagName("CredencialMotivoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CredencialMotivoID")[i].InnerText);
                _ColaboradorCredencial.Baixa = _xmlDoc.GetElementsByTagName("Baixa")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Baixa")[i].InnerText);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_ColaboradorCredencial.ColaboradorCredencialID != 0)
                {

                    _sqlCmd = new SqlCommand("Update ColaboradoresCredenciais Set " +
                            " ColaboradorPrivilegio1ID=@v1" +
                            ",CardHolderGUID=@v2" +
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
                            ",ColaboradorEmpresaID=@v13" +
                            ",Ativa=@v14" +
                            ",ColaboradorPrivilegio2ID=@v15" +
                            ",Colete=@v16" +
                            ",CredencialMotivoID=@v17" +
                            ",Baixa=@v18" +
                            ",Impressa=@v19" +
                            " Where ColaboradorCredencialID = @v0", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorCredencialID;
                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorPrivilegio1ID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.UniqueIdentifier).Value = _ColaboradorCredencial.CardHolderGuid;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.UniqueIdentifier).Value = _ColaboradorCredencial.CredencialGuid;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.Int).Value = _ColaboradorCredencial.TipoCredencialID;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _ColaboradorCredencial.TecnologiaCredencialID;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _ColaboradorCredencial.LayoutCrachaID;
                    _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _ColaboradorCredencial.FormatoCredencialID;
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _ColaboradorCredencial.NumeroCredencial;
                    _sqlCmd.Parameters.Add("@V9", SqlDbType.Int).Value = _ColaboradorCredencial.FC;
                    if (_ColaboradorCredencial.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _ColaboradorCredencial.Emissao;
                    }

                    if (_ColaboradorCredencial.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _ColaboradorCredencial.Validade;
                    }

                    _sqlCmd.Parameters.Add("@V12", SqlDbType.Int).Value = _ColaboradorCredencial.CredencialStatusID;
                    _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorEmpresaID;
                    _sqlCmd.Parameters.Add("@V14", SqlDbType.Bit).Value = _ColaboradorCredencial.Ativa;
                    _sqlCmd.Parameters.Add("@V15", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorPrivilegio2ID;
                    _sqlCmd.Parameters.Add("@V16", SqlDbType.NVarChar).Value = _ColaboradorCredencial.Colete;
                    _sqlCmd.Parameters.Add("@V17", SqlDbType.Int).Value = _ColaboradorCredencial.CredencialMotivoID;
                    if (_ColaboradorCredencial.Baixa == null)
                    {
                        _sqlCmd.Parameters.Add("@V18", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V18", SqlDbType.DateTime).Value = _ColaboradorCredencial.Baixa;
                    }
                    _sqlCmd.Parameters.Add("@V19", SqlDbType.Bit).Value = _ColaboradorCredencial.Impressa;

                    _sqlCmd.ExecuteNonQuery();
                    _novID = _ColaboradorCredencial.ColaboradorCredencialID;
                }
                else
                {
                    //ColaboradorID,EmpresaID,EmpresaContratoID,
                    _sqlCmd = new SqlCommand("Insert into ColaboradoresCredenciais(ColaboradorPrivilegio1ID,CardHolderGUID,CredencialGUID," +
                        "TipoCredencialID,TecnologiaCredencialID,LayoutCrachaID,FormatoCredencialID,NumeroCredencial,FC," +
                            "Emissao,Validade,CredencialStatusID,ColaboradorEmpresaID,Ativa,ColaboradorPrivilegio2ID," +
                            "Colete,CredencialMotivoID,Baixa,Impressa) " +
                            "values (@V1,@V2,@V3,@V4,@V5,@V6,@V7,@V8,@V9,@V10,@V11,@V12,@v13,@V14,@V15,@V16,@V17,@V18,@V19);SELECT SCOPE_IDENTITY();", _Con);

                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorPrivilegio1ID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.UniqueIdentifier).Value = _ColaboradorCredencial.CardHolderGuid;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.UniqueIdentifier).Value = _ColaboradorCredencial.CredencialGuid;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.Int).Value = _ColaboradorCredencial.TipoCredencialID;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _ColaboradorCredencial.TecnologiaCredencialID;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _ColaboradorCredencial.LayoutCrachaID;
                    _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _ColaboradorCredencial.FormatoCredencialID;
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _ColaboradorCredencial.NumeroCredencial;
                    _sqlCmd.Parameters.Add("@V9", SqlDbType.Int).Value = _ColaboradorCredencial.FC;
                    if (_ColaboradorCredencial.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _ColaboradorCredencial.Emissao;
                    }

                    if (_ColaboradorCredencial.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _ColaboradorCredencial.Validade;
                    }

                    _sqlCmd.Parameters.Add("@V12", SqlDbType.Int).Value = _ColaboradorCredencial.CredencialStatusID;
                    _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorEmpresaID;
                    _sqlCmd.Parameters.Add("@V14", SqlDbType.Bit).Value = _ColaboradorCredencial.Ativa;
                    _sqlCmd.Parameters.Add("@V15", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorPrivilegio2ID;
                    _sqlCmd.Parameters.Add("@V16", SqlDbType.VarChar).Value = _ColaboradorCredencial.Colete;
                    _sqlCmd.Parameters.Add("@V17", SqlDbType.Int).Value = _ColaboradorCredencial.CredencialMotivoID;

                    if (_ColaboradorCredencial.Baixa == null)
                    {
                        _sqlCmd.Parameters.Add("@V18", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V18", SqlDbType.DateTime).Value = _ColaboradorCredencial.Baixa;
                    }
                    _sqlCmd.Parameters.Add("@V19", SqlDbType.Bit).Value = _ColaboradorCredencial.Impressa;

                    _novID = Convert.ToInt32(_sqlCmd.ExecuteScalar());
                }

                _Con.Close();


            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereColaboradorCredencialBD ex: " + ex);
            }

            return _novID;
        }
        //private int InsereColaboradorCredencialBD(string xmlString)
        //{
        //    int _novID = 0;
        //    try
        //    {


        //        System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

        //        _xmlDoc.LoadXml(xmlString);
        //        // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
        //        ClasseColaboradoresCredenciais.ColaboradorCredencial _ColaboradorCredencial = new ClasseColaboradoresCredenciais.ColaboradorCredencial();
        //        //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
        //        //{
        //        int i = 0;

        //        _ColaboradorCredencial.ColaboradorCredencialID = _xmlDoc.GetElementsByTagName("ColaboradorCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorCredencialID")[i].InnerText);
        //        //_ColaboradorCredencial.ColaboradorID = _xmlDoc.GetElementsByTagName("ColaboradorID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorID")[i].InnerText);
        //        //_ColaboradorCredencial.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
        //        _ColaboradorCredencial.ColaboradorEmpresaID = _xmlDoc.GetElementsByTagName("ColaboradorEmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorEmpresaID")[i].InnerText);
        //        //_ColaboradorCredencial.EmpresaContratoID = _xmlDoc.GetElementsByTagName("EmpresaContratoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaContratoID")[i].InnerText);
        //        _ColaboradorCredencial.TecnologiaCredencialID = _xmlDoc.GetElementsByTagName("TecnologiaCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TecnologiaCredencialID")[i].InnerText);
        //        _ColaboradorCredencial.TipoCredencialID = _xmlDoc.GetElementsByTagName("TipoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoCredencialID")[i].InnerText);
        //        _ColaboradorCredencial.LayoutCrachaID = _xmlDoc.GetElementsByTagName("LayoutCrachaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("LayoutCrachaID")[i].InnerText);
        //        _ColaboradorCredencial.FormatoCredencialID = _xmlDoc.GetElementsByTagName("FormatoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FormatoCredencialID")[i].InnerText);
        //        _ColaboradorCredencial.NumeroCredencial = _xmlDoc.GetElementsByTagName("NumeroCredencial")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroCredencial")[i].InnerText;
        //        _ColaboradorCredencial.FC = _xmlDoc.GetElementsByTagName("FC")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FC")[i].InnerText);
        //        //var teste = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText;
        //        _ColaboradorCredencial.Emissao = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Emissao")[i].InnerText);
        //        _ColaboradorCredencial.Validade = _xmlDoc.GetElementsByTagName("Validade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Validade")[i].InnerText);
        //        _ColaboradorCredencial.CredencialStatusID = _xmlDoc.GetElementsByTagName("CredencialStatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CredencialStatusID")[i].InnerText);
        //        _ColaboradorCredencial.CardHolderGuid = _xmlDoc.GetElementsByTagName("CardHolderGuid")[i].InnerText == "" ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(_xmlDoc.GetElementsByTagName("CardHolderGuid")[i].InnerText);
        //        _ColaboradorCredencial.CredencialGuid = _xmlDoc.GetElementsByTagName("CredencialGuid")[i].InnerText == "" ? new Guid("00000000-0000-0000-0000-000000000000") : new Guid(_xmlDoc.GetElementsByTagName("CredencialGuid")[i].InnerText);
        //        _ColaboradorCredencial.ColaboradorPrivilegio1ID = _xmlDoc.GetElementsByTagName("ColaboradorPrivilegio1ID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorPrivilegio1ID")[i].InnerText);
        //        _ColaboradorCredencial.ColaboradorPrivilegio2ID = _xmlDoc.GetElementsByTagName("ColaboradorPrivilegio2ID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorPrivilegio2ID")[i].InnerText);
        //        bool _ativa;
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Ativa")[i].InnerText, out _ativa);
        //        _ColaboradorCredencial.Ativa = _xmlDoc.GetElementsByTagName("Ativa")[i] == null ? false : _ativa;
        //        bool _Impressa;
        //        Boolean.TryParse(_xmlDoc.GetElementsByTagName("Impressa")[i].InnerText, out _Impressa);
        //        _ColaboradorCredencial.Impressa = _xmlDoc.GetElementsByTagName("Impressa")[i] == null ? false : _Impressa;


        //        _ColaboradorCredencial.Colete = _xmlDoc.GetElementsByTagName("Colete")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Colete")[i].InnerText;
        //        _ColaboradorCredencial.CredencialMotivoID = _xmlDoc.GetElementsByTagName("CredencialMotivoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CredencialMotivoID")[i].InnerText);

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

        //        SqlCommand _sqlCmd;
        //        if (_ColaboradorCredencial.ColaboradorCredencialID != 0)
        //        {

        //            _sqlCmd = new SqlCommand("Update ColaboradoresCredenciais Set " +
        //                    " ColaboradorPrivilegio1ID=@v1" +
        //                    ",CardHolderGUID=@v2" +
        //                    ",CredencialGUID=@v3" +
        //                    ",TipoCredencialID=@v4" +
        //                    ",TecnologiaCredencialID=@v5" +
        //                    ",LayoutCrachaID=@v6" +
        //                    ",FormatoCredencialID=@v7" +
        //                    ",NumeroCredencial=@v8" +
        //                    ",FC=@v9" +
        //                    ",Emissao=@v10" +
        //                    ",Validade=@v11" +
        //                    ",CredencialStatusID=@v12" +
        //                    ",ColaboradorEmpresaID=@v13" +
        //                    ",Ativa=@v14" +
        //                    ",ColaboradorPrivilegio2ID=@v15" +
        //                    ",Colete=@v16" +
        //                    ",CredencialMotivoID=@v17" +
        //                    ",Impressa=@v18" +
        //                    " Where ColaboradorCredencialID = @v0", _Con);

        //            _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorCredencialID;
        //            _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorPrivilegio1ID;
        //            _sqlCmd.Parameters.Add("@V2", SqlDbType.UniqueIdentifier).Value = _ColaboradorCredencial.CardHolderGuid;
        //            _sqlCmd.Parameters.Add("@V3", SqlDbType.UniqueIdentifier).Value = _ColaboradorCredencial.CredencialGuid;
        //            _sqlCmd.Parameters.Add("@V4", SqlDbType.Int).Value = _ColaboradorCredencial.TipoCredencialID;
        //            _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _ColaboradorCredencial.TecnologiaCredencialID;
        //            _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _ColaboradorCredencial.LayoutCrachaID;
        //            _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _ColaboradorCredencial.FormatoCredencialID;
        //            _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _ColaboradorCredencial.NumeroCredencial;
        //            _sqlCmd.Parameters.Add("@V9", SqlDbType.Int).Value = _ColaboradorCredencial.FC;
        //            if (_ColaboradorCredencial.Emissao == null)
        //            {
        //                _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _ColaboradorCredencial.Emissao;
        //            }

        //            if (_ColaboradorCredencial.Validade == null)
        //            {
        //                _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _ColaboradorCredencial.Validade;
        //            }

        //            _sqlCmd.Parameters.Add("@V12", SqlDbType.Int).Value = _ColaboradorCredencial.CredencialStatusID;
        //            _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorEmpresaID;
        //            _sqlCmd.Parameters.Add("@V14", SqlDbType.Bit).Value = _ColaboradorCredencial.Ativa;
        //            _sqlCmd.Parameters.Add("@V15", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorPrivilegio2ID;
        //            _sqlCmd.Parameters.Add("@V16", SqlDbType.NVarChar).Value = _ColaboradorCredencial.Colete;
        //            _sqlCmd.Parameters.Add("@V17", SqlDbType.Int).Value = _ColaboradorCredencial.CredencialMotivoID;
        //            _sqlCmd.Parameters.Add("@V18", SqlDbType.Bit).Value = _ColaboradorCredencial.Impressa;

        //            _sqlCmd.ExecuteNonQuery();
        //            _novID = _ColaboradorCredencial.ColaboradorCredencialID;
        //        }
        //        else
        //        {
        //            //ColaboradorID,EmpresaID,EmpresaContratoID,
        //            _sqlCmd = new SqlCommand("Insert into ColaboradoresCredenciais(ColaboradorPrivilegio1ID,CardHolderGUID,CredencialGUID," +
        //                "TipoCredencialID,TecnologiaCredencialID,LayoutCrachaID,FormatoCredencialID,NumeroCredencial,FC," +
        //                    "Emissao,Validade,CredencialStatusID,ColaboradorEmpresaID,Ativa,ColaboradorPrivilegio2ID,Colete,CredencialMotivoID, Impressa) " +
        //                    "values (@V1,@V2,@V3,@V4,@V5,@V6,@V7,@V8,@V9,@V10,@V11,@V12,@v13,@V14,@V15,@V16,@V17,@V18);SELECT SCOPE_IDENTITY();", _Con);

        //            _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorPrivilegio1ID;
        //            _sqlCmd.Parameters.Add("@V2", SqlDbType.UniqueIdentifier).Value = _ColaboradorCredencial.CardHolderGuid;
        //            _sqlCmd.Parameters.Add("@V3", SqlDbType.UniqueIdentifier).Value = _ColaboradorCredencial.CredencialGuid;
        //            _sqlCmd.Parameters.Add("@V4", SqlDbType.Int).Value = _ColaboradorCredencial.TipoCredencialID;
        //            _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _ColaboradorCredencial.TecnologiaCredencialID;
        //            _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _ColaboradorCredencial.LayoutCrachaID;
        //            _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _ColaboradorCredencial.FormatoCredencialID;
        //            _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _ColaboradorCredencial.NumeroCredencial;
        //            _sqlCmd.Parameters.Add("@V9", SqlDbType.Int).Value = _ColaboradorCredencial.FC;
        //            if (_ColaboradorCredencial.Emissao == null)
        //            {
        //                _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _ColaboradorCredencial.Emissao;
        //            }

        //            if (_ColaboradorCredencial.Validade == null)
        //            {
        //                _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _ColaboradorCredencial.Validade;
        //            }

        //            _sqlCmd.Parameters.Add("@V12", SqlDbType.Int).Value = _ColaboradorCredencial.CredencialStatusID;
        //            _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorEmpresaID;
        //            _sqlCmd.Parameters.Add("@V14", SqlDbType.Bit).Value = _ColaboradorCredencial.Ativa;
        //            _sqlCmd.Parameters.Add("@V15", SqlDbType.Int).Value = _ColaboradorCredencial.ColaboradorPrivilegio2ID;
        //            _sqlCmd.Parameters.Add("@V16", SqlDbType.VarChar).Value = _ColaboradorCredencial.Colete;
        //            _sqlCmd.Parameters.Add("@V17", SqlDbType.Int).Value = _ColaboradorCredencial.CredencialMotivoID;
        //            _sqlCmd.Parameters.Add("@V18", SqlDbType.Bit).Value = _ColaboradorCredencial.Impressa;

        //            _novID = Convert.ToInt32(_sqlCmd.ExecuteScalar());
        //        }

        //        _Con.Close();


        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void InsereColaboradorCredencialBD ex: " + ex);
        //    }

        //    return _novID;
        //}

        private void InsereImpressaoDB(int colaboradorCredencialID)
        {
            try
            {
                if (colaboradorCredencialID != 0)
                {
                    SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                    SqlCommand _sqlCmd;


                    _sqlCmd = new SqlCommand("Insert into ColaboradoresCredenciaisImpressoes(ColaboradorCredencialID) values (@V1)", _Con);

                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = colaboradorCredencialID;

                    _sqlCmd.ExecuteNonQuery();

                    _sqlCmd = new SqlCommand("Update ColaboradoresCredenciais Set Impressa=@v1" +
                            " Where ColaboradorCredencialID = @v0", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = colaboradorCredencialID;

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

        private void ExcluiColaboradorCredencialBD(int _ColaboradorCredencialID) // alterar para xml
        {
            try
            {


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from ColaboradoresCredenciais where ColaboradorCredencialID=" + _ColaboradorCredencialID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiColaboradorCredencialBD ex: " + ex);


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


        #endregion

        #region Metodos Privados

        public void OnImprimirCommand()
        {
            try
            {
                if (ColaboradorCredencialSelecionado.Validade == null || !ColaboradorCredencialSelecionado.Ativa  ||
                    ColaboradorCredencialSelecionado.LayoutCrachaID == 0 )
                {
                    Global.PopupBox("Não é possível imprimir esta credencial!" , 4);
                    return;
                }

                //if (!Global.PopupBox("Confirma Impressão da Credencial para " + ColaboradorCredencialSelecionado.ColaboradorNome, 2))
                //{
                //    return;
                //}

                string _xml = RequisitaCredencial(ColaboradorCredencialSelecionado.ColaboradorCredencialID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseCredencial));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseCredencial Credencial = new ClasseCredencial();
                Credencial = (ClasseCredencial)deserializer.Deserialize(reader);


                string _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;
                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                byte[] buffer = Convert.FromBase64String(Credencial.LayoutRPT.Trim());

                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                //reportDocument.Load("D:\\Meus Documentos\\CrachaModelo - Motorista.rpt");
                reportDocument.Load(_ArquivoRPT);

                //var report = new Cracha();
                var x = new List<ClasseCredencial>();
                x.Add(Credencial);
                reportDocument.SetDataSource(x);

                //Thread CarregaCracha_thr = new Thread(() =>
                //{

                //    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                //    {

                        PopupCredencial _popupCredencial = new PopupCredencial(reportDocument);
                _popupCredencial.ShowDialog();

                bool _result = _popupCredencial.Result;
                //    });

                //}
                //);

                //CarregaCracha_thr.Start();

                // GenericReportViewer.ViewerCore.ReportSource = reportDocument;




                //bool _resposta = SCManager.ImprimirCredencial(ColaboradorCredencialSelecionado);
                //if (_resposta)
                //{
                if (_result)
                {

                        InsereImpressaoDB(ColaboradorCredencialSelecionado.ColaboradorCredencialID);
                       // Global.PopupBox("Impressão Efetuada com Sucesso!", 1);
                        ColaboradorCredencialSelecionado.Impressa = true;
                        int _selectindex = SelectedIndex;
                        CarregaColecaoColaboradoresCredenciais(ColaboradorCredencialSelecionado.ColaboradorID); //revisar a necessidade do carregamento
                        SelectedIndex = _selectindex;
                }
                System.IO.File.Delete(_ArquivoRPT);
                //}

            }
            catch (Exception ex)
            {
            }
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
                int idx=0;
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
