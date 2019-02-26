// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System.Windows;
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

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados (entity);
            cmbEmpresaVinculo.Items.Refresh();
        }

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.ColaboradorEmpresa == null) return;
            _viewModel.ListarCracha (_viewModel.ColaboradorEmpresa.EmpresaId);
            _viewModel.ObterValidade();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEmpresaVinculo.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
        }

        private void OnFormatData_LostFocus(object sender, RoutedEventArgs e)
        {
            var str = txtDtValidade.Text;
            txtDtValidade.Text = str.FormatarData();
        }

        #endregion
    }
}