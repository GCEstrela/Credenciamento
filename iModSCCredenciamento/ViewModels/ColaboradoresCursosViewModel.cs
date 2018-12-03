using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using iModSCCredenciamento.ViewModels;
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
using AutoMapper;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradoresCursosViewModel : ViewModelBase
    {
        Global g = new Global();
        #region Inicializacao
        public ColaboradoresCursosViewModel()
        {
            Thread CarregaColecaoCursos_thr = new Thread(() => CarregaColecaoCursos());
            CarregaColecaoCursos_thr.Start();
            //CarregaColecaoCursos();
        }

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso> _ColaboradoresCursos;

        private ClasseColaboradoresCursos.ColaboradorCurso _ColaboradorCursoSelecionado;

        private ClasseColaboradoresCursos.ColaboradorCurso _ColaboradorCursoTemp = new ClasseColaboradoresCursos.ColaboradorCurso();

        private List<ClasseColaboradoresCursos.ColaboradorCurso> _ColaboradoresCursosTemp = new List<ClasseColaboradoresCursos.ColaboradorCurso>();

        private ObservableCollection<ClasseCursos.Curso> _Cursos;

        PopupPesquisaColaboradorCurso popupPesquisaColaboradorCurso;

        private int _selectedIndex;

        private int _ColaboradorCursoSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso> ColaboradoresCursos
        {
            get
            {
                return _ColaboradoresCursos;
            }

            set
            {
                if (_ColaboradoresCursos != value)
                {
                    _ColaboradoresCursos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseCursos.Curso> Cursos
        {
            get
            {
                return _Cursos;
            }

            set
            {
                if (_Cursos != value)
                {
                    _Cursos = value;
                    OnPropertyChanged();
                }
            }
        }
        public ClasseColaboradoresCursos.ColaboradorCurso ColaboradorCursoSelecionado
        {
            get
            {
                return this._ColaboradorCursoSelecionado;
            }
            set
            {
                this._ColaboradorCursoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (ColaboradorCursoSelecionado != null)
                {
                    //OnEmpresaSelecionada();
                }

            }
        }

        public int ColaboradorCursoSelecionadaID
        {
            get
            {
                return this._ColaboradorCursoSelecionadaID;
            }
            set
            {
                this._ColaboradorCursoSelecionadaID = value;
                base.OnPropertyChanged();
                if (ColaboradorCursoSelecionadaID != null)
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
        public void OnAtualizaCommand(object _colaboradorCursoID)
        {

                ColaboradorCursoSelecionadaID = Convert.ToInt32(_colaboradorCursoID);
            Thread CarregaColecaoColaboradorerCursos_thr = new Thread(() => CarregaColecaoColaboradorerCursos(Convert.ToInt32(_colaboradorCursoID)));
            CarregaColecaoColaboradorerCursos_thr.Start();
            //CarregaColecaoColaboradorerCursos(Convert.ToInt32(_colaboradorCursoID));

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

                _ColaboradorCursoTemp.NomeArquivo = _nomecompletodoarquivo;
                _ColaboradorCursoTemp.Arquivo = _arquivoSTR;

                if (ColaboradoresCursos != null)
                {
                    ColaboradoresCursos[0].NomeArquivo = _nomecompletodoarquivo;
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

                    if (_ColaboradorCursoTemp != null)
                    {
                        if (_ColaboradorCursoTemp.Arquivo != null && _ColaboradorCursoTemp.ColaboradorCursoID == ColaboradorCursoSelecionado.ColaboradorCursoID)
                        {
                            _ArquivoPDF = _ColaboradorCursoTemp.Arquivo;

                        }
                    }
                    if (_ArquivoPDF == null)
                    {
                        string _xmlstring = CriaXmlImagem(ColaboradorCursoSelecionado.ColaboradorCursoID);

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
                    Global.Log("Erro na void ListaARQColaboradorAnexo_lv_PreviewMouseDoubleClick ex: " + ex);

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
                _ColaboradorCursoTemp = ColaboradorCursoSelecionado.CriaCopia(ColaboradorCursoSelecionado);
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
                ColaboradoresCursos[_selectedIndexTemp] = _ColaboradorCursoTemp;
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresCursos));

                ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso> _ColaboradoresCursosTemp = new ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso>();
                ClasseColaboradoresCursos _ClasseColaboradoresCursosTemp = new ClasseColaboradoresCursos();
                _ColaboradoresCursosTemp.Add(ColaboradorCursoSelecionado);
                _ClasseColaboradoresCursosTemp.ColaboradoresCursos = _ColaboradoresCursosTemp;

                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseColaboradoresCursosTemp);
                //        xmlString = sw.ToString();
                //    }

                //}

                //InsereColaboradorCursoBD(xmlString);

                IMOD.Domain.Entities.ColaboradorCurso ColaboradorCursoEntity = new IMOD.Domain.Entities.ColaboradorCurso();
                g.TranportarDados(ColaboradorCursoSelecionado, 1, ColaboradorCursoEntity);

                var repositorio = new IMOD.Infra.Repositorios.ColaboradorCursoRepositorio();
                repositorio.Alterar(ColaboradorCursoEntity);
                var id = ColaboradorCursoSelecionado.ColaboradorID;

                Thread CarregaColecaoColaboradorerCursos_thr = new Thread(() => CarregaColecaoColaboradorerCursos(id));
                CarregaColecaoColaboradorerCursos_thr.Start();

                _ColaboradoresCursosTemp = null;

                //_ColaboradoresCursosTemp.Clear();
                //_ColaboradorCursoTemp = null;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresCursos));

                ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso> _ColaboradoresCursosPro = new ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso>();
                ClasseColaboradoresCursos _ClasseColaboradoresCursosPro = new ClasseColaboradoresCursos();
                _ColaboradoresCursosPro.Add(ColaboradorCursoSelecionado);
                _ClasseColaboradoresCursosPro.ColaboradoresCursos = _ColaboradoresCursosPro;

                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseColaboradoresCursosPro);
                //        xmlString = sw.ToString();
                //    }

                //}

                //InsereColaboradorCursoBD(xmlString);

                IMOD.Domain.Entities.ColaboradorCurso ColaboradorCursoEntity = new IMOD.Domain.Entities.ColaboradorCurso();
                g.TranportarDados(ColaboradorCursoSelecionado, 1, ColaboradorCursoEntity);

                var repositorio = new IMOD.Infra.Repositorios.ColaboradorCursoRepositorio();
                repositorio.Criar(ColaboradorCursoEntity);
                var id = ColaboradorCursoSelecionado.ColaboradorID;

                Thread CarregaColecaoColaboradorerCursos_thr = new Thread(() => CarregaColecaoColaboradorerCursos(id));
                CarregaColecaoColaboradorerCursos_thr.Start();
                //_ColaboradoresCursosTemp.Add(ColaboradorCursoSelecionado);
                //ColaboradoresCursos = null;
                //ColaboradoresCursos = new ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso>(_ColaboradoresCursosTemp);
                //SelectedIndex = _selectedIndexTemp;
                //_ColaboradoresCursosTemp.Clear();

                _ColaboradoresCursosPro = null;

                //_ColaboradoresCursosPro.Clear();
                _ColaboradorCursoTemp = null;



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
                foreach (var x in ColaboradoresCursos)
                {
                    _ColaboradoresCursosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                ColaboradoresCursos.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _ColaboradorCursoTemp = new ClasseColaboradoresCursos.ColaboradorCurso();
                _ColaboradorCursoTemp.ColaboradorID = ColaboradorCursoSelecionadaID;
                ColaboradoresCursos.Add(_ColaboradorCursoTemp);
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
                ColaboradoresCursos = null;
                ColaboradoresCursos = new ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso>(_ColaboradoresCursosTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresCursosTemp.Clear();
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
                //if (MessageBox.Show("Tem certeza que deseja excluir este curso?", "Excluir Curso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados deste curso, inclusive histórico. Confirma exclusão?", "Excluir Curso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {

                //        ExcluiColaboradorCursoBD(ColaboradorCursoSelecionado.ColaboradorCursoID);

                //        ColaboradoresCursos.Remove(ColaboradorCursoSelecionado);

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {

                        IMOD.Domain.Entities.ColaboradorCurso ColaboradorCursoEntity = new IMOD.Domain.Entities.ColaboradorCurso();
                        g.TranportarDados(ColaboradorCursoSelecionado, 1, ColaboradorCursoEntity);

                        var repositorio = new IMOD.Infra.Repositorios.ColaboradorCursoRepositorio();
                        repositorio.Remover(ColaboradorCursoEntity);
                       
                        ColaboradoresCursos.Remove(ColaboradorCursoSelecionado);
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
                popupPesquisaColaboradorCurso = new PopupPesquisaColaboradorCurso();
                popupPesquisaColaboradorCurso.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaColaboradorCurso.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaColaboradorCurso.Criterio.Split((char)(20));
            int _colaboradorID = ColaboradorCursoSelecionadaID;
            string _id = ((string[])vetor)[0];
            string _numeroapolice = ((string[])vetor)[1];
            CarregaColecaoColaboradorerCursos(Convert.ToInt32(_id), _numeroapolice);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        public void CarregaColecaoColaboradorerCursos(int _colaboradorID, string _descricao = "", string _curso = "")
        {
            try
            {
                
                var service = new IMOD.Application.Service.ColaboradorCursosService();
                if (!string.IsNullOrWhiteSpace(_descricao)) _descricao = $"%{_descricao}%";
                if (!string.IsNullOrWhiteSpace(_curso)) _curso = $"%{_curso}%";
                var list1 = service.Listar(_colaboradorID, _descricao, _curso);

                var list2 = Mapper.Map<List<ClasseColaboradoresCursos.ColaboradorCurso>>(list1);

                var observer = new ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.ColaboradoresCursos = observer;
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        public void CarregaColecaoCursos()
        {
            try
            {
                string _xml = RequisitaCursos();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseCursos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseCursos classeCursos = new ClasseCursos();
                classeCursos = (ClasseCursos)deserializer.Deserialize(reader);
                Cursos = new ObservableCollection<ClasseCursos.Curso>();
                Cursos = classeCursos.Cursos;
                SelectedIndex = -1;

                /////////////////////////////////////////////
                //var service = new IMOD.Application.Service.ColaboradorCursosService();
                //if (!string.IsNullOrWhiteSpace(_descricao)) _descricao = $"%{_descricao}%";
                //if (!string.IsNullOrWhiteSpace(_curso)) _curso = $"%{_curso}%";
                //var list1 = service.Listar(_colaboradorID, _descricao, _curso);

                //var list2 = Mapper.Map<List<ClasseColaboradoresCursos.ColaboradorCurso>>(list1);

                //var observer = new ObservableCollection<ClasseColaboradoresCursos.ColaboradorCurso>();
                //list2.ForEach(n =>
                //{
                //    observer.Add(n);
                //});

                //this.ColaboradoresCursos = observer;
                //SelectedIndex = 0;
                /////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access
        private string RequisitaColaboradoresCursos(string _colaboradorID, string _descricao = "", string _curso = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseColaboradoresCursos = _xmlDocument.CreateElement("ClasseColaboradoresCursos");
                _xmlDocument.AppendChild(_ClasseColaboradoresCursos);

                XmlNode _ColaboradoresCursos = _xmlDocument.CreateElement("ColaboradoresCursos");
                _ClasseColaboradoresCursos.AppendChild(_ColaboradoresCursos);

                string _strSql;

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                _curso = "%" + _curso + "%";
                _descricao = "%" + _descricao + "%";
                _strSql = "SELECT dbo.ColaboradoresCursos.ColaboradorCursoID, dbo.ColaboradoresCursos.ColaboradorID, dbo.ColaboradoresCursos.CursoID," +
                    " dbo.ColaboradoresCursos.NomeArquivo, dbo.ColaboradoresCursos.Validade, dbo.ColaboradoresCursos.Controlado, dbo.Cursos.Descricao " +
                    "FROM dbo.ColaboradoresCursos INNER JOIN dbo.Cursos ON dbo.ColaboradoresCursos.CursoID = dbo.Cursos.CursoID where ColaboradorID = " +
                    _colaboradorID + " and NomeArquivo Like '" + _descricao + "' order by ColaboradorCursoID desc"; // and NumeroApolice Like '" + _numeroapolice + "'
                //_strSql = "select * from ColaboradoresCursos where ColaboradorID = " + _colaboradorID + "";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _ColaboradorCurso = _xmlDocument.CreateElement("ColaboradorCurso");
                    _ColaboradoresCursos.AppendChild(_ColaboradorCurso);

                    XmlNode _ColaboradorCursoID = _xmlDocument.CreateElement("ColaboradorCursoID");
                    _ColaboradorCursoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorCursoID"].ToString())));
                    _ColaboradorCurso.AppendChild(_ColaboradorCursoID);

                    XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
                    _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
                    _ColaboradorCurso.AppendChild(_ColaboradorID);

                    XmlNode _CursoID = _xmlDocument.CreateElement("CursoID");
                    _CursoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CursoID"].ToString())));
                    _ColaboradorCurso.AppendChild(_CursoID);

                    XmlNode _CursoNome = _xmlDocument.CreateElement("CursoNome");
                    _CursoNome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString()).Trim()));
                    _ColaboradorCurso.AppendChild(_CursoNome);

                    XmlNode _NomeArquivo = _xmlDocument.CreateElement("NomeArquivo");
                    _NomeArquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivo"].ToString()).Trim()));
                    _ColaboradorCurso.AppendChild(_NomeArquivo);

                    //XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                    //_Validade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Validade"].ToString()).Trim()));
                    //_ColaboradorCurso.AppendChild(_Validade);

                    var dt1 = (_sqlreader["Validade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dt1))
                    {
                        var dt2 = Convert.ToDateTime(dt1);
                        XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                        //format valid for XML W3C yyyy-MM-ddTHH:mm:ss
                        _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _ColaboradorCurso.AppendChild(_Validade);
                    }

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Arquivo"].ToString())));
                    _ColaboradorCurso.AppendChild(_Arquivo);

                    XmlNode _Controlado = _xmlDocument.CreateElement("Controlado");
                    _Controlado.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Controlado"])).ToString()));
                    _ColaboradorCurso.AppendChild(_Controlado);

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
        private string RequisitaCursos()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClassesCursos = _xmlDocument.CreateElement("ClasseCursos");
                _xmlDocument.AppendChild(_ClassesCursos);

                XmlNode _sCursos = _xmlDocument.CreateElement("Cursos");
                _ClassesCursos.AppendChild(_sCursos);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from Cursos ", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Curso = _xmlDocument.CreateElement("Curso");
                    _sCursos.AppendChild(_Curso);

                    XmlNode _CursoID = _xmlDocument.CreateElement("CursoID");
                    _CursoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["CursoID"].ToString())));
                    _Curso.AppendChild(_CursoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _Curso.AppendChild(_Descricao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaCursos ex: " + ex);
                
                return null;

            }
        }
        private void InsereColaboradorCursoBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();
                _xmlDoc.LoadXml(xmlString);

                ClasseColaboradoresCursos.ColaboradorCurso _ColaboradorCurso = new ClasseColaboradoresCursos.ColaboradorCurso();

                int i = 0;

                _ColaboradorCurso.ColaboradorCursoID = _xmlDoc.GetElementsByTagName("ColaboradorCursoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorCursoID")[i].InnerText);
                _ColaboradorCurso.ColaboradorID = _xmlDoc.GetElementsByTagName("ColaboradorID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorID")[i].InnerText);
                _ColaboradorCurso.CursoID = _xmlDoc.GetElementsByTagName("CursoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("CursoID")[i].InnerText);
                _ColaboradorCurso.NomeArquivo = _xmlDoc.GetElementsByTagName("NomeArquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText;

                _ColaboradorCurso.Validade = _xmlDoc.GetElementsByTagName("Validade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Validade")[i].InnerText);

                bool _controlado;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Controlado")[i].InnerText, out _controlado);

                _ColaboradorCurso.Controlado = _xmlDoc.GetElementsByTagName("Controlado")[i] == null ? false : _controlado;

                _ColaboradorCurso.NomeArquivo = _ColaboradorCursoTemp.NomeArquivo == null ? "" : _ColaboradorCursoTemp.NomeArquivo;
                _ColaboradorCurso.Arquivo = _ColaboradorCursoTemp.Arquivo == null ? "" : _ColaboradorCursoTemp.Arquivo;

                
                //_Con.Close();
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand _sqlCmd;
                if (_ColaboradorCurso.ColaboradorCursoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update ColaboradoresCursos Set " +
                        "ColaboradorID=@V0,CursoID= @V1,NomeArquivo=@V2 ,Validade=@V3,Arquivo= @V4,Controlado=@V5 " +
                        " Where ColaboradorCursoID = @V6", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = _ColaboradorCurso.ColaboradorID;
                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _ColaboradorCurso.CursoID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _ColaboradorCurso.NomeArquivo;
                    if (_ColaboradorCurso.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = _ColaboradorCurso.Validade;
                    }

                    _sqlCmd.Parameters.Add("@V4", SqlDbType.VarChar).Value = _ColaboradorCurso.Arquivo;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.Bit).Value = _ColaboradorCurso.Controlado;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _ColaboradorCurso.ColaboradorCursoID;

                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into ColaboradoresCursos (ColaboradorID  ,CursoID  ,NomeArquivo ,Validade,Arquivo,Controlado)" +
                        " values (@V0,@v1,@v2,@v3,@v4,@v5)", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = _ColaboradorCurso.ColaboradorID;
                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _ColaboradorCurso.CursoID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _ColaboradorCurso.NomeArquivo;
                    if (_ColaboradorCurso.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = _ColaboradorCurso.Validade;
                    }
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.VarChar).Value = _ColaboradorCurso.Arquivo;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.Bit).Value = _ColaboradorCurso.Controlado;

                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();

                if (_controlado)
                {
                    _Con = new SqlConnection(Global._connectionString); _Con.Open();

                    _sqlCmd = new SqlCommand("SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome, dbo.ColaboradoresEmpresas.Ativo, dbo.ColaboradoresEmpresas.Validade " +
                        "FROM dbo.Colaboradores INNER JOIN dbo.ColaboradoresEmpresas ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresEmpresas.ColaboradorID " +
                        "WHERE(dbo.Colaboradores.ColaboradorID = " + _ColaboradorCurso.ColaboradorID + " ) AND(dbo.ColaboradoresEmpresas.Ativo = 1)",_Con);

                    SqlDataReader _sqlreader = _sqlCmd.ExecuteReader(CommandBehavior.Default);
                    while (_sqlreader.Read())
                    {
                        string _dataTeste = _sqlreader["Validade"].ToString().Trim();

                        if ((_dataTeste != null || _dataTeste=="") && _ColaboradorCurso.Validade != null)
                        {
                            if (Convert.ToDateTime(_sqlreader["Validade"].ToString()) > Convert.ToDateTime(_ColaboradorCurso.Validade))
                            {
                                Global.PopupBox("O colaborador [" + _sqlreader["Nome"].ToString() + "] está inativo devido a data de validade!",4);
                                SqlConnection _Con2 = new SqlConnection(Global._connectionString); _Con2.Open();
                                SqlCommand _sqlCmd2 = new SqlCommand("Update ColaboradoresEmpresas Set Validade = '" + _ColaboradorCurso.Validade + "'" +
                                " Where ColaboradorID = " + _ColaboradorCurso.ColaboradorID + " AND Ativo = 1", _Con2);
                                _sqlCmd2.ExecuteNonQuery();
                                _Con2.Close();
                            }
                        }

                        

                    }
                    _sqlreader.Close();

                    _Con.Close();
                }




            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereColaboradorCursoBD ex: " + ex);
                

            }
        }

        private void ExcluiColaboradorCursoBD(int _ColaboradorCursoID) // alterar para xml
        {
            try
            {

                
                //_Con.Close();
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from ColaboradoresCursos where ColaboradorCursoID=" + _ColaboradorCursoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiColaboradorCursoBD ex: " + ex);
                

            }
        }
        #endregion

        #region Metodos privados
        private string CriaXmlImagem(int colaboradorCursoID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From ColaboradoresCursos Where ColaboradorCursoID = " + colaboradorCursoID + "", _Con);
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

        private void ValidadeCurso()
        {
            try
            {

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _strSql = "SELECT dbo.Colaboradores.ColaboradorID, dbo.Colaboradores.Nome,CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) " +
                                "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103)) AS Dias FROM dbo.Colaboradores " +
                                "INNER JOIN dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorID = dbo.ColaboradoresCursos.ColaboradorID where dbo.Colaboradores.Excluida = 0 And dbo.ColaboradoresCursos.Controlado = 1 And CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) < GETDATE()";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    if (Convert.ToDateTime(_sqlreader["ValidadeCurso"].ToString()) < DateTime.Now)
                    {
                        Global.PopupBox("Data de validade do curso do colaborador [ " + _sqlreader["Nome"].ToString() + " ] vencida!",  4);
                    }

                }
                _sqlreader.Close();

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

    }
}
