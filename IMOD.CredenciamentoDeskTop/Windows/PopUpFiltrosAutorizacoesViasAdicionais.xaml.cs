// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

#endregion

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    ///     Interaction logic for PopUpFiltrosAutorizacoesViasAdicionais.xaml
    /// </summary>
    public partial class PopUpFiltrosAutorizacoesViasAdicionais : Window
    {
        public PopUpFiltrosAutorizacoesViasAdicionais()
        {
            InitializeComponent(); 
            DataContext = new RelatoriosViewModel(); 
            MouseDown += Window_MouseDown;
            List<int> status = new List<int> { 2, 3 }; 
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciaisViaAdicionais(status); 
        } 

        #region  Metodos

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {
             
            int motivoTipo = 0;
            IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView motivoCredencialSelecionado = null; 
            string dataIni = dp_dataInicial.Text;
            string dataFim = dp_dataFinal.Text;

            if (lstMotivoCredencial.SelectedItem != null) 
            {
                motivoCredencialSelecionado = (IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView)lstMotivoCredencial.SelectedItem;
                motivoTipo = ((IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView)lstMotivoCredencial.SelectedItem).CredencialMotivoId;
            }

            ((RelatoriosViewModel) DataContext).OnFiltroAutorizacaoViasAdicionaisCommand (motivoTipo, dataIni, dataFim);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }
}