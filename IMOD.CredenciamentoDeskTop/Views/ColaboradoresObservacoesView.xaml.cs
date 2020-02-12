// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Windows;
using System.Windows.Controls;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views
{
    /// <summary>
    ///     Interação lógica para ColaboradoresAnexosView.xam
    /// </summary>
    public partial class ColaboradoresObservacoesView : UserControl
    {
        private readonly ColaboradoresObservacoesViewModel _viewModel;

        public ColaboradoresObservacoesView()
        {
            try
            {
                InitializeComponent();
                _viewModel = new ColaboradoresObservacoesViewModel();
                DataContext = _viewModel;
            }
            catch (Exception ex)
            {
                //WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
        }

        #region  Metodos

        /// <summary>
        ///     Atualizar dados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModelParent"></param>
        public void AtualizarDados(Model.ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            //if (entity == null) return;
            _viewModel.AtualizarDados (entity, viewModelParent);
        }


        #endregion

        private void ListaObservacoes_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_viewModel.Entity == null) return;
                //_viewModel.BuscarObservacoes(_viewModel.Entity.ColaboradorId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}