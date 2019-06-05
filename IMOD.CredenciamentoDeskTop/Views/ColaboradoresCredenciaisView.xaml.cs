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
using IMOD.Domain.EntitiesCustom;

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
            //if (entity == null) return;
            _viewModel.AtualizarDados (entity, viewModelParent); 
        }

       

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.ColaboradorEmpresa == null) return;
            _viewModel.ListarCracha (_viewModel.ColaboradorEmpresa.EmpresaId);
            _viewModel.ObterValidade();
            _viewModel.CarregarCaracteresColete(_viewModel.ColaboradorEmpresa);

            
            if (_viewModel.ColaboradorEmpresa.ColaboradorId > 0 & _viewModel.ColaboradorEmpresa.EmpresaId > 0)
            {
                _viewModel.CarregarVinculosAtivos(_viewModel.ColaboradorEmpresa.ColaboradorId, _viewModel.ColaboradorEmpresa.EmpresaId);
            }

            //if (cmbEmpresaVinculo_cb.IsEnabled)
            //{
            //    _viewModel.HabilitaCriar(_viewModel.ColaboradorEmpresa,_viewModel);                
            //}
            //else
            //{
            //    _viewModel.Entity.ClearMessageErro();
            //}

        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEmpresaVinculo_cb.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
            cmbCredencialStatus.SelectionChanged += OnAlterarStatus_SelectonChanged;

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

        private void OnAlterarStatus_SelectonChanged(object sender, SelectionChangedEventArgs e)
        {

            btnImprimirCredencial.IsEnabled = true;
            _viewModel.ContentImprimir = "Imprimir Credencial";
            if (_viewModel.HabilitaImpressao)
            {
                btnImprimirCredencial.Content = "Imprimir Credencial";
                btnImprimirCredencial.ToolTip = "Imprimir Credencial";
            }
            else
            {
                btnImprimirCredencial.Content = "Visualizar Credencial";
                btnImprimirCredencial.ToolTip = "Visualizar Credencial";
            }

            if ((CredencialStatus)cmbCredencialStatus.SelectedItem != null)
            {
                _viewModel.HabilitaCheckDevolucao(((CredencialStatus)cmbCredencialStatus.SelectedItem).CredencialStatusId, 0);
                chkDevolucaoMotivo.IsChecked = _viewModel.IsCheckDevolucao;
                chkDevolucaoMotivo.Visibility = _viewModel.VisibilityCheckDevolucao;
                chkDevolucaoMotivo.Content = _viewModel.TextCheckDevolucao;
            }

            if (_viewModel.ColaboradorEmpresa == null) return;
            if (_viewModel.ColaboradorEmpresa.ColaboradorId > 0 & _viewModel.ColaboradorEmpresa.EmpresaId > 0)
            {
                _viewModel.CarregarVinculosAtivosOutrasCredenciais(_viewModel.ColaboradorEmpresa.ColaboradorId, _viewModel.ColaboradorEmpresa.EmpresaId);
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

        private void TecnologiaCredencial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TecnologiaCredencial_cb.SelectedItem != null)
            {
                FormatoCredencial_cb.IsEnabled = (!((IMOD.Domain.Entities.TecnologiaCredencial)TecnologiaCredencial_cb.SelectedItem).Descricao.Equals("N/D"));
                if (((IMOD.Domain.Entities.TecnologiaCredencial)TecnologiaCredencial_cb.SelectedItem).Descricao.Equals("N/D"))
                {
                    FormatoCredencial_cb.SelectedIndex = 0;
                }
            }
        }

        private void FormatoCredencial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FormatoCredencial_cb.SelectedItem != null)
            {
                if (((IMOD.Domain.Entities.FormatoCredencial)FormatoCredencial_cb.SelectedItem).Descricao.Trim().Equals("CSN"))
                {
                    FC_tb.Visibility = Visibility.Hidden;
                    lblFC.Visibility = Visibility.Hidden;
                    NumeroCredencial_tb.Visibility = Visibility.Visible;
                    lblNumero.Visibility = Visibility.Visible;
                    NumeroCredencial_tb.Focus();
                }
                else if (((IMOD.Domain.Entities.FormatoCredencial)FormatoCredencial_cb.SelectedItem).Descricao.Trim().Equals("N/D"))
                {
                    FC_tb.Visibility = Visibility.Hidden;
                    lblFC.Visibility = Visibility.Hidden;
                    NumeroCredencial_tb.Visibility = Visibility.Hidden;
                    lblNumero.Visibility = Visibility.Hidden;
                }
                else
                {
                    FC_tb.Visibility = Visibility.Visible;
                    lblFC.Visibility = Visibility.Visible;
                    NumeroCredencial_tb.Visibility = Visibility.Visible;
                    lblNumero.Visibility = Visibility.Visible;
                    NumeroCredencial_tb.Focus();
                }

            }
        }

        private void NumeroCredencial_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Entity == null) return;
            try
            {
                var nCredencial = _viewModel.Entity.NumeroCredencial;
                if (_viewModel.ExisteNumeroCredencial())
                    _viewModel.Entity.SetMessageErro("NumeroCredencial", "Nº da Credencial já existe");
                    NumeroCredencial_tb.Text = nCredencial;
                
            }
            catch (Exception)
            {
                _viewModel.Entity.SetMessageErro("Cnpj", "CNPJ inválido");
            }
        }

        private void CmbEmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}