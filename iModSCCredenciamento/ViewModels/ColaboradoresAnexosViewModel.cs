using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using AutoMapper;

namespace iModSCCredenciamento.ViewModels
{
   public class ColaboradoresAnexosViewModel : ViewModelBase
   {
        Global g = new Global();
        private IColaboradorAnexoService _colaboradorAnexoService;
       
        #region Inicializacao
        public ColaboradoresAnexosViewModel()
        {
            _colaboradorAnexoService=new ColaboradorAnexoService();
            CarregaUI();
        }
        private void CarregaUI()
        {
            //CarregaColecaoColaboradorerAnexos();
            //CarregaColecaoAnexos();
        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo> _ColaboradoresAnexos;

        private ClasseColaboradoresAnexos.ColaboradorAnexo _ColaboradorAnexoSelecionado;

        private ClasseColaboradoresAnexos.ColaboradorAnexo _ColaboradorAnexoTemp = new ClasseColaboradoresAnexos.ColaboradorAnexo();

        private List<ClasseColaboradoresAnexos.ColaboradorAnexo> _ColaboradoresAnexosTemp = new List<ClasseColaboradoresAnexos.ColaboradorAnexo>();

        PopupPesquisaColaboradorAnexo popupPesquisaColaboradorAnexo;

        private int _selectedIndex;

        private int _ColaboradorAnexoSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo> ColaboradoresAnexos
        {
            get
            {
                return _ColaboradoresAnexos;
            }

            set
            {
                if (_ColaboradoresAnexos != value)
                {
                    _ColaboradoresAnexos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseColaboradoresAnexos.ColaboradorAnexo ColaboradorAnexoSelecionado
        {
            get
            {
                return this._ColaboradorAnexoSelecionado;
            }
            set
            {
                this._ColaboradorAnexoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (ColaboradorAnexoSelecionado != null)
                {
                    //OnEmpresaSelecionada();
                }

            }
        }

        public int ColaboradorAnexoSelecionadaID
        {
            get
            {
                return this._ColaboradorAnexoSelecionadaID;
            }
            set
            {
                this._ColaboradorAnexoSelecionadaID = value;
                base.OnPropertyChanged();
                if (ColaboradorAnexoSelecionadaID != null)
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
        #endregion

        #region Comandos dos Botoes
        public void OnAtualizaCommand(object _colaboradorAnexoID)
        {

            ColaboradorAnexoSelecionadaID = Convert.ToInt32(_colaboradorAnexoID);
            Thread CarregaColecaoColaboradoresAnexos_thr = new Thread(() => CarregaColecaoColaboradoresAnexos(Convert.ToInt32(_colaboradorAnexoID)));
            CarregaColecaoColaboradoresAnexos_thr.Start();
            //CarregaColecaoColaboradorerAnexos(Convert.ToInt32(_colaboradorAnexoID));

        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog _arquivoPDF = new System.Windows.Forms.OpenFileDialog();

                string _nomecompletodoarquivo;
                string _arquivoSTR;
                _arquivoPDF.InitialDirectory = "c:\\\\";
                _arquivoPDF.Filter = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                _arquivoPDF.RestoreDirectory = true;
                _arquivoPDF.ShowDialog();

                _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);

                _ColaboradorAnexoTemp.NomeArquivo = _nomecompletodoarquivo;
                _ColaboradorAnexoTemp.Arquivo = _arquivoSTR;

                if (ColaboradoresAnexos != null)
                {
                    ColaboradoresAnexos[0].NomeArquivo = _nomecompletodoarquivo;
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
                    if (_ColaboradorAnexoTemp != null)
                    {
                        if (_ColaboradorAnexoTemp.Arquivo != null && _ColaboradorAnexoTemp.ColaboradorAnexoID == ColaboradorAnexoSelecionado.ColaboradorAnexoID)
                        {
                            _ArquivoPDF = _ColaboradorAnexoTemp.Arquivo;

                        }
                    }
                    if (_ArquivoPDF == null)
                    {
                        string _xmlstring = CriaXmlImagem( ColaboradorAnexoSelecionado.ColaboradorAnexoID);

                        XmlDocument xmldocument = new XmlDocument();
                        xmldocument.LoadXml(_xmlstring);
                        XmlNode node = (XmlNode)xmldocument.DocumentElement;
                        XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                        _ArquivoPDF = arquivoNode.FirstChild.Value;
                    }
                    Global.PopupPDF(_ArquivoPDF);
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
                    Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);

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
                _ColaboradorAnexoTemp = ColaboradorAnexoSelecionado.CriaCopia(ColaboradorAnexoSelecionado);
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
                ColaboradoresAnexos[_selectedIndexTemp] = _ColaboradorAnexoTemp;
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresAnexos));

                ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo> _ColaboradoresAnexosTemp = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>();
                ClasseColaboradoresAnexos _ClasseColaboradoresAnexosTemp = new ClasseColaboradoresAnexos();
                _ColaboradoresAnexosTemp.Add(ColaboradorAnexoSelecionado);
                _ClasseColaboradoresAnexosTemp.ColaboradoresAnexos = _ColaboradoresAnexosTemp;


                IMOD.Domain.Entities.ColaboradorAnexo ColaboradorAnexoEntity = new IMOD.Domain.Entities.ColaboradorAnexo();
                g.TranportarDados(ColaboradorAnexoSelecionado, 1, ColaboradorAnexoEntity);

                var repositorio = new IMOD.Infra.Repositorios.ColaboradorAnexoRepositorio();
                repositorio.Alterar(ColaboradorAnexoEntity);
                var id = ColaboradorAnexoEntity.ColaboradorAnexoId;

                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseColaboradoresAnexosTemp);
                //        xmlString = sw.ToString();
                //    }

                //}

                //InsereColaboradorAnexoBD(xmlString);

                _ColaboradoresAnexosTemp = null;

                _ColaboradoresAnexosTemp.Clear();
                _ColaboradorAnexoTemp = null;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

       public void OnSalvarAdicaoCommand2()
       {
            //_colaboradorService.Criar(Anexo);
       }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresAnexos));

                ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo> _ColaboradoresAnexosPro = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>();
                ClasseColaboradoresAnexos _ClasseColaboradoresAnexosPro = new ClasseColaboradoresAnexos();
                _ColaboradoresAnexosPro.Add(ColaboradorAnexoSelecionado);
                _ClasseColaboradoresAnexosPro.ColaboradoresAnexos = _ColaboradoresAnexosPro;

                IMOD.Domain.Entities.ColaboradorAnexo ColaboradorAnexoEntity = new IMOD.Domain.Entities.ColaboradorAnexo();
                g.TranportarDados(ColaboradorAnexoSelecionado, 1, ColaboradorAnexoEntity);

                var repositorio = new IMOD.Infra.Repositorios.ColaboradorAnexoRepositorio();
                repositorio.Criar(ColaboradorAnexoEntity);
                var id = ColaboradorAnexoEntity.ColaboradorAnexoId;

                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseColaboradoresAnexosPro);
                //        xmlString = sw.ToString();
                //    }

