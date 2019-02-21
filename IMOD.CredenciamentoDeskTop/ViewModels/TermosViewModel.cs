using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class TermosViewModel : ViewModelBase
    {

        #region Variaveis Privadas

        private readonly IVeiculoCredencialService objVeiculoCredencial = new VeiculoCredencialService();

        private ObservableCollection<RelatorioView> _Relatorios;

        private RelatorioView _RelatorioSelecionado;

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;

        private string formula;
        private string _mensagem;
        private string verbo;

        private readonly IRelatorioGerencialService _relatorioGerencialServiceService = new RelatorioGerencialService();

        private RelatoriosGerenciais termo = new RelatoriosGerenciais(); 

        private readonly IColaboradorCredencialService objColaboradorCredencial = new ColaboradorCredencialService(); 

        #endregion

        #region Comandos dos Botoes 

        /// <summary>
        /// Filtrar Termos de Credenciais
        /// </summary>
        /// <param name="_report"></param>
        /// <param name="_status"></param>
        /// <param name="_periodo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltrosTermosCredenciaisCommand(int report, int status, int periodo, string dataIni, string dataFim)
        {
            switch (report)
            {
                case 11:
                    this.CarregarCredencialConcedidas(report, status, periodo, dataIni, dataFim);
                    break; 
                case 13:
                case 15:
                case 17:
                    this.CarregarCredencialInativas(report, status, periodo, dataIni, dataFim);
                    break; 
                default:
                    this.CarregarCredencialViasAdicionais(report, status, periodo, dataIni, dataFim);
                    break;
            }
        }


        /// <summary>
        /// Filtrar Termos de Credenciais
        /// </summary>
        /// <param name="_report"></param>
        /// <param name="_status"></param>
        /// <param name="_periodo"></param>
        /// <param name="_dataIni"></param>
        /// <param name="_dataFim"></param>
        public void OnFiltrosTermosAutorizacoesCommand(int _report, int _status, int _periodo, string _dataIni, string _dataFim)
        {

            var data = DateTime.Now;
            ////var novaData = data.AddMonths(-3);
            //var novaDatadias = data.AddDays(-7);

            string mensagem = string.Empty;
            string mensagemPeriodo = string.Empty;
            // objeto com o filtro de parâmetros da consulta
            FiltroVeiculoCredencial veiculoCredencial = new FiltroVeiculoCredencial();
            var fileName = "";
            int para1 = 0;
            int para2 = 0;
            switch (_report)
            {
                case 12:
                    veiculoCredencial.CredencialStatusId = 1;
                    veiculoCredencial.CredencialMotivoId = 0;
                    veiculoCredencial.CredencialMotivoId1 = 5;
                    para1 = 1;
                    para2 = 0;
                    verbo = "concedeu";
                    fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\TERMO_concesso_autorizacao.rpt";
                    break;
                case 16:
                    veiculoCredencial.CredencialMotivoId = 12;
                    para1 = 12;
                    para2 = 0;
                    //result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial).Where(n => n.CredencialMotivoId.Equals(2) || n.CredencialMotivoId.Equals(3));
                    verbo = "indeferiu";
                    fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\TERMO_indeferimento_autorizacao.rpt";
                    break;
                case 14:
                    veiculoCredencial.CredencialStatusId = 2;
                    //veiculoCredencial.CredencialMotivoId = 15;
                    para1 = 15;
                    para2 = 0;
                    verbo = "cancelou";
                    fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\TERMO_cancelamento_autorizacao.rpt";
                    break;
                case 18:
                    veiculoCredencial.CredencialStatusId = 2;
                    veiculoCredencial.CredencialMotivoId = 13;
                    para1 = 13;
                    para2 = 0;
                    verbo = "destruiu";
                    fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\TERMO_destruicao_autorizacao.rpt";
                    break;
                default:
                    veiculoCredencial.CredencialStatusId = 1;
                    veiculoCredencial.CredencialMotivoId = 2;
                    veiculoCredencial.CredencialMotivoId1 = 5;
                    para1 = 1;
                    para2 = 0;
                    verbo = "emitiu";
                    fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\TERMO_vias_adicionais_autorizacao.rpt";
                    break;
            }
            //termo = _relatorioGerencialServiceService.BuscarPelaChave(_report);


            if (_periodo == 1)
            {
                var novaDatadias = data.AddDays(-1); //subtrai dia
                //string minhaData = DateTime.Now.ToShortDateString().ToString();
                veiculoCredencial.Emissao = novaDatadias;
                veiculoCredencial.EmissaoFim = DateTime.Now;
                _mensagem = "Na data " + DateTime.Now.ToShortDateString() + " " +
                                    "este setor de credenciamento do AEROPORTO INTERNACIONAL " +
                                    "DE PORTO ALEGRE, " + verbo + " as seguintes vias adicionais de autorizações:";
            }
            else if (_periodo == 7)
            {

                //var novaData = data.AddMonths(-3); //subtrai mês
                var novaDatadias = data.AddDays(-7); //subtrai dia
                veiculoCredencial.Emissao = novaDatadias;
                veiculoCredencial.EmissaoFim = DateTime.Now;

                _mensagem = "Durante o período de " + novaDatadias.ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo +
                                    " as seguintes vias adicionais de autorizações:";
            }
            else if (_periodo == 30)
            {
                //var novaData = data.AddMonths(-3); //subtrai mês
                var novaDatadias = data.AddDays(-30); //subtrai dia

                switch (_report)
                {
                    case 12:
                        veiculoCredencial.Emissao = novaDatadias;
                        veiculoCredencial.EmissaoFim = DateTime.Now;
                        break;
                    case 14:
                        veiculoCredencial.Baixa = novaDatadias;
                        veiculoCredencial.BaixaFim = DateTime.Now;
                        break;
                    case 16:
                        veiculoCredencial.Baixa = novaDatadias;
                        veiculoCredencial.BaixaFim = DateTime.Now;
                        break;
                    case 18:
                        veiculoCredencial.Baixa = novaDatadias;
                        veiculoCredencial.BaixaFim = DateTime.Now;
                        break;
                    default:
                        veiculoCredencial.Emissao = novaDatadias;
                        veiculoCredencial.EmissaoFim = DateTime.Now;
                        break;

                }
                _mensagem = "Durante o período de " + novaDatadias.ToShortDateString() + " " +
                                    "a " + DateTime.Now.ToShortDateString() + " esse setor de credenciamento do " +
                                    "AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " + verbo +
                                    " as seguintes vias adicionais de autorizações:";
            }
            else
            {
                //if (!(_dataIni.Equals(string.Empty) || _dataFim.Equals(string.Empty)))
                //{

                //    veiculoCredencial.Emissao = DateTime.Parse(_dataIni);
                //    veiculoCredencial.EmissaoFim = DateTime.Parse(_dataFim);
                //    //mensagemPeriodo = "entre " + _dataIni + " e " + _dataFim + "";
                //    //mensagem += mensagemPeriodo;

                //    _mensagem = "Durante o período de " + _dataIni + " a " + _dataFim + " " +
                //                    "esse setor de credenciamento do AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " +
                //                    "" + verbo + " as seguintes vias adicionais de autorizações:";
                //}
                switch (_report)
                {
                    case 12:
                        if (!(_dataIni.Equals(string.Empty) || _dataFim.Equals(string.Empty)))
                        {

                            veiculoCredencial.Emissao = DateTime.Parse(_dataIni);
                            veiculoCredencial.EmissaoFim = DateTime.Parse(_dataFim);

                        }
                        break;
                    case 16:
                        if (!(_dataIni.Equals(string.Empty) || _dataFim.Equals(string.Empty)))
                        {

                            veiculoCredencial.Baixa = DateTime.Parse(_dataIni);
                            veiculoCredencial.BaixaFim = DateTime.Parse(_dataFim);

                        }
                        break;
                    case 14:
                        if (!(_dataIni.Equals(string.Empty) || _dataFim.Equals(string.Empty)))
                        {

                            veiculoCredencial.Baixa = DateTime.Parse(_dataIni);
                            veiculoCredencial.BaixaFim = DateTime.Parse(_dataFim);

                        }
                        break;
                    case 18:
                        if (!(_dataIni.Equals(string.Empty) || _dataFim.Equals(string.Empty)))
                        {

                            veiculoCredencial.Baixa = DateTime.Parse(_dataIni);
                            veiculoCredencial.BaixaFim = DateTime.Parse(_dataFim);

                        }
                        break;
                    default:
                        if (!(_dataIni.Equals(string.Empty) || _dataFim.Equals(string.Empty)))
                        {

                            veiculoCredencial.Emissao = DateTime.Parse(_dataIni);
                            veiculoCredencial.EmissaoFim = DateTime.Parse(_dataFim);

                        }
                        break;
                }

                _mensagem = "Durante o período de " + _dataIni + " a " + _dataFim + " " +
                                           "esse setor de credenciamento do AEROPORTO INTERNACIONAL DE PORTO ALEGRE, " +
                                           "" + verbo + " as seguintes vias adicionais de autorizações:";
            }

            if (para1 > 0)
            {
                //var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial).Where(n => n.CredencialMotivoId.Equals(para1));
                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial);
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);

                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(_mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = _mensagem;
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }
            else if (para1 > 0 & para2 > 0)
            {
                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(veiculoCredencial).Where(n => n.CredencialMotivoId.Equals(para1) || n.CredencialMotivoId.Equals(para2));
                var resultMapeado = Mapper.Map<List<RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);

                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(_mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = _mensagem;
                }

                WpfHelp.ShowRelatorio(reportDoc);
            }

        }

        #endregion

        #region Métodos de Termos Credenciais 

        public void CarregarCredencialViasAdicionais(int report, int status, int periodo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                mensagemComplemento = "AEROPORTO INTERNACIONAL DE PORTO ALEGRE";

                // objeto com o filtro de parâmetros da consulta
                Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();

                // busca o arquivo TermoViaAdicionalCredencial_19.rpt
                var termo = _relatorioGerencialServiceService.BuscarPelaChave(report);

                //Quando o período for definido
                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                        colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "o perído de  " + dataIni + " a " + dataFim + "";
                    }
                }
                else
                {
                    colaboradorCredencial.Emissao = DateTime.Now.AddDays(-periodo);
                    colaboradorCredencial.EmissaoFim = DateTime.Now;
                    mensagemPeriodo = "o perído de  " + periodo.ToString() + " dias";
                }
                colaboradorCredencial.Periodo = periodo;
                colaboradorCredencial.CredencialMotivoId = 0;


                mensagem = "Durante " + mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", emitiu as seguintes credenciais: ";


                //Faz a busca do registros de colaboradores credenciais vias adicionais:  2 - segunda e 3 - terceira
                var result = objColaboradorCredencial.ListarColaboradorCredencialViaAdicionaisView(colaboradorCredencial).Where(n => n.CredencialMotivoId == 2 || n.CredencialMotivoId == 3);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\TermoViasAdicionaisCredenciais_19.rpt";

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

        public void CarregarCredencialConcedidas(int report, int status, int periodo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                mensagemComplemento = "AEROPORTO INTERNACIONAL DE PORTO ALEGRE";

                // objeto com o filtro de parâmetros da consulta
                Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();

                // busca o arquivo TermoConcessaoCredenciais_11.rpt
                var termo = _relatorioGerencialServiceService.BuscarPelaChave(report);

                //Quando o período for definido
                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                        colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "o perído de  " + dataIni + " a " + dataFim + "";
                    }
                }
                else
                {
                    colaboradorCredencial.Emissao = DateTime.Now.AddDays(-periodo);
                    colaboradorCredencial.EmissaoFim = DateTime.Now;
                    mensagemPeriodo = "o perído de  " + periodo.ToString() + " dias";
                }
                colaboradorCredencial.Periodo = periodo;
                colaboradorCredencial.CredencialStatusId = 1;

                mensagem = "Durante " + mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", concedeu as seguintes credenciais: ";

                //Faz a busca do registros de colaboradores credenciais concedidas 
                var result = objColaboradorCredencial.ListarColaboradorCredencialConcedidasView(colaboradorCredencial);

                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                //Busca o layout do relatório (arquivo .rpt) no banco de dados
                //var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);
                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\TermoConcessaoCredenciais_11.rpt";

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

        public void CarregarCredencialInativas(int report, int status, int periodo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                string arquivoTermo = string.Empty;

                mensagemComplemento = "AEROPORTO INTERNACIONAL DE PORTO ALEGRE";
                // objeto com o filtro de parâmetros da consulta
                Domain.EntitiesCustom.RelColaboradoresCredenciaisView colaboradorCredencial = new Domain.EntitiesCustom.RelColaboradoresCredenciaisView();

                switch (report)
                {
                    case 15:
                        verbo = "indeferiu";
                        colaboradorCredencial.CredencialMotivoId = 12; // 12 - id motivo indeferidas
                        arquivoTermo = "TermoIndeferimentoCredenciais_15.rpt";
                        // busca o arquivo Termo de credencial ".rpt"
                        // arquivoTermo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                        break;
                    case 13:
                        verbo = "cancelou";
                        colaboradorCredencial.CredencialMotivoId = 15; // 15 - id motivo canceladas
                        arquivoTermo = "TermoCancelamentoCredenciais_13.rpt";
                        // busca o arquivo Termo de credencial ".rpt"
                        // arquivoTermo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                        break;
                    case 17:
                        verbo = "destruiu";
                        colaboradorCredencial.CredencialMotivoId = 13; // 13 - id motivo destruídas
                        arquivoTermo = "TermoDestruicaoCredenciais_17.rpt";
                        // busca o arquivo Termo de credencial ".rpt"
                        // arquivoTermo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                        break;
                }

                //Quando o período for definido
                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                        colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "o perído de  " + dataIni + " a " + dataFim + "";
                    }
                }
                else
                {
                    colaboradorCredencial.Emissao = DateTime.Now.AddDays(-periodo);
                    colaboradorCredencial.EmissaoFim = DateTime.Now;
                    mensagemPeriodo = "o perído de  " + periodo.ToString() + " dias";
                }

                colaboradorCredencial.Periodo = periodo;
                colaboradorCredencial.CredencialStatusId = 2; // status desativado

                mensagem = "Durante " + mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", " + verbo + " as seguintes credenciais: ";

                //Faz a busca do registros de colaboradores credenciais status 2 - inativas
                var result = objColaboradorCredencial.ListarColaboradorCredencialInvalidasView(colaboradorCredencial).Where(n => n.CredencialStatusId == 2 && n.CredencialMotivoId == colaboradorCredencial.CredencialStatusId);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                //Busca o layout do relatório (arquivo .rpt) no banco de dados
                //var relatorioGerencial = _relatorioGerencialServiceService.BuscarPelaChave(1);
                //byte[] arrayFile = Convert.FromBase64String(relatorioGerencial.ArquivoRpt);

                var fileName = @"C:\projetos\credenciamento_homologa1\IMOD.CredenciamentoDeskTop\Relatorio\" + arquivoTermo;

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
    }
}
