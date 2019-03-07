// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.ViewModels;

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
        public void AtualizarDados(Model.VeiculoView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados (entity);
           // EmpresaVinculo_cb.Items.Refresh();
        }

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.VeiculoEmpresa == null) return;
            _viewModel.CarregaColecaoLayoutsCrachas ((int) _viewModel.VeiculoEmpresa.EmpresaId);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            EmpresaVinculo_cb.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
        }

        #endregion
    }
}