// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#endregion

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    ///     Lógica interna para Popup.xaml
    /// </summary>
    public partial class PopupBox : Window
    {
        public bool Result;

        public PopupBox(string msg, int iconBox)
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            Mensagem_tb.Text = msg;
            switch (iconBox)
            {
                case 1:
                    Icone_im.Source = new BitmapImage (new Uri ("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Informacao.ico", UriKind.Absolute));
                    btnOk.Visibility = Visibility.Visible;
                    btnSim.Visibility = Visibility.Collapsed;
                    btnNao.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    Icone_im.Source = new BitmapImage (new Uri ("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Interrogacao.png", UriKind.Absolute));
                    btnOk.Visibility = Visibility.Collapsed;
                    btnSim.Visibility = Visibility.Visible;
                    btnNao.Visibility = Visibility.Visible;
                    break;
                case 3:
                    Icone_im.Source = new BitmapImage (new Uri ("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Exclamacao.ico", UriKind.Absolute));
                    btnOk.Visibility = Visibility.Visible;
                    btnSim.Visibility = Visibility.Collapsed;
                    btnNao.Visibility = Visibility.Collapsed;

                    break;
                case 4:
                    Icone_im.Source = new BitmapImage (new Uri ("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Proibido.ico", UriKind.Absolute));
                    btnOk.Visibility = Visibility.Visible;
                    btnSim.Visibility = Visibility.Collapsed;
                    btnNao.Visibility = Visibility.Collapsed;

                    break;
            }
        }

        #region  Metodos

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void OnButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void OnButtonSim_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void OnButtonNao_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private void OnTecla_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;

            Result = false;
            Close();
        }

        #endregion
    }
}