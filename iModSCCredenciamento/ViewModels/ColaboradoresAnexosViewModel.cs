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
       
        private IColaboradorAnexoService _colaboradorAnexoService;
       
        #region Inicializacao
        public ColaboradoresAnexosViewModel()
        {
            _colaboradorAnexoService=new ColaboradorAnexoService();
            CarregaUI();
        }
        private void CarregaUI()
        {
            CarregaColecaoColaboradoresAnexos();
            
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

                long tamanho = new System.IO.FileInfo(_arquivoPDF.FileName).Length;
                if (tamanho > 200)
                {
                    System.Windows.MessageBox.Show("Tamanho ( " + tamanho.ToString() + " ) inválido, só é permitido arquivo com o máximo de 200");
                    return;
                }

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
                //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradoresAnexos));

                //ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo> _ColaboradoresAnexosTemp = new ObservableCollection<ClasseColaboradoresAnexos.ColaboradorAnexo>();
                //ClasseColaboradoresAnexos _ClasseColaboradoresAnexosTemp = new ClasseColaboradoresAnexos();
                //_ColaboradoresAnexosTemp.Add(ColaboradorAnexoSelecionado);
                //_ClasseColaboradoresAnexosTemp.ColaboradoresAnexos = _ColaboradoresAnexosTemp;


                //IMOD.Domain.Entities.ColaboradorAnexo ColaboradorAnexoEntity = new IMOD.Domain.Entities.ColaboradorAnexo();
                //g.TranportarDados(ColaboradorAnexoSelecionado, 1, ColaboradorAnexoEntity);

                //var repositorio = new IMOD.Infra.Repositorios.ColaboradorAnexoRepositorio();


                var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorAnexo>(ColaboradorAnexoSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorAnexoService();
                repositorio.Alterar(entity);



                var id = entity.ColaboradorId;

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

                //IMOD.Domain.Entities.ColaboradorAnexo ColaboradorAnexoEntity = new IMOD.Domain.Entities.ColaboradorAnexo();
                //g.TranportarDados(ColaboradorAnexoSelecionado, 1, ColaboradorAnexoEntity);


                var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorAnexo>(ColaboradorAnexoSelecionado);
                var repositorio = new IMOD.Application.Service.ColaboradorAnexoService();
                repositorio.Criar(entity);
                var id = entity.ColaboradorId;

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
                
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {

                       
                        var entity = Mapper.Map<IMOD.Domain.Entities.ColaboradorAnexo>(ColaboradorAnexoSelecionado);
                        var repositorio = new IMOD.Application.Service.ColaboradorAnexoService();
                        repositorio.Remover(entity);
                        
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
        public void CarregaColecaoColaboradoresAnexos(int _colaboradorID = 0, string _nome = "")
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
