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
    public class EmpresasAnexosViewModel : ViewModelBase
    {

        #region Inicializacao

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<EmpresaAnexoView> _Anexos;

        private EmpresaAnexoView _AnexoSelecionado;

        private EmpresaAnexoView _anexoTemp = new EmpresaAnexoView();

        private List<EmpresaAnexoView> _AnexosTemp = new List<EmpresaAnexoView>();

        PopupPesquisaEmpresasAnexos popupPesquisaEmpresasAnexos;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        private readonly IEmpresaAnexoService _service = new EmpresaAnexoService();

        #endregion

        #region Contrutores
        public ObservableCollection<EmpresaAnexoView> Anexos
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

        public EmpresaAnexoView AnexoSelecionado
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
                var arquivoStr = AnexoSelecionado.Anexo;
                var nomeArquivo = AnexoSelecionado.NomeAnexo;
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
               //_anexoTemp = AnexoSelecionado.CriaCopia(AnexoSelecionado);
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
                Anexos[_selectedIndexTemp] = _anexoTemp;
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

                var entity = AnexoSelecionado;
                var entityConv = Mapper.Map<EmpresaAnexo>(entity);

                _service.Alterar(entityConv);

                Thread CarregaColecaoAnexosSignatarios_thr = new Thread(() => CarregaColecaoAnexos(AnexoSelecionado.EmpresaId));
                CarregaColecaoAnexosSignatarios_thr.Start();

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
                foreach (var x in Anexos)
                {
                    _AnexosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Anexos.Clear();

                _anexoTemp = new EmpresaAnexoView();
                _anexoTemp.EmpresaId = EmpresaSelecionadaID;
                Anexos.Add(_anexoTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
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

                ObservableCollection<EmpresaAnexoView> _EmpresasAnexosPro = new ObservableCollection<EmpresaAnexoView>();
                EmpresaAnexoView _ClasseEmpresasAnexosTemp = new EmpresaAnexoView();

                _EmpresasAnexosPro.Add(AnexoSelecionado);
                //_ClasseEmpresasAnexosTemp.EmpresasAnexos = _EmpresasAnexosPro;

                var entity = AnexoSelecionado;
                var entityConv = Mapper.Map<EmpresaAnexo>(entity);

                _service.Criar(entityConv);

                Thread CarregaColecaoAnexosSignatarios_thr = new Thread(() => CarregaColecaoAnexos(AnexoSelecionado.EmpresaId));
                CarregaColecaoAnexosSignatarios_thr.Start();

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }
        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Anexos = null;
                Anexos = new ObservableCollection<EmpresaAnexoView>(_AnexosTemp);
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
                        var emp = _service.BuscarPelaChave(AnexoSelecionado.EmpresaAnexoId);
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
                Utils.TraceException(ex);
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
                var list2 = Mapper.Map<List<EmpresaAnexoView>>(list1);

                var observer = new ObservableCollection<EmpresaAnexoView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Anexos = observer;

                //Hotfix auto-selecionar registro no topo da ListView
                var topList = observer.FirstOrDefault();
                AnexoSelecionado = topList;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        #endregion

        #region Data Access

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
