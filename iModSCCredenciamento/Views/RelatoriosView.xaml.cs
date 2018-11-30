using iModSCCredenciamento.ViewModels;
using iModSCCredenciamento.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace iModSCCredenciamento.Views
{
    /// <summary>
    /// Interação lógica para RelatoriosView.xam
    /// </summary>
    public partial class RelatoriosView : UserControl
    {
        private PopUpFiltrosCredenciaisViasAdicionais popupfiltroscredenciaisviasadicionais;
        private PopupFiltrosTermos PopupFiltrosTermos;
        private PopUpFiltrosImpressoes popupfiltrosimpressoes;
        private PopUpFiltrosCredenciaisPorArea popupfiltroscredenciaisporarea;
        private PopUpFiltrosCredenciaisPorEmpresa popupfiltroscredenciaisporempresa;
        private PopUpFiltrosCredenciaisInvalidas popupfiltroscredenciaisinvalidas;
        private PopUpFiltrosAutorizacoesInvalidas popupfiltrosautorizacoesinvalidas;
        private PopUpFiltrosCredenciais popupFiltrosCredenciais;
        private PopUpFiltrosAutorizacoes PopUpFiltrosAutorizacoes;

        public RelatoriosView()
        {
            InitializeComponent();
            this.DataContext = new RelatoriosViewModel();
        }

        #region Comando dos Botoes
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button _botao = (Button)sender;
            ((RelatoriosViewModel)this.DataContext).OnAbrirRelatorioCommand(_botao.Tag.ToString());
        }


        #endregion


        private void ButtonCredenciais_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosCredenciais = new PopUpFiltrosCredenciais();
                popupFiltrosCredenciais.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonAutorizacoes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PopUpFiltrosAutorizacoes = new PopUpFiltrosAutorizacoes();
                PopUpFiltrosAutorizacoes.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void BotaoFiltroPorEmpresa_bt_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                popupfiltroscredenciaisporempresa = new PopUpFiltrosCredenciaisPorEmpresa();
                popupfiltroscredenciaisporempresa.ShowDialog();
            }
            catch (Exception)
            {

            }


        }

        private void BotaoFiltroPorArea_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popupfiltroscredenciaisporarea = new PopUpFiltrosCredenciaisPorArea();
                popupfiltroscredenciaisporarea.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void BotaoCredenciaisInvalidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupfiltroscredenciaisinvalidas = new PopUpFiltrosCredenciaisInvalidas();
                popupfiltroscredenciaisinvalidas.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void BotaoAutorizacoesInvalidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupfiltrosautorizacoesinvalidas = new PopUpFiltrosAutorizacoesInvalidas();
                popupfiltrosautorizacoesinvalidas.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void BotaoImpressoesCredenciaisClick(object sender, RoutedEventArgs e)
        {
            try
            {

                popupfiltrosimpressoes = new PopUpFiltrosImpressoes();
                popupfiltrosimpressoes.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void BotaoImpressoesAutorizacoesClick(object sender, RoutedEventArgs e)
        {
            try
            {

                popupfiltrosimpressoes = new PopUpFiltrosImpressoes();
                popupfiltrosimpressoes.ShowDialog();
            }
            catch (Exception)
            {

            }

        }
        //TODO: Mihai (30-10-2018)
        private void ButtonTermoConcessaoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //12_TermoConcessaoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(12, 1);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoIndeferimentoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //16_TermoIndeferimentoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(16, 5);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoCancelamentoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //14_TermoCancelamentoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(14, 2);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoDestruicaoCredenciaisClick(object sender, RoutedEventArgs e)
        {

            try
            {
                //18_TermoDestruicaoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(18, 3);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoViasAdicionaisCredenciaisClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //20_TermoViaAdicionalCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(20, 1);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonRelatorioViasAdicionaisCredenciaisClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //18_TermoDestruicaoCredencial.rpt
                popupfiltroscredenciaisviasadicionais = new PopUpFiltrosCredenciaisViasAdicionais();
                popupfiltroscredenciaisviasadicionais.ShowDialog();
            }
            catch (Exception)
            {

            }
        }
    }
}
