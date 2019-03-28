using IMOD.CredenciamentoDeskTop.ViewModels;
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
using IMOD.CredenciamentoDeskTop.Helpers;
using System.Windows.Shapes;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupWebCam.xaml
    /// </summary>
    public partial class PopupWebCam : Window
    {
        private readonly ColaboradorViewModel _viewModel;
        public PopupWebCam()
        {
            InitializeComponent();
            _viewModel = new ColaboradorViewModel();
            DataContext = _viewModel;
        }

        
        public BitmapSource Captura
        {
            get { return (BitmapSource)imgCapture.Source; }
           
        }

        WebCam webcam;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            webcam = new WebCam();
            
            webcam.InitializeWebCam(ref imgVideo);

            webcam.Start();
        }

        private void Label_Unloaded(object sender, RoutedEventArgs e)
        {
            webcam.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Capturar_bt_Click(object sender, RoutedEventArgs e)
        {         
            imgCapture.Source = imgVideo.Source;
        }

        private void Aceitar_bt_Click(object sender, RoutedEventArgs e)
        {           
            this.Close();
        }
    }
}
