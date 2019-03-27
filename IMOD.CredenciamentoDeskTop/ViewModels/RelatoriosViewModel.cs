using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private readonly IRelatorioService _relatorioService = new RelatorioService();
        private readonly IRelatorioGerencialService _relatorioGerencialServiceService = new RelatorioGerencialService();
        private readonly IEmpresaService _empresaService = new EmpresaService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        private Relatorios relatorio = new Relatorios();
        private RelatoriosGerenciais relatorioGerencial = new RelatoriosGerenciais();
        private readonly IColaboradorCredencialService objColaboradorCredencial = new ColaboradorCredencialService();
        private readonly IVeiculoCredencialService objVeiculoCredencial = new VeiculoCredencialService();

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

        private void CarregaColecaoRelatorios()
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

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void CarregaColecaoAreasAcessos()
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

        private void CarregaColecaoEmpresas(int? idEmpresa = null, string nome = null, string apelido = null, string cnpj = null)
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

        public void CarregaMotivoCredenciais(int statusId)
        {
            try
            {
                var lst1 = _auxiliaresService.CredencialMotivoService.Listar().Where(n => n.CredencialStatusId == statusId).ToList();

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
                mensagem = "Todas as CREDENCIAIS ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " ativas concedidas entre " + dataIni + " e " + dataFim + "";
                }
                
                if (tipo)
                {
                    colaboradorCredencial.TipoCredencialId = 1;
                    mensagemComplemento = " PERMANENTES ";
                }
                
                else
                {
                    colaboradorCredencial.TipoCredencialId = 2;
                    mensagemComplemento = " TEMPORÁRIAS ";
                }

                var result = objColaboradorCredencial.ListarColaboradorCredencialConcedidasView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);

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
        public void OnRelatorioCredenciaisInvalidasFiltroCommand(int status, CredencialMotivoView credencialMotivoSelecionado, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                
                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                mensagem = "Todas as CREDENCIAIS ";
                colaboradorCredencial.CredencialStatusId = 2;

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Baixa = DateTime.Parse(dataIni);
                    colaboradorCredencial.BaixaFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }
                
                if (status == 0)
                {
                    mensagemComplemento = "INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas) ";
                }
                else
                {
                    colaboradorCredencial.CredencialMotivoId = credencialMotivoSelecionado.CredencialMotivoId;
                    mensagemComplemento = credencialMotivoSelecionado.Descricao.ToString();
                }
                mensagem += mensagemComplemento + mensagemPeriodo;

                var result = objColaboradorCredencial.ListarColaboradorCredencialInvalidasView(colaboradorCredencial).Where(n => n.CredencialStatusId == 2);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(5);

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
        public void OnRelatorioCredencialPorAreaCommand(string area, bool _check, AreaAcessoView objAreaSelecionado)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                
                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                colaboradorCredencial.CredencialStatusId = 1;

                

                if (objAreaSelecionado.AreaAcessoId > 0)
                {
                    mensagemComplemento = " - " + objAreaSelecionado.Identificacao + " / " + objAreaSelecionado.Descricao;
                }

                mensagem = " Todas as CREDENCIAIS PERMANENTES E VÁLIDAS por ÁREA DE ACESSO " + mensagemComplemento;

                if (area != "")
                {
                    colaboradorCredencial.AreaAcessoId = Convert.ToInt16(area);
                }

                var result = objColaboradorCredencial.ListarColaboradorCredencialPermanentePorAreaView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(7);
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
        public void OnRelatorioFiltroCredencialPorEmpresaCommand(string empresa, bool check, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;
                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                mensagem = "Todas as CREDENCIAIS emitidas por entidade solicitante";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " no período de  " + dataIni + " e " + dataFim + "";
                }
                mensagem += mensagemPeriodo;

                if (!string.IsNullOrEmpty(empresa))
                {
                    colaboradorCredencial.EmpresaId = Convert.ToInt16(empresa);
                }

                //Faz a busca do registros de colaboradores credenciais concedidas
                var result = objColaboradorCredencial.ListarColaboradorCredencialConcedidasView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(9); 
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt); 
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
        public void OnFiltrosColaboradorCredencialImpressoesCommand(string empresa, bool check, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;

                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();

                mensagem = " Impressões de Credenciais registradas ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }

                colaboradorCredencial.EmpresaId = Convert.ToInt16(empresa);

                mensagem += mensagemPeriodo;

                var result = objColaboradorCredencial.ListarColaboradorCredencialImpressoesView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(23);
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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

                //Faz a busca do registros de colaboradores credenciais vias adicionais:  2 - segunda e 3 - terceira
                var result = objColaboradorCredencial.ListarColaboradorCredencialViaAdicionaisView(colaboradorCredencial).Where(n => n.CredencialMotivoId == 2 || n.CredencialMotivoId == 3);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(21);
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
                veiculoCredencial.TipoCredencialId = tipo;
                switch (tipo)
                {
                    case 1:
                        verbo = "PERMANENTES";
                        break;
                    case 2:
                        verbo = "TEMPORÁRIAS";
                        break;
                }

                mensagem = "Todas as AUTORIZAÇÕES " + verbo + " ativas ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                    veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagem = "Todas as AUTORIZAÇÕES " + verbo + " ativas concedidas entre " + dataIni + " e " + dataFim + "";
                }

                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(2);

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                reportDoc.Refresh();

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
        public void OnRelatorioAutorizacoesInvalidasFiltroCommand(int status, CredencialMotivoView credencialMotivoSelecionado, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;

                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();
                mensagem = "Todas as AUTORIZAÇÕES ";
                veiculoCredencial.CredencialStatusId = 2;

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Baixa = DateTime.Parse(dataIni);
                    veiculoCredencial.BaixaFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }

                if (status == 0)
                {
                    mensagemComplemento = "INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas) ";
                }
                else
                {
                    veiculoCredencial.CredencialMotivoId = credencialMotivoSelecionado.CredencialMotivoId;
                    mensagemComplemento = credencialMotivoSelecionado.Descricao.ToString();
                }
                mensagem += mensagemComplemento + mensagemPeriodo;

                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(6);

                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
        public void OnRelatorioAutorizacoesPorAreaCommand(string area, bool _check, AreaAcessoView objAreaSelecionado)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                
                Domain.EntitiesCustom.FiltroReportVeiculoCredencial veiculoCredencial = new Domain.EntitiesCustom.FiltroReportVeiculoCredencial();
                veiculoCredencial.CredencialStatusId = 1;

                if (objAreaSelecionado.AreaAcessoId > 0)
                {
                    mensagemComplemento = " - " + objAreaSelecionado.Identificacao + " / " + objAreaSelecionado.Descricao;
                }

                mensagem = " Todas as AUTORIZAÇÕES PERMANENTES E VÁLIDAS por ÁREA DE ACESSO " + mensagemComplemento;

                if (area != string.Empty)
                {
                    veiculoCredencial.AreaAcessoId = Convert.ToInt16(area); 
                }
                 
                var result = objVeiculoCredencial.ListarVeiculoCredencialPermanentePorAreaView(veiculoCredencial);
                 
                var resultMapeado = Mapper.Map<List<Views.Model.RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(8);
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado); 

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
        public void OnRelatorioAutorizacoesPorEmpresaCommand(string empresa, bool check, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;
                
                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();

                mensagem = "Todas as AUTORIZAÇÕES emitidas por entidade solicitante";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                    veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " no período de  " + dataIni + " e " + dataFim + "";
                }
                mensagem += mensagemPeriodo;

                if (!string.IsNullOrEmpty(empresa))
                {
                    veiculoCredencial.EmpresaId = Convert.ToInt16(empresa);
                }

                var result = objVeiculoCredencial.ListarVeiculoCredencialConcedidasView(veiculoCredencial); 
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(10);
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
        public void OnFiltrosImpressoesAutorizacoesCommand(string empresa, bool check, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;

                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();

                mensagem = " Impressões de Autorizações registradas ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                    veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }

                veiculoCredencial.EmpresaId = Convert.ToInt16(empresa); 

                mensagem += mensagemPeriodo;

                List<VeiculosCredenciaisView> result = new List<VeiculosCredenciaisView>();
                 result = objVeiculoCredencial.ListarVeiculoCredencialImpressoesView(veiculoCredencial);
                List<Views.Model.RelVeiculosCredenciaisView> resultMapeado = Mapper.Map<List<Views.Model.RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(23);
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome); 
                reportDoc.SetDataSource(resultMapeado);  

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
        public void OnFiltroAutorizacaoViasAdicionaisCommand(int _tipo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;
                string mensagemComplemento = string.Empty;

                FiltroReportVeiculoCredencial veiculoCredencial = new FiltroReportVeiculoCredencial();

                mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES emitidas";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                    veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = "entre " + dataIni + " e " + dataFim + "";
                }

                if (_tipo > 0)
                {
                    veiculoCredencial.CredencialMotivoId = _tipo;

                    switch (_tipo)
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
                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(22);
                byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, relatorioGerencial.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

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
