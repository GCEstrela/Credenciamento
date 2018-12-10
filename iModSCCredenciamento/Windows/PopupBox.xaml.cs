using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
            PreviewKeyDown += (ss, ee) =>
            {
                if (ee.Key == Key.Escape)
                {
                    Result = false;
                    Close();
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
            Close();
        }

        private void Sim_bt_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void Nao_bt_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}
