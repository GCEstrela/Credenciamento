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

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupWebCam.xaml
    /// </summary>
    public partial class PopupWebCam : Window
    {

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
            var encoder = new JpegBitmapEncoder();
            
            //imgCapture.Source = HandleImageUpload(ImageSourceToBytes(encoder, (BitmapSource)imgVideo.Source));

            //Serve para testar, salvando local para ver arquivo
            WpfHelp.SaveImageCapture(HandleImageUpload(ImageSourceToBytes(encoder, (BitmapSource)imgVideo.Source)));
        }

        public byte[] ImageSourceToBytes(BitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        public BitmapSource HandleImageUpload(byte[] binaryImage)
        {
            //354x472 tamanho 3x4
            System.Drawing.Image img = ResizeImage(System.Drawing.Image.FromStream(BytearrayToStream(binaryImage)), 354, 472);
            //img.Save("IMAGELOCATION.png", System.Drawing.Imaging.ImageFormat.Gif);

            return Convert(img);
        }

        private MemoryStream BytearrayToStream(byte[] arr)
        {
            return new MemoryStream(arr, 0, arr.Length);
        }

        public BitmapImage Convert(System.Drawing.Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private System.Drawing.Image ResizeImage(System.Drawing.Image img, int maxWidth, int maxHeight)
        {
            //Se a imagem já estiver em tamanho paisagem, retorna a própria imagem
            if (img.Height > maxHeight && img.Width > maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = (double)img.Height / maxHeight;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor((img.Height * 1.78) / ratio);
                Bitmap cpy = new Bitmap(nnx, nny, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(cpy))
                {
                    gr.Clear(System.Drawing.Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    gr.DrawImage(img,
                        new System.Drawing.Rectangle(0, 0, nnx, nny),
                        new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
                }
                return cpy;
            }

        }

        private void Aceitar_bt_Click(object sender, RoutedEventArgs e)
        {
            //imgCapture.Source = BitmapImageFromBitmapSourceResized((BitmapSource)imgVideo.Source, 180);
            //imgCapture.Width = 354;
            //imgCapture.Height = 472;
            //WpfHelp.SaveImageCapture((BitmapSource)imgCapture.Source);
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
