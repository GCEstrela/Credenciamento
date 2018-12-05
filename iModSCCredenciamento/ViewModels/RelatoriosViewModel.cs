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
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;

namespace iModSCCredenciamento.ViewModels
{
    public class RelatoriosViewModel : ViewModelBase
    {
        #region Inicializacao
        public RelatoriosViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
        }

        private void CarregaUI()
        {
            CarregaColecaoRelatorios();
            CarregaColecaoEmpresas();
            CarregaColecaoAreasAcessos();
        }

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;


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

        private readonly IRelatorioService _service = new RelatorioService();
        private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        #endregion

        #region Contrutores

        public ObservableCollection<ClasseAreasAcessos.AreaAcesso> AreasAcessos
        {
            get
            {
                return _AreasAcessos;
            }

            set
            {
                if (_AreasAcessos != value)
                {
                    _AreasAcessos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseEmpresas.Empresa> Empresas

        {
            get
            {
                return _Empresas;
            }

            set
            {
                if (_Empresas != value)
                {
                    _Empresas = value;
                    OnPropertyChanged();

                }
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

        public BitmapImage Waiting
        {
            get
            {
                return this._Waiting;
            }
            set
            {
                this._Waiting = value;
                base.OnPropertyChanged();
            }
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoRelatorios()
        {
            try
            {
                var list1 = _service.Listar();

                var list2 = Mapper.Map<List<ClasseRelatorios.Relatorio>>(list1);
                var observer = new ObservableCollection<ClasseRelatorios.Relatorio>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Relatorios = observer;
            }

            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                var list1 = _auxiliaresService.AreaAcessoService.Listar();

                var list2 = Mapper.Map<List<ClasseAreasAcessos.AreaAcesso>>(list1);
                var observer = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.AreasAcessos = observer;
            }
            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        private void CarregaColecaoEmpresas(int? idEmpresa = null, string nome = null, string apelido = null, string cnpj = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(cnpj)) cnpj = $"%{cnpj}%";

                var list1 = _empresaService.Listar(idEmpresa, nome, apelido, cnpj);
                var list2 = Mapper.Map<List<ClasseEmpresas.Empresa>>(list1.OrderByDescending(a => a.EmpresaId));

                var observer = new ObservableCollection<ClasseEmpresas.Empresa>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Empresas = observer;
                SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        #endregion

        #region Data Access (Vazia)


        #endregion

        #region Comandos dos Botoes

        public void OnAbrirRelatorioCommand(string _Tag)
        {
            try
            {
                string _xmlstring = CriaXmlRelatorio(Convert.ToInt32(_Tag));

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

                reportDocument.Load(_ArquivoRPT);
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
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                });

                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);
            }

        }

        #region Comandos dos Botoes (RELATÓRIOS GERENCIAIS)

        public void OnFiltroRelatorioCredencialCommand(bool _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                string _xmlstring;

                if (_tipo)
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(1);
                }
                else
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(5);
                }



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

                if (_tipo)
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS CREDENCIAIS PERMANENTES E ATIVAS
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 and {CredenciaisStatus.CredencialStatusID} = 1 ";
                    }
                    else
                    {
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }
                else
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS CREDENCIAIS TEMPORÁRIAS E ATIVAS
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 and {CredenciaisStatus.CredencialStatusID} = 1 ";
                    }
                    else
                    {
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }

