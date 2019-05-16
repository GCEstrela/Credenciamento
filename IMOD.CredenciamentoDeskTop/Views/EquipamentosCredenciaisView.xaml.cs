﻿#region

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
    ///     Interação lógica para VeiculoCredencialView.xam
    /// </summary>
    public partial class EquipamentosCredenciaisView : UserControl
    {
        private readonly EquipamentosCredenciaisViewModel _viewModel;

        #region Inicializacao

        public EquipamentosCredenciaisView()
        {
            InitializeComponent();
            _viewModel = new EquipamentosCredenciaisViewModel();
            DataContext = _viewModel;
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.VeiculoView entity, EquipamentosViewModel viewModelParent)
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

            if (StatusCredencial_cb.SelectedItem != null && cmbMotivacao.SelectedItem != null)
            {
                var statusId = ((CredencialStatus)StatusCredencial_cb.SelectedItem).CredencialStatusId;
                var motivoId = ((CredencialMotivo)cmbMotivacao.SelectedItem).CredencialMotivoId;

                _viewModel.HabilitaCheckDevolucao(statusId, motivoId);
                chkDevolucaoMotivo.IsChecked = _viewModel.IsCheckDevolucao; 
                chkDevolucaoMotivo.Visibility = _viewModel.VisibilityCheckDevolucao; 
                chkDevolucaoMotivo.Content = _viewModel.TextCheckDevolucao; 
            }
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
            
            if (StatusCredencial_cb.SelectedItem != null && cmbMotivacao.SelectedItem != null)
            {
                var statusId = ((CredencialStatus)StatusCredencial_cb.SelectedItem).CredencialStatusId;
                var motivoId = ((CredencialMotivo)cmbMotivacao.SelectedItem).CredencialMotivoId;

                _viewModel.HabilitaCheckDevolucao(statusId, motivoId);
                chkDevolucaoMotivo.IsChecked = _viewModel.IsCheckDevolucao;
                chkDevolucaoMotivo.Visibility = _viewModel.VisibilityCheckDevolucao;
                chkDevolucaoMotivo.Content = _viewModel.TextCheckDevolucao;
            }
        }
    }
}