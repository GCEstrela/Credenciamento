using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        //private ObservableCollection<RelatorioView> _Relatorios;

        //private RelatorioView _RelatorioSelecionado;

       // PopupMensagem _PopupSalvando;

        //private int _selectedIndex;

        //private int _selectedIndexTemp = 0;

        //private bool _atualizandoFoto = false;

        //private BitmapImage _Waiting;

        private const string consNomeArquivoEmpresaOperadora = "logoEmpresaOperadora.png";

        //private string formula;
        //private string _mensagem;
        private string verbo;

        private readonly IRelatorioGerencialService _relatorioGerencialServiceService = new RelatorioGerencialService();

        private RelatoriosGerenciais termo = new RelatoriosGerenciais(); 

        private readonly IColaboradorCredencialService objColaboradorCredencial = new ColaboradorCredencialService();

        private readonly IConfiguraSistemaService objConfiguraSistema = new ConfiguraSistemaService();

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
        public void OnFiltrosTermosAutorizacoesCommand(int report, int status, int periodo, string dataIni, string dataFim)
        {
            switch (report)
            {
                case 12:
                    this.CarregarAutorizacaoConcedidas(report, status, periodo, dataIni, dataFim);
                    break;
                case 14:
                case 16:
                case 18:
                    this.CarregarAutorizacaoInativas(report, status, periodo, dataIni, dataFim);
                    break;
                default:
                    this.CarregarAutorizacaoViasAdicionais(report, status, periodo, dataIni, dataFim);
                    break;
            }
        }

        #endregion

        #region Métodos de Termos Credenciais - COLABORADOR

        public void CarregarCredencialViasAdicionais(int report, int status, int periodo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                mensagemComplemento = "AEROPORTO ";

                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();
                colaboradorCredencial.Impressa = true;
                var configSistema = objConfiguraSistema.BuscarPelaChave(1);

                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                        colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "Durante o período de  " + dataIni + " a " + dataFim + "";
                    }
                }
                else
                {
                    colaboradorCredencial.Emissao = DateTime.Now.AddDays(-periodo).Date;
                    colaboradorCredencial.EmissaoFim = DateTime.Now;
                    if (periodo > 0)
                    {
                        mensagemPeriodo = "Durante o período de  " + periodo.ToString() + " dias";
                    }
                    else
                    {
                        mensagemPeriodo = "Hoje";
                    }
                }
                colaboradorCredencial.Periodo = periodo;
                colaboradorCredencial.CredencialMotivoId = 0;

                mensagemComplemento = !string.IsNullOrEmpty(configSistema.NomeAeroporto) ? configSistema.NomeAeroporto?.Trim() : mensagemComplemento;
                mensagem = mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", emitiu as seguintes vias adicionais : ";

                var termo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                if (termo == null || termo.ArquivoRpt == null || String.IsNullOrEmpty(termo.ArquivoRpt)) return;

                var result = objColaboradorCredencial.ListarColaboradorCredencialViaAdicionaisView(colaboradorCredencial).Where(n => n.CredencialMotivoId == 2 || n.CredencialMotivoId == 3);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(termo.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, termo.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem)) 
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem; 
                }
                reportDoc.Refresh();

                WpfHelp.ShowRelatorio(CarregaLogoMarcaEmpresa(reportDoc));

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
                mensagemComplemento = "AEROPORTO ";

                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();

                var termo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                if (termo == null || termo.ArquivoRpt == null || String.IsNullOrEmpty(termo.ArquivoRpt)) return;
                colaboradorCredencial.Impressa = true;
                var configSistema = objConfiguraSistema.BuscarPelaChave(1);

                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        colaboradorCredencial.Emissao = DateTime.Parse(dataIni);
                        colaboradorCredencial.EmissaoFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "Durante o período de  " + dataIni + " a " + dataFim + "";
                    }
                }
                else
                {
                    colaboradorCredencial.Emissao = DateTime.Now.AddDays(-periodo).Date;
                    colaboradorCredencial.EmissaoFim = DateTime.Now;
                    if (periodo > 0)
                    {
                        mensagemPeriodo = "Durante o período de  " + periodo.ToString() + " dias";
                    }
                    else
                    {
                        mensagemPeriodo = "Hoje";
                    }
                }
                colaboradorCredencial.Periodo = periodo;

                mensagemComplemento = !string.IsNullOrEmpty(configSistema.NomeAeroporto) ? configSistema.NomeAeroporto?.Trim() : mensagemComplemento;
                mensagem = mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", concedeu as seguintes credenciais: ";

                var result = objColaboradorCredencial.ListarColaboradorCredencialConcedidasView(colaboradorCredencial);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());
                byte[] arrayFile = Convert.FromBase64String(termo.ArquivoRpt); 
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, termo.Nome); 
                reportDoc.SetDataSource(resultMapeado); 

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                reportDoc.Refresh();

                WpfHelp.ShowRelatorio(CarregaLogoMarcaEmpresa(reportDoc));

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

                mensagemComplemento = "AEROPORTO ";
                Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais colaboradorCredencial = new Domain.EntitiesCustom.FiltroReportColaboradoresCredenciais();

                var termo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                if (termo == null || termo.ArquivoRpt == null || String.IsNullOrEmpty(termo.ArquivoRpt)) return;
                var configSistema = objConfiguraSistema.BuscarPelaChave(1);
                //lista de motivos de credenciais destruidas
                List<int> statusList = new List<int>() { 6, 8, 15 };

                switch (report)
                {
                    case 15:
                        verbo = "indeferiu";
                        colaboradorCredencial.CredencialMotivoId = 12; // 12 - id motivo indeferidas
                        break;
                    case 13:
                        verbo = "cancelou";
                        colaboradorCredencial.CredencialMotivoId = 15; // 15 - id motivo canceladas
                        colaboradorCredencial.flaTodasDevolucaoEntregaBO = null;
                        break;
                    case 17:
                        verbo = "destruiu";
                        colaboradorCredencial.Impeditivo = true;
                        break;
                }

                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        colaboradorCredencial.DataStatus = DateTime.Parse(dataIni); 
                        colaboradorCredencial.DataStatusFim = DateTime.Parse(dataFim); 
                        mensagemPeriodo = "Durante o período de  " + dataIni + " a " + dataFim + ""; 
                    }
                }
                else
                {
                    colaboradorCredencial.DataStatus = DateTime.Now.AddDays(-periodo).Date;
                    colaboradorCredencial.DataStatusFim = DateTime.Now;
                    if (periodo > 0)
                    {
                        mensagemPeriodo = "Durante o período de  " + periodo.ToString() + " dias";
                    }
                    else
                    {
                        mensagemPeriodo = "Hoje";
                    }
                }

                colaboradorCredencial.Periodo = periodo;
                colaboradorCredencial.CredencialStatusId = 2; // status desativado
                colaboradorCredencial.Impressa = true;

                mensagemComplemento = !string.IsNullOrEmpty(configSistema.NomeAeroporto) ? configSistema.NomeAeroporto?.Trim() : mensagemComplemento;
                mensagem = mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", " + verbo + " as seguintes credenciais: ";

                var result = objColaboradorCredencial.ListarColaboradorCredencialInvalidasView(colaboradorCredencial).Where(n => n.CredencialStatusId == 2);
                var resultMapeado = Mapper.Map<List<Views.Model.RelColaboradoresCredenciaisView>>(result.OrderByDescending(n => n.ColaboradorCredencialId).ToList());

                if (17.Equals(report))
                {
                    resultMapeado = resultMapeado.Where(r => statusList.Contains(r.CredencialMotivoId)).ToList();
                }

                byte[] arrayFile = Convert.FromBase64String(termo.ArquivoRpt); 
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, termo.Nome); 
                reportDoc.SetDataSource(resultMapeado); 
                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                WpfHelp.ShowRelatorio(CarregaLogoMarcaEmpresa(reportDoc));

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #region Métodos de Termos Autorizações - VEICULOS

        public void CarregarAutorizacaoViasAdicionais(int report, int status, int periodo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                mensagemComplemento = "AEROPORTO ";

                Domain.EntitiesCustom.FiltroReportVeiculoCredencial filtroAutorizacao = new Domain.EntitiesCustom.FiltroReportVeiculoCredencial();
                filtroAutorizacao.Impressa = true;

                var termo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                if (termo == null || termo.ArquivoRpt == null || String.IsNullOrEmpty(termo.ArquivoRpt)) return;

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);

                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        filtroAutorizacao.Emissao = DateTime.Parse(dataIni);
                        filtroAutorizacao.EmissaoFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "Durante o período de  " + dataIni + " a " + dataFim + "";
                    }
                }
                else
                {
                    filtroAutorizacao.Emissao = DateTime.Now.AddDays(-periodo).Date;
                    filtroAutorizacao.EmissaoFim = DateTime.Now;
                    if (periodo > 0)
                    {
                        mensagemPeriodo = "Durante o período de  " + periodo.ToString() + " dias";
                    }
                    else
                    {
                        mensagemPeriodo = "Hoje";
                    }
                }
                filtroAutorizacao.Periodo = periodo;
                filtroAutorizacao.CredencialMotivoId = 0;

                mensagemComplemento = !string.IsNullOrEmpty(configSistema.NomeAeroporto) ? configSistema.NomeAeroporto?.Trim() : mensagemComplemento;
                mensagem = mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", emitiu as seguintes vias adicionais : ";

                var result = objVeiculoCredencial.ListarVeiculoCredencialViaAdicionaisView(filtroAutorizacao).Where(n => n.CredencialMotivoId == 2 || n.CredencialMotivoId == 3);
                var resultMapeado = Mapper.Map<List<Views.Model.RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                
                byte[] arrayFile = Convert.FromBase64String(termo.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, termo.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }
                reportDoc.Refresh();

                WpfHelp.ShowRelatorio(CarregaLogoMarcaEmpresa(reportDoc));
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregarAutorizacaoConcedidas(int report, int status, int periodo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                mensagemComplemento = "AEROPORTO ";

                Domain.EntitiesCustom.FiltroReportVeiculoCredencial filtroAutorizacao = new Domain.EntitiesCustom.FiltroReportVeiculoCredencial();
                filtroAutorizacao.Impressa = true;

                var termo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                if (termo == null || termo.ArquivoRpt == null || String.IsNullOrEmpty(termo.ArquivoRpt)) return;

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);

                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        filtroAutorizacao.Emissao = DateTime.Parse(dataIni);
                        filtroAutorizacao.EmissaoFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "Durante o período de  " + dataIni + " a " + dataFim + "";
                    }
                }
                else
                {
                    filtroAutorizacao.Emissao = DateTime.Now.AddDays(-periodo).Date;
                    filtroAutorizacao.EmissaoFim = DateTime.Now;
                    if (periodo > 0)
                    {
                        mensagemPeriodo = "Durante o período de  " + periodo.ToString() + " dias";
                    }
                    else
                    {
                        mensagemPeriodo = "Hoje";
                    }
                }
                filtroAutorizacao.Periodo = periodo;

                mensagemComplemento = !string.IsNullOrEmpty(configSistema.NomeAeroporto) ? configSistema.NomeAeroporto?.Trim() : mensagemComplemento;
                mensagem = mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", concedeu as seguintes autorizações: ";

                var result = objVeiculoCredencial.ListarVeiculoCredencialConcedidasView(filtroAutorizacao); 
                var resultMapeado = Mapper.Map<List<Views.Model.RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());

                byte[] arrayFile = Convert.FromBase64String(termo.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, termo.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                reportDoc.Refresh();

                WpfHelp.ShowRelatorio(CarregaLogoMarcaEmpresa(reportDoc));

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        public void CarregarAutorizacaoInativas(int report, int status, int periodo, string dataIni, string dataFim)
        {
            try
            {
                string mensagem = string.Empty;
                string mensagemComplemento = string.Empty;
                string mensagemPeriodo = string.Empty;
                string arquivoTermo = string.Empty;
                
                //lista de motivos de autorizações destruidas
                List<int> statusList = new List<int>() { 6, 8, 15 };


                mensagemComplemento = "AEROPORTO ";
                
                Domain.EntitiesCustom.FiltroReportVeiculoCredencial filtroAutorizacao = new Domain.EntitiesCustom.FiltroReportVeiculoCredencial();
                filtroAutorizacao.Impressa = true;
                var termo = _relatorioGerencialServiceService.BuscarPelaChave(report);
                if (termo == null || termo.ArquivoRpt == null || String.IsNullOrEmpty(termo.ArquivoRpt)) return;

                var configSistema = objConfiguraSistema.BuscarPelaChave(1);

                switch (report)
                {
                    case 16:
                        verbo = "indeferiu";
                        filtroAutorizacao.CredencialMotivoId = 12; // 12 - id motivo indeferidas 
                        break;
                    case 14:
                        verbo = "cancelou";
                        filtroAutorizacao.CredencialMotivoId = 15; // 15 - id motivo canceladas 
                        break;
                    case 18:
                        verbo = "destruiu";
                        filtroAutorizacao.Impeditivo = true;
                        //filtroAutorizacao.CredencialMotivoId = 6;
                        //filtroAutorizacao.CredencialMotivoId1 = 8;
                        //filtroAutorizacao.CredencialMotivoId2 = 15;
                        break;
                }

                if (periodo > 30)
                {
                    if (!(dataIni.Equals(string.Empty) || dataFim.Equals(string.Empty)))
                    {
                        filtroAutorizacao.DataStatus = DateTime.Parse(dataIni);
                        filtroAutorizacao.DataStatusFim = DateTime.Parse(dataFim);
                        mensagemPeriodo = "Durante o período de  " + dataIni + " a " + dataFim + "";
                    }
                }
                else
                {
                    filtroAutorizacao.DataStatus = DateTime.Now.AddDays(-periodo).Date; 
                    filtroAutorizacao.DataStatusFim = DateTime.Now;
                    if (periodo > 0)
                    {
                        mensagemPeriodo = "Durante o período de  " + periodo.ToString() + " dias";
                    }
                    else
                    {
                        mensagemPeriodo = "Hoje";
                    }
                }

                filtroAutorizacao.Periodo = periodo;
                filtroAutorizacao.CredencialStatusId = 2;

                mensagemComplemento = !string.IsNullOrEmpty(configSistema.NomeAeroporto) ? configSistema.NomeAeroporto?.Trim() : mensagemComplemento;
                mensagem = mensagemPeriodo + " esse setor  de credenciamento do " + mensagemComplemento + ", " + verbo + " as seguintes autorizações: ";

                var result = objVeiculoCredencial.ListarVeiculoCredencialInvalidasView(filtroAutorizacao).Where(n => n.CredencialStatusId == 2);
                var resultMapeado = Mapper.Map<List<Views.Model.RelVeiculosCredenciaisView>>(result.OrderByDescending(n => n.VeiculoCredencialId).ToList());
                if (18.Equals(report))
                {
                    resultMapeado = resultMapeado.Where(r => statusList.Contains(r.CredencialMotivoId)).ToList();
                }


                byte[] arrayFile = Convert.FromBase64String(termo.ArquivoRpt);
                var reportDoc = WpfHelp.ShowRelatorioCrystalReport(arrayFile, termo.Nome);
                reportDoc.SetDataSource(resultMapeado);

                if (!string.IsNullOrWhiteSpace(mensagem))
                {
                    TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                    txt.Text = mensagem;
                }

                reportDoc.Refresh();

                WpfHelp.ShowRelatorio(CarregaLogoMarcaEmpresa(reportDoc));
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        #endregion

        #region Métodos Privados

        private ReportDocument CarregaLogoMarcaEmpresa(ReportDocument reportDoc)
        {
            var configSistema = objConfiguraSistema.BuscarPelaChave(1);
            var tempArea = Path.GetTempPath();
            if (configSistema.EmpresaLOGO != null)
            {
                byte[] testeArquivo = Convert.FromBase64String(configSistema.EmpresaLOGO);
                System.IO.File.WriteAllBytes(tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora, testeArquivo);
                reportDoc.SetParameterValue("MarcaEmpresa", tempArea + Constantes.Constantes.consNomeArquivoEmpresaOperadora);
            }
            return reportDoc;
        }

        #endregion
    }
}
