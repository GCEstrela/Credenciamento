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
            VeiculosBt.Visibility = Visibility.Collapsed;
            ConfiguracoesBt.Visibility = Visibility.Collapsed;
            this.PreviewKeyDown += (ss, ee) =>
            {
                if (ee.Key == Key.F12)
                {
                    if (VeiculosBt.Visibility == Visibility.Collapsed)
                    {
                        VeiculosBt.Visibility = Visibility.Visible;
                        ConfiguracoesBt.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        VeiculosBt.Visibility = Visibility.Collapsed;
                        ConfiguracoesBt.Visibility = Visibility.Collapsed;
                    }

                }
            };
        }
        public event RoutedEventHandler ButtonClick;

        private void OnButtonEmpresas_bt(object sender, RoutedEventArgs e)
        {
            EmpresasBt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            ColaboradoresBt.Background = Brushes.Transparent;
            VeiculosBt.Background = Brushes.Transparent;
            ConfiguracoesBt.Background = Brushes.Transparent;
            RelatoriosBt.Background = Brushes.Transparent;
            TermosBt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonColaboradores_bt(object sender, RoutedEventArgs e)
        {
            EmpresasBt.Background = Brushes.Transparent;
            ColaboradoresBt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            VeiculosBt.Background = Brushes.Transparent;
            ConfiguracoesBt.Background = Brushes.Transparent;
            RelatoriosBt.Background = Brushes.Transparent;
            TermosBt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonVeiculos_bt(object sender, RoutedEventArgs e)
        {
            EmpresasBt.Background = Brushes.Transparent;
            ColaboradoresBt.Background = Brushes.Transparent;
            VeiculosBt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            ConfiguracoesBt.Background = Brushes.Transparent;
            RelatoriosBt.Background = Brushes.Transparent;
            TermosBt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonConfiguracoes_bt(object sender, RoutedEventArgs e)
        {
            EmpresasBt.Background = Brushes.Transparent;
            ColaboradoresBt.Background = Brushes.Transparent;
            VeiculosBt.Background = Brushes.Transparent;
            ConfiguracoesBt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            RelatoriosBt.Background = Brushes.Transparent;
            TermosBt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonRelatorios_bt(object sender, RoutedEventArgs e)
        {
            EmpresasBt.Background = Brushes.Transparent;
            ColaboradoresBt.Background = Brushes.Transparent;
            VeiculosBt.Background = Brushes.Transparent;
            ConfiguracoesBt.Background = Brushes.Transparent;
            RelatoriosBt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            TermosBt.Background = Brushes.Transparent;
            ButtonClick(sender, new RoutedEventArgs());
        }

        private void OnButtonTermos_bt(object sender, RoutedEventArgs e)
        {
            EmpresasBt.Background = Brushes.Transparent;
            ColaboradoresBt.Background = Brushes.Transparent;
            VeiculosBt.Background = Brushes.Transparent;
            ConfiguracoesBt.Background = Brushes.Transparent;
            RelatoriosBt.Background = Brushes.Transparent;
            TermosBt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
            ButtonClick(sender, new RoutedEventArgs());
        }
    }
}
