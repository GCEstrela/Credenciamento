﻿using System;
using System.Windows;
using System.Windows.Controls;
using iModSCCredenciamento.Windows;

namespace iModSCCredenciamento.Views
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
                //12_TermoConcessaoCredencial.rpt
                PopupFiltrosTermos = new PopupFiltrosTermos(12,1);
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
                PopupFiltrosTermos = new PopupFiltrosTermos(16,5);
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
                PopupFiltrosTermos = new PopupFiltrosTermos(14,2);
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
                PopupFiltrosTermos = new PopupFiltrosTermos(18,3);
                PopupFiltrosTermos.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonTermoViasAdicionaisCredenciaisClick(object sender, RoutedEventArgs e)
        {
            PopupFiltrosTermos = new PopupFiltrosTermos(20,1);
            PopupFiltrosTermos.ShowDialog();
        }
    }
}
