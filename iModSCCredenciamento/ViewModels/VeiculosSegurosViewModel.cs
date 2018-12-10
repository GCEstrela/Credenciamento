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
    class VeiculosSegurosViewModel : ViewModelBase
    {
        #region Inicializacao

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro> _Seguros;

        private ClasseVeiculosSeguros.VeiculoSeguro _SeguroSelecionado;

        private ClasseVeiculosSeguros.VeiculoSeguro _seguroTemp = new ClasseVeiculosSeguros.VeiculoSeguro();

        private List<ClasseVeiculosSeguros.VeiculoSeguro> _SegurosTemp = new List<ClasseVeiculosSeguros.VeiculoSeguro>();

        PopupPesquisaSeguro popupPesquisaSeguro;

        private int _selectedIndex;

        private int _VeiculoSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro> Seguros
        {
            get
            {
                return _Seguros;
            }

            set
            {
                if (_Seguros != value)
                {
                    _Seguros = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseVeiculosSeguros.VeiculoSeguro SeguroSelecionado
        {
            get
            {
                return _SeguroSelecionado;
            }
            set
            {
                _SeguroSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (SeguroSelecionado != null)
                {
                    //OnVeiculoSelecionada();
                }

            }
        }

        public int VeiculoSelecionadaID
        {
            get
            {
                return _VeiculoSelecionadaID;
            }
            set
            {
                _VeiculoSelecionadaID = value;
                base.OnPropertyChanged();
                if (VeiculoSelecionadaID != null)
                {
                    //OnVeiculoSelecionada();
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
        public void OnAtualizaCommand(object veiculoID)
        {
            VeiculoSelecionadaID = Convert.ToInt32(veiculoID);
            Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(Convert.ToInt32(veiculoID)));
            CarregaColecaoSeguros_thr.Start();
        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _seguroTemp.NomeArquivo = arq.Nome;
                _seguroTemp.Arquivo = arq.FormatoBase64;
                if (Seguros != null)
                    Seguros[0].NomeArquivo = arq.Nome;

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
                var arquivoStr = SeguroSelecionado.Arquivo;
                var nomeArquivo = SeguroSelecionado.NomeArquivo;
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
                _seguroTemp = SeguroSelecionado.CriaCopia(SeguroSelecionado);
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
                Seguros[_selectedIndexTemp] = _seguroTemp;
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

                var entity = Mapper.Map<VeiculoSeguro>(SeguroSelecionado);
                var repositorio = new VeiculoSeguroService();
                repositorio.Alterar(entity);

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
                foreach (var x in Seguros)
                {
                    _SegurosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Seguros.Clear();

                _seguroTemp = new ClasseVeiculosSeguros.VeiculoSeguro();
                _seguroTemp.VeiculoID = VeiculoSelecionadaID;
                Seguros.Add(_seguroTemp);
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

                var entity = Mapper.Map<VeiculoSeguro>(SeguroSelecionado);
                var repositorio = new VeiculoSeguroService();
                repositorio.Criar(entity);

                Thread CarregaColecaoAnexos_thr = new Thread(() => CarregaColecaoSeguros(SeguroSelecionado.VeiculoID));
                CarregaColecaoAnexos_thr.Start();

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
                Seguros = null;
                Seguros = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>(_SegurosTemp);
                SelectedIndex = _selectedIndexTemp;
                _SegurosTemp.Clear();
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
                        var entity = Mapper.Map<VeiculoSeguro>(SeguroSelecionado);
                        var repositorio = new VeiculoSeguroService();
                        repositorio.Remover(entity);

                        Seguros.Remove(SeguroSelecionado);
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
                popupPesquisaSeguro = new PopupPesquisaSeguro();
                popupPesquisaSeguro.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaSeguro.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaSeguro.Criterio.Split((char)(20));
            int _veiculoID = VeiculoSelecionadaID;
            string _seguradora = ((string[])vetor)[0];
            string _numeroapolice = ((string[])vetor)[1];
            CarregaColecaoSeguros(_veiculoID, _seguradora, _numeroapolice);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoSeguros(int veiculoID, string _seguradora = "", string _numeroapolice = "")
        {
            try
            {
                var service = new VeiculoSeguroService();
                var list1 = service.Listar(veiculoID, null, null);

                var list2 = Mapper.Map<List<ClasseVeiculosSeguros.VeiculoSeguro>>(list1);
                var observer = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Seguros = observer;

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
        private string CriaXmlImagem(int veiculoSeguroID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From VeiculosSeguros Where VeiculoSeguroID = " + veiculoSeguroID + "", _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    //XmlNode _ArquivoImagemID = _xmlDocument.CreateElement("ArquivoImagemID");
                    //_ArquivoImagemID.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["VeiculoSeguroID"].ToString())));
                    //_ArquivoImagem.AppendChild(_ArquivoImagemID);

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
