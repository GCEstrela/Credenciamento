// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System.Windows;
using System.Windows.Controls;
using iModSCCredenciamento.ViewModels;

#endregion

namespace iModSCCredenciamento.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradoresEmpresasView.xam
    /// </summary>
    public partial class ColaboradoresEmpresasView : UserControl
    {
        private readonly ColaboradoresEmpresasViewModel _viewModel;

        public ColaboradoresEmpresasView()
        {
            InitializeComponent();
            _viewModel = new ColaboradoresEmpresasViewModel();
            DataContext = _viewModel;
        }

        #region  Metodos

        private void Frm_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEmpresa.SelectionChanged += OnSelecionaContrato_SelectionChanged;
        }

        private void OnSelecionaContrato_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.Empresa == null) return;
            _viewModel.ListarContratos(_viewModel.Empresa);
        }

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity);
        }

        #endregion
    }
}