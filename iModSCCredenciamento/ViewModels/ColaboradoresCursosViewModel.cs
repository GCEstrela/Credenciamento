using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
using iModSCCredenciamento.Views.Model;

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

        private ObservableCollection<ColaboradorCursoView> _ColaboradoresCursos;

        private ColaboradorCursoView _ColaboradorCursoSelecionado;

        private ColaboradorCursoView _ColaboradorCursoTemp = new ColaboradorCursoView();

        private List<ColaboradorCursoView> _ColaboradoresCursosTemp = new List<ColaboradorCursoView>();

        private ObservableCollection<ClasseCursos.Curso> _Cursos;

        PopupPesquisaColaboradorCurso popupPesquisaColaboradorCurso;

        private int _selectedIndex;

        private int _ColaboradorCursoSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Contrutores
        public ObservableCollection<ColaboradorCursoView> ColaboradoresCursos
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
        public ColaboradorCursoView ColaboradorCursoSelecionado
        {
            get
            {
                return _ColaboradorCursoSelecionado;
            }
            set
            {
                _ColaboradorCursoSelecionado = value;
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
                return _ColaboradorCursoSelecionadaID;
            }
            set
            {
                _ColaboradorCursoSelecionadaID = value;
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
                var arquivoStr = ColaboradorCursoSelecionado.Arquivo;
                var nomeArquivo = ColaboradorCursoSelecionado.NomeArquivo;
                var arrBytes = Convert.FromBase64String(arquivoStr);
                WpfHelp.DownloadArquivoDialog(nomeArquivo, arrBytes);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                //_ColaboradorCursoTemp = ColaboradorCursoSelecionado.CriaCopia(ColaboradorCursoSelecionado);
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


                var entity = Mapper.Map<ColaboradorCurso>(ColaboradorCursoSelecionado);
                var repositorio = new ColaboradorCursosService();
                repositorio.Alterar(entity);
                var id = ColaboradorCursoSelecionado.ColaboradorId;

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


                var entity = Mapper.Map<ColaboradorCurso>(ColaboradorCursoSelecionado);
                var repositorio = new ColaboradorCursosService();
                repositorio.Criar(entity);
                var id = ColaboradorCursoSelecionado.ColaboradorId;

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
                _ColaboradorCursoTemp = new ColaboradorCursoView();
                _ColaboradorCursoTemp.ColaboradorId = ColaboradorCursoSelecionadaID;
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
                ColaboradoresCursos = new ObservableCollection<ColaboradorCursoView>(_ColaboradoresCursosTemp);
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

                        var entity = Mapper.Map<ColaboradorCurso>(ColaboradorCursoSelecionado);
                        var repositorio = new ColaboradorCursosService();
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
                popupPesquisaColaboradorCurso.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaColaboradorCurso.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaColaboradorCurso.Criterio.Split((char)(20));
            int _ColaboradorId = ColaboradorCursoSelecionadaID;
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
        public void CarregaColecaoColaboradorerCursos(int _ColaboradorId, string _descricao = "", string _curso = "")
        {
            try
            {

                var service = new ColaboradorCursosService();
                if (!string.IsNullOrWhiteSpace(_descricao)) _descricao = $"%{_descricao}%";
                if (!string.IsNullOrWhiteSpace(_curso)) _curso = $"%{_curso}%";
                var list1 = service.Listar(_ColaboradorId, _descricao, _curso);

                var list2 = Mapper.Map<List<ColaboradorCursoView>>(list1);

                var observer = new ObservableCollection<ColaboradorCursoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                ColaboradoresCursos = observer;

                //Hotfix auto-selecionar registro no topo da ListView
                var topList = observer.FirstOrDefault();
                ColaboradorCursoSelecionado = topList;

                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                Utils.TraceException(ex);
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


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

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

                string _strSql = "SELECT dbo.Colaboradores.ColaboradorId, dbo.Colaboradores.Nome,CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) " +
                                "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103)) AS Dias FROM dbo.Colaboradores " +
                                "INNER JOIN dbo.ColaboradoresCursos ON dbo.Colaboradores.ColaboradorId = dbo.ColaboradoresCursos.ColaboradorId where dbo.Colaboradores.Excluida = 0 And dbo.ColaboradoresCursos.Controlado = 1 And CONVERT(datetime, dbo.ColaboradoresCursos.Validade, 103) < GETDATE()";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    if (Convert.ToDateTime(_sqlreader["ValidadeCurso"].ToString()) < DateTime.Now)
                    {
                        Global.PopupBox("Data de validade do curso do colaborador [ " + _sqlreader["Nome"] + " ] vencida!", 4);
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
