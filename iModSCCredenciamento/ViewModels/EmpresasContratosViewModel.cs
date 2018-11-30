﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using Utils = IMOD.CrossCutting.Utils;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasContratosViewModel : ViewModelBase
    {
        private IEmpresaContratosService _service = new EmpresaContratoService();
        private readonly IEstadoService _serviceEstado = new EstadoService();
        private  readonly  IMunicipioService _serviceMunicipio = new MunicipioService();
        /// <summary>
        /// Lista de municipios
        /// </summary>
        public List<ClasseMunicipios.Municipio> ListMunicipios { get; private set; }
        /// <summary>
        /// Lista de estados
        /// </summary>
        public List<ClasseEstados.Estado> ListEstados { get; private set; }
        #region  Metodos

        private string CriaXmlImagem(int empresaContratoID)
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseArquivosImagens = _xmlDocument.CreateElement ("ClasseArquivosImagens");
                _xmlDocument.AppendChild (_ClasseArquivosImagens);

                XmlNode _ArquivosImagens = _xmlDocument.CreateElement ("ArquivosImagens");
                _ClasseArquivosImagens.AppendChild (_ArquivosImagens);

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();

                var SQCMDXML = new SqlCommand ("Select * From EmpresasContratos Where EmpresaContratoID = " + empresaContratoID + "", _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader (CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement ("ArquivoImagem");
                    _ArquivosImagens.AppendChild (_ArquivoImagem);

                    //XmlNode _ArquivoImagemID = _xmlDocument.CreateElement("ArquivoImagemID");
                    //_ArquivoImagemID.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["EmpresaContratoID"].ToString())));
                    //_ArquivoImagem.AppendChild(_ArquivoImagemID);

                    XmlNode _Arquivo = _xmlDocument.CreateElement ("Arquivo");
                    _Arquivo.AppendChild (_xmlDocument.CreateTextNode (SQDR_XML["Arquivo"].ToString()));
                    _ArquivoImagem.AppendChild (_Arquivo);
                }
                SQDR_XML.Close();

                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void CriaXmlImagem ex: " + ex);
                return null;
            }
        }

        private void OnContratoSelecionado()
        {
            try
            {
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void OnEmpresaSelecionada ex: " + ex.Message);
            }
        }

        #endregion

        #region Inicializacao

        public EmpresasContratosViewModel()
        {
            CarregarDadosComunsEmMemoria();

            CarregaColecaoEstados();
            CarregaColecaoStatuss();
            CarregaColeçãoTiposAcessos();
            CarregaColeçãoTiposCobrancas();
        }

        private void CarregarDadosComunsEmMemoria()
        {
            try
            {
                //Estados
                var e1 = _serviceEstado.Listar();
                ListEstados = Mapper.Map<List<ClasseEstados.Estado>>(e1);
                //Municipios
                var list = _serviceMunicipio.Listar();
                ListMunicipios = Mapper.Map<List<ClasseMunicipios.Municipio>>(list);
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> _Contratos;

        private ClasseEmpresasContratos.EmpresaContrato _ContratoSelecionado;

        private ClasseEmpresasContratos.EmpresaContrato _contratoTemp = new ClasseEmpresasContratos.EmpresaContrato();

        private readonly List<ClasseEmpresasContratos.EmpresaContrato> _ContratosTemp = new List<ClasseEmpresasContratos.EmpresaContrato>();

        private ObservableCollection<ClasseEstados.Estado> _Estados;

        private ObservableCollection<ClasseMunicipios.Municipio> _Municipios;

        private ObservableCollection<ClasseStatus.Status> _Statuss;

        private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _TiposAcessos;

        private ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _TiposCobrancas;

        private PopupPesquisaContrato popupPesquisaContrato;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Contrutores

        public ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> Contratos
        {
            get { return _Contratos; }

            set
            {
                if (_Contratos != value)
                {
                    _Contratos = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseEstados.Estado> Estados
        {
            get { return _Estados; }

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
            get { return _Municipios; }

            set
            {
                if (_Municipios != value)
                {
                    _Municipios = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseStatus.Status> Statuss
        {
            get { return _Statuss; }

            set
            {
                if (_Statuss != value)
                {
                    _Statuss = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseTiposAcessos.TipoAcesso> TiposAcessos
        {
            get { return _TiposAcessos; }

            set
            {
                if (_TiposAcessos != value)
                {
                    _TiposAcessos = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ClasseTiposCobrancas.TipoCobranca> TiposCobrancas
        {
            get { return _TiposCobrancas; }

            set
            {
                if (_TiposCobrancas != value)
                {
                    _TiposCobrancas = value;
                    OnPropertyChanged();
                }
            }
        }

        public ClasseEmpresasContratos.EmpresaContrato ContratoSelecionado
        {
            get { return _ContratoSelecionado; }
            set
            {
                _ContratoSelecionado = value;
                OnPropertyChanged ("SelectedItem");
                if (ContratoSelecionado != null)
                {
                    OnContratoSelecionado();
                }
            }
        }

        public int EmpresaSelecionadaID
        {
            get { return _EmpresaSelecionadaID; }
            set
            {
                _EmpresaSelecionadaID = value;
                OnPropertyChanged();
                if (EmpresaSelecionadaID != null)
                {
                    //OnEmpresaSelecionada();
                }
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged ("SelectedIndex");
            }
        }

        public bool HabilitaEdicao
        {
            get { return _HabilitaEdicao; }
            set
            {
                _HabilitaEdicao = value;
                OnPropertyChanged();
            }
        }

        public string Criterios
        {
            get { return _Criterios; }
            set
            {
                _Criterios = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Comandos dos Botoes

        public void OnAtualizaCommand(object empresaID)
        {
            EmpresaSelecionadaID = Convert.ToInt32 (empresaID);
            var CarregaColecaoContratos_thr = new Thread (() => CarregaColecaoContratos (Convert.ToInt32 (empresaID)));
            CarregaColecaoContratos_thr.Start();
            //CarregaColecaoContratos(Convert.ToInt32(empresaID));
        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                var _arquivoPDF = new OpenFileDialog();

                string _nomecompletodoarquivo;
                string _arquivoSTR;
                _arquivoPDF.InitialDirectory = "c:\\\\";
                _arquivoPDF.Filter = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                _arquivoPDF.RestoreDirectory = true;
                _arquivoPDF.ShowDialog();

                _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString (_arquivoPDF.FileName);
                _contratoTemp.NomeArquivo = _nomecompletodoarquivo;
                _contratoTemp.Arquivo = _arquivoSTR;

                if (Contratos != null)
                {
                    Contratos[0].NomeArquivo = _nomecompletodoarquivo;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void OnAbrirArquivoCommand()
        {
            try
            {
                try
                {
                    string _ArquivoPDF = null;
                    if (_contratoTemp != null)
                    {
                        if (_contratoTemp.Arquivo != null && _contratoTemp.EmpresaContratoID == ContratoSelecionado.EmpresaContratoID)
                        {
                            _ArquivoPDF = _contratoTemp.Arquivo;
                        }
                    }
                    if (_ArquivoPDF == null)
                    {
                        var _xmlstring = CriaXmlImagem (ContratoSelecionado.EmpresaContratoID);

                        var xmldocument = new XmlDocument();
                        xmldocument.LoadXml (_xmlstring);
                        XmlNode node = xmldocument.DocumentElement;
                        var arquivoNode = node.SelectSingleNode ("ArquivosImagens/ArquivoImagem/Arquivo");

                        _ArquivoPDF = arquivoNode.FirstChild.Value;
                    }
                    Global.PopupPDF (_ArquivoPDF);
                    //byte[] buffer = Conversores.StringToPDF(_ArquivoPDF);
                    //_ArquivoPDF = System.IO.Path.GetTempFileName();
                    //_ArquivoPDF = System.IO.Path.GetRandomFileName();
                    //_ArquivoPDF = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoPDF;

                    ////File.Move(_caminhoArquivoPDF, Path.ChangeExtension(_caminhoArquivoPDF, ".pdf"));
                    //_ArquivoPDF = System.IO.Path.ChangeExtension(_ArquivoPDF, ".pdf");
                    //System.IO.File.WriteAllBytes(_ArquivoPDF, buffer);
                    ////Action<string> act = new Action<string>(Global.AbrirArquivoPDF);
                    ////act.BeginInvoke(_ArquivoPDF, null, null);
                    //Global.PopupPDF(_ArquivoPDF);
                    //System.IO.File.Delete(_ArquivoPDF);
                }
                catch (Exception ex)
                {
                    Global.Log ("Erro na void ListaARQColaboradorAnexo_lv_PreviewMouseDoubleClick ex: " + ex);
                }
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
                _contratoTemp = ContratoSelecionado.CriaCopia (ContratoSelecionado);
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
                Contratos[_selectedIndexTemp] = _contratoTemp;
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
                HabilitaEdicao = false;
                var serializer = new XmlSerializer (typeof(ClasseEmpresasContratos));

                var _EmpresasContratosTemp = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>();
                var _ClasseEmpresasContratosTemp = new ClasseEmpresasContratos();
                _EmpresasContratosTemp.Add (ContratoSelecionado);
                _ClasseEmpresasContratosTemp.EmpresasContratos = _EmpresasContratosTemp;

                string xmlString;

                using (var sw = new StringWriterWithEncoding (Encoding.UTF8))
                {
                    using (var xw = new XmlTextWriter (sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize (xw, _ClasseEmpresasContratosTemp);
                        xmlString = sw.ToString();
                    }
                }

                InsereContratoBD (xmlString);

                _ClasseEmpresasContratosTemp = null;

                _ContratosTemp.Clear();
                //_contratoTemp = null;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Contratos)
                {
                    _ContratosTemp.Add (x);
                }

                _selectedIndexTemp = SelectedIndex;
                Contratos.Clear();
                //ClasseEmpresasContratos.EmpresaContrato _contrato = new ClasseEmpresasContratos.EmpresaContrato();
                //_contrato.EmpresaID = EmpresaSelecionadaID;
                //Contratos.Add(_contrato);
                _contratoTemp = new ClasseEmpresasContratos.EmpresaContrato();
                _contratoTemp.EmpresaID = EmpresaSelecionadaID;
                Contratos.Add (_contratoTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;
                var serializer = new XmlSerializer (typeof(ClasseEmpresasContratos));

                var _EmpresasContratosPro = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>();
                var _ClasseEmpresasContratosPro = new ClasseEmpresasContratos();
                _EmpresasContratosPro.Add (ContratoSelecionado);
                _ClasseEmpresasContratosPro.EmpresasContratos = _EmpresasContratosPro;

                string xmlString;

                using (var sw = new StringWriterWithEncoding (Encoding.UTF8))
                {
                    using (var xw = new XmlTextWriter (sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize (xw, _ClasseEmpresasContratosPro);
                        xmlString = sw.ToString();
                    }
                }

                InsereContratoBD (xmlString);
                var CarregaColecaoContratos_thr = new Thread (() => CarregaColecaoContratos (ContratoSelecionado.EmpresaID));
                CarregaColecaoContratos_thr.Start();
                //_ContratosTemp.Add(ContratoSelecionado);
                //Contratos = null;
                //Contratos = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>(_ContratosTemp);
                //SelectedIndex = _selectedIndexTemp;
                //_ContratosTemp.Clear();
                //_ClasseEmpresasContratosPro = null;

                //_ContratosTemp.Clear();
                ////_contratoTemp = null;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Contratos = null;
                Contratos = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> (_ContratosTemp);
                SelectedIndex = _selectedIndexTemp;
                _ContratosTemp.Clear();
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
                //if (MessageBox.Show("Tem certeza que deseja excluir este contrato?", "Excluir Contrato", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados deste contrato, inclusive histórico. Confirma exclusão?", "Excluir Contrato", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        ExcluiContratoBD(ContratoSelecionado.EmpresaContratoID);
                //        Contratos.Remove(ContratoSelecionado);

                //    }
                //}

                if (Global.PopupBox ("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox ("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiContratoBD (ContratoSelecionado.EmpresaContratoID);
                        Contratos.Remove (ContratoSelecionado);
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
                popupPesquisaContrato = new PopupPesquisaContrato();
                popupPesquisaContrato.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaContrato.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaContrato.Criterio.Split ((char) 20);
            var _empresaID = EmpresaSelecionadaID;
            var _descricao = ((string[]) vetor)[0];
            var _numerocontrato = ((string[]) vetor)[1];
            CarregaColecaoContratos (_empresaID, _descricao, _numerocontrato);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes

        private void CarregaColecaoContratos(int empresaID, string _seguradora = "", string _numeroapolice = "")
        {
            try
            {
                var _xml = RequisitaContratos (empresaID, _seguradora, _numeroapolice);

                var deserializer = new XmlSerializer (typeof(ClasseEmpresasContratos));

                var xmldocument = new XmlDocument();
                xmldocument.LoadXml (_xml);

                TextReader reader = new StringReader (_xml);
                var classeContratosEmpresa = new ClasseEmpresasContratos();
                classeContratosEmpresa = (ClasseEmpresasContratos) deserializer.Deserialize (reader);
                Contratos = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>();
                Contratos = classeContratosEmpresa.EmpresasContratos;
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoEstados()
        {
            try
            {
                var convert = Mapper.Map<List<ClasseEstados.Estado>> (ListEstados);
                Estados = new ObservableCollection<ClasseEstados.Estado>();
                convert.ForEach (n => { Estados.Add (n); });
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoMunicipios(string uf)
        {
            try
            {
                var list = ListMunicipios.Where (n => n.UF == uf).ToList();
                Municipios = new ObservableCollection<ClasseMunicipios.Municipio>();
                list.ForEach(n=>Municipios.Add(n));

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoStatuss()
        {
            try
            {
                var _xml = RequisitaStatuss();

                var deserializer = new XmlSerializer (typeof(ClasseStatus));
                var xmldocument = new XmlDocument();
                xmldocument.LoadXml (_xml);
                TextReader reader = new StringReader (_xml);
                var classeStatus = new ClasseStatus();
                classeStatus = (ClasseStatus) deserializer.Deserialize (reader);
                Statuss = new ObservableCollection<ClasseStatus.Status>();
                Statuss = classeStatus.Statuss;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void CarregaColeçãoStatuss ex: " + ex);
            }
        }

        private void CarregaColeçãoTiposAcessos()
        {
            try
            {
                var _xml = RequisitaTiposAcessos();

                var deserializer = new XmlSerializer (typeof(ClasseTiposAcessos));
                var xmldocument = new XmlDocument();
                xmldocument.LoadXml (_xml);
                TextReader reader = new StringReader (_xml);
                var classeTiposAcessos = new ClasseTiposAcessos();
                classeTiposAcessos = (ClasseTiposAcessos) deserializer.Deserialize (reader);
                TiposAcessos = new ObservableCollection<ClasseTiposAcessos.TipoAcesso>();
                TiposAcessos = classeTiposAcessos.TiposAcessos;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void CarregaColeçãoTiposAcessos ex: " + ex);
            }
        }

        private void CarregaColeçãoTiposCobrancas()
        {
            try
            {
                var _xml = RequisitaTiposCobrancas();

                var deserializer = new XmlSerializer (typeof(ClasseTiposCobrancas));
                var xmldocument = new XmlDocument();
                xmldocument.LoadXml (_xml);
                TextReader reader = new StringReader (_xml);
                var classeTiposCobrancas = new ClasseTiposCobrancas();
                classeTiposCobrancas = (ClasseTiposCobrancas) deserializer.Deserialize (reader);
                TiposCobrancas = new ObservableCollection<ClasseTiposCobrancas.TipoCobranca>();
                TiposCobrancas = classeTiposCobrancas.TiposCobrancas;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void CarregaColeçãoTiposCobrancas ex: " + ex);
            }
        }

        #endregion

        #region Data Access

        private string RequisitaContratos(int _empresaID, string _descricao = "", string _numerocontrato = "")
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseContratosEmpresas = _xmlDocument.CreateElement ("ClasseEmpresasContratos");
                _xmlDocument.AppendChild (_ClasseContratosEmpresas);

                XmlNode _EmpresasContratos = _xmlDocument.CreateElement ("EmpresasContratos");
                _ClasseContratosEmpresas.AppendChild (_EmpresasContratos);

                string _strSql;

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();

                _descricao = "%" + _descricao + "%";
                _numerocontrato = "%" + _numerocontrato + "%";

                _strSql = "select * from EmpresasContratos where EmpresaID = " + _empresaID + " and Descricao Like '" +
                          _descricao + "' and NumeroContrato Like '" + _numerocontrato + "'  order by EmpresaContratoID desc";

                var _sqlcmd = new SqlCommand (_strSql, _Con);
                var _sqlreader = _sqlcmd.ExecuteReader (CommandBehavior.Default);
                while (_sqlreader.Read())
                {
                    XmlNode _EmpresaContrato = _xmlDocument.CreateElement ("EmpresaContrato");
                    _EmpresasContratos.AppendChild (_EmpresaContrato);

                    XmlNode _EmpresaContratoID = _xmlDocument.CreateElement ("EmpresaContratoID");
                    _EmpresaContratoID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EmpresaContratoID"].ToString()));
                    _EmpresaContrato.AppendChild (_EmpresaContratoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement ("EmpresaID");
                    _EmpresaID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EmpresaID"].ToString()));
                    _EmpresaContrato.AppendChild (_EmpresaID);

                    XmlNode _NumeroContrato = _xmlDocument.CreateElement ("NumeroContrato");
                    _NumeroContrato.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["NumeroContrato"].ToString()));
                    _EmpresaContrato.AppendChild (_NumeroContrato);

                    XmlNode _Descricao = _xmlDocument.CreateElement ("Descricao");
                    _Descricao.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Descricao"].ToString()));
                    _EmpresaContrato.AppendChild (_Descricao);

                    var dateStr = _sqlreader["Emissao"].ToString();
                    if (!string.IsNullOrWhiteSpace (dateStr))
                    {
                        var dt2 = Convert.ToDateTime (dateStr);
                        XmlNode _Emissao = _xmlDocument.CreateElement ("Emissao");
                        _Emissao.AppendChild (_xmlDocument.CreateTextNode (dt2.ToString ("yyyy-MM-ddTHH:mm:ss")));
                        _EmpresaContrato.AppendChild (_Emissao);
                    }

                    dateStr = _sqlreader["Validade"].ToString();
                    if (!string.IsNullOrWhiteSpace (dateStr))
                    {
                        var dt2 = Convert.ToDateTime (dateStr);
                        XmlNode _Validade = _xmlDocument.CreateElement ("Validade");
                        _Validade.AppendChild (_xmlDocument.CreateTextNode (dt2.ToString ("yyyy-MM-ddTHH:mm:ss")));
                        _EmpresaContrato.AppendChild (_Validade);
                    }

                    XmlNode _Terceirizada = _xmlDocument.CreateElement ("Terceirizada");
                    _Terceirizada.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Terceirizada"].ToString()));
                    _EmpresaContrato.AppendChild (_Terceirizada);

                    XmlNode _Contratante = _xmlDocument.CreateElement ("Contratante");
                    _Contratante.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Contratante"].ToString()));
                    _EmpresaContrato.AppendChild (_Contratante);

                    XmlNode _IsencaoCobranca = _xmlDocument.CreateElement ("IsencaoCobranca");
                    _IsencaoCobranca.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["IsencaoCobranca"].ToString()));
                    _EmpresaContrato.AppendChild (_IsencaoCobranca);

                    XmlNode _TipoCobrancaID = _xmlDocument.CreateElement ("TipoCobrancaID");
                    _TipoCobrancaID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["TipoCobrancaID"].ToString()));
                    _EmpresaContrato.AppendChild (_TipoCobrancaID);

                    XmlNode _CobrancaEmpresaID = _xmlDocument.CreateElement ("CobrancaEmpresaID");
                    _CobrancaEmpresaID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["CobrancaEmpresaID"].ToString()));
                    _EmpresaContrato.AppendChild (_CobrancaEmpresaID);

                    XmlNode _CEP = _xmlDocument.CreateElement ("CEP");
                    _CEP.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["CEP"].ToString()));
                    _EmpresaContrato.AppendChild (_CEP);

                    XmlNode _Endereco = _xmlDocument.CreateElement ("Endereco");
                    _Endereco.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Endereco"].ToString()));
                    _EmpresaContrato.AppendChild (_Endereco);

                    XmlNode _Complemento = _xmlDocument.CreateElement ("Complemento");
                    _Complemento.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Complemento"].ToString()));
                    _EmpresaContrato.AppendChild (_Complemento);

                    XmlNode _Bairro = _xmlDocument.CreateElement ("Bairro");
                    _Bairro.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Bairro"].ToString()));
                    _EmpresaContrato.AppendChild (_Bairro);

                    XmlNode _MunicipioID = _xmlDocument.CreateElement ("MunicipioID");
                    _MunicipioID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["MunicipioID"].ToString()));
                    _EmpresaContrato.AppendChild (_MunicipioID);

                    XmlNode _EstadoID = _xmlDocument.CreateElement ("EstadoID");
                    _EstadoID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EstadoID"].ToString()));
                    _EmpresaContrato.AppendChild (_EstadoID);

                    XmlNode _NomeResp = _xmlDocument.CreateElement ("NomeResp");
                    _NomeResp.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["NomeResp"].ToString()));
                    _EmpresaContrato.AppendChild (_NomeResp);

                    XmlNode _TelefoneResp = _xmlDocument.CreateElement ("TelefoneResp");
                    _TelefoneResp.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["TelefoneResp"].ToString()));
                    _EmpresaContrato.AppendChild (_TelefoneResp);

                    XmlNode _CelularResp = _xmlDocument.CreateElement ("CelularResp");
                    _CelularResp.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["CelularResp"].ToString()));
                    _EmpresaContrato.AppendChild (_CelularResp);

                    XmlNode _EmailResp = _xmlDocument.CreateElement ("EmailResp");
                    _EmailResp.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EmailResp"].ToString()));
                    _EmpresaContrato.AppendChild (_EmailResp);

                    XmlNode _Numero = _xmlDocument.CreateElement ("Numero");
                    _Numero.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Numero"].ToString()));
                    _EmpresaContrato.AppendChild (_Numero);

                    XmlNode _StatusID = _xmlDocument.CreateElement ("StatusID");
                    _StatusID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["StatusID"].ToString()));
                    _EmpresaContrato.AppendChild (_StatusID);

                    XmlNode _TipoAcessoID = _xmlDocument.CreateElement ("TipoAcessoID");
                    _TipoAcessoID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["TipoAcessoID"].ToString()));
                    _EmpresaContrato.AppendChild (_TipoAcessoID);

                    XmlNode _NomeArquivo = _xmlDocument.CreateElement ("NomeArquivo");
                    _NomeArquivo.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["NomeArquivo"].ToString()));
                    _EmpresaContrato.AppendChild (_NomeArquivo);

                    XmlNode _Arquivo = _xmlDocument.CreateElement ("Arquivo");
                    //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Arquivo"].ToString())));
                    _EmpresaContrato.AppendChild (_Arquivo);
                }

                _sqlreader.Close();

                _Con.Close();
                var _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch
            {
                return null;
            }
            return null;
        }

        //private string RequisitaContratos(int _empresaID, string _descricao = "", string _numerocontrato = "")
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseContratosEmpresas = _xmlDocument.CreateElement("ClasseEmpresasContratos");
        //        _xmlDocument.AppendChild(_ClasseContratosEmpresas);

        //        XmlNode _EmpresasContratos = _xmlDocument.CreateElement("EmpresasContratos");
        //        _ClasseContratosEmpresas.AppendChild(_EmpresasContratos);

        //        string _strSql;

        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

        //        _descricao = "%" + _descricao + "%";
        //        _numerocontrato = "%" + _numerocontrato + "%";

        //        _strSql = "select * from EmpresasContratos where EmpresaID = " + _empresaID + " and Descricao Like '" +
        //            _descricao + "' and NumeroContrato Like '" + _numerocontrato + "'  order by EmpresaContratoID desc";

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _EmpresaContrato = _xmlDocument.CreateElement("EmpresaContrato");
        //            _EmpresasContratos.AppendChild(_EmpresaContrato);

        //            XmlNode _EmpresaContratoID = _xmlDocument.CreateElement("EmpresaContratoID");
        //            _EmpresaContratoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaContratoID"].ToString())));
        //            _EmpresaContrato.AppendChild(_EmpresaContratoID);

        //            XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
        //            _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
        //            _EmpresaContrato.AppendChild(_EmpresaID);

        //            XmlNode _NumeroContrato = _xmlDocument.CreateElement("NumeroContrato");
        //            _NumeroContrato.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroContrato"].ToString())));
        //            _EmpresaContrato.AppendChild(_NumeroContrato);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
        //            _EmpresaContrato.AppendChild(_Descricao);

        //            XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
        //            _Emissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Emissao"].ToString())));
        //            _EmpresaContrato.AppendChild(_Emissao);

        //            XmlNode _Validade = _xmlDocument.CreateElement("Validade");
        //            _Validade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Validade"].ToString())));
        //            _EmpresaContrato.AppendChild(_Validade);

        //            XmlNode _Terceirizada = _xmlDocument.CreateElement("Terceirizada");
        //            _Terceirizada.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Terceirizada"].ToString())));
        //            _EmpresaContrato.AppendChild(_Terceirizada);

        //            XmlNode _Contratante = _xmlDocument.CreateElement("Contratante");
        //            _Contratante.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contratante"].ToString())));
        //            _EmpresaContrato.AppendChild(_Contratante);

        //            XmlNode _IsencaoCobranca = _xmlDocument.CreateElement("IsencaoCobranca");
        //            _IsencaoCobranca.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["IsencaoCobranca"].ToString())));
        //            _EmpresaContrato.AppendChild(_IsencaoCobranca);

        //            XmlNode _TipoCobrancaID = _xmlDocument.CreateElement("TipoCobrancaID");
        //            _TipoCobrancaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoCobrancaID"].ToString())));
        //            _EmpresaContrato.AppendChild(_TipoCobrancaID);

        //            XmlNode _CobrancaEmpresaID = _xmlDocument.CreateElement("CobrancaEmpresaID");
        //            _CobrancaEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CobrancaEmpresaID"].ToString())));
        //            _EmpresaContrato.AppendChild(_CobrancaEmpresaID);

        //            XmlNode _CEP = _xmlDocument.CreateElement("CEP");
        //            _CEP.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CEP"].ToString())));
        //            _EmpresaContrato.AppendChild(_CEP);

        //            XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
        //            _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
        //            _EmpresaContrato.AppendChild(_Endereco);

        //            XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
        //            _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
        //            _EmpresaContrato.AppendChild(_Complemento);

        //            XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
        //            _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
        //            _EmpresaContrato.AppendChild(_Bairro);

        //            XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
        //            _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
        //            _EmpresaContrato.AppendChild(_MunicipioID);

        //            XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
        //            _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
        //            _EmpresaContrato.AppendChild(_EstadoID);

        //            XmlNode _NomeResp = _xmlDocument.CreateElement("NomeResp");
        //            _NomeResp.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeResp"].ToString())));
        //            _EmpresaContrato.AppendChild(_NomeResp);

        //            XmlNode _TelefoneResp = _xmlDocument.CreateElement("TelefoneResp");
        //            _TelefoneResp.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneResp"].ToString())));
        //            _EmpresaContrato.AppendChild(_TelefoneResp);

        //            XmlNode _CelularResp = _xmlDocument.CreateElement("CelularResp");
        //            _CelularResp.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CelularResp"].ToString())));
        //            _EmpresaContrato.AppendChild(_CelularResp);

        //            XmlNode _EmailResp = _xmlDocument.CreateElement("EmailResp");
        //            _EmailResp.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmailResp"].ToString())));
        //            _EmpresaContrato.AppendChild(_EmailResp);

        //            XmlNode _Numero = _xmlDocument.CreateElement("Numero");
        //            _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
        //            _EmpresaContrato.AppendChild(_Numero);

        //            XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
        //            _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
        //            _EmpresaContrato.AppendChild(_StatusID);

        //            XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
        //            _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
        //            _EmpresaContrato.AppendChild(_TipoAcessoID);

        //            XmlNode _NomeArquivo = _xmlDocument.CreateElement("NomeArquivo");
        //            _NomeArquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivo"].ToString())));
        //            _EmpresaContrato.AppendChild(_NomeArquivo);

        //            XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
        //            //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Arquivo"].ToString())));
        //            _EmpresaContrato.AppendChild(_Arquivo);

        //        }

        //        _sqlreader.Close();

        //        _Con.Close();
        //        string _xml = _xmlDocument.InnerXml;
        //        _xmlDocument = null;
        //        return _xml;
        //    }
        //    catch
        //    {

        //        return null;
        //    }
        //    return null;
        //}

        private string RequisitaEstados()
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseEstados = _xmlDocument.CreateElement ("ClasseEstados");
                _xmlDocument.AppendChild (_ClasseEstados);

                XmlNode _Estados = _xmlDocument.CreateElement ("Estados");
                _ClasseEstados.AppendChild (_Estados);

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();
                var _sqlcmd = new SqlCommand ("select * from Estados", _Con);
                var _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Estado = _xmlDocument.CreateElement ("Estado");
                    _Estados.AppendChild (_Estado);

                    XmlNode _EstadoID = _xmlDocument.CreateElement ("EstadoID");
                    _EstadoID.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["EstadoID"].ToString()));
                    _Estado.AppendChild (_EstadoID);

                    XmlNode _Nome = _xmlDocument.CreateElement ("Nome");
                    _Nome.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["Nome"].ToString()));
                    _Estado.AppendChild (_Nome);

                    XmlNode _UF = _xmlDocument.CreateElement ("UF");
                    _UF.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["UF"].ToString()));
                    _Estado.AppendChild (_UF);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void RequisitaEstados ex: " + ex);

                return null;
            }
        }

        private string RequisitaMunicipios(string _EstadoUF = "%")
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseMunicipios = _xmlDocument.CreateElement ("ClasseMunicipios");
                _xmlDocument.AppendChild (_ClasseMunicipios);

                XmlNode _Municipios = _xmlDocument.CreateElement ("Municipios");
                _ClasseMunicipios.AppendChild (_Municipios);

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();
                var _sqlcmd = new SqlCommand ("select * from Municipios where UF Like '" + _EstadoUF + "'", _Con);
                var _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Municipio = _xmlDocument.CreateElement ("Municipio");
                    _Municipios.AppendChild (_Municipio);

                    XmlNode _MunicipioID = _xmlDocument.CreateElement ("MunicipioID");
                    _MunicipioID.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["MunicipioID"].ToString()));
                    _Municipio.AppendChild (_MunicipioID);

                    XmlNode _Nome = _xmlDocument.CreateElement ("Nome");
                    _Nome.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["Nome"].ToString()));
                    _Municipio.AppendChild (_Nome);

                    XmlNode _UF = _xmlDocument.CreateElement ("UF");
                    _UF.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["UF"].ToString()));
                    _Municipio.AppendChild (_UF);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void RequisitaMunicipios ex: " + ex);

                return null;
            }
        }

        private string RequisitaStatuss()
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseStatus = _xmlDocument.CreateElement ("ClasseStatus");
                _xmlDocument.AppendChild (_ClasseStatus);

                XmlNode _Statuss = _xmlDocument.CreateElement ("Statuss");
                _ClasseStatus.AppendChild (_Statuss);

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();
                var _sqlcmd = new SqlCommand ("select * from Status", _Con);
                var _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Status = _xmlDocument.CreateElement ("Status");
                    _Statuss.AppendChild (_Status);

                    XmlNode _StatusID = _xmlDocument.CreateElement ("StatusID");
                    _StatusID.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["StatusID"].ToString()));
                    _Status.AppendChild (_StatusID);

                    XmlNode _Descricao = _xmlDocument.CreateElement ("Descricao");
                    _Descricao.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["Descricao"].ToString()));
                    _Status.AppendChild (_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void RequisitaStatus ex: " + ex);

                return null;
            }
        }

        private string RequisitaTiposAcessos()
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseTiposAcessos = _xmlDocument.CreateElement ("ClasseTiposAcessos");
                _xmlDocument.AppendChild (_ClasseTiposAcessos);

                XmlNode _TiposAcessos = _xmlDocument.CreateElement ("TiposAcessos");
                _ClasseTiposAcessos.AppendChild (_TiposAcessos);

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();
                var _sqlcmd = new SqlCommand ("select * from TiposAcessos", _Con);
                var _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoAcesso = _xmlDocument.CreateElement ("TipoAcesso");
                    _TiposAcessos.AppendChild (_TipoAcesso);

                    XmlNode _TipoAcessoID = _xmlDocument.CreateElement ("TipoAcessoID");
                    _TipoAcessoID.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["TipoAcessoID"].ToString()));
                    _TipoAcesso.AppendChild (_TipoAcessoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement ("Descricao");
                    _Descricao.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["Descricao"].ToString()));
                    _TipoAcesso.AppendChild (_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void RequisitaTipoAcesso ex: " + ex);

                return null;
            }
        }

        private string RequisitaTiposCobrancas()
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseTiposCobrancas = _xmlDocument.CreateElement ("ClasseTiposCobrancas");
                _xmlDocument.AppendChild (_ClasseTiposCobrancas);

                XmlNode _TiposCobrancas = _xmlDocument.CreateElement ("TiposCobrancas");
                _ClasseTiposCobrancas.AppendChild (_TiposCobrancas);

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();
                var _sqlcmd = new SqlCommand ("select * from TiposCobrancas", _Con);
                var _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoCobranca = _xmlDocument.CreateElement ("TipoCobranca");
                    _TiposCobrancas.AppendChild (_TipoCobranca);

                    XmlNode _TipoCobrancaID = _xmlDocument.CreateElement ("TipoCobrancaID");
                    _TipoCobrancaID.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["TipoCobrancaID"].ToString()));
                    _TipoCobranca.AppendChild (_TipoCobrancaID);

                    XmlNode _Descricao = _xmlDocument.CreateElement ("Descricao");
                    _Descricao.AppendChild (_xmlDocument.CreateTextNode (_sqldatareader["Descricao"].ToString()));
                    _TipoCobranca.AppendChild (_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void RequisitaTipoCobranca ex: " + ex);

                return null;
            }
        }

        private void InsereContratoBD(string xmlString)
        {
            try
            {
                var _xmlDoc = new XmlDocument();

                _xmlDoc.LoadXml (xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                var _empresaContrato = new ClasseEmpresasContratos.EmpresaContrato();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                var i = 0;
                _empresaContrato.EmpresaContratoID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("EmpresaContratoID")[i].InnerText);
                _empresaContrato.EmpresaID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("EmpresaID")[i].InnerText);

                _empresaContrato.NumeroContrato = _xmlDoc.GetElementsByTagName ("NumeroContrato")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("NumeroContrato")[i].InnerText;
                _empresaContrato.Descricao = _xmlDoc.GetElementsByTagName ("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("Descricao")[i].InnerText;
                _empresaContrato.Emissao = _xmlDoc.GetElementsByTagName ("Emissao")[i].InnerText == "" ? null : (DateTime?) Convert.ToDateTime (_xmlDoc.GetElementsByTagName ("Emissao")[i].InnerText);
                _empresaContrato.Validade = _xmlDoc.GetElementsByTagName ("Validade")[i].InnerText == "" ? null : (DateTime?) Convert.ToDateTime (_xmlDoc.GetElementsByTagName ("Validade")[i].InnerText);
                _empresaContrato.Terceirizada = _xmlDoc.GetElementsByTagName ("Terceirizada")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("Terceirizada")[i].InnerText;
                _empresaContrato.Contratante = _xmlDoc.GetElementsByTagName ("Contratante")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("Contratante")[i].InnerText;
                _empresaContrato.IsencaoCobranca = _xmlDoc.GetElementsByTagName ("IsencaoCobranca")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("IsencaoCobranca")[i].InnerText;

                //'obs'
                _empresaContrato.TipoCobrancaID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("TipoCobrancaID")[i].InnerText); // == null ? "" : _xmlDoc.GetElementsByTagName("TipoCobrancaID")[i].InnerText;
                _empresaContrato.CobrancaEmpresaID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("CobrancaEmpresaID")[i].InnerText); // == null ? "" : _xmlDoc.GetElementsByTagName("CobrancaEmpresaID")[i].InnerText;

                _empresaContrato.CEP = _xmlDoc.GetElementsByTagName ("CEP")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("CEP")[i].InnerText;
                _empresaContrato.Endereco = _xmlDoc.GetElementsByTagName ("Endereco")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("Endereco")[i].InnerText;
                _empresaContrato.Complemento = _xmlDoc.GetElementsByTagName ("Complemento")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("Complemento")[i].InnerText;
                _empresaContrato.Numero = _xmlDoc.GetElementsByTagName ("Numero")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("Numero")[i].InnerText;
                _empresaContrato.Bairro = _xmlDoc.GetElementsByTagName ("Bairro")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("Bairro")[i].InnerText;

                _empresaContrato.MunicipioID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("MunicipioID")[i].InnerText);
                _empresaContrato.EstadoID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("EstadoID")[i].InnerText);

                _empresaContrato.NomeResp = _xmlDoc.GetElementsByTagName ("NomeResp")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("NomeResp")[i].InnerText;
                _empresaContrato.TelefoneResp = _xmlDoc.GetElementsByTagName ("TelefoneResp")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("TelefoneResp")[i].InnerText;
                _empresaContrato.CelularResp = _xmlDoc.GetElementsByTagName ("CelularResp")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("CelularResp")[i].InnerText;

                _empresaContrato.EmailResp = _xmlDoc.GetElementsByTagName ("EmailResp")[i] == null ? "" : _xmlDoc.GetElementsByTagName ("EmailResp")[i].InnerText;
                _empresaContrato.StatusID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("StatusID")[i].InnerText); //null ? "" : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
                //_empresaContrato.NomeArquivo = _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText;
                //_empresaContrato.Arquivo = _xmlDoc.GetElementsByTagName("Arquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Arquivo")[i].InnerText;
                _empresaContrato.NomeArquivo = _contratoTemp.NomeArquivo == null ? "" : _contratoTemp.NomeArquivo;
                _empresaContrato.Arquivo = _contratoTemp.Arquivo == null ? "" : _contratoTemp.Arquivo;

                _empresaContrato.TipoAcessoID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("TipoAcessoID")[i].InnerText);

                //_Con.Close();
                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();

                SqlCommand _sqlCmd;
                if (_empresaContrato.EmpresaContratoID != 0)
                {
                    _sqlCmd = new SqlCommand ("Update EmpresasContratos Set" +
                                              " EmpresaID=@v1" +
                                              ",NumeroContrato=@v2" +
                                              ",Descricao=@v3" +
                                              ",Emissao=@v4" +
                                              ",Validade=@v5" +
                                              ",Terceirizada=@v6" +
                                              ",Contratante=@v7" +
                                              ",IsencaoCobranca=@v8" +
                                              ",TipoCobrancaID =@v9" +
                                              ",CobrancaEmpresaID=@v10" +
                                              ",CEP=@v11" +
                                              ",Endereco=@v21" +
                                              ",Complemento=@v13" +
                                              ",Numero=@v14" +
                                              ",Bairro=@v15" +
                                              ",MunicipioID=@v16" +
                                              ",EstadoID=@v17" +
                                              ",NomeResp=@v18" +
                                              ",TelefoneResp=@v19" +
                                              ",CelularResp=@v20" +
                                              ",EmailResp=@v21" +
                                              ",StatusID=@v22" +
                                              ",NomeArquivo=@v23" +
                                              ",Arquivo=@v24" +
                                              ",TipoAcessoID=@v25" +
                                              " Where EmpresaContratoID = @v0", _Con);

                    _sqlCmd.Parameters.Add ("@V0", SqlDbType.Int).Value = _empresaContrato.EmpresaContratoID;
                    _sqlCmd.Parameters.Add ("@V1", SqlDbType.Int).Value = _empresaContrato.EmpresaID;
                    _sqlCmd.Parameters.Add ("@V2", SqlDbType.VarChar).Value = _empresaContrato.NumeroContrato;
                    _sqlCmd.Parameters.Add ("@V3", SqlDbType.VarChar).Value = _empresaContrato.Descricao;

                    if (_empresaContrato.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add ("@V4", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add ("@V4", SqlDbType.DateTime).Value = _empresaContrato.Emissao;
                    }

                    if (_empresaContrato.Validade == null)
                    {
                        _sqlCmd.Parameters.Add ("@V5", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add ("@V5", SqlDbType.DateTime).Value = _empresaContrato.Validade;
                    }
                    _sqlCmd.Parameters.Add ("@V6", SqlDbType.VarChar).Value = _empresaContrato.Terceirizada;
                    _sqlCmd.Parameters.Add ("@V7", SqlDbType.VarChar).Value = _empresaContrato.Contratante;
                    _sqlCmd.Parameters.Add ("@V8", SqlDbType.VarChar).Value = _empresaContrato.IsencaoCobranca;
                    _sqlCmd.Parameters.Add ("@V9", SqlDbType.Int).Value = _empresaContrato.TipoCobrancaID;
                    _sqlCmd.Parameters.Add ("@V10", SqlDbType.Int).Value = _empresaContrato.CobrancaEmpresaID;
                    _sqlCmd.Parameters.Add ("@V11", SqlDbType.VarChar).Value = _empresaContrato.CEP;
                    _sqlCmd.Parameters.Add ("@V12", SqlDbType.VarChar).Value = _empresaContrato.Endereco;
                    _sqlCmd.Parameters.Add ("@V13", SqlDbType.VarChar).Value = _empresaContrato.Complemento;
                    _sqlCmd.Parameters.Add ("@V14", SqlDbType.VarChar).Value = _empresaContrato.Numero;
                    _sqlCmd.Parameters.Add ("@V15", SqlDbType.VarChar).Value = _empresaContrato.Bairro;
                    _sqlCmd.Parameters.Add ("@V16", SqlDbType.VarChar).Value = _empresaContrato.MunicipioID;
                    _sqlCmd.Parameters.Add ("@V17", SqlDbType.VarChar).Value = _empresaContrato.EstadoID;
                    _sqlCmd.Parameters.Add ("@V18", SqlDbType.VarChar).Value = _empresaContrato.NomeResp;
                    _sqlCmd.Parameters.Add ("@V19", SqlDbType.VarChar).Value = _empresaContrato.TelefoneResp.RetirarCaracteresEspeciais();
                    _sqlCmd.Parameters.Add ("@V20", SqlDbType.VarChar).Value = _empresaContrato.CelularResp.RetirarCaracteresEspeciais();
                    _sqlCmd.Parameters.Add ("@V21", SqlDbType.VarChar).Value = _empresaContrato.EmailResp;
                    _sqlCmd.Parameters.Add ("@V22", SqlDbType.Int).Value = _empresaContrato.StatusID;
                    _sqlCmd.Parameters.Add ("@V23", SqlDbType.VarChar).Value = _empresaContrato.NomeArquivo;
                    _sqlCmd.Parameters.Add ("@V24", SqlDbType.VarChar).Value = _empresaContrato.Arquivo;
                    _sqlCmd.Parameters.Add ("@V25", SqlDbType.Int).Value = _empresaContrato.TipoAcessoID;
                }
                else
                {
                    _sqlCmd = new SqlCommand ("Insert into EmpresasContratos(EmpresaID,NumeroContrato,Descricao,Emissao,Validade,Terceirizada,Contratante,IsencaoCobranca" +
                                              ",TipoCobrancaID,CobrancaEmpresaID,CEP,Endereco,Complemento,Numero,Bairro" +
                                              ",MunicipioID,EstadoID,NomeResp,TelefoneResp,CelularResp,EmailResp,StatusID,NomeArquivo,Arquivo,TipoAcessoID) " +
                                              "values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25)", _Con);

                    _sqlCmd.Parameters.Add ("@V1", SqlDbType.Int).Value = _empresaContrato.EmpresaID;
                    _sqlCmd.Parameters.Add ("@V2", SqlDbType.VarChar).Value = _empresaContrato.NumeroContrato;
                    _sqlCmd.Parameters.Add ("@V3", SqlDbType.VarChar).Value = _empresaContrato.Descricao;

                    if (_empresaContrato.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add ("@V4", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add ("@V4", SqlDbType.DateTime).Value = _empresaContrato.Emissao;
                    }

                    if (_empresaContrato.Validade == null)
                    {
                        _sqlCmd.Parameters.Add ("@V5", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add ("@V5", SqlDbType.DateTime).Value = _empresaContrato.Validade;
                    }
                    _sqlCmd.Parameters.Add ("@V6", SqlDbType.VarChar).Value = _empresaContrato.Terceirizada;
                    _sqlCmd.Parameters.Add ("@V7", SqlDbType.VarChar).Value = _empresaContrato.Contratante;
                    _sqlCmd.Parameters.Add ("@V8", SqlDbType.VarChar).Value = _empresaContrato.IsencaoCobranca;
                    _sqlCmd.Parameters.Add ("@V9", SqlDbType.Int).Value = _empresaContrato.TipoCobrancaID;
                    _sqlCmd.Parameters.Add ("@V10", SqlDbType.Int).Value = _empresaContrato.CobrancaEmpresaID;
                    _sqlCmd.Parameters.Add ("@V11", SqlDbType.VarChar).Value = _empresaContrato.CEP;
                    _sqlCmd.Parameters.Add ("@V12", SqlDbType.VarChar).Value = _empresaContrato.Endereco;
                    _sqlCmd.Parameters.Add ("@V13", SqlDbType.VarChar).Value = _empresaContrato.Complemento;
                    _sqlCmd.Parameters.Add ("@V14", SqlDbType.VarChar).Value = _empresaContrato.Numero;
                    _sqlCmd.Parameters.Add ("@V15", SqlDbType.VarChar).Value = _empresaContrato.Bairro;
                    _sqlCmd.Parameters.Add ("@V16", SqlDbType.VarChar).Value = _empresaContrato.MunicipioID;
                    _sqlCmd.Parameters.Add ("@V17", SqlDbType.VarChar).Value = _empresaContrato.EstadoID;
                    _sqlCmd.Parameters.Add ("@V18", SqlDbType.VarChar).Value = _empresaContrato.NomeResp;
                    _sqlCmd.Parameters.Add ("@V19", SqlDbType.VarChar).Value = _empresaContrato.TelefoneResp.RetirarCaracteresEspeciais();
                    _sqlCmd.Parameters.Add ("@V20", SqlDbType.VarChar).Value = _empresaContrato.CelularResp.RetirarCaracteresEspeciais();
                    _sqlCmd.Parameters.Add ("@V21", SqlDbType.VarChar).Value = _empresaContrato.EmailResp;
                    _sqlCmd.Parameters.Add ("@V22", SqlDbType.Int).Value = _empresaContrato.StatusID;
                    _sqlCmd.Parameters.Add ("@V23", SqlDbType.VarChar).Value = _empresaContrato.NomeArquivo;
                    _sqlCmd.Parameters.Add ("@V24", SqlDbType.VarChar).Value = _empresaContrato.Arquivo;
                    _sqlCmd.Parameters.Add ("@V25", SqlDbType.Int).Value = _empresaContrato.TipoAcessoID;
                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void InsereEmpresaBD ex: " + ex);
            }
        }

        //private void InsereContratoBD(string xmlString)
        //{
        //    try
        //    {

        //        System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

        //        _xmlDoc.LoadXml(xmlString);
        //        // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
        //        ClasseEmpresasContratos.EmpresaContrato _empresaContrato = new ClasseEmpresasContratos.EmpresaContrato();
        //        //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
        //        //{
        //        int i = 0;
        //        _empresaContrato.EmpresaContratoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaContratoID")[i].InnerText);
        //        _empresaContrato.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);

        //        _empresaContrato.NumeroContrato = _xmlDoc.GetElementsByTagName("NumeroContrato")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("NumeroContrato")[i].InnerText);
        //        _empresaContrato.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
        //        _empresaContrato.Emissao = _xmlDoc.GetElementsByTagName("Emissao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText;
        //        _empresaContrato.Validade = _xmlDoc.GetElementsByTagName("Validade")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Validade")[i].InnerText;
        //        _empresaContrato.Terceirizada = _xmlDoc.GetElementsByTagName("Terceirizada")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Terceirizada")[i].InnerText;
        //        _empresaContrato.Contratante = _xmlDoc.GetElementsByTagName("Contratante")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Contratante")[i].InnerText;
        //        _empresaContrato.IsencaoCobranca = _xmlDoc.GetElementsByTagName("IsencaoCobranca")[i] == null ? "" : _xmlDoc.GetElementsByTagName("IsencaoCobranca")[i].InnerText;

        //        //'obs'
        //        _empresaContrato.TipoCobrancaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoCobrancaID")[i].InnerText);   // == null ? "" : _xmlDoc.GetElementsByTagName("TipoCobrancaID")[i].InnerText;
        //        _empresaContrato.CobrancaEmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("CobrancaEmpresaID")[i].InnerText); // == null ? "" : _xmlDoc.GetElementsByTagName("CobrancaEmpresaID")[i].InnerText;

        //        _empresaContrato.CEP = _xmlDoc.GetElementsByTagName("CEP")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CEP")[i].InnerText;
        //        _empresaContrato.Endereco = _xmlDoc.GetElementsByTagName("Endereco")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Endereco")[i].InnerText;
        //        _empresaContrato.Complemento = _xmlDoc.GetElementsByTagName("Complemento")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Complemento")[i].InnerText;
        //        _empresaContrato.Numero = _xmlDoc.GetElementsByTagName("Numero")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Numero")[i].InnerText;
        //        _empresaContrato.Bairro = _xmlDoc.GetElementsByTagName("Bairro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Bairro")[i].InnerText;

        //        _empresaContrato.MunicipioID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);
        //        _empresaContrato.EstadoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);

        //        _empresaContrato.NomeResp = _xmlDoc.GetElementsByTagName("NomeResp")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeResp")[i].InnerText;
        //        _empresaContrato.TelefoneResp = _xmlDoc.GetElementsByTagName("TelefoneResp")[i] == null ? "" : _xmlDoc.GetElementsByTagName("TelefoneResp")[i].InnerText;
        //        _empresaContrato.CelularResp = _xmlDoc.GetElementsByTagName("CelularResp")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CelularResp")[i].InnerText;

        //        _empresaContrato.EmailResp = _xmlDoc.GetElementsByTagName("EmailResp")[i] == null ? "" : _xmlDoc.GetElementsByTagName("EmailResp")[i].InnerText;
        //        _empresaContrato.StatusID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);   //null ? "" : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
        //                                                                                                              //_empresaContrato.NomeArquivo = _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText;
        //                                                                                                              //_empresaContrato.Arquivo = _xmlDoc.GetElementsByTagName("Arquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Arquivo")[i].InnerText;
        //        _empresaContrato.NomeArquivo = _contratoTemp.NomeArquivo == null ? "" : _contratoTemp.NomeArquivo;
        //        _empresaContrato.Arquivo = _contratoTemp.Arquivo == null ? "" : _contratoTemp.Arquivo;

        //        _empresaContrato.TipoAcessoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoAcessoID")[i].InnerText);

        //        //_Con.Close();
        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

        //        SqlCommand _sqlCmd;
        //        if (_empresaContrato.EmpresaContratoID != 0)
        //        {
        //            _sqlCmd = new SqlCommand("Update EmpresasContratos Set" +
        //                " EmpresaID= " + _empresaContrato.EmpresaID + "" +
        //                ",NumeroContrato= '" + _empresaContrato.NumeroContrato + "'" +
        //                ",Descricao= '" + _empresaContrato.Descricao + "'" +
        //                ",Emissao= '" + _empresaContrato.Emissao + "'" +
        //                ",Validade= '" + _empresaContrato.Validade + "'" +
        //                ",Terceirizada= '" + _empresaContrato.Terceirizada + "'" +
        //                ",Contratante= '" + _empresaContrato.Contratante + "'" +
        //                ",IsencaoCobranca='" + _empresaContrato.IsencaoCobranca + "'" +
        //                ",TipoCobrancaID =" + _empresaContrato.TipoCobrancaID + "" +
        //                ",CobrancaEmpresaID= " + _empresaContrato.CobrancaEmpresaID + "" +
        //                ",CEP= '" + _empresaContrato.CEP + "'" +
        //                ",Endereco= '" + _empresaContrato.Endereco + "'" +
        //                ",Complemento= '" + _empresaContrato.Complemento + "'" +
        //                ",Numero= '" + _empresaContrato.Numero + "'" +
        //                ",Bairro= '" + _empresaContrato.Bairro + "'" +
        //                ",MunicipioID= " + _empresaContrato.MunicipioID + "" +
        //                ",EstadoID= " + _empresaContrato.EstadoID + "" +
        //                ",NomeResp= '" + _empresaContrato.NomeResp + "'" +
        //                ",TelefoneResp='" + _empresaContrato.TelefoneResp + "'" +
        //                ",CelularResp='" + _empresaContrato.CelularResp + "'" +
        //                ",EmailResp='" + _empresaContrato.EmailResp + "'" +
        //                ",StatusID=" + _empresaContrato.StatusID + "" +
        //                ",NomeArquivo='" + _empresaContrato.NomeArquivo + "'" +
        //                ",Arquivo='" + _empresaContrato.Arquivo + "'" +
        //                ",TipoAcessoID=" + _empresaContrato.TipoAcessoID + "" +
        //                " Where EmpresaContratoID = " + _empresaContrato.EmpresaContratoID + "", _Con);
        //        }
        //        else
        //        {
        //            _sqlCmd = new SqlCommand("Insert into EmpresasContratos(EmpresaID,NumeroContrato,Descricao,Emissao,Validade,Terceirizada,Contratante,IsencaoCobranca" +
        //                                     ",TipoCobrancaID,CobrancaEmpresaID,CEP,Endereco,Complemento,Numero,Bairro" +
        //                                     ",MunicipioID,EstadoID,NomeResp,TelefoneResp,CelularResp,EmailResp,StatusID,NomeArquivo,Arquivo,TipoAcessoID) values (" +
        //                                     _empresaContrato.EmpresaID + ",'" + _empresaContrato.NumeroContrato + "','" + _empresaContrato.Descricao + "','" + _empresaContrato.Emissao + "','" + _empresaContrato.Validade + "','" + _empresaContrato.Terceirizada + "','" + _empresaContrato.Contratante + "','" + _empresaContrato.IsencaoCobranca +
        //                                     "'," + _empresaContrato.TipoCobrancaID + "," + _empresaContrato.CobrancaEmpresaID + ",'" + _empresaContrato.CEP + "','" + _empresaContrato.Endereco + "','" + _empresaContrato.Complemento + "','" + _empresaContrato.Numero + "','" + _empresaContrato.Bairro + "'," +
        //                                     _empresaContrato.MunicipioID + "," + _empresaContrato.EstadoID + ",'" + _empresaContrato.NomeResp + "','" + _empresaContrato.TelefoneResp + "'" +
        //                                     ",'" + _empresaContrato.CelularResp + "','" + _empresaContrato.EmailResp + "'," + _empresaContrato.StatusID + ",'" + _empresaContrato.NomeArquivo + "','" + _empresaContrato.Arquivo +
        //                                     "'," + _empresaContrato.TipoAcessoID + ")", _Con);

        //        }

        //        _sqlCmd.ExecuteNonQuery();
        //        _Con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void InsereEmpresaBD ex: " + ex);

        //    }
        //}

        private void ExcluiContratoBD(int _EmpresaContratoID) // alterar para xml
        {
            try
            {
                //_Con.Close();
                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand ("Delete from EmpresasContratos where EmpresaContratoID=" + _EmpresaContratoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void ExcluiContratoBD ex: " + ex);
            }
        }

        #endregion
    }
}