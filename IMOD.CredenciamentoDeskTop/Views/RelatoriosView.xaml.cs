﻿using System;
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
        private PopUpFiltrosAutorizacoesViasAdicionais popupfiltrosautorizacoesviasadicionais;
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
                popupfiltroscredenciaisporempresa = new PopUpFiltrosCredenciaisPorEmpresa();
                popupfiltroscredenciaisporempresa.ShowDialog();
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
                popupfiltroscredenciaisporarea = new PopUpFiltrosCredenciaisPorArea();
                popupfiltroscredenciaisporarea.ShowDialog();
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
                popupfiltroscredenciaisinvalidas = new PopUpFiltrosCredenciaisInvalidas();
                popupfiltroscredenciaisinvalidas.ShowDialog();
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
                popupfiltrosautorizacoesinvalidas = new PopUpFiltrosAutorizacoesInvalidas();
                popupfiltrosautorizacoesinvalidas.ShowDialog();
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

                popupfiltrosimpressoes = new PopUpFiltrosImpressoes();
                popupfiltrosimpressoes.ShowDialog();
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

                popupfiltrosimpressoes = new PopUpFiltrosImpressoes();
                popupfiltrosimpressoes.ShowDialog();
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
                popupfiltroscredenciaisviasadicionais = new PopUpFiltrosCredenciaisViasAdicionais();
                popupfiltroscredenciaisviasadicionais.ShowDialog();
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
                popupfiltrosautorizacoesviasadicionais = new PopUpFiltrosAutorizacoesViasAdicionais();
                popupfiltrosautorizacoesviasadicionais.ShowDialog();
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
