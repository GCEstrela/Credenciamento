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
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml;

namespace iModSCCredenciamento.ViewModels
{
    public class TermosViewModel : ViewModelBase
    {

        #region Variaveis Privadas

        //private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;

        //private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;


        private ObservableCollection<ClasseRelatorios.Relatorio> _Relatorios;

        private ClasseRelatorios.Relatorio _RelatorioSelecionado;

        private ClasseRelatorios.Relatorio _relatorioTemp = new ClasseRelatorios.Relatorio();

        private List<ClasseRelatorios.Relatorio> _RelatoriosTemp = new List<ClasseRelatorios.Relatorio>();

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;

        private string formula;
        private string _mensagem;
        private string verbo;

        #endregion

        #region Contrutores

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
                if (RelatorioSelecionado != null)
                {
                    //    //BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                    //    //string _imgstr = Conversores.IMGtoSTR(_img);
                    //    //ColaboradorSelecionado.Foto = _imgstr;
                    //    if (!_atualizandoFoto)
                    //    {
                    //        Thread CarregaRelatorio_thr = new Thread(() => CarregaRelatorio(RelatorioSelecionado.RelatorioID));
                    //        CarregaRelatorio_thr.Start();
                    //    }

                    //    //CarregaFoto(ColaboradorSelecionado.ColaboradorID);
                }

            }
        }

        #endregion

        #region Carregamento das Colecoes


        #endregion

        #region Data Access


        #endregion

        #region Comandos dos Botoes 

        public void OnFiltrosTermosCommand(int _report, int _status, int _periodo, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            switch (_report)
            {
                case 12:
                    verbo = "concedeu";
                    break;
                case 16:
                    verbo = "indeferiu";
                    break;
                case 14:
                    verbo = "cancelou";
                    break;
                case 18:
                    verbo = "destruiu";
                    break;
                default:
                    verbo = "emitiu";
                    break;
            }

            try
            {
                _xmlstring = CriaXmlRelatoriosGerenciais(_report);

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;

                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();

                Tables CrTables;

                //Filtro Hoje
                if (_periodo == 1)
                {
                    //Termos com status Vias Adicionais (1)
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {ColaboradoresCredenciais.Emissao} = CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Na data " + DateTime.Now.ToShortDateString() + " " +
                                    "este setor de credenciamento do AEROPORTO INTERNACIONAL " +
                                    "DE PORTO ALEGRE, " + verbo + " as seguintes vias adicionais de credenciais:";
                    }

                    //Outros status (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = "{ColaboradoresCredenciais.Emissao} = CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {ColaboradoresCredenciais.Baixa} = CurrentDate " +
                                      " and {ColaboradoresCredenciais.CredencialStatusID} = " + _status;
                        }


                        _mensagem = "Na data " + DateTime.Now.ToShortDateString() + " " +
                                    "este setor de credenciamento do AEROPORTO INTERNACIONAL " +
                                    "DE PORTO ALEGRE, " + verbo + " as seguintes credenciais:";
                    }

                    reportDocument.Load(_ArquivoRPT);
                }
                //Filtro últimos 7 dias
                else if (_periodo == 2)
                {
                    //Termo de Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {ColaboradoresCredenciais.Emissao} >= CurrentDate-7 " +
                                  " and {ColaboradoresCredenciais.Emissao} <= CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-7).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo + " as seguintes vias adicionais de credenciais:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {ColaboradoresCredenciais.Emissao} >= CurrentDate-7" +
                                      " and {ColaboradoresCredenciais.Emissao} <= CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {ColaboradoresCredenciais.Baixa} >= CurrentDate-7" +
                                      " and {ColaboradoresCredenciais.Baixa} <= CurrentDate " +
                                      " and {ColaboradoresCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-7).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo + " as seguintes vias adicionais de credenciais:";
                    }

                    reportDocument.Load(_ArquivoRPT);
                }
                //Filtro Último Mês
                else if (_periodo == 3)
                {
                    //Termo Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = " {ColaboradoresCredenciais.Emissao} >= CurrentDate-30 " +
                                  " and {ColaboradoresCredenciais.Emissao} <= CurrentDate " +
                                  " and {CredenciaisMotivos.CredencialmotivoID} in [2,3]";

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-30).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo + " as seguintes vias adicionais de credenciais:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {ColaboradoresCredenciais.Emissao} >= CurrentDate-30 " +
                                      " and {ColaboradoresCredenciais.Emissao} <= CurrentDate ";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {ColaboradoresCredenciais.Baixa} >= CurrentDate-30 " +
                                      " and {ColaboradoresCredenciais.Baixa} <= CurrentDate " +
                                      " and {ColaboradoresCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + DateTime.Now.AddDays(-30).ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo + " as seguintes vias adicionais de credenciais:";
                    }

                    reportDocument.Load(_ArquivoRPT);
                }
                //Filtro por Período Determinado (Entre datas)
                else
                {
                    //Termo Vias Adicionais
                    if (_report == 20 && _status == 1)
                    {
                        formula = "  {CredenciaisMotivos.CredencialmotivoID} in [2,3]" +
                                  " and {ColaboradoresCredenciais.Emissao} >= cdate('" + _dataIni + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataFim + "') ";


                        _mensagem = "Durante o período de " + _dataIni + " a " + _dataFim + " " +
                                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " +
                                    "" + verbo + " as seguintes vias adicionais de credenciais:";
                    }

                    //Outros termos (Concedidas/Indeferidas/Canceladas/Destruidas)
                    else
                    {
                        ///Concedidas
                        if (_status == 1)
                        {
                            formula = " {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataFim + "') " +
                                      " and {ColaboradoresCredenciais.Emissao} >= cdate('" + _dataIni + "')";
                        }
                        ///Indeferidas/Canceladas/Destruidas
                        else
                        {
                            formula = " {ColaboradoresCredenciais.Baixa} >= cdate('" + _dataIni + "')" +
                                     " and {ColaboradoresCredenciais.Baixa} <= cdate('" + _dataFim + "') " +
                                     " and {ColaboradoresCredenciais.CredencialStatusID} = " + _status;
                        }

                        _mensagem = "Durante o período de " + _dataIni + " a " + _dataFim + " " +
                                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " +
                                    "" + verbo + " as seguintes credenciais:";
                    }

                    reportDocument.Load(_ArquivoRPT);
                }

                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
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

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {

                        reportDocument.RecordSelectionFormula = formula;

                        TextObject txt = (TextObject)reportDocument.ReportDefinition.ReportObjects["TextoPrincipal"];
                        txt.Text = _mensagem;
                        //System.Drawing.Font fonte = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
                        //txt.ApplyFont(fonte);

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });
                }

                );
                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnFiltrosTermosCommand ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }


        #endregion


        #region Metodos privados

        private string CriaXmlRelatoriosGerenciais(int relatorioID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseRelatoriosGerenciais = _xmlDocument.CreateElement("ClasseRelatoriosGerenciais");
                _xmlDocument.AppendChild(_ClasseRelatoriosGerenciais);

                XmlNode _RelatoriosGerenciais = _xmlDocument.CreateElement("RelatoriosGerenciais");
                _ClasseRelatoriosGerenciais.AppendChild(_RelatoriosGerenciais);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand SQCMDXML = new SqlCommand("Select * From RelatoriosGerenciais Where RelatorioID = " + relatorioID, _Con);
                SqlDataReader SQDR_XML;

                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoRelatorio");
                    _RelatoriosGerenciais.AppendChild(_ArquivoImagem);
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
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }
        #endregion


    }
}
