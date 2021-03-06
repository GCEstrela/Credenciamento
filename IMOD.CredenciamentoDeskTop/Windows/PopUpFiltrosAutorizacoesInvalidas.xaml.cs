﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosAutorizacoesInvalidas.xaml
    /// </summary>
    public partial class PopUpFiltrosAutorizacoesInvalidas : Window
    {
        public PopUpFiltrosAutorizacoesInvalidas()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciais(2);//Carregar os motivos do status 2 - inativo 
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {

            string dataIni = dp_dataInicial.Text; 
            string dataFim = dp_dataFinal.Text;
            
            IEnumerable<object> motivoCredencialSelecionados = new List<object>();

            if (lstMotivoCredencial.SelectedItems.Count > 0)
            {
                motivoCredencialSelecionados = (IEnumerable<object>)lstMotivoCredencial.SelectedItems;

                var teste = lstMotivoCredencial.SelectedItems;
            }

            var checkTipo =   (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value? false : true);

            bool flaTodasDevolucaoEntregue = (bool)RbtnTodasDevolucaoEntregue.IsChecked.Value;
            bool flaSimNaoDevolucaoEntregue = (bool)RbtnSimDevolucaoEntregue.IsChecked.Value ? true : (bool)RbtnNaoDevolucaoEntregue.IsChecked.Value ? false : true;


            ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesInvalidasFiltroCommand(checkTipo, motivoCredencialSelecionados, dataIni, dataFim, flaTodasDevolucaoEntregue, flaSimNaoDevolucaoEntregue);

            Close();
        }

       
    }
}
