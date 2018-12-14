using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using iModSCCredenciamento.Funcoes;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupPdfViewer.xaml
    /// </summary>
    public partial class PopupPdfViewer : Window
    {
        private string _ArquivoPDF = "";
        public PopupPdfViewer(string _PDF)
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;

            _ArquivoPDF = _PDF;
            byte[] buffer = Conversores.StringToPDF(_ArquivoPDF);
            _ArquivoPDF = Path.GetRandomFileName();
            _ArquivoPDF = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoPDF;
            _ArquivoPDF = Path.ChangeExtension(_ArquivoPDF, ".pdf");
            File.WriteAllBytes(_ArquivoPDF, buffer);

            pdfWebViewer.Navigate(new Uri(_ArquivoPDF));

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            pdfWebViewer.Dispose();

                Thread.Sleep(500);

            File.Delete(_ArquivoPDF);
            Close();
        }
    }
}
