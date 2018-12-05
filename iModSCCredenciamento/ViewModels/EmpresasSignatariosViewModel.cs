using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

using System.Xml;
using System.Xml.Serialization;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using AutoMapper;
using IMOD.Domain.Entities;

namespace iModSCCredenciamento.ViewModels
{

    public class EmpresasSignatariosViewModel : ViewModelBase
    {
        public EmpresasSignatariosViewModel()
        {
            //CarregaColecaoEmpresasSignatarios(1);
        }

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario> _Signatarios;

        private ClasseEmpresasSignatarios.EmpresaSignatario _SignatarioSelecionado;

        private ClasseEmpresasSignatarios.EmpresaSignatario _signatarioTemp = new ClasseEmpresasSignatarios.EmpresaSignatario();

        private List<ClasseEmpresasSignatarios.EmpresaSignatario> _SignatarioTemp = new List<ClasseEmpresasSignatarios.EmpresaSignatario>();

        //private ObservableCollection<ClasseEstados.Estado> _Estados;

        //private ObservableCollection<ClasseMunicipios.Municipio> _Municipios;

        //private ObservableCollection<ClasseStatus.Status> _Statuss;

        //private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _TiposAcessos;

        //private ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _TiposCobrancas;

        PopupPesquisaContrato PopupPesquisaSignatarios;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario> Signatarios
        {
            get
            {
                return _Signatarios;
            }

            set
            {
                if (_Signatarios != value)
                {
                    _Signatarios = value;
                    OnPropertyChanged();

                }
            }
        }

        //public ObservableCollection<ClasseEstados.Estado> Estados
        //{
        //    get
        //    {
        //        return _Estados;
        //    }

        //    set
        //    {
        //        if (_Estados != value)
        //        {
        //            _Estados = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public ObservableCollection<ClasseMunicipios.Municipio> Municipios
        //{
        //    get
        //    {
        //        return _Municipios;
        //    }

        //    set
        //    {
        //        if (_Municipios != value)
        //        {
        //            _Municipios = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public ObservableCollection<ClasseStatus.Status> Statuss
        //{
        //    get
        //    {
        //        return _Statuss;
        //    }

        //    set
        //    {
        //        if (_Statuss != value)
        //        {
        //            _Statuss = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public ObservableCollection<ClasseTiposAcessos.TipoAcesso> TiposAcessos
        //{
        //    get
        //    {
        //        return _TiposAcessos;
        //    }

        //    set
        //    {
        //        if (_TiposAcessos != value)
        //        {
        //            _TiposAcessos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public ObservableCollection<ClasseTiposCobrancas.TipoCobranca> TiposCobrancas
        //{
        //    get
        //    {
        //        return _TiposCobrancas;
        //    }

        //    set
        //    {
        //        if (_TiposCobrancas != value)
        //        {
        //            _TiposCobrancas = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        public ClasseEmpresasSignatarios.EmpresaSignatario SignatarioSelecionado
        {
            get
            {
                return this._SignatarioSelecionado;
            }
            set
            {
                this._SignatarioSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (SignatarioSelecionado != null)
                {
                    OnSignatarioSelecionado();
                }

            }
        }

        private void OnSignatarioSelecionado()
        {
            try
            {



            }
            catch (Exception ex)
            {
                //Global.Log("Erro void OnEmpresaSelecionada ex: " + ex.Message);
            }
        }

        public int EmpresaSelecionadaID
        {
            get
            {
                return this._EmpresaSelecionadaID;
            }
            set
            {
                this._EmpresaSelecionadaID = value;
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
        public void OnAtualizaCommand(object empresaID)
        {
            EmpresaSelecionadaID = Convert.ToInt32(empresaID);
            Thread CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresasSignatarios(Convert.ToInt32(empresaID)));
            CarregaColecaoEmpresasSignatarios_thr.Start();
        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog _arquivoPDF = new System.Windows.Forms.OpenFileDialog();
                string _sql;
                string _nomecompletodoarquivo;
                string _arquivoSTR;
                _arquivoPDF.InitialDirectory = "c:\\\\";
                _arquivoPDF.Filter = "(*.pdf)|*.pdf|All Files (*.*)|*.*";
                _arquivoPDF.RestoreDirectory = true;
                _arquivoPDF.ShowDialog();
                //if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
                //{
                _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
                _signatarioTemp.Assinatura = _arquivoSTR;


            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnBuscarArquivoCommand ex: " + ex.Message);
            }
        }

