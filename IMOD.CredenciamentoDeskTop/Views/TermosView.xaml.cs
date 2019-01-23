using System;
using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para TermosView.xam
    /// </summary>
    public partial class TermosView : UserControl
    {
        private PopupFiltrosTermos PopupFiltrosTermos;
        public TermosView()
        {
            InitializeComponent();
        }

        private void ButtonTermoConcessaoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //11_TermoConcessaoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(11, 1, true);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ButtonTermoIndeferimentoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //15_TermoIndeferimentoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(15, 5, true);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ButtonTermoCancelamentoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //13_TermoCancelamentoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(13, 2, true);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ButtonTermoDestruicaoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //17_TermoDestruicaoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(17, 3, true);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ButtonTermoViasAdicionaisCredenciaisClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //19_TermoViaAdicionalCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(19, 1, true);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }


        private void ButtonTermoConcessaoAutorizacoesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //12_TermoConcessaoAutorizacao.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(12, 1, false);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ButtonTermoIndeferimentoAutorizacoesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //16_TermoIndeferimentoAutorizacao.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(16, 5, false);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ButtonTermoCancelamentoAutorizacoesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //14_TermoCancelamentoAutorizacao.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(14, 2, false);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ButtonTermoDestruicaoAutorizacoesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //18_TermoDestruicaoAutorizacao.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(18, 3, false);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void ButtonTermoViasAdicionaisAutorizacoesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //20_TermoViaAdicionalAutorizacao.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(20, 1, false);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
    }
}
