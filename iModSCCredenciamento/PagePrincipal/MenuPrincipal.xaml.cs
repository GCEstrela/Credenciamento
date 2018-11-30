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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iModSCCredenciamento.PagePrincipal
{
    /// <summary>
    /// Interação lógica para MenuPrincipal.xam
    /// </summary>
    public partial class MenuPrincipal : UserControl
    {
        public MenuPrincipal()
        {
            InitializeComponent();
            Veiculos_bt.Visibility = Visibility.Collapsed;
            Configuracoes_bt.Visibility = Visibility.Collapsed;
            this.PreviewKeyDown += (ss, ee) =>
            {
                if (ee.Key == Key.F12)
                {
                    if (Veiculos_bt.Visibility == Visibility.Collapsed)
                    {
                        Veiculos_bt.Visibility = Visibility.Visible;
                        Configuracoes_bt.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Veiculos_bt.Visibility = Visibility.Collapsed;
                        Configuracoes_bt.Visibility = Visibility.Collapsed;
                    }

                }
            };
        }
        public event RoutedEventHandler ButtonClick;

        private void OnButtonEmpresas_bt(object sender, RoutedEventArgs e)
        {
            Empresas_bt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            Colaboradores_bt.Background = Brushes.Transparent;
            Veiculos_bt.Background = Brushes.Transparent;
            Configuracoes_bt.Background = Brushes.Transparent;
            Relatorios_bt.Background = Brushes.Transparent;
            Termos_bt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonColaboradores_bt(object sender, RoutedEventArgs e)
        {
            Empresas_bt.Background = Brushes.Transparent;
            Colaboradores_bt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            Veiculos_bt.Background = Brushes.Transparent;
            Configuracoes_bt.Background = Brushes.Transparent;
            Relatorios_bt.Background = Brushes.Transparent;
            Termos_bt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonVeiculos_bt(object sender, RoutedEventArgs e)
        {
            Empresas_bt.Background = Brushes.Transparent;
            Colaboradores_bt.Background = Brushes.Transparent;
            Veiculos_bt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            Configuracoes_bt.Background = Brushes.Transparent;
            Relatorios_bt.Background = Brushes.Transparent;
            Termos_bt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonConfiguracoes_bt(object sender, RoutedEventArgs e)
        {
            Empresas_bt.Background = Brushes.Transparent;
            Colaboradores_bt.Background = Brushes.Transparent;
            Veiculos_bt.Background = Brushes.Transparent;
            Configuracoes_bt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            Relatorios_bt.Background = Brushes.Transparent;
            Termos_bt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonRelatorios_bt(object sender, RoutedEventArgs e)
        {
            Empresas_bt.Background = Brushes.Transparent;
            Colaboradores_bt.Background = Brushes.Transparent;
            Veiculos_bt.Background = Brushes.Transparent;
            Configuracoes_bt.Background = Brushes.Transparent;
            Relatorios_bt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            Termos_bt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonTermos_bt(object sender, RoutedEventArgs e)
        {
            Empresas_bt.Background = Brushes.Transparent;
            Colaboradores_bt.Background = Brushes.Transparent;
            Veiculos_bt.Background = Brushes.Transparent;
            Configuracoes_bt.Background = Brushes.Transparent;
            Relatorios_bt.Background = Brushes.Transparent;
            Termos_bt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            ButtonClick(sender, new RoutedEventArgs());
        }
    }
}
