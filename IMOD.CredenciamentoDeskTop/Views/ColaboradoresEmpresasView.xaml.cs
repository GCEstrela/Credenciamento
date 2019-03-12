// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.ViewModels;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
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
            cmbContrato.Items.Refresh();
        }

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(Model.ColaboradorView entity,ColaboradorViewModel viewModelParent)
        {
            if (entity == null) return;
            _viewModel.AtualizarDados(entity,viewModelParent);
            //if (!_viewModel.IsEnableComboContrato)
            //{
            //    ListaSegnatarios_lv.Columns[2].Visible = false;
            //    ListaSegnatarios_lv.gri
            //}
        }

        #endregion

        private void cmbEmpresa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}