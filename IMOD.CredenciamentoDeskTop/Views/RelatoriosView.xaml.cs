using System;
using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para RelatoriosView.xam
    /// </summary>
    public partial class RelatoriosView : UserControl
    {
        private PopUpFiltrosAutorizacoesViasAdicionais popupFiltrosAutorizacoesViasAdicionais;
        private PopUpFiltrosCredenciaisViasAdicionais popupFiltrosCredenciaisViasAdicionais;
        private PopupFiltrosTermos popupFiltrosTermos;
        private PopUpFiltrosImpressoes popupFiltrosImpressoes;
        private PopUpFiltrosCredenciaisPorArea popupFiltrosCredenciaisPorArea;
        private PopUpFiltrosCredenciaisPorEmpresa popupFiltrosCredenciaisPorEmpresa;
        private PopUpFiltrosCredenciaisInvalidas popupFiltrosCredenciaisInvalidas;
        private PopUpFiltrosAutorizacoesInvalidas popupFiltrosAutorizacoesInvalidas;
        private PopUpFiltrosCredenciais popupFiltrosCredenciais;
        private PopUpFiltrosAutorizacoes PopUpFiltrosAutorizacoes;
        private PopUpFiltrosCredenciaisDestruidas popupFiltrosCredenciaisDestruidas;
        private PopUpFiltrosAutorizacoesDestruidas popupFiltrosAutorizacoesDestruidas;
        private PopUpFiltrosCredenciaisExtraviadas popupFiltrosCredenciaisExtraviadas;
        private PopUpFiltrosAutorizacoesExtraviadas popupFiltrosAutorizacaoExtraviadas;

        public RelatoriosView()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            ((RelatoriosViewModel)DataContext).CarregaColecaoRelatorios();
        }

        #region Comando dos Botoes

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button _botao = (Button)sender;
            ((RelatoriosViewModel)DataContext).OnAbrirRelatorioCommand(_botao.Tag.ToString());
        }

        private void ButtonCredenciais_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosCredenciais = new PopUpFiltrosCredenciais();
                popupFiltrosCredenciais.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }
        }
        private void ButtonAutorizacoes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PopUpFiltrosAutorizacoes = new PopUpFiltrosAutorizacoes();
                PopUpFiltrosAutorizacoes.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoFiltroPorEmpresa_bt_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                popupFiltrosCredenciaisPorEmpresa = new PopUpFiltrosCredenciaisPorEmpresa();
                popupFiltrosCredenciaisPorEmpresa.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }


        }

        private void BotaoFiltroPorArea_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosCredenciaisPorArea = new PopUpFiltrosCredenciaisPorArea();
                popupFiltrosCredenciaisPorArea.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoCredenciaisInvalidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosCredenciaisInvalidas = new PopUpFiltrosCredenciaisInvalidas();
                popupFiltrosCredenciaisInvalidas.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoAutorizacoesInvalidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosAutorizacoesInvalidas = new PopUpFiltrosAutorizacoesInvalidas();
                popupFiltrosAutorizacoesInvalidas.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoImpressoesCredenciaisClick(object sender, RoutedEventArgs e)
        {
            try
            {

                popupFiltrosImpressoes = new PopUpFiltrosImpressoes();
                popupFiltrosImpressoes.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoImpressoesAutorizacoesClick(object sender, RoutedEventArgs e)
        {
            try
            {

                popupFiltrosImpressoes = new PopUpFiltrosImpressoes();
                popupFiltrosImpressoes.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }

        }

        private void ButtonRelatorioViasAdicionaisCredenciaisClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosCredenciaisViasAdicionais = new PopUpFiltrosCredenciaisViasAdicionais();
                popupFiltrosCredenciaisViasAdicionais.ShowDialog();
            }
            catch (Exception ex)
            { 
                Utils.TraceException(ex);
                throw;
            }
        }

        private void ButtonRelatorioViasAdicionaisAutorizacoesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosAutorizacoesViasAdicionais = new PopUpFiltrosAutorizacoesViasAdicionais();
                popupFiltrosAutorizacoesViasAdicionais.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoCredenciaisDestruidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosCredenciaisDestruidas = new PopUpFiltrosCredenciaisDestruidas();
                popupFiltrosCredenciaisDestruidas.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoAutorizacoesDestruidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosAutorizacoesDestruidas = new PopUpFiltrosAutorizacoesDestruidas();
                popupFiltrosAutorizacoesDestruidas.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoCredenciaisExtraviadasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosCredenciaisExtraviadas = new PopUpFiltrosCredenciaisExtraviadas();
                popupFiltrosCredenciaisExtraviadas.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        private void BotaoAutorizacaoExtraviadasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltrosAutorizacaoExtraviadas = new PopUpFiltrosAutorizacoesExtraviadas();
                popupFiltrosAutorizacaoExtraviadas.ShowDialog();
            }
            catch (Exception ex) 
            {
                Utils.TraceException(ex);
                throw;
            }
        }

        #endregion

    }
}
