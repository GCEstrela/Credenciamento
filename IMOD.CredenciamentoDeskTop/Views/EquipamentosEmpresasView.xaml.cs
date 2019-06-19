using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para VeiculosEmpresasView.xam
    /// </summary>
    public partial class EquipamentosEmpresasView : UserControl
    {
        private readonly EquipamentosEmpresasViewModel _viewModel;

        public EquipamentosEmpresasView()
        {
            InitializeComponent();
            _viewModel = new EquipamentosEmpresasViewModel();
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
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.VeiculoView entity, EquipamentosViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
        }
         

        #endregion
    }
}
