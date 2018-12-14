using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

namespace iModSCCredenciamento.ViewModels
{
    public class VeiculosAnexosViewModel : ViewModelBase
    {
        #region Inicializacao
        public VeiculosAnexosViewModel()
        {
            CarregaUI();
        }
        private void CarregaUI()
        {
            //CarregaColecaoVeiculosAnexos();
            //CarregaColecaoAnexos();
        }
        #endregion


        #region Variaveis Privadas

        private ObservableCollection<ClasseVeiculosAnexos.VeiculoAnexo> _VeiculosAnexos;

        private ClasseVeiculosAnexos.VeiculoAnexo _VeiculoAnexoSelecionado;

        private ClasseVeiculosAnexos.VeiculoAnexo _VeiculoAnexoTemp = new ClasseVeiculosAnexos.VeiculoAnexo();

        private List<ClasseVeiculosAnexos.VeiculoAnexo> _VeiculosAnexosTemp = new List<ClasseVeiculosAnexos.VeiculoAnexo>();

        PopupPesquisaVeiculoAnexo popupPesquisaVeiculoAnexo;

        private int _selectedIndex;

        private int _VeiculoAnexoSelecionadoID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion


        #region Contrutores
        public ObservableCollection<ClasseVeiculosAnexos.VeiculoAnexo> VeiculosAnexos
        {
            get
            {
                return _VeiculosAnexos;
            }

            set
            {
                if (_VeiculosAnexos != value)
                {
                    _VeiculosAnexos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseVeiculosAnexos.VeiculoAnexo VeiculoAnexoSelecionado
        {
            get
            {
                return _VeiculoAnexoSelecionado;
            }
            set
            {
                _VeiculoAnexoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (VeiculoAnexoSelecionado != null)
                {
                    //OnEmpresaSelecionada();
                }

            }
        }

        public int VeiculoAnexoSelecionadoID
        {
            get
            {
                return _VeiculoAnexoSelecionadoID;
            }
            set
            {
                _VeiculoAnexoSelecionadoID = value;
                base.OnPropertyChanged();
                if (VeiculoAnexoSelecionadoID != null)
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
        public void OnAtualizaCommand(object _VeiculoAnexoID)
        {

            VeiculoAnexoSelecionadoID = Convert.ToInt32(_VeiculoAnexoID);
            Thread CarregaColecaoVeiculosAnexos_thr = new Thread(() => CarregaColecaoVeiculosAnexos(Convert.ToInt32(_VeiculoAnexoID)));
            CarregaColecaoVeiculosAnexos_thr.Start();
            //CarregaColecaoVeiculorAnexos(Convert.ToInt32(_VeiculoAnexoID));

        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _VeiculoAnexoTemp.NomeArquivo = arq.Nome;
                _VeiculoAnexoTemp.Arquivo = arq.FormatoBase64;
                if (VeiculosAnexos != null)
                    VeiculosAnexos[0].NomeArquivo = arq.Nome;

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
                var arquivoStr = VeiculoAnexoSelecionado.Arquivo;
                var nomeArquivo = VeiculoAnexoSelecionado.NomeArquivo;
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
                _VeiculoAnexoTemp = VeiculoAnexoSelecionado.CriaCopia(VeiculoAnexoSelecionado);
                _selectedIndexTemp = SelectedIndex;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                VeiculosAnexos[_selectedIndexTemp] = _VeiculoAnexoTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;

                var entity = Mapper.Map<VeiculoAnexo>(VeiculoAnexoSelecionado);
                var repositorio = new VeiculoAnexoService();
                repositorio.Alterar(entity);

                var id = entity.VeiculoId;

                _VeiculoAnexoTemp = null;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;

                var entity = Mapper.Map<VeiculoAnexo>(VeiculoAnexoSelecionado);
                var repositorio = new VeiculoAnexoService();

                repositorio.Criar(entity);

                Thread CarregaColecaoAnexos_thr = new Thread(() => CarregaColecaoVeiculosAnexos(VeiculoAnexoSelecionado.VeiculoID));
                CarregaColecaoAnexos_thr.Start();

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in VeiculosAnexos)
                {
                    _VeiculosAnexosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                VeiculosAnexos.Clear();
                _VeiculoAnexoTemp = new ClasseVeiculosAnexos.VeiculoAnexo();
                _VeiculoAnexoTemp.VeiculoID = VeiculoAnexoSelecionadoID;
                VeiculosAnexos.Add(_VeiculoAnexoTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                VeiculosAnexos = null;
                VeiculosAnexos = new ObservableCollection<ClasseVeiculosAnexos.VeiculoAnexo>(_VeiculosAnexosTemp);
                SelectedIndex = _selectedIndexTemp;
                _VeiculosAnexosTemp.Clear();
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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

                        var entity = Mapper.Map<VeiculoAnexo>(VeiculoAnexoSelecionado);
                        var repositorio = new VeiculoAnexoService();
                        repositorio.Remover(entity);

                        VeiculosAnexos.Remove(VeiculoAnexoSelecionado);
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }
        public void OnPesquisarCommand()
        {
            try
            {
                popupPesquisaVeiculoAnexo = new PopupPesquisaVeiculoAnexo();
                popupPesquisaVeiculoAnexo.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaVeiculoAnexo.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaVeiculoAnexo.Criterio.Split((char)(20));
            int _VeiculoID = VeiculoAnexoSelecionadoID;
            string _id = ((string[])vetor)[0];
            string _numeroapolice = ((string[])vetor)[1];
            CarregaColecaoVeiculosAnexos(_VeiculoID);
            SelectedIndex = 0;
        }

        #endregion


        #region Carregamento das Colecoes

        public void CarregaColecaoVeiculosAnexos(int _veiculoID)
        {
            try
            {
                var service = new VeiculoAnexoService();

                var list1 = service.Listar(_veiculoID, null, null);

                var list2 = Mapper.Map<List<ClasseVeiculosAnexos.VeiculoAnexo>>(list1);
                var observer = new ObservableCollection<ClasseVeiculosAnexos.VeiculoAnexo>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                VeiculosAnexos = observer;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion


        #region Data Access
        private string RequisitaVeiculosAnexos(string _veiculoID, string _descricao = "", string _curso = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseVeiculosAnexos = _xmlDocument.CreateElement("ClasseVeiculosAnexos");
                _xmlDocument.AppendChild(_ClasseVeiculosAnexos);

                XmlNode _VeiculosAnexos = _xmlDocument.CreateElement("VeiculosAnexos");
                _ClasseVeiculosAnexos.AppendChild(_VeiculosAnexos);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _curso = "%" + _curso + "%";
                _descricao = "%" + _descricao + "%";
                _strSql = "select * from VeiculosAnexos where VeiculoID = " + _veiculoID + " and NomeArquivo Like '" + _descricao + "' order by VeiculoAnexoID desc"; // and NumeroApolice Like '" + _numeroapolice + "'
                //_strSql = "select * from VeiculosAnexos where VeiculoID = " + _VeiculoID + "";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _VeiculoAnexo = _xmlDocument.CreateElement("VeiculoAnexo");
                    _VeiculosAnexos.AppendChild(_VeiculoAnexo);

                    XmlNode _VeiculoAnexoID = _xmlDocument.CreateElement("VeiculoAnexoID");
                    _VeiculoAnexoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoAnexoID"].ToString())));
                    _VeiculoAnexo.AppendChild(_VeiculoAnexoID);

                    XmlNode _VeiculoID = _xmlDocument.CreateElement("VeiculoID");
                    _VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoID"].ToString())));
                    _VeiculoAnexo.AppendChild(_VeiculoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString()).Trim()));
                    _VeiculoAnexo.AppendChild(_Descricao);

                    XmlNode _NomeArquivo = _xmlDocument.CreateElement("NomeArquivo");
                    _NomeArquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivo"].ToString()).Trim()));
                    _VeiculoAnexo.AppendChild(_NomeArquivo);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Arquivo"].ToString())));
                    _VeiculoAnexo.AppendChild(_Arquivo);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch (Exception)
            {

                return null;
            }
        }

        private void InsereVeiculoAnexoBD(string xmlString)
        {
            try
            {
                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.LoadXml(xmlString);

                ClasseVeiculosAnexos.VeiculoAnexo _VeiculoAnexo = new ClasseVeiculosAnexos.VeiculoAnexo();

                int i = 0;

                _VeiculoAnexo.VeiculoAnexoID = _xmlDoc.GetElementsByTagName("VeiculoAnexoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoAnexoID")[i].InnerText);
                _VeiculoAnexo.VeiculoID = _xmlDoc.GetElementsByTagName("VeiculoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoID")[i].InnerText);
                _VeiculoAnexo.NomeArquivo = _xmlDoc.GetElementsByTagName("NomeArquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText;
                _VeiculoAnexo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;

                _VeiculoAnexo.Arquivo = _VeiculoAnexoTemp.Arquivo == null ? "" : _VeiculoAnexoTemp.Arquivo;


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_VeiculoAnexo.VeiculoAnexoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update VeiculosAnexos Set " +
                        "Descricao= '" + _VeiculoAnexo.Descricao + "'" +
                        ",NomeArquivo= '" + _VeiculoAnexo.NomeArquivo + "'" +
                        ",Arquivo= '" + _VeiculoAnexo.Arquivo + "'" +
                        " Where VeiculoAnexoID = " + _VeiculoAnexo.VeiculoAnexoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into VeiculosAnexos (VeiculoID,Descricao,NomeArquivo ,Arquivo) values (" +
                                                          _VeiculoAnexo.VeiculoID + ",'" + _VeiculoAnexo.Descricao + "','" +
                                                          _VeiculoAnexo.NomeArquivo + "','" + _VeiculoAnexo.Arquivo + "')", _Con);
                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereVeiculoAnexoBD ex: " + ex);


            }
        }

        private void ExcluiVeiculoAnexoBD(int _VeiculoAnexoID) // alterar para xml
        {
            try
            {


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from VeiculosAnexos where VeiculoAnexoID=" + _VeiculoAnexoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiVeiculoAnexoBD ex: " + ex);


            }
        }
        #endregion


        #region Metodos privados
        private string CriaXmlImagem(int VeiculoAnexoID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From VeiculosAnexos Where  VeiculoAnexoID = " + VeiculoAnexoID + "", _Con);
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
