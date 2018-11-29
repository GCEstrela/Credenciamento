using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
using System.Xml;
using System.Xml.Serialization;

namespace iModSCCredenciamento.ViewModels
{
    public class ConfiguracoesViewModel : ViewModelBase
    {
        #region Inicializacao
        public ConfiguracoesViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
        }

        private void CarregaUI()
        {
            CarregaColecaoTiposEquipamentos();
            CarregaColecaoTiposAtividades();
            CarregaColecaoTiposCobrancas();
            CarregaColecaoStatus();
            CarregaColecaoCursos();
            CarregaColecaoAreasAcessos();
            CarregaColecaoTiposAcessos();
            CarregaColecaoRelatorios();
            CarregaColecaoRelatoriosGerenciais();
            CarregaColecaoLayoutsCrachas();
        }
        #endregion

        #region Variaveis privadas
        private SynchronizationContext MainThread;

        private ObservableCollection<ClasseRelatorios.Relatorio> _Relatorios;
        private ObservableCollection<ClasseRelatoriosGerenciais.RelatorioGerencial> _RelatoriosGerenciais;



        private ClasseRelatorios.Relatorio _RelatorioTemp = new ClasseRelatorios.Relatorio();
        private List<ClasseRelatorios.Relatorio> _RelatoriosTemp = new List<ClasseRelatorios.Relatorio>();

        private ClasseRelatoriosGerenciais.RelatorioGerencial _RelatorioGerencialTemp = new ClasseRelatoriosGerenciais.RelatorioGerencial();
        private List<ClasseRelatoriosGerenciais.RelatorioGerencial> _RelatoriosGerenciaisTemp = new List<ClasseRelatoriosGerenciais.RelatorioGerencial>();

        private ClasseLayoutsCrachas.LayoutCracha _LayoutCrachaTemp = new ClasseLayoutsCrachas.LayoutCracha();
        private List<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachasTemp = new List<ClasseLayoutsCrachas.LayoutCracha>();

        private ClasseRelatorios.Relatorio _RelatorioSelecionado;
        private ClasseRelatoriosGerenciais.RelatorioGerencial _RelatorioGerencialSelecionado;


        private int _relatorioSelectedIndex;
        private int _relatorioGerencialSelectedIndex;

        //private ObservableCollection<ClasseTiposEquipamento.TipoEquipamento> _TiposEquipamento;
        private ObservableCollection<ClasseTiposEquipamento.TipoEquipamento> _TiposEquipamentos;
        private List<ClasseTiposEquipamento.TipoEquipamento> _TiposEquipamentosTemp = new List<ClasseTiposEquipamento.TipoEquipamento>();
        private ClasseTiposEquipamento.TipoEquipamento _TipoEquipamentoSelecionado;
        private int _tipoEquipamentoSelectedIndex;

        private ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _TiposCobrancas;
        //private ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _TiposCobranca;
        private List<ClasseTiposCobrancas.TipoCobranca> _TiposCobrancasTemp = new List<ClasseTiposCobrancas.TipoCobranca>();
        private ClasseTiposCobrancas.TipoCobranca _CobrancaSelecionada;
        private int _tipoCobrancaSelectedIndex;

        //private ObservableCollection<ClasseTiposAtividades.TipoAtividade> _TiposAtividades;
        private ObservableCollection<ClasseTiposAtividades.TipoAtividade> _TiposAtividade;
        private List<ClasseTiposAtividades.TipoAtividade> _TiposAtividadesTemp = new List<ClasseTiposAtividades.TipoAtividade>();
        private ClasseTiposAtividades.TipoAtividade _AtividadeSelecionada;
        private int _tipoAtividadeSelectedIndex;

        private ObservableCollection<ClasseCursos.Curso> _Cursos;
        //private ObservableCollection<ClasseCursos.Curso> _TiposCurso;       
        private List<ClasseCursos.Curso> _cursosTemp = new List<ClasseCursos.Curso>();
        private ClasseCursos.Curso _CursosSelecionado;
        private int _cursoSelectedIndex;


        private ObservableCollection<ClasseStatus.Status> _TiposStatus;
        private List<ClasseStatus.Status> _StatusTemp = new List<ClasseStatus.Status>();
        private ClasseStatus.Status _StatusSelecionado;
        private int _tipoStatusSelectedIndex;

        private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _TiposAcessos;
        private List<ClasseTiposAcessos.TipoAcesso> _TiposAcessosTemp = new List<ClasseTiposAcessos.TipoAcesso>();
        private ClasseTiposAcessos.TipoAcesso _TiposAcessoSelecionado;
        private int _tipoAcessoSelectedIndex;
        //private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _TiposAcesso;



        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreaAcessos;
        private ClasseAreasAcessos.AreaAcesso _AcessoAreaSelecionada;
        private int _areaAcessoSelectedIndex;

        private int _selectedAcessoIndex;

        private int _selectedIndex;
        private int _selectedIndexTemp = 0;
        private ClasseLayoutsCrachas.LayoutCracha _LayoutCrachaSelecionado;
        private ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachas;
        private int _LayoutCrachaSelectedIndex;


        #endregion

