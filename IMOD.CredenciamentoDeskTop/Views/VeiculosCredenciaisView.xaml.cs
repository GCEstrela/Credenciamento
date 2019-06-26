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
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para VeiculoCredencialView.xam
    /// </summary>
    public partial class VeiculosCredenciaisView : UserControl
    {
        private readonly VeiculosCredenciaisViewModel _viewModel;

        #region Inicializacao

        public VeiculosCredenciaisView()
        {
            InitializeComponent();
            _viewModel = new VeiculosCredenciaisViewModel();
            DataContext = _viewModel;
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.VeiculoView entity, VeiculoViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados (entity, viewModelParent);
        }

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.VeiculoEmpresa == null) return;
            _viewModel.CarregaColecaoLayoutsCrachas ((int) _viewModel.VeiculoEmpresa.EmpresaId,2);
            _viewModel.ObterValidade();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //EmpresaVinculo_cb.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
            cmbEmpresaVinculo.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
        }
        private void OnAlterarStatus_SelectonChanged(object sender, SelectionChangedEventArgs e)
        {

            btnImprimirCredencial.IsEnabled = _viewModel.HabilitaImpressao;

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
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Validade", "Data inválida");
            }
        }

        #endregion

        private void CmbMotivacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbCredencialStatus.SelectedItem != null && cmbMotivacao.SelectedItem != null)
            {
                if (((CredencialStatus)cmbCredencialStatus.SelectedItem).CredencialStatusId == 2
                                      && ((CredencialMotivo)cmbMotivacao.SelectedItem).CredencialMotivoId > 0)
                {
                    switch (((CredencialMotivo)cmbMotivacao.SelectedItem).CredencialMotivoId)
                    {
                        case 6:
                        case 8:
                        case 15:
                            chkDevolucaoMotivo.Content = DevoluçãoCredencial.Devolucao.Descricao();
                            chkDevolucaoMotivo.Visibility = Visibility.Visible;
                            break;
                        case 9:
                        case 10:
                            chkDevolucaoMotivo.Content = DevoluçãoCredencial.EntregaBO.Descricao();
                            chkDevolucaoMotivo.Visibility = Visibility.Visible;
                            break;
                        default:
                            chkDevolucaoMotivo.IsChecked = false;
                            chkDevolucaoMotivo.Content = String.Empty;
                            chkDevolucaoMotivo.Visibility = Visibility.Hidden;
                            break;
                    }
                }
                else
                {
                    chkDevolucaoMotivo.IsChecked = false;
                    chkDevolucaoMotivo.Content = String.Empty;
                    chkDevolucaoMotivo.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}