// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using IMOD.Application.Interfaces;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Infra.Servicos;
using Microsoft.Expression.Encoder;

#endregion

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    ///     Interaction logic for PopupAutorizacao.xaml
    /// </summary>
    public partial class PopupAutorizacao : Window
    {
        private VeiculosCredenciaisView _entity;
        private readonly ReportDocument _report;
        private IVeiculoCredencialService _service;
        private LayoutCracha _layoutCracha;
        private bool _firstPage;
       

        public PopupAutorizacao(ReportDocument reportDocument, IVeiculoCredencialService service, VeiculosCredenciaisView entity, LayoutCracha layoutCracha)
        {
            InitializeComponent();
            try
            {
                _report = reportDocument;
                _service = service;
                _entity = entity;
                _layoutCracha = layoutCracha;

                GenericReportViewer.Background = Brushes.Transparent;
                GenericReportViewer.ShowSearchTextButton = false;
                GenericReportViewer.ShowExportButton = false;
                GenericReportViewer.ShowCopyButton = false;
                GenericReportViewer.ShowRefreshButton = false;
                GenericReportViewer.ShowToggleSidePanelButton = false; 
                // GenericReportViewer.ShowToolbar = false;
                GenericReportViewer.ShowOpenFileButton = false;
                GenericReportViewer.ShowLogo = false;
                GenericReportViewer.ViewerCore.Zoom (150);
                GenericReportViewer.ViewerCore.ReportSource = reportDocument;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
            MouseDown += Window_MouseDown;
        }

        #region  Metodos

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ImprimirCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //A impressão foi realizada corretamente?
                DialogResult impressaoRealizadaResult;
                DialogResult reImpressaoResult;
                DialogResult podeCobrarResult; 
                var impressaoCorreta = true;

                Imprimir();

                impressaoRealizadaResult = WpfHelp.MboxDialogYesNo("A impressão foi corretamente realizada?", true);
                impressaoCorreta = impressaoRealizadaResult == System.Windows.Forms.DialogResult.Yes;

                if (!impressaoCorreta)
                {
                    reImpressaoResult = WpfHelp.MboxDialogYesNo("Deseja imprimir mais uma vez?", true);
                    if (reImpressaoResult != System.Windows.Forms.DialogResult.Yes) return;

                    //Re imprimir
                    Imprimir();

                    impressaoRealizadaResult = WpfHelp.MboxDialogYesNo("A impressão foi corretamente realizada?", true);
                    impressaoCorreta = impressaoRealizadaResult == System.Windows.Forms.DialogResult.Yes;
                }

                //Sendo a impressao realizada corretamente, então, solicitar autorização de cobrança
                if (impressaoCorreta)
                {
                    //Registrar a data da emissão da autorização..
                    var n1 = Mapper.Map<VeiculoCredencial>(_entity);
                    n1.Emissao = DateTime.Today.Date;
                    n1.Impressa = true;
                    _service.Alterar(n1);
                    //Passar dados para atualizar entidade 
                    _entity.Emissao = n1.Emissao;

                    podeCobrarResult = WpfHelp.MboxDialogYesNo($"Autoriza a cobrança pela impressão no valor de {$"{_layoutCracha.Valor:C} ?"}", true);
                    var impressaoCobrar = podeCobrarResult == System.Windows.Forms.DialogResult.Yes;

                    _service.ImpressaoCredencial.Criar(new VeiculoCredencialimpressao
                    {
                        VeiculoCredencialId = _entity.VeiculoCredencialId,
                        Cobrar = impressaoCobrar,
                        DataImpressao = DateTime.Today.Date,
                        Valor = _layoutCracha.Valor

                    });
 
                    this.Close();

                }

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
             
        }

        private void Imprimir()
        {

            var dialog1 = new PrintDialog();
            dialog1.AllowSomePages = true;
            dialog1.AllowPrintToFile = false;

            if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int copies = dialog1.PrinterSettings.Copies;
                var fromPage = dialog1.PrinterSettings.FromPage;
                var toPage = dialog1.PrinterSettings.ToPage;
                var collate = dialog1.PrinterSettings.Collate;

                _report.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName;
                _report.PrintToPrinter(copies, collate, fromPage, toPage);
            }

            dialog1.Dispose();
        }

        private void ChangePage_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_firstPage)
                {
                    GenericReportViewer.ViewerCore.ShowFirstPage();
                    _firstPage = false;
                }
                else
                {
                    GenericReportViewer.ViewerCore.ShowLastPage();
                    _firstPage = true;
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion
    }
}