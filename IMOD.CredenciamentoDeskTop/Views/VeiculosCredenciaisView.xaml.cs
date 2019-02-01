// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting; 

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
            //DataContext = new VeiculosCredenciaisViewModel();
            //ImprimirCredencial_bt.IsHitTestVisible = true;
        }

        #endregion


        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.VeiculoView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity);

            if (entity != null)
            {
                _viewModel.AtualizarVinculoVeiculoEmpresa(entity);
            }

            EmpresaVinculo_cb.Items.Refresh();
        }


        private void ListaVeiculosCredenciais_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaVeiculosCredenciais_lv.SelectedIndex == -1)
            {
                //Retirado pois não está funcionando o SelectedIndex=0!
                //Linha0_sp.IsEnabled = false;
                //Editar_bt.IsEnabled = false;
            }
            else
            {
                btnEditar.IsEnabled = true;
                //btnImprimirCredencial.IsHitTestVisible = true;
                btnImprimirCredencial.IsEnabled = true;
            }
        }

        private void ImprimirCredencial_bt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                _viewModel.OnImprimirAutorizacao();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }

        }

        private void StatusCredencial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {

                if (_viewModel.Entity == null) return;

                if (_viewModel.Entity.CredencialStatusId == 1)
                {
                    Ativa_tw.IsChecked = true;
                }
                else
                {
                    Ativa_tw.IsChecked = false;
                }

                if (_viewModel.Entity.CredencialStatusId > 0)
                {
                    _viewModel.CarregaColecaoCredenciaisMotivos(_viewModel.Entity.CredencialStatusId);
                    MotivoCredencial_cb.Items.Refresh();
                }

            }
        }

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.VeiculoEmpresa == null) return;

            _viewModel.CarregaColecaoLayoutsCrachas((int)_viewModel.VeiculoEmpresa.EmpresaId);

        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            EmpresaVinculo_cb.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
        }

    }
}