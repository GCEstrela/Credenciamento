// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System.Windows.Controls;
using iModSCCredenciamento.ViewModels;

#endregion

namespace iModSCCredenciamento.Views
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
            //DataContext = new ColaboradoresCredenciaisViewModel();
            //ImprimirCredencial_bt.IsHitTestVisible = true;
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
        }

        private void ListaColaboradoresCredenciais_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}