        public void OnAbrirArquivoCommand()
        {
            try
            {
                string _ArquivoPDF = null;
                if (_signatarioTemp != null)
                {
                    if (_signatarioTemp.Assinatura != null && _signatarioTemp.EmpresaSignatarioID == SignatarioSelecionado.EmpresaSignatarioID)
                    {
                        _ArquivoPDF = _signatarioTemp.Assinatura;

                    }
                }
                if (string.IsNullOrWhiteSpace(_ArquivoPDF))
                {
                    string _xmlstring = CriaXmlImagem(SignatarioSelecionado.EmpresaSignatarioID);

                    XmlDocument xmldocument = new XmlDocument();
                    xmldocument.LoadXml(_xmlstring);
                    XmlNode node = (XmlNode)xmldocument.DocumentElement;
                    XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                    _ArquivoPDF = arquivoNode.FirstChild.Value;
                }

                Global.PopupPDF(_ArquivoPDF, false);


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
                _signatarioTemp = SignatarioSelecionado.CriaCopia(SignatarioSelecionado);
                _selectedIndexTemp = SelectedIndex;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnEditarCommand ex: " + ex.Message);
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                Signatarios[_selectedIndexTemp] = _signatarioTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnCancelarEdicaoCommand ex: " + ex.Message);
            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;

                var service = new IMOD.Application.Service.EmpresaSignatarioService();
                var entity = SignatarioSelecionado;
                var entityConv = Mapper.Map<EmpresaSignatario>(entity);

                service.Alterar(entityConv);

                Thread CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresasSignatarios(SignatarioSelecionado.EmpresaId, null));
                CarregaColecaoEmpresasSignatarios_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnSalvarEdicaoCommand ex: " + ex.Message);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Signatarios)
                {
                    _SignatarioTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Signatarios.Clear();

                _signatarioTemp = new ClasseEmpresasSignatarios.EmpresaSignatario();
                _signatarioTemp.EmpresaId = EmpresaSelecionadaID;
                Signatarios.Add(_signatarioTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnAdicionarCommand ex: " + ex.Message);
            }

        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;

                var service = new IMOD.Application.Service.EmpresaSignatarioService();
                var entity = SignatarioSelecionado;
                var entityConv = Mapper.Map<EmpresaSignatario>(entity);

                service.Criar(entityConv);

                Thread CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresasSignatarios(SignatarioSelecionado.EmpresaId));
                CarregaColecaoEmpresasSignatarios_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnSalvarAdicaoCommand ex: " + ex.Message);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Signatarios = null;
                Signatarios = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>(_SignatarioTemp);
                SelectedIndex = _selectedIndexTemp;
                _SignatarioTemp.Clear();
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
                        var service = new IMOD.Application.Service.EmpresaSignatarioService();
                        var emp = service.BuscarPelaChave(SignatarioSelecionado.EmpresaSignatarioID);
                        service.Remover(emp);

                        Signatarios.Remove(SignatarioSelecionado);
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
                PopupPesquisaSignatarios popupPesquisaSegnatarios = new PopupPesquisaSignatarios();
                popupPesquisaSegnatarios.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaSegnatarios.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            int _empresaID = EmpresaSelecionadaID;
            string _nome = ((iModSCCredenciamento.Windows.PopupPesquisaSignatarios)sender).Criterio.Trim();
            CarregaColecaoEmpresasSignatarios(_empresaID, _nome);
            //CarregaColecaoContratos(_empresaID, _descricao, _numerocontrato);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoEmpresasSignatarios(int? empresaID = null, string nome = null)
        {
            try
            {
                var service = new IMOD.Application.Service.EmpresaSignatarioService();
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";

                var list1 = service.Listar(empresaID, nome, null, null, null, null);
                var list2 = Mapper.Map<List<ClasseEmpresasSignatarios.EmpresaSignatario>>(list1);

                var observer = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Signatarios = observer;
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void CarregaColecaoEmpresasSignatarios ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access


        #endregion

        #region Métodos Públicos

        Global g = new Global();

        #endregion

        #region Metodos privados
        private string CriaXmlImagem(int empresaSignatarioID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From EmpresasSignatarios Where EmpresaSignatarioID = " + empresaSignatarioID + "", _Con);
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
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Assinatura"].ToString())));
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