                //}

                //InsereColaboradorAnexoBD(xmlString);



                Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoColaboradoresAnexos(id));
                CarregaColecaoSeguros_thr.Start();
                _ColaboradoresAnexosTemp.Add(ColaboradorAnexoSelecionado);
                ColaboradoresAnexos = null;
                ColaboradoresAnexos = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>(_ColaboradoresAnexosTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresAnexosTemp.Clear();


                _ColaboradoresAnexosPro = null;

                _ColaboradoresAnexosPro.Clear();
                _ColaboradorAnexoTemp = null;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
    
                foreach (var x in ColaboradoresAnexos)
                {
                    _ColaboradoresAnexosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                ColaboradoresAnexos.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _ColaboradorAnexoTemp = new ClasseColaboradoresAnexos.ColaboradorAnexo();
                _ColaboradorAnexoTemp.ColaboradorID = ColaboradorAnexoSelecionadaID;
                ColaboradoresAnexos.Add(_ColaboradorAnexoTemp);
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
                ColaboradoresAnexos = null;
                ColaboradoresAnexos = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>(_ColaboradoresAnexosTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresAnexosTemp.Clear();
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
                //if (MessageBox.Show("Tem certeza que deseja excluir este anexo?", "Excluir Anexo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados deste anexo, inclusive histórico. Confirma exclusão?", "Excluir Anexo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {

                //        ExcluiColaboradorAnexoBD(ColaboradorAnexoSelecionado.ColaboradorAnexoID);

                //        ColaboradoresAnexos.Remove(ColaboradorAnexoSelecionado);

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {

                        IMOD.Domain.Entities.ColaboradorAnexo ColaboradorAnexoEntity = new IMOD.Domain.Entities.ColaboradorAnexo();
                        g.TranportarDados(ColaboradorAnexoSelecionado, 1, ColaboradorAnexoEntity);

                        var repositorio = new IMOD.Infra.Repositorios.ColaboradorAnexoRepositorio();
                        repositorio.Remover(ColaboradorAnexoEntity);
                        
                        ColaboradoresAnexos.Remove(ColaboradorAnexoSelecionado);
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
                popupPesquisaColaboradorAnexo = new PopupPesquisaColaboradorAnexo();
                popupPesquisaColaboradorAnexo.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaColaboradorAnexo.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaColaboradorAnexo.Criterio.Split((char)(20));
            int _colaboradorID = ColaboradorAnexoSelecionadaID;
            string _DescricaoAnexo = ((string[])vetor)[0];
            CarregaColecaoColaboradoresAnexos(_colaboradorID, _DescricaoAnexo);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        public void CarregaColecaoColaboradoresAnexos(int _colaboradorID, string _nome = "")
        {
            try
            {
                
                var service = new IMOD.Application.Service.ColaboradorAnexoService();
                if (!string.IsNullOrWhiteSpace(_nome)) _nome = $"%{_nome}%";
                var list1 = service.Listar(_colaboradorID, _nome);
                //var list1 = service.Listar(_colaboradorID, _descricao);

                var list2 = Mapper.Map<List<ClasseColaboradoresAnexos.ColaboradorAnexo>>(list1);

                var observer = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.ColaboradoresAnexos = observer;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        #endregion

        #region Data Access
        private string RequisitaColaboradoresAnexos(string _colaboradorID, string _descricao = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseColaboradoresAnexos = _xmlDocument.CreateElement("ClasseColaboradoresAnexos");
                _xmlDocument.AppendChild(_ClasseColaboradoresAnexos);

                XmlNode _ColaboradoresAnexos = _xmlDocument.CreateElement("ColaboradoresAnexos");
                _ClasseColaboradoresAnexos.AppendChild(_ColaboradoresAnexos);

                string _strSql;

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                _descricao = "%" + _descricao + "%";
                _strSql = "select * from ColaboradoresAnexos where ColaboradorID = " + _colaboradorID + " and Descricao Like '" + _descricao + "' order by ColaboradorAnexoID desc"; // and NumeroApolice Like '" + _numeroapolice + "'
                //_strSql = "select * from ColaboradoresAnexos where ColaboradorID = " + _colaboradorID + "";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _ColaboradorAnexo = _xmlDocument.CreateElement("ColaboradorAnexo");
                    _ColaboradoresAnexos.AppendChild(_ColaboradorAnexo);

                    XmlNode _ColaboradorAnexoID = _xmlDocument.CreateElement("ColaboradorAnexoID");
                    _ColaboradorAnexoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorAnexoID"].ToString())));
                    _ColaboradorAnexo.AppendChild(_ColaboradorAnexoID);

                    XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
                    _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
                    _ColaboradorAnexo.AppendChild(_ColaboradorID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString()).Trim()));
                    _ColaboradorAnexo.AppendChild(_Descricao);

                    XmlNode _NomeArquivo = _xmlDocument.CreateElement("NomeArquivo");
                    _NomeArquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivo"].ToString()).Trim()));
                    _ColaboradorAnexo.AppendChild(_NomeArquivo);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Arquivo"].ToString())));
                    _ColaboradorAnexo.AppendChild(_Arquivo);

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

        private void InsereColaboradorAnexoBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();
                _xmlDoc.LoadXml(xmlString);

                ClasseColaboradoresAnexos.ColaboradorAnexo _ColaboradorAnexo = new ClasseColaboradoresAnexos.ColaboradorAnexo();

                int i = 0;

                _ColaboradorAnexo.ColaboradorAnexoID = _xmlDoc.GetElementsByTagName("ColaboradorAnexoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorAnexoID")[i].InnerText);
                _ColaboradorAnexo.ColaboradorID = _xmlDoc.GetElementsByTagName("ColaboradorID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorID")[i].InnerText);
                _ColaboradorAnexo.NomeArquivo = _xmlDoc.GetElementsByTagName("NomeArquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText;
                _ColaboradorAnexo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;

                _ColaboradorAnexo.Arquivo = _ColaboradorAnexoTemp.Arquivo == null ? "" : _ColaboradorAnexoTemp.Arquivo;

                
                //_Con.Close();
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand _sqlCmd;
                if (_ColaboradorAnexo.ColaboradorAnexoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update ColaboradoresAnexos Set " +
                        "Descricao= '" + _ColaboradorAnexo.Descricao + "'" +
                        ",NomeArquivo= '" + _ColaboradorAnexo.NomeArquivo + "'" +
                        ",Arquivo= '" + _ColaboradorAnexo.Arquivo + "'" +
                        " Where ColaboradorAnexoID = " + _ColaboradorAnexo.ColaboradorAnexoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into ColaboradoresAnexos (ColaboradorID,Descricao,NomeArquivo ,Arquivo) values (" +
                                                          _ColaboradorAnexo.ColaboradorID + ",'" + _ColaboradorAnexo.Descricao + "','" +
                                                          _ColaboradorAnexo.NomeArquivo + "','" +  _ColaboradorAnexo.Arquivo + "')", _Con);
                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereColaboradorAnexoBD ex: " + ex);
                

            }
        }

        private void ExcluiColaboradorAnexoBD(int _ColaboradorAnexoID) // alterar para xml
        {
            try
            {

                
                //_Con.Close();
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from ColaboradoresAnexos where ColaboradorAnexoID=" + _ColaboradorAnexoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiColaboradorAnexoBD ex: " + ex);
                

            }
        }
        #endregion

        #region Metodos privados
        private string CriaXmlImagem( int colaboradorAnexoID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseArquivosImagens = _xmlDocument.CreateElement("ClasseArquivosImagens");
                _xmlDocument.AppendChild(_ClasseArquivosImagens);

                XmlNode _ArquivosImagens = _xmlDocument.CreateElement("ArquivosImagens");
                _ClasseArquivosImagens.AppendChild(_ArquivosImagens);

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand SQCMDXML = new SqlCommand("Select * From ColaboradoresAnexos Where  ColaboradorAnexoID = " + colaboradorAnexoID + "", _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Arquivo"].ToString())));
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