        #region Contrutores
        public ObservableCollection<ClasseTiposEquipamento.TipoEquipamento> TiposEquipamentos
        {
            get
            {
                return _TiposEquipamentos;
            }

            set
            {
                if (_TiposEquipamentos != value)
                {
                    _TiposEquipamentos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> LayoutsCrachas
        {
            get
            {
                return _LayoutsCrachas;
            }

            set
            {
                if (_LayoutsCrachas != value)
                {
                    _LayoutsCrachas = value;
                    OnPropertyChanged();

                }
            }
        }
        public ClasseTiposEquipamento.TipoEquipamento TipoEquipamentoSelecionado
        {
            get
            {
                return this._TipoEquipamentoSelecionado;
            }
            set
            {
                this._TipoEquipamentoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoEquipamentoSelecionado != null)
                {

                }

            }
        }
        public int TipoEquipamentoSelectedIndex
        {
            get
            {
                return _tipoEquipamentoSelectedIndex;
            }
            set
            {
                _tipoEquipamentoSelectedIndex = value;
                OnPropertyChanged("TipoEquipamentoSelectedIndex");
            }
        }

        public ObservableCollection<ClasseRelatorios.Relatorio> Relatorios
        {
            get
            {
                return _Relatorios;
            }

            set
            {
                if (_Relatorios != value)
                {
                    _Relatorios = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseRelatoriosGerenciais.RelatorioGerencial> RelatoriosGerenciais
        {
            get
            {
                return _RelatoriosGerenciais;
            }

            set
            {
                if (_RelatoriosGerenciais != value)
                {
                    _RelatoriosGerenciais = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseRelatorios.Relatorio RelatorioSelecionado
        {
            get
            {
                return this._RelatorioSelecionado;
            }
            set
            {
                this._RelatorioSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (_RelatorioSelecionado != null)
                {

                }

            }
        }

        public ClasseLayoutsCrachas.LayoutCracha LayoutCrachaSelecionado
        {
            get
            {
                return this._LayoutCrachaSelecionado;
            }
            set
            {
                this._LayoutCrachaSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (_LayoutCrachaSelecionado != null)
                {

                }

            }
        }

        public ClasseRelatoriosGerenciais.RelatorioGerencial RelatorioGerencialSelecionado
        {
            get
            {
                return this._RelatorioGerencialSelecionado;
            }
            set
            {
                this._RelatorioGerencialSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (_RelatorioGerencialSelecionado != null)
                {

                }

            }
        }

        public int RelatorioSelectedIndex
        {
            get
            {
                return _relatorioSelectedIndex;
            }
            set
            {
                _relatorioSelectedIndex = value;
                OnPropertyChanged("RelatorioSelectedIndex");
            }
        }

        public int LayoutCrachaSelectedIndex
        {
            get
            {
                return _LayoutCrachaSelectedIndex;
            }
            set
            {
                _LayoutCrachaSelectedIndex = value;
                OnPropertyChanged("RelatorioSelectedIndex");
            }
        }

        public int RelatorioGerencialSelectedIndex
        {
            get
            {
                return _relatorioGerencialSelectedIndex;
            }
            set
            {
                _relatorioGerencialSelectedIndex = value;
                OnPropertyChanged("RelatorioGerencialSelectedIndex");
            }
        }

        //public ObservableCollection<ClasseTiposAtividades.TipoAtividade> TiposAtividade
        //{
        //    get
        //    {
        //        return _TiposAtividade;
        //    }

        //    set
        //    {
        //        if (_TiposAtividade != value)
        //        {
        //            _TiposAtividade = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}
        public ObservableCollection<ClasseTiposAtividades.TipoAtividade> TiposAtividades
        {
            get
            {
                return _TiposAtividade;
            }

            set
            {
                if (_TiposAtividade != value)
                {
                    _TiposAtividade = value;
                    OnPropertyChanged();

                }
            }
        }
        public ClasseTiposAtividades.TipoAtividade TipoAtividadeSelecionada
        {
            get
            {
                return this._AtividadeSelecionada;
            }
            set
            {
                this._AtividadeSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoAtividadeSelecionada != null)
                {

                }

            }
        }
        public int TipoAtividadeSelectedIndex
        {
            get
            {
                return _tipoAtividadeSelectedIndex;
            }
            set
            {
                _tipoAtividadeSelectedIndex = value;
                OnPropertyChanged("TipoAtividadeSelectedIndex");
            }
        }

        public ObservableCollection<ClasseTiposCobrancas.TipoCobranca> TiposCobrancas
        {
            get
            {
                return _TiposCobrancas;
            }

            set
            {
                if (_TiposCobrancas != value)
                {
                    _TiposCobrancas = value;
                    OnPropertyChanged();

                }
            }
        }
        public ClasseTiposCobrancas.TipoCobranca TipoCobrancaSelecionado
        {
            get
            {
                return this._CobrancaSelecionada;
            }
            set
            {
                this._CobrancaSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoCobrancaSelecionado != null)
                {

                }

            }
        }
        public int TipoCobrancaSelectedIndex
        {
            get
            {
                return _tipoCobrancaSelectedIndex;
            }
            set
            {
                _tipoCobrancaSelectedIndex = value;
                OnPropertyChanged("TipoCobrancaSelectedIndex");
            }
        }

        public ObservableCollection<ClasseAreasAcessos.AreaAcesso> AreasAcessos
        {
            get
            {
                return _AreaAcessos;
            }

            set
            {
                if (_AreaAcessos != value)
                {
                    _AreaAcessos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ClasseAreasAcessos.AreaAcesso AreaAcessoSelecionada
        {
            get
            {
                return this._AcessoAreaSelecionada;
            }
            set
            {
                this._AcessoAreaSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (AreaAcessoSelecionada != null)
                {

                }

            }
        }
        public int AreaAcessoSelectedIndex
        {
            get
            {
                return _areaAcessoSelectedIndex;
            }
            set
            {
                _areaAcessoSelectedIndex = value;
                OnPropertyChanged("AreaAcessoSelectedIndex");
            }
        }

        public ObservableCollection<ClasseStatus.Status> TiposStatus
        {
            get
            {
                return _TiposStatus;
            }

            set
            {
                if (_TiposStatus != value)
                {
                    _TiposStatus = value;
                    OnPropertyChanged();

                }
            }
        }
        public ClasseStatus.Status TipoStatusSelecionado
        {
            get
            {
                return this._StatusSelecionado;
            }
            set
            {
                this._StatusSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoStatusSelecionado != null)
                {

                }

            }
        }
        public int TipoStatusSelectedIndex
        {
            get
            {
                return _tipoStatusSelectedIndex;
            }
            set
            {
                _tipoStatusSelectedIndex = value;
                OnPropertyChanged("TipoStatusSelectedIndex");
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
        public ClasseCursos.Curso CursoSelecionado
        {
            get
            {
                return this._CursosSelecionado;
            }
            set
            {
                this._CursosSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (CursoSelecionado != null)
                {

                }

            }
        }
        public int CursoSelectedIndex
        {
            get
            {
                return _cursoSelectedIndex;
            }
            set
            {
                _cursoSelectedIndex = value;
                OnPropertyChanged("CursoSelectedIndex");
            }
        }

        public ObservableCollection<ClasseTiposAcessos.TipoAcesso> TiposAcessos
        {
            get
            {
                return _TiposAcessos;
            }

            set
            {
                if (_TiposAcessos != value)
                {
                    _TiposAcessos = value;
                    OnPropertyChanged();

                }
            }
        }




        public ClasseTiposAcessos.TipoAcesso TipoAcessoSelecionado
        {
            get
            {
                return this._TiposAcessoSelecionado;
            }
            set
            {
                this._TiposAcessoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (TipoAcessoSelecionado != null)
                {

                }

            }
        }
        public int TipoAcessoSelectedIndex
        {
            get
            {
                return _tipoAcessoSelectedIndex;
            }
            set
            {
                _tipoAcessoSelectedIndex = value;
                OnPropertyChanged("TipoAcessoSelectedIndex");
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


        #endregion

        #region Comandos dos Botoes

        #region Comandos dos Botoes TiposEquipamentos
        public void OnSalvarEdicaoCommand_TiposEquipamentos()
        {
            try
            {
                //HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseTiposEquipamento));

                ObservableCollection<ClasseTiposEquipamento.TipoEquipamento> _TiposEquipamentoTemp = new ObservableCollection<ClasseTiposEquipamento.TipoEquipamento>();
                ClasseTiposEquipamento _ClasseEquipamentoTemp = new ClasseTiposEquipamento();
                _TiposEquipamentoTemp.Add(TipoEquipamentoSelecionado);
                _ClasseEquipamentoTemp.TiposEquipamentos = _TiposEquipamentoTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEquipamentoTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereTipoEquipamentoBD(xmlString);

                CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {
            }
        }
        public void OnAdicionarCommand_TiposEquipamentos()
        {
            try
            {
                foreach (var x in TiposEquipamentos)
                {
                    _TiposEquipamentosTemp.Add(x);
                }

                _tipoEquipamentoSelectedIndex = TipoEquipamentoSelectedIndex;
                TiposEquipamentos.Clear();
                ClasseTiposEquipamento.TipoEquipamento _tipoEquipamento = new ClasseTiposEquipamento.TipoEquipamento();
                TiposEquipamentos.Add(_tipoEquipamento);

                TipoEquipamentoSelectedIndex = 0;

            }
            catch (Exception ex)
            {
            }

        }
        public void OnExcluirCommand_TiposEquipamentos()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este tipo de equipamento?", "Excluir tipo de equipamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                //    ExcluiTipoEquipamentoBD(TipoEquipamentoSelecionado.TipoEquipamentoID);

                //    TiposEquipamentos.Remove(TipoEquipamentoSelecionado);
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiTipoEquipamentoBD(TipoEquipamentoSelecionado.TipoEquipamentoID);

                        TiposEquipamentos.Remove(TipoEquipamentoSelecionado);
                    }
                }


            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region Comandos dos Botoes Relatorios

        public void OnSalvarRelatorioCommand()
        {
            try
            {
                //HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseRelatorios));

                ObservableCollection<ClasseRelatorios.Relatorio> _RelatorioPro = new ObservableCollection<ClasseRelatorios.Relatorio>();
                ClasseRelatorios _ClasseRelatoriosPro = new ClasseRelatorios();
                _RelatorioPro.Add(RelatorioSelecionado);
                _ClasseRelatoriosPro.Relatorios = _RelatorioPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseRelatoriosPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereRelatoriosBD(xmlString);

                CarregaColecaoRelatorios();
            }
            catch (Exception ex)
            {
            }
        }
        public void OnAdicionarRelatorioCommand()
        {
            try
            {

                foreach (var x in Relatorios)
                {
                    _RelatoriosTemp.Add(x);
                }

                _relatorioSelectedIndex = RelatorioSelectedIndex;
                Relatorios.Clear();

                _RelatorioTemp = new ClasseRelatorios.Relatorio();
                Relatorios.Add(_RelatorioTemp);

                RelatorioSelectedIndex = 0;


            }
            catch (Exception ex)
            {
            }

        }
        public void OnExcluirRelatorioCommand()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este relatório?", "Excluir Relatório", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                //    ExcluiRelatorioBD(RelatorioSelecionado.RelatorioID);

                //    Relatorios.Remove(RelatorioSelecionado);
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiRelatorioBD(RelatorioSelecionado.RelatorioID);

                    Relatorios.Remove(RelatorioSelecionado);

                }
            }
            catch (Exception ex)
            {
            }

        }
        public void OnBuscarRelatorioCommand()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog _arquivoRPT = new System.Windows.Forms.OpenFileDialog();

                string _nomecompletodoarquivo;
                string _arquivoSTR;
                _arquivoRPT.InitialDirectory = "c:\\\\";
                _arquivoRPT.Filter = "Reports Files|*.rpt"; ;
                _arquivoRPT.RestoreDirectory = true;
                _arquivoRPT.ShowDialog();
                //if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
                //{
                _nomecompletodoarquivo = _arquivoRPT.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoRPT.FileName);

                _RelatorioTemp.NomeArquivoRPT = _nomecompletodoarquivo;
                _RelatorioTemp.ArquivoRPT = _arquivoSTR;
                //InsereArquivoBD(Convert.ToInt32(empresaID), _nomecompletodoarquivo, _arquivoSTR);

                //AtualizaListaAnexos(_resp);

                //}
            }
            catch (Exception ex)
            {

            }
        }
        public void OnAbrirRelatorioCommand()
        {
            try
            {
                string _xmlstring = CriaXmlRelatorio(RelatorioSelecionado.RelatorioID);

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = (XmlNode)xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;

                //ReportDocument reportDocument = new ReportDocument();


                //File.Move(_caminhoArquivoPDF, Path.ChangeExtension(_caminhoArquivoPDF, ".pdf"));
                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                //Esse ponto de implementação para a alteração da instancia do SQL, banco, usuário e senha
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                reportDocument.Load(_ArquivoRPT);
                crConnectionInfo.ServerName = "(localdb)\\SQLEXPRESS";
                crConnectionInfo.DatabaseName = "D_iModCredenciamento";
                crConnectionInfo.UserID = "imod"; // Global._usuario;
                crConnectionInfo.Password = "imod"; //Global._senha;
                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    }));

                }

                );
                //CarregaRel_thr.SetApartmentState(ApartmentState.STA);
                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);

            }

        }

        #endregion

        #region Comandos dos Botoes Relatorios Gerenciais

        public void OnSalvarRelatorioGerencialCommand()
        {
            try
            {
                //HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseRelatoriosGerenciais));

                ObservableCollection<ClasseRelatoriosGerenciais.RelatorioGerencial> _RelatorioPro = new ObservableCollection<ClasseRelatoriosGerenciais.RelatorioGerencial>();
                ClasseRelatoriosGerenciais _ClasseRelatoriosGerenciaisPro = new ClasseRelatoriosGerenciais();
                _RelatorioPro.Add(RelatorioGerencialSelecionado);
                _ClasseRelatoriosGerenciaisPro.RelatoriosGerenciais = _RelatorioPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseRelatoriosGerenciaisPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereRelatoriosGerenciaisBD(xmlString);

                CarregaColecaoRelatoriosGerenciais();
            }
            catch (Exception ex)
            {
            }
        }
        public void OnAdicionarRelatorioGerencialCommand()
        {
            try
            {

                foreach (var x in RelatoriosGerenciais)
                {
                    _RelatoriosGerenciaisTemp.Add(x);
                }

                _relatorioGerencialSelectedIndex = RelatorioGerencialSelectedIndex;
                RelatoriosGerenciais.Clear();

                _RelatorioGerencialTemp = new ClasseRelatoriosGerenciais.RelatorioGerencial();
                RelatoriosGerenciais.Add(_RelatorioGerencialTemp);

                RelatorioGerencialSelectedIndex = 0;

            }
            catch (Exception ex)
            {

            }

        }
        public void OnExcluirRelatorioGerencialCommand()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiRelatorioGerencialBD(RelatorioGerencialSelecionado.RelatorioID);

                    RelatoriosGerenciais.Remove(RelatorioGerencialSelecionado);

                }
            }
            catch (Exception ex)
            {
            }

        }
        public void OnBuscarRelatorioGerencialCommand()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog _arquivoRPT = new System.Windows.Forms.OpenFileDialog();

                string _nomecompletodoarquivo;
                string _arquivoSTR;

                _arquivoRPT.InitialDirectory = "c:\\\\";
                _arquivoRPT.Filter = "Reports Files|*.rpt"; ;
                _arquivoRPT.RestoreDirectory = true;
                _arquivoRPT.ShowDialog();

                _nomecompletodoarquivo = _arquivoRPT.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoRPT.FileName);

                _RelatorioGerencialTemp.NomeArquivoRPT = _nomecompletodoarquivo;
                _RelatorioGerencialTemp.ArquivoRPT = _arquivoSTR;

            }
            catch (Exception ex)
            {

            }
        }
        public void OnAbrirRelatorioGerencialCommand()
        {
            try
            {
                string _xmlstring = CriaXmlRelatorioGerencial(RelatorioGerencialSelecionado.RelatorioID);

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = (XmlNode)xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;


                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                //Esse ponto de implementação para a alteração da instancia do SQL, banco, usuário e senha
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                reportDocument.Load(_ArquivoRPT);
                crConnectionInfo.ServerName = "(localdb)\\SQLEXPRESS";
                crConnectionInfo.DatabaseName = "D_iModCredenciamento";
                crConnectionInfo.UserID = "imod"; // Global._usuario;
                crConnectionInfo.Password = "imod"; //Global._senha;
                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    }));

                }

                );
                //CarregaRel_thr.SetApartmentState(ApartmentState.STA);
                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);

            }

        }

        #endregion

        #region Comandos dos Botoes LayoutCrachas

        public void OnSalvarLayoutCrachaCommand()
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseLayoutsCrachas));

                ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> _LayoutCrachaPro = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();
                ClasseLayoutsCrachas _ClasseLayoutsCrachasPro = new ClasseLayoutsCrachas();
                _LayoutCrachaPro.Add(LayoutCrachaSelecionado);
                _ClasseLayoutsCrachasPro.LayoutsCrachas = _LayoutCrachaPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseLayoutsCrachasPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereLayoutsCrachasBD(xmlString);

                CarregaColecaoLayoutsCrachas();
            }
            catch (Exception ex)
            {
            }
        }
        public void OnAdicionarLayoutCrachaCommand()
        {
            try
            {

                foreach (var x in LayoutsCrachas)
                {
                    _LayoutsCrachasTemp.Add(x);
                }

                _LayoutCrachaSelectedIndex = LayoutCrachaSelectedIndex;
                LayoutsCrachas.Clear();

                _LayoutCrachaTemp = new ClasseLayoutsCrachas.LayoutCracha();
                LayoutsCrachas.Add(_LayoutCrachaTemp);

                LayoutCrachaSelectedIndex = 0;

            }
            catch (Exception ex)
            {

            }

        }
        public void OnExcluirLayoutCrachaCommand()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiLayoutCrachaBD(LayoutCrachaSelecionado.LayoutCrachaID);

                    LayoutsCrachas.Remove(LayoutCrachaSelecionado);

                }
            }
            catch (Exception ex)
            {
            }

        }
        public void OnBuscarLayoutCrachaCommand()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog _arquivoRPT = new System.Windows.Forms.OpenFileDialog();

                string _nomecompletodoarquivo;
                string _arquivoSTR;

                _arquivoRPT.InitialDirectory = "c:\\\\";
                _arquivoRPT.Filter = "Reports Files|*.rpt"; ;
                _arquivoRPT.RestoreDirectory = true;
                _arquivoRPT.ShowDialog();

                _nomecompletodoarquivo = _arquivoRPT.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoRPT.FileName);

                _LayoutCrachaTemp.Nome = _nomecompletodoarquivo;
                _LayoutCrachaTemp.LayoutRPT = _arquivoSTR;

            }
            catch (Exception ex)
            {

            }
        }
        public void OnAbrirLayoutCrachaCommand()
        {
            try
            {
                string _xmlstring = CriaXmlRelatorioGerencial(RelatorioGerencialSelecionado.RelatorioID);

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = (XmlNode)xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;


                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                //Esse ponto de implementação para a alteração da instancia do SQL, banco, usuário e senha
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                reportDocument.Load(_ArquivoRPT);
                crConnectionInfo.ServerName = Global._instancia; // "(localdb)\\SQLEXPRESS";
                crConnectionInfo.DatabaseName = Global._bancoDados; // "D_iModCredenciamento";
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;
                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    }));

                }

                );
                //CarregaRel_thr.SetApartmentState(ApartmentState.STA);
                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);

            }

        }

        #endregion

        #region Comandos dos Botoes TiposAtividades
        public void OnAdicionarCommand_TiposAtividades()
        {
            try
            {
                foreach (var x in TiposAtividades)
                {
                    _TiposAtividadesTemp.Add(x);
                }

                _tipoAtividadeSelectedIndex = TipoAtividadeSelectedIndex;
                TiposAtividades.Clear();
                ClasseTiposAtividades.TipoAtividade _atividade = new ClasseTiposAtividades.TipoAtividade();
                TiposAtividades.Add(_atividade);

                TipoAtividadeSelectedIndex = 0;

            }
            catch (Exception ex)
            {

            }

        }
        public void OnSalvarEdicaoCommand_TiposAtividades()
        {
            try
            {
                //HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseTiposAtividades));

                ObservableCollection<ClasseTiposAtividades.TipoAtividade> _TiposAtividadesTemp = new ObservableCollection<ClasseTiposAtividades.TipoAtividade>();
                ClasseTiposAtividades _ClasseAtividadeTemp = new ClasseTiposAtividades();
                _TiposAtividadesTemp.Add(TipoAtividadeSelecionada);
                _ClasseAtividadeTemp.TiposAtividades = _TiposAtividadesTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseAtividadeTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereTipoAtividadeBD(xmlString);
                CarregaColecaoTiposAtividades();
            }
            catch (Exception ex)
            {
            }
        }
        public void OnExcluirCommand_TiposAtividades()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este tipo de atividade?", "Excluir tipo de atividade", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                //    ExcluiTipoAtividadeBD(TipoAtividadeSelecionada.TipoAtividadeID);
                //    //TiposAtividade.Remove(AtividadeSelecionada);
                //    CarregaColecaoTiposAtividades();
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiTipoAtividadeBD(TipoAtividadeSelecionada.TipoAtividadeId);
                    //TiposAtividade.Remove(AtividadeSelecionada);
                    CarregaColecaoTiposAtividades();

                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region Comandos dos Botoes TiposCobrancas
        public void OnAdicionarCommand_TiposCobrancas()
        {
            try
            {
                foreach (var x in TiposCobrancas)
                {
                    _TiposCobrancasTemp.Add(x);
                }

                _tipoCobrancaSelectedIndex = TipoCobrancaSelectedIndex;
                TiposCobrancas.Clear();
                ClasseTiposCobrancas.TipoCobranca _cobranca = new ClasseTiposCobrancas.TipoCobranca();
                TiposCobrancas.Add(_cobranca);

                TipoCobrancaSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {

            }

        }
        public void OnSalvarEdicaoCommand_TiposCobrancas()
        {
            try
            {
                //HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseTiposCobrancas));

                ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _TiposCobrancaTemp = new ObservableCollection<ClasseTiposCobrancas.TipoCobranca>();
                ClasseTiposCobrancas _ClasseAtividadeTemp = new ClasseTiposCobrancas();
                _TiposCobrancaTemp.Add(TipoCobrancaSelecionado);
                _ClasseAtividadeTemp.TiposCobrancas = _TiposCobrancaTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseAtividadeTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereTiposCobrancasBD(xmlString);
                CarregaColecaoTiposCobrancas();


            }
            catch (Exception ex)
            {
            }
        }
        public void OnExcluirCommand_TiposCobrancas()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este tipo de cobrança?", "Excluir tipo de cobrança", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                //    ExcluiTiposCobrancasBD(TipoCobrancaSelecionado.TipoCobrancaID);
                //    //TiposAtividade.Remove(AtividadeSelecionada);
                //    CarregaColecaoTiposCobrancas();

                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiTiposCobrancasBD(TipoCobrancaSelecionado.TipoCobrancaID);
                    //TiposAtividade.Remove(AtividadeSelecionada);
                    CarregaColecaoTiposCobrancas();

                }

            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region Comandos dos Botoes TiposAcesso
        public void OnAdicionarCommand_TiposAcesso()
        {
            try
            {
                foreach (var x in TiposAcessos)
                {
                    _TiposAcessosTemp.Add(x);
                }

                _tipoAcessoSelectedIndex = TipoAcessoSelectedIndex;
                TiposAcessos.Clear();
                ClasseTiposAcessos.TipoAcesso _acesso = new ClasseTiposAcessos.TipoAcesso();
                TiposAcessos.Add(_acesso);

                TipoAcessoSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {

            }

        }
        public void OnSalvarEdicaoCommand_TiposAcesso()
        {
            try
            {
                //HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseTiposAcessos));

                ObservableCollection<ClasseTiposAcessos.TipoAcesso> _TiposAcessoTemp = new ObservableCollection<ClasseTiposAcessos.TipoAcesso>();
                ClasseTiposAcessos _ClasseAcessoTemp = new ClasseTiposAcessos();
                _TiposAcessoTemp.Add(TipoAcessoSelecionado);
                _ClasseAcessoTemp.TiposAcessos = _TiposAcessoTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseAcessoTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereTiposAcessosBD(xmlString);
                CarregaColecaoTiposAcessos();


            }
            catch (Exception ex)
            {
            }
        }
        public void OnExcluirCommand_TiposAcesso()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este tipo de acesso?", "Excluir tipo de acesso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                //    ExcluiTiposAcessosBD(TipoAcessoSelecionado.TipoAcessoID);
                //    //TiposAtividade.Remove(AtividadeSelecionada);
                //    CarregaColecaoTiposAcessos();

                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiTiposAcessosBD(TipoAcessoSelecionado.TipoAcessoID);
                    //TiposAtividade.Remove(AtividadeSelecionada);
                    CarregaColecaoTiposAcessos();

                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region Comandos dos Botoes TiposStatus
        public void OnAdicionarCommand_TiposStatus()
        {
            try
            {
                foreach (var x in TiposStatus)
                {
                    _StatusTemp.Add(x);
                }

                _tipoStatusSelectedIndex = TipoStatusSelectedIndex;
                TiposStatus.Clear();
                ClasseStatus.Status _status = new ClasseStatus.Status();
                TiposStatus.Add(_status);

                TipoStatusSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {

            }

        }
        public void OnSalvarEdicaoCommand_TiposStatus()
        {
            try
            {
                //HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseStatus));

                ObservableCollection<ClasseStatus.Status> _StatusTemp = new ObservableCollection<ClasseStatus.Status>();
                ClasseStatus _ClasseStatusTemp = new ClasseStatus();
                _StatusTemp.Add(TipoStatusSelecionado);
                _ClasseStatusTemp.Statuss = _StatusTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseStatusTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereStatusBD(xmlString);
                CarregaColecaoStatus();


            }
            catch (Exception ex)
            {
            }
        }
        public void OnExcluirCommand_TiposStatus()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este tipo de status?", "Excluir tipo de status", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                //    ExcluiStatusBD(TipoStatusSelecionado.StatusID);
                //    //TiposAtividade.Remove(AtividadeSelecionada);
                //    CarregaColecaoStatus();

                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiStatusBD(TipoStatusSelecionado.StatusID);
                    //TiposAtividade.Remove(AtividadeSelecionada);
                    CarregaColecaoStatus();

                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region Comandos dos Botoes TiposCursos
        public void OnAdicionarCommand_TiposCursos()
        {
            try
            {
                foreach (var x in Cursos)
                {
                    _cursosTemp.Add(x);
                }

                _cursoSelectedIndex = CursoSelectedIndex;
                Cursos.Clear();
                ClasseCursos.Curso _cursos = new ClasseCursos.Curso();
                Cursos.Add(_cursos);

                CursoSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {

            }

        }
        public void OnSalvarEdicaoCommand_TiposCursos()
        {
            try
            {
                //HabilitaEdicao = false;Curso
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseCursos));

                ObservableCollection<ClasseCursos.Curso> _CursosTemp = new ObservableCollection<ClasseCursos.Curso>();
                ClasseCursos _ClasseCursosTemp = new ClasseCursos();
                _CursosTemp.Add(CursoSelecionado);
                _ClasseCursosTemp.Cursos = _CursosTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseCursosTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereCursosBD(xmlString);
                CarregaColecaoCursos();


            }
            catch (Exception ex)
            {
            }
        }
        public void OnExcluirCommand_TiposCursos()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este tipo de curso?", "Excluir tipo de curso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                //    ExcluiCursosBD(CursoSelecionado.CursoID);
                //    //TiposAtividade.Remove(AtividadeSelecionada);
                //    CarregaColecaoCursos();

                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiCursosBD(CursoSelecionado.CursoID);
                    //TiposAtividade.Remove(AtividadeSelecionada);
                    CarregaColecaoCursos();

                }

            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #region Comandos dos Botoes AreaAcesso
        public void OnAdicionarCommand_AreaAcesso()
        {
            try
            {
                foreach (var x in Cursos)
                {
                    _cursosTemp.Add(x);
                }

                _areaAcessoSelectedIndex = AreaAcessoSelectedIndex;
                AreasAcessos.Clear();
                ClasseAreasAcessos.AreaAcesso _areaAcesso = new ClasseAreasAcessos.AreaAcesso();
                AreasAcessos.Add(_areaAcesso);

                AreaAcessoSelectedIndex = 0;
                //CarregaColecaoTiposEquipamentos();
            }
            catch (Exception ex)
            {

            }

        }
        public void OnSalvarEdicaoCommand_AreaAcesso()
        {
            try
            {
                //HabilitaEdicao = false;Curso
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseAreasAcessos));

                ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreaAcessoTemp = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
                ClasseAreasAcessos _ClasseAreaAcessoTemp = new ClasseAreasAcessos();
                _AreaAcessoTemp.Add(AreaAcessoSelecionada);
                _ClasseAreaAcessoTemp.AreasAcessos = _AreaAcessoTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseAreaAcessoTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereAreasAcessosBD(xmlString);
                CarregaColecaoAreasAcessos();


            }
            catch (Exception ex)
            {
            }
        }
        public void OnExcluirCommand_AreaAcesso()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este tipo de área de acesso?", "Excluir tipo de área de acesso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                //    ExcluiAreasAcessosBD(AreaAcessoSelecionada.AreaAcessoID);
                //    //TiposAtividade.Remove(AtividadeSelecionada);
                //    CarregaColecaoAreasAcessos();

                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                    ExcluiAreasAcessosBD(AreaAcessoSelecionada.AreaAcessoID);
                    //TiposAtividade.Remove(AtividadeSelecionada);
                    CarregaColecaoAreasAcessos();

                }

            }
            catch (Exception ex)
            {
            }

        }
        #endregion

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoTiposEquipamentos()
        {
            try
            {
                string _xml = RequisitaTiposEquipamentos();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposEquipamento));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseTiposEquipamento classeTiposEquipamentos = new ClasseTiposEquipamento();
                classeTiposEquipamentos = (ClasseTiposEquipamento)deserializer.Deserialize(reader);
                TiposEquipamentos = new ObservableCollection<ClasseTiposEquipamento.TipoEquipamento>();
                TiposEquipamentos = classeTiposEquipamentos.TiposEquipamentos;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoTiposAtividades()
        {
            try
            {
                string _xml = RequisitaTiposAtividades();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposAtividades));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseTiposAtividades classeTiposAtividades = new ClasseTiposAtividades();
                classeTiposAtividades = (ClasseTiposAtividades)deserializer.Deserialize(reader);
                TiposAtividades = new ObservableCollection<ClasseTiposAtividades.TipoAtividade>();
                TiposAtividades = classeTiposAtividades.TiposAtividades;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoTiposCobrancas()
        {
            try
            {

                string _xml = RequisitaTiposCobrancas();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposCobrancas));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseTiposCobrancas classeTiposCobrancas = new ClasseTiposCobrancas();
                classeTiposCobrancas = (ClasseTiposCobrancas)deserializer.Deserialize(reader);
                TiposCobrancas = new ObservableCollection<ClasseTiposCobrancas.TipoCobranca>();
                TiposCobrancas = classeTiposCobrancas.TiposCobrancas;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoTiposCobrancas ex: " + ex);
            }
        }

        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                string _xml = RequisitaAreasAcessos();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseAreasAcessos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseAreasAcessos classeAreasAcessos = new ClasseAreasAcessos();
                classeAreasAcessos = (ClasseAreasAcessos)deserializer.Deserialize(reader);
                AreasAcessos = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
                AreasAcessos = classeAreasAcessos.AreasAcessos;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoStatus()
        {
            try
            {
                string _xml = RequisitaTiposStatus();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseStatus));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseStatus classeTiposStatus = new ClasseStatus();
                classeTiposStatus = (ClasseStatus)deserializer.Deserialize(reader);
                TiposStatus = new ObservableCollection<ClasseStatus.Status>();
                TiposStatus = classeTiposStatus.Statuss;

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
                //SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void CarregaColecaoLayoutsCrachas()
        {
            try
            {
                string _xml = RequisitaLayoutsCrachas();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseLayoutsCrachas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseLayoutsCrachas classeLayoutsCrachas = new ClasseLayoutsCrachas();
                classeLayoutsCrachas = (ClasseLayoutsCrachas)deserializer.Deserialize(reader);
                LayoutsCrachas = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();
                LayoutsCrachas = classeLayoutsCrachas.LayoutsCrachas;
                //SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoTiposAcessos()
        {
            try
            {

                string _xml = RequisitaTiposAcessos();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposAcessos));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseTiposAcessos classeTiposAcessos = new ClasseTiposAcessos();
                classeTiposAcessos = (ClasseTiposAcessos)deserializer.Deserialize(reader);
                TiposAcessos = new ObservableCollection<ClasseTiposAcessos.TipoAcesso>();
                TiposAcessos = classeTiposAcessos.TiposAcessos;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoTiposAcessos ex: " + ex);
            }
        }

        private void CarregaColecaoRelatorios()
        {
            try
            {

                string _xml = RequisitaRelatorios();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseRelatorios));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);

                ClasseRelatorios classeRelatorios = new ClasseRelatorios();
                classeRelatorios = (ClasseRelatorios)deserializer.Deserialize(reader);
                Relatorios = new ObservableCollection<ClasseRelatorios.Relatorio>();
                Relatorios = classeRelatorios.Relatorios;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoRelatorios ex: " + ex);
            }
        }

        private void CarregaColecaoRelatoriosGerenciais()
        {
            try
            {

                string _xml = RequisitaRelatoriosGerenciais();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseRelatoriosGerenciais));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);

                ClasseRelatoriosGerenciais classeRelatoriosGerenciais = new ClasseRelatoriosGerenciais();
                classeRelatoriosGerenciais = (ClasseRelatoriosGerenciais)deserializer.Deserialize(reader);
                RelatoriosGerenciais = new ObservableCollection<ClasseRelatoriosGerenciais.RelatorioGerencial>();
                RelatoriosGerenciais = classeRelatoriosGerenciais.RelatoriosGerenciais;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoRelatoriosGerenciais ex: " + ex);
            }
        }


        #endregion

        #region Data Access
        private string RequisitaTiposEquipamentos()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposEquipamento = _xmlDocument.CreateElement("ClasseTiposEquipamento");
                _xmlDocument.AppendChild(_ClasseTiposEquipamento);

                XmlNode _TiposEquipamentos = _xmlDocument.CreateElement("TiposEquipamentos");
                _ClasseTiposEquipamento.AppendChild(_TiposEquipamentos);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlcmd = new SqlCommand("select * from TiposEquipamentos order by TipoEquipamentoID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoEquipamento = _xmlDocument.CreateElement("TipoEquipamento");
                    _TiposEquipamentos.AppendChild(_TipoEquipamento);

                    XmlNode _TipoEquipamentoID = _xmlDocument.CreateElement("TipoEquipamentoID");
                    _TipoEquipamentoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoEquipamentoID"].ToString())));
                    _TipoEquipamento.AppendChild(_TipoEquipamentoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _TipoEquipamento.AppendChild(_Descricao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaTiposAtividades ex: " + ex);

                return null;
            }
        }

        private string RequisitaTiposAtividades()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposAtividades = _xmlDocument.CreateElement("ClasseTiposAtividades");
                _xmlDocument.AppendChild(_ClasseTiposAtividades);

                XmlNode _TiposAtividades = _xmlDocument.CreateElement("TiposAtividades");
                _ClasseTiposAtividades.AppendChild(_TiposAtividades);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from TiposAtividades order by TipoAtividadeID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoAtividade = _xmlDocument.CreateElement("TipoAtividade");
                    _TiposAtividades.AppendChild(_TipoAtividade);

                    XmlNode _TipoAtividadeID = _xmlDocument.CreateElement("TipoAtividadeID");
                    _TipoAtividadeID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoAtividadeID"].ToString())));
                    _TipoAtividade.AppendChild(_TipoAtividadeID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _TipoAtividade.AppendChild(_Descricao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaTiposAtividades ex: " + ex);

                return null;
            }
        }

        private string RequisitaTiposCobrancas()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposCobrancas = _xmlDocument.CreateElement("ClasseTiposCobrancas");
                _xmlDocument.AppendChild(_ClasseTiposCobrancas);

                XmlNode _TiposCobrancas = _xmlDocument.CreateElement("TiposCobrancas");
                _ClasseTiposCobrancas.AppendChild(_TiposCobrancas);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from TiposCobrancas", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoCobranca = _xmlDocument.CreateElement("TipoCobranca");
                    _TiposCobrancas.AppendChild(_TipoCobranca);

                    XmlNode _TipoCobrancaID = _xmlDocument.CreateElement("TipoCobrancaID");
                    _TipoCobrancaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoCobrancaID"].ToString())));
                    _TipoCobranca.AppendChild(_TipoCobrancaID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _TipoCobranca.AppendChild(_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaTipoCobranca ex: " + ex);

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

        private string RequisitaTiposStatus()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseStatus = _xmlDocument.CreateElement("ClasseStatus");
                _xmlDocument.AppendChild(_ClasseStatus);

                XmlNode _Status = _xmlDocument.CreateElement("Statuss");
                _ClasseStatus.AppendChild(_Status);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlcmd = new SqlCommand("select * from Status order by StatusID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Statu = _xmlDocument.CreateElement("Status");
                    _Status.AppendChild(_Statu);

                    XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
                    _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["StatusID"].ToString())));
                    _Statu.AppendChild(_StatusID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _Statu.AppendChild(_Descricao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaTiposAtividades ex: " + ex);

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

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
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

        private string RequisitaLayoutsCrachas()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClassesLayoutsCrachas = _xmlDocument.CreateElement("ClasseLayoutsCrachas");
                _xmlDocument.AppendChild(_ClassesLayoutsCrachas);

                XmlNode _sLayoutsCrachas = _xmlDocument.CreateElement("LayoutsCrachas");
                _ClassesLayoutsCrachas.AppendChild(_sLayoutsCrachas);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from LayoutsCrachas ", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _LayoutCracha = _xmlDocument.CreateElement("LayoutCracha");
                    _sLayoutsCrachas.AppendChild(_LayoutCracha);

                    XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
                    _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaID"].ToString().Trim())));
                    _LayoutCracha.AppendChild(_LayoutCrachaID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString().Trim())));
                    _LayoutCracha.AppendChild(_Nome);

                    XmlNode _LayoutCrachaGUID = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _LayoutCrachaGUID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaGUID"].ToString().Trim())));
                    _LayoutCracha.AppendChild(_LayoutCrachaGUID);

                    XmlNode _LayoutRPT = _xmlDocument.CreateElement("LayoutRPT");
                    _LayoutRPT.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutRPT"].ToString().Trim())));
                    _LayoutCracha.AppendChild(_LayoutRPT);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaLayoutsCrachas ex: " + ex);

                return null;

            }
        }

        private string RequisitaTiposAcessos()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposAcessos = _xmlDocument.CreateElement("ClasseTiposAcessos");
                _xmlDocument.AppendChild(_ClasseTiposAcessos);

                XmlNode _TiposAcessos = _xmlDocument.CreateElement("TiposAcessos");
                _ClasseTiposAcessos.AppendChild(_TiposAcessos);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from TiposAcessos", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoAcesso = _xmlDocument.CreateElement("TipoAcesso");
                    _TiposAcessos.AppendChild(_TipoAcesso);

                    XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
                    _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoAcessoID"].ToString())));
                    _TipoAcesso.AppendChild(_TipoAcessoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _TipoAcesso.AppendChild(_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaTipoAcesso ex: " + ex);

                return null;
            }
        }

        private string RequisitaRelatorios()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseRelatorios = _xmlDocument.CreateElement("ClasseRelatorios");
                _xmlDocument.AppendChild(_ClasseRelatorios);

                XmlNode _Relatorios = _xmlDocument.CreateElement("Relatorios");
                _ClasseRelatorios.AppendChild(_Relatorios);

                string _strSql = " [RelatorioID],[Nome],[NomeArquivoRPT],[Ativo]";


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _strSql = "select " + _strSql + " from Relatorios order by RelatorioID ASC";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Relatorio = _xmlDocument.CreateElement("Relatorio");
                    _Relatorios.AppendChild(_Relatorio);

                    XmlNode _RelatorioID = _xmlDocument.CreateElement("RelatorioID");
                    _RelatorioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RelatorioID"].ToString())));
                    _Relatorio.AppendChild(_RelatorioID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _Relatorio.AppendChild(_Nome);

                    XmlNode _NomeArquivoRPT = _xmlDocument.CreateElement("NomeArquivoRPT");
                    _NomeArquivoRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivoRPT"].ToString())));
                    _Relatorio.AppendChild(_NomeArquivoRPT);

                    XmlNode _ArquivoRPT = _xmlDocument.CreateElement("ArquivoRPT");
                    //_ArquivoRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ArquivoRPT"].ToString())));
                    _Relatorio.AppendChild(_ArquivoRPT);

                    XmlNode _Ativo = _xmlDocument.CreateElement("Ativo");
                    _Ativo.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativo"])).ToString()));
                    _Relatorio.AppendChild(_Ativo);
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


        private string RequisitaRelatoriosGerenciais()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseRelatorios = _xmlDocument.CreateElement("ClasseRelatoriosGerenciais");
                _xmlDocument.AppendChild(_ClasseRelatorios);

                XmlNode _Relatorios = _xmlDocument.CreateElement("RelatoriosGerenciais");
                _ClasseRelatorios.AppendChild(_Relatorios);

                string _strSql = " [RelatorioID],[Nome],[NomeArquivoRPT],[Ativo]";


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _strSql = "select " + _strSql + " from RelatoriosGerenciais order by RelatorioID ASC";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Relatorio = _xmlDocument.CreateElement("RelatorioGerencial");
                    _Relatorios.AppendChild(_Relatorio);

                    XmlNode _RelatorioID = _xmlDocument.CreateElement("RelatorioID");
                    _RelatorioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RelatorioID"].ToString())));
                    _Relatorio.AppendChild(_RelatorioID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _Relatorio.AppendChild(_Nome);

                    XmlNode _NomeArquivoRPT = _xmlDocument.CreateElement("NomeArquivoRPT");
                    _NomeArquivoRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivoRPT"].ToString())));
                    _Relatorio.AppendChild(_NomeArquivoRPT);

                    XmlNode _ArquivoRPT = _xmlDocument.CreateElement("ArquivoRPT");
                    //_ArquivoRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ArquivoRPT"].ToString())));
                    _Relatorio.AppendChild(_ArquivoRPT);

                    XmlNode _Ativo = _xmlDocument.CreateElement("Ativo");
                    _Ativo.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativo"])).ToString()));
                    _Relatorio.AppendChild(_Ativo);
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



        private void InsereTipoEquipamentoBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseTiposEquipamento.TipoEquipamento _equipamento = new ClasseTiposEquipamento.TipoEquipamento();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _equipamento.TipoEquipamentoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoEquipamentoID")[i].InnerText);
                _equipamento.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Descricao")[i].InnerText);

                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_equipamento.TipoEquipamentoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update TiposEquipamentos Set" +
                        " Descricao= '" + _equipamento.Descricao + "'" +
                        " Where TipoEquipamentoID = " + _equipamento.TipoEquipamentoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into TiposEquipamentos(Descricao) values ('" + _equipamento.Descricao + "')", _Con);

                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereTipoEquipamentoBD ex: " + ex);


            }

        }

        private void InsereTipoAtividadeBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);

                ClasseTiposAtividades.TipoAtividade _atividade = new ClasseTiposAtividades.TipoAtividade();
                int i = 0;

                //_atividade.TipoAtividadeID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoAtividadeID")[i].InnerText);
                _atividade.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Descricao")[i].InnerText);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                //if (_atividade.TipoAtividadeID != 0)
                //{
                //    _sqlCmd = new SqlCommand("Update TiposAtividades Set" +
                //        " Descricao= '" + _atividade.Descricao + "'" +
                //        " Where TipoAtividadeID = " + _atividade.TipoAtividadeID + "", _Con);
                //}
                //else
                //{
                //    _sqlCmd = new SqlCommand("Insert into TiposAtividades(Descricao) values ('" + _atividade.Descricao + "')", _Con);

                //}

                //_sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereTipoEquipamentoBD ex: " + ex);


            }

        }

        private void InsereTiposCobrancasBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                ClasseTiposCobrancas.TipoCobranca _cobranca = new ClasseTiposCobrancas.TipoCobranca();
                int i = 0;

                _cobranca.TipoCobrancaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoCobrancaID")[i].InnerText);
                _cobranca.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Descricao")[i].InnerText);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_cobranca.TipoCobrancaID != 0)
                {
                    _sqlCmd = new SqlCommand("Update TiposCobrancas Set" +
                        " Descricao= '" + _cobranca.Descricao + "'" +
                        " Where TipoCobrancaID = " + _cobranca.TipoCobrancaID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into TiposCobrancas(Descricao) values ('" + _cobranca.Descricao + "')", _Con);

                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereTiposCobrancasBD ex: " + ex);


            }

        }

        private void InsereTiposAcessosBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                ClasseTiposAcessos.TipoAcesso _acesso = new ClasseTiposAcessos.TipoAcesso();
                int i = 0;

                _acesso.TipoAcessoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoAcessoID")[i].InnerText);
                _acesso.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Descricao")[i].InnerText);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_acesso.TipoAcessoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update TiposAcessos Set" +
                        " Descricao= '" + _acesso.Descricao + "'" +
                        " Where TipoAcessoID = " + _acesso.TipoAcessoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into TiposAcessos(Descricao) values ('" + _acesso.Descricao + "')", _Con);

                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereTiposAcessosBD ex: " + ex);

            }

        }

        private void InsereStatusBD(string xmlString)
        {
            try
            {
                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                ClasseStatus.Status _status = new ClasseStatus.Status();

                int i = 0;

                _status.StatusID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
                _status.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Descricao")[i].InnerText);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_status.StatusID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Status Set" +
                        " Descricao= '" + _status.Descricao + "'" +
                        " Where StatusID = " + _status.StatusID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into Status(Descricao) values ('" + _status.Descricao + "')", _Con);

                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereStatusBD ex: " + ex);
            }
        }

        private void InsereCursosBD(string xmlString)
        {
            try
            {
                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                ClasseCursos.Curso _cursos = new ClasseCursos.Curso();

                int i = 0;

                _cursos.CursoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("CursoID")[i].InnerText);
                _cursos.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Descricao")[i].InnerText);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_cursos.CursoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Cursos Set" +
                        " Descricao= '" + _cursos.Descricao + "'" +
                        " Where CursoID = " + _cursos.CursoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into Cursos(Descricao) values ('" + _cursos.Descricao + "')", _Con);

                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereCursosBD ex: " + ex);
            }
        }

        private void InsereAreasAcessosBD(string xmlString)
        {
            try
            {
                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                ClasseAreasAcessos.AreaAcesso _areaacesso = new ClasseAreasAcessos.AreaAcesso();

                int i = 0;

                _areaacesso.AreaAcessoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("AreaAcessoID")[i].InnerText);
                _areaacesso.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Descricao")[i].InnerText);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_areaacesso.AreaAcessoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update AreasAcessos Set" +
                        " Descricao= '" + _areaacesso.Descricao + "'" +
                        " Where AreaAcessoID = " + _areaacesso.AreaAcessoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into AreasAcessos(Descricao) values ('" + _areaacesso.Descricao + "')", _Con);

                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereAreasAcessosBD ex: " + ex);
            }
        }

        private void InsereRelatoriosBD(string xmlString)
        {
            try
            {
                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                ClasseRelatorios.Relatorio _relatorio = new ClasseRelatorios.Relatorio();

                int i = 0;

                _relatorio.RelatorioID = _xmlDoc.GetElementsByTagName("RelatorioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("RelatorioID")[i].InnerText);
                _relatorio.Nome = _xmlDoc.GetElementsByTagName("Nome")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Nome")[i].InnerText;
                _relatorio.NomeArquivoRPT = _RelatorioTemp.NomeArquivoRPT == null ? "" : _RelatorioTemp.NomeArquivoRPT;
                _relatorio.ArquivoRPT = _RelatorioTemp.ArquivoRPT == null ? "" : _RelatorioTemp.ArquivoRPT;
                bool _ativo;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Ativo")[i].InnerText, out _ativo);

                _relatorio.Ativo = _xmlDoc.GetElementsByTagName("Ativo")[i] == null ? false : _ativo;

                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_relatorio.RelatorioID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Relatorios Set " +
                        "Nome= '" + _relatorio.Nome + "'" +
                        ",NomeArquivoRPT= '" + _relatorio.NomeArquivoRPT + "'" +
                        ",ArquivoRPT= '" + _relatorio.ArquivoRPT + "'" +
                        ",Ativo= '" + _relatorio.Ativo + "'" +
                        " Where RelatorioID = " + _relatorio.RelatorioID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into Relatorios (Nome  ,NomeArquivoRPT ,ArquivoRPT,Ativo) values ('" +
                        _relatorio.Nome + "','" + _relatorio.NomeArquivoRPT + "','" + _relatorio.ArquivoRPT + "','" + _relatorio.Ativo + "')", _Con);

                }


                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereRelatoriosBD ex: " + ex);
            }
        }

        private void InsereRelatoriosGerenciaisBD(string xmlString)
        {
            try
            {
                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                ClasseRelatoriosGerenciais.RelatorioGerencial _relatorioGerencial = new ClasseRelatoriosGerenciais.RelatorioGerencial();

                int i = 0;

                _relatorioGerencial.RelatorioID = _xmlDoc.GetElementsByTagName("RelatorioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("RelatorioID")[i].InnerText);
                _relatorioGerencial.Nome = _xmlDoc.GetElementsByTagName("Nome")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Nome")[i].InnerText;
                _relatorioGerencial.NomeArquivoRPT = _RelatorioGerencialTemp.NomeArquivoRPT == null ? "" : _RelatorioGerencialTemp.NomeArquivoRPT;
                _relatorioGerencial.ArquivoRPT = _RelatorioGerencialTemp.ArquivoRPT == null ? "" : _RelatorioGerencialTemp.ArquivoRPT;
                bool _ativo;

                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Ativo")[i].InnerText, out _ativo);

                _relatorioGerencial.Ativo = _xmlDoc.GetElementsByTagName("Ativo")[i] == null ? false : _ativo;

                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_relatorioGerencial.RelatorioID != 0)
                {
                    _sqlCmd = new SqlCommand("Update RelatoriosGerenciais Set " +
                        "Nome= '" + _relatorioGerencial.Nome + "'" +
                        ",NomeArquivoRPT= '" + _relatorioGerencial.NomeArquivoRPT + "'" +
                        ",ArquivoRPT= '" + _relatorioGerencial.ArquivoRPT + "'" +
                        ",Ativo= '" + _relatorioGerencial.Ativo + "'" +
                        " Where RelatorioID = " + _relatorioGerencial.RelatorioID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into RelatoriosGerenciais (Nome  ,NomeArquivoRPT ,ArquivoRPT,Ativo) values ('" +
                        _relatorioGerencial.Nome + "','" + _relatorioGerencial.NomeArquivoRPT + "','" + _relatorioGerencial.ArquivoRPT + "','" + _relatorioGerencial.Ativo + "')", _Con);

                }


                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereRelatoriosGerenciaisBD ex: " + ex);
            }
        }

        private void InsereLayoutsCrachasBD(string xmlString)
        {
            try
            {
                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                ClasseLayoutsCrachas.LayoutCracha _layoutCracha = new ClasseLayoutsCrachas.LayoutCracha();

                int i = 0;

                _layoutCracha.LayoutCrachaID = _xmlDoc.GetElementsByTagName("LayoutCrachaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("LayoutCrachaID")[i].InnerText);
                _layoutCracha.Nome = _LayoutCrachaTemp.Nome == null ? "" : _LayoutCrachaTemp.Nome;
                _layoutCracha.LayoutRPT = _LayoutCrachaTemp.LayoutRPT == null ? "" : _LayoutCrachaTemp.LayoutRPT;

                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_layoutCracha.LayoutCrachaID != 0)
                {
                    _sqlCmd = new SqlCommand("Update LayoutsCrachas Set " +
                        "Nome= '" + _layoutCracha.Nome + "'" +
                        ",LayoutRPT= '" + _layoutCracha.LayoutRPT + "'" +
                        " Where LayoutCrachaID = " + _layoutCracha.LayoutCrachaID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into LayoutsCrachas (Nome  ,LayoutRPT ) values ('" +
                        _layoutCracha.Nome + "','" + _layoutCracha.LayoutRPT + "')", _Con);

                }


                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereLayoutsCrachasBD ex: " + ex);
            }
        }


        private void ExcluiTipoEquipamentoBD(int _TipoEquipamentoID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from TiposEquipamentos where TipoEquipamentoID=" + _TipoEquipamentoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiSeguroBD ex: " + ex);
            }
        }

        private void ExcluiTipoAtividadeBD(int _TipoAtividadeID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from TiposAtividades where TipoAtividadeID=" + _TipoAtividadeID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiSeguroBD ex: " + ex);
            }
        }

        private void ExcluiTiposCobrancasBD(int _TipoCobrancaID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from TiposCobrancas where TipoCobrancaID=" + _TipoCobrancaID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiSeguroBD ex: " + ex);
            }
        }

        private void ExcluiTiposAcessosBD(int _TipoAcessoID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from TiposAcessos where TipoAcessoID=" + _TipoAcessoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiTiposAcessosBD ex: " + ex);
            }
        }

        private void ExcluiStatusBD(int _StatusID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from Status where StatusID=" + _StatusID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiStatusBD ex: " + ex);
            }
        }

        private void ExcluiCursosBD(int _CursoID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from Cursos where CursoID=" + _CursoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiCursosBD ex: " + ex);
            }
        }

        private void ExcluiAreasAcessosBD(int _AreaAcessoID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from AreasAcessos where AreaAcessoID=" + _AreaAcessoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiAreasAcessosBD ex: " + ex);
            }
        }

        private void ExcluiRelatorioBD(int _RelatorioID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from Relatorios where RelatorioID=" + _RelatorioID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiRelatorioBD ex: " + ex);
            }
        }

        private void ExcluiRelatorioGerencialBD(int _RelatorioID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from RelatoriosGerenciais where RelatorioID=" + _RelatorioID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiRelatorioGerencialBD ex: " + ex);
            }
        }

        private void ExcluiLayoutCrachaBD(int _LayoutCrachaID)
        {
            try
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from LayoutsCrachas where LayoutCrachaID=" + _LayoutCrachaID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiLayoutCrachaBD ex: " + ex);
            }
        }

        #endregion

        #region Metodos privados
        private string CriaXmlRelatorio(int relatorioID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From Relatorios Where RelatorioID = " + relatorioID, _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["ArquivoRPT"].ToString())));
                    _ArquivoImagem.AppendChild(_Arquivo);

                }
                SQDR_XML.Close();

                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CriaXmlRelatorio ex: " + ex);
                return null;
            }
        }

        private string CriaXmlRelatorioGerencial(int relatorioID)
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

                SqlCommand SQCMDXML = new SqlCommand("SELECT * FROM RelatoriosGerenciais WHERE RelatorioID = " + relatorioID, _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["ArquivoRPT"].ToString())));
                    _ArquivoImagem.AppendChild(_Arquivo);

                }
                SQDR_XML.Close();

                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CriaXmlRelatorioGerencial ex: " + ex);
                return null;
            }
        }
        #endregion

    }
}
