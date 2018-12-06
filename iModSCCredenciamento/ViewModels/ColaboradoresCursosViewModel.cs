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
using iModSCCredenciamento.Helpers;
using IMOD.Application.Service;
using IMOD.Application.Interfaces;
using IMOD.CrossCutting;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradoresCursosViewModel : ViewModelBase
    {
       
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        public List<ClasseCursos.Curso> ObterListaListaCursos { get; private set; }
        #region Inicializacao
        public ColaboradoresCursosViewModel()
        {
            CarregarDadosComunsEmMemoria();

            //Thread CarregaColecaoCursos_thr = new Thread(() => CarregaColecaoCursos());
            //CarregaColecaoCursos_thr.Start();

            CarregaColecaoCursos();
            
            //Thread CarregaUI_thr = new Thread(() => CarregaUI());
            //CarregaUI_thr.Start();
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
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _ColaboradorCursoTemp.NomeArquivo = arq.Nome;
                _ColaboradorCursoTemp.Arquivo = arq.FormatoBase64;
                if (ColaboradoresCursos != null)
                    ColaboradoresCursos[0].NomeArquivo = arq.Nome;

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
                

                var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorCurso>(ColaboradorCursoSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorCursosService();
                repositorio.Alterar(entity);
                var id = ColaboradorCursoSelecionado.ColaboradorID;

                Thread CarregaColecaoColaboradorerCursos_thr = new Thread(() => CarregaColecaoColaboradorerCursos(id));
                CarregaColecaoColaboradorerCursos_thr.Start();

                _ColaboradorCursoTemp = null;


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
               

                var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorCurso>(ColaboradorCursoSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorCursosService();
                repositorio.Criar(entity);
                var id = ColaboradorCursoSelecionado.ColaboradorID;

                Thread CarregaColecaoColaboradorerCursos_thr = new Thread(() => CarregaColecaoColaboradorerCursos(id));
                CarregaColecaoColaboradorerCursos_thr.Start();
               
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

                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {

                        var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorCurso>(ColaboradorCursoSelecionado);
                        var repositorio = new IMOD.Application.Service.ColaboradorCursosService();
                        repositorio.Remover(entity);
                       
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
        private void CarregarDadosComunsEmMemoria()
        {
            //Cursos
            var e1 = _auxiliaresService.CursoService.Listar();
            ObterListaListaCursos = Mapper.Map<List<ClasseCursos.Curso>>(e1);            

        }
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
                IMOD.CrossCutting.Utils.TraceException(ex);
            }
        }
        public void CarregaColecaoCursos()
        {
            try
            {
                

                var convert = Mapper.Map<List<ClasseCursos.Curso>>(ObterListaListaCursos);
                Cursos = new ObservableCollection<ClasseCursos.Curso>();
                convert.ForEach(n => { Cursos.Add(n); });


            }
            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
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
