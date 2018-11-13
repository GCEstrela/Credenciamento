using iModSCCredenciamento.Funcoes;
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

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para Popup.xaml
    /// </summary>
    public partial class PopupBox : Window
    {
        public bool Result;
        public PopupBox(string Texto, int Icone)
        {
            InitializeComponent();
            this.PreviewKeyDown += (ss, ee) =>
            {
                if (ee.Key == Key.Escape)
                {
                    Result = false;
                    this.Close();
                }
            };

            MouseDown += Window_MouseDown;
            Mensagem_tb.Text = Texto;
            switch (Icone)
            {
                case 1:
                    Icone_im.Source= new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Informacao.ico", UriKind.Absolute));
                    Ok_bt.Visibility =  Visibility.Visible;
                    Sim_bt.Visibility = Visibility.Collapsed;
                    Nao_bt.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    Icone_im.Source = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Interrogacao.png", UriKind.Absolute));
                    Ok_bt.Visibility = Visibility.Collapsed;
                    Sim_bt.Visibility = Visibility.Visible;
                    Nao_bt.Visibility = Visibility.Visible;
                    break;
                case 3:
                    Icone_im.Source = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Exclamacao.ico", UriKind.Absolute));
                    Ok_bt.Visibility = Visibility.Visible;
                    Sim_bt.Visibility = Visibility.Collapsed;
                    Nao_bt.Visibility = Visibility.Collapsed;

                    break;
                case 4:
                    Icone_im.Source = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Proibido.ico", UriKind.Absolute));
                    Ok_bt.Visibility = Visibility.Visible;
                    Sim_bt.Visibility = Visibility.Collapsed;
                    Nao_bt.Visibility = Visibility.Collapsed;

                    break;
            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Ok_bt_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            this.Close();
        }

        private void Sim_bt_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            this.Close();
        }

        private void Nao_bt_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            this.Close();
        }
    }
}
