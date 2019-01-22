// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System.Windows;
using System.Windows.Input;
using iModSCCredenciamento.ViewModels;

#endregion

namespace iModSCCredenciamento.Windows
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
            var tipo = 0;
            var DataIni = dp_dataInicial.Text;
            var DataFim = dp_dataFinal.Text;

            if (segundaemissao_rb.IsChecked.Value)
            {
                tipo = 2;
            }
            else if (terceiraemissao_rb.IsChecked.Value)
            {
                tipo = 3;
            }
            else
            {
                tipo = 0;
            }

            ((RelatoriosViewModel) DataContext).OnFiltroAutorizacaoViasAdicionaisCommand (tipo, DataIni, DataFim);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }
}