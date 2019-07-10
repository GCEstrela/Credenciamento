﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;
using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class RelatoriosViewModel : ViewModelBase
    {
        #region Inicializacao
        public RelatoriosViewModel()
        {
           // Thread CarregaUI_thr = new Thread(() => CarregaUI());
           // CarregaUI_thr.Start();
        }

        private void CarregaUI()
        {
            //CarregaColecaoRelatorios(); 
            //CarregaColecaoEmpresas(); 
            //CarregaColecaoAreasAcessos(); 
        }

        #endregion

        #region Variaveis Privadas
        private ObservableCollection<CredencialMotivoView> _CredencialMotivo;

        private ObservableCollection<AreaAcessoView> _AreasAcessos;

        private ObservableCollection<EmpresaView> _Empresas;


        private ObservableCollection<RelatorioView> _Relatorios;

        private RelatorioView _RelatorioSelecionado;

        private RelatorioView _relatorioTemp = new RelatorioView();

        private List<RelatorioView> _RelatoriosTemp = new List<RelatorioView>();

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;

        private string formula;
        private string verbo;
        private string mensagem;

        //private const string Constantes.Constantes.consNomeArquivoEmpresaOperadora = "logoEmpresaOperadora.png";

        private readonly IRelatorioService _relatorioService = new RelatorioService();
        private readonly IRelatorioGerencialService _relatorioGerencialServiceService = new RelatorioGerencialService();
        private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        private Relatorios relatorio = new Relatorios();
        private RelatoriosGerenciais relatorioGerencial = new RelatoriosGerenciais();
        private readonly IColaboradorCredencialService objColaboradorCredencial = new ColaboradorCredencialService();
        private readonly IVeiculoCredencialService objVeiculoCredencial = new VeiculoCredencialService();
        private readonly IConfiguraSistemaService objConfiguraSistema = new ConfiguraSistemaService();
        

        #endregion

        #region Contrutores

        public ObservableCollection<AreaAcessoView> AreasAcessos
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
        public ObservableCollection<EmpresaView> Empresas

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
        public ObservableCollection<RelatorioView> Relatorios
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

        public RelatorioView RelatorioSelecionado
        {
            get
            {
                return _RelatorioSelecionado;
            }
            set
            {
                _RelatorioSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (RelatorioSelecionado != null)
                {

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
                return _Waiting;
            }
            set
            {
                _Waiting = value;
                base.OnPropertyChanged();
            }
        }

        public ObservableCollection<CredencialMotivoView> MotivosCredenciais
        {
            get
            {
                return _CredencialMotivo;
            }

            set
            {
                if (_CredencialMotivo != value)
                {
                    _CredencialMotivo = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Carregamento das Colecoes 

        public void CarregaColecaoRelatorios()
        {
            try
            {
                var list1 = _relatorioService.Listar();

                var list2 = Mapper.Map<List<RelatorioView>>(list1);
                var observer = new ObservableCollection<RelatorioView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Relatorios = observer;

                if (Relatorios.Count() <= 24)
                {
                    var quantidadeRestante = 24 - Relatorios.Count();
                    for (int count = 0; count < quantidadeRestante; count++)
                    {
                        Relatorios.Add(new RelatorioView());
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoAreasAcessos()
        {
            try
            {
                var list1 = _auxiliaresService.AreaAcessoService.Listar();

                var list2 = Mapper.Map<List<AreaAcessoView>>(list1);
                var observer = new ObservableCollection<AreaAcessoView>();

                list2.ForEach(n => 
                {
                    observer.Add(n); 
                });

                AreasAcessos = observer; 
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaColecaoEmpresas(int? idEmpresa = null, string nome = null, string apelido = null, string cnpj = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(cnpj)) cnpj = $"%{cnpj}%";

                var list1 = _empresaService.Listar(idEmpresa, nome, apelido, cnpj);
                var list2 = Mapper.Map<List<EmpresaView>>(list1.OrderByDescending(a => a.EmpresaId));

                var observer = new ObservableCollection<EmpresaView>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                Empresas = observer;
                SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private List<CredencialMotivoView> ListarMotivoCredenciais
        {
            get
            {
                List<CredencialMotivoView> lstMapeadaMotivo = new List<CredencialMotivoView>();

                    var lstCredencialMotivo = _auxiliaresService.CredencialMotivoService.Listar().ToList();

                if (lstCredencialMotivo != null)
                    {
                        lstMapeadaMotivo = Mapper.Map<List<CredencialMotivoView>>(lstCredencialMotivo);
                    }
                    return lstMapeadaMotivo;
                }
        }

        public void CarregaMotivoCredenciais(int statusId)
        {
            try
            {
                var lst1 = this.ListarMotivoCredenciais.Where(n => n.CredencialStatusID == statusId).ToList();

                var list2 = Mapper.Map<List<CredencialMotivoView>>(lst1);
                var observer = new ObservableCollection<CredencialMotivoView>();

                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                MotivosCredenciais = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregaMotivoCredenciaisViaAdicionais(List<int> listaStatusID)
        {
            try
            {
                var lst1 = this.ListarMotivoCredenciais.Where(n => listaStatusID.Contains(n.CredencialMotivoId)).ToList();

                var list2 = Mapper.Map<List<CredencialMotivoView>>(lst1);
                var observer = new ObservableCollection<CredencialMotivoView>();

                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                MotivosCredenciais = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #region Comandos dos Botoes

        public void OnAbrirRelatorioCommand(string _Tag)
        {
            try
            {
                relatorio = _relatorioService.BuscarPelaChave(Convert.ToInt32(_Tag));

                var arrayFile = Convert.FromBase64String(relatorio.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "Relatorio " + _Tag, formula, "");

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #region Comandos dos Botoes (RELATÓRIOS GERENCIAIS)


        #region CREDENCIAIS

        /// <summary>
        ///  Relatório de Credenciais Permanentes/Temporárias
        /// </summary>
        /// <param name="_tipo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltroRelatorioCredenciaisCommand(bool tipo, string dataIni, string dataFim)
        {
            string mensagem = string.Empty;
            string mensagemComplemento = string.Empty;
            string mensagemPeriodo = string.Empty;
            Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();

            try
            {
                colaboradorCredencial.CredencialStatusId = 1;
                colaboradorCredencial.Impressa = true;

                mensagem = "Todas as CREDENCIAIS ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " ativas concedidas entre " + dataIni + " e " + dataFim + "";
                }
               
                colaboradorCredencial.TipoCredencialId = tipo ? 1 : 2;
                mensagemComplemento = tipo ? " PERMANENTES " : " TEMPORÁRIAS ";


                var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                var result = objColaboradorCredencial.ListarColaboradorCredencialConcedidasView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                mensagem += mensagemComplemento + mensagemPeriodo;
                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                } 

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///  Relatório de Credenciais Inválidas
        /// (Indeferidas,Roubadas,Extraviadas,Não-Devolvidas...)
        /// </summary>
        /// <param name="_status"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioCredenciaisInvalidasFiltroCommand(bool tipo, int status, CredencialMotivoView credencialMotivoSelecionado, string dataIni, string dataFim, bool flaDevolucaoEntregaBO)
        {
            try 
            {
                string mensagem = string.Empty; 
                string mensagemComplemento = string.Empty;
                string mensagemComplementoTipo = string.Empty;
                string mensagemPeriodo = string.Empty; 
                
                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                mensagem = "Todas as CREDENCIAIS ";
                colaboradorCredencial.CredencialStatusId = 2;
                colaboradorCredencial.Impressa = true;
                colaboradorCredencial.DevolucaoEntregaBo = flaDevolucaoEntregaBO;

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.DataStatus = DateTime.Parse(dataIni);
                    colaboradorCredencial.DataStatusFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }

                colaboradorCredencial.TipoCredencialId = tipo ? 1 : 2;
                mensagemComplementoTipo = tipo ? " PERMANENTES " : " TEMPORÁRIAS ";

                if (status == 0)
                {
                    mensagemComplemento = "INVÁLIDAS ";
                }
                else
                {
                    colaboradorCredencial.CredencialMotivoId = credencialMotivoSelecionado.CredencialMotivoId;
                    mensagemComplemento = credencialMotivoSelecionado.Descricao.ToString();
                }
                mensagem += mensagemComplementoTipo + mensagemComplemento + mensagemPeriodo;

                var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(5);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                var result = objColaboradorCredencial.ListarColaboradorCredencialInvalidasView(colaboradorCredencial).Where(n => n.CredencialStatusId == 2);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary> 
        ///  Relatório de Credenciais por Área de Acesso
        /// </summary> 
        /// <param name="_area"></param>
        /// <param name="_check"></param>
        public void OnRelatorioCredencialPorAreaCommand(bool tipo, string area, bool _check, AreaAcessoView objAreaSelecionado)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplementoArea = string.Empty; 
                string mensagemComplemento = string.Empty; 
                string mensagemPeriodo = string.Empty; 
                
                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                colaboradorCredencial.CredencialStatusId = 1;
                colaboradorCredencial.Impressa = true;


                if (objAreaSelecionado.AreaAcessoId > 0)
                {
                    mensagemComplementoArea = " - " + objAreaSelecionado.Identificacao + " / " + objAreaSelecionado.Descricao;
                }

                colaboradorCredencial.TipoCredencialId = tipo ? 1 : 2; 
                mensagemComplemento = tipo ? " PERMANENTES " : " TEMPORÁRIAS "; 

                mensagem = " Todas as CREDENCIAIS " + mensagemComplemento + " E VÁLIDAS por ÁREA DE ACESSO " + mensagemComplementoArea;

                if (area != string.Empty)
                { 
                    colaboradorCredencial.AreaAcessoId = Convert.ToInt16(area);
                }
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(7);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                var result = objColaboradorCredencial.ListarColaboradorCredencialPermanentePorAreaView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Relatório de Credenciais por Empresas
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="_check"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioFiltroCredencialPorEmpresaCommand(bool tipo, string empresa, bool check, string dataIni, string dataFim)
        {
            try 
            {
                string mensagem = string.Empty; 
                string mensagemPeriodo = string.Empty;
                string mensagemComplemento = string.Empty;
                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                
                colaboradorCredencial.Impressa = true;

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " no período de  " + dataIni + " e " + dataFim + "";
                }
                mensagem += mensagemPeriodo;

                colaboradorCredencial.TipoCredencialId = tipo ? 1 : 2;
                mensagemComplemento = tipo ? " PERMANENTES " : " TEMPORÁRIAS ";

                mensagem = "Todas as CREDENCIAIS "  + mensagemComplemento + " emitidas por entidade solicitante"; 

                if (!string.IsNullOrEmpty(empresa))
                {
                    colaboradorCredencial.EmpresaId = Convert.ToInt16(empresa);
                }

                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(9);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                //Faz a busca do registros de colaboradores credenciais concedidas
                var result = objColaboradorCredencial.ListarColaboradorCredencialConcedidasView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());
                
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt); 
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome); 
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);


            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Relatório de impressões de Credenciais e Autorizações por Empresas
        /// </summary>
        /// <param name="_empresa"></param>
        /// <param name="_check"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltrosColaboradorCredencialImpressoesCommand(bool tipo,string empresa, bool check, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty; 
                string mensagemPeriodo = string.Empty; 
                string mensagemComplemento = string.Empty; 

                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                colaboradorCredencial.Impressa = true;
                mensagem = " Impressões de Credenciais ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }

                colaboradorCredencial.EmpresaId = Convert.ToInt16(empresa);

                colaboradorCredencial.TipoCredencialId = tipo ? 1 : 2;
                mensagemComplemento = tipo ? " Permanentes " : " Temporárias ";


                mensagem += mensagemComplemento + mensagemPeriodo;

                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(23);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                var result = objColaboradorCredencial.ListarColaboradorCredencialImpressoesView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt); 
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///  Relatório de vias adicionais de Credenciais
        /// </summary>
        /// <param name="_tipo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltroCredencialViasAdicionaisCommand(int _tipo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;

                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                colaboradorCredencial.Impressa = true;

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = "entre " + dataIni + " e " + dataFim + "";
                }

                if (_tipo > 0)
                {
                    colaboradorCredencial.CredencialMotivoId = _tipo;

                    switch (_tipo)
                    {
                        case 2:
                            mensagemComplemento = "SEGUNDA EMISSÃO";
                            break;
                        case 3:
                            mensagemComplemento = "TERCEIRA EMISSÃO";
                            break;
                    }
                    mensagem = "Todas as VIAS ADICIONAIS " + mensagemComplemento + " de CREDENCIAIS emitidas " + mensagemPeriodo;
                }
                else
                {
                    colaboradorCredencial.CredencialMotivoId = 0;
                    mensagem = "Todas as VIAS ADICIONAIS de CREDENCIAIS emitidas" + mensagemPeriodo;
                }

                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(21);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                //Faz a busca do registros de colaboradores credenciais vias adicionais:  2 - segunda e 3 - terceira
                var result = objColaboradorCredencial.ListarColaboradorCredencialViaAdicionaisView(colaboradorCredencial).Where(n => n.CredencialMotivoId == 2 || n.CredencialMotivoId == 3);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1); 
                var tempArea = Path.GetTempPath(); 
                if (configSistema.EmpresaLOGO != null) 
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #region AUTORIZAÇÃO 

        /// <summary>
        /// Filtrar Relatório de Autorizações Permanentes/Temporárias
        /// </summary>
        /// <param name="_check"></param>
        public void OnFiltroRelatorioAutorizacoesCommand(int tipo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;
                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();

                veiculoCredencial.CredencialStatusId = 1;
                veiculoCredencial.Impressa = true;
                veiculoCredencial.TipoCredencialId = tipo;
                verbo = tipo == 1 ? "PERMANENTES" : "TEMPORÁRIAS";

                mensagem = "Todas as AUTORIZAÇÕES " + verbo + " ativas ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                    veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagem = "Todas as AUTORIZAÇÕES " + verbo + " ativas concedidas entre " + dataIni + " e " + dataFim + "";
                }

                var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(2);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                var result = objVeiculoCredencial.ListarVeiculoCredencialConcedidasView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///   Relatório de veiculos Credenciais( Autorizações ) inválidas 
        /// </summary>
        /// <param name="_tipo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioAutorizacoesInvalidasFiltroCommand(bool tipo, int status, CredencialMotivoView credencialMotivoSelecionado, string dataIni, string dataFim, bool flaDevolucaoEntregaBO)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                string mensagemComplementoTipo = string.Empty;

                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();
                mensagem = "Todas as AUTORIZAÇÕES ";
                veiculoCredencial.CredencialStatusId = 2;
                veiculoCredencial.Impressa = true;
                veiculoCredencial.DevolucaoEntregaBo = flaDevolucaoEntregaBO; 

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Baixa = DateTime.Parse(dataIni);
                    veiculoCredencial.BaixaFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }

                veiculoCredencial.TipoCredencialId = tipo ? 1 : 2; 
                mensagemComplementoTipo = tipo ? " PERMANENTES " : " TEMPORÁRIAS "; 

                if (status == 0)
                {
                    mensagemComplemento = "INVÁLIDAS ";
                }
                else
                {
                    veiculoCredencial.CredencialMotivoId = credencialMotivoSelecionado.CredencialMotivoId;
                    mensagemComplemento = credencialMotivoSelecionado.Descricao.ToString();
                }
                mensagem += mensagemComplementoTipo + mensagemComplemento + mensagemPeriodo;

                var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(6);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                var result = objVeiculoCredencial.ListarVeiculoCredencialInvalidasView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        ///  Relatório de veiculos Credenciais( Autorizações ) por área
        /// </summary>
        /// <param name="area"></param>
        /// <param name="_check"></param>
        /// <param name="objAreaSelecionado"></param>
        public void OnRelatorioAutorizacoesPorAreaCommand(bool tipo,  string area, bool _check, AreaAcessoView objAreaSelecionado)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                string mensagemComplementoTipo = string.Empty;


                Domain.EntitiesCustom.FiltroReportVeiculoCredencial veiculoCredencial = new Domain.EntitiesCustom.FiltroReportVeiculoCredencial();
                veiculoCredencial.CredencialStatusId = 1;
                veiculoCredencial.Impressa = true;

                if (objAreaSelecionado.AreaAcessoId > 0)
                {
                    mensagemComplemento = " - " + objAreaSelecionado.Identificacao + " / " + objAreaSelecionado.Descricao;
                }

                veiculoCredencial.TipoCredencialId = tipo ? 1 : 2;
                mensagemComplementoTipo = tipo ? " PERMANENTES " : " TEMPORÁRIAS ";

                mensagem = " Todas as AUTORIZAÇÕES " + mensagemComplementoTipo  + " E VÁLIDAS por ÁREA DE ACESSO " + mensagemComplemento;

                if (area != string.Empty)
                {
                    veiculoCredencial.AreaAcessoId = Convert.ToInt16(area); 
                }

                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(8); 
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return; 

                var result = objVeiculoCredencial.ListarVeiculoCredencialPermanentePorAreaView(veiculoCredencial);
                 
                var resultMapeado = Mapper.Map<List<Views.Model.RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado); 

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de veiculos Credenciais( Autorizações ) por Empresas
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="_check"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioAutorizacoesPorEmpresaCommand(bool tipo, string empresa, bool check, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;
                string mensagemComplementoTipo = string.Empty;


                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();
                veiculoCredencial.Impressa = true;

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                    veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " no período de  " + dataIni + " e " + dataFim + "";
                }

                veiculoCredencial.TipoCredencialId = tipo ? 1 : 2;
                mensagemComplementoTipo = tipo ? " PERMANENTES " : " TEMPORÁRIAS ";

                mensagem = "Todas as AUTORIZAÇÕES " + mensagemComplementoTipo + "emitidas por entidade solicitante";
                mensagem += mensagemPeriodo;

                if (!string.IsNullOrEmpty(empresa))
                {
                    veiculoCredencial.EmpresaId = Convert.ToInt16(empresa);
                }
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(10);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                var result = objVeiculoCredencial.ListarVeiculoCredencialConcedidasView(veiculoCredencial); 
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de impressões veiculos Credenciais( Autorizações ) por Empresas
        /// </summary>
        /// <param name="_empresa"></param>
        /// <param name="_check"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltrosImpressoesAutorizacoesCommand(bool tipo, string empresa, bool check, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;
                string mensagemComplementoTipo = string.Empty;

                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();
                veiculoCredencial.Impressa = true;
                mensagem = " Impressões de Autorizações  ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                    veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }

                veiculoCredencial.EmpresaId = Convert.ToInt16(empresa);

                veiculoCredencial.TipoCredencialId = tipo ? 1 : 2;
                mensagemComplementoTipo = tipo ? " Permanentes " : " Temporárias "; 

                mensagem += mensagemComplementoTipo + mensagemPeriodo;

                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(23);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                List<VeiculosCredenciaisView> result = new List<VeiculosCredenciaisView>();
                 result = objVeiculoCredencial.ListarVeiculoCredencialImpressoesView(veiculoCredencial);
                List<Views.Model.RelVeiculosCredenciaisView> resultMapeado = Mapper.Map<List<Views.Model.RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome); 
                reportDoc.SetDataSource(resultMapeado);  

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                var tempArea = Path.GetTempPath();
                if (configSistema.EmpresaLOGO != null)
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de vias adicionais de Credenciais
        /// </summary>
        /// <param name="_tipo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltroAutorizacaoViasAdicionaisCommand(int motivoTipo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;
                string mensagemComplemento = string.Empty;

                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();

                veiculoCredencial.Impressa = true;
                mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES emitidas";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                    veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = "entre " + dataIni + " e " + dataFim + "";
                }

                if (motivoTipo > 0)
                {
                    veiculoCredencial.CredencialMotivoId = motivoTipo;

                    switch (motivoTipo)
                    {
                        case 2:
                            mensagemComplemento = "SEGUNDA EMISSÃO";
                            break;
                        case 3:
                            mensagemComplemento = "TERCEIRA EMISSÃO";
                            break;
                    }
                    mensagem = "Todas as VIAS ADICIONAIS " + mensagemComplemento + " de AUTORIZAÇÕES emitidas " + mensagemPeriodo;
                }
                else
                {
                    veiculoCredencial.CredencialMotivoId = 0;
                    mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES emitidas" + mensagemPeriodo;
                }

                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(22);
                if (relatorioGerencial == null || relatorioGerencial.ArquivoRpt == null || String.IsNullOrEmpty(relatorioGerencial.ArquivoRpt)) return;

                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial).Where(n => n.CredencialMotivoId == 2 || n.CredencialMotivoId == 3);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado); 

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                var configSistema = objConfiguraSistema.BuscarPelaChave(1); 
                var tempArea = Path.GetTempPath(); 
                if (configSistema.EmpresaLOGO != null) 
                {
                    byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                    System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                    reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
                }

                WpfHelp.ShowRelatorio(reportDoc);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        #endregion

        #endregion


        #endregion



    }
}
