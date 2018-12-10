﻿using System.Windows;
using System.Windows.Media.Imaging;
using iModSCCredenciamento.Funcoes;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupWebCam.xaml
    /// </summary>
    public partial class PopupWebCam : Window
    {
        public PopupWebCam()
        {
            InitializeComponent();
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
            Close();
        }

        private void Capturar_bt_Click(object sender, RoutedEventArgs e)
        {
            
            imgCapture.Source = imgVideo.Source;
        }

        private void Aceitar_bt_Click(object sender, RoutedEventArgs e)
        {
            //result = "Fechou";
            Close();
            //BitmapImage _img = (BitmapImage)imgCapture.Source;

            //string _imgstr = Conversores.IMGtoSTR(_img);

            //Foto_im.Source = _img;
            //((ClasseColaboradores.Colaborador)ListaColaboradores_lv.SelectedItem).Foto = _imgstr; 
        }
    }
}
