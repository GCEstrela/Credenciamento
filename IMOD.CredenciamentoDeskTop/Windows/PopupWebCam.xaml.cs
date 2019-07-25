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
using System.Drawing.Drawing2D;
using AForge.Imaging.Filters;
using CroppingImageLibrary;
namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupWebCam.xaml
    /// </summary>
    public partial class PopupWebCam : Window
    {
        public CroppingAdorner CroppingAdorner;
       

        public bool aceitarImg = true;
        private readonly ColaboradorViewModel _viewModel;
        private int resolucaoImg = 150;
        public PopupWebCam(int resolucao)
        {
            resolucaoImg = resolucao;
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

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(CanvasPanel);
            CroppingAdorner = new CroppingAdorner(CanvasPanel);
            adornerLayer.Add(CroppingAdorner);
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
            //var encoder = new JpegBitmapEncoder();

            ////imgCapture.Source = (BitmapSource)imgVideo.Source;

            //Rect rect = new Rect(0, 0, 190, 224);

            //// Create a DrawingVisual/Context to render with
            //DrawingVisual drawingVisual = new DrawingVisual();
            //using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            //{
            //    drawingContext.DrawImage(imgVideo.Source, rect);
            //}

            //// Use RenderTargetBitmap to resize the original image
            //RenderTargetBitmap resizedImage = new RenderTargetBitmap(
            //    (int)rect.Width, (int)rect.Height,  // Resized dimensions
            //    96, 96,                             // Default DPI values
            //    PixelFormats.Default);              // Default pixel format
            //resizedImage.Render(drawingVisual);
            //imgCapture.Source = (BitmapSource)resizedImage;
            ////WpfHelp.SaveImageCapture(HandleImageUpload(ImageSourceToBytes(encoder, (BitmapSource)imgVideo.Source)));

            BitmapFrame croppedBitmapFrame = CroppingAdorner.GetCroppedBitmapFrame();
            imgCapture.Width = croppedBitmapFrame.Width;
            imgCapture.Height  = croppedBitmapFrame.Height;

            imgCapture.Source = (BitmapSource)croppedBitmapFrame;
        }

        private void Aceitar_bt_Click(object sender, RoutedEventArgs e)
        {

            
            //WpfHelp.SaveImageCapture((BitmapSource)resizedImage);
            this.Close();
        }

        private void RootGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CroppingAdorner.CaptureMouse();
            CroppingAdorner.MouseLeftButtonDownEventHandler(sender, e);
        }
    }
}