                reportDocument.Load(_ArquivoRPT);

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

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });
                });

                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnFiltroRelatorioAutorizacoesCommand(bool _check)
        {
            try
            {
                string _xmlstring = CriaXmlRelatoriosGerenciais(2);

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

                if (_check)
                {   //AUTORIZAÇÕES PERMANENTES E ATIVAS
                    formula = " {TiposCredenciais.TipoCredencialID} = 1 AND {CredenciaisStatus.CredencialStatusID} = 1 ";
                }
                else
                {
                    //AUTORIZAÇÕES TEMPORÁRIAS E ATIVAS
                    formula = " {TiposCredenciais.TipoCredencialID} = 2 AND {CredenciaisStatus.CredencialStatusID} = 1 ";
                }

                reportDocument.Load(_ArquivoRPT);
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
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });
                });

                CarregaRel_thr.Start();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnFiltroRelatorioAutorizacoesCommand ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnRelatorioCredenciaisInvalidasFiltroCommand(int _status, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            try
            {
                _xmlstring = CriaXmlRelatoriosGerenciais(3);

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

                //DATA VAZIA (TODOS)
                if ((_dataFim == "" || _dataIni == "") && _status == 0)

                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 ";
                }
                //CAMPOS VAZIOS (TODOS)
                else if ((_dataFim == "" && _dataIni == "") && _status != 0)
                {
                    if (_status == 2)
                    {
                        //Credenciais Roubadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  "AND {CredenciaisMotivos.CredencialmotivoID} = 10";
                    }
                    else if (_status == 1)
                    {
                        //Credenciais Extraviadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  "AND {CredenciaisMotivos.CredencialmotivoID} = 9";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status + "";
                    }

                }
                else
                {
                    if (_status == 0)
                    {
                        formula = " ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }

                    else if (_status == 2)
                    {
                        //Credenciais Roubadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 10" +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else if (_status == 1)
                    {
                        //Credenciais Extraviadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 9" +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                }

                reportDocument.Load(_ArquivoRPT);
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
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });
                });

                CarregaRel_thr.Start();

            }

            catch (Exception ex)
            {
                Global.Log("Erro na void OnRelatorioCredenciaisInvalidasFiltroCommand ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;

            }
        }

        public void OnRelatorioAutorizacoesInvalidasFiltroCommand(int _check, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            try
            {    //4_Relatório_AutorizacoesInvalidas.rpt
                _xmlstring = CriaXmlRelatoriosGerenciais(4);

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
                //Esse ponto de implementação para a alteração da instancia do SQL, banco, usuário e senha
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                //DATA VAZIA (TODOS)
                if ((_dataFim == "" || _dataIni == "") && _check == 10)

                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 ";
                }
                //CAMPOS VAZIOS (TODOS)
                else if ((_dataFim == "" && _dataIni == "") && _check != 10)
                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                    " AND {CredenciaisStatus.CredencialStatusID} = " + _check + "";
                }
                else
                {
                    if (_check == 10)
                    {
                        formula = " ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _check +
                                  " AND ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                }


                reportDocument.Load(_ArquivoRPT);
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

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                }

                );
                //CarregaRel_thr.SetApartmentState(ApartmentState.STA);
                CarregaRel_thr.Start();

            }

            catch (Exception ex)
            {
                Global.Log("Erro na void OnRelatorioAutorizacoesInvalidasFiltroCommand ex: " + ex);

            }
        }

        public void OnRelatorioFiltroPorAreaCommand(string _area, bool _check)
        {
            string _xmlstring;

            try
            {
                //6_Relatorio_CredenciaisPorArea.rpt
                if (_check)
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(6);
                }
                //7_Relatorio_AutorizacoesPorArea.rpt
                else
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(7);
                }

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
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

                        if (_area != "")
                        {
                            reportDocument.RecordSelectionFormula = " {AreasAcessos_0.AreaAcessoID} = " + _area;
                        }

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                }

                );
                //CarregaRel_thr.SetApartmentState(ApartmentState.STA);
                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnRelatorioFiltroPorAreaCommand ex: " + ex);

            }
        }

        public void OnRelatorioFiltroPorEmpresaCommand(string empresa, bool _check, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            try
            {
                //8_Relatorio_CredenciaisNoPeriodoPorEntidade.rpt
                if (_check)
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(8);
                }
                //9_Relatorio_AutorizacoesNoPeriodoPorEntidade.rpt
                else
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(9);
                }

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

                //Todas Empresas
                if (empresa == "" && _dataIni == "" && _dataFim == "")
                {
                    formula = "";
                    reportDocument.Load(_ArquivoRPT);
                }

                //Uma Empresa
                else if (_dataIni == "" && _dataFim == "" && _check)
                {
                    formula = " {Empresas.EmpresaID} = " + empresa;
                    reportDocument.Load(_ArquivoRPT);
                }

                //Credenciais
                else if (_check)
                {
                    formula = "{ColaboradoresEmpresas.Emissao}  <= '" + _dataFim + "' " +
                              " and {ColaboradoresEmpresas.Emissao} >= '" + _dataIni +
                              "' and {Empresas.EmpresaID} = " + empresa + "";

                    reportDocument.Load(_ArquivoRPT);
                }

                //Autorizacoes
                else
                {
                    formula = "{EmpresasVeiculos.Emissao}  <= '" + _dataFim + "' " +
                          " and {EmpresasVeiculos.Emissao} >= '" + _dataIni +
                          "' and {Empresas.EmpresaID} = " + empresa + "";

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

                        if (empresa != "")
                        {
                            reportDocument.RecordSelectionFormula = formula;
                        }

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                });
                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnRelatorioFiltroPorEmpresaCommand ex: " + ex);

            }
        }

        public void OnFiltrosImpressoesCommand(string _empresa, string _area, bool _check, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            try
            {
                //10_Relatório_ImpressoesCredenciais.rpt
                if (_check)
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(10);
                }
                //11_Relatório_ImpressoesAutorizacoes.rpt
                else
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(11);
                }

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

                //Tudo
                if (_empresa == "" || _dataIni == "" || _dataFim == "" || _area == "")
                {
                    formula = "";
                    reportDocument.Load(_ArquivoRPT);
                }

                //Autorizacoes
                else
                {
                    formula = " 1=1 and {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataFim + "') " +
                          " and {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataIni + "')" +
                          " and  {EmpresasAreasAcessos.AreaAcessoID} = " + _area + "" +
                          " and {Empresas.EmpresaID} = " + _empresa + "";

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

                        if (_empresa != "")
                        {
                            reportDocument.RecordSelectionFormula = formula;
                        }

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                });

                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnFiltrosImpressoesCommand ex: " + ex);

            }
        }

        public void OnFiltroCredencialViasAdicionaisCommand(int _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                string _xmlstring;

                //21_Relatório_ViasAdicionaisCredenciais.rpt
                _xmlstring = CriaXmlRelatoriosGerenciais(21);


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

                if (_tipo == 0)
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS VIAS ADICIONAIS EMITIDAS
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3]";
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3] " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }
                else
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //VIAS ADICIONAIS EMITIDAS (1a,2a,3a VIA)
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo;
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }

                reportDocument.Load(_ArquivoRPT);

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

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                });

                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnFiltroRelatorioCredencialCommand ex: " + ex);
            }
        }

        #endregion


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

        private string CriaXmlRelatoriosGerenciais(int relatorioID)
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


                SqlCommand SQCMDXML = new SqlCommand("Select * From RelatoriosGerenciais Where RelatorioID = " + relatorioID, _Con);
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
        #endregion


    }
}
