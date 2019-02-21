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
        /// Filtrar Relatório de Credenciais Permanentes/Temporárias
        /// </summary>
        /// <param name="_tipo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltroRelatorioCredencialCommand(bool tipo, string dataIni, string dataFim)
        {
            string mensagem = string.Empty;
            string mensagemComplemento = string.Empty;
            string mensagemPeriodo = string.Empty;
            // objeto com o filtro de parâmetros da consulta
            Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();

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
                //Tipo PERMANENTE
                if (tipo)
                {
                    colaboradorCredencial.TipoCredencialId = 1;
                    mensagemComplemento = " PERMANENTES ";
                }
                // Tipo TEMPORÁRIO
                else
                {
                    colaboradorCredencial.TipoCredencialId = 2;
                    mensagemComplemento = " TEMPORÁRIAS ";
                }

                //Faz a busca do registros de colaboradores credenciais concedidas
                var result = objColaboradorCredencial.ListarColaboradorCredencialConcedidasView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                //Busca o layout do relatório (arquivo .rpt) no banco de dados
                //var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);

                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\RelatorioConcessaoCredenciais.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);
                reportDoc.SetDataSource(resultMapeado);

                mensagem += mensagemComplemento + mensagemPeriodo;
                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de Credenciais Inválidas
        /// (Indeferidas,Roubadas,Extraviadas,Não-Devolvidas...)
        /// </summary>
        /// <param name="_status"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioCredenciaisInvalidasFiltroCommand(int status, CredencialMotivoView credencialMotivoSelecionado, string dataIni, string dataFim)
        {
            try
            {
                //CREDENCIAIS PERMANENTES - (5)
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(5);

                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                // objeto com o filtro de parâmetros da consulta
                Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();
                mensagem = "Todas as CREDENCIAIS ";
                colaboradorCredencial.CredencialStatusId = 2;

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Baixa = DateTime.Parse(dataIni);
                    colaboradorCredencial.BaixaFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }
                // se for zero, trazer todas as credenciais inválidas 
                if (status == 0)
                {
                    mensagemComplemento = "INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas/destruídas) ";
                }
                else
                {
                    colaboradorCredencial.CredencialMotivoId = credencialMotivoSelecionado.CredencialMotivoId;
                    mensagemComplemento = credencialMotivoSelecionado.Descricao.ToString();
                }
                mensagem += mensagemComplemento + mensagemPeriodo;

                //Faz a busca do registros de colaboradores credenciais status 2 - inativas
                var result = objColaboradorCredencial.ListarColaboradorCredencialInvalidasView(colaboradorCredencial).Where(n => n.CredencialStatusId == 2);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                //Busca o layout do relatório (arquivo .rpt) no banco de dados
                //var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);

                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\RelatorioCredenciaisInvalidas.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);
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
        /// Filtrar Relatório de Credenciais por Área de Acesso
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
                // objeto com o filtro de parâmetros da consulta
                Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();
                colaboradorCredencial.CredencialStatusId = 1;

                //Busca layout do relatório
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(7);

                if (objAreaSelecionado.AreaAcessoId > 0)
                {
                    mensagemComplemento = " - " + objAreaSelecionado.Identificacao + " / " + objAreaSelecionado.Descricao;
                }

                mensagem = " Todas as CREDENCIAIS PERMANENTES E VÁLIDAS por ÁREA DE ACESSO " + mensagemComplemento;

                if (area != "")
                {
                    colaboradorCredencial.AreaAcessoId = Convert.ToInt16(area);
                }

                //Faz a busca do registros de colaboradores credenciais por área
                var result = objColaboradorCredencial.ListarColaboradorCredencialPermanentePorAreaView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                //Busca o layout do relatório (arquivo .rpt) no banco de dados
                //var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);

                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\RelatorioCredenciaisEmitidasPorArea.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);
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
        /// Filtrar Relatório de Credenciais por Empresas
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
                // objeto com o filtro de parâmetros da consulta
                Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(9);
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

                //Busca o layout do relatório (arquivo .rpt) no banco de dados
                //var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);

                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\RelatorioCredenciaisEmitidasPorEmpresa.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);
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
        /// Filtrar Relatório de impressões de Credenciais e Autorizações por Empresas
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

                // objeto com o filtro de parâmetros da consulta
                Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();

                //CREDENCIAIS 
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(23);

                mensagem = " Impressões de Credenciais registradas ";

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = " entre " + dataIni + " e " + dataFim + "";
                }

                colaboradorCredencial.EmpresaId = Convert.ToInt16(empresa);

                mensagem += mensagemPeriodo;

                //Faz a busca do registros de colaboradores credenciais concedidas
                var result = objColaboradorCredencial.ListarColaboradorCredencialImpressoesView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                //Busca o layout do relatório (arquivo .rpt) no banco de dados
                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\RelatorioImpressaoCredencial.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);
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
        public void OnFiltroCredencialViasAdicionaisCommand(int _tipo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;

                // objeto com o filtro de parâmetros da consulta
                Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();

                //CREDENCIAIS
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(21);

                if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                {
                    colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                    colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                    mensagemPeriodo = "entre " + dataIni + " e " + dataFim + "";
                }

                if (_tipo > 0)
                {
                    //Seta o tipo de motivo
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

                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\RelatorioViasAdicionaisCredenciais.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);
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
                // objeto com o filtro de parâmetros da consulta
                FiltroVeiculoCredencial veiculoCredencial = new FiltroVeiculoCredencial();
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(2);

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
                //if (tipo == 1 )
                //{



                //    ////if (dataIni == "" || dataFim == "")
                //    ////{
                //    ////    //Filtrar Autorizações Permanentes e Ativas
                //    ////    formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                //    ////              " AND {CredenciaisStatus.CredencialStatusID} = 1 ";

                //    ////    mensagem = "Todas as AUTORIZAÇÕES PERMANENTES ativas ";
                //    ////}
                //    //else
                //    //{
                //    //    veiculoCredencial.CredencialStatusId = 1;
                //    //    veiculoCredencial.TipoCredencialId = 1;
                //    //    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                //    //    {
                //    //        veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                //    //        veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                //    //        mensagem = "Todas as AUTORIZAÇÕES PERMANENTES ativas concedidas entre " + dataIni + " e " + dataFim + "";
                //    //    }
                //    //    ////Listar Autorizações Permanentes e Ativas por período
                //    //    //formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                //    //    //          " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                //    //    //          " and ({VeiculosCredenciais.Emissao} <= CDate ('" + dataFim + "')" +
                //    //    //          " and {VeiculosCredenciais.Emissao} >= CDate ('" + dataIni + "') ) ";

                //    //    //mensagem = "Todas as AUTORIZAÇÕES PERMANENTES ativas concedidas entre " + dataIni + " e " + dataFim + "";
                //    //}
                //}
                //AUTORIZAÇÕES TEMPORÁRIAS - (3)
                //else
                //{
                //    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(4);
                //    veiculoCredencial.CredencialStatusId = 1;
                //    veiculoCredencial.TipoCredencialId = 1;
                //    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                //    {
                //        veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                //        veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                //        mensagem = "Todas as AUTORIZAÇÕES PERMANENTES ativas ";
                //    }

                //    if (dataIni == "" || dataFim == "")
                //    {
                //        //Filtrar Autorizações Temporárias e Ativas
                //        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                //                  "AND {CredenciaisStatus.CredencialStatusID} = 1 ";

                //        mensagem = "Todas as AUTORIZAÇÕES TEMPORÁRIAS ativas ";
                //    }
                //    else
                //    {
                //        //Listar Autorizações Permanentes e Ativas por período
                //        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                //                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                //                  " and ({VeiculosCredenciais.Emissao} <= CDate ('" + dataFim + "')" +
                //                  " and {VeiculosCredenciais.Emissao} >= CDate ('" + dataIni + "') ) ";

                //        mensagem = "Todas as AUTORIZAÇÕES TEMPORÁRIAS ativas concedidas entre " + dataIni + " e " + dataFim + "";
                //    }
                //}

                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt); 
                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\relatorioAutorizacaoValidas.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);

                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_status"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioAutorizacoesInvalidasFiltroCommand(int _tipo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemPeriodo = string.Empty;
                // objeto com o filtro de parâmetros da consulta
                FiltroVeiculoCredencial veiculoCredencial = new FiltroVeiculoCredencial();

                mensagem = "Todas as AUTORIZAÇÕES INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas/destruídas)";
                //6_Relatório_AutorizacoesInvalidas.rpt
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(6);
                veiculoCredencial.CredencialStatusId = 2;
                //Caso período de datas seja vazio (Todas Inválidas)
                if (_tipo == 0)
                {
                    // 2, 3 código de todas as vias adicionais emitidas
                    //colaboradorCredencial.CredencialMotivoId1 = 2;
                    //colaboradorCredencial.CredencialMotivoId2 = 3;

                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                        veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        mensagem = "Todas as AUTORIZAÇÕES INVÁLIDAS (vencidas/indeferidas/canceladas/extraviadas/destruídas) ";
                    }
                    else
                    {
                        if (!(_tipo.Equals(0)))
                        {
                            veiculoCredencial.CredencialMotivoId = _tipo;
                        }
                        if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                        {
                            veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                            veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        }

                        //veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                        //veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);

                        //formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3] " +
                        //          " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + dataFim + "')" +
                        //          " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + dataIni + "') ) ";

                        //mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES ATIV emitidas entre " + dataIni + " e " + dataFim;
                        mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES ATIV";
                    }
                }
                else
                {
                    //Seta o tipo de motivo
                    veiculoCredencial.CredencialStatusId = 2;
                    veiculoCredencial.CredencialMotivoId = _tipo;
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                        veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        switch (_tipo)
                        {
                            case 13:
                                verbo = "DESTRUÍDAS";
                                break;
                            case 14:
                                verbo = "NÃO DEVOLVIDAS";
                                break;
                            case 12:
                                verbo = "INDEFERIDAS";
                                break;
                            case 9:
                                verbo = "EXTRAVIDA";
                                break;
                            case 10:
                                verbo = "ROUBADA";
                                break;
                        }
                        mensagem = "Todas as AUTORIZAÇÕES " + verbo + "entre " + dataIni + " e " + dataFim + "";

                    }
                    else
                    {
                        switch (_tipo)
                        {
                            case 13:
                                verbo = "DESTRUÍDAS";
                                break;
                            case 14:
                                verbo = "NÃO DEVOLVIDAS";
                                break;
                            case 12:
                                verbo = "INDEFERIDAS";
                                break;
                            case 9:
                                verbo = "EXTRAVIDA";
                                break;
                            case 10:
                                verbo = "ROUBADA";
                                break;
                        }
                        mensagem = "Todas as VIAS DE ( " + verbo + " ) ADICIONAIS de AUTORIZAÇÕES";
                    }
                }

                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt); 
                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\relatorioAutorizacaoInvalidas.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);

                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de Credenciais por Área de Acesso
        /// </summary>
        /// <param name="_area"></param>
        /// <param name="_check"></param>
        public void OnRelatorioFiltroPorAreaCommand(string _area, bool _check)
        {
            try
            {
                //Relatório de Credenciais
                if (_check)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(7);
                    mensagem = "Todas as CREDENCIAIS PERMANENTES E VÁLIDAS por ÁREA DE ACESSO";
                }
                //Relatório de Autorizações
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(8);
                    mensagem = "Todas as AUTORIZAÇÕES PERMANENTES E VÁLIDAS por ÁREA DE ACESSO";
                }
                if (_area != "")
                {
                    formula = " {AreasAcessos_0.AreaAcessoID} = " + _area;
                }
                else
                {
                    formula = "";
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioPorArea", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de Credenciais e Autorizações por Empresas
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="_check"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnRelatorioFiltroPorEmpresaCommand(string empresa, bool _check, string _dataIni, string _dataFim)
        {
            try
            {
                //CREDENCIAIS
                if (_check)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(9);
                }
                //AUTORIZAÇÕES
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(10);
                }


                //Filtra Todas Empresas
                if (empresa == "" && _dataIni == "" && _dataFim == "")
                {
                    formula = "";
                    mensagem = "Todas as CREDENCIAIS emitidas por entidade solicitante";
                }

                //Uma Empresa
                else if (_dataIni == "" && _dataFim == "" && _check)
                {
                    formula = " {Empresas.EmpresaID} = " + empresa;
                    mensagem = "Todas as CREDENCIAIS emitidas por entidade solicitante";
                }

                //Credenciais
                else if (_check)
                {
                    //Uma empresas (filtrando por data)
                    if (empresa != "")
                    {
                        formula = "{ColaboradoresCredenciais.Emissao}  <= CDate ('" + _dataFim + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "')" +
                                  " and {Empresas.EmpresaID} = " + empresa + "";
                    }
                    //Todas empresas (filtrando por data)
                    else
                    {
                        formula = "{ColaboradoresCredenciais.Emissao}  <= CDate ('" + _dataFim + "')" +
                                  " and {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "')";
                    }

                    mensagem = "Todas as CREDENCIAIS emitidas por entidade solicitante no período de " + _dataIni + " a " + _dataFim;
                }

                //Autorizacoes
                else
                {
                    //Uma empresa (filtrando por data)
                    if (empresa != "")
                    {
                        formula = "{VeiculosCredenciais.Emissao}  <= CDate ('" + _dataFim + "')" +
                                  " and {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "')" +
                                  " and {Empresas.EmpresaID} = " + empresa + "";
                    }
                    //Todas empresa (filtrando por data)
                    else
                    {
                        formula = "{VeiculosCredenciais.Emissao}  <= CDate ('" + _dataFim + "')" +
                                  " and {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "')";
                    }

                    mensagem = "Todas as AUTORIZAÇÕES emitidas por entidade solicitante no período de " + _dataIni + " a " + _dataFim;

                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioPorEmpresa", formula, mensagem);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Filtrar Relatório de impressões de Credenciais e Autorizações por Empresas
        /// </summary>
        /// <param name="_empresa"></param>
        /// <param name="_check"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltrosImpressoesCommand(string _empresa, bool _check, string _dataIni, string _dataFim)
        {
            try
            {
                //CREDENCIAIS
                if (_check)
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(23);
                }
                //AUTORIZAÇÕES
                else
                {
                    relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(24);
                }

                if (_dataIni == "" || _dataFim == "")
                {
                    if (_empresa != "")
                    {
                        formula = "{ Empresas.EmpresaID} = " + _empresa + "";
                    }
                    else
                    {
                        formula = "";
                    }
                }

                else
                {
                    //CREDENCIAIS
                    if (_check)
                    {
                        //Uma empresas (filtrando por data)
                        if (_empresa != "")
                        {
                            formula = "{ColaboradoresCredenciaisImpressoes.DataImpressao} <= cdate('" + _dataFim + "') " +
                                      " and {ColaboradoresCredenciaisImpressoes.DataImpressao} >= cdate('" + _dataIni + "')" +
                                      " and {Empresas.EmpresaID} = " + _empresa + "";
                        }
                        else
                        {
                            formula = "{ColaboradoresCredenciaisImpressoes.DataImpressao} <= cdate('" + _dataFim + "') " +
                                      " and {ColaboradoresCredenciaisImpressoes.DataImpressao} >= cdate('" + _dataIni + "')";
                        }
                    }

                    //AUTORIZAÇÕES
                    else
                    {
                        //Uma empresas (filtrando por data)
                        if (_empresa != "")
                        {
                            formula = "{VeiculosCredenciaisImpressoes.DataImpressao} <= cdate('" + _dataFim + "') " +
                                      " and {VeiculosCredenciaisImpressoes.DataImpressao} >= cdate('" + _dataIni + "')" +
                                      " and {Empresas.EmpresaID} = " + _empresa + "";
                        }
                        else
                        {
                            formula = "{VeiculosCredenciaisImpressoes.DataImpressao} <= cdate('" + _dataFim + "') " +
                                      " and {VeiculosCredenciaisImpressoes.DataImpressao} >= cdate('" + _dataIni + "')";
                        }
                    }
                    mensagem = "Impressões de Credenciais registradas entre " + _dataIni + " e " + _dataFim;
                }

                var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                WpfHelp.ShowRelatorio(arrayFile, "RelatorioImpressoes", formula, "");
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
                // objeto com o filtro de parâmetros da consulta
                FiltroVeiculoCredencial veiculoCredencial = new FiltroVeiculoCredencial();

                mensagem = "Todas as VIAS ADICIONAIS de AURORIZAÇÕES emitidas";

                //CREDENCIAIS
                relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(22);

                if (_tipo == 0)
                {
                    // 2, 3 código de todas as vias adicionais emitidas
                    veiculoCredencial.CredencialMotivoId = 2;
                    veiculoCredencial.CredencialMotivoId1 = 5;

                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                        veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "entre " + dataIni + " e " + dataFim + "";
                        mensagem += mensagemPeriodo;
                    }
                    else
                    {
                        if (!(_tipo.Equals(0)))
                        {
                            veiculoCredencial.CredencialMotivoId = _tipo;
                        }
                        if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                        {
                            veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                            veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        }

                        //veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                        //veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);

                        //formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3] " +
                        //          " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + dataFim + "')" +
                        //          " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + dataIni + "') ) ";

                        //mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES ATIV emitidas entre " + dataIni + " e " + dataFim;
                        mensagem = "Todas as VIAS ADICIONAIS de AUTORIZAÇÕES ATIV";
                    }
                }
                else
                {
                    //Seta o tipo de motivo
                    veiculoCredencial.CredencialMotivoId = _tipo;

                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        veiculoCredencial.Emissao = DateTime.Parse(dataIni);
                        veiculoCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        switch (_tipo)
                        {
                            case 2:
                                verbo = "SEGUNDA EMISSÃO";
                                break;
                            case 3:
                                verbo = "TERCEIRA EMISSÃO";
                                break;
                        }
                        mensagem = "Todas as VIAS DE ( " + verbo + " ) ADICIONAIS de AUTORIZAÇÕES emitidas entre " + dataIni + " e " + dataFim;

                    }
                    else
                    {
                        switch (_tipo)
                        {
                            case 2:
                                verbo = "SEGUNDA EMISSÃO";
                                break;
                            case 3:
                                verbo = "TERCEIRA EMISSÃO";
                                break;
                        }
                        mensagem = "Todas as VIAS DE ( " + verbo + " ) ADICIONAIS de AUTORIZAÇÕES";
                    }
                }
                //Faz a busca do registros de colaboradores credenciais concedidas  
                //var result = objColaboradorCredencial.ListarColaboradorCredencialViaAdicionaisView(veiculoCredencial);
                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt); 
                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\relatorioViasAdicionaisAutorizacao.rpt";

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);

                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                WpfHelp.ShowRelatorio(reportDoc);
                //var arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                //WpfHelp.ShowRelatorio(arrayFile, "RelatorioAutorizacoesViasAdicionais", formula, mensagem);
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
