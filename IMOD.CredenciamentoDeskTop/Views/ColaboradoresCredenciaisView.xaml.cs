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


        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity);
                _viewModel.AtualizarVinculoColaboradorEmpresa(entity);

            EmpresaVinculo_cb.Items.Refresh();
        }

        private void StatusCredencial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (StatusCredencial_cb.SelectedValue == null) return;
            string _valorSelecionado = StatusCredencial_cb.SelectedValue.ToString();
            _viewModel.ListarMotivos(_valorSelecionado);
            MotivoCredencial_cb.Items.Refresh();
            
        }

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.ColaboradorEmpresa == null) return;

            _viewModel.ListarCracha(_viewModel.ColaboradorEmpresa.EmpresaId);

        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            EmpresaVinculo_cb.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
        }

    }
}