using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    /// Interação lógica para VeiculosEmpresasView.xam
    /// </summary>
    public partial class VeiculosEmpresasView : UserControl
    {
        private readonly VeiculosEmpresasViewModel _viewModel;

        public VeiculosEmpresasView()
        {
            InitializeComponent();
            _viewModel = new VeiculosEmpresasViewModel();
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
            chk_areaManobra.IsChecked = _viewModel.Entity.AreaManobra;
            cmbContrato.Items.Refresh();
        }

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.VeiculoView entity, VeiculoViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados(entity, viewModelParent);
        }


        #endregion

        private void ListaSegnatarios_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (_viewModel.Empresa == null) return;
            //chk_areaManobra.IsChecked = _viewModel.Entity.AreaManobra;
        }
    }
}
