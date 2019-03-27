// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

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
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciais(1); 
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
             
            int tipo = 0;
            IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView motivoCredencialSelecionado = null; 
            string dataIni = dp_dataInicial.Text;
            string dataFim = dp_dataFinal.Text;

            if (lstMotivoCredencial.SelectedItem != null) 
            {
                motivoCredencialSelecionado = (IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView)lstMotivoCredencial.SelectedItem;
                tipo = ((IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView)lstMotivoCredencial.SelectedItem).CredencialMotivoId;
            }

            ((RelatoriosViewModel) DataContext).OnFiltroAutorizacaoViasAdicionaisCommand (tipo, dataIni, dataFim);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }
}