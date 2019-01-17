// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Windows.Controls;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.ViewModels;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

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
            if (ListaColaboradoresCredenciais_lv.SelectedIndex == -1)
            {
                //Retirado pois não está funcionando o SelectedIndex=0!
                //Linha0_sp.IsEnabled = false;
                //Editar_bt.IsEnabled = false;
            }
            else
            {
                btnEditar.IsEnabled = true;
                brnImprimirCredencial.IsHitTestVisible = true;
            }
        }

        private void ImprimirCredencial_bt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                _viewModel.OnImprimirCredencial();
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
                //if (((ClasseCredenciaisStatus.CredencialStatus)((object[])e.AddedItems)[0]).CredencialStatusID == 1)
                //{
                //Ativa_tw.IsChecked = true;
                //    ((ColaboradoresCredenciaisViewModel)DataContext).CarregaColecaoCredenciaisMotivos(1);
                //}
                //else
                //{
                //    Ativa_tw.IsChecked = false;
                //    ((ColaboradoresCredenciaisViewModel)DataContext).CarregaColecaoCredenciaisMotivos(2);
                //}
            }
        }

        private void EmpresaVinculo_cb_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void EmpresaVinculo_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////var _empresa = EmpresaVinculo_cb.SelectedItem.ToString();
            if (_viewModel.ColaboradorEmpresa == null) return;

            //var _empresa2 = EmpresaVinculo_cb.Tag.ToString();
            //var _empresa = EmpresaVinculo_cb.SelectedValue.ToString();
            _viewModel.CarregaColecaoLayoutsCrachas(_viewModel.ColaboradorEmpresa.EmpresaId);

        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            EmpresaVinculo_cb.SelectionChanged += EmpresaVinculo_cb_SelectionChanged;
        }
    }
}