﻿// ***********************************************************************
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

        private void StatusCredencial_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                
                //if (((CredencialStatusView)((object[])e.AddedItems)[0]).CredencialStatusId == 1)
                //{
                //    Ativa_tw = true;
                //    ((ColaboradoresCredenciaisViewModel)DataContext).CarregaColecaoCredenciaisMotivos(1);
                //}
                //else
                //{
                //    Ativa_tw.IsChecked = false;
                //    ((ColaboradoresCredenciaisViewModel)DataContext).CarregaColecaoCredenciaisMotivos(2);
                //}
            }
        }
    }
}