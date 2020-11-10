// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradoresAnexosView.xam
    /// </summary>
    public partial class ColaboradoresObservacoesView : UserControl
    {
        private readonly ColaboradoresObservacoesViewModel _viewModel;

        public ColaboradoresObservacoesView()
        {
            try
            {
                InitializeComponent();
                _viewModel = new ColaboradoresObservacoesViewModel();
                DataContext = _viewModel;
            }
            catch (Exception ex)
            {
                //WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
        }


        #endregion

        private void ListaObservacoes_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_viewModel.Entity == null) return;

                if (_viewModel.Entity.Resolvido)
                {
                    this.btnAceitar.Visibility = Visibility.Collapsed;
                    this.btnInteragir.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.btnAceitar.Visibility = Visibility.Visible;
                    this.btnInteragir.Visibility = Visibility.Visible;
                }

                _viewModel.atualizarGrid(_viewModel.Entity.ColaboradorId);
                //_viewModel.BuscarObservacoes(_viewModel.Entity.ColaboradorId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnInteragir_Click(object sender, RoutedEventArgs e)
        {
            this.Resposta_tb.Visibility = Visibility.Visible;
            this.Resposta_lb.Visibility = Visibility.Visible;

            this.Resposta_tb.Text = "";

            //this.ObservacaoIntegra_tb.IsReadOnly = true;

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Resposta_tb.Visibility = Visibility.Collapsed;
            this.Resposta_lb.Visibility = Visibility.Collapsed;

            this.Observacao_tb.Visibility = Visibility.Collapsed;
            this.ObservacaoIntegra_tb.Visibility = Visibility.Visible;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            this.Resposta_tb.Visibility = Visibility.Collapsed;
            this.Resposta_lb.Visibility = Visibility.Collapsed;

            this.Observacao_tb.Visibility = Visibility.Collapsed;
            this.ObservacaoIntegra_tb.Visibility = Visibility.Visible;
        }
        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            this.Observacao_tb.Visibility = Visibility.Visible;
            this.ObservacaoIntegra_tb.Visibility = Visibility.Collapsed;

        }
    }
}