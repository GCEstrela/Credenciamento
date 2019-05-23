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
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupWebCam.xaml
    /// </summary>
    public partial class PopupWebCam : Window
    {
        
        public bool aceitarImg = true;
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
            if (webcam != null)
                webcam.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            aceitarImg = false;
            this.Close();
        }

        private void Capturar_bt_Click(object sender, RoutedEventArgs e)
        {
            imgCapture.Source = imgVideo.Source;
            //imgCapture.Source = BitmapImageFromBitmapSourceResized((BitmapSource)imgVideo.Source, 190);
        }

        private void Aceitar_bt_Click(object sender, RoutedEventArgs e)
        {
            imgCapture.Source = BitmapImageFromBitmapSourceResized((BitmapSource)imgVideo.Source, 250);
            //SaveImageCapture((BitmapSource)imgCapture.Source);
            this.Close();
        }
        public static BitmapSource BitmapImageFromBitmapSourceResized(BitmapSource bitmapSource, int newWidth)
        {
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
            bImg.DecodePixelWidth = newWidth;
            bImg.EndInit();
            memoryStream.Close();
            return bImg;
        }
        
    }
}
