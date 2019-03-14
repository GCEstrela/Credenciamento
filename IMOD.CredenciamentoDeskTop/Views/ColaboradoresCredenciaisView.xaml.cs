// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradorCredencialView.xam
    /// </summary>
    public partial class ColaboradoresCredenciaisView : UserControl
    {
        private readonly ColaboradoresCredenciaisViewModel _viewModel;

        #region Inicializacao

        public ColaboradoresCredenciaisView()
        {
            InitializeComponent();
            _viewModel = new ColaboradoresCredenciaisViewModel();
            DataContext = _viewModel; 
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados (entity, viewModelParent); 
        }

       

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.ColaboradorEmpresa == null) return;
            _viewModel.ListarCracha (_viewModel.ColaboradorEmpresa.EmpresaId);
            _viewModel.ObterValidade();
            _viewModel.CarregarCaracteresColete(_viewModel.ColaboradorEmpresa);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEmpresaVinculo.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;

            
        }
         

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OnFormatData_LostFocus(object sender, RoutedEventArgs e)
        {

            if (_viewModel.Entity == null) return;
            try
            {
                var str = txtDtValidade.Text;
                if (string.IsNullOrWhiteSpace(str)) return;
                txtDtValidade.Text = str.FormatarData();
                //_viewModel.Entity.Validade = Convert.ToDateTime (str.FormatarData());
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Validade", "Data inválida");
            }
          
            
        }

        #endregion

        private void OnAlterarStatus_SelectonChanged(object sender, SelectionChangedEventArgs e)
        {
            
            btnImprimirCredencial.IsEnabled = _viewModel.HabilitaImpressao;

            if ((CredencialStatus)cmbCredencialStatus.SelectedItem != null)
            {
                _viewModel.HabilitaCheckDevolucao(((CredencialStatus)cmbCredencialStatus.SelectedItem).CredencialStatusId, 0);
                chkDevolucaoMotivo.IsChecked = _viewModel.IsCheckDevolucao;
                chkDevolucaoMotivo.Visibility = _viewModel.VisibilityCheckDevolucao;
                chkDevolucaoMotivo.Content = _viewModel.TextCheckDevolucao;
            }
        }

        private void CmbMotivacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                    if (cmbCredencialStatus.SelectedItem != null && cmbMotivacao.SelectedItem != null)
                    {
                        _viewModel.HabilitaCheckDevolucao(((CredencialStatus)cmbCredencialStatus.SelectedItem).CredencialStatusId, ((CredencialMotivo)cmbMotivacao.SelectedItem).CredencialMotivoId);
                        chkDevolucaoMotivo.IsChecked = _viewModel.IsCheckDevolucao;
                        chkDevolucaoMotivo.Visibility = _viewModel.VisibilityCheckDevolucao;
                        chkDevolucaoMotivo.Content = _viewModel.TextCheckDevolucao;
                    }
        }
    }
}