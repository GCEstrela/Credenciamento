using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Xml;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasAnexosViewModel : ViewModelBase
    {

        #region Inicializacao

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo> _Anexos;

        private ClasseEmpresasAnexos.EmpresaAnexo _AnexoSelecionado;

        private ClasseEmpresasAnexos.EmpresaAnexo _anexoTemp = new ClasseEmpresasAnexos.EmpresaAnexo();

        private List<ClasseEmpresasAnexos.EmpresaAnexo> _AnexosTemp = new List<ClasseEmpresasAnexos.EmpresaAnexo>();

        PopupPesquisaEmpresasAnexos popupPesquisaEmpresasAnexos;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        private readonly IEmpresaAnexoService _service = new EmpresaAnexoService();

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo> Anexos
        {
            get
            {
                return _Anexos;
            }

            set
            {
                if (_Anexos != value)
                {
                    _Anexos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseEmpresasAnexos.EmpresaAnexo AnexoSelecionado
        {
            get
            {
                return _AnexoSelecionado;
            }
            set
            {
                _AnexoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (AnexoSelecionado != null)
                {
                    //OnEmpresaSelecionada();
                }

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
                return _HabilitaEdicao;
            }
            set
            {
                _HabilitaEdicao = value;
                base.OnPropertyChanged();
            }
        }

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
        #endregion

        #region Comandos dos Botoes
        public void OnAtualizaCommand(object empresaID)
        {
            EmpresaSelecionadaID = Convert.ToInt32(empresaID);
            Thread CarregaColecaoAnexos_thr = new Thread(() => CarregaColecaoAnexos(Convert.ToInt32(empresaID)));
            CarregaColecaoAnexos_thr.Start();
            //CarregaColecaoAnexos(Convert.ToInt32(empresaID));
        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _anexoTemp.NomeAnexo = arq.Nome;
                _anexoTemp.Anexo = arq.FormatoBase64;
                if (Anexos != null)
                    Anexos[0].NomeAnexo = arq.Nome;

            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }

        }

        public void OnAbrirArquivoCommand()
        {
            try
            {
                try
                {
                    string _ArquivoPDF = null;
                    if (_anexoTemp != null)
                    {
                        if (_anexoTemp.Anexo != null && _anexoTemp.EmpresaAnexoID == AnexoSelecionado.EmpresaAnexoID)
                        {
                            _ArquivoPDF = _anexoTemp.Anexo;

                        }
                    }
                    if (_ArquivoPDF == null)
                    {
                        string _xmlstring = CriaXmlImagem(AnexoSelecionado.EmpresaAnexoID);

                        XmlDocument xmldocument = new XmlDocument();
                        xmldocument.LoadXml(_xmlstring);
                        XmlNode node = xmldocument.DocumentElement;
                        XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                        _ArquivoPDF = arquivoNode.FirstChild.Value;
                    }
                    Global.PopupPDF(_ArquivoPDF);
                }
                catch (Exception ex)
                {


                }
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);
            }
        }

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                _anexoTemp = AnexoSelecionado.CriaCopia(AnexoSelecionado);
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
                Anexos[_selectedIndexTemp] = _anexoTemp;
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

                var entity = AnexoSelecionado;
                var entityConv = Mapper.Map<EmpresaAnexo>(entity);

                _service.Alterar(entityConv);

                Thread CarregaColecaoAnexosSignatarios_thr = new Thread(() => CarregaColecaoAnexos(AnexoSelecionado.EmpresaID));
                CarregaColecaoAnexosSignatarios_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnSalvarEdicaoCommand ex: " + ex.Message);
                Utils.TraceException(ex);
                throw;
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Anexos)
                {
                    _AnexosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Anexos.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _anexoTemp = new ClasseEmpresasAnexos.EmpresaAnexo();
                _anexoTemp.EmpresaID = EmpresaSelecionadaID;
                Anexos.Add(_anexoTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnSalvarEdicaoCommand ex: " + ex.Message);
                Utils.TraceException(ex);
                throw;
            }

        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;

                ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo> _EmpresasAnexosPro = new ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo>();
                ClasseEmpresasAnexos _ClasseEmpresasAnexosTemp = new ClasseEmpresasAnexos();

                _EmpresasAnexosPro.Add(AnexoSelecionado);
                _ClasseEmpresasAnexosTemp.EmpresasAnexos = _EmpresasAnexosPro;

                var entity = AnexoSelecionado;
                var entityConv = Mapper.Map<EmpresaAnexo>(entity);

                _service.Criar(entityConv);

                Thread CarregaColecaoAnexosSignatarios_thr = new Thread(() => CarregaColecaoAnexos(AnexoSelecionado.EmpresaID));
                CarregaColecaoAnexosSignatarios_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnSalvarAdicaoCommand ex: " + ex.Message);
                Utils.TraceException(ex);
                throw;
            }
        }
        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Anexos = null;
                Anexos = new ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo>(_AnexosTemp);
                SelectedIndex = _selectedIndexTemp;
                _AnexosTemp.Clear();
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
                        var emp = _service.BuscarPelaChave(AnexoSelecionado.EmpresaAnexoID);
                        _service.Remover(emp);
                        Anexos.Remove(AnexoSelecionado);
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
                popupPesquisaEmpresasAnexos = new PopupPesquisaEmpresasAnexos();
                popupPesquisaEmpresasAnexos.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaEmpresasAnexos.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaEmpresasAnexos.Criterio.Split((char)(20));
            int _empresaID = EmpresaSelecionadaID;
            string _descricao = ((string[])vetor)[0];
            CarregaColecaoAnexos(_empresaID, _descricao);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoAnexos(int? empresaID = null, string _descricao = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_descricao)) _descricao = $"%{_descricao}%";

                var list1 = _service.Listar(empresaID, _descricao, null, null, null, null);
                var list2 = Mapper.Map<List<ClasseEmpresasAnexos.EmpresaAnexo>>(list1);

                var observer = new ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Anexos = observer;

            }
            catch (Exception ex)
            {
                Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access
        private string RequisitaAnexos(int _empresaID, string _DescricaoAnexo = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasAnexos = _xmlDocument.CreateElement("ClasseEmpresasAnexos");
                _xmlDocument.AppendChild(_ClasseEmpresasAnexos);

                XmlNode _EmpresasAnexos = _xmlDocument.CreateElement("EmpresasAnexos");
                _ClasseEmpresasAnexos.AppendChild(_EmpresasAnexos);

                string _strSql;

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _DescricaoAnexo = "%" + _DescricaoAnexo + "%";

                _strSql = "select [EmpresaAnexoID],[EmpresaID],[Descricao],[NomeAnexo] " +
                    "from EmpresasAnexos where EmpresaID = " + _empresaID + " and Descricao Like '" +
                    _DescricaoAnexo + "' order by EmpresaAnexoID desc";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _EmpresaAnexo = _xmlDocument.CreateElement("EmpresaAnexo");
                    _EmpresasAnexos.AppendChild(_EmpresaAnexo);

                    XmlNode _EmpresaAnexoID = _xmlDocument.CreateElement("EmpresaAnexoID");
                    _EmpresaAnexoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaAnexoID"].ToString())));
                    _EmpresaAnexo.AppendChild(_EmpresaAnexoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _EmpresaAnexo.AppendChild(_EmpresaID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _EmpresaAnexo.AppendChild(_Descricao);

                    XmlNode _NomeAnexo = _xmlDocument.CreateElement("NomeAnexo");
                    _NomeAnexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeAnexo"].ToString())));
                    _EmpresaAnexo.AppendChild(_NomeAnexo);

                    XmlNode _Anexo = _xmlDocument.CreateElement("Anexo");
                    //_Anexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Anexo"].ToString())));
                    _EmpresaAnexo.AppendChild(_Anexo);

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

        private void InsereAnexoBD(string xmlString)
        {
            try
            {


                XmlDocument _xmlDoc = new XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                //
                ClasseEmpresasAnexos.EmpresaAnexo _empresaAnexo = new ClasseEmpresasAnexos.EmpresaAnexo();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _empresaAnexo.EmpresaAnexoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaAnexoID")[i].InnerText);
                _empresaAnexo.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
                _empresaAnexo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText.Trim();
                //_empresaAnexo.NomeAnexo = _xmlDoc.GetElementsByTagName("NomeSeguradora")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeSeguradora")[i].InnerText;
                //_empresaAnexo.NumeroApolice = _xmlDoc.GetElementsByTagName("NumeroApolice")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroApolice")[i].InnerText;
                //_empresaSeguro.ValorCobertura = _xmlDoc.GetElementsByTagName("ValorCobertura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ValorCobertura")[i].InnerText;
                //_empresaSeguro.Arquivo = _xmlDoc.GetElementsByTagName("Arquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Arquivo")[i].InnerText;

                _empresaAnexo.NomeAnexo = _anexoTemp.NomeAnexo == null ? "" : _anexoTemp.NomeAnexo.Trim();
                _empresaAnexo.Anexo = _anexoTemp.Anexo == null ? "" : _anexoTemp.Anexo.Trim();


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                //_Con.Close();


                SqlCommand _sqlCmd;
                if (_empresaAnexo.EmpresaAnexoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update EmpresasAnexos Set" +
                        " EmpresaID= " + _empresaAnexo.EmpresaID + "" +
                        ",Descricao= '" + _empresaAnexo.Descricao + "'" +
                        ",NomeAnexo= '" + _empresaAnexo.NomeAnexo + "'" +
                        ",Anexo= '" + _empresaAnexo.Anexo + "'" +
                        " Where EmpresaAnexoID = " + _empresaAnexo.EmpresaAnexoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into EmpresasAnexos(EmpresaID,Descricao,NomeAnexo,Anexo) values (" +
                                                          _empresaAnexo.EmpresaID + ",'" + _empresaAnexo.Descricao + "','" +
                                                          _empresaAnexo.NomeAnexo + "','" + _empresaAnexo.Anexo + "')", _Con);


                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresaBD ex: " + ex);


            }
        }

        private void ExcluiSeguroBD(int _EmpresaAnexoID) // alterar para xml
        {
            try
            {

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                //_Con.Close();


                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from EmpresasAnexos where EmpresaAnexoID=" + _EmpresaAnexoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void EmpresasAnexos ex: " + ex);

            }
        }
        #endregion

        #region Métodos Públicos

        Global g = new Global();

        #endregion

        #region Metodos privados
        private string CriaXmlImagem(int empresaAnexoID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From EmpresasAnexos Where EmpresaAnexoID = " + empresaAnexoID + "", _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    //XmlNode _ArquivoImagemID = _xmlDocument.CreateElement("ArquivoImagemID");
                    //_ArquivoImagemID.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["EmpresaSeguroID"].ToString())));
                    //_ArquivoImagem.AppendChild(_ArquivoImagemID);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Anexo"].ToString())));
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
        #endregion
    }
}
