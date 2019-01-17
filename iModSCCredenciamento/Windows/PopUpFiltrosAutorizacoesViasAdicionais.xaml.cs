using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iModSCCredenciamento.ViewModels;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Interaction logic for PopUpFiltrosAutorizacoesViasAdicionais.xaml
    /// </summary>
    public partial class PopUpFiltrosAutorizacoesViasAdicionais : Window
    {
        public PopUpFiltrosAutorizacoesViasAdicionais()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
        }

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
            string DataIni = dp_dataInicial.Text;
            string DataFim = dp_dataFinal.Text;

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

            ((RelatoriosViewModel)DataContext).OnFiltroAutorizacaoViasAdicionaisCommand(tipo, DataIni, DataFim);


            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
